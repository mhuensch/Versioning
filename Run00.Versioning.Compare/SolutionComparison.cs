using Roslyn.Compilers.Common;
using Roslyn.Services;
using Run00.Utilities;
using Run00.Versioning.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Compare
{
	public class SolutionComparison
	{
		public SolutionComparison(ISymbolComparisonFactory<IAssemblySymbol> assemblyFactory)
		{
			_assemblyFactory = assemblyFactory;
		}

		public IEnumerable<ISymbolComparison> Compare(ISolution original, ISolution compareTo)
		{
			var oAssemblies = original.Projects.Select(p => p.GetCompilation().Assembly);
			var cAssemblies = compareTo.Projects.Select(p => p.GetCompilation().Assembly);
			var result = oAssemblies.FullOuterJoin(cAssemblies, (t) => t.Name, (a, b) => _assemblyFactory.Compare(a, b));
			return result;
		}

		private readonly ISymbolComparisonFactory<IAssemblySymbol> _assemblyFactory;
	}
}
