using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Utilities
{
	public static class EnumerableOfTExtensions
	{
		/// <summary>
		/// Creates a full outer join between the two enumerables using the key selector to return an enumerable of objects created by the projection.
		/// </summary>
		/// <typeparam name="T">The type of the enumerable lists to be joined</typeparam>
		/// <typeparam name="TReturn">The type of the enumerable to be returned.</typeparam>
		/// <typeparam name="TKey">The type of the key to be matched on.</typeparam>
		/// <param name="right">The right side of the join.</param>
		/// <param name="left">The left side of the join.</param>
		/// <param name="keySelector">The key selector.</param>
		/// <param name="projection">The projection.</param>
		/// <returns>Enumerable of objects created by the join.</returns>
		public static IEnumerable<TReturn> FullOuterJoin<T, TReturn, TKey>(this IEnumerable<T> right, IEnumerable<T> left, Func<T, TKey> keySelector, Func<T, T, TReturn> projection)
		{
			Contract.Requires(right != null);
			Contract.Requires(left != null);
			Contract.Requires(keySelector != null);
			Contract.Requires(projection != null);
			Contract.Ensures(Contract.Result<IEnumerable<TReturn>>() != null);

			var comparer = new KeyComparer<T, TKey>(keySelector);

			var alookup = right.ToLookup(g => g, comparer);
			var blookup = left.ToLookup(g => g, comparer);

			var keys = new HashSet<T>(right, comparer);
			keys.UnionWith(left);

			var join =
				from key in keys
				from xa in alookup[key].DefaultIfEmpty()
				from xb in blookup[key].DefaultIfEmpty()
				select projection(xa, xb);

			return join;
		}

		/// <summary>
		/// Creates a full outer join between the two enumerables using the key selector to return an enumerable of objects created by the projection.
		/// </summary>
		/// <typeparam name="T">The type of the enumerable lists to be joined</typeparam>
		/// <typeparam name="TReturn">The type of the enumerable to be returned.</typeparam>
		/// <typeparam name="TKey">The type of the key to be matched on.</typeparam>
		/// <param name="right">The right side of the join.</param>
		/// <param name="left">The left side of the join.</param>
		/// <param name="comparison">The function to use for joining the two lists.</param>
		/// <param name="projection">The projection.</param>
		/// <returns>Enumerable of objects created by the join.</returns>
		public static IEnumerable<TResult> FullOuterJoin<T, TResult>(this IEnumerable<T> right, IEnumerable<T> left, Func<T, T, bool> comparison, Func<T, T, TResult> projection)
		{
			Contract.Requires(right != null);
			Contract.Requires(left != null);
			Contract.Requires(comparison != null);
			Contract.Requires(projection != null);
			Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

			var bCopy = left.ToList();

			var allAsMatchingBs =
				from aItem in right
				let bMatch = bCopy.SingleOrDefault(bj => comparison(aItem, bj))
				let unused = bCopy.Remove(bMatch)
				select projection(aItem, bMatch);

			return allAsMatchingBs.ToList().Union(bCopy.Select(bItem => projection(default(T), bItem)));
		}
	}
}
