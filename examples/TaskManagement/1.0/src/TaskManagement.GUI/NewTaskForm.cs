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
using TaskManagement.ObjectModel;

namespace TaskManagement.GUI
{
	/// <summary>
	/// Summary description for NewTaskForm.
	/// </summary>
	public class NewTaskForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox _txtTitle;
		private System.Windows.Forms.TextBox _txtDetails;
		private System.Windows.Forms.Button _cmdOK;
		private System.Windows.Forms.Button _cmdCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected Task _task;

		public NewTaskForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		public Task Task
		{
			get
			{
				return _task;
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._txtTitle = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._txtDetails = new System.Windows.Forms.TextBox();
			this._cmdOK = new System.Windows.Forms.Button();
			this._cmdCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _txtTitle
			// 
			this._txtTitle.Location = new System.Drawing.Point(88, 16);
			this._txtTitle.Name = "_txtTitle";
			this._txtTitle.Size = new System.Drawing.Size(320, 20);
			this._txtTitle.TabIndex = 0;
			this._txtTitle.Text = "";
			this._txtTitle.Validating += new System.ComponentModel.CancelEventHandler(this._txtTitle_Validating);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Título: ";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this._txtDetails});
			this.groupBox1.Location = new System.Drawing.Point(24, 48);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(384, 128);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Detalhes ";
			// 
			// _txtDetails
			// 
			this._txtDetails.Location = new System.Drawing.Point(16, 24);
			this._txtDetails.Multiline = true;
			this._txtDetails.Name = "_txtDetails";
			this._txtDetails.Size = new System.Drawing.Size(352, 88);
			this._txtDetails.TabIndex = 0;
			this._txtDetails.Text = "";
			// 
			// _cmdOK
			// 
			this._cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._cmdOK.Location = new System.Drawing.Point(112, 192);
			this._cmdOK.Name = "_cmdOK";
			this._cmdOK.Size = new System.Drawing.Size(88, 24);
			this._cmdOK.TabIndex = 3;
			this._cmdOK.Text = "OK";
			this._cmdOK.Click += new System.EventHandler(this._cmdOK_Click);
			// 
			// _cmdCancel
			// 
			this._cmdCancel.CausesValidation = false;
			this._cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cmdCancel.Location = new System.Drawing.Point(216, 192);
			this._cmdCancel.Name = "_cmdCancel";
			this._cmdCancel.Size = new System.Drawing.Size(80, 24);
			this._cmdCancel.TabIndex = 4;
			this._cmdCancel.Text = "Cancelar";
			// 
			// NewTaskForm
			// 
			this.AcceptButton = this._cmdOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this._cmdCancel;
			this.ClientSize = new System.Drawing.Size(440, 237);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._cmdCancel,
																		  this._cmdOK,
																		  this.groupBox1,
																		  this.label1,
																		  this._txtTitle});
			this.Name = "NewTaskForm";
			this.Text = "Nova Tarefa";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void _txtTitle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = 0 == _txtTitle.Text.Length;
		}

		private void _cmdOK_Click(object sender, System.EventArgs e)
		{
			Task task = new Task(_txtTitle.Text);
			_task = task;
		}
	}
}
