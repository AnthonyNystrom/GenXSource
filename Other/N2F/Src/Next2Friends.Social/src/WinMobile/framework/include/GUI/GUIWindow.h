//// =================================================================
/*!	\file GUIWindow.h

	Revision History:

	\par [9.8.2007]	13:40 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_WINDOW__
#define __GUI_WINDOW__

#include "GUIControl.h"

/*! \brief The GUIWindow class provides a window control 
	\todo comments.
*/
class GUIWindow: public GUIControl
{
public:

	enum eWindowOrder
	{
		EWO_TOP,
		EWO_BOTTOM
	};

	GUIWindow(const Rect & _rect, eWindowOrder _order = EWO_TOP);
	virtual ~GUIWindow();

	virtual bool	IsClass(uint32 classType) const;
	virtual void	UpdateScreenRect();

protected:
};

#endif // __GUI_WINDOW__