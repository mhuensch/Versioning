using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Services;
using Run00.MsTest;
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


	}
}
