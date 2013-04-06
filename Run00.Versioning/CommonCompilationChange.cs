using Roslyn.Compilers.Common;
using Run00.Versioning.Link;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Run00.Versioning
{
	[DebuggerDisplay("{DisplayString}")]
	public class CommonCompilationChange
	{
		[DebuggerDisplay("Compilation")]
		public CommonCompilation Original { get; private set; }
		[DebuggerDisplay("Compilation")]
		public CommonCompilation ComparedTo { get; private set; }
		[DebuggerDisplay("Changes")]
		public IEnumerable<CommonSyntaxTreeChange> SyntaxTreeChanges { get; private set; }
		public ContractChangeType ChangeType { get; private set; }

		private string DisplayString { get { return Original != null ? Original.Assembly.Name : ComparedTo.Assembly.Name; } }

		public CommonCompilationChange(CommonCompilation original, CommonCompilation comparedTo, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			SyntaxTreeChanges = Enumerable.Empty<CommonSyntaxTreeChange>();
			ChangeType = changeType;
		}

		public CommonCompilationChange(CommonCompilation original, CommonCompilation comparedTo, IEnumerable<CommonSyntaxTreeChange> treeChanges, ContractChangeType changeType)
		{
			Original = original;
			ComparedTo = comparedTo;
			SyntaxTreeChanges = treeChanges;
			ChangeType = changeType;
		}
	}
}
