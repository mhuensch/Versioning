using System.Collections.Generic;

namespace Run00.Utilities
{
	public interface ITree<T> where T : ITree<T>
	{
		IEnumerable<T> Children { get; }
	}
}
