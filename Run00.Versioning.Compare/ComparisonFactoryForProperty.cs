using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Compare
{
	public class ComparisonFactoryForProperty : ISymbolComparisonFactory<IPropertySymbol>
	{
		public ComparisonFactoryForProperty(SymbolChangeCalculator calculator)
		{
			_calculator = calculator;
		}

		ISymbolComparison ISymbolComparisonFactory<IPropertySymbol>.Compare(IPropertySymbol original, IPropertySymbol compareTo)
		{
			var children = Enumerable.Empty<ISymbolComparison>();
			var change = CalculateChange(original, compareTo, children);
			return new SymbolComparison<IPropertySymbol>(original, compareTo, change, children);
		}

		private ContractChange CalculateChange(IPropertySymbol original, IPropertySymbol compareTo, IEnumerable<ISymbolComparison> children)
		{
			var change = _calculator.CalculateChange(original, compareTo, children);
			if (change.ChangeType != ContractChangeType.None)
				return change;

			if (original.GetMethod.ReturnType.Name != compareTo.GetMethod.ReturnType.Name)
				return new ContractChange(ContractChangeType.Modifying, "IPropertySymbol.ReturnType changed from " + original.GetMethod.ReturnType.Name + " to " + compareTo.GetMethod.ReturnType.Name + ".");

			if (original.SetMethod.Parameters.Count != compareTo.SetMethod.Parameters.Count)
				return new ContractChange(ContractChangeType.Modifying, "IPropertySymbol.Parameters.Count changed from " + original.SetMethod.Parameters.Count + " to " + compareTo.SetMethod.Parameters.Count + ".");

			for (var index = 0; index < original.SetMethod.Parameters.Count; index++)
			{
				var oParam = original.SetMethod.Parameters.ElementAt(index);
				var cParam = compareTo.SetMethod.Parameters.ElementAt(index);
				if (oParam.Name != cParam.Name)
					return new ContractChange(ContractChangeType.Modifying, "A IPropertySymbol.Parameter changed from " + cParam.Name + " to " + cParam.Name + ".");
			}

			return new ContractChange(ContractChangeType.None, "Method signatures match.");
		}

		private readonly SymbolChangeCalculator _calculator;
	}
}
