/*
============================================================================
 Name        : AafApplication.h
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Main application class
============================================================================
*/

#ifndef __AAFAPPLICATION_H__
#define __AAFAPPLICATION_H__

// INCLUDES
#include <aknapp.h>
#include "Aaf.hrh"

// UID for the application;
// this should correspond to the uid defined in the mmp file
const TUid KUidAafApp = { _UID3 };

// CLASS DECLARATION

/**
* CAafApplication application class.
* Provides factory to create concrete document object.
* An instance of CAafApplication is the application part of the
* AVKON application framework for the Aaf example application.
*/
class CAafApplication : public CAknApplication
	{
	public: // Functions from base classes

		/**
		* From CApaApplication, AppDllUid.
		* @return Application's UID (KUidAafApp).
		*/
		TUid AppDllUid() const;

	protected: // Functions from base classes

		/**
		* From CApaApplication, CreateDocumentL.
		* Creates CAafDocument document object. The returned
		* pointer in not owned by the CAafApplication object.
		* @return A pointer to the created document object.
		*/
		CApaDocument* CreateDocumentL();
	};

#endif // __AAFAPPLICATION_H__

// End of File
