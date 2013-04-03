using Roslyn.Compilers.Common;

namespace Run00.Versioning
{
	public static class MethodSymbolExtensions
	{
		//public static ChangeType CalculateChange(this IMethodSymbol original, IMethodSymbol compareTo)
		//{
		//	if (original == null)
		//		return ChangeType.Enhancement;

		//	if (compareTo == null)
		//		return ChangeType.Breaking;

		//	if (original.ReturnType != compareTo.ReturnType)
		//		return ChangeType.Breaking;

		//	if (original.DeclaredAccessibility != compareTo.DeclaredAccessibility)
		//		return ChangeType.Breaking;

		//	if (original.Parameters.Count != compareTo.Parameters.Count)
		//		return ChangeType.Breaking;

		//	for (var index = 0; index < original.Parameters.Count; index++)
		//	{
		//		if (original.Parameters.ElementAt(index).Type == compareTo.Parameters.ElementAt(index).Type)
		//			return ChangeType.Breaking;
		//	}

		//	return ChangeType.Cosmetic;
		//}
	}
}
