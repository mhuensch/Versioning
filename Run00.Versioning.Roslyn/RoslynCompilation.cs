using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynCompilation : ICompilation, IContractItem
	{
		public RoslynCompilation(CommonCompilation compilation)
		{
			Contract.Requires(compilation == null);

			_compilation = compilation;
		}

		IEnumerable<ISyntaxTree> ICompilation.SyntaxTrees
		{
			get
			{
				return _compilation.SyntaxTrees.Select(t => new RoslynSyntaxTree(t));
			}
		}

		IAssembly ICompilation.Assembly
		{
			get
			{
				return new RoslynAssembly(_compilation.Assembly);
			}
		}

		bool IContractItem.IsPrivate { get { return false; } }

		bool IContractItem.IsCodeBlock { get { return false; } }

		IEnumerable<IContractItem> IContractItem.Children
		{
			get
			{
				return _compilation.Assembly.GlobalNamespace.GetTypeMembers().AsEnumerable().Select(n => (IContractItem)new RoslynType(n)).Union(
							_compilation.Assembly.GlobalNamespace.GetNamespaceMembers().AsEnumerable().Select(m => (IContractItem)new RoslynNamespace(m)));
			}
		}

		string IContractItem.Name
		{
			get { return _compilation.GlobalNamespace.Name; }
		}

		private readonly CommonCompilation _compilation;
	}
}
