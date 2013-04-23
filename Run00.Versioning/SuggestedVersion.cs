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
		[DebuggerDisplay("Compilation")]
		public Version Original { get; private set; }

		/// <summary>
		/// Gets the suggested.
		/// </summary>
		/// <value>
		/// The suggested.
		/// </value>
		[DebuggerDisplay("Compilation")]
		public Version Suggested { get; private set; }

		/// <summary>
		/// Gets or sets the justification.
		/// </summary>
		/// <value>
		/// The justification.
		/// </value>
		[DebuggerDisplay("Changes")]
		public ChangesInCompilation Justification { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SuggestedVersion"/> class.
		/// </summary>
		/// <param name="original">The original version.</param>
		/// <param name="suggested">The suggested version.</param>
		/// <param name="justification">The justification for the suggested version.</param>
		public SuggestedVersion(Version original, Version suggested, ChangesInCompilation justification)
		{
			Contract.Requires(original != null);
			Contract.Requires(suggested != null);
			Contract.Requires(justification != null);

			Original = original;
			Suggested = suggested;
			Justification = justification;
		}
	}
}
