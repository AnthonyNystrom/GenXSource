/* ------------------------------------------------
 * PhotoOrganiseSoapStub.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

import java.rmi.RemoteException;
import javax.xml.rpc.JAXRPCException;
import javax.xml.rpc.Stub;
import javax.xml.namespace.QName;
import javax.microedition.xml.rpc.Operation;
import javax.microedition.xml.rpc.Type;
import javax.microedition.xml.rpc.ComplexType;
import javax.microedition.xml.rpc.Element;

/**
 * @author Alex Nesterov
 */
public class PhotoOrganiseSoapStub
	implements PhotoOrganiseSoap, Stub
{
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
	
	if (ENDPOINT_ADDRESS_PROPERTY.equals(name)
	|| USERNAME_PROPERTY.equals(name)
	|| PASSWORD_PROPERTY.equals(name))
	{
	    return null;
	}
	
	if (SESSION_MAINTAIN_PROPERTY.equals(name))
	{
	    return new Boolean(false);
	}
	
	throw new JAXRPCException("Stub does not recognize property: " + name);
    }
    
    protected void _prepOperation(Operation op)
    {
	for (int i = 0; i < _propertyNames.length; ++i)
	{
	    op.setProperty(_propertyNames[i], _propertyValues[i].toString());
	}
    }
    
    public DeviceUploadPhotoResponse deviceUploadPhoto(
	    String webMemberID
	    , String webPassword
	    , String base64StringPhoto
	    , String dateTime) throws RemoteException
    {
	// Copy the incoming values into an Object array if needed.
	Object[] inputObject = new Object[4];
	inputObject[0] = webMemberID;
	inputObject[1] = webPassword;
	inputObject[2] = base64StringPhoto;
	inputObject[3] = dateTime;
	
	Operation op = Operation.newInstance(
		_qname_DeviceUploadPhoto
		, _type_DeviceUploadPhoto
		, _type_DeviceUploadPhotoResponse
		);
	
	_prepOperation(op);
	op.setProperty(Operation.SOAPACTION_URI_PROPERTY, "http://tempuri.org/DeviceUploadPhoto");
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
		throw (RemoteException)cause;
	    }
	    
	    throw e;
	}
	
	DeviceUploadPhotoResponse result;
	// Convert the result into the right Java type.
	if (resultObj == null)
	{
	    result = null;
	}
	else
	{
	    result = new DeviceUploadPhotoResponse();
	}
	
	return result;
    }
    
    protected static final QName _qname_Base64StringPhoto = new QName("http://tempuri.org/", "Base64StringPhoto");
    protected static final QName _qname_DateTime = new QName("http://tempuri.org/", "DateTime");
    protected static final QName _qname_DeviceUploadPhoto = new QName("http://tempuri.org/", "DeviceUploadPhoto");
    protected static final QName _qname_DeviceUploadPhotoResponse = new QName("http://tempuri.org/", "DeviceUploadPhotoResponse");
    protected static final QName _qname_WebMemberID = new QName("http://tempuri.org/", "WebMemberID");
    protected static final QName _qname_WebPassword = new QName("http://tempuri.org/", "WebPassword");
    
    protected static final Element _type_DeviceUploadPhoto;
    protected static final Element _type_DeviceUploadPhotoResponse;
    
    static
    {
	Element _type_WebMemberID = new Element(_qname_WebMemberID, Type.STRING, 0, 1, false);
	Element _type_WebPassword = new Element(_qname_WebPassword, Type.STRING, 0, 1, false);
	Element _type_Base64StringPhoto = new Element(_qname_Base64StringPhoto, Type.STRING, 0, 1, false);
	Element _type_DateTime = new Element(_qname_DateTime, Type.STRING, 0, 1, false);
	
	ComplexType _complexType_getCollections = new ComplexType();
	_complexType_getCollections.elements = new Element[0];
	
	ComplexType _complexType_deviceUploadPhoto = new ComplexType();
	_complexType_deviceUploadPhoto.elements = new Element[4];
	_complexType_deviceUploadPhoto.elements[0] = _type_WebMemberID;
	_complexType_deviceUploadPhoto.elements[1] = _type_WebPassword;
	_complexType_deviceUploadPhoto.elements[2] = _type_Base64StringPhoto;
	_complexType_deviceUploadPhoto.elements[3] = _type_DateTime;
	
	_type_DeviceUploadPhoto = new Element(_qname_DeviceUploadPhoto, _complexType_deviceUploadPhoto);
	_type_DeviceUploadPhotoResponse = new Element(_qname_DeviceUploadPhotoResponse, _complexType_getCollections);
    }
    
    private String[] _propertyNames;
    private Object[] _propertyValues;
    
    public PhotoOrganiseSoapStub()
    {
	_propertyNames = new String[] { ENDPOINT_ADDRESS_PROPERTY };
	_propertyValues = new Object[] { "http://services.next2friends.com/N2FWebservices/photoorganise.asmx" };
    }
}
