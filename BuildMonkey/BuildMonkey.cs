using System.Linq;
using BuildMonkey.SelectionStrategies;
using BuildMonkey.TeamCity;
using log4net;

namespace BuildMonkey
{
	public class BuildMonkey
	{
		private readonly IProjectRepository _projectRepository;
		private readonly ISelectionStrategy<Project> _projectSelectionStrategy;
		private readonly ISelectionStrategy<BuildType> _buildTypeSelectionStrategy;
		private readonly IBuildTypeRunner _buildTypeRunner;
		private readonly ILog _logger;

		public BuildMonkey(IProjectRepository projectRepository, ISelectionStrategy<Project> projectSelectionStrategy, 
			ISelectionStrategy<BuildType> buildTypeSelectionStrategy, IBuildTypeRunner buildTypeRunner, ILog logger)
		{
			_projectRepository = projectRepository;
			_projectSelectionStrategy = projectSelectionStrategy;
			_buildTypeSelectionStrategy = buildTypeSelectionStrategy;
			_buildTypeRunner = buildTypeRunner;
			_logger = logger;
		}

		public void GrabABanana()
		{
			var allProjects = _projectRepository.GetAllProjects();

			if (allProjects.Count() == 0)
			{
				_logger.Warn("No projects found. Abandon ship!");
				return;
			}

			var selectedProject = _projectSelectionStrategy.ChooseFrom(allProjects);
			var selectedBuildType = _buildTypeSelectionStrategy.ChooseFrom(selectedProject.GetBuildTypes());
			_buildTypeRunner.Run(selectedBuildType);
		}
	}
}