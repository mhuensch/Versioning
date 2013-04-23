using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.Versioning
{
	[DebuggerDisplay("{DisplayString()}")]
	public class ChangesInCompilation
	{
		/// <summary>
		/// Gets the original compilation given to the constructor.
		/// </summary>
		/// <value>
		/// The compilation <see cref="Roslyn.Compilers.Common.CommonCompilation"/>
		/// </value>
		[DebuggerDisplay("Compilation")]
		public ICompilation Original { get; private set; }

		/// <summary>
		/// Gets the compilation to compare to the original given to the constructor.
		/// </summary>
		/// <value>
		/// The compilation <see cref="Roslyn.Compilers.Common.CommonCompilation"/>
		/// </value>
		[DebuggerDisplay("Compilation")]
		public ICompilation ComparedTo { get; private set; }

		/// <summary>
		/// Gets the syntax tree changes between the original compilation and the compare to compilation
		/// </summary>
		/// <value>
		/// The syntax tree changes.
		/// </value>
		[DebuggerDisplay("Changes")]
		public IEnumerable<ChangesInType> TypeChanges { get; private set; }

		/// <summary>
		/// Gets the type of the change.
		/// </summary>
		/// <value>
		/// The type of the change.
		/// </value>
		public ContractChangeType ChangeType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangesInCompilation"/> class.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="comparedTo">The compared to.</param>
		/// <param name="changeType">Type of the change.</param>
		public ChangesInCompilation(ICompilation original, ICompilation comparedTo, ContractChangeType changeType)
		{
			Contract.Requires(original != null || comparedTo != null);
			Contract.Ensures(TypeChanges != null);
			Contract.Ensures(Enumerable.Count(TypeChanges) == 0);

			Original = original;
			ComparedTo = comparedTo;
			TypeChanges = Enumerable.Empty<ChangesInType>();
			ChangeType = changeType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangesInCompilation"/> class.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="comparedTo">The compared to.</param>
		/// <param name="typeChanges">The compilation changes produced when the original and compared were compared.</param>
		/// <param name="changeType">Type of the change when the original and compared to were compared</param>
		public ChangesInCompilation(ICompilation original, ICompilation comparedTo, IEnumerable<ChangesInType> typeChanges, ContractChangeType changeType)
		{
			Contract.Requires(original != null || comparedTo != null);
			Contract.Requires(typeChanges != null);

			Original = original;
			ComparedTo = comparedTo;
			TypeChanges = typeChanges;
			ChangeType = changeType;
		}

		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by debugger display")]
		private string DisplayString()
		{
			if (Original != null && Original.Assembly != null)
				return Original.Assembly.Name;

			if (ComparedTo != null && ComparedTo.Assembly != null)
				return ComparedTo.Assembly.Name;

			return this.GetType().ToString();
		}

		[ContractInvariantMethod, ExcludeFromCodeCoverage]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for code contracts.")]
		[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
		private void ObjectInvariant()
		{
			Contract.Invariant(Original != null || ComparedTo != null);
			Contract.Invariant(TypeChanges != null);
		}
	}
}
