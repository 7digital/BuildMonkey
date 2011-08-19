using System;
using System.Collections.Generic;
using BuildMonkey.Web;

namespace BuildMonkey.TeamCity
{
	public class GetAllProjectsCommand : IGetAllProjectsCommand
	{
		private readonly IWebRequestMaker _webRequestMaker;
		private readonly IXmlParser<Project> _parser;
		private readonly Uri _allProjectsUri;

		public GetAllProjectsCommand(IWebRequestMaker webRequestMaker, IXmlParser<Project> parser, string teamCityBaseUri)
		{
			_webRequestMaker = webRequestMaker;
			_parser = parser;
			_allProjectsUri = new Uri(teamCityBaseUri + "/httpAuth/app/rest/projects");
		}

		public IEnumerable<Project> Execute()
		{
			var responseBody = _webRequestMaker.Get(_allProjectsUri);
			return _parser.Parse(responseBody);
		}
	}
}