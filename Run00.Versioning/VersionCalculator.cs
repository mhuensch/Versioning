using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

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
		public static SuggestedVersion Calculate(CompilationChanges changes)
		{
			Contract.Ensures(Contract.Result<SuggestedVersion>() != null);

			var originalVersion = GetVersion(changes.Original != null ? changes.Original : changes.ComparedTo);
			if (originalVersion == null)
				originalVersion = new Version("0.0.0.0");

			var suggested = new Version("0.0.0.0");
			switch (changes.Changes.ChangeType)
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

			return new SuggestedVersion(originalVersion, suggested, changes.Original, changes.ComparedTo, changes.Changes);
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