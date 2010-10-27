/* ------------------------------------------------
 * MemberServicesStub.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

import java.rmi.*;
import javax.xml.rpc.JAXRPCException;
import javax.xml.namespace.QName;
import javax.microedition.xml.rpc.Operation;
import javax.microedition.xml.rpc.Type;
import javax.microedition.xml.rpc.ComplexType;
import javax.microedition.xml.rpc.Element;

/**
 * @author Alex Nesterov
 */
public class MemberServicesStub implements n2f.sup.webservice.MemberServicesSoap, javax.xml.rpc.Stub
{
    private String[] _propertyNames;
    private Object[] _propertyValues;
    
    public MemberServicesStub()
    {
	_propertyNames = new String[] { ENDPOINT_ADDRESS_PROPERTY };
	_propertyValues = new Object[] {"http://services.next2friends.com/N2FWebservices/memberservices.asmx"};
    }
    
    public void _setProperty(String name, Object value)
    {
	int size = _propertyNames.length;
	
	for (int i = 0; i < size; ++i)
	{
	    if (_propertyNames[i].equals(name))
	    {
		_propertyValues[i] = value;
		return;
	    }
	}
	
	// Need to expand our array for a new property
	String[] newPropNames = new String[size + 1];
	System.arraycopy(_propertyNames, 0, newPropNames, 0, size);
	_propertyNames = newPropNames;
	Object[] newPropValues = new Object[size + 1];
	System.arraycopy(_propertyValues, 0, newPropValues, 0, size);
	_propertyValues = newPropValues;
	
	_propertyNames[size] = name;
	_propertyValues[size] = value;
    }
    
    public Object _getProperty(String name)
    {
	for (int i = 0; i < _propertyNames.length; ++i)
	{
	    if (_propertyNames[i].equals(name))
	    {
		return _propertyValues[i];
	    }
	}
	
	if (ENDPOINT_ADDRESS_PROPERTY.equals(name) || USERNAME_PROPERTY.equals(name) || PASSWORD_PROPERTY.equals(name))
	{
	    return null;
	}
	
	if (SESSION_MAINTAIN_PROPERTY.equals(name))
	{
	    return new Boolean(false);
	}
	
	throw new JAXRPCException("Stub does not recognize property: "+name);
    }
    
    protected void _prepOperation(Operation op)
    {
	for (int i = 0; i < _propertyNames.length; ++i)
	{
	    op.setProperty(_propertyNames[i], _propertyValues[i].toString());
	}
    }
    
    public String getMemberID(String login, String password) throws RemoteException
    {
	// Copy the incoming values into an Object array if needed.
	Object[] inputObject = new Object[2];
	inputObject[0] = login;
	inputObject[1] = password;
	
	Operation op = Operation.newInstance(_qname_GetMemberID, _type_GetMemberID, _type_GetMemberIDResponse);
	_prepOperation(op);
	op.setProperty(Operation.SOAPACTION_URI_PROPERTY, "http://tempuri.org/GetMemberID");
	Object resultObj;
	
	try
	{
	    resultObj = op.invoke(inputObject);
	}
	catch (JAXRPCException e)
	{
	    Throwable cause = e.getLinkedCause();
	    
	    if (cause instanceof RemoteException)
	    {
		throw (RemoteException) cause;
	    }
	    
	    throw e;
	}
	
	String result;
	// Convert the result into the right Java type.
	// Unwrapped return value
	Object getMemberIDResultObj = ((Object[])resultObj)[0];
	result = (String)getMemberIDResultObj;
	
	return result;
    }
    
    protected static final QName _qname_GetMemberID = new QName("http://tempuri.org/", "GetMemberID");
    protected static final QName _qname_GetMemberIDResponse = new QName("http://tempuri.org/", "GetMemberIDResponse");
    protected static final QName _qname_GetMemberIDResult = new QName("http://tempuri.org/", "GetMemberIDResult");
    protected static final QName _qname_NickName = new QName("http://tempuri.org/", "NickName");
    protected static final QName _qname_WebMemberID = new QName("http://tempuri.org/", "WebMemberID");
    protected static final QName _qname_WebPassword = new QName("http://tempuri.org/", "WebPassword");
    protected static final Element _type_GetMemberID;
    protected static final Element _type_GetMemberIDResponse;
    
    static
    {
	Element _type_WebMemberID = new Element(_qname_WebMemberID, Type.STRING, 0, 1, false);
	Element _type_WebPassword = new Element(_qname_WebPassword, Type.STRING, 0, 1, false);
	
	ComplexType _complexType_getEncryptionKey = new ComplexType();
	_complexType_getEncryptionKey.elements = new Element[2];
	_complexType_getEncryptionKey.elements[0] = _type_WebMemberID;
	_complexType_getEncryptionKey.elements[1] = _type_WebPassword;
	
	Element _type_NickName = new Element(_qname_NickName, Type.STRING, 0, 1, false);
	
	ComplexType _complexType_getMemberID = new ComplexType();
	_complexType_getMemberID.elements = new Element[2];
	_complexType_getMemberID.elements[0] = _type_NickName;
	_complexType_getMemberID.elements[1] = _type_WebPassword;
	
	_type_GetMemberID = new Element(_qname_GetMemberID, _complexType_getMemberID);
	
	Element _type_GetMemberIDResult = new Element(_qname_GetMemberIDResult, Type.STRING, 0, 1, false);
	
	ComplexType _complexType_getMemberIDResponse = new ComplexType();
	_complexType_getMemberIDResponse.elements = new Element[1];
	_complexType_getMemberIDResponse.elements[0] = _type_GetMemberIDResult;
	
	_type_GetMemberIDResponse = new Element(_qname_GetMemberIDResponse, _complexType_getMemberIDResponse);
    }
}
