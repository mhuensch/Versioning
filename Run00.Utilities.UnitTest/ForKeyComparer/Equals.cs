using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;
using Run00.Utilities.UnitTest.Artifacts;
using System.Collections.Generic;

namespace Run00.Utilities.UnitTest.ForKeyComparer
{
	[TestClass, CategorizeByConventionClass(typeof(Equals))]
	public class Equals
	{
		[TestMethod, CategorizeByConvention]
		public void WhenPropertiesComparedAreEqual_ShouldReturnTrue()
		{
			//Arrange
			var comparer = new KeyComparer<TestPoco, int>(f => f.Id) as IEqualityComparer<TestPoco>;
			var one = new TestPoco() { Id = 1 };
			var two = new TestPoco() { Id = 1 };

			//Act
			var result = comparer.Equals(one, two);

			//Assert
			Assert.IsTrue(result);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenPropertiesComparedAreNotEqual_ShouldReturnFalse()
		{
			//Arrange
			var comparer = new KeyComparer<TestPoco, int>(f => f.Id) as IEqualityComparer<TestPoco>;
			var one = new TestPoco() { Id = 1 };
			var two = new TestPoco() { Id = 2 };

			//Act
			var result = comparer.Equals(one, two);

			//Assert
			Assert.IsFalse(result);
		}
	}
}
