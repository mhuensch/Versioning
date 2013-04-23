using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ICompilation
	{
		IAssembly Assembly { get; }
		IEnumerable<ISyntaxTree> SyntaxTrees { get; }
		//INamespace Namespace { get; }
	}
}
