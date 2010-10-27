//// =================================================================
/*!	\file GUIController.h

	Revision History:

	\par [9.8.2007]	13:40 by Ivan Petrochenko
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUICONTROLLER_H__
#define __FRAMEWORK_GUICONTROLLER_H__

#include "BaseTypes.h"

class GUISystem;
class GUIControl;

/*! \brief The GUIController class is a base for any GUI controller.
	
	Designation of the class is to switch focus between controls.
	It is also should provide keyboard events handling with sending
	corresponding GUI events.
*/	
class GUIController
{
public:
	//  ***************************************************
	//! \brief		Constructor.    	
	//! \param[in]	_guiSystem - pointer on GUISystem.
	//  ***************************************************
	GUIController(GUISystem * _guiSystem): guiSystem(_guiSystem) 
	{ }

	virtual ~GUIController()
	{ }
	
	// *************************************************************
	//! \brief    	Update cycle (logic). Will be called once per frame.
	// *************************************************************
	virtual void Update() = 0;

	// *************************************************************
	//! \brief    	Render cycle. Will be called once per frame.
	// *************************************************************
	virtual void Render() { };

protected:
	GUISystem	* guiSystem;  //!< Pointer on GUISystem.

private:
	Point		point;
};

#endif // __FRAMEWORK_GUICONTROLLER_H__