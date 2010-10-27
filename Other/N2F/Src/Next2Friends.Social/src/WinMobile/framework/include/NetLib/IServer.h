/*!
@file IServer.h
@brief Class IServer
*/

#ifndef __I_SERVER_H__
#define __I_SERVER_H__

#include "BaseTypes.h"
#include "PacketStream.h"
#include "IAnswer.h"

//! Interface for handled of server answers, update and error events
class IServer
{
public:

	//! @brief prototype of handler for parsing of server answers
	virtual void OnAnswer(PacketStream *answer)	= NULL;
	
	//! @brief prototype of handler for error events
	virtual void OnError()				= NULL;

};

#endif // __I_SERVER_H__