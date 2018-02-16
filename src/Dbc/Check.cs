using System;
using System.Diagnostics;
using System.Globalization;
using JetBrains.Annotations;

namespace Dbc
{
	/// <summary>
	///     Design By Contract Checks.
	///     Each method generates an exception if the contract is broken.
	/// </summary>
	/// <remarks>
	///     This example shows how to call the Require method.
	///     <code>
	///  public void Test(int x)
	///  {
	///  	try
	///  	{
	/// 			Check.Require(x > 1, "x must be > 1");
	/// 		}
	/// 		catch (System.Exception ex)
	/// 		{
	/// 			Console.WriteLine(ex.ToString());
	/// 		}
	/// 	}
	///  </code>
	///     Contracts and Inheritance
	///     There are certain rules that should be adhered to when Design by Contract is used with inheritance. Eiffel has
	///     language support to enforce these but in C# we must rely on the programmer. The rules are, in a derived class
	///     (Meyer, Chapter 16):
	///     1. An overriding method may [only] weaken the precondition. This means that the overriding precondition should be
	///     logically "or-ed" with the overridden precondition.
	///     2. An overriding method may [only] strengthen the postcondition. This means that the overriding postcondition
	///     should be logically "and-ed" with the overridden postcondition.
	///     3. A derived class invariant should be logically "and-ed" with its base class invariant.
	/// </remarks>
	public static class Check
	{
		public delegate void FormatParameters(string format, params object[] args);

		private const string _require = "Precondition failed.";
		private const string _ensure = "Postcondition failed.";
		private const string _invariant = "Invariant failed.";
		private const string _assert = "Assertion failed.";


		private static readonly IFormatProvider _defaultformatProvider = CultureInfo.InvariantCulture;


		public static IFormatProvider FormatProvider { get; set; } = _defaultformatProvider;

