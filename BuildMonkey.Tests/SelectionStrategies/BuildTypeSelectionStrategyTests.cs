using System.Collections.Generic;
using System.Linq;
using BuildMonkey.SelectionStrategies;
using BuildMonkey.TeamCity;
using FakeItEasy;
using log4net;
using NUnit.Framework;

namespace BuildMonkey.Tests.SelectionStrategies
{
	public class BuildTypeSelectionStrategyTests
	{
		[Test]
		public void Do_not_select_builds_with_live_in_the_name()
		{
			var logger = A.Fake<ILog>();
			var selectionStrategy = new BuildTypeSelectionStrategy(new PickFirstItemSelectionStrategy(), logger);
			var buildType = new BuildType { Name = "" };
			var buildTypes = new List<BuildType> { new BuildType { Name = "live"}, new BuildType { Name = "LIVE" }, buildType};

			var selectedBuildType = selectionStrategy.ChooseFrom(buildTypes);

			Assert.That(selectedBuildType, Is.EqualTo(buildType));
		}

		private class PickFirstItemSelectionStrategy : ISelectionStrategy<BuildType> 
		{
			public BuildType ChooseFrom(IEnumerable<BuildType> items)
			{
				return items.ElementAt(0);
			}
		}
	}
}