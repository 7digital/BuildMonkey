using System;
using System.Configuration;
using System.Timers;
using BuildMonkey.SelectionStrategies;
using BuildMonkey.TeamCity;
using BuildMonkey.Web;
using log4net;
using log4net.Config;

namespace BuildMonkey
{
	internal class Service
	{
		private Timer _timer;
		private readonly BuildMonkey _buildMonkey;
		private readonly ILog _logger;

		public Service()
		{
			var teamCityBaseUri = ConfigurationManager.AppSettings["teamCityBaseUri"];
			var teamCityUsername = ConfigurationManager.AppSettings["teamCityUsername"];
			var teamCityPassword = ConfigurationManager.AppSettings["teamCityPassword"];
			var webRequestMaker = new WebRequestMaker(teamCityUsername, teamCityPassword, GetLoggerFor<WebRequestMaker>());
			var getBuildTypesCommand = new GetBuildTypesCommand(webRequestMaker, new BuildTypeParser());
			var getAllProjectsCommand = new GetAllProjectsCommand(webRequestMaker, new ProjectParser(teamCityBaseUri, getBuildTypesCommand), teamCityBaseUri);
			var projectRepository = new ProjectRepository(getAllProjectsCommand, GetLoggerFor<ProjectRepository>());
			var projectSelectionStrategy = new RandomSelectionStrategy<Project>(GetLoggerFor<RandomSelectionStrategy<Project>>());
			var buildTypeSelectionStrategy = new BuildTypeSelectionStrategy(new RandomSelectionStrategy<BuildType>(GetLoggerFor<RandomSelectionStrategy<BuildType>>()), GetLoggerFor<BuildTypeSelectionStrategy>());
			var buildTypeRunner = new BuildTypeRunner(webRequestMaker, teamCityBaseUri, GetLoggerFor<BuildTypeRunner>());
			_buildMonkey = new BuildMonkey(projectRepository, projectSelectionStrategy, buildTypeSelectionStrategy, buildTypeRunner, GetLoggerFor<BuildMonkey>());
			_logger = GetLoggerFor<Service>();
		}

		private static ILog GetLoggerFor<T>()
		{
			return LogManager.GetLogger(typeof(T));
		}

		public void Start()
		{
			XmlConfigurator.Configure();

			_timer = new Timer
			{
				AutoReset = false,
				//Interval = TimeSpan.FromMinutes(1).TotalMilliseconds,
				Interval = TimeSpan.FromSeconds(5).TotalMilliseconds,
			};
			_timer.Elapsed += (s, e) => GrabABanana();
			_timer.Start();
		}

		private void GrabABanana()
		{
			try
			{
				_buildMonkey.GrabABanana();
				_timer.Start();
			}
			catch (Exception e)
			{
				_logger.Error("Unhandled exception", e);
			}
		}

		public void Stop()
		{
			_timer.Stop();
		}
	}
}