//// =================================================================
/*!	\file GUIParsedString.h

	Revision History:

	\par [9.8.2007]	13:38 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUIPARSEDSTRING_H__
#define __FRAMEWORK_GUIPARSEDSTRING_H__

#include "BaseTypes.h"

//! \brief Structure which represents a single line of plane text.
struct ParsedString 
{
	char16*	pStr;	//!< Pointer on first string character.
	uint32	count;	//!< Count of characters in string.
	int32	x;		//!< x coordinate, where string to draw (corresponding to the control).
	int32	y;		//!< y coordinate, where string to draw (corresponding to the control).

	ParsedString()
	{
		pStr	= NULL;
		count	= 0;
		x		= 0;
		y		= 0;
	}	
};

#endif // __FRAMEWORK_GUIPARSEDSTRING_H__