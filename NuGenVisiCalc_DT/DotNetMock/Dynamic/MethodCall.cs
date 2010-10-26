#region License
// Copyright (c) 2004 Griffin Caprio, Roman V. Gavrilov & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Reflection;
#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Method call.
	/// </summary>
	/// <author>Roman V. Gavrilov</author>
	/// <author>Choy Rim</author>
	public class MethodCall 
	{
		#region Private Instance Fields
		private MethodInfo _method;
		private object[]   _arguments;
		#endregion
		/// <summary>
		/// Create new method call instance.
		/// </summary>
		/// <param name="method">
		///  <see cref="MethodInfo"/> of method called
		/// </param>
		/// <param name="arguments">Arguments of the method call.</param>
		public MethodCall(MethodInfo method, params object[] arguments) 
		{
			if (arguments == null) 
			{
				arguments = new object[0];
			}
			int expectedArgumentCount = method.GetParameters().Length;
			if (expectedArgumentCount != arguments.Length) 
			{
				throw new InvalidOperationException(String.Format(
					"Method {0} takes {1} arguments but received {2}.",
					method.Name,
					expectedArgumentCount,
					arguments.Length
					));
			}
			_method = method;
			_arguments = arguments;
		}
		/// <summary>
		/// Name of method or property.
		/// </summary>
		/// <remarks>
		/// In the case of a property, we strip the "get_" or "set_"
		/// prefix.
		/// </remarks>
		public string MethodName 
		{
			get 
			{
				string name = _method.Name;
				if ( IsPropertyAccess )
				{
					return name.Substring(4);
				}
				return name;
			}
		}
		/// <summary>
		/// Are we accessing a property or calling a typical method?
		/// </summary>
		public bool IsPropertyAccess 
		{
			get 
			{
				if ( ! _method.IsSpecialName ) 
				{
					return false;
				}
				string name = _method.Name;
				return name.StartsWith("get_") || name.StartsWith("set_");
			}
		}
		/// <summary>
		/// Check if given and this object represent the same method call.
		/// </summary>
		public override bool Equals(object obj) 
		{
			MethodCall that = obj as MethodCall;
			if ( object.ReferenceEquals(that, null) ) 
			{
				return false;
			}
			if ( that.Method!=this.Method ) 
			{
				return false;
			}
			if (that.Arguments.Length != this.Arguments.Length) 
			{
				return false;
			}
			for (int i = 0; i < that.Arguments.Length; ++i) 
			{
				if (!that.Arguments[i].Equals(this.Arguments[i])) return false;
			}
			return true;
		}

		/// <summary>
		/// Get object's hash code.
		/// </summary>
		/// <returns>Object's hash code.</returns>
		public override int GetHashCode() 
		{
			int hashCode = Method.GetHashCode();
			foreach (object argument in Arguments) 
			{
				hashCode ^= argument.GetHashCode();
			}
			return hashCode;
		}
		/// <summary>
		/// String representation of method call.
		/// </summary>
		/// <returns>
		///  String of the form:
		///   <i>method-name</i>(<i>argument-name</i>=<i>argument-value</i>, ...)
		/// </returns>
		public override string ToString()
		{
			ParameterInfo[] pis = Method.GetParameters();
			string[] argumentTexts = new string[Arguments.Length];
			for ( int i = 0; i<Arguments.Length; ++i ) 
			{
				object argument = Arguments[i];
				string argumentName = pis[i].Name;
				string argumentValue;
				if ( object.ReferenceEquals(argument, null) ) 
				{
					argumentValue = "null";
				}
				else if( argument is string ) 
				{
					argumentValue = "\""+argument.ToString()+"\"";
				}
				else 
				{
					try 
					{
						argumentValue = argument.ToString();
					}
					catch 
					{
						argumentValue = "N/A";
					}
				}
				argumentTexts[i] = String.Format(
					"{0}={1}",
					argumentName,
					argumentValue
					);
			}
			return String.Format(
				"{0}({1})",
				Method.Name,
				String.Join(", ", argumentTexts)
				);
		}

		/// <summary>
		/// <see cref="MethodInfo"/> of method in method call.
		/// </summary>
		public MethodInfo Method { get { return _method; } }
		/// <summary>
		/// Array of all argument values for method call.
		/// </summary>
		public object[] Arguments { get { return _arguments; } }
	}
}
