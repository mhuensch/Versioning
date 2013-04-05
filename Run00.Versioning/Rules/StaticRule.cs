using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class StaticRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.OriginalSymbol.IsStatic != link.ComparedToSymbol.IsStatic)
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.IsStatic changed from " + link.OriginalSymbol.IsStatic + " to " + link.ComparedToSymbol.IsStatic + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
