using System.Collections.Generic;

namespace BuildMonkey.TeamCity
{
	public interface IProjectRepository {
		IEnumerable<Project> GetAllProjects();
	}
}