using Roslyn.Compilers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning.Compare
{
	public class SymbolChangeCalculator
	{
		public ContractChange CalculateChange(ISymbol original, ISymbol compareTo, IEnumerable<ISymbolComparison> children)
		{
			if (original == null)
				return new ContractChange(ContractChangeType.Adding, "No match for original symbol found.");

			if (compareTo == null)
				return new ContractChange(ContractChangeType.Deleting, "No match for comparison symbol found.");

			if (original.Kind != compareTo.Kind)
				return new ContractChange(ContractChangeType.Modifying, "ISymbol.Kind changed from " + original.Kind + " to " + compareTo.Kind + ".");

			if (original.DeclaredAccessibility != compareTo.DeclaredAccessibility)
				return new ContractChange(ContractChangeType.Modifying, "ISymbol.DeclaredAccessibility changed from " + original.DeclaredAccessibility + " to " + compareTo.DeclaredAccessibility + ".");

			if (original.IsAbstract != compareTo.IsAbstract)
				return new ContractChange(ContractChangeType.Modifying, "ISymbol.IsAbstract changed from " + original.IsAbstract + " to " + compareTo.IsAbstract + ".");

			if (original.IsSealed != compareTo.IsSealed)
				return new ContractChange(ContractChangeType.Modifying, "ISymbol.IsSealed changed from " + original.IsSealed + " to " + compareTo.IsSealed + ".");

			if (original.IsStatic != compareTo.IsStatic)
				return new ContractChange(ContractChangeType.Modifying, "ISymbol.IsStatic changed from " + original.IsStatic + " to " + compareTo.IsStatic + ".");

			if (original.IsVirtual != compareTo.IsVirtual)
				return new ContractChange(ContractChangeType.Modifying, "ISymbol.IsVirtual changed from " + original.IsVirtual + " to " + compareTo.IsVirtual + ".");

			if (children.Count() == 0)
				return new ContractChange(ContractChangeType.None, "The symbols match and no child symbols were found.");

			var childChange = children.Max(c => c.ContractChange.ChangeType);

			if (childChange == ContractChangeType.Adding)
				return new ContractChange(ContractChangeType.Adding, "Child symbols have been added.");

			if (childChange == ContractChangeType.Modifying)
				return new ContractChange(ContractChangeType.Modifying, "Child symbols have been modified.");

			if (childChange == ContractChangeType.Deleting)
				return new ContractChange(ContractChangeType.Modifying, "Child symbols have been removed.");

			return new ContractChange(ContractChangeType.None, "The symbols match and and all child symbols match.");
		}
	}
}
