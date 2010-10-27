/*!
@file	IApplicationCore.h
@brief	IApplicationCore class
*/
#ifndef __FRAMEWORK_IAPPLICATIONCORE_H__
#define __FRAMEWORK_IAPPLICATIONCORE_H__

#include "BaseTypes.h"

//! Base application class. Any Framework application should be derived from this class.
class IApplicationCore
{
public:
	virtual ~IApplicationCore(){}
	
	//! @brief The framework calls this member function each frame. Game logic should be implemented here.
	virtual void Update()			=	0;
	
	//! @brief The framework calls this member function each frame. Game render should be implemented here.
	virtual void Render()			=	0;

	//! @brief The framework calls this member function on Suspend event.
	virtual void OnSuspend()		=	0;

	//! @brief The framework calls this member function on Resume event.
	virtual void OnResume()			=	0;

	//! @brief The framework calls this member function when a keystroke translates to a nonsystem character.
	virtual void OnChar(char16 ch)	=	0;
};



#endif // __FRAMEWORK_IGAMECORE_H__


