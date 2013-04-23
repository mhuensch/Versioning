using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynAssembly : IAssembly
	{
		public RoslynAssembly(IAssemblySymbol assembly)
		{
			_assembly = assembly;
		}

		string IAssembly.Name
		{
			get
			{
				return _assembly.Name;
			}
		}

		INamespace IAssembly.Namespace
		{
			get
			{
				return new RoslynNamespace(_assembly.GlobalNamespace);
			}
		}

		IEnumerable<IAttribute> IAssembly.GetAttributes()
		{
			return _assembly.GetAttributes().AsEnumerable().Select(a => new RoslynAttribute(a));
		}

		private readonly IAssemblySymbol _assembly;
	}
}
