using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Compare
{
	public class ComparisonFactoryForMethod : ISymbolComparisonFactory<IMethodSymbol>
	{
		public ComparisonFactoryForMethod(SymbolChangeCalculator calculator)
		{
			_calculator = calculator;
		}

		ISymbolComparison ISymbolComparisonFactory<IMethodSymbol>.Compare(IMethodSymbol original, IMethodSymbol compareTo)
		{
			var children = Enumerable.Empty<ISymbolComparison>();
			var change = CalculateChange(original, compareTo, children);
			return new SymbolComparison<IMethodSymbol>(original, compareTo, change, children);
		}

		private ContractChange CalculateChange(IMethodSymbol original, IMethodSymbol compareTo, IEnumerable<ISymbolComparison> children)
		{
			var change = _calculator.CalculateChange(original, compareTo, children);
			if (change.ChangeType != ContractChangeType.None)
				return change;

			if (original.ReturnType.Name != compareTo.ReturnType.Name)
				return new ContractChange(ContractChangeType.Modifying, "IMethodSymbol.ReturnType changed from " + original.ReturnType.Name + " to " + compareTo.ReturnType.Name + ".");

			if (original.Parameters.Count != compareTo.Parameters.Count)
				return new ContractChange(ContractChangeType.Modifying, "IMethodSymbol.Parameters.Count changed from " + original.Parameters.Count + " to " + compareTo.Parameters.Count + ".");

			for (var index = 0; index < original.Parameters.Count; index++)
			{
				var oParam = original.Parameters.ElementAt(index);
				var cParam = compareTo.Parameters.ElementAt(index);
				if (oParam.Name != cParam.Name)
					return new ContractChange(ContractChangeType.Modifying, "A IMethodSymbol.Parameter changed from " + cParam.Name + " to " + cParam.Name + ".");
			}

			return new ContractChange(ContractChangeType.None, "Method signatures match.");
		}

		private readonly SymbolChangeCalculator _calculator;
	}
}
