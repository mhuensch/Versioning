using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Run00.Versioning
{
	public static class VersionSetter
	{
		/// <summary>
		/// Updates the assembly info for each of the projects in the suggested versions.
		/// </summary>
		/// <param name="solution">The solution to be updated.</param>
		/// <param name="suggestedVersions">The suggested versions.</param>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AssemblyInfo", Justification = "This is a known/valid file name.")]
		public static void UpdateAssemblyInfo(IEnumerable<SuggestedVersion> suggestedVersions)
		{
			Contract.Requires(suggestedVersions != null);
			Contract.Ensures(0 <= Enumerable.Count(suggestedVersions));

			foreach (var selectedVersion in suggestedVersions)
			{
				if (selectedVersion == null || selectedVersion.ComparedToComp == null || selectedVersion.ComparedToComp.SyntaxTrees == null)
					continue;

				var assemblyFiles = selectedVersion.ComparedToComp.SyntaxTrees.Where(t => Path.GetFileName(t.FilePath).Equals(_assemblyFileName));
				if (assemblyFiles.Count() != 1)
					throw new InvalidOperationException("More than one file found with the name: " + _assemblyFileName);

				var syntaxTree = assemblyFiles.Single();
				Contract.Assume(syntaxTree != null, "Single() can not return a null reference.");

				var root = syntaxTree.GetRoot();
				if (root == null)
					throw new InvalidOperationException(_assemblyFileName + " must have a valid root.");

				var contents = root.ToFullString();
				Contract.Assume(contents != null, "ToFullString() can not return a null string");

				var newContents = Regex.Replace(contents, _assemblyRegexPattern, "[assembly: AssemblyVersion(\"" + selectedVersion.Suggested + "\")]");
				newContents = Regex.Replace(newContents, _assemblyFileRegexPattern, "[assembly: AssemblyFileVersion(\"" + selectedVersion.Suggested + "\")]");
				File.WriteAllText(syntaxTree.FilePath, newContents);
			}
		}

		private const string _assemblyFileName = @"AssemblyInfo.cs";
		private const string _assemblyRegexPattern = @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
		private const string _assemblyFileRegexPattern = @"\[assembly\: AssemblyFileVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
	}
}
