using System;
using System.Runtime.Serialization;

namespace Dbc
{
	/// <inheritdoc />
	/// <summary>
	///     Exception raised when an invariant fails.
	/// </summary>
	[Serializable]
	public class InvariantException : DesignByContractException
	{
		/// <inheritdoc />
		/// <summary>
		///     Invariant Exception.
		/// </summary>
		public InvariantException()
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Invariant Exception.
		/// </summary>
		public InvariantException(string message)
			: base(message)
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Invariant Exception.
		/// </summary>
		public InvariantException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvariantException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}