using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TaskManagement.GUI
{
	/// <summary>
	/// Summary description for NewProjectForm.
	/// </summary>
	public class NewProjectForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _txtProjectName;
		private System.Windows.Forms.Button _cmdOK;
		private System.Windows.Forms.Button _cmdCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewProjectForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public string ProjectName
		{
			get
			{
				return _txtProjectName.Text;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.label1 = new System.Windows.Forms.Label();
			this._txtProjectName = new System.Windows.Forms.TextBox();
			this._cmdOK = new System.Windows.Forms.Button();
			this._cmdCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Nome: ";
			// 
			// _txtProjectName
			// 
			this._txtProjectName.Location = new System.Drawing.Point(72, 16);
			this._txtProjectName.Name = "_txtProjectName";
			this._txtProjectName.Size = new System.Drawing.Size(216, 20);
			this._txtProjectName.TabIndex = 1;
			this._txtProjectName.Text = "Novo Projeto";
			// 
			// _cmdOK
			// 
			this._cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._cmdOK.Location = new System.Drawing.Point(72, 56);
			this._cmdOK.Name = "_cmdOK";
			this._cmdOK.TabIndex = 2;
			this._cmdOK.Text = "OK";
			// 
			// _cmdCancel
			// 
			this._cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cmdCancel.Location = new System.Drawing.Point(168, 56);
			this._cmdCancel.Name = "_cmdCancel";
			this._cmdCancel.TabIndex = 3;
			this._cmdCancel.Text = "Cancelar";
			// 
			// NewProjectForm
			// 
			this.AcceptButton = this._cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._cmdCancel;
			this.ClientSize = new System.Drawing.Size(328, 93);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._cmdCancel,
																		  this._cmdOK,
																		  this._txtProjectName,
																		  this.label1});
			this.Name = "NewProjectForm";
			this.Text = "Novo Projeto";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
