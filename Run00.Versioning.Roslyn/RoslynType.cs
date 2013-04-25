using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynType : IContractItem
	{
		public RoslynType(INamedTypeSymbol type)
		{
			_type = type;
		}

		private readonly INamedTypeSymbol _type;



		bool IContractItem.IsPrivate
		{
			get
			{
				return _type.DeclaredAccessibility == CommonAccessibility.Private
					|| _type.DeclaredAccessibility == CommonAccessibility.Internal
					|| _type.DeclaredAccessibility == CommonAccessibility.ProtectedAndInternal;
			}
		}

		bool IContractItem.IsCodeBlock { get { return false; } }

		IEnumerable<IContractItem> IContractItem.Children
		{
			get
			{
				return _type.DeclaringSyntaxNodes.AsEnumerable().Select(n => new RoslynSyntaxNode(n));
			}
		}

		string IContractItem.Name
		{
			get { return _type.ToDisplayString(); }
		}
	}
}
