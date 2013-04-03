using Roslyn.Compilers.Common;
using Run00.Utilities;
using Run00.Versioning.Extensions;
using System.Linq;

namespace Run00.Versioning.Compare
{
	public class ComparisonFactoryForType : ISymbolComparisonFactory<INamedTypeSymbol>
	{
		public ComparisonFactoryForType(ISymbolComparisonFactory<IMethodSymbol> methodFactory, ISymbolComparisonFactory<IPropertySymbol> propertyFactory, SymbolChangeCalculator calculator)
		{
			_methodFactory = methodFactory;
			_propertyFactory = propertyFactory;
			_calculator = calculator;
		}

		ISymbolComparison ISymbolComparisonFactory<INamedTypeSymbol>.Compare(INamedTypeSymbol original, INamedTypeSymbol compareTo)
		{
			var methods = original.GetContractMethods().FullOuterJoin(compareTo.GetContractMethods(), (t) => t.Name, (a, b) => _methodFactory.Compare(a, b));
			var properties = original.GetContractProperties().FullOuterJoin(compareTo.GetContractProperties(), (t) => t.Name, (a, b) => _propertyFactory.Compare(a, b));
			var children = methods.Union(properties);

			return new SymbolComparison<INamedTypeSymbol>(original, compareTo, _calculator.CalculateChange(original, compareTo, children), children);
		}

		private readonly ISymbolComparisonFactory<IMethodSymbol> _methodFactory;
		private readonly ISymbolComparisonFactory<IPropertySymbol> _propertyFactory;
		private readonly SymbolChangeCalculator _calculator;
	}
}
