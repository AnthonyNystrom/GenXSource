/*!
@file	ArrayList.h
@brief	Class ArrayList
*/

#ifndef __FRAMEWORK_ARRAYLIST_H__
#define __FRAMEWORK_ARRAYLIST_H__

#include "Resource.h"

class Binary;

//! 2D array resource
class ArrayList : public Resource
{
	friend class ResourceSystem;

public:

	//! Get Array Count In This XML Array
	int32 GetArrayCount();
	
	//! Get size of array [Index]
	int32 GetArraySize(int32 index);

	//! Get [Index] array value 
	int32 GetArrayValue(int32 arrayIndex, int32 valueIndex);

private:

	ArrayList(Binary * binary, ResourceSystem * resourceSystem, int16 resourceID, char8 * resourceName);
	virtual ~ArrayList();

	int32 * data;

	Binary * binary;
};

#endif //__FRAMEWORK_ARRAYLIST_H__

