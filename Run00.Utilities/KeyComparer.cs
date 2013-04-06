using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Run00.Utilities
{
	/// <summary>
	/// Generic implementation for an equality comparer.
	/// </summary>
	/// <typeparam name="T">Type of the object to compare.</typeparam>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	public class KeyComparer<T, TKey> : IEqualityComparer<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyComparer{T, TKey}"/> class.
		/// </summary>
		/// <param name="keySelector">The key selector used for comparison.</param>
		public KeyComparer(Func<T, TKey> keySelector)
		{
			Contract.Requires(keySelector != null);

			_keySelector = keySelector;
		}

		bool IEqualityComparer<T>.Equals(T x, T y)
		{
			return _keySelector(x).Equals(_keySelector(y));
		}

		int IEqualityComparer<T>.GetHashCode(T obj)
		{
			return _keySelector(obj).GetHashCode();
		}

		private readonly Func<T, TKey> _keySelector;
	}
}