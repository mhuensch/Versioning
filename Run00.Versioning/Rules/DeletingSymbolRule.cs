using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class DeletingSymbolRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.ComparedToSymbol == null)
				return new SymbolChange(link, SymbolChangeType.Deleting, "No match for original symbol found.");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
