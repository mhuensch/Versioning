﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	[DebuggerDisplay("{DisplayString()}")]
	public class ChangesInSyntaxNode
	{
		/// <summary>
		/// Gets the syntax node given to the constructor.
		/// </summary>
		/// <value>
		/// The syntax node <see cref="Roslyn.Compilers.Common.CommonSyntaxNode"/>
		/// </value>
		[DebuggerDisplay("Node")]
		public ISyntaxNode Original { get; private set; }

		/// <summary>
		/// Gets the syntax node to compare to the original given to the constructor.
		/// </summary>
		/// <value>
		/// The syntax node <see cref="Roslyn.Compilers.Common.CommonSyntaxNode"/>
		/// </value>
		[DebuggerDisplay("Node")]
		public ISyntaxNode ComparedTo { get; private set; }

		/// <summary>
		/// Gets the syntax node changes between the original syntax node and the compared syntax node
		/// </summary>
		/// <value>
		/// The changes.
		/// </value>
		[DebuggerDisplay("Changes")]
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
		public ChangesInSyntaxNode(ISyntaxNode original, ISyntaxNode comparedTo, ContractChangeType changeType)
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
		public ChangesInSyntaxNode(ISyntaxNode original, ISyntaxNode comparedTo, IEnumerable<ChangesInSyntaxNode> nodeChanges, ContractChangeType changeType)
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
				return Original.Kind.ToString();

			if (ComparedTo != null)
				ComparedTo.Kind.ToString();

			return this.GetType().ToString();
		}
	}
}