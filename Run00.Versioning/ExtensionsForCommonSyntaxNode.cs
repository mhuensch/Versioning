using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System.Linq;

namespace Run00.Versioning
{
	public static class ExtensionsForCommonSyntaxNode
	{
		public static bool CanBeMatchedWith(this CommonSyntaxNode original, CommonSyntaxNode compareTo)
		{
			if (original.IsEquivalentTo(compareTo, true))
				return true;

			var originalName = GetIdentifierNode((SyntaxNode)original, false);
			var compareToName = GetIdentifierNode((SyntaxNode)compareTo, false);
			if (originalName == null || compareToName == null)
				return false;

			return originalName.Equals(compareToName);
		}

		public static bool IsPrivate(this CommonSyntaxNode node)
		{
			switch (((SyntaxNode)node).Kind)
			{
				case SyntaxKind.ClassDeclaration:
					return ((ClassDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.InterfaceDeclaration:
					return ((InterfaceDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.StructDeclaration:
					return ((StructDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.EnumDeclaration:
					return ((EnumDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.DelegateDeclaration:
					return ((DelegateDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.DestructorDeclaration:
					return ((DestructorDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.EventDeclaration:
					return ((EventDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.MethodDeclaration:
					return ((MethodDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
				case SyntaxKind.PropertyDeclaration:
					return ((PropertyDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword);
			}

			return false;
		}

		private static string GetIdentifierNode(SyntaxNode node, bool examineChildren)
		{
			switch (node.Kind)
			{
				case SyntaxKind.ClassDeclaration:
					return ((ClassDeclarationSyntax)(node)).Identifier.Value.ToString();
				case SyntaxKind.InterfaceDeclaration:
					return ((InterfaceDeclarationSyntax)(node)).Identifier.Value.ToString();
				case SyntaxKind.StructDeclaration:
					return ((StructDeclarationSyntax)(node)).Identifier.Value.ToString();
				case SyntaxKind.EnumDeclaration:
					return ((EnumDeclarationSyntax)(node)).Identifier.Value.ToString();
			}

			foreach (var child in node.ChildNodes())
			{
				var result = GetIdentifierNode(child, examineChildren);
				if (result != null)
					return result;
			}

			return null;
		}
	}
}
