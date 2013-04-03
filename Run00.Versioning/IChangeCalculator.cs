using Roslyn.Compilers.Common;
using Run00.Versioning.Compare;

namespace Run00.Versioning
{
	public interface IChangeCalculator<T> where T : ISymbol
	{
		ContractChangeType CalculateChange(ISymbolComparison visitor);
	}
}
