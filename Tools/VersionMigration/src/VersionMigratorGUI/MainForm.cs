#region License
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
#endregion

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Bamboo.Prevalence.VersionMigration;

namespace VersionMigratorGUI
{
	public class Preferences
	{
		public string LastProject;

		public Preferences()
		{
			LastProject = string.Empty;
		}

		public void Save(string fname)
		{
			using (FileStream stream = File.OpenWrite(fname))
			{
				CreateSerializer().Serialize(stream, this);
				stream.Flush();
			}
		}

		public static Preferences Load(string fname)
		{
			using (FileStream stream = File.OpenRead(fname))
			{
				return (Preferences)CreateSerializer().Deserialize(stream);
			}
		}

		static XmlSerializer CreateSerializer()
		{
			return new XmlSerializer(typeof(Preferences));
		}
	}

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private VersionMigratorGUI.FileSelectionPane _selectMigrationPlan;
		private VersionMigratorGUI.FileSelectionPane _selectAssembly;
		private VersionMigratorGUI.FileSelectionPane _selectSourceFile;
		private VersionMigratorGUI.FileSelectionPane _selectTargetFile;
		private System.Windows.Forms.Button _btnMigrate;
		private System.Windows.Forms.OpenFileDialog _dlgSelectAssembly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu _menu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem _miNew;
		private System.Windows.Forms.MenuItem _miOpen;
		private System.Windows.Forms.MenuItem _miSave;
		private System.Windows.Forms.MenuItem _miSaveAs;
		private System.Windows.Forms.MenuItem _miExit;

		private Hashtable _resolvedAssemblies;

		private MigrationProject _project;
		private System.Windows.Forms.OpenFileDialog _dlgOpen;
		private System.Windows.Forms.SaveFileDialog _dlgSave;

		private Preferences _preferences;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			LoadPreferences();

			LoadLastProjectOrCreateNew();

