/*
============================================================================
 Name        : AafApplication.cpp
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Main application class
============================================================================
*/

// INCLUDE FILES
#include "AafDocument.h"
#include "AafApplication.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafApplication::CreateDocumentL()
// Creates CApaDocument object
// -----------------------------------------------------------------------------
//
CApaDocument* CAafApplication::CreateDocumentL()
	{
	// Create an Aaf document, and return a pointer to it
	return (static_cast<CApaDocument*>
		( CAafDocument::NewL( *this ) ) );
	}

// -----------------------------------------------------------------------------
// CAafApplication::AppDllUid()
// Returns application UID
// -----------------------------------------------------------------------------
//
TUid CAafApplication::AppDllUid() const
	{
	// Return the UID for the Aaf application
	return KUidAafApp;
	}

// End of File
