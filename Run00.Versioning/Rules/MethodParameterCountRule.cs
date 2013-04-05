using Roslyn.Compilers.Common;
using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class MethodParameterCountRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			var original = link.OriginalSymbol as IMethodSymbol;
			var compareTo = link.ComparedToSymbol as IMethodSymbol;

			if (original == null || compareTo == null)
				return null;

			if (original.Parameters.Count != compareTo.Parameters.Count)
				return new SymbolChange(link, SymbolChangeType.Modifying, "IMethodSymbol.Parameters.Count changed from " + original.Parameters.Count + " to " + compareTo.Parameters.Count + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return symbol.SymbolType is IMethodSymbol;
		}
	}
}
