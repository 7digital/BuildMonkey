using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace BuildMonkey.SelectionStrategies
{
	public class RandomSelectionStrategy<T> : ISelectionStrategy<T>
	{
		private readonly ILog _logger;
		private readonly Random _random;

		public RandomSelectionStrategy(ILog logger)
		{
			_logger = logger;
			_random = new Random();
		}

		public T ChooseFrom(IEnumerable<T> items)
		{
			var count = items.Count();
			_logger.InfoFormat("Selecting {0} at random, from {1} items", typeof(T).Name, count);

			var randomItem = _random.Next(0, count - 1);
			var item = items.ElementAt(randomItem);
			_logger.InfoFormat("Selected item {0} ({1})", randomItem, item);

			return item;
		}
	}
}