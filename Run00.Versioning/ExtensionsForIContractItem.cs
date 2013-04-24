using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning
{
	public static class ExtensionsForIContractItem
	{
		public static bool CanBeMatchedWith(this IContractItem original, IContractItem compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			var o = original as ISyntaxNode;
			var c = compareTo as ISyntaxNode;
			if (o != null && c != null)
			{
				if (o.IsEquivalentTo(c, true))
					return true;
			}

			return original.Name.Equals(compareTo.Name);
		}
	}
}
