using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Link
{
	public class LinkFactoryForProperty : ISymbolLinkFactory<IPropertySymbol>
	{
		ISymbolLink ISymbolLinkFactory<IPropertySymbol>.Link(IPropertySymbol original, IPropertySymbol compareTo)
		{
			var children = Enumerable.Empty<ISymbolLink>();
			return new SymbolLink<IPropertySymbol>(original, compareTo, children);
		}
	}
}
