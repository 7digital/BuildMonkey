using System;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace BuildMonkey.Web
{
	public class WebRequestMaker : IWebRequestMaker
	{
		private readonly string _username;
		private readonly string _password;
		private readonly ILog _logger;

		public WebRequestMaker(string username, string password, ILog logger)
		{
			_username = username;
			_password = password;
			_logger = logger;
		}

		public string Get(Uri uri)
		{
			_logger.DebugFormat("GET: {0}", uri);
			
			var webRequest = BuildWebRequest(uri);
			SetCredentials(webRequest);
			
			return GetResponseBody(webRequest);
		}

		public string Post(Uri uri, string data)
		{
			_logger.DebugFormat("POST: {0}", uri);
			_logger.DebugFormat("Data: {0}", data);

			var webRequest = BuildWebRequest(uri);
			SetCredentials(webRequest);
			AddPostData(webRequest, data);
			return GetResponseBody(webRequest);
		}

		private void SetCredentials(WebRequest webRequest)
		{
			webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(_username + ":" + _password));
		}

		private static void AddPostData(WebRequest webRequest, string data)
		{
			webRequest.Method = "POST";
			var bytes = Encoding.UTF8.GetBytes(data);
			webRequest.ContentLength = bytes.Length;
			webRequest.ContentType = "application/x-www-form-urlencoded";
			using (var requestStream = webRequest.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
			}
		}

		private static WebRequest BuildWebRequest(Uri uri) 
		{
			return WebRequest.Create(uri);
		}

		private string GetResponseBody(WebRequest webRequest)
		{
			using (var webResponse = webRequest.GetResponse())
			{
				using (var responseStream = webResponse.GetResponseStream())
				{
					var responseBody = new StreamReader(responseStream).ReadToEnd();
					_logger.DebugFormat("Response:\n{0}", responseBody);
					return responseBody;
				}
			}
		}
	}
}