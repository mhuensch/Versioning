using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynNamespace : INamespace, IContractItem
	{
		public RoslynNamespace(INamespaceSymbol namespaceSymbol)
		{
			Contract.Requires(namespaceSymbol == null);

			_namespace = namespaceSymbol;
		}

		IEnumerable<INamespace> INamespace.GetNamespaceMembers()
		{
			return _namespace.GetNamespaceMembers().Select(n => new RoslynNamespace(n));
		}

		IEnumerable<IType> INamespace.GetTypes()
		{
			return _namespace.GetTypeMembers().AsEnumerable().Select(m => new RoslynType(m));
			//.Where(m => m as INamespaceOrTypeSymbol != null).Select(m => new RoslynType((INamespaceOrTypeSymbol)m))
		}

		string INamespace.Name
		{
			get
			{
				return _namespace.Name;
			}
		}

		private readonly INamespaceSymbol _namespace;



		bool IContractItem.IsPrivate { get { return false; } }

		bool IContractItem.IsCodeBlock { get { return false; } }

		IEnumerable<IContractItem> IContractItem.Children
		{
			get
			{
				return _namespace.GetTypeMembers().AsEnumerable().Select(n => (IContractItem)new RoslynType(n)).Union(
							 _namespace.GetNamespaceMembers().Select(n => (IContractItem)new RoslynNamespace(n)));
			}
		}

		string IContractItem.Name
		{
			get { return _namespace.Name; }
		}
	}
}
