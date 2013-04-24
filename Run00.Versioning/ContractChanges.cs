using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	public class ContractChanges
	{
		public IContractItem Original { get; private set; }

		public IContractItem ComparedTo { get; private set; }

		public IEnumerable<ContractChanges> Changes { get; private set; }

		public ContractChangeType ChangeType { get; private set; }

		public ContractChanges(IContractItem original, IContractItem comparedTo, ContractChangeType changeType)
		{
			Contract.Requires(original != null || comparedTo != null);
			Contract.Ensures(Changes != null);
			Contract.Ensures(Enumerable.Count(Changes) == 0);

			Original = original;
			ComparedTo = comparedTo;
			Changes = Enumerable.Empty<ContractChanges>();
			ChangeType = changeType;
		}

		public ContractChanges(IContractItem original, IContractItem comparedTo, IEnumerable<ContractChanges> changes, ContractChangeType changeType)
		{
			Contract.Requires(original != null || comparedTo != null);
			Contract.Requires(changes != null);

			Original = original;
			ComparedTo = comparedTo;
			Changes = changes;
			ChangeType = changeType;
		}

		[ContractInvariantMethod, ExcludeFromCodeCoverage]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for code contracts.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(Original != null || ComparedTo != null);
			Contract.Invariant(Changes != null);
		}
	}
}
