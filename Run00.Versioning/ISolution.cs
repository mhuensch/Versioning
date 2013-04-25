using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ISolution
	{
		IEnumerable<ICompilation> Compilations { get; }
	}
}
