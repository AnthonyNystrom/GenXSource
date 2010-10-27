/*
============================================================================
Name        : Supappmainview.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Application main view
============================================================================
*/

#ifndef __SUPAPPMAINVIEW_H__
#define __SUPAPPMAINVIEW_H__

// INCLUDES
#include <coecntrl.h>
#include <coeview.h>
#include <aknlists.h>
#include "common.h"

// CLASS DECLARATION
class CSupAppMainView : public CCoeControl, public MCoeView, public MCoeControlObserver
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a CSupAppMainView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of CSupAppMainView.
	*/
	static CSupAppMainView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a CSupAppMainView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of CSupAppMainView.
	*/
	static CSupAppMainView* NewLC( const TRect& aRect );

	/**
	* ~CSupAppMainView
	* Virtual Destructor.
	*/
	virtual ~CSupAppMainView();

public:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this CSupAppMainView to the screen.
	* @param aRect the rectangle of this view that needs updating
	*/
	void Draw( const TRect& aRect ) const;

	/**
	* From CoeControl, SizeChanged.
	* Called by framework when the view size is changed.
	*/
	virtual void SizeChanged();

public: // From MCoeView
	TVwsViewId ViewId() const;

public: // For CAknAppUi
	TInt GetKeyEventCode() const;

private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	* Perform the second phase construction of a
	* CSupAppMainView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	* CSupAppMainView.
	* C++ default constructor.
	*/
	CSupAppMainView();

private:
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
	* From CCoeControl, HandleResourceChange
	*/
	void HandleResourceChange(TInt aType);

	/**
	* From MCoeControlObserver, HandleControlEventL
	*/
	void HandleControlEventL(CCoeControl* aControl, TCoeEvent aEventType);


private: // From MCoeView
	void ViewActivatedL(const TVwsViewId& aPrevViewId, TUid aCustomMessageId, const TDesC8& aCustomMessage);

	void ViewDeactivated();

private:

	// Listbox control
#if USE_MAINVIEW_LARGE_ICONS
	CAknSingleLargeStyleListBox* iListBox;
#else
	CAknSingleGraphicStyleListBox* iListBox;
#endif
	//HBufC* iViewParam; // Param's used to interchange with other views
};

#endif // __SUPAPPMAINVIEW_H__

// End of File
