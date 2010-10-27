/*!
@file IAnswer.h
@brief Class IAnswer
*/

#ifndef __I_ANSWER_H__
#define __I_ANSWER_H__

#include "BaseTypes.h"

//! Interface for server answer handler
class IAnswer
{
public:

	// ***************************************************
	//! \brief    	Single poor-virtual method of handler
	//! 
	//! \return   	No return value.
	// ***************************************************
// 	virtual void OnAnswer(int32 res)	=	NULL;

	//virtual	bool CanProcessNetwork()		=	NULL;

	virtual void OnSuccess(int32 packetId)	=	NULL;
	virtual void OnFailed(int32 packetId, char16 *errorString)	=	NULL;

	virtual	~IAnswer() {};
};

#endif // __I_ANSWER_H__