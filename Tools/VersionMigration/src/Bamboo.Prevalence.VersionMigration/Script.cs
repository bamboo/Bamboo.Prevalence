#region license
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// http://bbooprevalence.sourceforge.net
// mailto:rodrigobamboo@users.sourceforge.net
#endregion

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
		
		string _references;
		
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
			_references = element.GetAttribute("references");
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
				
			// context.AfterDeserialization += new System.EventHandler(this.<EventHandlerName>);
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
			
			if (_references.Length > 0)
			{
				foreach (string reference in _references.Split(';'))
				{
					string assemblyName = reference.Trim();
					parameters.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);
				}
			}
			
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