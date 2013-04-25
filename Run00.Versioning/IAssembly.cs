using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface IAssembly
	{
		string Name { get; }
		IEnumerable<IAttribute> GetAttributes();		
	}
}
