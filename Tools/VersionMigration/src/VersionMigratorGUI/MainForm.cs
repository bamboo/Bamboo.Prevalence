using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Bamboo.Prevalence.VersionMigration;

namespace VersionMigratorGUI
{
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

		private Hashtable _resolvedAssemblies;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_resolvedAssemblies = new Hashtable();
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
			this.SuspendLayout();
			// 
			// _selectMigrationPlan
			// 
			this._selectMigrationPlan.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._selectMigrationPlan.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Open;
			this._selectMigrationPlan.FileName = "";
			this._selectMigrationPlan.Filter = "Arquivos XML|*.xml|Todos os Arquivos (*.*)|*.*";
			this._selectMigrationPlan.Label = "Plano de Migração";
			this._selectMigrationPlan.Location = new System.Drawing.Point(4, 8);
			this._selectMigrationPlan.Name = "_selectMigrationPlan";
			this._selectMigrationPlan.Size = new System.Drawing.Size(408, 40);
			this._selectMigrationPlan.TabIndex = 0;
			// 
			// _selectAssembly
			// 
			this._selectAssembly.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Open;
			this._selectAssembly.FileName = "";
			this._selectAssembly.Filter = "Assemblies (*.dll)|*.dll|Todos os Arquivos (*.*)|*.*";
			this._selectAssembly.Label = "Assembly";
			this._selectAssembly.Location = new System.Drawing.Point(4, 56);
			this._selectAssembly.Name = "_selectAssembly";
			this._selectAssembly.Size = new System.Drawing.Size(408, 40);
			this._selectAssembly.TabIndex = 1;
			// 
			// _selectSourceFile
			// 
			this._selectSourceFile.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Open;
			this._selectSourceFile.FileName = "";
			this._selectSourceFile.Filter = "Bamboo.Prevalence Snapshots|*.snapshot|Data Files (*.dat)|*.dat|Todos os Arquivos" +
				" (*.*)|*.*";
			this._selectSourceFile.Label = "Arquivo de Origem";
			this._selectSourceFile.Location = new System.Drawing.Point(4, 104);
			this._selectSourceFile.Name = "_selectSourceFile";
			this._selectSourceFile.Size = new System.Drawing.Size(408, 40);
			this._selectSourceFile.TabIndex = 2;
			// 
			// _selectTargetFile
			// 
			this._selectTargetFile.FileMode = VersionMigratorGUI.FileSelectionPaneFileMode.Save;
			this._selectTargetFile.FileName = "";
			this._selectTargetFile.Filter = "Todos os Arquivos (*.*)|*.*";
			this._selectTargetFile.Label = "Arquivo de Destino";
			this._selectTargetFile.Location = new System.Drawing.Point(4, 152);
			this._selectTargetFile.Name = "_selectTargetFile";
			this._selectTargetFile.Size = new System.Drawing.Size(408, 40);
			this._selectTargetFile.TabIndex = 3;
			// 
			// _btnMigrate
			// 
			this._btnMigrate.Location = new System.Drawing.Point(160, 200);
			this._btnMigrate.Name = "_btnMigrate";
			this._btnMigrate.TabIndex = 4;
			this._btnMigrate.Text = "Iniciar";
			this._btnMigrate.Click += new System.EventHandler(this._btnMigrate_Click);
			// 
			// _dlgSelectAssembly
			// 
			this._dlgSelectAssembly.DefaultExt = "dll";
			this._dlgSelectAssembly.Filter = "Assemblies (*.dll)|*.dll|Assemblies (*.exe)|*.exe|Todos os Arquivos (*.*)|*.*";
			this._dlgSelectAssembly.Title = "Selecione o Assembly";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 237);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._btnMigrate,
																		  this._selectTargetFile,
																		  this._selectSourceFile,
																		  this._selectAssembly,
																		  this._selectMigrationPlan});
			this.Name = "MainForm";
			this.Text = "Version Migrator 0.1";
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

			System.Reflection.Assembly assembly = _resolvedAssemblies[fname] as System.Reflection.Assembly;
			if (null == assembly)
			{
				_dlgSelectAssembly.FileName = fname;
				if (DialogResult.OK == _dlgSelectAssembly.ShowDialog(this))
				{
					assembly = System.Reflection.Assembly.LoadFrom(_dlgSelectAssembly.FileName);
					_resolvedAssemblies[fname] = assembly;
				}
			}
			return assembly;
		}

		private void Migrate()
		{
			MigrationPlan plan = MigrationPlan.Load(_selectMigrationPlan.FileName);

			MigrationContext context = new MigrationContext(plan);
			context.TargetAssembly = System.Reflection.Assembly.LoadFrom(_selectAssembly.FileName);
			context.From = _selectSourceFile.FileName;
			context.To = _selectTargetFile.FileName;
			context.OverwriteFiles = false;

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
	}
}
