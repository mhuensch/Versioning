using Roslyn.Compilers.Common;
using Run00.Utilities;
using Run00.Versioning.Extensions;

namespace Run00.Versioning.Compare
{
	public class ComparisonFactoryForAssembly : ISymbolComparisonFactory<IAssemblySymbol>
	{
		public ComparisonFactoryForAssembly(ISymbolComparisonFactory<INamedTypeSymbol> typeFactory, SymbolChangeCalculator calculator)
		{
			_typeFactory = typeFactory;
			_calculator = calculator;
		}

		ISymbolComparison ISymbolComparisonFactory<IAssemblySymbol>.Compare(IAssemblySymbol original, IAssemblySymbol compareTo)
		{	
			var children = original.GetContractTypes().FullOuterJoin(compareTo.GetContractTypes(), (t) => t.Name, (a, b) => _typeFactory.Compare(a, b));

			return new SymbolComparison<IAssemblySymbol>(original, compareTo, _calculator.CalculateChange(original, compareTo, children), children);
		}

		private readonly ISymbolComparisonFactory<INamedTypeSymbol> _typeFactory;
		private readonly SymbolChangeCalculator _calculator;
	}
}
