using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
			Contract.Ensures(keySelector == _keySelector);

			_keySelector = keySelector;
		}

		bool IEqualityComparer<T>.Equals(T x, T y)
		{
			Contract.Ensures(_keySelector != null);
			return _keySelector(x).Equals(_keySelector(y));
		}

		int IEqualityComparer<T>.GetHashCode(T obj)
		{
			Contract.Ensures(_keySelector != null);
			return _keySelector(obj).GetHashCode();
		}

		[ContractInvariantMethod, ExcludeFromCodeCoverage]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for code contracts.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(_keySelector != null);
		}

		private readonly Func<T, TKey> _keySelector;
	}
}