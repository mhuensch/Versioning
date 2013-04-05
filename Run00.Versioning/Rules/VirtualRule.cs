using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class VirtualRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.OriginalSymbol.IsVirtual != link.ComparedToSymbol.IsVirtual)
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.IsVirtual changed from " + link.OriginalSymbol.IsVirtual + " to " + link.ComparedToSymbol.IsVirtual + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
