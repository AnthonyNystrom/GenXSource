/*
============================================================================
 Name        : AafDocument.cpp
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application document class (model)
============================================================================
*/

// INCLUDE FILES
#include "AafAppUi.h"
#include "AafDocument.h"

// ============================ MEMBER FUNCTIONS ===============================

// -----------------------------------------------------------------------------
// CAafDocument::NewL()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafDocument* CAafDocument::NewL( CEikApplication&
	aApp )
	{
	CAafDocument* self = NewLC( aApp );
	CleanupStack::Pop( self );
	return self;
	}

// -----------------------------------------------------------------------------
// CAafDocument::NewLC()
// Two-phased constructor.
// -----------------------------------------------------------------------------
//
CAafDocument* CAafDocument::NewLC( CEikApplication&
	aApp )
	{
	CAafDocument* self =
		new ( ELeave ) CAafDocument( aApp );

	CleanupStack::PushL( self );
	self->ConstructL();
	return self;
	}

// -----------------------------------------------------------------------------
// CAafDocument::ConstructL()
// Symbian 2nd phase constructor can leave.
// -----------------------------------------------------------------------------
//
void CAafDocument::ConstructL()
	{
	// No implementation required
	}

// -----------------------------------------------------------------------------
// CAafDocument::CAafDocument()
// C++ default constructor can NOT contain any code, that might leave.
// -----------------------------------------------------------------------------
//
CAafDocument::CAafDocument( CEikApplication& aApp )
	: CAknDocument( aApp )
	{
	// No implementation required
	}

// ---------------------------------------------------------------------------
// CAafDocument::~CAafDocument()
// Destructor.
// ---------------------------------------------------------------------------
//
CAafDocument::~CAafDocument()
	{
	// No implementation required
	}

// ---------------------------------------------------------------------------
// CAafDocument::CreateAppUiL()
// Constructs CreateAppUi.
// ---------------------------------------------------------------------------
//
CEikAppUi* CAafDocument::CreateAppUiL()
	{
	// Create the application user interface, and return a pointer to it;
	// the framework takes ownership of this object
	return ( static_cast <CEikAppUi*> ( new ( ELeave )
		CAafAppUi ) );
	}

// End of File
