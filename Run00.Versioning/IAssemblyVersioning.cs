using System;
using System.IO;

namespace Run00.Versioning
{
	public interface IAssemblyVersioning
	{
		Version Calculate(string currentDll, string previousDll);
		void UpdateAssemblyInfo(Stream assemblyInfoFile, Version version);
	}
}
