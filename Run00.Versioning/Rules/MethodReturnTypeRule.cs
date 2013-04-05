using Roslyn.Compilers.Common;
using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class MethodReturnTypeRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			var original = link.OriginalSymbol as IMethodSymbol;
			var compareTo = link.ComparedToSymbol as IMethodSymbol;

			if (original == null || compareTo == null)
				return null;

			if (original.ReturnType.Name != compareTo.ReturnType.Name)
				return new SymbolChange(link, SymbolChangeType.Modifying, "IMethodSymbol.ReturnType changed from " + original.ReturnType.Name + " to " + compareTo.ReturnType.Name + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return symbol.SymbolType is IMethodSymbol;
		}
	}
}
