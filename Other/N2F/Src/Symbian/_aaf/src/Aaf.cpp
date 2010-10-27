/*
============================================================================
 Name        : Aaf.cpp
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application entry point
============================================================================
*/

// INCLUDE FILES
#include <eikstart.h>
#include "AafApplication.h"


LOCAL_C CApaApplication* NewApplication()
	{
	return new CAafApplication;
	}

GLDEF_C TInt E32Main()
	{
	return EikStart::RunApplication( NewApplication );
	}
