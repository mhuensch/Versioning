using Roslyn.Compilers.Common;
using Roslyn.Services;
using Run00.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Link
{
	public class SolutionLinker
	{
		public SolutionLinker(ISymbolLinkFactory<IAssemblySymbol> assemblyFactory)
		{
			_assemblyFactory = assemblyFactory;
		}

		public IEnumerable<SymbolLink<IAssemblySymbol>> Link(ISolution original, ISolution compareTo)
		{
			var oAssemblies = original.Projects.Select(p => p.GetCompilation().Assembly);
			var cAssemblies = compareTo.Projects.Select(p => p.GetCompilation().Assembly);
			var result = oAssemblies.FullOuterJoin(cAssemblies, (t) => t.Name, (a, b) => (SymbolLink<IAssemblySymbol>)_assemblyFactory.Link(a, b));
			return result;
		}

		private readonly ISymbolLinkFactory<IAssemblySymbol> _assemblyFactory;
	}
}
