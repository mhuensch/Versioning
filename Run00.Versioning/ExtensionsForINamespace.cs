using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning
{
	public static class ExtensionsForINamespace
	{
		public static IEnumerable<IType> GetContractTypes(this INamespace namespaceSymbol)
		{
			if (namespaceSymbol == null)
				return Enumerable.Empty<IType>();

			var result = new List<IType>();

			result.AddRange(namespaceSymbol.GetTypes());

			foreach (var childNamespace in namespaceSymbol.GetNamespaceMembers())
				result.AddRange(childNamespace.GetContractTypes());

			return result.Where(t => t.IsContractType);
		}
	}
}
