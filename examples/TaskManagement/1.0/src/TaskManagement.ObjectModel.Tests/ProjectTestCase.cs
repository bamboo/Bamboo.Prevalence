using System;
using NUnit.Framework;
using TaskManagement.ObjectModel;

namespace TaskManagement.ObjectModel.Tests
{
	/// <summary>
	/// Teste para a classe Project.
	/// </summary>
	[TestFixture]
	public class ProjectTestCase : Assertion
	{		
		Project _project;

		[SetUp]
		public void SetUp()
		{
			_project = new Project("Artigos");
		}

		[Test]
		public void TestConstruct()
		{
			AssertEquals("O projeto deve armazenar seu nome!", "Artigos", _project.Name);
			AssertNotNull("Lista de tarefas não pode ser nula!", _project.Tasks);
			AssertEquals("Lista de tarefas deve estar vazia", 0, _project.Tasks.Count);
		}
	}
}
