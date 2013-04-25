using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ICompilation : IContractItem
	{
		IEnumerable<IAttribute> GetAttributes();
		IEnumerable<ISyntaxTree> SyntaxTrees { get; }
	}
}