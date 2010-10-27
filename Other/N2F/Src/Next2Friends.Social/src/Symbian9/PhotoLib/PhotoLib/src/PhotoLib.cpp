/*
============================================================================
 Name        : CPhotoLib from PhotoLib.h
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : CPhotoLib LIB source
============================================================================
*/

//  Include Files  

#include "PhotoLib.h"	// CPhotoLib
#include "PhotoLib.pan"   // panic codes

//  Member Functions

CPhotoLib* CPhotoLib::NewLC()
    {
    CPhotoLib* self = new (ELeave) CPhotoLib;
    CleanupStack::PushL(self);
    self->ConstructL();
    return self;
    }


CPhotoLib* CPhotoLib::NewL()
    {
    CPhotoLib* self = CPhotoLib::NewLC();
    CleanupStack::Pop(self);
    return self;
    }


CPhotoLib::CPhotoLib()
    {
    // note, CBase initialises all member variables to zero
    }


void CPhotoLib::ConstructL()
    {
    // second phase constructor, anything that may leave must be constructed here
    iString = new (ELeave) TPhotoLibExampleString;
    }


CPhotoLib::~CPhotoLib()
    {
    delete iString;
    }


TVersion CPhotoLib::Version() const
    {
    // Version number of example API
    const TInt KMajor = 1;
    const TInt KMinor = 0;
    const TInt KBuild = 1;
    return TVersion(KMajor, KMinor, KBuild);
    }


void CPhotoLib::ExampleFuncAddCharL(const TChar& aChar)
    {
    __ASSERT_ALWAYS(iString != NULL, Panic(EPhotoLibNullPointer));

    if (iString->Length() >= KPhotoLibBufferLength)
        {
        User::Leave(KErrTooBig);
        }

    iString->Append(aChar);
    }

void CPhotoLib::ExampleFuncClearBuffer()
	{
	iString->Zero();
    }

void CPhotoLib::ExampleFuncRemoveLast()
    {
    __ASSERT_ALWAYS(iString != NULL, Panic(EPhotoLibNullPointer));

    if (iString->Length() > 0)
        {
        iString->SetLength(iString->Length() - 1);
        }
    }


const TPtrC CPhotoLib::ExampleFuncString() const
    {
    __ASSERT_ALWAYS(iString != NULL, Panic(EPhotoLibNullPointer));
    return *iString;
    }



