using System;
using System.Diagnostics;

namespace Run00.Versioning.Link
{
	[DebuggerDisplay("{DisplayString}")]
	public class SymbolChange
	{
		public ISymbolLink SymbolLink { get; private set; }
		public SymbolChangeType ChangeType { get; private set; }
		public string Reason { get; private set; }

		public string DisplayString
		{
			get
			{
				return SymbolLink.ObjectName + "(" + SymbolLink.SymbolType.Name +") - " + ChangeType + ": " + Reason;
			}
		}

		public SymbolChange(ISymbolLink symbolLink, SymbolChangeType type, string reason)
		{
			SymbolLink = symbolLink;
			ChangeType = type;
			Reason = reason;
		}
	}
}
