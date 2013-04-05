using Roslyn.Compilers.Common;
using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class MethodParameterTypeRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			var original = link.OriginalSymbol as IMethodSymbol;
			var compareTo = link.ComparedToSymbol as IMethodSymbol;

			if (original == null || compareTo == null)
				return null;

			for (var index = 0; index < original.Parameters.Count; index++)
			{
				var oParam = original.Parameters.ElementAt(index);
				var cParam = compareTo.Parameters.ElementAt(index);
				if (oParam.Name != cParam.Name)
					return new SymbolChange(link, SymbolChangeType.Modifying, "A IMethodSymbol.Parameter changed from " + cParam.Name + " to " + cParam.Name + ".");
			}

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return symbol.SymbolType is IMethodSymbol;
		}
	}
}
