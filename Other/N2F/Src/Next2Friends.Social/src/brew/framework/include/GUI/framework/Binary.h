/*!
@file	Binary.h
@brief	Class Binary
*/

#ifndef __FRAMEWORK_BINARY_H__
#define __FRAMEWORK_BINARY_H__

#include "Resource.h"

//! Binary resource class
class Binary : public Resource
{
	friend class ResourceSystem;

public:

	//! Data buffer pointer
	void * data;

	//! Data buffer size
	uint32 size;

private:

	//! @brief    	
	//! @param[in]	buf			- data buffer pointer.
	//! @param[in]	bufSize		- data buffer size.
	//! @param[in]	resSys		- pointer on ResourceSystem instance.
	//! @param[in]	resourceID	- resource ID.
	//! @param[in]	resourceName- resource name.
	Binary(void * buf, uint32 bufSize, ResourceSystem * resSys, int16 resourceID, char8 * resourceName);

	virtual ~Binary();
};

#endif //__FRAMEWORK_BINARY_H__