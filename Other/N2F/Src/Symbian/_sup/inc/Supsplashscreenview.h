/*
============================================================================
Name        : Supsplashscreenview.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Splash screen view class
============================================================================
*/

#ifndef __SUPSPLASHSCREENVIEW_H__
#define __SUPSPLASHSCREENVIEW_H__

#include <coecntrl.h>
#include <coeview.h>

// FORWARD DECLARATION
class CSupSplashScreen;

// CLASS DECLARATION
class CSupSplashScreenView : public CCoeControl, public MCoeView
{
public: // New methods

	/**
	* NewL.
	* Two-phased constructor.
	* Create a CSupSplashScreenView object, which will draw itself to aRect.
	* @param aRect The rectangle this view will be drawn to.
	* @return a pointer to the created instance of CSupSplashScreenView.
	*/
	static CSupSplashScreenView* NewL( const TRect& aRect );

	/**
	* NewLC.
	* Two-phased constructor.
	* Create a CSupSplashScreenView object, which will draw itself
	* to aRect.
	* @param aRect Rectangle this view will be drawn to.
	* @return A pointer to the created instance of CSupSplashScreenView.
	*/
	static CSupSplashScreenView* NewLC( const TRect& aRect );

	/**
	* ~CSupSplashScreenView
	* Virtual Destructor.
	*/
	virtual ~CSupSplashScreenView();

public:  // Functions from base classes

	/**
	* From CCoeControl, Draw
	* Draw this CSupSplashScreenView to the screen.
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
	* CSupSplashScreenView object.
	* @param aRect The rectangle this view will be drawn to.
	*/
	void ConstructL(const TRect& aRect);

	/**
	* CSupSplashScreenView.
	* C++ default constructor.
	*/
	CSupSplashScreenView();

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

	CSupSplashScreen* iSplashScreen;

};

#endif // __SUPSPLASHSCREENVIEW_H__

// End of file