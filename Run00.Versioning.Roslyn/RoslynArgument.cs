using Roslyn.Compilers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Run00.Versioning.Roslyn
{
	public class RoslynArgument : IArgument
	{
		public RoslynArgument(CommonTypedConstant argument)
		{
			_argument = argument;
		}

		object IArgument.Value
		{
			get 
			{
				return _argument.Value;
			}
		}

		private readonly CommonTypedConstant _argument;
	}
}
