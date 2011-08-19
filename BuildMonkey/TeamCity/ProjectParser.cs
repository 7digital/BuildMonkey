using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BuildMonkey.TeamCity
{
	public class ProjectParser : IXmlParser<Project>
	{
		private readonly string _teamCityBaseUri;
		private readonly IGetBuildTypesCommand _getBuildTypesCommand;

		public ProjectParser(string teamCityBaseUri, IGetBuildTypesCommand getBuildTypesCommand)
		{
			_teamCityBaseUri = teamCityBaseUri;
			_getBuildTypesCommand = getBuildTypesCommand;
		}

		public IEnumerable<Project> Parse(string xml)
		{
			var xDocument = XDocument.Parse(xml);

			return xDocument.Element("projects").Elements("project")
				.Select(projectElement => new Project(_getBuildTypesCommand)
				{
					Id = projectElement.Attribute("id").Value, 
					Name = projectElement.Attribute("name").Value,
					Link = new Uri(_teamCityBaseUri + projectElement.Attribute("href").Value),
				});
		}
	}
}