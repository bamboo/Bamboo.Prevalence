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
