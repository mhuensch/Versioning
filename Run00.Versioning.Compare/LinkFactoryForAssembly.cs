using Roslyn.Compilers.Common;
using Run00.Utilities;
using Run00.Versioning.Extensions;

namespace Run00.Versioning.Link
{
	public class LinkFactoryForAssembly : ISymbolLinkFactory<IAssemblySymbol>
	{
		public LinkFactoryForAssembly(ISymbolLinkFactory<INamedTypeSymbol> typeFactory)
		{
			_typeFactory = typeFactory;
		}

		ISymbolLink ISymbolLinkFactory<IAssemblySymbol>.Link(IAssemblySymbol original, IAssemblySymbol compareTo)
		{	
			var children = original.GetContractTypes().FullOuterJoin(compareTo.GetContractTypes(), (t) => t.Name, (a, b) => _typeFactory.Link(a, b));
			return new SymbolLink<IAssemblySymbol>(original, compareTo, children);
		}

		private readonly ISymbolLinkFactory<INamedTypeSymbol> _typeFactory;
	}
}
