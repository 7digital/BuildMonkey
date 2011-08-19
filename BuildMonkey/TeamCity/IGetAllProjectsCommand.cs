using System.Collections.Generic;

namespace BuildMonkey.TeamCity
{
	public interface IGetAllProjectsCommand {
		IEnumerable<Project> Execute();
	}
}