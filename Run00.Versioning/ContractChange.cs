using Roslyn.Compilers.Common;
using Run00.Versioning.Link;
using System.Collections.Generic;

namespace Run00.Versioning
{
	public class ContractChange
	{
		public SymbolLink<IAssemblySymbol> LinkedAssemblies { get; private set; }
		public IEnumerable<SymbolChange> Changes { get; private set; }
		public ContractChangeType ChangeType { get; private set; }

		public ContractChange(SymbolLink<IAssemblySymbol> linkedAssemblies, IEnumerable<SymbolChange> changes, ContractChangeType changeType)
		{
			LinkedAssemblies = linkedAssemblies;
			Changes = changes;
			ChangeType = changeType;
		}
	}
}
