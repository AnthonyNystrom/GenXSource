/*
============================================================================
Name        : Aafsocketutils.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Miscellaneous socket utils for gSOAP library usage
============================================================================
*/

#ifndef __AAFSOCKETUTILS_H__
#define __AAFSOCKETUTILS_H__

#include <e32base.h>
#include "stdsoap2.h"

class CAafSocketUtils
{
public:
	/**
	* Open socket
	*/
	static SOAP_SOCKET SocketOpen(struct soap *soap, const char *endpoint, const char *host, int port);
	
	/**
	* Close socket
	*/
	static int SocketClose(struct soap*);

	/**
	* Read from socket
	*/
	static size_t SocketRead(struct soap *soap, char *s, size_t n);

	/**
	* Write to socket
	*/
	static int SocketWrite(struct soap *soap, const char *s, size_t n);
};

#endif // __AAFSOCKETUTILS_H__