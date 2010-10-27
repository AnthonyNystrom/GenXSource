/*
============================================================================
Name        : Aafappfilebrowserview.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : File browser view
============================================================================
*/

#ifndef __AAFAPPFILEBROWSERVIEW_H__
#define __AAFAPPFILEBROWSERVIEW_H__

// INCLUDES
#include <coecntrl.h>
#include <coeview.h>
#include <aknlists.h>
#include <eiklbx.h>


// FORWARD DECLARATION
class CFileBrowserEngine;

// CLASS DECLARATION
class CAafAppFileBrowserView : public CCoeControl, public MCoeView, public MCoeControlObserver
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a CAafAppFileBrowserView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of CAafAppFileBrowserView.
	*/
	static CAafAppFileBrowserView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a CAafAppFileBrowserView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of CAafAppFileBrowserView.
	*/
	static CAafAppFileBrowserView* NewLC( const TRect& aRect );

	/**
	* ~CAafAppFileBrowserView
	* Virtual Destructor.
	*/
	virtual ~CAafAppFileBrowserView();

public: // New functions
	/**
	* Launches the specified file with the env defined application
	*/
	void LaunchCurrentL();

	/**
	* Add currently selected image to upload cart
	*/
	void AddImageToUpload(); 

	/**
	* Determines whether current item is directory
	*/
	TBool IsCurrentItemDir();

	/**
	* Shows file properties dialog
	*/
	void ShowFileProperties();

private:
	/**
	*
	*/
	void SetFileListL(TFileName &aDirectory);

	/**
	* Handles current item, i.e. opens file preview or open directory
	*/
	void HandleCurrentItemL();

public:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this CAafAppFileBrowserView to the screen.
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

private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	* Perform the second phase construction of a
	* CAafAppFileBrowserView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	* CAafAppFileBrowserView.
	* C++ default constructor.
	*/
	CAafAppFileBrowserView();

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
	/**
	*
	*/
	void ViewActivatedL(const TVwsViewId& aPrevViewId, TUid aCustomMessageId, const TDesC8& aCustomMessage);

	/**
	*
	*/
	void ViewDeactivated();

private:
	CFileBrowserEngine* iBrowserEngine; // File browser engine

	CAknSingleGraphicStyleListBox* iListBox;

	TInt iCurrentItemIndex; // Currently selected listbox item index
};

#endif // __AAFAPPFILEBROWSERVIEW_H__

// End of File