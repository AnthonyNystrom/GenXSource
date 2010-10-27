/*!
@file	SmartPtr.h
@brief	Class SmartPtr
*/
#ifndef __FRAMEWORK_SMARTPTR_H__
#define __FRAMEWORK_SMARTPTR_H__


//! Class SmartPtr
template <class Type>
class SmartPtr
{
public:
	SmartPtr() : object(0) {}
	
	SmartPtr(Type* _object) : object(_object) {	}
	
	SmartPtr(const SmartPtr<Type>& cp) : object(cp.object) 
	{ 
		if (object)
			object->AddReference(); 
	}

	~SmartPtr() 
	{ 
		if (object) object->Release(); 
	}

	SmartPtr<Type> & operator = (Type * _object)
	{
		if (object) object->Release();
		object = _object;
		return *this;
	}

	SmartPtr<Type> & operator = (const SmartPtr<Type> & _smart)
	{
		if (this == &_smart) return *this;
		if (object) object->Release();
		object = _smart.object;
		if (object) object->AddReference();
		
		return *this;
	}

	Type* operator->() { return object; }

	operator void * ()
	{
		return (void *)object;
	}
private:
	Type * object;
};

#endif // __FRAMEWORK_SMARTPTR_H__




