using System;
using System.Collections.Generic;
using BuildMonkey.Web;

namespace BuildMonkey.TeamCity
{
	public class GetBuildTypesCommand : IGetBuildTypesCommand
	{
		private readonly IWebRequestMaker _webRequestMaker;
		private readonly IXmlParser<BuildType> _parser;

		public GetBuildTypesCommand(IWebRequestMaker webRequestMaker, IXmlParser<BuildType> parser)
		{
			_webRequestMaker = webRequestMaker;
			_parser = parser;
		}

		public IEnumerable<BuildType> Execute(Uri uri)
		{
			var responseBody = _webRequestMaker.Get(uri);
			return _parser.Parse(responseBody);
		}
	}
}