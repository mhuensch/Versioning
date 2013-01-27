using System;
using System.Collections.Generic;
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

			//There should only be a major change, if there was a breaking change from a previous version
			var major = CalculateMajorIncrement(current, previous) + previousVersion.Major;
			if (major != previousVersion.Major)
				return new Version(major, 0, 0, revision);

			//There should only be a minor change, if functionality was added
			var minor = CalculateMinorIncrement(current, previous) + previousVersion.Minor;
			if (minor != previousVersion.Minor)
				return new Version(major, minor, 0, revision);

			//If there were no other changes, increment the build version
			return new Version(major, minor, previousVersion.Build + 1, revision);
		}

		private int CalculateMajorIncrement(Assembly current, Assembly previous)
		{
			foreach (var pType in previous.GetTypesAndReferancedTypes())
			{
				var cType = current.GetTypesAndReferancedTypes().SingleOrDefault(t => pType.FullName == t.FullName);
				if (cType == null)
					return 1;

				if (TypeHasBreakingChange(cType, pType))
					return 1;
			}

			return 0;
		}

		private bool TypeHasBreakingChange(Type currentType, Type previousType)
		{
			foreach (var pMethod in previousType.GetMethods())
			{
				var cMethod = default(MethodInfo);
				try
				{
					cMethod = currentType.GetMethod(pMethod.Name, pMethod.GetParameters().Select(p => p.ParameterType).ToArray());
				}
				catch (AmbiguousMatchException)
				{
					//When a method has more than one method matching a method signature, 
					//we have no way to test this method for a breaking change.
					continue;
				}

				if (cMethod == null)
					return true;

				if (cMethod.ReturnType == null)
					return true;

				if (cMethod.ReturnType.FullName != pMethod.ReturnType.FullName)
					return true;

				if (TypeHasBreakingChange(cMethod.ReturnType, pMethod.ReturnType))
					return true;

				for (var i = 0; i < pMethod.GetParameters().Count(); i++)
				{
					if (TypeHasBreakingChange(cMethod.GetParameters()[i].ParameterType, pMethod.GetParameters()[i].ParameterType))
						return true;
				}
			}

			//foreach (var pProperty in previousType.GetProperties())
			//{
			//	var cProperty = currentType.GetProperty(pProperty.Name);

			//	if (cProperty == null)
			//		return true;

			//	if (cProperty.PropertyType != pProperty.PropertyType)
			//		return true;

			//	if (TypeHasBreakingChange(cProperty.PropertyType, pProperty.PropertyType, testedTypes))
			//		return true;
			//}

			return false;
		}

		private int CalculateMinorIncrement(Assembly current, Assembly previous)
		{
			return 0;
		}
	}
}
