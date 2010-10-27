/*!
@file	Telephony.h
@brief	Class 
*/
#ifndef __FRAMEWORK_TELEPHONY_H__
#define __FRAMEWORK_TELEPHONY_H__

enum eTAPIResult
{
	TAPI_SUCCESS = 0,
	TAPI_ERROR = -1
};

#include "basetypes.h"

class TAPIListener
{
public:
	virtual void OnSMSRecieved(int32 result);
	virtual void OnSMSSent(int32 result);
};

class TAPIManager
{
public:
	static TAPIManager * CreateTapiManager();
	virtual eTAPIResult MakeCall(char16 * num) = 0;
	virtual eTAPIResult SendSMS(char16 * num, char16 * mes) = 0;
	virtual int32 SMSRecieveSubscribe(TAPIListener * callback) = 0;
	virtual int32 SMSRecieveUnsubscribe(void) = 0;
};

#endif //__FRAMEWORK_TELEPHONY_H__

