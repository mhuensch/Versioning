using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Run00.Versioning
{
	public class SuggestedVersion
	{
		/// <summary>
		/// Gets the original.
		/// </summary>
		/// <value>
		/// The original.
		/// </value>
		public Version Original { get; private set; }

		/// <summary>
		/// Gets the suggested.
		/// </summary>
		/// <value>
		/// The suggested.
		/// </value>
		public Version Suggested { get; private set; }


		public ICompilation OriginalComp { get; private set; }

		public ICompilation ComparedToComp { get; private set; }


		/// <summary>
		/// Gets or sets the justification.
		/// </summary>
		/// <value>
		/// The justification.
		/// </value>
		public ContractChanges Justification { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SuggestedVersion"/> class.
		/// </summary>
		/// <param name="original">The original version.</param>
		/// <param name="suggested">The suggested version.</param>
		/// <param name="justification">The justification for the suggested version.</param>
		public SuggestedVersion(Version original, Version suggested, ICompilation originalComp, ICompilation comparedToComp, ContractChanges justification)
		{
			Contract.Requires(original != null);
			Contract.Requires(suggested != null);
			Contract.Requires(originalComp != null);
			Contract.Requires(comparedToComp != null);
			Contract.Requires(justification != null);

			Original = original;
			Suggested = suggested;
			OriginalComp = originalComp;
			ComparedToComp = comparedToComp;
			Justification = justification;
		}
	}
}