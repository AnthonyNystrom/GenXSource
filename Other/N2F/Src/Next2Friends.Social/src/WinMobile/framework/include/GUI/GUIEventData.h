//// =================================================================
/*!	\file GUIEventData.h
	
	Revision History:

	\par [15.5.2007] 15:58 by Ivan Petrochenko
	Constructor.

	\par [15.5.2007] 15:26 by Vitaliy Borodovsky
	Add comments.
	
	\par [10.5.2007]	12:47 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIEVENTDATA_H__
#define __FRAMEWORK_GUIEVENTDATA_H__

//! \brief	Class represents additional events data
class GUIEventData
{
public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_x		- x coordinate.
	//! \param[in]	_y		- y coordinate.
	//! \param[in]	_key	- keyboard key.
	//! \param[in]	_character - keyboard character.
	//! \param[in]	_lParam	- l-param.
	//! \param[in]	_wParam	- w-param.
	//  ***************************************************
	GUIEventData(int32 _x=0, int32 _y=0, uint32 _key=0, uint32 _character=0, uint32 _lParam=0, uint32 _wParam=0)
	{
		x = _x;
		y = _y;
		key = _key;
		character = _character;
		lParam = _lParam;
		wParam = _wParam;
	}

	int32	x;			//!< x coordinate
	int32	y;			//!< y coordinate
	int32	key;		//!< keyboard key
	int32	character;	//!< keyboard character
	uint32	lParam;		//!< l-param
	uint32	wParam;		//!< w-param
};

#endif // __FRAMEWORK_GUIEVENTDATA_H__
