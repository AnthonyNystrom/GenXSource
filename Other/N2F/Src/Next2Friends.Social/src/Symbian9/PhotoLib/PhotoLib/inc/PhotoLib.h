/*
============================================================================
 Name        : $(EngineBasename).h
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : PhotoLib.h - CPhotoLib class header
               Defines the API for PhotoLib.lib
============================================================================
*/

#ifndef __PHOTOLIB_H__
#define __PHOTOLIB_H__

// INCLUDES
#include <e32base.h>    // CBase
#include <e32std.h>     // TBuf

// CONSTANTS
const TInt KPhotoLibBufferLength = 15;
typedef TBuf<KPhotoLibBufferLength> TPhotoLibExampleString;

//  CLASS DEFINITIONS

class CPhotoLib : public CBase
    {
    public:     // new functions
        static CPhotoLib* NewL();
        static CPhotoLib* NewLC();
        ~CPhotoLib();

    public:     // new functions, example API
        TVersion Version() const;
        void ExampleFuncAddCharL(const TChar& aChar);
        void ExampleFuncRemoveLast();
        void ExampleFuncClearBuffer();
        const TPtrC ExampleFuncString() const;

    private:    // new functions
        CPhotoLib();
        void ConstructL();

    private:    // data
        TPhotoLibExampleString* iString;
    };


#endif  // __PHOTOLIB_H__


