using Roslyn.Compilers.Common;
using Run00.Versioning.Link;
using System.Diagnostics;
using System.IO;

namespace Run00.Versioning
{
	[DebuggerDisplay("{DisplayString}")]
	public class CommonSyntaxTreeChange
	{
		[DebuggerDisplay("SyntaxTree")]
		public CommonSyntaxTree Original { get; private set; }
		[DebuggerDisplay("SyntaxTree")]
		public CommonSyntaxTree ComparedTo { get; private set; }
		[DebuggerDisplay("Node Change")]
		public CommonSyntaxNodeChange NodeChange { get; private set; }
		public ContractChangeType ChangeType { get; private set; }

		private string DisplayString { get { return Original != null ? Path.GetFileName(Original.FilePath) : Path.GetFileName(ComparedTo.FilePath); } }

		public CommonSyntaxTreeChange(CommonSyntaxTree original, CommonSyntaxTree comparedTo, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			NodeChange = null;
			ChangeType = changeType;
		}

		public CommonSyntaxTreeChange(CommonSyntaxTree original, CommonSyntaxTree comparedTo, CommonSyntaxNodeChange nodeChange, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			NodeChange = nodeChange;
			ChangeType = changeType;
		}
	}
}
