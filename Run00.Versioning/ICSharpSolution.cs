using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ICSharpSolution
	{
		IEnumerable<ICompilation> Compilations { get; }
	}
}