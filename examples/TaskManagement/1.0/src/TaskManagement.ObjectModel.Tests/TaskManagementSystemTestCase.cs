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
