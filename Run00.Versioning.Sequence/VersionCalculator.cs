using ApiChange.Api.Introspection;
using Mono.Cecil;
using System;
using System.Linq;
using System.Reflection;

namespace Run00.Versioning.Sequence
{
	public class VersionCalculator : IVersionCalculator
	{
		Version IVersionCalculator.Calculate(string currentDll, string previousDll)
		{
			var current = Assembly.ReflectionOnlyLoadFrom(currentDll);
			var previous = Assembly.ReflectionOnlyLoadFrom(previousDll);

			var previousVersion = previous.GetName().Version;

			//The revision should always be incremented.
			var revision = previousVersion.Revision + 1;
			var differ = new AssemblyDiffer(previousDll, currentDll);
			var diffs = differ.GenerateTypeDiff(QueryAggregator.PublicApiQueries);

			if (diffs.AddedRemovedTypes.RemovedCount > 0)
				return new Version(previousVersion.Major + 1, 0, 0, revision);

			if (diffs.AddedRemovedTypes.AddedCount > 0)
				return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, revision);

			var previousAss = AssemblyFactory.GetAssembly(previousDll);
			var currentAss = AssemblyFactory.GetAssembly(currentDll);
			foreach (var type in previousAss.MainModule.Types.Cast<TypeDefinition>())
			{
				var x = type as TypeDefinition;
				var cType = currentAss.MainModule.Types.Cast<TypeDefinition>().Single(c => c.FullName == type.FullName);
				var differ2 = TypeDiff.GenerateDiff(type, cType, QueryAggregator.PublicApiQueries);

				if (differ2.Methods.Where(m => m.Operation.IsRemoved).Count() > 0)
					return new Version(previousVersion.Major + 1, 0, 0, revision);

				if (differ2.Methods.Where(m => m.Operation.IsAdded).Count() > 0)
					return new Version(previousVersion.Major, previousVersion.Minor + 1, 0, revision);
			}

			//If there were no other changes, increment the build version
			return new Version(previousVersion.Major, previousVersion.Minor, previousVersion.Build + 1, revision);
		}
	}
}
