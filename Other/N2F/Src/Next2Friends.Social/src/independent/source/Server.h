/*! =================================================================
	\file Server.h
	
	Revision History:
	
	[12.11.2007] 16:55 by Victor Kleschenko
	\changed	Change type of the usernames from char8 to char16.

    ================================================================= */

/*!
@file Server.h
@brief Class Server
*/

#ifndef __SERVER_H__
#define __SERVER_H__

#include "BaseTypes.h"
#include "PacketStream.h"
#include "IServer.h"
#include "IAnswer.h"
#include "NetSystem.h"
#include "VList.h"


class BaseService;

class ServerListener
{
	public:
		virtual void OnServerError() = 0;
};

struct SingleRequest 
{
	BaseService *service;
	IAnswer		*answer;
	uint8		*dataPoint;
	uint32		dataSize;
	int32		requestID;
};

// #define CACHING_ENABLED			//! Enabled caching of network data

//! User class for forming requests to server and parsing answers from it
class Server : public IServer
{
public:

	Server(ServerListener *listener);
	~Server();



	void CancelRequest();


	virtual void OnAnswer(PacketStream *answer);
	virtual void OnError();

	void Update();

	PacketStream *GetStream()
	{
		return writingStream;
	};

	BaseService *GetCurrentReadService()
	{
		if (currentSendingRequest)
		{
			return currentSendingRequest->service;
		}
		return NULL;
	};

	bool StartRequest(BaseService *service, IAnswer *answerCB, int32 reqID);
	bool EndRequest();

private:

	SingleRequest *GetFreeRequest();
	void ReturnFreeRequest(SingleRequest *req);

	NetSystem		*netSystem;	//!< pointer to network system

	PacketStream	*sendingStream;
	PacketStream	*writingStream;

	SingleRequest	*currentSendingRequest;
	SingleRequest	*currentWritingRequest;

	VList			*sendRequestsList;
	VList			*writeRequestsList;
	VList			*freeRequestsList;


	ServerListener	*pListener;
};

#endif // __SERVER_H__
