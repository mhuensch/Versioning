using Roslyn.Compilers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning.Link
{
	public interface ISymbolLinkFactory<T> where T : ISymbol
	{
		ISymbolLink Link(T original, T compareTo);
	}
}
