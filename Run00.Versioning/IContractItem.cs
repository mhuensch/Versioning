
using System.Collections.Generic;
namespace Run00.Versioning
{
	public interface IContractItem
	{
		string Name { get; }
		bool IsPrivate { get; }
		bool IsCodeBlock { get; }
		IEnumerable<IContractItem> Children { get; }
		bool IsMatchedWith(IContractItem item);
	}
}