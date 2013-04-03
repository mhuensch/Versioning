using Roslyn.Compilers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning.Compare
{
	public interface ISymbolComparisonFactory<T> where T : ISymbol
	{
		ISymbolComparison Compare(T original, T compareTo);
	}
}
