using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Run00.Utilities
{
	public class KeyComparer<T, TKey> : IEqualityComparer<T>
	{
		public KeyComparer(Func<T, TKey> keySelector)
		{
			Contract.Requires(keySelector != null);

			_keySelector = keySelector;
		}

		public bool Equals(T x, T y)
		{
			return _keySelector(x).Equals(_keySelector(y));
		}

		public int GetHashCode(T obj)
		{
			return _keySelector(obj).GetHashCode();
		}

		private readonly Func<T, TKey> _keySelector;
	}
}
