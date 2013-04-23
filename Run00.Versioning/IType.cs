using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface IType
	{
		string Name { get; }
		bool IsContractType { get; }
		IEnumerable<ISyntaxNode> SyntaxNodes { get; }
	}
}
