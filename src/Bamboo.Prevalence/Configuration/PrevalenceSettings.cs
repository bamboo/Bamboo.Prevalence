using System;
using System.IO;

namespace Bamboo.Prevalence.Configuration
{
	/// <summary>
	/// Stores configuration settings for the Bamboo.Prevalence engine.
	/// This class allows you to use .NET config files to automatically
	/// set up prevalent systems and make them available to your
	/// code at runtime.
	/// </summary>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <!--
	///		This is a sample configuration file that demonstrates
	///		the use of Bamboo.Prevalence.Configuration.PrevalenceSettings
	///	-->
	/// <configuration>
	///		<!--
	///			First you need to declare the section bamboo.prevalence,
	///			note that you have to spell the section name exactly
	///			like this: bamboo.prevalence
	///		-->
	///		<configSections>
	///			<section name="bamboo.prevalence" type="Bamboo.Prevalence.Configuration.PrevalenceSettings, Bamboo.Prevalence" />
	///		</configSections>
	///
	///		<!-- Add the section to your .config file -->		
	///		<bamboo.prevalence>
	///		
	///			<!--
	///			Add any engines you want. The configuration
	///			handler will automatically set the engines up
	///			for you and make them available to your code
	///			through PrevalenceSettings.GetEngine.
	///			-->
	///			<engines>
	///				<add id="tasks"
	///					type="Bamboo.Prevalence.Demo.TaskSystem"
	///					base="c:\tasks" />
	///			</engines>
	///		</bamboo.prevalence>
	///	</configuration>
	///	]]>
	///	
	///	Your application code:
	///	
	///	using Bamboo.Prevalence;
	///	using Bamboo.Prevalence.Configuration;
	///	
	///	public class App
	///	{
	///		public static void Main(string[] args)
	///		{
	///			PrevalenceEngine engine = PrevalenceSettings.Current.GetEngine("demo");
	///			// use the engine...
	///		}
	///	}
	///	</code>
	/// </example>
	public class PrevalenceSettings : System.Configuration.IConfigurationSectionHandler
	{			
		/// <summary>
		/// Config file section name that <b>MUST</b> be used for 
		/// Bamboo.Prevalence.Configuration.PrevalenceSettings.
		/// </summary>
		public const string SectionName = "bamboo.prevalence";		

		/// <summary>
		/// Returns the current ConfigurationSettings instance or
		/// null if Bamboo.Prevalence was not configured for the
		/// running application.
		/// </summary>
		public static PrevalenceSettings Current
		{
			get
			{
				return (PrevalenceSettings)System.Configuration.ConfigurationSettings.GetConfig(SectionName);
			}
		}

		private System.Collections.IDictionary _engines;

		/// <summary>
		/// Creates a ConfigurationSettings from a XML
		/// configuration section node.
		/// </summary>
		/// <param name="section">configuration section node</param>
		private PrevalenceSettings(System.Xml.XmlNode section)
		{
			_engines = SetUpEngines(section);
		}	

		/// <summary>
		/// Do not call this constructor! it will be called
		/// by the system at configuration loading time.
		/// </summary>
		public PrevalenceSettings()
		{
		}

		/// <summary>
		/// Returns a configured engine.
		/// </summary>
		/// <param name="id">engine id</param>
		/// <returns>the engine with the id id or null</returns>
		public Bamboo.Prevalence.PrevalenceEngine GetEngine(string id)
		{
			return (Bamboo.Prevalence.PrevalenceEngine)_engines[id];
		}

		#region Implementation of IConfigurationSectionHandler
		object System.Configuration.IConfigurationSectionHandler.Create(object parent, object configContext, System.Xml.XmlNode section)
		{			
			if (!section.LocalName.Equals(SectionName))
			{
				InvalidSectionNameError(section);
			}
			return new PrevalenceSettings(section);
		}	
		#endregion			

		private System.Collections.IDictionary SetUpEngines(System.Xml.XmlNode section)
		{		
			System.Xml.XmlNodeList list = section.SelectNodes("engines/add");
			System.Collections.Hashtable engines = new System.Collections.Hashtable(list.Count);
			foreach (System.Xml.XmlElement item in list)
			{
				SetUpEngine(engines, item);
			}			
			return engines;
		}

		private void SetUpEngine(System.Collections.Hashtable engines, System.Xml.XmlElement item)
		{
			string id = GetRequiredAttribute(item, "id");
			string type = GetRequiredAttribute(item, "type");
			string prevalenceBase = GetOptionalPrevalenceBase(item, id);

			System.Type systemType = System.Type.GetType(type);
			if (null == systemType)
			{
				TypeNotFoundError(type, item);
			}

			try
			{
				engines[id] = new PrevalenceEngine(systemType, prevalenceBase);
			}
			catch (System.Exception x)
			{
				SetUpError(x, item);
			}			
		}

		private string GetOptionalPrevalenceBase(System.Xml.XmlElement element, string id)
		{
			string prevalenceBase = element.GetAttribute("base");
			if (null == prevalenceBase)
			{
				prevalenceBase = Path.Combine(GetPersonalPrevalenceFolder(), id);					
			}
			return prevalenceBase;
		}

		private string GetPersonalPrevalenceFolder()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "prevalence");
		}

		private string GetRequiredAttribute(System.Xml.XmlElement element, string name)
		{
			string value = element.GetAttribute(name);
			if (null == value || 0 == value.Length)
			{
				AttributeRequiredError(name, element);
			}
			return value;
		}

		private void InvalidSectionNameError(System.Xml.XmlNode section)
		{
			throw new System.Configuration.ConfigurationException(
				"Bamboo.Prevalence configuration section must be named " + SectionName + "!", section
				);
		}	

		private void TypeNotFoundError(string type, System.Xml.XmlNode section)
		{
			throw new System.Configuration.ConfigurationException(
				"Type " + type + " not found!", section
				);
		}

		private void SetUpError(Exception inner, System.Xml.XmlNode section)
		{
			throw new System.Configuration.ConfigurationException(
				"Error setting up prevalent engine!", section
				);
		}

		private void AttributeRequiredError(string name, System.Xml.XmlNode section)
		{
			throw new System.Configuration.ConfigurationException(
				"Attribute " + name + " expected!", section
				);
		}
	}
}
