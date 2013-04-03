using Roslyn.Compilers.Common;

namespace Run00.Versioning
{
	public static class SymbolExtensions
	{
		//public static ChangeType CalculateChange(this ISymbol original, ISymbol compareTo)
		//{
		//	if (compareTo == null)
		//		return ChangeType.Breaking;

		//	if (original == null)
		//		return ChangeType.Enhancement;

		//	//if (original == null)
		//	//{
		//	//	if (original.Children.Count() > 0)
		//	//		return ChangeType.Enhancement;
		//	//	else
		//	//		return ChangeType.Cosmetic;
		//	//}

		//	if (original.Kind != compareTo.Kind)
		//		return ChangeType.Breaking;

		//	if (original.DeclaredAccessibility != compareTo.DeclaredAccessibility)
		//		return ChangeType.Breaking;

		//	//return visitor.Children.Max(c => c.CalculateChange());

		//	return ChangeType.Unknown;
		//}
	}
}
