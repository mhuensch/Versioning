using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Run00.Versioning
{
	public interface IProject
	{
		ICompilation GetCompilation();
	}
}
