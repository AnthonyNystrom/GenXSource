/*! =================================================================
	\file HTTPClient.h
	
	Revision History:
	
	[13.10.2007] 11:41 by Victor Kleschenko
	\add file created		
    ================================================================= */

#ifndef __HTTP_CLIENT_H__
#define __HTTP_CLIENT_H__

#define	USE_HTTP_POST
#undef	USE_HTTP_GET

// Имплементация класса работы с сервером по HTTP протоколу
#include <WinBase.h>
#include <Wininet.h>
#include "BaseTypes.h"
#include "INetClient.h"
#include "PacketStream.h"


class NetSystem;
class CConnection;

//! Class for transferring data with HTTP-protocol
class HTTPClient : public INetClient
{
	enum eConst
	{
		GET_REQUEST_SZ		=	255,
        BUFFER_SZ			=	1024,
		REQUEST_STRING_SZ	=	1024
	};

public:

	HTTPClient(const char8 *gameAddr/*, const char8 *updateAddr*/, NetSystem *pNS);
	virtual ~HTTPClient();

	//virtual bool Init(const char8 *addr);
	virtual bool Request(const uint8 *data, uint32 sz/*, bool isUpdateRequest*/);

	static DWORD WINAPI HTTPClient::Update(LPVOID who);

private:

	bool GETRequest(char8 *url);
	bool POSTRequest(char *url, uint8 *data, int32 size);

	static void IWebCB(void *p);
	static void ReadCB(void *p);

private:

	bool ConnectToServer();
	void ClearHandles();
	// WinMobile Web API variables
	HINTERNET hInternet;
	HINTERNET hRequest;
	HINTERNET hSession;


	CConnection *pConnection;

	PacketStream		response;
	char8				reqString [REQUEST_STRING_SZ];
	char8				*gameServerAddr;
	//char8				*updateServerAddr;

	static NetSystem	*pNetSystem;
	static uint8		buffer [BUFFER_SZ];
	char16 objectNameW[100];

	bool isConnected;
	volatile bool isInRequest;
	volatile bool needToClose;
};

#endif // __HTTP_CLIENT_H__
