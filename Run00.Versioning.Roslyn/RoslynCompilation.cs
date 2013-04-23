using Roslyn.Compilers.Common;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynCompilation : ICompilation
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

		private readonly CommonCompilation _compilation;

		//IEnumerable<IType> ICompilation.ContractTypes
		//{
		//	get 
		//	{
		//		if (_contractTypes != null)
		//			return _contractTypes;

		//		var result = new List<IType>();

		//		var types = GetContractTypes(_compilation.GlobalNamespace.GetNamespaceMembers());
		//		foreach(var type in types)
		//		{
		//			var node = FindSyntaxNode(type);
		//			if (node == null)
		//				throw new InvalidOperationException("No syntax node found for " + type.Name);

		//			result.Add(new RoslynType(type, node));
		//		}
		//		_contractTypes = result;

		//		return _contractTypes;
		//	}
		//}

		//ISyntaxTree ICompilation.GetAssemblyInfoSyntaxTree()
		//{
		//	var assemblyFiles = _compilation.SyntaxTrees.Where(t => Path.GetFileName(t.FilePath).Equals(_assemblyFileName));
		//	if (assemblyFiles.Count() != 1)
		//		throw new InvalidOperationException("More than one file found with the name: " + _assemblyFileName);

		//	var syntaxTree = assemblyFiles.Single();
		//	Contract.Assume(syntaxTree != null, "Single() can not return a null reference.");

		//	return new RoslynSyntaxTree(syntaxTree);
		//}

		//private ISyntaxNode FindSyntaxNode(INamedTypeSymbol type)
		//{
		//	foreach (var tree in _compilation.SyntaxTrees)
		//	{
		//		var node = FindSyntaxNode(type.Name, tree.GetRoot());
		//		if (node != null)
		//			return node;
		//	}

		//	return null;
		//}

		//private ISyntaxNode FindSyntaxNode(string name, CommonSyntaxNode node)
		//{
		//	var result = new RoslynSyntaxNode(node) as ISyntaxNode;
		//	var nodeName = result.GetIdentifierName();
		//	if (name == nodeName)
		//		return result;

		//	foreach (var child in node.ChildNodes())
		//	{
		//		result = FindSyntaxNode(name, child);
		//		if (result != null)
		//			return result;
		//	}

		//	return null;
		//}

		//private IEnumerable<INamedTypeSymbol> GetContractTypes(IEnumerable<INamespaceSymbol> namespaceSymbols)
		//{
		//	var result = new List<INamedTypeSymbol>();

		//	foreach (var space in namespaceSymbols)
		//	{
		//		if (space == null)
		//			continue;

		//		if (space.ContainingAssembly == null || space.ContainingAssembly.BaseName != _compilation.Assembly.BaseName)
		//			continue;

		//		result.AddRange(space.GetTypeMembers().AsEnumerable());
		//		result.AddRange(GetContractTypes(space.GetNamespaceMembers()));
		//	}

		//	return result.Where(t => IsContractMember(t));
		//}

		//private static bool IsContractMember(ISymbol symbol)
		//{
		//	return symbol.CanBeReferencedByName && (
		//		symbol.DeclaredAccessibility == CommonAccessibility.Public ||
		//		symbol.DeclaredAccessibility == CommonAccessibility.Internal ||
		//		symbol.DeclaredAccessibility == CommonAccessibility.ProtectedOrInternal
		//	);
		//}

		//private const string _assemblyFileName = @"AssemblyInfo.cs";

		//private IEnumerable<IType> _contractTypes;



	}
}
