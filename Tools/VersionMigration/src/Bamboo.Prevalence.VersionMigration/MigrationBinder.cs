using System;
using System.Runtime.Serialization;

namespace Bamboo.Prevalence.VersionMigration
{
	/// <summary>
	/// SerializationBinder for migration. It supports
	/// class renaming.
	/// </summary>
	public class MigrationBinder : SerializationBinder
	{
		MigrationContext _context;

		public MigrationBinder(MigrationContext context)
		{
			_context = context;
		}

		public override System.Type BindToType(string assemblyName, string typeName)
		{
			_context.Trace("\nBindToType(\"{0}\", \"{1}\")", assemblyName, typeName);

			string actualAssemblyName = assemblyName;
			string actualTypeName = typeName;

			TypeMapping mapping = _context.MigrationPlan.TypeMappings[typeName];
			if (null != mapping)
			{				
				if (string.Empty == mapping.AssemblyName)
				{	
					_context.Trace("{0} bound to {1}, {2}", typeName, mapping.TypeName, _context.TargetAssembly.FullName);
					return _context.TargetAssembly.GetType(mapping.TypeName);
				}
				else
				{					
					actualAssemblyName = mapping.AssemblyName;
					actualTypeName = mapping.TypeName;
				}
			}

			string boundTypeName = actualTypeName + ", " + actualAssemblyName;

			_context.Trace("{0} bound to {1}", typeName, boundTypeName);

			return Type.GetType(boundTypeName);
		}
	}
}
