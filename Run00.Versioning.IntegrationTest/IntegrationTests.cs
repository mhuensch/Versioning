using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Compilers.Common;
using Roslyn.Services;
using Run00.MsTest;
using Run00.Utilities;
using Run00.Versioning.Link;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Run00.Versioning.IntegrationTest
{
	[TestClass, CategorizeByConventionClass(typeof(IntegrationTests))]
	[DeploymentItem(@"..\..\Artifacts")]
	public class IntegrationTests
	{
		[TestMethod, CategorizeByConvention]
		public void WhenComparingProjectsWithDeletedItems_ShouldReturnDeletedDiffrences()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Deleted\Test.Sample.sln"));

			var container = new WindsorContainer();
			container.Install(FromAssembly.InDirectory(new AssemblyFilter(Directory.GetCurrentDirectory())));


			var linker = container.Resolve<SolutionChangeCalculator>();

			//Act
			var assemblyLinks = linker.GetChanges(controlGroup, testGroup);
			var symbolLinks = assemblyLinks.First().Changes;
		}

		[TestMethod, CategorizeByConvention]
		public void WhenComparingProjectsWithDeletedItems_ShouldReturnDeletedDiffrences2()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Refactor\Test.Sample.sln"));

			var container = new WindsorContainer();
			container.Install(FromAssembly.InDirectory(new AssemblyFilter(Directory.GetCurrentDirectory())));


			var linker = container.Resolve<SolutionChangeCalculator>();

			//Act
			var assemblyLinks = linker.GetChanges(controlGroup, testGroup);
			var symbolLinks = assemblyLinks.First().Changes;
		}

		[TestMethod, CategorizeByConvention]
		public void RefactorHaHa()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Refactor\Test.Sample.sln"));
			var link = Link(controlGroup, testGroup);

			var result = ContractChangeType.Cosmetic;
			foreach (var l in link)
			{
				if (l.Original == null)
					result = ContractChangeType.Enhancement;

				if (l.CompareTo == null)
					result = ContractChangeType.Breaking;

				var treeLinks = Link(l.Original, l.CompareTo).ToList();
				foreach (var treeLink in treeLinks)
				{
					if (treeLink.Original == null)
						result = ContractChangeType.Enhancement;

					if (treeLink.CompareTo == null)
						result = ContractChangeType.Enhancement;

					if (treeLink.Original.IsEquivalentTo(treeLink.CompareTo))
						result = ContractChangeType.Cosmetic;

					result = ContractChangeType.Refactor;

				}

			}
			//var oAssemblies = controlGroup.Projects.Select(p => p.GetCompilation()).First();
			//var cAssemblies = testGroup.Projects.Select(p => p.GetCompilation()).First();

			//var oTree = oAssemblies.SyntaxTrees.First();
			//var cTree = cAssemblies.SyntaxTrees.First();

		}

		public IEnumerable<CompLink> Link(ISolution original, ISolution compareTo)
		{
			var oAssemblies = original.Projects.Select(p => p.GetCompilation());
			var cAssemblies = compareTo.Projects.Select(p => p.GetCompilation());
			var result = oAssemblies.FullOuterJoin(cAssemblies, (t) => t.Assembly.Name, (o, c) => new CompLink() { Original = o, CompareTo = c });
			return result;
		}

		public IEnumerable<TreeLink> Link(CommonCompilation original, CommonCompilation compareTo)
		{
			var result = original.SyntaxTrees.FullOuterJoin(compareTo.SyntaxTrees, (a, b) => a.IsEquivalentTo(b, true), (o, c) => new TreeLink() { Original = o, CompareTo = c });
			return result;
		}

		public class CompLink
		{
			public CommonCompilation Original { get; set; }
			public CommonCompilation CompareTo { get; set; }
		}

		[DebuggerDisplay("{TypeCompared}")]
		public class TreeLink
		{
			public CommonSyntaxTree Original { get; set; }
			public CommonSyntaxTree CompareTo { get; set; }

			public string TypeCompared
			{
				get
				{
					return Original != null ? Original.FilePath : CompareTo.FilePath;
				}
			}
		}
	}
}
