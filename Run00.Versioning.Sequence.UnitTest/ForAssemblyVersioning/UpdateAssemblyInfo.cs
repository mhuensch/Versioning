using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;

namespace Run00.Versioning.Sequence.UnitTest.ForAssemblyVersioning
{
	[TestClass, CategorizeByConventionClass(typeof(UpdateAssemblyInfo))]
	public class UpdateAssemblyInfo
	{
		[TestMethod, CategorizeByConvention]
		public void WhenGivenInfoContent_ShouldUpdateAssemblyVersion()
		{
			//Arrange
			var testContent = @"[assembly: AssemblyFileVersion(""1.0.0.0"")] [assembly: AssemblyVersion(""1.0.0.0"")]";
			var expected = @"[assembly: AssemblyFileVersion(""1.0.0.0"")] [assembly: AssemblyVersion(""1.2.3.4"")]";
			var version = "1.2.3.4";
			var calculator = new AssemblyVersioning() as IAssemblyVersioning;

			//Act
			var result = calculator.UpdateAssemblyInfo(testContent, version);

			//Assert
			Assert.AreEqual(expected, result);
		}
	}
}
