using System;
using System.Runtime.Serialization;

namespace Dbc
{
	/// <inheritdoc />
	/// <summary>
	///     Exception raised when an assertion fails.
	/// </summary>
	[Serializable]
	public class AssertionException : DesignByContractException
	{
		/// <inheritdoc />
		/// <summary>
		///     Assertion Exception.
		/// </summary>
		public AssertionException()
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Assertion Exception.
		/// </summary>
		public AssertionException(string message)
			: base(message)
		{
		}

		/// <inheritdoc />
		/// <summary>
		///     Assertion Exception.
		/// </summary>
		public AssertionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected AssertionException(SerializationInfo info, StreamingContext context)
		{
		}
	}
}