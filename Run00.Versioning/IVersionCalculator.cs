using System;

namespace Run00.Versioning
{
	public interface IVersionCalculator
	{
		Version Calculate(string currentDll, string previousDll);
	}
}
