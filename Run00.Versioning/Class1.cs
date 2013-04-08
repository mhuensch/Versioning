using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;

namespace Run00.Versioning
{
	public static class Class1
	{
		public static bool CanBeMatchedWith(this CommonSyntaxTree original, CommonSyntaxTree compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			if (original.IsEquivalentTo(compareTo, true))
				return true;

			var originalName = GetIdentifierNode((SyntaxNode)original.GetRoot(), true);
			var compareToName = GetIdentifierNode((SyntaxNode)compareTo.GetRoot(), true);
			if (originalName == null || compareToName == null)
				return false;

			return originalName.Equals(compareToName);
		}

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
				//case SyntaxKind.DelegateDeclaration:
				//	return ((DelegateDeclarationSyntax)(node)).Identifier.Value.ToString();
				//case SyntaxKind.DestructorDeclaration:
				//	return ((DestructorDeclarationSyntax)(node)).Identifier.Value.ToString();
				//case SyntaxKind.EnumMemberDeclaration:
				//	return ((EnumMemberDeclarationSyntax)(node)).Identifier.Value.ToString();
				//case SyntaxKind.EventDeclaration:
				//	return ((EventDeclarationSyntax)(node)).Identifier.Value.ToString();
				//case SyntaxKind.MethodDeclaration:
				//	return ((MethodDeclarationSyntax)(node)).Identifier.Value.ToString();
				//case SyntaxKind.PropertyDeclaration:
				//	return ((PropertyDeclarationSyntax)(node)).Identifier.Value.ToString();
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
