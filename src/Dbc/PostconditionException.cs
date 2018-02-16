using System;
using System.Runtime.Serialization;

namespace Dbc
{
	/// <inheritdoc />
	/// <summary>
	///     Exception raised when a postcondition fails.
	/// </summary>
	[Serializable]
	public class PostconditionException : DesignByContractException
	{
		/// <inheritdoc />
		/// <summary>
		///     Postcondition Exception.
		/// </summary>
		public PostconditionException()
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Postcondition Exception.
		/// </summary>
		public PostconditionException(string message)
			: base(message)
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Postcondition Exception.
		/// </summary>
		public PostconditionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected PostconditionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}