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

		bool ISyntaxTree.IsEquivalentTo(ISyntaxTree tree)
		{
			return ((ISyntaxTree)this).IsEquivalentTo(tree, false);
		}

		bool ISyntaxTree.IsEquivalentTo(ISyntaxTree tree, bool topLevel)
		{
			var privateTree = tree as RoslynSyntaxTree;
			if (privateTree == null)
				return false;

			return _tree.IsEquivalentTo(privateTree._tree, topLevel);
		}

		bool ISyntaxTree.HasChanges(ISyntaxTree compareTo)
		{
			var privateTree = compareTo as RoslynSyntaxTree;
			if (privateTree == null)
				return false;

			return _tree.GetChanges(privateTree._tree).Count() != 0;
		}

		string ISyntaxTree.GetIdentifierName()
		{
			return GetFirstNodeName(((ISyntaxTree)this).GetRoot());
		}

		private string GetFirstNodeName(ISyntaxNode node)
		{
			var result = node.GetIdentifierName();
			if (result != null)
				return result;

			foreach (var child in node.ChildNodes())
			{
				result = GetFirstNodeName(child);
				if (result != null)
					return result;
			}

			return null;
		}

		private readonly CommonSyntaxTree _tree;
	}
}
