#ifndef __GUI_ITEMS_LIST_DELEGATE__
#define __GUI_ITEMS_LIST_DELEGATE__

#include "BaseTypes.h"

class GUIControl;

class ItemsListDelegate
{
	friend class GUIItemsList;
protected:

	virtual void			ItemsListOnItem(GUIItemsList *forList, void *owner) = 0;

	virtual GUIControl*		ItemsListCreateListItem(GUIItemsList *forList) = 0;

	virtual bool			ItemsListIsOwnerValid(GUIItemsList *forList, void *owner) = 0;
	virtual void			*ItemsListGetFirstOwner(GUIItemsList *forList) = 0;
	virtual void			*ItemsListGetNextOwner(GUIItemsList *forList) = 0;

	// ***************************************************
	//! \brief    	TuneItemByOwner allow delegate tune gui item on adding to the list
	//! Calls only when item adding to the list.
	//! 
	//! \param      item
	//! \param      owner
	//! \return   	void 
	// ***************************************************
	virtual void			ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner) = 0;

	// ***************************************************
	//! \brief    	CorrectItemByOwner allow delegate to correct gui item data
	//! if you need to correct some data on list rebuilding, but item already tuned
	//! make it in this method. Calls every time on list rebuild.
	//! 
	//! \param      item
	//! \param      owner
	//! \return   	void 
	// ***************************************************
	virtual void			ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner) = 0;
};


#endif//__GUI_BUTTON__