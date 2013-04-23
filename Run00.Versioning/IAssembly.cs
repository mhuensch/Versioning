using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface IAssembly
	{
		string Name { get; }
		INamespace Namespace { get; }
		IEnumerable<IAttribute> GetAttributes();		
	}
}
