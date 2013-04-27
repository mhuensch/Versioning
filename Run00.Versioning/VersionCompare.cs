using Run00.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	public static class VersionCompare
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
		public static IEnumerable<CompilationChanges> Compare(ICSharpSolution original, ICSharpSolution compareTo)
		{
			Contract.Requires(original != null);
			Contract.Requires(compareTo != null);
			Contract.Ensures(Contract.Result<IEnumerable<CompilationChanges>>() != null);
			Contract.Ensures(Enumerable.Count(Contract.Result<IEnumerable<CompilationChanges>>()) >= 0);

			return original.Compilations.FullOuterJoin(
				compareTo.Compilations,
				(a, b) => IsMatchedWith(a, b),
				(o, c) => new CompilationChanges(o, c, GetContractChanges(o, c))
			);
		}

		private static ContractChanges GetContractChanges(IContractItem original, IContractItem compareTo)
		{
			Contract.Ensures(Contract.Result<ContractChanges>() != null);

			if (original == null && compareTo == null)
				throw new InvalidOperationException("Original and Compare to can not both be null.");

			if (original == null)
			{
				if (compareTo != null && compareTo.IsPrivate)
					return new ContractChanges(original, compareTo, ContractChangeType.Refactor);
				else
					return new ContractChanges(original, compareTo, ContractChangeType.Enhancement);
			}

			if (compareTo == null)
			{
				if (original != null && original.IsPrivate)
					return new ContractChanges(original, compareTo, ContractChangeType.Refactor);
				else
					return new ContractChanges(original, compareTo, ContractChangeType.Breaking);
			}

			if (original.IsCodeBlock)
			{
				if (original.IsEquivalentTo(compareTo))
					return new ContractChanges(original, compareTo, ContractChangeType.Cosmetic);
				else
					return new ContractChanges(original, compareTo, ContractChangeType.Refactor);
			}


			if (original.Children.Count() == 0 && compareTo.Children.Count() == 0)
				return new ContractChanges(original, compareTo, ContractChangeType.None);

			var nodeChanges = original.Children.FullOuterJoin(compareTo.Children, (a, b) => IsMatchedWith(a, b), (o, c) => GetContractChanges(o, c));
			var maxChange = nodeChanges.Max(n => n.ChangeType);
			return new ContractChanges(original, compareTo, nodeChanges, maxChange);
		}

		private static bool IsMatchedWith(IContractItem original, IContractItem compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			return original.IsMatchedWith(compareTo);
		}
	}
}