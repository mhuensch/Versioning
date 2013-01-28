using System;

namespace Run00.Versioning
{
	public interface IAssemblyVersioning
	{
		Version Calculate(string currentDll, string previousDll);
		string UpdateAssemblyInfo(string fileContents, string version);
	}
}
