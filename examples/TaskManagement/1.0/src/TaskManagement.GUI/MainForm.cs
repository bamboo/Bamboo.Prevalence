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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Bamboo.Prevalence;
using TaskManagement.ObjectModel;
using TaskManagement.ObjectModel.Commands;

namespace TaskManagement.GUI
{
	/// <summary>
	/// 
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;

		
		private System.Windows.Forms.ToolBar _toolbar;
		private System.Windows.Forms.ToolBarButton _btnNewProject;
		private System.Windows.Forms.ImageList _images;
		private System.Windows.Forms.StatusBar _status;
		private System.Windows.Forms.TreeView _tvProjects;
		private System.Windows.Forms.Splitter _vsplitter;
		private System.Windows.Forms.ContextMenu _menuProject;
		private System.Windows.Forms.MenuItem _menuItemNewTask;
		private System.Windows.Forms.Splitter _hsplitter;
		private System.Windows.Forms.ListView _lvTasks;
		private System.Windows.Forms.ListView _lvWorkRecords;
		private System.Windows.Forms.ColumnHeader columnHeader1;

		PrevalenceEngine _engine;
		TaskManagementSystem _system;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			_engine = PrevalenceActivator.CreateEngine(typeof(TaskManagementSystem), PrevalenceBase);
			_system = _engine.PrevalentSystem as TaskManagementSystem;

			RefreshProjectView();

