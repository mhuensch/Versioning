using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ISyntaxTree
	{
		string FilePath { get; }
		ISyntaxNode GetRoot();
		bool IsEquivalentTo(ISyntaxTree tree);
		bool IsEquivalentTo(ISyntaxTree tree, bool topLevel);
		bool HasChanges(ISyntaxTree compareTo);
		string GetIdentifierName();
	}
}