		public static void ResetFormatProvider()
		{
			FormatProvider = _defaultformatProvider;
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message)
		{
			if (!assertion)
			{
				ThrowPrecondition(message);
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string format,
			params object[] args)
		{
			if (!assertion)
			{
				ThrowPrecondition(format, args);
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, Func<string> message)
		{
			if (!assertion)
			{
				ThrowPrecondition(message());
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<FormatParameters> action)
		{
			if (!assertion)
			{
				action(ThrowPrecondition);
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message,
			Exception inner)
		{
			if (!assertion)
			{
				ThrowPrecondition(message, inner);
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<Action<string, Exception>> action)
		{
			if (!assertion)
			{
				action(ThrowPrecondition);
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
		{
			if (!assertion)
			{
				ThrowPrecondition(_require);
			}
		}


		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
			where TException : Exception,new()
		{
			if (!assertion)
			{
				throw new TException();
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Require<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Func<TException> exception) where TException : Exception
		{
			if (!assertion)
			{
				throw exception();
			}
		}

		/// <summary>
		///     Precondition check.
		/// </summary>
		[DebuggerStepThrough]
		public static RequireParameter<T> Require<T>(T parameter, string paramName)
		{
			return new RequireParameter<T> { Source = parameter, Name = paramName };
		}

		private static void ThrowPrecondition(string message)
		{
			throw new PreconditionException(message);
		}

		private static void ThrowPrecondition(string format, params object[] args)
		{
			throw new PreconditionException(string.Format(FormatProvider, format, args));
		}

		private static void ThrowPrecondition(string message, Exception inner)
		{
			throw new PreconditionException(message, inner);
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message)
		{
			if (!assertion)
			{
				ThrowPostcondition(message);
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string format,
			params object[] args)
		{
			if (!assertion)
			{
				ThrowPostcondition(format, args);
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, Func<string> message)
		{
			if (!assertion)
			{
				ThrowPostcondition(message());
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<FormatParameters> action)
		{
			if (!assertion)
			{
				action(ThrowPostcondition);
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message,
			Exception inner)
		{
			if (!assertion)
			{
				ThrowPostcondition(message, inner);
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<Action<string, Exception>> action)
		{
			if (!assertion)
			{
				action(ThrowPostcondition);
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
		{
			if (!assertion)
			{
				ThrowPostcondition(_ensure);
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
			where TException : Exception,new()
		{
			if (!assertion)
			{
				throw new TException();
			}
		}

		/// <summary>
		///     Postcondition check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Ensure<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Func<TException> exception) where TException : Exception
		{
			if (!assertion)
			{
				throw exception();
			}
		}

		private static void ThrowPostcondition(string message)
		{
			throw new PostconditionException(message);
		}

		private static void ThrowPostcondition(string format, params object[] args)
		{
			throw new PostconditionException(string.Format(FormatProvider, format, args));
		}

		private static void ThrowPostcondition(string message, Exception inner)
		{
			throw new PostconditionException(message, inner);
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message)
		{
			if (!assertion)
			{
				ThrowInvariant(message);
			}
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string format,
			params object[] args)
		{
			if (!assertion)
			{
				ThrowInvariant(format, args);
			}
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, Func<string> message)
		{
			if (!assertion)
			{
				ThrowInvariant(message());
			}
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<FormatParameters> action)
		{
			if (!assertion)
			{
				action(ThrowInvariant);
			}
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message,
			Exception inner)
		{
			if (!assertion)
			{
				ThrowInvariant(message, inner);
			}
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<Action<string, Exception>> action)
		{
			if (!assertion)
			{
				action(ThrowInvariant);
			}
		}

		/// <summary>
		///     Invariant check. Used for checking members variables
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
		{
			if (!assertion)
			{
				ThrowInvariant(_invariant);
			}
		}

		/// <summary>
		///     Invariant check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
			where TException : Exception,new()
		{
			if (!assertion)
			{
				throw new TException();
			}
		}

		/// <summary>
		///     Invariant check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Invariant<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Func<TException> exception) where TException : Exception
		{
			if (!assertion)
			{
				throw exception();
			}
		}

		private static void ThrowInvariant(string message)
		{
			throw new InvariantException(message);
		}

		private static void ThrowInvariant(string format, params object[] args)
		{
			throw new InvariantException(string.Format(FormatProvider, format, args));
		}

		private static void ThrowInvariant(string message, Exception inner)
		{
			throw new InvariantException(message, inner);
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message)
		{
			if (!assertion)
			{
				ThrowAssertion(message);
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string format,
			params object[] args)
		{
			if (!assertion)
			{
				ThrowAssertion(format, args);
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, Func<string> message)
		{
			if (!assertion)
			{
				ThrowAssertion(message());
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<FormatParameters> action)
		{
			if (!assertion)
			{
				action(ThrowAssertion);
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion, string message,
			Exception inner)
		{
			if (!assertion)
			{
				ThrowAssertion(message, inner);
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Action<Action<string, Exception>> action)
		{
			if (!assertion)
			{
				action(ThrowAssertion);
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
		{
			if (!assertion)
			{
				ThrowAssertion(_assert);
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion)
			where TException : Exception,new()
		{
			if (!assertion)
			{
				throw new TException();
			}
		}

		/// <summary>
		///     Assertion check.
		/// </summary>
		[DebuggerStepThrough]
		[AssertionMethod]
		[ContractAnnotation("halt <= assertion:false")]
		public static void Assert<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)] bool assertion,
			Func<TException> exception) where TException : Exception
		{
			if (!assertion)
			{
				throw exception();
			}
		}

		private static void ThrowAssertion(string message)
		{
			throw new AssertionException(message);
		}

		private static void ThrowAssertion(string format, params object[] args)
		{
			throw new AssertionException(string.Format(FormatProvider, format, args));
		}

		private static void ThrowAssertion(string message, Exception inner)
		{
			throw new AssertionException(message, inner);
		}
	}
}