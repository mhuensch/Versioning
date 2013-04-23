using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface INamespace
	{
		string Name { get; }
		IEnumerable<INamespace> GetNamespaceMembers();
		IEnumerable<IType> GetTypes();
	}
}