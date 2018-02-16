using System;
using System.Runtime.Serialization;

namespace Dbc
{
	/// <inheritdoc />
	/// <summary>
	///     Exception raised when a precondition fails.
	/// </summary>
	[Serializable]
	public class PreconditionException : DesignByContractException
	{
		/// <inheritdoc />
		/// <summary>
		///     Precondition Exception.
		/// </summary>
		public PreconditionException()
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Precondition Exception.
		/// </summary>
		public PreconditionException(string message)
			: base(message)
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Precondition Exception.
		/// </summary>
		public PreconditionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected PreconditionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}