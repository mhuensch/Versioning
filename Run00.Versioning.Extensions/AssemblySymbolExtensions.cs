using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Extensions
{
	public static class AssemblySymbolExtensions
	{
		public static IEnumerable<INamedTypeSymbol> GetContractTypes(this IAssemblySymbol assembly)
		{
			if (assembly == null)
				return Enumerable.Empty<INamedTypeSymbol>();

			return assembly.GlobalNamespace.GetContractTypes();
		}
	}
}
