/*
============================================================================
Name        : SupDocument.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Application document class (model)
============================================================================
*/

// INCLUDE FILES
#include "SupAppUi.h"
#include "SupDocument.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CSupDocument::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupDocument* CSupDocument::NewL( CEikApplication&
								 aApp )
{
	CSupDocument* self = NewLC( aApp );
	CleanupStack::Pop( self );
	return self;
}

// -----------------------------------------------------------------------------
// CSupDocument::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CSupDocument* CSupDocument::NewLC( CEikApplication&
								  aApp )
{
	CSupDocument* self =
		new ( ELeave ) CSupDocument( aApp );

	CleanupStack::PushL( self );
	self->ConstructL();
	return self;
}

// -----------------------------------------------------------------------------
// CSupDocument::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CSupDocument::ConstructL()
{
	// No implementation required
}

// -----------------------------------------------------------------------------
// CSupDocument::CSupDocument()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CSupDocument::CSupDocument( CEikApplication& aApp )
: CAknDocument( aApp )
{
	// No implementation required
}

// ---------------------------------------------------------------------------
// CSupDocument::~CSupDocument()
// Destructor.
// ---------------------------------------------------------------------------
//
CSupDocument::~CSupDocument()
{
	// No implementation required
}

// ---------------------------------------------------------------------------
// CSupDocument::CreateAppUiL()
// Constructs CreateAppUi.
// ---------------------------------------------------------------------------
//
CEikAppUi* CSupDocument::CreateAppUiL()
{
	// Create the application user interface, and return a pointer to it;
	// the framework takes ownership of this object
	return ( static_cast <CEikAppUi*> ( new ( ELeave )
		CSupAppUi ) );
}

// End of File
