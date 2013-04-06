using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Run00.Utilities;
using Run00.Versioning.Link;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	public class ContractChangeCalculator
	{
		/// <summary>
		/// Gets the changes between the original solution and the compareTo solution.
		/// </summary>
		/// <param name="original">The original solution.</param>
		/// <param name="compareTo">The solution to compare the original to.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">Original.Projects and Compare.Projects to can not be null.</exception>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked by code contracts.")]
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Checked by code contracts.")]
		public IEnumerable<CommonCompilationChange> GetChanges(ISolution original, ISolution compareTo)
		{
			Contract.Requires(original != null);
			Contract.Requires(compareTo != null);
			Contract.Ensures(Contract.Result<IEnumerable<CommonCompilationChange>>() != null);
			Contract.Ensures(Enumerable.Count(Contract.Result<IEnumerable<CommonCompilationChange>>()) >= 0);

			if (original.Projects == null || compareTo.Projects == null)
				throw new InvalidOperationException("Original.Projects and Compare.Projects to can not be null.");

			var oAssemblies = original.Projects.Select(p => p.GetCompilation());
			var cAssemblies = compareTo.Projects.Select(p => p.GetCompilation());
			return oAssemblies.FullOuterJoin(cAssemblies, (t) => t.Assembly.Name, (o, c) => GetCompilationChange(o, c));
		}

		private CommonCompilationChange GetCompilationChange(CommonCompilation original, CommonCompilation compareTo)
		{
			Contract.Ensures(Contract.Result<CommonCompilationChange>() != null);

			if (original == null && compareTo == null)
				throw new InvalidOperationException("Original and Compare to can not both be null.");

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
			Contract.Ensures(Contract.Result<CommonSyntaxTreeChange>() != null);

			if (original == null)
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.Enhancement);

			if (compareTo == null)
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.Breaking);

			var textChanges = original.GetChanges(compareTo);
			if (textChanges != null && textChanges.Count == 0)
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.None);

			if (original.IsEquivalentTo(compareTo))
				return new CommonSyntaxTreeChange(original, compareTo, ContractChangeType.Cosmetic);

			var nodeChange = GetNodeChange(original.GetRoot(), compareTo.GetRoot());
			return new CommonSyntaxTreeChange(original, compareTo, nodeChange, nodeChange.ChangeType);
		}

		private CommonSyntaxNodeChange GetNodeChange(CommonSyntaxNode original, CommonSyntaxNode compareTo)
		{
			Contract.Ensures(Contract.Result<CommonSyntaxNodeChange>() != null);

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
	}
}