using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Run00.Mock;
using Run00.MsTest;
using System;
using System.IO;
using System.Text;

namespace Run00.Versioning.Sequence.UnitTest.ForAssemblyVersioning
{
	[TestClass, CategorizeByConventionClass(typeof(UpdateAssemblyInfo))]
	public class UpdateAssemblyInfo
	{
		[TestMethod, CategorizeByConvention]
		public void WhenGivenInfoContent_ShouldUpdateAssemblyVersion()
		{
			//Arrange
			var expected = @"[assembly: AssemblyFileVersion(""1.0.0.0"")] [assembly: AssemblyVersion(""1.2.3.4"")]";
			var version = new Version("1.2.3.4");
			var bytes = Encoding.UTF8.GetBytes(@"[assembly: AssemblyFileVersion(""1.0.0.0"")] [assembly: AssemblyVersion(""1.0.0.0"")]");

			var stream = new MemoryStream();
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();

			var calculatorCreate = Create<AssemblyVersioning>.WithMocks();
			var calculator = calculatorCreate.Target as IAssemblyVersioning;

			//Act
			calculator.UpdateAssemblyInfo(stream, version);

			//Assert
			stream.Position = 0;
			var sr = new StreamReader(stream);
			var result = sr.ReadToEnd();
			Assert.AreEqual(expected, result);
		}
	}
}
