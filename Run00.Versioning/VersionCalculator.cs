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

		/// <summary>
		/// Updates the assembly info for each of the projects in the suggested versions.
		/// </summary>
		/// <param name="solution">The solution to be updated.</param>
		/// <param name="suggestedVersions">The suggested versions.</param>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AssemblyInfo", Justification = "This is a known/valid file name.")]
		public static void UpdateAssemblyInfo(ISolution solution, IEnumerable<SuggestedVersion> suggestedVersions)
		{
			Contract.Requires(solution != null);
			Contract.Requires(suggestedVersions != null);
			Contract.Ensures(0 <= Enumerable.Count(suggestedVersions));

			var selectedVersions = suggestedVersions
				.Where(v => v.Justification.ComparedTo != null)
				.Select(v => new { Version = v.Suggested, Compilation = v.Justification.ComparedTo });

			foreach (var selectedVersion in selectedVersions)
			{
				if (selectedVersion == null || selectedVersion.Compilation == null || selectedVersion.Compilation.SyntaxTrees == null)
					continue;

				var assemblyFiles = selectedVersion.Compilation.SyntaxTrees.Where(t => Path.GetFileName(t.FilePath).Equals(_assemblyFileName));
				if (assemblyFiles.Count() != 1)
					throw new InvalidOperationException("More than one file found with the name: " + _assemblyFileName);

				var syntaxTree = assemblyFiles.Single();
				Contract.Assume(syntaxTree != null, "Single() can not return a null reference.");

				var root = syntaxTree.GetRoot();
				if (root == null)
					throw new InvalidOperationException(_assemblyFileName + " must have a valid root.");

				var contents = root.ToFullString();
				Contract.Assume(contents != null, "ToFullString() can not return a null string");

				var newContents = Regex.Replace(contents, _assemblyRegexPattern, "[assembly: AssemblyVersion(\"" + selectedVersion.Version + "\")]");
				newContents = Regex.Replace(newContents, _assemblyFileRegexPattern, "[assembly: AssemblyFileVersion(\"" + selectedVersion.Version + "\")]");
				File.WriteAllText(syntaxTree.FilePath, newContents);
			}
		}

		private static SuggestedVersion GetSuggestedVersion(ICompilation original, ICompilation compareTo)
		{
			Contract.Requires(original != null || compareTo != null);
			Contract.Ensures(Contract.Result<SuggestedVersion>() != null);

			var justification = GetCompilationChange(original, compareTo);

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

			return new SuggestedVersion(originalVersion, suggested, justification);
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

		private static ChangesInCompilation GetCompilationChange(ICompilation original, ICompilation compareTo)
		{
			Contract.Ensures(Contract.Result<ChangesInCompilation>() != null);

			if (original == null && compareTo == null)
				throw new InvalidOperationException("Original and Compare to can not both be null.");

			if (original == null)
				return new ChangesInCompilation(original, compareTo, ContractChangeType.Enhancement);

			if (compareTo == null)
				return new ChangesInCompilation(original, compareTo, ContractChangeType.Breaking);


			var typeChanges = original.Assembly.Namespace.GetContractTypes()
				.FullOuterJoin(compareTo.Assembly.Namespace.GetContractTypes(), (a, b) => a.CanBeMatchedWith(b), (o, c) => GetTypeChange(o, c));
			
			var treeChanges = Enumerable.Empty<ChangesInSyntaxNode>();
			var maxChange = typeChanges.Max(t => t.ChangeType);
			return new ChangesInCompilation(original, compareTo, typeChanges, maxChange);
		}

		//private static ChangesInSyntaxTree GetTreeChange(ISyntaxTree original, ISyntaxTree compareTo)
		//{
		//	Contract.Ensures(Contract.Result<ChangesInSyntaxTree>() != null);

		//	if (original == null)
		//		return new ChangesInSyntaxTree(original, compareTo, ContractChangeType.Enhancement);

		//	if (compareTo == null)
		//		return new ChangesInSyntaxTree(original, compareTo, ContractChangeType.Breaking);

		//	if (original.HasChanges(compareTo) == false)
		//		return new ChangesInSyntaxTree(original, compareTo, ContractChangeType.None);

		//	if (original.IsEquivalentTo(compareTo))
		//		return new ChangesInSyntaxTree(original, compareTo, ContractChangeType.Cosmetic);

		//	var nodeChange = GetNodeChange(original.GetRoot(), compareTo.GetRoot());
		//	return new ChangesInSyntaxTree(original, compareTo, nodeChange, nodeChange.ChangeType);
		//}

		private static ChangesInType GetTypeChange(IType original, IType compareTo)
		{
			Contract.Ensures(Contract.Result<ChangesInType>() != null);

			if (original == null && compareTo == null)
				throw new InvalidOperationException("Original and Compare to can not both be null.");

			if (original == null)
				return new ChangesInType(original, compareTo, ContractChangeType.Enhancement);

			if (compareTo == null)
				return new ChangesInType(original, compareTo, ContractChangeType.Breaking);

			var nodeChanges = original.SyntaxNodes.FullOuterJoin(compareTo.SyntaxNodes, (a, b) => a.CanBeMatchedWith(b), (o, c) => GetNodeChange(o, c));
			var maxChange = nodeChanges.Max(t => t.ChangeType);

			return new ChangesInType(original, compareTo, nodeChanges, maxChange);
		}

		private static ChangesInSyntaxNode GetNodeChange(ISyntaxNode original, ISyntaxNode compareTo)
		{
			Contract.Ensures(Contract.Result<ChangesInSyntaxNode>() != null);


			if (original == null)
			{
				if (compareTo != null && compareTo.IsPrivate())
					return new ChangesInSyntaxNode(original, compareTo, ContractChangeType.Refactor);
				else
					return new ChangesInSyntaxNode(original, compareTo, ContractChangeType.Enhancement);
			}

			if (compareTo == null)
			{
				if (original != null && original.IsPrivate())
					return new ChangesInSyntaxNode(original, compareTo, ContractChangeType.Refactor);
				else
					return new ChangesInSyntaxNode(original, compareTo, ContractChangeType.Breaking);
			}

			if (original.IsEquivalentTo(compareTo))
				return new ChangesInSyntaxNode(original, compareTo, ContractChangeType.Cosmetic);

			if (original.IsBlock())
				return new ChangesInSyntaxNode(original, compareTo, ContractChangeType.Refactor);

			var nodeChanges = original.ChildNodes().FullOuterJoin(compareTo.ChildNodes(), (a, b) => a.CanBeMatchedWith(b), (o, c) => GetNodeChange(o, c));
			var maxChange = nodeChanges.Max(n => n.ChangeType);
			return new ChangesInSyntaxNode(original, compareTo, nodeChanges, maxChange);
		}

		private const string _assemblyFileName = @"AssemblyInfo.cs";
		private const string _assemblyRegexPattern = @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
		private const string _assemblyFileRegexPattern = @"\[assembly\: AssemblyFileVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
	}
}