using System;
using System.Runtime.Serialization;

namespace Dbc
{
	/// <inheritdoc />
	/// <summary>
	///     Exception raised when a contract is broken.
	///     Catch this exception type if you wish to differentiate between
	///     any DesignByContract exception and other runtime exceptions.
	/// </summary>
	[Serializable]
	public class DesignByContractException : ApplicationException
	{
		/// <inheritdoc />
		/// <summary>
		///     Initializes a new instance of the <see cref="T:Dbc.DesignByContractException" /> class.
		/// </summary>
		protected DesignByContractException()
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Initializes a new instance of the <see cref="T:Dbc.DesignByContractException" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		protected DesignByContractException(string message)
			: base(message)
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Initializes a new instance of the <see cref="T:Dbc.DesignByContractException" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		protected DesignByContractException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected DesignByContractException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}