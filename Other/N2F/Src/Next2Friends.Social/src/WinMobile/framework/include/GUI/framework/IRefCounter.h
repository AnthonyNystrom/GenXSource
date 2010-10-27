/*!
@file	IRefCounter.h
@brief	Class IRefCounter
*/
#ifndef __FRAMEWORK_IBASE_H__
#define __FRAMEWORK_IBASE_H__

#include "BaseTypes.h"

//! Class IRefCounter
class IRefCounter
{
public:
	//! @brief Increment reference counter
	virtual void AddReference()
	{
		++referenceCount;
	}

	//! @brief WARNING! instead this function recommend use @ref SafeRelease()
	//! Decrement reference counter, if counter == 0, resource removed 
	//! @brief 
	virtual void Release()
	{
		if(!--referenceCount) 
		{
			delete this;
		}
	}

	//! @brief Function return current reference counter
	//! @return Current reference counter
	virtual uint32 GetReferenceCount()
	{
		return referenceCount;
	}

protected:
	IRefCounter() : referenceCount	(1)
	{
	}
	virtual ~IRefCounter(){};

	uint32	referenceCount;
};



#endif // __FRAMEWORK_IBASE_H__
