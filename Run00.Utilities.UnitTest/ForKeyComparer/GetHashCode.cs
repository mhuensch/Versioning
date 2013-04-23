using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;
using Run00.Utilities.UnitTest.Artifacts;
using System.Collections.Generic;

namespace Run00.Utilities.UnitTest.ForKeyComparer
{
	[TestClass, CategorizeByConventionClass(typeof(GetHashCode))]
	public class GetHashCode
	{
		[TestMethod, CategorizeByConvention]
		public void WhenDataGiven_ShouldReturnHashCodeOfProperty()
		{
			//Arrange
			var comparer = new KeyComparer<TestPoco, int>(f => f.Id) as IEqualityComparer<TestPoco>;
			var data = new TestPoco() { Id = 1 };

			//Act
			var result = comparer.GetHashCode(data);

			//Assert
			Assert.AreEqual(data.Id.GetHashCode(), result);
		}
	}
}
