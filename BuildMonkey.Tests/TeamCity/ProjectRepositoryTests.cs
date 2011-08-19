using System;
using System.Configuration;
using BuildMonkey.TeamCity;
using BuildMonkey.Web;
using FakeItEasy;
using log4net;
using NUnit.Framework;

namespace BuildMonkey.Tests.TeamCity
{
	public class ProjectRepositoryTests
	{
		[Test]
		public void Get_all_projects()
		{
			var logger = A.Fake<ILog>();
			var teamCityBaseUri = ConfigurationManager.AppSettings["teamCityBaseUri"];
			var teamCityUsername = ConfigurationManager.AppSettings["teamCityUsername"];
			var teamCityPassword = ConfigurationManager.AppSettings["teamCityPassword"];
			var webRequestMaker = new WebRequestMaker(teamCityUsername, teamCityPassword, logger);
			var getBuildTypesCommand = new GetBuildTypesCommand(webRequestMaker, new BuildTypeParser());
			var getAllProjectsCommand = new GetAllProjectsCommand(webRequestMaker, new ProjectParser(teamCityBaseUri, getBuildTypesCommand), teamCityBaseUri);
			var projectRepository = new ProjectRepository(getAllProjectsCommand, logger);

			var allProjects = projectRepository.GetAllProjects();

			Console.WriteLine("All projects:");
			foreach (var project in allProjects)
			{
				Console.WriteLine(project);
				Console.WriteLine("Build types:");
				foreach (var buildType in project.GetBuildTypes())
				{
					Console.WriteLine(buildType);
				}
			}
		}
	}
}