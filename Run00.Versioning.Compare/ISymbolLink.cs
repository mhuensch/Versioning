using Roslyn.Compilers.Common;
using Run00.Utilities;
using System;

namespace Run00.Versioning.Link
{
	public interface ISymbolLink : ITree<ISymbolLink>
	{
		ISymbol OriginalSymbol { get; }
		ISymbol ComparedToSymbol { get; }
		Type SymbolType { get; }
		string ObjectName { get; }
	}
}
