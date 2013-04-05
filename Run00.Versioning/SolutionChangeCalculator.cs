using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Run00.Utilities;
using Run00.Versioning.Link;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning
{
	public class SolutionChangeCalculator
	{
		public SolutionChangeCalculator(SolutionLinker linker, IEnumerable<ISymbolChangeRule> rules)
		{
			_rules = rules;
			_linker = linker;
		}

		public IEnumerable<ContractChange> GetChanges(ISolution original, ISolution compareTo)
		{
			var changes = new List<ContractChange>();

			var assemblyLinks = _linker.Link(original, compareTo);
			foreach (var link in assemblyLinks)
			{
				var childChanges = link.RollUp<ISymbolLink>().SelectMany(l => CalculateSymbolChanges(l));

				var change = ContractChangeType.Cosmetic;

				if (childChanges.Count() > 0)
				{
					var maxChange = childChanges.Max(c => c.ChangeType);
					if (maxChange == SymbolChangeType.Deleting || maxChange == SymbolChangeType.Modifying)
						change = ContractChangeType.Breaking;

					if (maxChange == SymbolChangeType.Adding)
						change = ContractChangeType.Enhancement;
				}

				var refactor = false;
				if (change == ContractChangeType.Cosmetic)
					refactor = GetFirstRefactor(original, compareTo, link.Original.BaseName);

				if (refactor)
					change = ContractChangeType.Refactor;

				changes.Add(new ContractChange(link, childChanges, change));
			}

			return changes;
		}

		private bool GetFirstRefactor(ISolution original, ISolution compareTo, string assemblyName)
		{
			var pOriginal = original.Projects.Where(p => p.AssemblyName == assemblyName).Single();
			var pCompareTo = compareTo.Projects.Where(p => p.AssemblyName == assemblyName).Single();

			if (pOriginal.Documents.Count() != pOriginal.Documents.Count())
				return true;

			for (var index = 0; index < pOriginal.Documents.Count(); index++)
			{
				var dOriginal = pOriginal.Documents.ElementAt(index);
				var dCompareTo = pCompareTo.Documents.ElementAt(index);

				if (dOriginal.Name != dCompareTo.Name)
					return true;

				var tOriginal = SyntaxTree.ParseText(dOriginal.GetText());
				var tCompareTo = SyntaxTree.ParseText(dCompareTo.GetText());


				var spans = tCompareTo.GetChanges(tOriginal);
				var spans2 = tCompareTo.GetChangedSpans(tOriginal);
				
			}

			//var result = tree.GetLineSpan(spans.First(), false);
			//var something = tree.GetChanges(tree2);
			//var test = devDocuments[1].GetCodeRefactorings(spans.First());
			return false;
		}


		public IEnumerable<SymbolChange> CalculateSymbolChanges(ISymbolLink link)
		{
			var result = new List<SymbolChange>();
			foreach (var rule in _rules)
			{
				if (rule.IsValidFor(link) == false)
					continue;

				var change = rule.GetChange(link);
				if (change == null)
					continue;

				result.Add(change);
			}
			return result;
		}

		private readonly IEnumerable<ISymbolChangeRule> _rules;
		private readonly SolutionLinker _linker;
	}
}
