using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Run00.MsTest;
using Run00.Utilities.UnitTest.Artifacts;

namespace Run00.Utilities.UnitTest.ForKeyComparer
{
	[TestClass, CategorizeByConventionClass(typeof(Constructor))]
	public class Constructor
	{
		[TestMethod, CategorizeByConvention]
		public void WhenFunctionIsProvided_KeyComparerIsCreated()
		{
			//Act
			var comparer = new KeyComparer<TestPoco, int>(f => f.Id);

			//Assert
			Assert.IsNotNull(comparer);
		}
	}
}
