using Roslyn.Compilers.Common;
using System.Linq;

namespace Run00.Versioning.Link
{
	public class LinkFactoryForMethod : ISymbolLinkFactory<IMethodSymbol>
	{
		ISymbolLink ISymbolLinkFactory<IMethodSymbol>.Link(IMethodSymbol original, IMethodSymbol compareTo)
		{
			var children = Enumerable.Empty<ISymbolLink>();
			return new SymbolLink<IMethodSymbol>(original, compareTo, children);
		}
	}
}
