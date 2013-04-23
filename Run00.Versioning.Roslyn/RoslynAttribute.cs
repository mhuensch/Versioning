using Roslyn.Compilers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Run00.Versioning.Roslyn
{
	public class RoslynAttribute : IAttribute
	{
		public RoslynAttribute(CommonAttributeData attribute)
		{
			_attribute = attribute;
		}

		string IAttribute.AttributeClass
		{
			get
			{
				return _attribute.AttributeClass.Name;
			}
		}

		IEnumerable<IArgument> IAttribute.ConstructorArguments
		{
			get
			{
				return _attribute.ConstructorArguments.AsEnumerable().Select(a => new RoslynArgument(a));
			}
		}

		private readonly CommonAttributeData _attribute;
	}
}
