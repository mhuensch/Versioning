using Roslyn.Services;
using Run00.Versioning;
using System.Collections.Generic;
using System.Linq;

namespace Run00.VersioningRoslyn
{
	public class RoslynSolution : ICSharpSolution
	{
		public static ICSharpSolution Load(string filePath)
		{
			return new RoslynSolution(Solution.Load(filePath));
		}

		public RoslynSolution(ISolution solution)
		{
			_solution = solution;
		}

		IEnumerable<ICompilation> ICSharpSolution.Compilations
		{
			get
			{
				return _solution.Projects.Select(p => new RoslynCompilation(p.GetCompilation()));
			}
		}

		private readonly ISolution _solution;
	}
}
