using Roslyn.Compilers.Common;
using Run00.Versioning.Link;

namespace Run00.Versioning
{
	public interface ISymbolChangeRule
	{
		bool IsValidFor(ISymbolLink symbol);
		SymbolChange GetChange(ISymbolLink link);
	}

	public abstract class BaseSymbolChangeRule<T> : ISymbolChangeRule where T : ISymbol
	{
		public bool IsValidFor(ISymbolLink symbol)
		{
			return symbol is SymbolLink<T>;
		}

		public SymbolChange GetChange(ISymbolLink link)
		{
			return GetChange((SymbolLink<T>)link);
		}

		public abstract SymbolChange GetChange(SymbolLink<T> link);
	}
}
