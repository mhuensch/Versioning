using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Run00.Versioning.Compare
{
	[DebuggerDisplay("{DisplayString}")]
	public class SymbolComparison<T> : ISymbolComparison where T : ISymbol
	{
		public IEnumerable<ISymbolComparison> Children { get; private set; }
		public T Original { get; private set; }
		public T ComparedTo { get; private set; }
		public ContractChange ContractChange { get; private set; }

		public string DisplayString
		{
			get
			{
				var typeName = Original != null ? Original.ToDisplayString() : ComparedTo.ToDisplayString();
				return typeName + " - " + ContractChange.ChangeType + ": " + ContractChange.Reason;
			}
		}

		public SymbolComparison(T original, T comparedTo, ContractChange contractChange, IEnumerable<ISymbolComparison> children)
		{
			Original = original;
			ComparedTo = comparedTo;
			ContractChange = contractChange;
			Children = children;
		}
	}
}
