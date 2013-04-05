using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class AccessibilityRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.OriginalSymbol.Kind != link.ComparedToSymbol.Kind)
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.Kind changed from " + link.OriginalSymbol.Kind + " to " + link.ComparedToSymbol.Kind + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
