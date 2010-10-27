/*
============================================================================
Name        : Aafsocketutils.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Miscellaneous socket utils for gSOAP library usage
============================================================================
*/

#include <in_sock.h>
#include "Aafsocketutils.h"
#include "Aafconnectionmanager.h"
#include "Aafprivatequestionsserviceprovider.h"
#include "aafuserquestionsserviceprovider.h"
#include "Aafapputils.h"

SOAP_SOCKET CAafSocketUtils::SocketOpen(struct soap *soap, const char *endpoint, const char *host, int port)
{
	__LOGSTR_TOFILE("CAafSocketUtils::SocketOpen() begins");

	// Set endpoint
	if (endpoint)
		strncpy(soap->endpoint, endpoint, sizeof(soap->endpoint)-1);

	// Get thread entry point data
	MGSoapData* entryPointData = reinterpret_cast<MGSoapData*>(soap->user);

	// Open socket
	entryPointData->GetSocketInstance()->Open(*entryPointData->GetConnectionManager()->GetSocketServ(), KAfInet, KSockStream, KProtocolInetTcp, *entryPointData->GetConnectionManager()->GetConnection());

	RHostResolver hostResolver;

	// Open resolver socket
	User::LeaveIfError(hostResolver.Open(*entryPointData->GetConnectionManager()->GetSocketServ(), KAfInet, KProtocolInetTcp)) ;

	TNameEntry hostAddress;
	TRequestStatus status;

	HBufC* serverName = CAafUtils::StringToDescriptorLC(host);

	// Attempt to resolve name
	hostResolver.GetByName(*serverName, hostAddress, status); 

	CleanupStack::PopAndDestroy(serverName);

	User::WaitForRequest(status);

	// Connect to the specified host
	TInetAddr addrOnPort;

	addrOnPort = hostAddress().iAddr;
	addrOnPort.SetPort((TUint)port);

	entryPointData->GetSocketInstance()->Connect(addrOnPort, status);

	User::WaitForRequest(status);

	__LOGSTR_TOFILE("CAafSocketUtils::SocketOpen() ends");

	if (status.Int() == KErrNone)
		return SOAP_OK;

	return SOAP_ERR;
}

int CAafSocketUtils::SocketClose(struct soap *soap)
{
	__LOGSTR_TOFILE("CAafSocketUtils::SocketClose() begins");

	// Get thread entry point data
	MGSoapData* entryPointData = reinterpret_cast<MGSoapData*>(soap->user);
		
	if (entryPointData->GetSocketInstance())
	{		
		entryPointData->GetSocketInstance()->Close();
	}
	else
	{
		return SOAP_ERR;
	}

	__LOGSTR_TOFILE("CAafSocketUtils::SocketClose() ends");

	return SOAP_OK;
}

size_t CAafSocketUtils::SocketRead(struct soap *soap, char *s, size_t n)
{
	__LOGSTR_TOFILE("CAafSocketUtils::SocketRead() begins");

	// Get thread entry point data
	MGSoapData* entryPointData = reinterpret_cast<MGSoapData*>(soap->user);
	
	// Return value
	size_t bufferLength = 0;

	// Status object
	TRequestStatus status;

	// Initialize memory for income buffer
	TUint8* desBuffer = new TUint8[TInt(n)];

	TPtr8 bufPtr(NULL, 0);	

	bufPtr.Set(desBuffer, 0, TInt(n));

	// Read socket content
	entryPointData->GetSocketInstance()->Read(bufPtr, status);

	User::WaitForRequest(status);
	
	// Convert retrieved buffer to temp char* pointer
	char* tempString = reinterpret_cast<char*>(desBuffer);

	// Find last occurrence of the '>' symbol
	char* lastOccurrence = NULL;
	lastOccurrence = strrchr(tempString, '>');


	if (lastOccurrence)
	{
		// Get position of the last occurrence of the '>' symbol
		int pos = lastOccurrence - tempString;

		// Copy read buffer content to output array
		Mem::Copy(s, tempString, pos+1);

		// Set end null value
		s[pos+1] = '\0';

		delete [] desBuffer;
		desBuffer = NULL;

		// Get read buffer length
		bufferLength = strlen(s);
	}

	__LOGSTR_TOFILE("CAafSocketUtils::SocketRead() ends");

	// In case of success return size of read buffer
	if (status.Int() == KErrNone || status.Int() == KErrEof)
		return bufferLength;

	return 0;
}

int CAafSocketUtils::SocketWrite(struct soap *soap, const char *s, size_t n)
{
	__LOGSTR_TOFILE("CAafSocketUtils::SocketWrite() begins");

	// Get thread entry point data
	MGSoapData* entryPointData = reinterpret_cast<MGSoapData*>(soap->user);
	
	TRequestStatus status;

	// Convert char* to descriptor
	HBufC8* data = CAafUtils::StringToDescriptor8LC(s);

	// Write data to socket
	entryPointData->GetSocketInstance()->Write(*data, status);

	User::WaitForRequest(status);

	CleanupStack::PopAndDestroy(data);

	__LOGSTR_TOFILE("CAafSocketUtils::SocketWrite() ends");

	if (status.Int() == KErrNone)
		return SOAP_OK;

	return SOAP_ERR;
}

// end of file
