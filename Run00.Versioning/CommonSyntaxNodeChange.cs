using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Run00.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Run00.Versioning.Link
{
	[DebuggerDisplay("{DisplayString}")]
	public class CommonSyntaxNodeChange
	{
		[DebuggerDisplay("Node")]
		public CommonSyntaxNode Original { get; private set; }
		[DebuggerDisplay("Node")]
		public CommonSyntaxNode ComparedTo { get; private set; }
		[DebuggerDisplay("Changes")]
		public IEnumerable<CommonSyntaxNodeChange> Children { get; private set; }
		public ContractChangeType ChangeType { get; private set; }

		public string DisplayString { get { return Original != null ? ((SyntaxKind)Original.Kind).ToString() : ((SyntaxKind)ComparedTo.Kind).ToString(); } }

		public CommonSyntaxNodeChange(CommonSyntaxNode original, CommonSyntaxNode comparedTo, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			Children = Enumerable.Empty<CommonSyntaxNodeChange>();
			ChangeType = changeType;
		}

		public CommonSyntaxNodeChange(CommonSyntaxNode original, CommonSyntaxNode comparedTo, IEnumerable<CommonSyntaxNodeChange> nodeChanges, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			Children = nodeChanges;
			ChangeType = changeType;
		}
	}
}
