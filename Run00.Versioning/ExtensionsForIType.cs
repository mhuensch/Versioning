
namespace Run00.Versioning
{
	public static class ExtensionsForIType
	{
		public static bool CanBeMatchedWith(this IType original, IType compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			return original.Name.Equals(compareTo.Name);
		}
	}
}
