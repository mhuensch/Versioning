using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ICompilation : IContractItem
	{
		IAssembly Assembly { get; }
		IEnumerable<ISyntaxTree> SyntaxTrees { get; }
		INamespace GlobalNamespace { get; }
	}
}
