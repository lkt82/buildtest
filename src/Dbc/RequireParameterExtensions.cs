using System;
using JetBrains.Annotations;

namespace Dbc
{
	public static class RequireParameterExtensions
	{
		[AssertionMethod]
		public static void IsNotNull<T>(this RequireParameter<T> requireParameter)
		{
			Check.Require(requireParameter.Source != null, () => new ArgumentNullException(requireParameter.Name));
		}

		[AssertionMethod]
		public static void IsNotNull<T>(this RequireParameter<T> requireParameter, string message)
		{
			Check.Require(requireParameter.Source != null, () => new ArgumentNullException(requireParameter.Name, message));
		}

		[AssertionMethod]
		public static void IsNotNullOrEmpty(this RequireParameter<string> requireParameter)
		{
			Check.Require(!string.IsNullOrEmpty(requireParameter.Source), () => new ArgumentNullException(requireParameter.Name));
		}

		[AssertionMethod]
		public static void IsNotNullOrEmpty(this RequireParameter<string> requireParameter, string message)
		{
			Check.Require(!string.IsNullOrEmpty(requireParameter.Source), () => new ArgumentNullException(requireParameter.Name, message));
		}
	}
}
