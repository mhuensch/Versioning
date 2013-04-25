using System;
using System.Collections.Generic;

namespace Run00.Versioning
{
	public interface IAttribute
	{
		string AttributeClass { get; }
		IEnumerable<object> ConstructorArguments { get; }
	}
}