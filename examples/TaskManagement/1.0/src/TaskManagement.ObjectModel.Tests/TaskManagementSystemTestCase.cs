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
using System.IO;
using NUnit.Framework;
using Bamboo.Prevalence;
using TaskManagement.ObjectModel;
using TaskManagement.ObjectModel.Commands;

namespace TaskManagement.ObjectModel.Tests
{
	/// <summary>
	/// Testes para a classe TaskManagementSystem.
	/// </summary>
	[TestFixture]
	public class TaskManagementSystemTestCase : Assertion
	{
		protected PrevalenceEngine _engine;

		protected TaskManagementSystem _system;

		[SetUp]
		public void SetUp()
		{
			// O primeiro passo � limpar qualquer resqu�cio de
			// testes anteriores para come�ar com uma "base" limpa
			ClearPrevalenceBase();

			_engine = PrevalenceActivator.CreateEngine(typeof(TaskManagementSystem), PrevalenceBase);
			_system = _engine.PrevalentSystem as TaskManagementSystem;
		}

		[TearDown]
		public void TearDown()
		{
			// Caso exista um PrevalenceEngine
			// assegura que ele "tire suas m�os do log"
			// para permitir a limpeza da base
			if (null != _engine)
			{
				_engine.HandsOffOutputLog();
			}
		}

		[Test]
		public void TestConstruct()
		{
			AssertNotNull("A cole��o de projetos n�o deve ser nula!", _system.Projects);
			AssertEquals("A cole��o de projetos deve estar vazia!", 0, _system.Projects.Count);
		}

		[Test]
		public void TestAddProject()
		{
			Project project = new Project("Artigos");
			ExecuteCommand(new AddProjectCommand(project));

			AssertEquals(1, _system.Projects.Count);
			AssertSame(project, _system.Projects[0]);
		}

		[Test]
		public void TestAddTask()
		{
			Project project = new Project("Artigos");
			ExecuteCommand(new AddProjectCommand(project));

			Task task = new Task("Preval�ncia de Objetos");
			ExecuteCommand(new AddTaskCommand(project.ID, task));

			AssertEquals(1, project.Tasks.Count);
			AssertSame(task, project.Tasks[0]);
		}

		[Test]
		public void TestAddWorkRecord()
		{
			Project project = new Project("Artigos");
			ExecuteCommand(new AddProjectCommand(project));

			Task task = new Task("Preval�ncia de Objetos");
			ExecuteCommand(new AddTaskCommand(project.ID, task));

			DateTime startTime = new DateTime(2003, 6, 29, 13, 26, 0);
			DateTime endTime = startTime.AddHours(5);
			WorkRecord record = new WorkRecord(startTime, endTime);

			ExecuteCommand(new AddWorkRecordCommand(project.ID, task.ID, record));
			AssertEquals(1, task.WorkRecords.Count);
			AssertSame(record, task.WorkRecords[0]);
		}

		/// <summary>
		/// Executa um comando.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		protected object ExecuteCommand(ICommand command)
		{
			return _engine.ExecuteCommand(command);
		}

		/// <summary>
		/// Caminho completo para o diret�rio onde ser�o
		/// armazenados arquivos de log Bamboo.Prevalence.
		/// </summary>
		protected string PrevalenceBase
		{
			get
			{
				// calcula um caminho abaixo da pasta
				// de arquivos tempor�rios
				return Path.Combine(Path.GetTempPath(), "TaskManagementSystem");
			}
		}

		/// <summary>
		/// Remove o diret�rio PrevalenceBase caso ele exista.
		/// </summary>
		protected void ClearPrevalenceBase()
		{
			if (Directory.Exists(PrevalenceBase))
			{
				Directory.Delete(PrevalenceBase, true);
			}
		}
	}
}
