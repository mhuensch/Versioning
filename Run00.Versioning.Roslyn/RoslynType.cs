﻿using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynType : IType
	{
		public RoslynType(INamedTypeSymbol type)
		{
			_type = type;
		}

		string IType.Name
		{
			get { return _type.ToDisplayString(); }
		}

		bool IType.IsContractType
		{
			get
			{
				return _type.CanBeReferencedByName && (
					_type.DeclaredAccessibility == CommonAccessibility.Public ||
					_type.DeclaredAccessibility == CommonAccessibility.Internal ||
					_type.DeclaredAccessibility == CommonAccessibility.ProtectedOrInternal
				);
			}
		}

		IEnumerable<ISyntaxNode> IType.SyntaxNodes
		{
			get
			{
				return _type.DeclaringSyntaxNodes.AsEnumerable().Select(n => new RoslynSyntaxNode(n));
			}
		}

		private readonly INamedTypeSymbol _type;
	}
}