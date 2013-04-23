using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	[DebuggerDisplay("{DisplayString()}")]
	public class ChangesInType
	{

		public IType Original { get; private set; }

		public IType ComparedTo { get; private set; }

		public IEnumerable<ChangesInSyntaxNode> NodeChanges { get; private set; }

		/// <summary>
		/// Gets the type of the change.
		/// </summary>
		/// <value>
		/// The type of the change.
		/// </value>
		public ContractChangeType ChangeType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangesInSyntaxNode"/> class.
		/// </summary>
		/// <param name="original">The original node</param>
		/// <param name="comparedTo">The node it was compared to.</param>
		/// <param name="changeType">Type of the change when the original and compared to were compared</param>
		public ChangesInType(IType original, IType comparedTo, ContractChangeType changeType)
		{
			Contract.Ensures(NodeChanges != null);
			Contract.Ensures(Enumerable.Count(NodeChanges) == 0);

			Original = original;
			ComparedTo = comparedTo;
			NodeChanges = Enumerable.Empty<ChangesInSyntaxNode>();
			ChangeType = changeType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangesInSyntaxNode"/> class.
		/// </summary>
		/// <param name="original">The original node</param>
		/// <param name="comparedTo">The node it was compared to.</param>
		/// <param name="nodeChanges">The node changes produced when the original and compared were compared.</param>
		/// <param name="changeType">Type of the change when the original and compared to were compared</param>
		public ChangesInType(IType original, IType comparedTo, IEnumerable<ChangesInSyntaxNode> nodeChanges, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			NodeChanges = nodeChanges;
			ChangeType = changeType;
		}

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by debugger display")]
		private string DisplayString()
		{
			Contract.Ensures(Contract.Result<string>() != null);

			if (Original != null)
				return Original.Name.ToString();

			if (ComparedTo != null)
				ComparedTo.Name.ToString();

			return this.GetType().ToString();
		}
	}
}
