using Roslyn.Compilers.Common;
using System.Collections.Generic;

namespace Run00.Versioning.Link
{
	public class SolutionLinkerResult
	{
		public CommonCompilation OriginalCompilation { get; set; }
		public CommonCompilation ComparedToCompilation { get; set; }
		IEnumerable<SymbolLink<IAssemblySymbol>> AssemblyLinks { get; set; }
	}
}
