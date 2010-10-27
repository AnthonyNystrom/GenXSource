/*
============================================================================
 Name        : Supappuploadimagesview.h
 Author      : Next2Friends
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Upload images view
============================================================================
*/

#ifndef __SUPAPPUPLOADIMAGESVIEW_H__
#define __SUPAPPUPLOADIMAGESVIEW_H__

// INCLUDES
#include <coecntrl.h>
#include <coeview.h>

// CLASS DECLARATION
class CSupAppUploadImagesView : public CCoeControl, public MCoeView
	{
	public: // New methods

		/**
		* NewL.
		* Two-phased constructor.
		* Create a CSupAppUploadImagesView object, which will draw itself to aRect.
		* @param aRect The rectangle this view will be drawn to.
		* @return a pointer to the created instance of CSupAppUploadImagesView.
		*/
		static CSupAppUploadImagesView* NewL( const TRect& aRect );

		/**
		* NewLC.
		* Two-phased constructor.
		* Create a CSupAppUploadImagesView object, which will draw itself
		* to aRect.
		* @param aRect Rectangle this view will be drawn to.
		* @return A pointer to the created instance of CSupAppUploadImagesView.
		*/
		static CSupAppUploadImagesView* NewLC( const TRect& aRect );

		/**
		* ~CSupAppUploadImagesView
		* Virtual Destructor.
		*/
		virtual ~CSupAppUploadImagesView();

	public:  // Functions from base classes

		/**
		* From CCoeControl, Draw
		* Draw this CSupAppUploadImagesView to the screen.
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
		* CSupAppUploadImagesView object.
		* @param aRect The rectangle this view will be drawn to.
		*/
		void ConstructL(const TRect& aRect);

		/**
		* CSupAppUploadImagesView.
		* C++ default constructor.
		*/
		CSupAppUploadImagesView();
		
	private: // From MCoeView
		void ViewActivatedL(const TVwsViewId& aPrevViewId, TUid aCustomMessageId, const TDesC8& aCustomMessage);

		void ViewDeactivated();

		/**
		* From CCoeControl, HandleResourceChange
		*/
		void HandleResourceChange(TInt aType);

	private:
		HBufC* iViewParam; // Param's used to interchange with other views

	};

#endif // __SUPAPPIMAGECARDVIEW_H__

// End of File