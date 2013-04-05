using Run00.Versioning.Link;
using System.Linq;

namespace Run00.Versioning.Rules
{
	public class ChildCountRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			if (link.OriginalSymbol == null || link.ComparedToSymbol == null)
				return null;

			if (link.Children.Count() != link.Children.Count())
				return new SymbolChange(link, SymbolChangeType.Modifying, "ISymbol.Children count changed from " + link.Children.Count() + " to " + link.Children.Count() + ".");

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return true;
		}
	}
}
