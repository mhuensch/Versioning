using System;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Utilities
{
	public static class EnumerableOfTExtensions
	{
		public static IEnumerable<TR> FullOuterJoin<T, TR, TK>(this IEnumerable<T> a, IEnumerable<T> b, Func<T, TK> keySelector, Func<T, T, TR> projection)
		{
			var comparer = new KeyComparer<T, TK>(keySelector);

			var alookup = a.ToLookup(g => g, comparer);
			var blookup = b.ToLookup(g => g, comparer);

			var keys = new HashSet<T>(a, comparer);
			keys.UnionWith(b);

			var join =
				from key in keys
				from xa in alookup[key].DefaultIfEmpty()
				from xb in blookup[key].DefaultIfEmpty()
				select projection(xa, xb);

			return join;
		}
	}
}
