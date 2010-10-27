#ifndef _STRING_WRAPPER_
#define _STRING_WRAPPER_

#include "BaseTypes.h"
//#include "String.h"
#include "ResourceSystem.h"

#define STRING_CNT 25

class StringWrapper
{
public:
	StringWrapper(ResourceSystem * _pStrResSys);
	~StringWrapper();

	const char16*	GetStringText(uint16 stringRecourceID);

private:
	ResourceSystem * pStrResSys;

	String* pStrings[STRING_CNT];
	uint16  stringsID[STRING_CNT];

	int16	begin;
	int16	end;
};


#endif//_STRING_WRAPPER_