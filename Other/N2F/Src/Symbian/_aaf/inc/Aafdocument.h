/*
============================================================================
 Name        : AafDocument.h
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application document class (model)
============================================================================
*/

#ifndef __AAFDOCUMENT_H__
#define __AAFDOCUMENT_H__

// INCLUDES
#include <akndoc.h>

// FORWARD DECLARATIONS
class CAafAppUi;
class CEikApplication;


// CLASS DECLARATION

/**
* CAafDocument application class.
* An instance of class CAafDocument is the Document part of the
* AVKON application framework for the Aaf example application.
*/
class CAafDocument : public CAknDocument
	{
	public: // Constructors and destructor

		/**
		* NewL.
		* Two-phased constructor.
		* Construct a CAafDocument for the AVKON application aApp
		* using two phase construction, and return a pointer
		* to the created object.
		* @param aApp Application creating this document.
		* @return A pointer to the created instance of CAafDocument.
		*/
		static CAafDocument* NewL( CEikApplication& aApp );

		/**
		* NewLC.
		* Two-phased constructor.
		* Construct a CAafDocument for the AVKON application aApp
		* using two phase construction, and return a pointer
		* to the created object.
		* @param aApp Application creating this document.
		* @return A pointer to the created instance of CAafDocument.
		*/
		static CAafDocument* NewLC( CEikApplication& aApp );

		/**
		* ~CAafDocument
		* Virtual Destructor.
		*/
		virtual ~CAafDocument();

	public: // Functions from base classes

		/**
		* CreateAppUiL
		* From CEikDocument, CreateAppUiL.
		* Create a CAafAppUi object and return a pointer to it.
		* The object returned is owned by the Uikon framework.
		* @return Pointer to created instance of AppUi.
		*/
		CEikAppUi* CreateAppUiL();

	private: // Constructors

		/**
		* ConstructL
		* 2nd phase constructor.
		*/
		void ConstructL();

		/**
		* CAafDocument.
		* C++ default constructor.
		* @param aApp Application creating this document.
		*/
		CAafDocument( CEikApplication& aApp );

	};

#endif // __AAFDOCUMENT_H__

// End of File