			_resolvedAssemblies = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._selectMigrationPlan = new VersionMigratorGUI.FileSelectionPane();
			this._selectAssembly = new VersionMigratorGUI.FileSelectionPane();
			this._selectSourceFile = new VersionMigratorGUI.FileSelectionPane();
			this._selectTargetFile = new VersionMigratorGUI.FileSelectionPane();
			this._btnMigrate = new System.Windows.Forms.Button();
			this._dlgSelectAssembly = new System.Windows.Forms.OpenFileDialog();
			this._menu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this._miNew = new System.Windows.Forms.MenuItem();
			this._miOpen = new System.Windows.Forms.MenuItem();
			this._miSave = new System.Windows.Forms.MenuItem();
			this._miSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this._miExit = new System.Windows.Forms.MenuItem();
			this._dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this._dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// _selectMigrationPlan
			// 
			this._selectMigrationPlan.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._selectMigrationPlan.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Open;
			this._selectMigrationPlan.FileName = "";
			this._selectMigrationPlan.Filter = "XML Files|*.xml|All Files (*.*)|*.*";
			this._selectMigrationPlan.Label = "Migration Plan";
			this._selectMigrationPlan.Location = new System.Drawing.Point(4, 8);
			this._selectMigrationPlan.Name = "_selectMigrationPlan";
			this._selectMigrationPlan.Size = new System.Drawing.Size(408, 40);
			this._selectMigrationPlan.TabIndex = 0;
			this._selectMigrationPlan.FileNameChanged += new System.EventHandler(this._selectMigrationPlan_FileNameChanged);
			// 
			// _selectAssembly
			// 
			this._selectAssembly.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._selectAssembly.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Open;
			this._selectAssembly.FileName = "";
			this._selectAssembly.Filter = "Libraries (*.dll)|*.dll|Executables (*.exe)|*.exe|All Files (*.*)|*.*";
			this._selectAssembly.Label = "Main Assembly";
			this._selectAssembly.Location = new System.Drawing.Point(4, 56);
			this._selectAssembly.Name = "_selectAssembly";
			this._selectAssembly.Size = new System.Drawing.Size(408, 40);
			this._selectAssembly.TabIndex = 1;
			this._selectAssembly.FileNameChanged += new System.EventHandler(this._selectAssembly_FileNameChanged);
			// 
			// _selectSourceFile
			// 
			this._selectSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._selectSourceFile.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Open;
			this._selectSourceFile.FileName = "";
			this._selectSourceFile.Filter = "Bamboo.Prevalence Snapshots|*.snapshot|Data Files (*.dat)|*.dat|All Files (*.*)|*" +
				".*";
			this._selectSourceFile.Label = "Source File";
			this._selectSourceFile.Location = new System.Drawing.Point(4, 104);
			this._selectSourceFile.Name = "_selectSourceFile";
			this._selectSourceFile.Size = new System.Drawing.Size(408, 40);
			this._selectSourceFile.TabIndex = 2;
			this._selectSourceFile.FileNameChanged += new System.EventHandler(this._selectSourceFile_FileNameChanged);
			// 
			// _selectTargetFile
			// 
			this._selectTargetFile.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._selectTargetFile.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Save;
			this._selectTargetFile.FileName = "";
			this._selectTargetFile.Filter = "All Files (*.*)|*.*";
			this._selectTargetFile.Label = "Target File";
			this._selectTargetFile.Location = new System.Drawing.Point(4, 152);
			this._selectTargetFile.Name = "_selectTargetFile";
			this._selectTargetFile.Size = new System.Drawing.Size(408, 40);
			this._selectTargetFile.TabIndex = 3;
			this._selectTargetFile.FileNameChanged += new System.EventHandler(this._selectTargetFile_FileNameChanged);
			// 
			// _btnMigrate
			// 
			this._btnMigrate.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._btnMigrate.Location = new System.Drawing.Point(160, 200);
			this._btnMigrate.Name = "_btnMigrate";
			this._btnMigrate.TabIndex = 4;
			this._btnMigrate.Text = "Migrate";
			this._btnMigrate.Click += new System.EventHandler(this._btnMigrate_Click);
			// 
			// _dlgSelectAssembly
			// 
			this._dlgSelectAssembly.DefaultExt = "dll";
			this._dlgSelectAssembly.Filter = "Assemblies (*.dll)|*.dll|Assemblies (*.exe)|*.exe|Todos os Arquivos (*.*)|*.*";
			this._dlgSelectAssembly.Title = "Selecione o Assembly";
			// 
			// _menu
			// 
			this._menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this._miNew,
																					  this._miOpen,
																					  this._miSave,
																					  this._miSaveAs,
																					  this.menuItem6,
																					  this._miExit});
			this.menuItem1.Text = "&File";
			// 
			// _miNew
			// 
			this._miNew.Index = 0;
			this._miNew.Text = "&New";
			this._miNew.Click += new System.EventHandler(this._miNew_Click);
			// 
			// _miOpen
			// 
			this._miOpen.Index = 1;
			this._miOpen.Text = "&Open...";
			this._miOpen.Click += new System.EventHandler(this._miOpen_Click);
			// 
			// _miSave
			// 
			this._miSave.Index = 2;
			this._miSave.Text = "&Save";
			this._miSave.Click += new System.EventHandler(this._miSave_Click);
			// 
			// _miSaveAs
			// 
			this._miSaveAs.Index = 3;
			this._miSaveAs.Text = "Save &as...";
			this._miSaveAs.Click += new System.EventHandler(this._miSaveAs_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// _miExit
			// 
			this._miExit.Index = 5;
			this._miExit.Text = "E&xit";
			this._miExit.Click += new System.EventHandler(this._miExit_Click);
			// 
			// _dlgOpen
			// 
			this._dlgOpen.DefaultExt = "xml";
			this._dlgOpen.Filter = "Project Files (*.xml)|*.xml|All Files (*.*)|*.*";
			// 
			// _dlgSave
			// 
			this._dlgSave.DefaultExt = "xml";
			this._dlgSave.Filter = "Project Files (*.xml)|*.xml|All Files (*.*)|*.*";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(408, 237);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._btnMigrate,
																		  this._selectTargetFile,
																		  this._selectSourceFile,
																		  this._selectAssembly,
																		  this._selectMigrationPlan});
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Menu = this._menu;
			this.Name = "MainForm";
			this.Text = "Version Migrator 0.1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void _btnMigrate_Click(object sender, System.EventArgs e)
		{
			if (CheckFileNames())
			{
				Migrate();
			}			
		}

		private bool CheckFileNames()
		{
			if (0 == _selectMigrationPlan.FileName.Length)
			{
				_selectMigrationPlan.Focus();
				return false;
			}

			if (0 == _selectAssembly.FileName.Length)
			{
				_selectAssembly.Focus();
				return false;
			}

			if (0 == _selectSourceFile.FileName.Length)
			{
				_selectSourceFile.Focus();
				return false;
			}

			if (0 == _selectTargetFile.FileName.Length)
			{
				_selectTargetFile.Focus();
				return false;
			}
			return true;
		}

		private System.Reflection.Assembly HandleResolveAssembly(object sender, ResolveEventArgs args)
		{
			string fname = args.Name.Split(',')[0] + ".dll";

			if (_resolvedAssemblies.ContainsKey(fname))
			{
				return (System.Reflection.Assembly)_resolvedAssemblies[fname];
			}

			System.Reflection.Assembly assembly = null;

			_dlgSelectAssembly.FileName = fname;
			if (DialogResult.OK == _dlgSelectAssembly.ShowDialog(this))
			{
				assembly = System.Reflection.Assembly.LoadFrom(_dlgSelectAssembly.FileName);					
			}
			_resolvedAssemblies[fname] = assembly;
			return assembly;
		}

		private void Migrate()
		{
			MigrationContext context = new MigrationContext(_project);
			
			context.ResolveAssembly += new ResolveEventHandler(HandleResolveAssembly);

			try
			{
				context.Migrate();
			}
			finally
			{
				_resolvedAssemblies.Clear();
			}
		}

		string PreferencesPath
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VersionMigratorGUI.preferences.xml");
			}
		}

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (CloseProject())
			{
				SavePreferences();
			}
			else
			{
				e.Cancel = true;
			}
		}

		void SavePreferences()
		{
			_preferences.LastProject = _project.FileName;
			_preferences.Save(PreferencesPath);
		}

		void LoadLastProjectOrCreateNew()
		{
			if (File.Exists(_preferences.LastProject))
			{
				_project = MigrationProject.Load(_preferences.LastProject);				
			}
			else
			{
				_project = new MigrationProject();
			}
			TransferProjectData();			
			UpdateTitle();
		}

		void UpdateTitle()
		{
			Text = "VersionMigratorGUI 0.1 [" + (string.Empty != _project.FileName ? Path.GetFileName(_project.FileName) : "Unnamed")  + "]" + (_project.IsDirty ? " *" : "");
		}

		void TransferDialogData()
		{			
			_project.MigrationPlan = _selectMigrationPlan.FileName;
			_project.MainAssembly =  _selectAssembly.FileName;
			_project.SourceFile = _selectSourceFile.FileName;
			_project.TargetFile = _selectTargetFile.FileName;			
		}

		void TransferProjectData()
		{
			bool oldIsDirty = _project.IsDirty;
			_selectMigrationPlan.FileName = _project.MigrationPlan;
			_selectAssembly.FileName = _project.MainAssembly;
			_selectSourceFile.FileName = _project.SourceFile;
			_selectTargetFile.FileName = _project.TargetFile;			
			_project.IsDirty = oldIsDirty;
		}
		
		void LoadPreferences()
		{
			if (File.Exists(PreferencesPath))
			{	
				try
				{
					_preferences = Preferences.Load(PreferencesPath);
				}
				catch (Exception)
				{
					_preferences = new Preferences();
				}
			}
			else
			{
				_preferences = new Preferences();
			}
		}

		private void _miExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void _miOpen_Click(object sender, System.EventArgs e)
		{
			if (DialogResult.OK == _dlgOpen.ShowDialog())
			{
				_project = MigrationProject.Load(_dlgOpen.FileName);
				TransferProjectData();
				UpdateTitle();
			}
		}

		private void _miSave_Click(object sender, System.EventArgs e)
		{
			if (string.Empty != _project.FileName)
			{
				Save();
			}
			else
			{
				SaveAs();
			}
		}

		void Save()
		{
			_project.Save();
			UpdateTitle();
		}

		void SaveAs()
		{
			if (DialogResult.OK == _dlgSave.ShowDialog(this))
			{
				_project.Save(_dlgSave.FileName);
				UpdateTitle();
			}
		}

		private void _selectMigrationPlan_FileNameChanged(object sender, System.EventArgs e)
		{
			_project.MigrationPlan = _selectMigrationPlan.FileName;
			UpdateTitle();
		}

		private void _selectAssembly_FileNameChanged(object sender, System.EventArgs e)
		{
			_project.MainAssembly = _selectAssembly.FileName;
			UpdateTitle();
		}

		private void _selectSourceFile_FileNameChanged(object sender, System.EventArgs e)
		{
			_project.SourceFile  = _selectSourceFile.FileName;
			UpdateTitle();
		}

		private void _selectTargetFile_FileNameChanged(object sender, System.EventArgs e)
		{
			_project.TargetFile = _selectTargetFile.FileName;			
			UpdateTitle();
		}

		private void _miNew_Click(object sender, System.EventArgs e)
		{
			if (CloseProject())
			{
				_project = new MigrationProject();
				TransferProjectData();
				UpdateTitle();
			}
		}

		bool CloseProject()
		{
			if (_project.IsDirty)
			{
				return DialogResult.Yes == MessageBox.Show(this, "Do you really want to close the current project and lose your changes?", "VersionMigratorGUI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			}
			return true;
		}

		private void _miSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveAs();
		}
	}
}
