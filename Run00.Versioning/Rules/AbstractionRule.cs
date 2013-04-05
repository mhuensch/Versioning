using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class AbstractionRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.OriginalSymbol.IsAbstract != link.ComparedToSymbol.IsAbstract)
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.IsAbstract changed from " + link.OriginalSymbol.IsAbstract + " to " + link.ComparedToSymbol.IsAbstract + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
