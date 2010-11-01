using System;
using System.Collections.Generic;

namespace Genetibase.Network.Sockets.Protocols.Authentication
{
	public sealed class AuthenticationRegistry
	{
	  static AuthenticationRegistry()
	  {
	    RegisterAuthenticationMethod("Basic", typeof(BasicAuthentication));
	  }
	  
	  private static Dictionary<string, Type> _AuthenticationMethods = new Dictionary<string,Type>(StringComparer.InvariantCultureIgnoreCase);
	  	  
	  public static void RegisterAuthenticationMethod(string MethodName, Type AuthenticationType)
	  {
	    if (!AuthenticationType.IsSubclassOf(typeof(AuthenticationBase)))
	    {
	      throw new ArgumentException("AuthenticationType must derive from AuthenticationBase", "AuthenticationType");
	    }
	    if (FindAuthenticationMethod(MethodName) != null)
	    {
	      throw new AlreadyRegisteredAuthenticationMethodException(FindAuthenticationMethod(MethodName).FullName);
	    }
	    else
	    {
	      _AuthenticationMethods.Add(MethodName, AuthenticationType);
	    }
	  }
	  
	  public static void UnregisterAuthenticationMethod(string MethodName)
	  {
	    if (FindAuthenticationMethod(MethodName) != null)
	    {
        _AuthenticationMethods.Remove(MethodName);
	    }
	  }
	  
	  public static Type FindAuthenticationMethod(string MethodName)
	  {
	    if (!_AuthenticationMethods.ContainsKey(MethodName))
	    {
	      return null;
	    }
	    else
	    {
	      return (Type)_AuthenticationMethods[MethodName];
	    }
	  }
	  
	  public static AuthenticationBase CreateAuthenticationMethod(Type AuthenticationMethod)
	  {
	    if (AuthenticationMethod.IsSubclassOf(typeof(AuthenticationBase)))
	    {
	      return (AuthenticationBase)Activator.CreateInstance(AuthenticationMethod);
	    }
	    else
	    {
	      return null;
	    }
	  }
	  
	  public static AuthenticationBase GetAuthenticationMethodHandler(string MethodName)
	  {
	    if (FindAuthenticationMethod(MethodName) != null)
	    {
	      return CreateAuthenticationMethod(FindAuthenticationMethod(MethodName));
	    }
	    return null;
	  }
	}
}
