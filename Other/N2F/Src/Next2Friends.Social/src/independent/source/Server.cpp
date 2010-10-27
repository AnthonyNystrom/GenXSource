#include "Server.h"
#include "Utils.h"
#include "BaseService.h"


Server::Server(ServerListener *listener)
{
//	stable server
	FASSERT(listener);

	pListener = listener;
	
	//netSystem	= new NetSystem("http://next2friends.com:100/soap2bin.handler", this);
	netSystem	= new NetSystem("http://69.21.114.101:100/soap2bin.handler", this);

	freeRequestsList = new VList();
	writeRequestsList = new VList();
	sendRequestsList = new VList();
	writingStream = new PacketStream();
	sendingStream = new PacketStream();
}

Server::~Server()
{
	//TODO: free memory
	SAFE_DELETE(netSystem);
	SAFE_DELETE(currentSendingRequest);
	SAFE_DELETE(currentWritingRequest);
	while (!sendRequestsList->Empty())
	{
		SingleRequest *sr = (SingleRequest*)*sendRequestsList->Begin();
		SAFE_DELETE(sr);
		sendRequestsList->Erase(sendRequestsList->Begin());
	}
	SAFE_DELETE(sendRequestsList);

	while (!writeRequestsList->Empty())
	{
		SingleRequest *sr = (SingleRequest*)*writeRequestsList->Begin();
		SAFE_DELETE(sr);
		writeRequestsList->Erase(writeRequestsList->Begin());
	}
	SAFE_DELETE(writeRequestsList);

	while (!freeRequestsList->Empty())
	{
		SingleRequest *sr = (SingleRequest*)*freeRequestsList->Begin();
		SAFE_DELETE(sr);
		freeRequestsList->Erase(freeRequestsList->Begin());
	}
	SAFE_DELETE(freeRequestsList);
	SAFE_DELETE(writingStream);
	SAFE_DELETE(sendingStream);
}

void Server::OnAnswer(PacketStream *answer)
{
	UTILS_LOG(EDMP_DEBUG, "Server::OnAnswer: packetID");

	int16 res;
	answer->ReadInt16(res);
	if (res != 1)
	{
		char16 *temp = answer->GetWString();
		currentSendingRequest->service->OnError(currentSendingRequest->answer, currentSendingRequest->requestID, temp);
		SAFE_DELETE(temp);
		ReturnFreeRequest(currentSendingRequest);
		currentSendingRequest = NULL;
		return;
	}

	currentSendingRequest->service->OnAnswer(answer, currentSendingRequest->answer, currentSendingRequest->requestID);
	ReturnFreeRequest(currentSendingRequest);
	currentSendingRequest = NULL;

}



void Server::Update()
{
	netSystem->Update();

	if (netSystem->IsLocked())
	{
		return;
	}
	if (!currentSendingRequest)
	{
		if (sendRequestsList->Empty() && !writeRequestsList->Empty())
		{
			VList *temp = sendRequestsList;
			sendRequestsList = writeRequestsList;
			writeRequestsList = temp;

			PacketStream *tempStream = sendingStream;
			sendingStream = writingStream;
			writingStream = tempStream;
			writingStream->Reset();
		}

		if (!sendRequestsList->Empty())
		{
			currentSendingRequest = (SingleRequest*)(*sendRequestsList->Begin());
			sendRequestsList->Erase(sendRequestsList->Begin());
			netSystem->StartRequest();
			netSystem->EndRequest(currentSendingRequest->dataPoint, currentSendingRequest->dataSize);
		}
	}
}






void Server::CancelRequest()
{
	//netSystem->CancelRequest();
}

void Server::OnError()
{
	UTILS_TRACE("   !!!   Server error   !!!   ");

	pListener->OnServerError();
	if (currentSendingRequest)
	{
		ReturnFreeRequest(currentSendingRequest);
		currentSendingRequest = NULL;
	}
}

bool Server::StartRequest(BaseService *service, IAnswer *answerCB, int32 reqID)
{
	currentWritingRequest = GetFreeRequest();
	currentWritingRequest->answer = answerCB;
	currentWritingRequest->requestID = reqID;
	currentWritingRequest->service = service;
	currentWritingRequest->dataPoint = writingStream->GetDataBuffer() + writingStream->GetTail();

	return writingStream->WriteInt16(1);//invocation code
}

bool Server::EndRequest()
{
	currentWritingRequest->dataSize = writingStream->GetDataBuffer() + writingStream->GetTail() - currentWritingRequest->dataPoint;
	writeRequestsList->PushBack(currentWritingRequest);
	currentWritingRequest = NULL;
	return true;
}


SingleRequest * Server::GetFreeRequest()
{
	if (freeRequestsList->Empty())
	{
		return new SingleRequest();
	}
	SingleRequest *r = (SingleRequest *)(*(freeRequestsList->Begin()));
	freeRequestsList->Erase(freeRequestsList->Begin());
	return r;
}

void Server::ReturnFreeRequest( SingleRequest *req )
{
	freeRequestsList->PushBack(req);
}








