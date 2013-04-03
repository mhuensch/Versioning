using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Extensions
{
	public static class NamespaceExtensions
	{
		public static IEnumerable<INamedTypeSymbol> GetContractTypes(this INamespaceSymbol namespaceSymbol)
		{
			if (namespaceSymbol == null)
				return Enumerable.Empty<INamedTypeSymbol>();

			var result = new List<INamedTypeSymbol>();

			result.AddRange(namespaceSymbol.GetTypeMembers().AsEnumerable());

			foreach (var childNamespace in namespaceSymbol.GetNamespaceMembers())
				result.AddRange(childNamespace.GetContractTypes());

			return result.Where(t => t.IsContractMember());
		}
	}
}
