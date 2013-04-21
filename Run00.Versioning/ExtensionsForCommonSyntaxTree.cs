using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System.Linq;

namespace Run00.Versioning
{
	public static class ExtensionsForCommonSyntaxTree
	{
		/// <summary>
		/// Determines whether this instance [can be matched with] the specified original.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="compareTo">The compare to.</param>
		/// <returns>
		///   <c>true</c> if this instance [can be matched with] the specified original; otherwise, <c>false</c>.
		/// </returns>
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
