/*
============================================================================
Name        : SupApplication.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Main application class
============================================================================
*/

// INCLUDE FILES
#include "SupDocument.h"
#include "SupApplication.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupApplication::CreateDocumentL()
// Creates CApaDocument object
// -----------------------------------------------------------------------------
//
CApaDocument* CSupApplication::CreateDocumentL()
{
	// Create an Sup document, and return a pointer to it
	return (static_cast<CApaDocument*>
		( CSupDocument::NewL( *this ) ) );
}

// -----------------------------------------------------------------------------
// CSupApplication::AppDllUid()
// Returns application UID
// -----------------------------------------------------------------------------
//
TUid CSupApplication::AppDllUid() const
{
	// Return the UID for the Sup application
	return KUidSupApp;
}

// End of File
