using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Run00.Versioning.Roslyn
{
	public class RoslynProject : IProject
	{
		public RoslynProject(global::Roslyn.Services.IProject project)
		{
			_project = project;
		}

		ICompilation IProject.GetCompilation()
		{
			return new RoslynCompilation(_project.GetCompilation());
		}

		private readonly global::Roslyn.Services.IProject _project;
	}
}
