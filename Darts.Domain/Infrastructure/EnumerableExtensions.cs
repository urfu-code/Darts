using System.Collections.Generic;
using System.Linq;

namespace Darts.Infrastructure
{
	public static class EnumerableExtensions
	{
		public static int IndexOf<T>(this IReadOnlyList<T> readOnlyList, T value)
		{
			var count = readOnlyList.Count;
			var equalityComparer = EqualityComparer<T>.Default;
			for (var i = 0; i < count; i++)
			{
				var current = readOnlyList[i];
				if (equalityComparer.Equals(current, value)) { return i; }
			}
			return -1;
		}

		public static int ElementwiseHashcode<T>(this IEnumerable<T> items)
		{
			unchecked
			{
				return items.Select(t => t.GetHashCode()).Aggregate((res, next) => (res * 379) ^ next);
			}
		}
	}
}