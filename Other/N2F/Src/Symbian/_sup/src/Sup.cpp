/*
============================================================================
 Name        : Sup.cpp
 Author      : Vitaly Vinogradov
 Version     : 1.0.0
 Copyright   : (c) Next2Friends, 2008
 Description : Application entry point
============================================================================
*/

// INCLUDE FILES
#include <eikstart.h>
#include "SupApplication.h"


LOCAL_C CApaApplication* NewApplication()
	{
	return new CSupApplication;
	}

GLDEF_C TInt E32Main()
	{
	return EikStart::RunApplication( NewApplication );
	}
