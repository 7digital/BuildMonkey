using System.Collections.Generic;

namespace BuildMonkey.SelectionStrategies
{
	public interface ISelectionStrategy<T>
	{
		T ChooseFrom(IEnumerable<T> items);
	}
}