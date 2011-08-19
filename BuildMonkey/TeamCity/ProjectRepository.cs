using System.Collections.Generic;
using System.Linq;
using log4net;

namespace BuildMonkey.TeamCity
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly ILog _logger;
		private readonly IGetAllProjectsCommand _getAllProjectsCommand;

		public ProjectRepository(IGetAllProjectsCommand getAllProjectsCommand, ILog logger)
		{
			_logger = logger;
			_getAllProjectsCommand = getAllProjectsCommand;
		}

		public IEnumerable<Project> GetAllProjects()
		{
			_logger.Info("Getting list of all projects");

			var allProjects = _getAllProjectsCommand.Execute().ToList();
			_logger.InfoFormat("Found {0} projects", allProjects.Count);

			return allProjects;
		}
	}
}
