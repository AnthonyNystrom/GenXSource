#include "stringwrapper.h"

StringWrapper::StringWrapper(ResourceSystem * _pStrResSys)
{
	pStrResSys = _pStrResSys;

	begin	= -1;
	end		= 0;
}

StringWrapper::~StringWrapper()
{
	for (int i = 0; i < STRING_CNT; i++)
	{
		SAFE_RELEASE(pStrings[i]);
	}
}

const char16*	StringWrapper::GetStringText(uint16 stringRecourceID)
{
	for (int i = 0; i < STRING_CNT; i++)
	{
		if (stringsID[i] == stringRecourceID)
		{
			return (pStrings[i])->GetString();
		}
	}

	begin++;
	if (begin >= STRING_CNT)
	{
		begin = 0;
	}

	if (begin == end)
	{
		end++;
		if (end >= STRING_CNT)
		{
			end = 0;
		}
	}
	SAFE_RELEASE(pStrings[begin]);
	pStrings[begin] = pStrResSys->CreateString(stringRecourceID);
	stringsID[begin] = stringRecourceID;
	return (pStrings[begin])->GetString();
}

