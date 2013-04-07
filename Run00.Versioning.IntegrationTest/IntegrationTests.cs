using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roslyn.Services;
using Run00.MsTest;
using Run00.Versioning.Link;
using System;
using System.IO;
using System.Linq;

namespace Run00.Versioning.IntegrationTest
{
	[TestClass, CategorizeByConventionClass(typeof(IntegrationTests))]
	[DeploymentItem(@"..\..\Artifacts")]
	public class IntegrationTests
	{
		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCodeBlockIsChanged_ShouldBeRefactor()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Refactor\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("1.0.1.0", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Refactor, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenOnlyCommentsAreChanged_ShouldBeCosmetic()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Comments\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("1.0.0.1", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Cosmetic, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsDeleted_ShouldBeBreaking()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Deleted\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("2.0.0.0", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Breaking, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenClassIsAdded_ShouldBeEnhancement()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Adding\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("1.1.0.0", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Enhancement, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenMethodIsModified_ShouldBeBreaking()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Modifying\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("2.0.0.0", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Breaking, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenNamespaceIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Namespace\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("2.0.0.0", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Breaking, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenGenericIsChanged_ShouldBeBreaking()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Generic\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var result = calc.SuggestVersions(controlGroup, testGroup);

			Assert.AreEqual("2.0.0.0", result.Single().Suggested.ToString());
			Assert.AreEqual(ContractChangeType.Breaking, result.Single().Justification.ChangeType);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenChangingVersion_ShouldReturnTrue()
		{
			//Arrange
			var controlGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"ControlGroup\Test.Sample.sln"));
			var testGroup = Solution.Load(Path.Combine(Directory.GetCurrentDirectory(), @"Generic\Test.Sample.sln"));
			var calc = new ContractChangeCalculator();
			var versions = calc.SuggestVersions(controlGroup, testGroup);

			calc.UpdateAssemblyInfo(testGroup, versions);
		}


	}
}
