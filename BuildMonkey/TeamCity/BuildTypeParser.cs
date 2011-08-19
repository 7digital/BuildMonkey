using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BuildMonkey.TeamCity
{
	public class BuildTypeParser : IXmlParser<BuildType>
	{
		public IEnumerable<BuildType> Parse(string xml)
		{
			var xDocument = XDocument.Parse(xml);

			return xDocument.Element("project").Element("buildTypes").Elements("buildType")
				.Select(projectElement => new BuildType
				{
					Id = projectElement.Attribute("id").Value,
					Name = projectElement.Attribute("name").Value,
				});
		}
	}
}