#include "HTTPClient.h"
#include "NetSystem.h"
#include "Utils.h"
#include "IServer.h"

NetSystem::NetSystem( const char8 *gameAddr, IServer *pIS ) : 
	pIServer		(pIS)
,	isLock			(false)
,	isReaded		(false)
,	packetSzPos		(-1)
,	forceUpdate		(false)
{
	pINetClient = new HTTPClient(gameAddr, this);

#ifdef SIMULATOR_WAIT
	lastTime	=	Utils::GetTime();
#endif
}

NetSystem::~NetSystem()
{
	SAFE_DELETE(pINetClient);
}

bool NetSystem::StartRequest()
{
	//packetSzPos = multiRequest.GetTail();

	return true;
}


bool NetSystem::EndRequest( uint8* buffer, int32 dataSize )
{

	pSendBuffer = buffer;
	sendBufferSize = dataSize;
	return true;
}

void NetSystem::Update()
{
	uint32 curTime = Utils::GetTime();

	//////////////////////////////////////////////////////////////////////////
#ifdef SIMULATOR_WAIT
	
	if(curTime - lastTime < REQUEST_WAITING_TIME)
	{
		return;
	}

#endif

	//////////////////////////////////////////////////////////////////////////

	uint32 playbackState = GetApplication()->GetPlaybackState();

	if (playbackState == Application::EPS_READ)
	{
		RestoreAnswerFromFile();
	}

	if (playbackState == Application::EPS_WRITE)
	{
		SaveAnswerToFile();
	}


	if (isReaded)
	{
		ParseAnswer();
		isReaded = false;
#ifdef SIMULATOR_WAIT
		lastTime = curTime;
		return;
#endif
	}

	if (isLock)
		return;

//	if	(	(curTime - requestTime > UPDATE_REQUEST_AGE || forceUpdate)
//		&&	sessionId
//		&&	!multiRequest.GetDataSize()	)
//	{
//		EmptyRequest(EPID_UPDATE, NULL);
//
//		requestTime		=	curTime;
//
//		isLock			=	true;
//		isReaded		=	false;
//
//		forceUpdate		=	false;
//
//
//		if (playbackState != Application::EPS_READ)
//		{
//			pINetClient->Request(multiRequest.GetDataBuffer(), multiRequest.GetDataSize(), true);
//		}
//		multiRequest.Reset();
//
//#ifdef SIMULATOR_WAIT
//		lastTime = curTime;
//#endif
//	}

	if (sendBufferSize > 0)
	{
		//if (curTime - requestTime > REQUEST_2_SERVER_AGE)
		{
			//update to all packets
			//if(sessionId)
			//{
			//	EmptyRequest(EPID_UPDATE, NULL);
			//}
	
			requestTime = curTime;

			isLock = true;
			isReaded = false;

			if (playbackState != Application::EPS_READ)
			{
				pINetClient->Request(pSendBuffer, sendBufferSize);
			}
			pSendBuffer = NULL;
			sendBufferSize = 0;

#ifdef SIMULATOR_WAIT
			lastTime = curTime;
#endif
		}
	}
}

void NetSystem::OnStartRead()
{
	multiAnswer.Reset();
}

void NetSystem::OnRead(uint8 *data, int32 sz)
{
	multiAnswer.WriteBuffer(data, sz);
}



void NetSystem::OnFinishRead()
{
	isReaded = true;
	isLock = false;
}

void NetSystem::OnError()
{
	UTILS_LOG(EDMP_ERROR, "NetSystem::OnError");

	isLock = false;
	isReaded = false;

	pIServer->OnError();
}

void NetSystem::SaveAnswerToFile()
{
	uint32 size		=	0;
	uint8 * buffer	=	NULL;
	if (isReaded)
	{
		size = multiAnswer.GetDataSize();
		buffer = multiAnswer.GetDataBuffer();
	}
	GetApplication()->SaveUserDataToPlayback((void *)&size, 4);
	if (size > 0)
	{
		GetApplication()->SaveUserDataToPlayback((void *) buffer, size);
	}
}

void NetSystem::RestoreAnswerFromFile()
{
	uint32 size = 0;
	GetApplication()->LoadUserDataToPlayback((void *)&size, 4);	
	if (size > 0)
	{
		uint8 * buffer = new uint8[1000];
		GetApplication()->LoadUserDataToPlayback((void *) buffer, size);
		multiAnswer.Reset();
		multiAnswer.WriteBuffer(buffer, size);
		delete [] buffer;

		isReaded = true;
		isLock = false;
	}
}

void NetSystem::ParseAnswer()
{
	if (multiAnswer.GetDataSize())
	{
		pIServer->OnAnswer(&multiAnswer);
		if (multiAnswer.GetDataSize())
		{
			UTILS_LOG(EDMP_WARNING, "NetSystem::OnFinishRead: wrong packet size");
			multiAnswer.Reset();
		}
	}
}

void NetSystem::ForceUpdate()
{
	forceUpdate	=	true;
}

