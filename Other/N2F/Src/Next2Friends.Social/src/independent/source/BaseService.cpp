#include "BaseService.h"
#include "Server.h"

BaseService::BaseService( Server *server )
{
	pServer = server;
}

BaseService::~BaseService()
{

}



bool BaseService::StartRequest( IAnswer *answerCB, int32 requestID )
{
	return pServer->StartRequest(this, answerCB, requestID);
}

bool BaseService::EndRequest()
{
	return pServer->EndRequest();
}




void BaseService::OnAnswer( PacketStream *answer, IAnswer *answerCB, int32 requestID )
{
	answerStream = answer;
	answerCB->OnSuccess(requestID);

	answerStream = NULL;
}


void BaseService::OnError( IAnswer *answerCB, int32 requestID, char16 *errorString )
{
	answerCB->OnFailed(requestID, errorString);
}