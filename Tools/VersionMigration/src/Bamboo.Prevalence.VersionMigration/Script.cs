// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2002 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
//
// Contact Information
//
// http://bbooprevalence.sourceforge.net
// mailto:rodrigobamboo@users.sourceforge.net

using System;
using System.Collections;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Bamboo.Prevalence.VersionMigration
{
	public class Script
	{
		string _targetObject;
		
		string _targetEvent;
		
		string _code;
		
		string _language;
		
		ScriptImportCollection _imports;
		
		internal Script()
		{
			_imports = new ScriptImportCollection();
		}
		
		internal Script(XmlElement element) : this()
		{
			_targetObject = GetRequiredAttribute(element, "for");
			_targetEvent = GetRequiredAttribute(element, "event");
			_code = GetRequiredNode(element, "code").InnerText;
			_language = element.GetAttribute("language");
			if (0 == _language.Length)
			{
				_language = "c#";
			}
			
			foreach (XmlElement importElement in element.SelectNodes("import"))
			{
				_imports.Add(new ScriptImport(GetRequiredAttribute(importElement, "namespace")));				
			}
		}
		
		public ScriptImportCollection Imports
		{
			get
			{
				return _imports;
			}
		}
		
		public string TargetObject
		{
			get
			{
				return _targetObject;
			}
		}
		
		public string TargetEvent
		{
			get
			{
				return _targetEvent;
			}
		}
		
		public string Code
		{
			get
			{
				return _code;
			}
		}
		
		public string Language
		{
			get
			{
				return _language;
			}
		}
		
		string EventHandlerName
		{
			get
			{
				return string.Format("{0}_{1}", _targetObject, _targetEvent);
			}
		}
		
		internal void SetUp(string className, MigrationContext context)
		{	
			CodeNamespace ns = new CodeNamespace("__MigrationPlan__");
			foreach (ScriptImport si in _imports)
			{
				ns.Imports.Add(new CodeNamespaceImport(si.Namespace));
			}
			
			ns.Types.Add(GetTypeDeclaration(className));
			
			CodeCompileUnit unit = new CodeCompileUnit();
			unit.Namespaces.Add(ns);
			
			CompilerResults results = Compile(context, unit);
			if (0 == results.Errors.Count)
			{
				context.Trace("script compiled successfully.");
				Assembly assembly = results.CompiledAssembly;
				Type type = assembly.GetType(string.Format("__MigrationPlan__.{0}", className), true);
				Activator.CreateInstance(type, new object[] { context });
			}
			else
			{
				context.Trace("{0} script compilation error(s)!", results.Errors.Count);
				foreach (CompilerError error in results.Errors)
				{
					context.Trace(error.ToString());
				}
				throw new ApplicationException(results.Errors[0].ToString());
			}
		}		
		
		CodeTypeDeclaration GetTypeDeclaration(string className)
		{
			CodeTypeDeclaration type = new CodeTypeDeclaration();
			type.Name = className;
			type.Members.Add(GetConstructor());
			type.Members.Add(GetMemberMethod());			
			return type;
		}
		
		CodeConstructor GetConstructor()
		{
			CodeConstructor constructor = new CodeConstructor();			
			constructor.Attributes = MemberAttributes.Public;
			constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(MigrationContext), "context"));
			
			CodeDelegateCreateExpression createDelegate = new CodeDelegateCreateExpression( 
				new CodeTypeReference("System.EventHandler"),
				new CodeThisReferenceExpression(),
				EventHandlerName
				);
				
			constructor.Statements.Add(
				new CodeAttachEventStatement(
					new CodeArgumentReferenceExpression("context"),
					"AfterDeserialization",
					createDelegate
					)
				);				
			
			return constructor;
		}
		
		CodeMemberMethod GetMemberMethod()
		{
			CodeMemberMethod method = new CodeMemberMethod();
			
			// void context_AfterDeserialization(object __sender, EventArgs __args)
			// {
			method.Name = EventHandlerName;
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "__sender"));
			method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(EventArgs), "__args"));
			
			// MigrationContext context = (MigrationContext)__sender;
			method.Statements.Add(
				new CodeVariableDeclarationStatement(
					typeof(MigrationContext), 
					"context",
					new CodeCastExpression(
						typeof(MigrationContext),
						new CodeArgumentReferenceExpression("__sender")
						)
					)
				);
				
			// AfterDeserializationEventArgs args = (AfterDeserializationEventArgs)__args;
			method.Statements.Add(
				new CodeVariableDeclarationStatement(
					typeof(AfterDeserializationEventArgs),
					"args",
					new CodeCastExpression(
						typeof(AfterDeserializationEventArgs),
						new CodeArgumentReferenceExpression("__args")
						)
					)
				);
				
			method.Statements.Add(
				new CodeSnippetStatement(_code)
				);
			// }
				
			return method;
		}
		
		CompilerResults Compile(MigrationContext context, CodeCompileUnit dom)
		{
			CompilerParameters parameters = new CompilerParameters();
			parameters.GenerateInMemory = true;
			parameters.ReferencedAssemblies.Add(GetType().Assembly.Location);
			parameters.ReferencedAssemblies.Add(context.TargetAssembly.Location);
			
			CodeDomProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
			ICodeCompiler compiler = provider.CreateCompiler();
			return compiler.CompileAssemblyFromDom(parameters, dom);
		}
		
		string GetRequiredAttribute(XmlElement element, string attributeName)
		{
			string value = element.GetAttribute(attributeName);
			if (null == value || 0 == value.Length)
			{
				throw new ApplicationException(string.Format("Expected attribute '{0}' for element '{1}'!", attributeName, element.LocalName));
			}		
			return value;	
		}
		
		XmlNode GetRequiredNode(XmlElement element, string name)
		{
			XmlNode node = element.SelectSingleNode("code");
			if (null == node)
			{
				throw new ApplicationException(string.Format("Expected child node '{0}' for element '{1}'!", name, element.LocalName));
			}
			return node;
		}
	}
}