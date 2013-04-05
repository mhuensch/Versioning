using Roslyn.Compilers.Common;
using Run00.Utilities;
using Run00.Versioning.Extensions;
using System.Linq;

namespace Run00.Versioning.Link
{
	public class LinkFactoryForType : ISymbolLinkFactory<INamedTypeSymbol>
	{
		public LinkFactoryForType(ISymbolLinkFactory<IMethodSymbol> methodFactory, ISymbolLinkFactory<IPropertySymbol> propertyFactory)
		{
			_methodFactory = methodFactory;
			_propertyFactory = propertyFactory;
		}

		ISymbolLink ISymbolLinkFactory<INamedTypeSymbol>.Link(INamedTypeSymbol original, INamedTypeSymbol compareTo)
		{
			var methods = original.GetContractMethods().FullOuterJoin(compareTo.GetContractMethods(), (t) => t.Name, (a, b) => _methodFactory.Link(a, b));
			var properties = original.GetContractProperties().FullOuterJoin(compareTo.GetContractProperties(), (t) => t.Name, (a, b) => _propertyFactory.Link(a, b));
			var children = methods.Union(properties);

			return new SymbolLink<INamedTypeSymbol>(original, compareTo, children);
		}

		private readonly ISymbolLinkFactory<IMethodSymbol> _methodFactory;
		private readonly ISymbolLinkFactory<IPropertySymbol> _propertyFactory;
	}
}
