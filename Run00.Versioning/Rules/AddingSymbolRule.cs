using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class AddingSymbolRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null)
				return new SymbolChange(link, SymbolChangeType.Adding, "No match for comparison symbol found.");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
