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
using System.ComponentModel;
using System.Windows.Forms;

namespace VersionMigratorGUI
{
	public enum FileSelectionPaneFileMode
	{
		Open,
		Save
	}

	/// <summary>
	/// Summary description for FileSelectionPane.
	/// </summary>
	public class FileSelectionPane : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label _label;
		private System.Windows.Forms.TextBox _filename;
		private System.Windows.Forms.Button _btnBrowse;

		private string _filter = "Todos os Arquivos (*.*)|*.*";

		private FileSelectionPaneFileMode _mode = FileSelectionPaneFileMode.Open;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FileSelectionPane()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		public event EventHandler FileNameChanged;
		
		public FileSelectionPaneFileMode FileMode
		{
			get
			{
				return _mode;
			}

			set
			{
				_mode = value;
			}
		}

		public string FileName
		{
			get
			{
				return _filename.Text;
			}

			set
			{
				_filename.Text = value;
			}
		}

		public string Label
		{
			get
			{
				return _label.Text;
			}

			set
			{
				_label.Text = value;
			}
		}

		public string Filter
		{
			get
			{
				return _filter;
			}

			set
			{
				_filter = value;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._label = new System.Windows.Forms.Label();
			this._filename = new System.Windows.Forms.TextBox();
			this._btnBrowse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _label
			// 
			this._label.Location = new System.Drawing.Point(16, 12);
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size(100, 16);
			this._label.TabIndex = 0;
			this._label.Text = "Arquivo";
			// 
			// _filename
			// 
			this._filename.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this._filename.Location = new System.Drawing.Point(120, 10);
			this._filename.Name = "_filename";
			this._filename.Size = new System.Drawing.Size(296, 20);
			this._filename.TabIndex = 1;
			this._filename.Text = "";
			this._filename.TextChanged += new System.EventHandler(this._filename_TextChanged);
			// 
			// _btnBrowse
			// 
			this._btnBrowse.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this._btnBrowse.Location = new System.Drawing.Point(424, 8);
			this._btnBrowse.Name = "_btnBrowse";
			this._btnBrowse.Size = new System.Drawing.Size(48, 24);
			this._btnBrowse.TabIndex = 2;
			this._btnBrowse.Text = "...";
			this._btnBrowse.Click += new System.EventHandler(this._btnBrowse_Click);
			// 
			// FileSelectionPane
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._btnBrowse,
																		  this._filename,
																		  this._label});
			this.Name = "FileSelectionPane";
			this.Size = new System.Drawing.Size(496, 40);
			this.ResumeLayout(false);

		}
		#endregion

		private void _btnBrowse_Click(object sender, System.EventArgs e)
		{
			FileDialog dlg = null;
			if (_mode == FileSelectionPaneFileMode.Open)
			{
				dlg = new OpenFileDialog();
			}
			else
			{
				SaveFileDialog sdlg = new SaveFileDialog();
				sdlg.OverwritePrompt = true;
				dlg = sdlg;
			}
			dlg.Filter = Filter;		
			if (DialogResult.OK == dlg.ShowDialog(this))
			{
				_filename.Text = dlg.FileName;
			}
		}

		private void _filename_TextChanged(object sender, System.EventArgs e)
		{
			if (null != FileNameChanged)
			{
				FileNameChanged(this, e);
			}
		}
	}
}
