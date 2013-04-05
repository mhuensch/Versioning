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
				changes.Add(new ContractChange(link, childChanges));
			}

			return changes;
		}

		public IEnumerable<SymbolChange> CalculateSymbolChanges(ISymbolLink link)
		{
			var result = new List<SymbolChange>();
			foreach(var rule in _rules)
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
