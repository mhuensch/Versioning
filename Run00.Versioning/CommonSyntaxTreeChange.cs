using Roslyn.Compilers.Common;
using Run00.Versioning.Link;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Run00.Versioning
{
	[DebuggerDisplay("{DisplayString()}")]
	public class CommonSyntaxTreeChange
	{
		/// <summary>
		/// Gets the original syntax tree passed to the constructor
		/// </summary>
		/// <value>
		/// The original.
		/// </value>
		[DebuggerDisplay("SyntaxTree")]
		public CommonSyntaxTree Original { get; private set; }

		/// <summary>
		/// Gets the compared to syntax tree passed to the constructor
		/// </summary>
		/// <value>
		/// The compared to.
		/// </value>
		[DebuggerDisplay("SyntaxTree")]
		public CommonSyntaxTree ComparedTo { get; private set; }

		/// <summary>
		/// Gets the node change passed to the constructor
		/// </summary>
		/// <value>
		/// The node change.
		/// </value>
		[DebuggerDisplay("Node Change")]
		public CommonSyntaxNodeChange NodeChange { get; private set; }

		/// <summary>
		/// Gets the type of the change passed to the constructor
		/// </summary>
		/// <value>
		/// The type of the change.
		/// </value>
		public ContractChangeType ChangeType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CommonSyntaxTreeChange"/> class.
		/// </summary>
		/// <param name="original">The original syntax tree that was compared</param>
		/// <param name="comparedTo">The syntax tree that the original was compared to</param>
		/// <param name="changeType">Type of the change when the original and comparedTo were compared.</param>
		public CommonSyntaxTreeChange(CommonSyntaxTree original, CommonSyntaxTree comparedTo, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			NodeChange = null;
			ChangeType = changeType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommonSyntaxTreeChange"/> class.
		/// </summary>
		/// <param name="original">The original syntax tree that was compared</param>
		/// <param name="comparedTo">The syntax tree that the original was compared to</param>
		/// <param name="nodeChange">The node change created when the original and comparedTo were compared.</param>
		/// <param name="changeType">Type of the change when the original and comparedTo were compared.</param>
		public CommonSyntaxTreeChange(CommonSyntaxTree original, CommonSyntaxTree comparedTo, CommonSyntaxNodeChange nodeChange, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			NodeChange = nodeChange;
			ChangeType = changeType;
		}

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by debugger display")]
		private string DisplayString()
		{
			if (Original != null && string.IsNullOrWhiteSpace(Original.FilePath) == false)
				return Path.GetFileName(Original.FilePath);

			if (ComparedTo != null && string.IsNullOrWhiteSpace(ComparedTo.FilePath) == false)
				return Path.GetFileName(ComparedTo.FilePath);

			return this.GetType().ToString();
		}
	}
}
