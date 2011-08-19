using System.Collections.Generic;

namespace BuildMonkey.TeamCity
{
	public interface IXmlParser<T>
	{
		IEnumerable<T> Parse(string xml);
	}
}