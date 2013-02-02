using ApiChange.Api.Introspection;
using Mono.Cecil;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Run00.Versioning.Sequence
{
	public class AssemblyVersioning : IAssemblyVersioning
	{
		Version IAssemblyVersioning.Calculate(string currentDll, string previousDll)
		{
			var currentAss = AssemblyFactory.GetAssembly(currentDll);
			if (string.IsNullOrEmpty(previousDll))
				return currentAss.Name.Version;


			var version = FileVersionInfo.GetVersionInfo(currentDll);
			var previousAss = AssemblyFactory.GetAssembly(previousDll);
			var previousVersion = previousAss.Name.Version;

			//The revision should always be incremented.
			var revision = previousVersion.Revision + 1;
			var differ = new AssemblyDiffer(currentDll, previousDll);
			var diffs = differ.GenerateTypeDiff(QueryAggregator.PublicApiQueries);

			if (diffs.AddedRemovedTypes.RemovedCount > 0)
				return new Version(previousVersion.Major + 1, 0, 0, revision);

			if (diffs.AddedRemovedTypes.AddedCount > 0)
				return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, revision);

			var minorChangeFound = false;
			foreach (var type in previousAss.MainModule.Types.Cast<TypeDefinition>())
			{
				var x = type as TypeDefinition;
				var cType = currentAss.MainModule.Types.Cast<TypeDefinition>().Single(c => c.FullName == type.FullName);
				var differ2 = TypeDiff.GenerateDiff(cType, type, QueryAggregator.PublicApiQueries);

				if (differ2.Methods.Any(m => m.Operation.IsRemoved) || differ2.Fields.Any(m => m.Operation.IsRemoved) || differ2.Events.Any(m => m.Operation.IsRemoved))
					return new Version(previousVersion.Major + 1, 0, 0, revision);

				if (differ2.Methods.Any(m => m.Operation.IsAdded) || differ2.Fields.Any(m => m.Operation.IsAdded) || differ2.Events.Any(m => m.Operation.IsAdded))
					minorChangeFound = true;
			}

			if (minorChangeFound)
				return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, revision);


			//If there were no other changes, increment the build version
			return new Version(previousVersion.Major, previousVersion.Minor, previousVersion.Build + 1, revision);
		}

		string IAssemblyVersioning.UpdateAssemblyInfo(Stream assemblyInfoFile, Version version)
		{
			var reader = new StreamReader(assemblyInfoFile);
			assemblyInfoFile.Seek(0, SeekOrigin.Begin);
			var contents = reader.ReadToEnd();

			var pattern = @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
			var replaceWith = "[assembly: AssemblyVersion(\"" + version + "\")]";
			var newContents = Regex.Replace(contents, pattern, replaceWith);

			pattern = @"\[assembly\: AssemblyFileVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
			replaceWith = "[assembly: AssemblyFileVersion(\"" + version + "\")]";
			newContents = Regex.Replace(newContents, pattern, replaceWith);

			return newContents;
		}
	}
}
