using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Run00.Utilities;
using Run00.Versioning.Link;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning
{
	public class SolutionChangeCalculator
	{
		public IEnumerable<CommonCompilationChange> GetChanges(ISolution original, ISolution compareTo)
		{
			var changes = new List<CommonCompilationChange>();

			var oAssemblies = original.Projects.Select(p => p.GetCompilation());
			var cAssemblies = compareTo.Projects.Select(p => p.GetCompilation());
			return oAssemblies.FullOuterJoin(cAssemblies, (t) => t.Assembly.Name, (o, c) => GetCompilationChange(o, c));
		}

		private CommonCompilationChange GetCompilationChange(CommonCompilation original, CommonCompilation compareTo)
		{
			if (original == null)
				return new CommonCompilationChange(original, compareTo, ContractChangeType.Enhancement);

			if (compareTo == null)
				return new CommonCompilationChange(original, compareTo, ContractChangeType.Breaking);

			var treeChanges = original.SyntaxTrees.FullOuterJoin(compareTo.SyntaxTrees, (a, b) => a.IsEquivalentTo(b, true), (o, c) => GetTreeChange(o, c));
			var maxChange = treeChanges.Max(t => t.ChangeType);
			return new CommonCompilationChange(original, compareTo, treeChanges, maxChange);
		}

		private CommonSyntaxTreeChange GetTreeChange(CommonSyntaxTree original, CommonSyntaxTree compareTo)
		{
			if (original == null)
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.Enhancement);

			if (compareTo == null)
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.Breaking);

			var textChanges = original.GetChanges(compareTo);
			if (textChanges.Count == 0)
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.None);

			if (original.IsEquivalentTo(compareTo))
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.Cosmetic);

			var nodeChange = GetNodeChange(original.GetRoot(), compareTo.GetRoot());
			return new CommonSyntaxTreeChange(original, compareTo, nodeChange, nodeChange.ChangeType);
		}

		private CommonSyntaxNodeChange GetNodeChange(CommonSyntaxNode original, CommonSyntaxNode compareTo)
		{
			if (original == null)
				return new CommonSyntaxNodeChange(original, compareTo, ContractChangeType.Enhancement);

			if (compareTo == null)
				return new CommonSyntaxNodeChange(original, compareTo, ContractChangeType.Breaking);

			if (original.IsEquivalentTo(compareTo))
				return new CommonSyntaxNodeChange(original, compareTo, ContractChangeType.Cosmetic);

			if ((SyntaxKind)original.Kind == SyntaxKind.Block)
				return new CommonSyntaxNodeChange(original, compareTo, ContractChangeType.Refactor);

			var nodeChanges = original.ChildNodes().FullOuterJoin(compareTo.ChildNodes(), (a, b) => a.IsEquivalentTo(b, true), (o, c) => GetNodeChange(o, c));
			var maxChange = nodeChanges.Max(n => n.ChangeType);
			return new CommonSyntaxNodeChange(original, compareTo, nodeChanges, maxChange);
		}

		//private IEnumerable<CommonSyntaxNodeChange> RollUpChanges(IEnumerable<CommonSyntaxTreeChange> trees)
		//{
		//	var result = new List<CommonSyntaxNodeChange>();
		//	foreach (var tree in trees)
		//	{
		//		if (tree.NodeChange == null)
		//			continue;
		//		BoilUpChange(tree.NodeChange, result);
		//	}
		//	return result;
		//}
		//private void BoilUpChange(CommonSyntaxNodeChange change, List<CommonSyntaxNodeChange> changes)
		//{
		//	if (change.Children.Count() == 0)
		//	{
		//		changes.Add(change);
		//		return;
		//	}

		//	foreach (var child in change.Children)
		//		BoilUpChange(child, changes);
		//}
	}
}