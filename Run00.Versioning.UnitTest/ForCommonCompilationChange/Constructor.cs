using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;
using Roslyn.Compilers.Common;
using Moq;
using Roslyn.Compilers.CSharp;

namespace Run00.Versioning.UnitTest.ForCommonCompilationChange
{
	[TestClass, CategorizeByConventionClass(typeof(Constructor))]
	public class Constructor
	{
		[TestMethod, CategorizeByConvention]
		public void WhenPassedVaildParameters_ShouldCreateObject()
		{
			//Arrange
			var moqOriginal = new Mock<ICompilation>(MockBehavior.Strict);
			var moqComparedTo = new Mock<ICompilation>(MockBehavior.Strict);
			var changeType = ContractChangeType.Breaking;


			//Act
			var result = new ChangesInCompilation(moqOriginal.Object, moqComparedTo.Object, changeType);

			//Assert
			Assert.IsNotNull(result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPassedOriginalValue_ShouldSetOrignalProperty()
		{
			//Arrange
			var moqOriginal = new Mock<ICompilation>(MockBehavior.Strict);
			var moqComparedTo = new Mock<ICompilation>(MockBehavior.Strict);
			var changeType = ContractChangeType.Breaking;


			//Act
			var result = new ChangesInCompilation(moqOriginal.Object, moqComparedTo.Object, changeType);

			//Assert
			Assert.AreEqual(result.Original, moqOriginal.Object);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPassedComparedToValue_ShouldSetComparedProperty()
		{
			//Arrange
			var moqOriginal = new Mock<ICompilation>(MockBehavior.Strict);
			var moqComparedTo = new Mock<ICompilation>(MockBehavior.Strict);
			var changeType = ContractChangeType.Breaking;

			//Act
			var result = new ChangesInCompilation(moqOriginal.Object, moqComparedTo.Object, changeType);

			//Assert
			Assert.AreEqual(result.ComparedTo, moqComparedTo.Object);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPassedChangeType_ShouldSetChangeTypeProperty()
		{
			//Arrange
			var moqOriginal = new Mock<ICompilation>(MockBehavior.Strict);
			var moqComparedTo = new Mock<ICompilation>(MockBehavior.Strict);
			var changeType = ContractChangeType.Breaking;


			//Act
			var result = new ChangesInCompilation(moqOriginal.Object, moqComparedTo.Object, changeType);

			//Assert
			Assert.AreEqual(result.ChangeType, changeType);
		}
	}
}
