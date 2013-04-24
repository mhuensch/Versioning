using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynSyntaxNode : ISyntaxNode, IContractItem
	{
		public RoslynSyntaxNode(CommonSyntaxNode node)
		{
			_node = node;
		}

		string ISyntaxNode.Kind
		{
			get
			{
				return ((SyntaxNode)_node).Kind.ToString();
			}
		}

		string ISyntaxNode.ToFullString()
		{
			return _node.ToFullString();
		}

		IEnumerable<ISyntaxNode> ISyntaxNode.ChildNodes()
		{
			return _node.ChildNodes().Select(c => new RoslynSyntaxNode(c));
		}

		bool ISyntaxNode.IsEquivalentTo(ISyntaxNode node)
		{
			return ((ISyntaxNode)this).IsEquivalentTo(node, false);
		}

		bool ISyntaxNode.IsEquivalentTo(ISyntaxNode node, bool topLevel)
		{
			var privateNode = node as RoslynSyntaxNode;
			if (privateNode == null)
				return false;

			return _node.IsEquivalentTo(privateNode._node, topLevel);
		}

		string ISyntaxNode.GetIdentifierName()
		{
			if (_node == null)
				return null;

			object value;
			switch (((SyntaxNode)_node).Kind)
			{
				case SyntaxKind.ClassDeclaration:
					value = ((ClassDeclarationSyntax)(_node)).Identifier.Value;
					break;
				case SyntaxKind.InterfaceDeclaration:
					value = ((InterfaceDeclarationSyntax)(_node)).Identifier.Value;
					break;
				case SyntaxKind.StructDeclaration:
					value = ((StructDeclarationSyntax)(_node)).Identifier.Value;
					break;
				case SyntaxKind.EnumDeclaration:
					value = ((EnumDeclarationSyntax)(_node)).Identifier.Value;
					break;
				default:
					value = null;
					break;
			}

			if (value == null)
				return null;

			return value.ToString();
		}

		bool ISyntaxNode.IsPrivate()
		{
			switch (((SyntaxNode)_node).Kind)
			{
				case SyntaxKind.ClassDeclaration:
					return ((ClassDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.InterfaceDeclaration:
					return ((InterfaceDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.StructDeclaration:
					return ((StructDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.EnumDeclaration:
					return ((EnumDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.DelegateDeclaration:
					return ((DelegateDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.DestructorDeclaration:
					return ((DestructorDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.EventDeclaration:
					return ((EventDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.MethodDeclaration:
					return ((MethodDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.PropertyDeclaration:
					return ((PropertyDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
			}

			return false;
		}

		bool ISyntaxNode.IsBlock()
		{
			return (SyntaxKind)_node.Kind == SyntaxKind.Block;
		}

		private readonly CommonSyntaxNode _node;






		bool IContractItem.IsPrivate
		{
			get
			{
				switch (((SyntaxNode)_node).Kind)
				{
					case SyntaxKind.ClassDeclaration:
						return ((ClassDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.InterfaceDeclaration:
						return ((InterfaceDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.StructDeclaration:
						return ((StructDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.EnumDeclaration:
						return ((EnumDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.DelegateDeclaration:
						return ((DelegateDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.DestructorDeclaration:
						return ((DestructorDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.EventDeclaration:
						return ((EventDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.MethodDeclaration:
						return ((MethodDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
					case SyntaxKind.PropertyDeclaration:
						return ((PropertyDeclarationSyntax)(_node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				}

				return false;
			}
		}

		bool IContractItem.IsCodeBlock
		{
			get { return (SyntaxKind)_node.Kind == SyntaxKind.Block; }
		}

		IEnumerable<IContractItem> IContractItem.Children
		{
			get { return _node.ChildNodes().Select(c => new RoslynSyntaxNode(c)); }
		}

		string IContractItem.Name
		{
			get
			{
				switch (((SyntaxNode)_node).Kind)
				{
					case SyntaxKind.ClassDeclaration:
						return ((ClassDeclarationSyntax)(_node)).Identifier.Value.ToString();
					case SyntaxKind.InterfaceDeclaration:
						return ((InterfaceDeclarationSyntax)(_node)).Identifier.Value.ToString();
					case SyntaxKind.StructDeclaration:
						return ((StructDeclarationSyntax)(_node)).Identifier.Value.ToString();
					case SyntaxKind.EnumDeclaration:
						return ((EnumDeclarationSyntax)(_node)).Identifier.Value.ToString();
					default:
						return Guid.NewGuid().ToString();
				}
			}
		}
	}
}
