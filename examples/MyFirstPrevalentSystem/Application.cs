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

using System;
using System.Collections;
using System.IO;
using Bamboo.Prevalence;

namespace MyFirstPrevalentSystem
{	
	public class Application
	{
		PrevalenceEngine _engine;
		ToDoList _system;		

		public static void Main(string[] args)
		{
			new Application().Run();
		}

		public Application()
		{
			string prevalenceBase = Path.Combine(Environment.CurrentDirectory, "data");

			_engine = PrevalenceActivator.CreateTransparentEngine(typeof(ToDoList), prevalenceBase);
			_system = _engine.PrevalentSystem as ToDoList;
		}

		public void Run()
		{
			bool quit = false;

			while (!quit)
			{
				ShowPendingTasks();

				try
				{
					quit = DisplayUserMenu();
				}
				catch (Exception x)
				{
					WriteLine("Error: {0}", x);
				}				
			}
		}

		private bool DisplayUserMenu()
		{
			switch (UserChoice())
			{
				case 'a':
				{
					AddTask();
					break;
				}

				case 'd':
				{
					DoneWithTask();
					break;
				}

				case 's':
				{
					SystemSnapshot();
					break;
				}

				case 'q':
				{
					return true;
				}
			}
			return false;
		}
		
		private char UserChoice()
		{			
			string choice = Prompt("(A)dd task\t(D)one with task\t(S)napshot\t(Q)uit");			
			return Char.ToLower(choice[0]);
		}

		private void AddTask()
		{
			WriteLine("\n\tAdd Task");			

			Task task = new Task();
			task.Summary = Prompt("Summary: ");

			_system.AddTask(task);
		}

		private void DoneWithTask()
		{
			WriteLine("\n\tDone With Task");
			
			int taskID = int.Parse(Prompt("TaskID: "));
			
			_system.MarkTaskAsDone(taskID);
		}

		private void SystemSnapshot()
		{
			WriteLine("Wait... ");

			_engine.TakeSnapshot();

			WriteLine("Done!");
		}

		private void ShowPendingTasks()
		{
			Console.WriteLine("ID\tDate Created\t\tSummary");
			foreach (Task task in _system.PendingTasks)
			{
				Console.WriteLine("{0}\t{1}\t\t{2}", task.ID, task.DateCreated, task.Summary);
			}
		}
		
		private void WriteLine(string message)
		{
			Console.WriteLine(message);
		}
		
		private void WriteLine(string format, params object[] args)
		{
			WriteLine(string.Format(format, args));
		}
		
		private string Prompt(string prompt)
		{
			WriteLine(prompt);
			return Console.ReadLine();
		}
	}
}