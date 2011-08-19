using System.Collections.Generic;
using BuildMonkey.SelectionStrategies;
using BuildMonkey.TeamCity;
using FakeItEasy;
using log4net;
using NUnit.Framework;

namespace BuildMonkey.Tests
{
	public class BuildMonkeyTests
	{
		private IProjectRepository _projectRepository;
		private BuildMonkey _buildMonkey;
		private ISelectionStrategy<Project> _projectSelectionStrategy;
		private ISelectionStrategy<BuildType> _buildTypeSelectionStrategy;
		private IBuildTypeRunner _buildTypeRunner;

		[SetUp]
		public void SetUp()
		{
			_projectRepository = A.Fake<IProjectRepository>();
			_projectSelectionStrategy = A.Fake<ISelectionStrategy<Project>>();
			_buildTypeSelectionStrategy = A.Fake<ISelectionStrategy<BuildType>>();
			_buildTypeRunner = A.Fake<IBuildTypeRunner>();
			var logger = A.Fake<ILog>();
			_buildMonkey = new BuildMonkey(_projectRepository, _projectSelectionStrategy, _buildTypeSelectionStrategy, _buildTypeRunner, logger);
		}

		[Test]
		public void Get_all_projects()
		{
			_buildMonkey.GrabABanana();

			A.CallTo(() => _projectRepository.GetAllProjects()).MustHaveHappened();
		}

		[Test]
		public void Do_nothing_if_no_projects_are_found()
		{
			A.CallTo(() => _projectRepository.GetAllProjects()).Returns(new List<Project>());

			_buildMonkey.GrabABanana();

			A.CallTo(() => _buildTypeRunner.Run(A<BuildType>.Ignored)).MustNotHaveHappened();
		}

		[Test]
		public void Select_a_project()
		{
			var allProjects = new List<Project> { new Project(null) };
			A.CallTo(() => _projectRepository.GetAllProjects()).Returns(allProjects);

			_buildMonkey.GrabABanana();

			A.CallTo(() => _projectSelectionStrategy.ChooseFrom(allProjects)).MustHaveHappened();
		}

		[Test]
		public void Select_a_build()
		{
			A.CallTo(() => _projectRepository.GetAllProjects()).Returns(new List<Project> { new Project(null) });
			var buildTypes = new List<BuildType>();
			A.CallTo(() => _projectSelectionStrategy.ChooseFrom(A<IEnumerable<Project>>.Ignored)).Returns(new TestProject(buildTypes));

			_buildMonkey.GrabABanana();

			A.CallTo(() => _buildTypeSelectionStrategy.ChooseFrom(buildTypes)).MustHaveHappened();
		}

		[Test]
		public void Run_it()
		{
			A.CallTo(() => _projectRepository.GetAllProjects()).Returns(new List<Project> { new Project(null) });
			var buildType = new BuildType();
			A.CallTo(() => _buildTypeSelectionStrategy.ChooseFrom(A<IEnumerable<BuildType>>.Ignored)).Returns(buildType);

			_buildMonkey.GrabABanana();

			A.CallTo(() => _buildTypeRunner.Run(buildType)).MustHaveHappened();
		}

		private class TestProject : Project
		{
			private readonly IEnumerable<BuildType> _buildTypes;

			public TestProject(IEnumerable<BuildType> buildTypes)
			{
				_buildTypes = buildTypes;
			}

			public override IEnumerable<BuildType> GetBuildTypes()
			{
				return _buildTypes;
			}
		}
	}
}