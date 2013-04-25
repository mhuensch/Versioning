using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning
{
	public class CompilationChanges
	{
		public ICompilation Original { get; private set; }

		public ICompilation ComparedTo { get; private set; }

		public ContractChanges Changes { get; private set; }

		public CompilationChanges(ICompilation original, ICompilation comparedTo, ContractChanges changes)
		{
			Contract.Requires(original != null || comparedTo != null);
			Contract.Requires(changes != null);

			Original = original;
			ComparedTo = comparedTo;
			Changes = changes;
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
