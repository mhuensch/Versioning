using Run00.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Run00.Versioning
{
	public static class VersionCalculator
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
		public static IEnumerable<SuggestedVersion> SuggestVersions(ISolution original, ISolution compareTo)
		{
			Contract.Requires(original != null);
			Contract.Requires(compareTo != null);
			Contract.Ensures(Contract.Result<IEnumerable<SuggestedVersion>>() != null);
			Contract.Ensures(Enumerable.Count(Contract.Result<IEnumerable<SuggestedVersion>>()) >= 0);

			if (original.Projects == null || compareTo.Projects == null)
				throw new InvalidOperationException("Original.Projects and Compare.Projects to can not be null.");

			var oAssemblies = original.Projects.Select(p => p.GetCompilation());
			var cAssemblies = compareTo.Projects.Select(p => p.GetCompilation());

			return oAssemblies.FullOuterJoin(cAssemblies, (t) => t.Assembly.Name, (o, c) => GetSuggestedVersion(o, c));
		}

		private static SuggestedVersion GetSuggestedVersion(ICompilation original, ICompilation compareTo)
		{
			Contract.Requires(original != null || compareTo != null);
			Contract.Ensures(Contract.Result<SuggestedVersion>() != null);

			var justification = GetContractChanges(original, compareTo);

			var originalVersion = GetVersion(original != null ? original : compareTo);
			if (originalVersion == null)
				originalVersion = new Version("0.0.0.0");

			var suggested = new Version("0.0.0.0");
			switch (justification.ChangeType)
			{
				case ContractChangeType.None:
					suggested = new Version(originalVersion.Major, originalVersion.Minor, originalVersion.Build, originalVersion.Revision);
					break;
				case ContractChangeType.Cosmetic:
					suggested = new Version(originalVersion.Major, originalVersion.Minor, originalVersion.Build, originalVersion.Revision + 1);
					break;
				case ContractChangeType.Refactor:
					suggested = new Version(originalVersion.Major, originalVersion.Minor, originalVersion.Build + 1, 0);
					break;
				case ContractChangeType.Enhancement:
					suggested = new Version(originalVersion.Major, originalVersion.Minor + 1, 0, 0);
					break;
				case ContractChangeType.Breaking:
					suggested = new Version(originalVersion.Major + 1, 0, 0, 0);
					break;
				default:
					throw new InvalidOperationException("Contract change type for justification is not valid.");
			}

			return new SuggestedVersion(originalVersion, suggested, original, compareTo, justification);
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
				if (((ISyntaxNode)original).IsEquivalentTo(((ISyntaxNode)compareTo)))
					return new ContractChanges(original, compareTo, ContractChangeType.Cosmetic);
				else
					return new ContractChanges(original, compareTo, ContractChangeType.Refactor);
			}


			if (original.Children.Count() == 0 && compareTo.Children.Count() == 0)
				return new ContractChanges(original, compareTo, ContractChangeType.None);

			var nodeChanges = original.Children.FullOuterJoin(compareTo.Children, (a, b) => a.CanBeMatchedWith(b), (o, c) => GetContractChanges(o, c));
			var maxChange = nodeChanges.Max(n => n.ChangeType);
			return new ContractChanges(original, compareTo, nodeChanges, maxChange);
		}

		private static Version GetVersion(ICompilation compliation)
		{
			if (compliation == null)
				return null;

			var assembly = compliation.Assembly;
			if (assembly == null)
				return null;

			var attributes = assembly.GetAttributes().AsEnumerable();
			if (attributes == null)
				return null;

			var attribute = default(IAttribute);
			foreach (var a in attributes)
			{
				if (a == null || a.AttributeClass == null)
					continue;

				if (a.AttributeClass.Equals("AssemblyVersionAttribute") == false)
					continue;

				attribute = a;
			}

			if (attribute == null || attribute.ConstructorArguments == null)
				return null;

			var value = attribute.ConstructorArguments.ElementAt(0);
			if (value.Value == null)
				return null;

			return new Version(value.Value.ToString());
		}

	}
}