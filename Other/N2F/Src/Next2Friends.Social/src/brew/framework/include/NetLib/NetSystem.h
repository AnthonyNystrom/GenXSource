#ifndef __NET_SYSTEM_H__
#define __NET_SYSTEM_H__

// Класс собирает запросы в мультизапросы, разбивает мультиответы на ответы,
// владеет информацией о структуре пакета

#include "BaseTypes.h"
#include "Vector.h"
#include "PacketStream.h"
#include "INetClient.h"
#include "IAnswer.h"
#include "Application.h"


#ifdef AEE_SIMULATOR
	#define SIMULATOR_WAIT	
#endif

class IServer;

class NetSystem
{
	enum eConst
	{
#ifdef SIMULATOR_WAIT
		REQUEST_WAITING_TIME	=	1000, //пауза  между отпарвкой пакетов - возможность сокету отработать корректно
#endif

		REQUEST_2_SERVER_AGE	=	100, // TODO: определить интервал
		UPDATE_REQUEST_AGE		=	30000,
		BUFFER_CB_SZ			=	10,
		HEADER_SZ				=	4 /*packetSz*/ + 4 /*transactionId*/ + 4 /*packetId*/ + 8 /*sessionId*/
	};

	struct CallBackObject
	{
		CallBackObject(IAnswer *cb, int32 id)
		:	answerCB		(cb)
		,	transactionId	(id)
		{
		}

		CallBackObject()
		{
		}

		IAnswer	*answerCB;
		int32	transactionId;
	};

public:

	NetSystem(const char8 *gameAddr, IServer *pIS);
	~NetSystem();

	bool StartRequest();
	bool EndRequest(uint8* buffer, int32 dataSize);

	void Update();

	void OnStartRead();
	void OnRead(uint8 *data, int32 sz);
	void OnFinishRead();
	void OnError();

	//PacketStream* GetRequestStream()	{	return &multiRequest;	}

	void SaveAnswerToFile();
	void RestoreAnswerFromFile();

	bool IsLocked()
	{
		return isLock;
	};


	void ForceUpdate();


	//TODO: сделать защищенным
	//int64			sessionId;

	//void	CancelRequest()			{ cancelTransactionID = transactionId;	}
	//void	SetStartID()			{ startTransactionID = transactionId;	}

protected:

	bool PrepareHeader(int32 packetId, int32 packetSz);

	void ParseAnswer();

	PacketStream	multiAnswer;

	uint32			requestTime;

#ifdef SIMULATOR_WAIT
	uint32			lastTime;	//	время последнего запроса
#endif

	INetClient		*pINetClient;
	IServer			*pIServer;

	int32 sendBufferSize;
	uint8 *pSendBuffer;

	//int32			transactionId;
	//int32			cancelTransactionID;
	//int32			startTransactionID;

	//Vector<CallBackObject>	bufferCB;

	bool			isLock;
	bool			isReaded;

	int32			packetSzPos;

	bool			forceUpdate;
};

#endif // __NET_SYSTEM_H__