
namespace Run00.Versioning
{
	public interface ICompilation : IContractItem
	{
		string GetVersion();
		void SetVersion(string value);
	}
}