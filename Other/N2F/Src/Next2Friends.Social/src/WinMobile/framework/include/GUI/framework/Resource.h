/*!
@file	Resource.h
@brief	Class Resource
*/
#ifndef __FRAMEWORK_RESOURCE_H__
#define __FRAMEWORK_RESOURCE_H__

#include "IRefCounter.h"
#include "Utils.h"

class ResourceSystem;

//! @brief Base resource class.
//! All resources that need reference count are derived from this class.
class Resource : public IRefCounter
{
public:
	//! Resource types
	enum eType
	{
		ET_BINARY = 0,//!< Binary resource
		ET_STRING,//!< String resource
		ET_SOUND, //!< Sound resource
		ET_SPRITE,//!< Sprite resource
		ET_FONT,//!< Font resource
		ET_ARRAY_LIST,//!< 2Da array resource

		ET_RESOURCE_COUNT
	};
protected:
	Resource(ResourceSystem * resourceSystem, int16 id, char8 * name, eType type);
	virtual ~Resource();

private:
	int16	id;
	char8 * name;
	eType	type;
	ResourceSystem * resourceSystem;
};

#endif // __FRAMEWORK_RESOURCE_H__

