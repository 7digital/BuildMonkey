using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildMonkey.TeamCity
{
	public class Project
	{
		private readonly IGetBuildTypesCommand _getBuildTypesCommand;

		public string Id { get; set; }
		public string Name { get; set; }
		public Uri Link { get; set; }

		protected Project() { }

		public Project(IGetBuildTypesCommand getBuildTypesCommand)
		{
			_getBuildTypesCommand = getBuildTypesCommand;
		}

		public virtual IEnumerable<BuildType> GetBuildTypes()
		{
			return _getBuildTypesCommand.Execute(Link).ToList();
		}

		public override string ToString()
		{
			return string.Format("Id: {0}, Name: {1}, Link: {2}", Id, Name, Link);
		}
	}
}