using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Extensions
{
	public static class TypeExtensions
	{
		public static IEnumerable<IMethodSymbol> GetContractMethods(this INamedTypeSymbol type)
		{
			if (type == null)
				return Enumerable.Empty<IMethodSymbol>();

			return
				from m
				in type.GetMembers().AsEnumerable()
				where
					m.IsContractMember()
					&& m.Kind == CommonSymbolKind.Method
				select (IMethodSymbol)m;
		}

		public static IEnumerable<IPropertySymbol> GetContractProperties(this INamedTypeSymbol type)
		{
			if (type == null)
				return Enumerable.Empty<IPropertySymbol>();

			return
				from m
				in type.GetMembers().AsEnumerable()
				where
					m.IsContractMember()
					&& m.Kind == CommonSymbolKind.Property
				select (IPropertySymbol)m;
		}
	}
}
