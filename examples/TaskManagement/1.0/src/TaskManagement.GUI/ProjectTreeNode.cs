using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using TaskManagement.ObjectModel;

namespace TaskManagement.GUI
{
	/// <summary>
	/// Nó que mantém um projeto
	/// </summary>
	public class ProjectTreeNode : TreeNode
	{
		protected Project _project;

		public ProjectTreeNode(Project project) : base(project.Name)
		{
			_project = project;
		}

		public Project Project
		{
			get
			{
				return _project;
			}
		}

		public void RefreshTaskView(ListView view)
		{
			view.BeginUpdate();
			try
			{
				view.Items.Clear();
				foreach (Task task in _project.Tasks)
				{
					ListViewItem item = view.Items.Add(task.Name);
				}
			}
			finally
			{
				view.EndUpdate();
			}
		}
	}
}
