using System;
using System.Collections.Generic;

namespace BuildMonkey.TeamCity
{
	public interface IGetBuildTypesCommand {
		IEnumerable<BuildType> Execute(Uri uri);
	}
}