using Roslyn.Compilers.Common;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynSyntaxTree : ISyntaxTree
	{
		public RoslynSyntaxTree(CommonSyntaxTree tree)
		{
			_tree = tree;
		}

		string ISyntaxTree.FilePath
		{
			get
			{
				return _tree.FilePath;
			}
		}

		ISyntaxNode ISyntaxTree.GetRoot()
		{
			return new RoslynSyntaxNode(_tree.GetRoot());
		}

		private readonly CommonSyntaxTree _tree;
	}
}
