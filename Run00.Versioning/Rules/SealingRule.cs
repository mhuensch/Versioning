using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class SealingRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.OriginalSymbol.IsSealed != link.ComparedToSymbol.IsSealed)
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.IsSealed changed from " + link.OriginalSymbol.IsSealed + " to " + link.ComparedToSymbol.IsSealed + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
