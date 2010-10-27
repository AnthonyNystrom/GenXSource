//// =================================================================
/*!	\file GUILayoutStacked.h

	Revision History:

	\par [9.8.2007]	13:52 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_LAYOUT_STACKED__
#define __GUI_LAYOUT_STACKED__

#include "Vector.h"
#include "GUIControl.h"

class GUILayoutStacked: public GUIControl
{
public:

	GUILayoutStacked(GUIControl * _parent, const Rect & _rect);
	virtual ~GUILayoutStacked();

	virtual GUIControl * Clone();

	virtual bool		IsClass(uint32 classType) const;

	uint32				AddItem(GUIControl * const pControl);

	void				RemoveItem(GUIControl * const pControl);

	void				RemoveItem(int32 _index);

	void				SetCurrentPage(int32 index);

	int32				GetCurrentPage();

protected:

	int32				currentIndexPage;
	List<GUIControl*>	pageList;

	HANDLER_PROTOTYPE(OnSizeChange);
};

#endif // __GUI_LAYOUT_STACKED__