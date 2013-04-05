using System.Collections.Generic;

namespace Run00.Utilities
{
	public static class TreeExtensions
	{
		public static IEnumerable<T> RollUp<T>(this ITree<T> value, List<T> result = null)
			where T : ITree<T>
		{
			if (result == null)
				result = new List<T>();

			if (value == null)
				return result;

			if (value.Children == null)
				return result;

			foreach (var child in value.Children)
			{
				result.Add(child);
				child.RollUp(result);
			}

			return result;
		}
	}
}
