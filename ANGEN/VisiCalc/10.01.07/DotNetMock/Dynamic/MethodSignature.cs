using System;
using System.Reflection;
namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Class encapsulates a method signature of a single method.
	/// </summary>
	/// <author>Roman V. Gavrilov</author>
	/// <author>Griffin Caprio</author>
	public class MethodSignature
	{
		private Type[] _paramTypes;
		private string _methodName;
		private Type _returnType;

		#region Constructors
		/// <summary>
		/// Create new instance based on given information.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="returnType">Method return type.</param>
		/// <param name="paramTypes">Parameter types.</param>
		public MethodSignature( string methodName, Type returnType, params Type[] paramTypes )
		{
			_methodName = methodName;
			_returnType = returnType;
			_paramTypes = ( Type[] )paramTypes.Clone( );
		}
		/// <summary>
		/// Create new instance of method signature from MethodInfo.
		/// </summary>
		/// <param name="methodInfo">MethodInfo to use for method signature instantiation.</param>
		public MethodSignature( MethodInfo methodInfo )
		{
			ParameterInfo[] paramInfos = methodInfo.GetParameters( );
			Type[] paramTypes = new Type[paramInfos.Length];
			foreach ( ParameterInfo paramInfo in paramInfos )
			{
				paramTypes[ paramInfo.Position ] = paramInfo.ParameterType;
			}
			_methodName = methodInfo.Name;
			_returnType = methodInfo.ReturnType;
			_paramTypes = paramTypes;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Returns <see cref="DotNetMock.Dynamic.MethodSignature"/>s parameter types.
		/// </summary>
		public Type[] ParamTypes
		{
			get
			{
				return _paramTypes;
			}
		}
		/// <summary>
		/// Returns <see cref="DotNetMock.Dynamic.MethodSignature"/>s method name.
		/// </summary>
		public string MethodName
		{
			get
			{
				return _methodName;
			}
		}
		/// <summary>
		/// Returns <see cref="DotNetMock.Dynamic.MethodSignature"/>s return type.
		/// </summary>
		public Type ReturnType
		{
			get
			{
				return _returnType;
			}
		}
		#endregion

		/// <summary>
		/// Compare two signatures by content.
		/// </summary>
		public override bool Equals( object obj )
		{
			if ( obj == null )
			{
				return false;
			}
			if ( !( obj is MethodSignature ) )
			{
				return false;
			}
			MethodSignature that = ( MethodSignature )obj;
			if ( that.MethodName.CompareTo( this.MethodName ) != 0 )
			{
				return false;
			}
			if ( !that.ReturnType.Equals( this.ReturnType ) )
			{
				return false;
			}
			if ( that.ParamTypes.Length != this.ParamTypes.Length )
			{
				return false;
			}
			for ( int idx = 0; idx < that.ParamTypes.Length; ++idx )
			{
				if ( !that.ParamTypes[ idx ].Equals( this.ParamTypes[ idx ] ) )
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Returns string representation of the instance of <see cref="DotNetMock.Dynamic.MethodSignature"/>
		/// </summary>
		/// <returns>string representation of the instance of <see cref="DotNetMock.Dynamic.MethodSignature"/></returns>
		public override string ToString( )
		{
			string paramTypesString = null;
			foreach ( Type paramType in _paramTypes )
			{
				if ( paramTypesString != null )
				{
					paramTypesString += ", ";
				}
				paramTypesString += paramType.ToString( );
			}
			string str = string.Format( "{0} {1}({2})", _returnType.ToString( ), _methodName, paramTypesString );
			return str;
		}
		/// <summary>
		/// Returns hashcode of base class
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode( )
		{
			return base.GetHashCode( );
		}

	}
}