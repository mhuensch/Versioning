using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;
using System;

namespace Run00.Versioning.Sequence.UnitTest.ForVersioning
{
	[TestClass, CategorizeByConventionClass(typeof(CalculateVersion))]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifactCopy.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifactRevision.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Rename.Method.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Rename.Class.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Rename.Namespace.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Change.Param.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Change.Result.dll")]
	[DeploymentItem("Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Add.Param.dll")]
	public class CalculateVersion
	{
		[TestMethod, CategorizeByConvention]
		public void WhenUsingTheSameAssembly_ShouldReturnIncrementedBuildAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var expected = new Version(1, 0, 1, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, simpleAssemblyPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingMatchingAssemblies_ShouldReturnIncrementedBuildAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var copyAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifactCopy.dll";
			var expected = new Version(1, 0, 1, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, copyAssemblyPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingRevisedAssembiles_ShouldReturnIncrementedBuildAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var copyAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifactRevision.dll";
			var expected = new Version(1, 0, 1, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, copyAssemblyPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingAssembilyWithNamespaceRename_ShouldReturnIncrementedMajorAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var namespacePath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Rename.Namespace.dll";
			var expected = new Version(2, 0, 0, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, namespacePath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingAssemblyWithClassRename_ShouldReturnIncrementedMajorAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var classPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Rename.Class.dll";
			var expected = new Version(2, 0, 0, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, classPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingAssemblyWithMethodRename_ShouldReturnIncrementedMajorAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var methodPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Rename.Method.dll";
			var expected = new Version(2, 0, 0, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, methodPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingAssemblyWithParameterChange_ShouldReturnIncrementedMajorAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var methodPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Change.Param.dll";
			var expected = new Version(2, 0, 0, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, methodPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingAssemblyWithResultChange_ShouldReturnIncrementedMajorAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var methodPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Change.Result.dll";
			var expected = new Version(2, 0, 0, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, methodPath);

			//Assert
			Assert.AreEqual(expected, result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenUsingAssemblyWithParameterAddition_ShouldReturnIncrementedMajorAndRevisionVersion()
		{
			//Arrange
			var simpleAssemblyPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.dll";
			var methodPath = "Run00.Versioning.Sequence.UnitTest.SimpleArtifact.Add.Param.dll";
			var expected = new Version(2, 0, 0, 26);
			var calculator = new VersionCalculator() as IVersionCalculator;

			//Act
			var result = calculator.Calculate(simpleAssemblyPath, methodPath);

			//Assert
			Assert.AreEqual(expected, result);
		}
	}
}
