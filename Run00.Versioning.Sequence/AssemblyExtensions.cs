using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning.Sequence
{
	public static class AssemblyExtensions
	{
		public static IEnumerable<Type> GetTypesAndReferancedTypes(this Assembly assembly)
		{
			var result = new List<Type>();

			return result;
		}

		private static IEnumerable<Type> GetReferancedTypes(Type type)
		{
			var result = new List<Type>();

			foreach (var property in type.GetProperties())
				result.Add(property.PropertyType);

			foreach (var method in type.GetMethods())
			{
				result.Add(method.ReturnType);
				foreach (var parameter in method.GetParameters())
					result.Add(parameter.ParameterType);
			}

			foreach (var foundType in result)
			{
				var x = GetReferancedTypes(foundType);
				foreach (var y in x)
				{
					if (result.Contains(y) == false)
						result.Add(y);
				}
			}

			return result;
		}
	}
}
