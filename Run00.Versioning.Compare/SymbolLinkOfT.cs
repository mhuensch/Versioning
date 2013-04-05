using Roslyn.Compilers.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Run00.Versioning.Link
{
	[DebuggerDisplay("{TypeCompared}")]
	public class SymbolLink<T> : ISymbolLink where T : ISymbol
	{
		public IEnumerable<ISymbolLink> Children { get; private set; }
		public T Original { get; private set; }
		public T ComparedTo { get; private set; }

		public ISymbol OriginalSymbol { get { return Original; } }
		public ISymbol ComparedToSymbol { get { return ComparedTo; } }
		public Type SymbolType { get { return typeof(T); } }
		public string ObjectName { get { return Original != null ? Original.ToDisplayString() : ComparedTo.ToDisplayString(); } }

		public SymbolLink(T original, T comparedTo, IEnumerable<ISymbolLink> children)
		{
			Original = original;
			ComparedTo = comparedTo;
			Children = children;
		}
	}
}
