using System;
using System.Linq;
using System.Xml.Linq;
using BuildMonkey.Web;
using log4net;

namespace BuildMonkey.TeamCity
{
	public class BuildTypeRunner : IBuildTypeRunner 
	{
		private readonly IWebRequestMaker _webRequestMaker;
		private readonly string _teamCityBaseUri;
		private readonly ILog _logger;

		public BuildTypeRunner(IWebRequestMaker webRequestMaker, string teamCityBaseUri, ILog logger)
		{
			_webRequestMaker = webRequestMaker;
			_teamCityBaseUri = teamCityBaseUri;
			_logger = logger;
		}

		public void Run(BuildType buildType)
		{
			_logger.InfoFormat("Running {0}", buildType.Name);

			var uri = new Uri(_teamCityBaseUri + "/ajax.html");
			var data = string.Format("add2Queue={0}&_=", buildType.Id);
			var response = _webRequestMaker.Post(uri, data);
			
			var xDocument = XDocument.Parse(response);

			// <response><errors /></response>
			if (xDocument.Element("response").Element("errors").Elements().Count() > 0)
			{
				_logger.ErrorFormat("Failed to run build: {0}", response);
			} 
			else
			{
				_logger.Info("Build queued successfully");
			}
		}
	}
}