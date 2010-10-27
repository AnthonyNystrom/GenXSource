/*
============================================================================
Name        : Aafimagecartview.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Image cart view
============================================================================
*/

#ifndef __AAFAPPIMAGECARDVIEW_H__
#define __AAFAPPIMAGECARDVIEW_H__

// INCLUDES
#include <coecntrl.h>
#include <coeview.h>
#include <aknlists.h>
#include <eiklbx.h>
#include <aknwaitdialog.h>
#include "common.h"


// FORWARD DECLARATIONS
class CAafUploadServiceProvider;
class CFileBrowserEngine;

// CLASS DECLARATION
class CAafAppImageCartView : public CCoeControl, public MCoeView, public MProgressDialogCallback,
	public MRequestObserver
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a CAafAppImageCartView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of CAafAppImageCartView.
	*/
	static CAafAppImageCartView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a CAafAppImageCartView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of CAafAppImageCartView.
	*/
	static CAafAppImageCartView* NewLC( const TRect& aRect );

	/**
	* ~CAafAppImageCartView
	* Virtual Destructor.
	*/
	virtual ~CAafAppImageCartView();

protected: // from MRequestObserver
	/**
	*
	*/
	void HandleRequestCompletedL(const TInt& aError);

public:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this CAafAppImageCartView to the screen.
	* @param aRect the rectangle of this view that needs updating
	*/
	void Draw( const TRect& aRect ) const;

	/**
	* From CoeControl, SizeChanged.
	* Called by framework when the view size is changed.
	*/
	virtual void SizeChanged();

public: // From MCoeView
	/**
	*
	*/
	TVwsViewId ViewId() const;

	/**
	*
	*/
	void CommandMark(); //

	/**
	*
	*/
	TBool IsCurrentItemMarked();

	/**
	* Initiates upload process
	*/
	void DoUpload();

	/**
	*
	*/
	void RemoveSelectedItems();

	/**
	*
	*/
	void RemoveAllItems();

	/**
	* Retrieves listbox items count
	*/
	TInt GetItemsCount();

	/**
	* Returns array of selected item indexes
	*/
	//void GetSelectedIndexesL(const CListBoxView::CSelectionIndexArray* aIndexesArray);

	/**
	*
	*/
	void PreviewCurrentItemL();

private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	* Perform the second phase construction of a
	* CAafAppImageCartView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	* CAafAppImageCartView.
	* C++ default constructor.
	*/
	CAafAppImageCartView();

private: // From MCoeView
	/**
	*
	*/
	void ViewActivatedL(const TVwsViewId& aPrevViewId, TUid aCustomMessageId, const TDesC8& aCustomMessage);

	/**
	*
	*/
	void ViewDeactivated();

	/**
	* From CCoeControl, HandleResourceChange
	*/
	void HandleResourceChange(TInt aType);

	/**
	* From CCoeControl, CountComponentControls
	*/
	TInt CountComponentControls() const;

	/**
	* From CCoeControl, ComponentControl
	*/
	CCoeControl* ComponentControl(TInt aIndex) const;

	/**
	* From CCoeControl, OfferKeyEventL
	*/
	TKeyResponse OfferKeyEventL(const TKeyEvent& aKeyEvent, TEventCode aType);

	/**
	* From MCoeControlObserver, HandleControlEventL
	*/
	//void HandleControlEventL(CCoeControl* aControl, TCoeEvent aEventType);

	/**
	*
	*/
	void SetListboxIcons();

	/**
	*
	*/
	void SetFileListL();

	/*
	* New functions
	* Starts wait dialog
	*/
	void StartWaitDialog();

	/**
	* Stops the wait dialog
	*/
	void StopWaitDialog();

	/**
	* Get's called when a dialog is dismissed
	* From MProgressDialogCallback
	*/
	void DialogDismissedL(TInt aButtonId);

private:
	CAknSingleGraphicStyleListBox* iListBox;

	CAafUploadServiceProvider* iUploadProvider; // Service provider for performing async requests

	CAknWaitDialog* iWaitDialog; // Wait dialog

	CFileBrowserEngine* iBrowserEngine; // File browser engine

	TInt iCurrentItemIndex; // Currently selected listbox item index
};

#endif // __AAFAPPIMAGECARDVIEW_H__

// End of File