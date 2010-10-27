/*
============================================================================
Name        : SupApplication.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Main application class
============================================================================
*/

#ifndef __SUPAPPLICATION_H__
#define __SUPAPPLICATION_H__

// INCLUDES
#include <aknapp.h>
#include "Sup.hrh"

// UID for the application;
// this should correspond to the uid defined in the mmp file
const TUid KUidSupApp = { _UID3 };

// CLASS DECLARATION

/**
* CSupApplication application class.
* Provides factory to create concrete document object.
* An instance of CSupApplication is the application part of the
* AVKON application framework for the Sup example application.
*/
class CSupApplication : public CAknApplication
{
public: // Functions from base classes

	/**
	* From CApaApplication, AppDllUid.
	* @return Application's UID (KUidSupApp).
	*/
	TUid AppDllUid() const;

protected: // Functions from base classes

	/**
	* From CApaApplication, CreateDocumentL.
	* Creates CSupDocument document object. The returned
	* pointer in not owned by the CSupApplication object.
	* @return A pointer to the created document object.
	*/
	CApaDocument* CreateDocumentL();
};

#endif // __SUPAPPLICATION_H__

// End of File
