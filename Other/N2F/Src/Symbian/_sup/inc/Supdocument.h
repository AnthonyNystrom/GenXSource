/*
============================================================================
 Name        : SupDocument.h
 Author      : Vitaly Vinogradov
 Version     : 1.0.0
 Copyright   : (c) Next2Friends, 2008
 Description : Application document class (model)
============================================================================
*/

#ifndef __SUPDOCUMENT_H__
#define __SUPDOCUMENT_H__

// INCLUDES
#include <akndoc.h>

// FORWARD DECLARATIONS
class CSupAppUi;
class CEikApplication;


// CLASS DECLARATION

/**
* CSupDocument application class.
* An instance of class CSupDocument is the Document part of the
* AVKON application framework for the Sup example application.
*/
class CSupDocument : public CAknDocument
	{
	public: // Constructors and destructor

		/**
		* NewL.
		* Two-phased constructor.
		* Construct a CSupDocument for the AVKON application aApp
		* using two phase construction, and return a pointer
		* to the created object.
		* @param aApp Application creating this document.
		* @return A pointer to the created instance of CSupDocument.
		*/
		static CSupDocument* NewL( CEikApplication& aApp );

		/**
		* NewLC.
		* Two-phased constructor.
		* Construct a CSupDocument for the AVKON application aApp
		* using two phase construction, and return a pointer
		* to the created object.
		* @param aApp Application creating this document.
		* @return A pointer to the created instance of CSupDocument.
		*/
		static CSupDocument* NewLC( CEikApplication& aApp );

		/**
		* ~CSupDocument
		* Virtual Destructor.
		*/
		virtual ~CSupDocument();

	public: // Functions from base classes

		/**
		* CreateAppUiL
		* From CEikDocument, CreateAppUiL.
		* Create a CSupAppUi object and return a pointer to it.
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
		* CSupDocument.
		* C++ default constructor.
		* @param aApp Application creating this document.
		*/
		CSupDocument( CEikApplication& aApp );

	};

#endif // __SUPDOCUMENT_H__

// End of File
