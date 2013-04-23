using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;
using Run00.Utilities.UnitTest.Artifacts;
using System.Linq;

namespace Run00.Utilities.UnitTest.ForExtensionsForEnumerableOfT
{
	[TestClass, CategorizeByConventionClass(typeof(FullOuterJoin))]
	public class FullOuterJoin
	{
		[TestMethod, CategorizeByConvention]
		public void WhenDiffrentValuesAreProvidedWithKeySelector_ShouldReturnFullyJoinedList()
		{
			//Arrange
			var right = new TestPoco[] { 
				new TestPoco { Id = 1 },
				new TestPoco { Id = 2 } };
			var left = new TestPoco[] { 
				new TestPoco { Id = 1 },
				new TestPoco { Id = 3 } 
			};


			//Act
			var result = right.FullOuterJoin(left, (t) => t.Id, (r, l) => new { One = r, Two = l });

			//Assert
			Assert.AreEqual(3, result.Count());
			Assert.AreEqual(1, result.ElementAt(0).One.Id);
			Assert.AreEqual(1, result.ElementAt(0).Two.Id);
			Assert.AreEqual(2, result.ElementAt(1).One.Id);
			Assert.AreEqual(null, result.ElementAt(1).Two);
			Assert.AreEqual(null, result.ElementAt(2).One);
			Assert.AreEqual(3, result.ElementAt(2).Two.Id);
		}

		[TestMethod, CategorizeByConvention]
		public void WhenDiffrentValuesAreProvidedAndComparisonGiven_ShouldReturnFullyJoinedList()
		{
			//Arrange
			var right = new TestPoco[] { 
				new TestPoco { Id = 1 },
				new TestPoco { Id = 2 } };
			var left = new TestPoco[] { 
				new TestPoco { Id = 1 },
				new TestPoco { Id = 3 } 
			};


			//Act
			var result = right.FullOuterJoin(left, (r, l) => r.Id == l.Id, (r, l) => new { One = r, Two = l });

			//Assert
			Assert.AreEqual(3, result.Count());
			Assert.AreEqual(1, result.ElementAt(0).One.Id);
			Assert.AreEqual(1, result.ElementAt(0).Two.Id);
			Assert.AreEqual(2, result.ElementAt(1).One.Id);
			Assert.AreEqual(null, result.ElementAt(1).Two);
			Assert.AreEqual(null, result.ElementAt(2).One);
			Assert.AreEqual(3, result.ElementAt(2).Two.Id);
		}
	}
}
