using Run00.Utilities;
using System.Collections.Generic;

namespace Run00.Versioning.Compare
{
	public interface ISymbolComparison : ITree<ISymbolComparison>
	{
		ContractChange ContractChange { get; }
	}
}
