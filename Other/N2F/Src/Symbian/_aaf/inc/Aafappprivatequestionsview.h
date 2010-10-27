/*
============================================================================
Name        : Aafappprivatequestionsview.h
Author      : Vitaly Vinogradov
Version     :
Copyright   : (c) Next2Friends, 2008
Description : Aafappprivatequestionsview.h - header file
============================================================================
*/

#ifndef __AAFAPPPRIVATEQUESTIONSVIEW_H__
#define __AAFAPPPRIVATEQUESTIONSVIEW_H__

// INCLUDES
#include <coecntrl.h>
#include <coeview.h>
#include <aknlists.h>
#include <eiklbx.h>
#include <aknwaitdialog.h>
#include "common.h"

// FORWARD DECLARATIONS
class CAafPrivateQuestionsProvider;


// CLASS DECLARATION
class  CAafAppPQuestionsView : public CCoeControl, public MCoeView, public MProgressDialogCallback,
	public MRequestObserver
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a  CAafAppPQuestionsView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of  CAafAppPQuestionsView.
	*/
	static  CAafAppPQuestionsView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a  CAafAppPQuestionsView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of  CAafAppPQuestionsView.
	*/
	static  CAafAppPQuestionsView* NewLC( const TRect& aRect );

	/**
	* ~ CAafAppPQuestionsView
	* Virtual Destructor.
	*/
	virtual ~ CAafAppPQuestionsView();

protected: // from MRequestObserver
	/**
	*
	*/
	void HandleRequestCompletedL(const TInt& aError);

public:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this  CAafAppPQuestionsView to the screen.
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
	* Retrieves listbox items count
	*/
	TInt GetItemsCount();

	/**
	* Initiate questions retrieving process
	*/
	void GetQuestions();

	/**
	* Get order index of currently selected item
	*/
	TInt CurrentItemIndex();

private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	* Perform the second phase construction of a
	*  CAafAppPQuestionsView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	*  CAafAppPQuestionsView.
	* C++ default constructor.
	*/
	CAafAppPQuestionsView();

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


private:
	/**
	*
	*/
	void SetItemListL();

	/*
	* New functions
	* Starts wait dialog
	*/
	void StartWaitDialog(TInt aDialogId);

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
	CAknDoubleGraphicStyleListBox* iListBox;

	CAknWaitDialog* iWaitDialog; // Wait dialog

	CAafPrivateQuestionsProvider* iQuestionsProvider;
};



#endif // __AAFAPPPRIVATEQUESTIONSVIEW_H__

// End of File
