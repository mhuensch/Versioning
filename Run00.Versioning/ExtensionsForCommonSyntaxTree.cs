using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System.Linq;

namespace Run00.Versioning
{
	public static class ExtensionsForCommonSyntaxTree
	{
		public static bool CanBeMatchedWith(this CommonSyntaxTree original, CommonSyntaxTree compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			if (original.IsEquivalentTo(compareTo, true))
				return true;

			return original.GetRoot().CanBeMatchedWith(compareTo.GetRoot());
		}
	}
}
