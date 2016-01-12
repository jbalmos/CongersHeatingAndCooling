using System;
using System.Collections.Generic;
using System.Linq;

namespace CHC.Common.Extensions
{
	public static class CollectionExtensions
	{
		public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
		{
			T element;

			for (int i = 0; i < collection.Count; i++)
			{
				element = collection.ElementAt(i);
				if (predicate(element))
				{
					collection.Remove(element);
					i--;
				}
			}
		}
	}
}
