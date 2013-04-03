
namespace Run00.Versioning.Compare
{
	public class ContractChange
	{
		public ContractChangeType ChangeType { get; private set; }
		public string Reason { get; private set; }

		public ContractChange(ContractChangeType type, string reason)
		{
			ChangeType = type;
			Reason = reason;
		}
	}
}
