/*
============================================================================
Name        : Aafsplashscreenview.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Splash screen view class
============================================================================
*/

#ifndef __AAFSPLASHSCREENVIEW_H__
#define __AAFSPLASHSCREENVIEW_H__

#include <coecntrl.h>
#include <coeview.h>

// FORWARD DECLARATION
class CAafSplashScreen;

// CLASS DECLARATION
class CAafSplashScreenView : public CCoeControl, public MCoeView
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a CAafSplashScreenView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of CAafSplashScreenView.
	*/
	static CAafSplashScreenView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a CAafSplashScreenView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of CAafSplashScreenView.
	*/
	static CAafSplashScreenView* NewLC( const TRect& aRect );

	/**
	* ~CAafSplashScreenView
	* Virtual Destructor.
	*/
	virtual ~CAafSplashScreenView();

public:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this CAafSplashScreenView to the screen.
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

private: // Constructors

	/**
	* ConstructL
	* 2nd phase constructor.
	* Perform the second phase construction of a
	* CAafSplashScreenView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	* CAafSplashScreenView.
	* C++ default constructor.
	*/
	CAafSplashScreenView();

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
	* From CCoeControl, HandleResourceChange
	*/
	void HandleResourceChange(TInt aType);

private: // From MCoeView
	void ViewActivatedL(const TVwsViewId& aPrevViewId, TUid aCustomMessageId, const TDesC8& aCustomMessage);

	void ViewDeactivated();

private: // Member variables

	CAafSplashScreen* iSplashScreen;

};

#endif // __AAFSPLASHSCREENVIEW_H__

// End of file