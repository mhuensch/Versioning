using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface ISyntaxTree
	{
		string FilePath { get; }
		ISyntaxNode GetRoot();
	}
}
