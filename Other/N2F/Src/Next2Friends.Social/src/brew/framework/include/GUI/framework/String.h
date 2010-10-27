/*!
@file String.h
@brief Class String
*/

#ifndef __FRAMEWORK_STRING_H__
#define __FRAMEWORK_STRING_H__

#include "Resource.h"

//! Class for getting resource string
class String : public Resource
{
	friend class ResourceSystem;

	String(void * data, ResourceSystem * resourceSystem, int16 resourceID, char8 * resourceName);
	String(uint32 _size, ResourceSystem * resourceSystem, int16 resourceID);

	virtual ~String();

	union
	{
		uint8 * data;
		char16 * data16;
	};
	uint32	size;
public:
	operator const char16*  () { return (const char16*)data; }

	//! @brief Function to getting string
	//! @return string
	const char16* GetString() { return (const char16*)data; }

	//! @brief Function to string size
	//! @return string size
	uint32 GetBufferSize();

	//! @brief Function to writable buffer
	//! @return pointer to writable buffer
	char16* GetWritableBuffer();
};

#endif // __FRAMEWORK_STRING_H__