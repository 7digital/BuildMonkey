using System.Collections.Generic;
using System.Linq;
using BuildMonkey.TeamCity;
using log4net;

namespace BuildMonkey.SelectionStrategies
{
	public class BuildTypeSelectionStrategy : ISelectionStrategy<BuildType>
	{
		private readonly ISelectionStrategy<BuildType> _selectionStrategy;
		private readonly ILog _logger;

		public BuildTypeSelectionStrategy(ISelectionStrategy<BuildType> selectionStrategy, ILog logger)
		{
			_selectionStrategy = selectionStrategy;
			_logger = logger;
		}

		public BuildType ChooseFrom(IEnumerable<BuildType> items)
		{
			_logger.Info("Removing \"Live\" builds");
			var validBuildTypes = items.Where(bt => !bt.Name.ToLower().Contains("live"));
			_logger.InfoFormat("Excluded {0} builds", items.Count() - validBuildTypes.Count());

			return _selectionStrategy.ChooseFrom(validBuildTypes);
		}
	}
}