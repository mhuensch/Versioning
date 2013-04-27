using Roslyn.Compilers.CSharp;
using Roslyn.Services.CSharp.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Run00.Versioning
{
	internal static class RoslynReferenceHack
	{
		/// <summary>
		/// This method is here in order to force the inclusion of Roslyn.Compilers.CSharp and Roslyn.Services.CSharp
		/// </summary>
		private static void Include()
		{
			typeof(AliasSymbol).ToString();
			typeof(SyntaxClassifier).ToString();
		}
	}
}
