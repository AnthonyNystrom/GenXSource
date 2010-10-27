/*
============================================================================
 Name        : AafView.h
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application view
============================================================================
*/

#ifndef __AAFAPPVIEW_H__
#define __AAFAPPVIEW_H__

// INCLUDES
#include <coecntrl.h>

// CLASS DECLARATION
class CAafAppView : public CCoeControl
	{
	public: // New methods

		/**
		* NewL.
		* Two-phased constructor.
		* Create a CAafAppView object, which will draw itself to aRect.
		* @param aRect The rectangle this view will be drawn to.
		* @return a pointer to the created instance of CAafAppView.
		*/
		static CAafAppView* NewL( const TRect& aRect );

		/**
		* NewLC.
		* Two-phased constructor.
		* Create a CAafAppView object, which will draw itself
		* to aRect.
		* @param aRect Rectangle this view will be drawn to.
		* @return A pointer to the created instance of CAafAppView.
		*/
		static CAafAppView* NewLC( const TRect& aRect );

		/**
		* ~CAafAppView
		* Virtual Destructor.
		*/
		virtual ~CAafAppView();

	public:  // Functions from base classes

		/**
		* From CCoeControl, Draw
		* Draw this CAafAppView to the screen.
		* @param aRect the rectangle of this view that needs updating
		*/
		void Draw( const TRect& aRect ) const;

		/**
		* From CoeControl, SizeChanged.
		* Called by framework when the view size is changed.
		*/
		virtual void SizeChanged();

	private: // Constructors

		/**
		* ConstructL
		* 2nd phase constructor.
		* Perform the second phase construction of a
		* CAafAppView object.
		* @param aRect The rectangle this view will be drawn to.
		*/
		void ConstructL(const TRect& aRect);

		/**
		* CAafAppView.
		* C++ default constructor.
		*/
		CAafAppView();

	};

#endif // __AAFAPPVIEW_H__

// End of File
