using Roslyn.Compilers.Common;
using Roslyn.Services;
using Run00.Utilities;
using Run00.Versioning.Compare;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning
{
	public static class SolutionExtensions
	{
		private static IEnumerable<ISymbolComparison> RollUpChildren(IEnumerable<ISymbolComparison> changes, List<ISymbolComparison> results)
		{
			results.AddRange(changes);
			foreach (var change in changes)
			{
				if (change.Children != null && change.Children.Count() > 0)
					RollUpChildren(change.Children, results);
			}
			return results;
		}
	}
}
