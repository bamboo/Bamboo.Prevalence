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