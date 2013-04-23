using Roslyn.Services;
using System.Collections.Generic;
using System.Linq;

namespace Run00.Versioning.Roslyn
{
	public class RoslynSolution : ISolution
	{
		public static ISolution Load(string filePath)
		{
			return new RoslynSolution(Solution.Load(filePath));
		}

		public RoslynSolution(global::Roslyn.Services.ISolution solution)
		{
			_solution = solution;
		}

		IEnumerable<IProject> ISolution.Projects
		{
			get
			{
				return _solution.Projects.Select(p => new RoslynProject(p));
			}
		}

		private readonly global::Roslyn.Services.ISolution _solution;
	}
}
