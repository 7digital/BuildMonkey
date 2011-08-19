using System.Configuration;
using BuildMonkey.TeamCity;
using BuildMonkey.Web;
using FakeItEasy;
using log4net;
using NUnit.Framework;

namespace BuildMonkey.Tests.TeamCity
{
	public class BuildTypeRunnerTests
	{
		[Test]
		public void Run_build()
		{
			var teamCityBaseUri = ConfigurationManager.AppSettings["teamCityBaseUri"];
			var teamCityUsername = ConfigurationManager.AppSettings["teamCityUsername"];
			var teamCityPassword = ConfigurationManager.AppSettings["teamCityPassword"];
			var buildTypeRunner = new BuildTypeRunner(new WebRequestMaker(teamCityUsername, teamCityPassword, A.Fake<ILog>()), teamCityBaseUri, A.Fake<ILog>());
			buildTypeRunner.Run(new BuildType { Id = "bt863" });
		}
	}
}