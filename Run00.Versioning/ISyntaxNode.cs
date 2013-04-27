
using System.Collections.Generic;
namespace Run00.Versioning
{
	public interface ISyntaxNode
	{
		string ToFullString();
		IEnumerable<ISyntaxNode> ChildNodes();
		bool IsEquivalentTo(ISyntaxNode node);
		string GetIdentifierName();
	}
}
