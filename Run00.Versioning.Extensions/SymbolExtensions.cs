using Roslyn.Compilers.Common;

namespace Run00.Versioning.Extensions
{
	public static class SymbolExtensions
	{
		public static bool IsContractMember(this ISymbol symbol)
		{
			return symbol.CanBeReferencedByName && (
				symbol.DeclaredAccessibility == CommonAccessibility.Public ||
				symbol.DeclaredAccessibility == CommonAccessibility.Internal ||
				symbol.DeclaredAccessibility == CommonAccessibility.ProtectedOrInternal
			);
		}
	}
}
