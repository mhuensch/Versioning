using ApiChange.Api.Introspection;
using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Run00.Versioning.Sequence
{
	public class AssemblyVersioning : IAssemblyVersioning
	{
		Version IAssemblyVersioning.Calculate(string currentDll, string previousDll)
		{
			var current = Assembly.ReflectionOnlyLoadFrom(currentDll);
			var previous = Assembly.ReflectionOnlyLoadFrom(previousDll);

			var previousVersion = previous.GetName().Version;

			//The revision should always be incremented.
			var revision = previousVersion.Revision + 1;
			var differ = new AssemblyDiffer(currentDll, previousDll);
			var diffs = differ.GenerateTypeDiff(QueryAggregator.PublicApiQueries);

			if (diffs.AddedRemovedTypes.RemovedCount > 0)
				return new Version(previousVersion.Major + 1, 0, 0, revision);

			if (diffs.AddedRemovedTypes.AddedCount > 0)
				return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, revision);

			var previousAss = AssemblyFactory.GetAssembly(previousDll);
			var currentAss = AssemblyFactory.GetAssembly(currentDll);
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

		void IAssemblyVersioning.UpdateAssemblyInfo(Stream assemblyInfoFile, Version version)
		{
			var pattern = @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
			var replaceWith = "[assembly: AssemblyVersion(\"" + version + "\")]";

			var reader = new StreamReader(assemblyInfoFile);
			assemblyInfoFile.Seek(0, SeekOrigin.Begin);
			var contents = reader.ReadToEnd();
			var newContents = Regex.Replace(contents, pattern, replaceWith);

			assemblyInfoFile.Seek(0, SeekOrigin.Begin);
			var writer = new StreamWriter(assemblyInfoFile);
			writer.Write(newContents);
			writer.Flush();
		}
	}
}
