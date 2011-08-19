using System;

namespace BuildMonkey.Web
{
	public interface IWebRequestMaker
	{
		string Get(Uri uri);
		string Post(Uri uri, string data);
	}
}