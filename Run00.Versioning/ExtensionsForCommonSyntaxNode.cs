using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	public static class ExtensionsForCommonSyntaxNode
	{
		/// <summary>
		/// Determines whether this instance [can be matched with] the specified original.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="compareTo">The compare to.</param>
		/// <returns>
		///   <c>true</c> if this instance [can be matched with] the specified original; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanBeMatchedWith(this ISyntaxNode original, ISyntaxNode compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			if (original.IsEquivalentTo(compareTo, true))
				return true;
			
			var originalName = original.GetIdentifierName();
			var compareToName = compareTo.GetIdentifierName();
			if (originalName == null || compareToName == null)
				return false;

			return originalName.Equals(compareToName);
		}





	}
}
