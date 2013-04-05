using Roslyn.Compilers.Common;
using Run00.Versioning.Link;

namespace Run00.Versioning.Rules
{
	public class PropertyTypeRule : ISymbolChangeRule
	{
		public SymbolChange GetChange(ISymbolLink link)
		{
			var original = link.OriginalSymbol as IPropertySymbol;
			var compareTo = link.ComparedToSymbol as IPropertySymbol;

			if (original == null || compareTo == null)
				return null;

			if (original.GetMethod.ReturnType.Name != compareTo.GetMethod.ReturnType.Name)
				return new SymbolChange(link, SymbolChangeType.Modifying, "IPropertySymbol.ReturnType changed from " + original.GetMethod.ReturnType.Name + " to " + compareTo.GetMethod.ReturnType.Name + ".");

			if (original.SetMethod.Parameters.Count != compareTo.SetMethod.Parameters.Count)
				return new SymbolChange(link, SymbolChangeType.Modifying, "IPropertySymbol.Parameters.Count changed from " + original.SetMethod.Parameters.Count + " to " + compareTo.SetMethod.Parameters.Count + ".");

			for (var index = 0; index < original.SetMethod.Parameters.Count; index++)
			{
				var oParam = original.SetMethod.Parameters.ElementAt(index);
				var cParam = compareTo.SetMethod.Parameters.ElementAt(index);
				if (oParam.Name != cParam.Name)
					return new SymbolChange(link, SymbolChangeType.Modifying, "A IPropertySymbol.Parameter changed from " + cParam.Name + " to " + cParam.Name + ".");
			}

			return null;
		}

		public bool IsValidFor(ISymbolLink symbol)
		{
			return symbol.SymbolType is IPropertySymbol;
		}
	}
}
