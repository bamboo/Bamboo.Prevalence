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
			// O primeiro passo é limpar qualquer resquício de
			// testes anteriores para começar com uma "base" limpa
			ClearPrevalenceBase();

			_engine = PrevalenceActivator.CreateEngine(typeof(TaskManagementSystem), PrevalenceBase);
			_system = _engine.PrevalentSystem as TaskManagementSystem;
		}

		[TearDown]
		public void TearDown()
		{
			// Caso exista um PrevalenceEngine
			// assegura que ele "tire suas mãos do log"
			// para permitir a limpeza da base
			if (null != _engine)
			{
				_engine.HandsOffOutputLog();
			}
		}

		[Test]
		public void TestConstruct()
		{
			AssertNotNull("A coleção de projetos não deve ser nula!", _system.Projects);
			AssertEquals("A coleção de projetos deve estar vazia!", 0, _system.Projects.Count);
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

			Task task = new Task("Prevalência de Objetos");
			ExecuteCommand(new AddTaskCommand(project.ID, task));

			AssertEquals(1, project.Tasks.Count);
			AssertSame(task, project.Tasks[0]);
		}

		[Test]
		public void TestAddWorkRecord()
		{
			Project project = new Project("Artigos");
			ExecuteCommand(new AddProjectCommand(project));

			Task task = new Task("Prevalência de Objetos");
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
		/// Caminho completo para o diretório onde serão
		/// armazenados arquivos de log Bamboo.Prevalence.
		/// </summary>
		protected string PrevalenceBase
		{
			get
			{
				// calcula um caminho abaixo da pasta
				// de arquivos temporários
				return Path.Combine(Path.GetTempPath(), "TaskManagementSystem");
			}
		}

		/// <summary>
		/// Remove o diretório PrevalenceBase caso ele exista.
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
