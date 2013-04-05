using Roslyn.Compilers.Common;
using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class SymbolKindRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.OriginalSymbol.DeclaredAccessibility != link.ComparedToSymbol.DeclaredAccessibility)
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.DeclaredAccessibility changed from " + link.OriginalSymbol.DeclaredAccessibility + " to " + link.ComparedToSymbol.DeclaredAccessibility + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