			_status.Text = PrevalenceBase;
		}

		object ExecuteCommand(ICommand command)
		{
			return _engine.ExecuteCommand(command);
		}

		string PrevalenceBase
		{
			get
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TaskManagementSystem");
			}
		}

		void RefreshProjectView()
		{
			_tvProjects.Nodes.Clear();

			TreeNode root = _tvProjects.Nodes.Add("Projetos");
			foreach (Project project in _system.Projects)
			{
				root.Nodes.Add(new ProjectTreeNode(project));				
			}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this._toolbar = new System.Windows.Forms.ToolBar();
			this._btnNewProject = new System.Windows.Forms.ToolBarButton();
			this._images = new System.Windows.Forms.ImageList(this.components);
			this._status = new System.Windows.Forms.StatusBar();
			this._tvProjects = new System.Windows.Forms.TreeView();
			this._vsplitter = new System.Windows.Forms.Splitter();
			this._menuProject = new System.Windows.Forms.ContextMenu();
			this._menuItemNewTask = new System.Windows.Forms.MenuItem();
			this._lvTasks = new System.Windows.Forms.ListView();
			this._hsplitter = new System.Windows.Forms.Splitter();
			this._lvWorkRecords = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// _toolbar
			// 
			this._toolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this._toolbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this._btnNewProject});
			this._toolbar.ButtonSize = new System.Drawing.Size(47, 33);
			this._toolbar.Cursor = System.Windows.Forms.Cursors.Arrow;
			this._toolbar.DropDownArrows = true;
			this._toolbar.ImageList = this._images;
			this._toolbar.Name = "_toolbar";
			this._toolbar.ShowToolTips = true;
			this._toolbar.Size = new System.Drawing.Size(552, 36);
			this._toolbar.TabIndex = 2;
			this._toolbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this._toolbar_ButtonClick);
			// 
			// _btnNewProject
			// 
			this._btnNewProject.ImageIndex = 0;
			// 
			// _images
			// 
			this._images.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this._images.ImageSize = new System.Drawing.Size(16, 16);
			this._images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_images.ImageStream")));
			this._images.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _status
			// 
			this._status.Location = new System.Drawing.Point(0, 311);
			this._status.Name = "_status";
			this._status.Size = new System.Drawing.Size(552, 22);
			this._status.TabIndex = 3;
			this._status.Text = "status";
			// 
			// _tvProjects
			// 
			this._tvProjects.Dock = System.Windows.Forms.DockStyle.Left;
			this._tvProjects.ImageIndex = -1;
			this._tvProjects.Location = new System.Drawing.Point(0, 36);
			this._tvProjects.Name = "_tvProjects";
			this._tvProjects.SelectedImageIndex = -1;
			this._tvProjects.Size = new System.Drawing.Size(160, 275);
			this._tvProjects.TabIndex = 4;
			this._tvProjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tvProjects_AfterSelect);
			// 
			// _vsplitter
			// 
			this._vsplitter.Location = new System.Drawing.Point(160, 36);
			this._vsplitter.Name = "_vsplitter";
			this._vsplitter.Size = new System.Drawing.Size(4, 275);
			this._vsplitter.TabIndex = 5;
			this._vsplitter.TabStop = false;
			// 
			// _menuProject
			// 
			this._menuProject.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this._menuItemNewTask});
			this._menuProject.Popup += new System.EventHandler(this._menuProject_Popup);
			// 
			// _menuItemNewTask
			// 
			this._menuItemNewTask.Index = 0;
			this._menuItemNewTask.Text = "&Nova Tarefa...";
			this._menuItemNewTask.Click += new System.EventHandler(this._menuItemNewTask_Click);
			// 
			// _lvTasks
			// 
			this._lvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.columnHeader1});
			this._lvTasks.Dock = System.Windows.Forms.DockStyle.Top;
			this._lvTasks.Location = new System.Drawing.Point(164, 36);
			this._lvTasks.Name = "_lvTasks";
			this._lvTasks.Size = new System.Drawing.Size(388, 172);
			this._lvTasks.TabIndex = 6;
			this._lvTasks.View = System.Windows.Forms.View.Details;
			// 
			// _hsplitter
			// 
			this._hsplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this._hsplitter.Location = new System.Drawing.Point(164, 208);
			this._hsplitter.Name = "_hsplitter";
			this._hsplitter.Size = new System.Drawing.Size(388, 3);
			this._hsplitter.TabIndex = 7;
			this._hsplitter.TabStop = false;
			// 
			// _lvWorkRecords
			// 
			this._lvWorkRecords.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lvWorkRecords.Location = new System.Drawing.Point(164, 211);
			this._lvWorkRecords.Name = "_lvWorkRecords";
			this._lvWorkRecords.Size = new System.Drawing.Size(388, 100);
			this._lvWorkRecords.TabIndex = 8;
			this._lvWorkRecords.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Tarefa";
			this.columnHeader1.Width = 300;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(552, 333);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._lvWorkRecords,
																		  this._hsplitter,
																		  this._lvTasks,
																		  this._vsplitter,
																		  this._tvProjects,
																		  this._status,
																		  this._toolbar});
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "MainForm";
			this.Text = "Gerenciamento de Tarefas";
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

		private void _toolbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (_btnNewProject == e.Button)
			{
				OnNewProject();
			}
		}

		void OnNewProject()
		{
			NewProjectForm dlg = new NewProjectForm();
			if (DialogResult.OK == dlg.ShowDialog(this))
			{
				ExecuteCommand(new AddProjectCommand(new Project(dlg.ProjectName)));
				RefreshProjectView();
			}
		}

		private void _menuProject_Popup(object sender, System.EventArgs e)
		{
		
		}

		private void _tvProjects_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			ProjectTreeNode node = e.Node as ProjectTreeNode;
			if (null != node)
			{
				_tvProjects.ContextMenu = _menuProject;
				node.RefreshTaskView(_lvTasks);
			}
			else
			{
				_tvProjects.ContextMenu = null;
				_lvTasks.Items.Clear();
			}
		}

		private void _menuItemNewTask_Click(object sender, System.EventArgs e)
		{
			ProjectTreeNode node = _tvProjects.SelectedNode as ProjectTreeNode;
			if (null != node)
			{
				NewTaskForm dlg = new NewTaskForm();
				if (DialogResult.OK == dlg.ShowDialog(this))
				{
					ExecuteCommand(new AddTaskCommand(node.Project.ID, dlg.Task));
				}
			}
		}
	}
}
