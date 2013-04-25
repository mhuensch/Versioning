
using System.Collections.Generic;
namespace Run00.Versioning
{
	public interface ISyntaxNode
	{
		string ToFullString();
		IEnumerable<ISyntaxNode> ChildNodes();
		bool IsEquivalentTo(ISyntaxNode node);
		bool IsEquivalentTo(ISyntaxNode node, bool topLevel);
		string GetIdentifierName();
	}
}
