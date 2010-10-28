/* ==========================================================================
	Class :			CUndoItem

	Date :			06/04/04

	Purpose :		The "CUndoItem" class represents a single state of the 
					application data, and is used for the undo-handling.

	Description :	The class is a simple data-holder with members for the 
					screen size and an array with copies of all the objects 
					in the container at the time the instance of this class 
					was created. Instantiation is made in 
					"CDiagramEntityContainer::Snapshot".

	Usage :			See "CDiagramEntityContainer" on how to use the class.

	Changes :		30/5 2004	Made CUndoItem dtor virtual. Allocating 
								members from the stack instead of the heap.

   ========================================================================*/

#include "stdafx.h"
#include "UndoItem.h"

// Construction/destruction
CUndoItem::CUndoItem()
/* ============================================================
	Function :		CUndoItem::CUndoItem
	Description :	constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{
}

CUndoItem::~CUndoItem()
/* ============================================================
	Function :		CUndoItem::~CUndoItem
	Description :	Destructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			Deletes all associated memory.

   ============================================================*/
{

	int max = arr.GetSize();
	for (int t = 0 ; t < max ; t++ )
		delete arr.GetAt( t );
	arr.RemoveAll();

}
