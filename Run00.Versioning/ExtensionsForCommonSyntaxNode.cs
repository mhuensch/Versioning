using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	public static class ExtensionsForCommonSyntaxNode
	{
		/// <summary>
		/// Determines whether this instance [can be matched with] the specified original.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="compareTo">The compare to.</param>
		/// <returns>
		///   <c>true</c> if this instance [can be matched with] the specified original; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanBeMatchedWith(this CommonSyntaxNode original, CommonSyntaxNode compareTo)
		{
			if (original == null || compareTo == null)
				return false;

			if (original.IsEquivalentTo(compareTo, true))
				return true;

			var originalName = GetIdentifierNode((SyntaxNode)original);
			var compareToName = GetIdentifierNode((SyntaxNode)compareTo);
			if (originalName == null || compareToName == null)
				return false;

			return originalName.Equals(compareToName);
		}

		/// <summary>
		/// Determines whether the specified node is private as indicated by its SyntaxKind.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>
		///   <c>true</c> if the specified node is private; otherwise, <c>false</c>.
		/// </returns>
		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification="Roslyn uses an enumeration to indicate syntax type.  As a result, we are forced to use a switch statement that makes this method overly complex.")]
		public static bool IsPrivate(this CommonSyntaxNode node)
		{
			Contract.Requires(node != null);

			switch (((SyntaxNode)node).Kind)
			{
				case SyntaxKind.ClassDeclaration:
					return ((ClassDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.InterfaceDeclaration:
					return ((InterfaceDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.StructDeclaration:
					return ((StructDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.EnumDeclaration:
					return ((EnumDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.DelegateDeclaration:
					return ((DelegateDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.DestructorDeclaration:
					return ((DestructorDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.EventDeclaration:
					return ((EventDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.MethodDeclaration:
					return ((MethodDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
				case SyntaxKind.PropertyDeclaration:
					return ((PropertyDeclarationSyntax)(node)).Modifiers.Any(m => m.Kind == SyntaxKind.PrivateKeyword || m.Kind == SyntaxKind.InternalKeyword);
			}

			return false;
		}

		private static string GetIdentifierNode(SyntaxNode node)
		{
			if (node == null)
				return null;

			object value;
			switch (node.Kind)
			{
				case SyntaxKind.ClassDeclaration:
					value = ((ClassDeclarationSyntax)(node)).Identifier.Value;
					break;
				case SyntaxKind.InterfaceDeclaration:
					value = ((InterfaceDeclarationSyntax)(node)).Identifier.Value;
					break;
				case SyntaxKind.StructDeclaration:
					value = ((StructDeclarationSyntax)(node)).Identifier.Value;
					break;
				case SyntaxKind.EnumDeclaration:
					value = ((EnumDeclarationSyntax)(node)).Identifier.Value;
					break;
				default:
					value = null;
					break;
			}

			if (value == null)
				return null;

			return value.ToString();
		}
	}
}
