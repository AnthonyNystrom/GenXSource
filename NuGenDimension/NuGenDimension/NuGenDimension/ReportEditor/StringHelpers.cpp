/* ==========================================================================
	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-06-21

	Purpose :		Misc string-related functions

	Description :	Contains several string-related function.

	Usage :			Call as needed.

   ========================================================================*/
#include "stdafx.h"
#include "StringHelpers.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

void MakeSaveString( CString& str )
{

	// This function replaces some non-alphanumeric 
	// character with tag-codes, as the characters 
	// are used for delimiting different kind of 
	// substrings.

	str.Replace( _T( ":" ), _T( "\\colon" ) );
	str.Replace( _T( ";" ), _T( "\\semicolon" ) );
	str.Replace( _T( "," ), _T( "\\comma" ) );
	str.Replace( _T( "|" ), _T( "\\bar" ) );
	str.Replace( _T( "#" ), _T( "\\hash" ) );
	str.Replace( _T( "\r\n" ), _T( "\\newline" ) );

}

void UnmakeSaveString( CString& str )
{

	// The function replaces some tag-strings 
	// with the corresponding characters after 
	// loading the string from file.

	str.Replace( _T( "\\colon" ), _T( ":" ) );
	str.Replace( _T( "\\semicolon" ) , _T( ";" ) );
	str.Replace( _T( "\\comma" ), _T( "," ) );
	str.Replace( _T( "\\bar" ), _T( "|" ) );
	str.Replace( _T( "\\hash" ), _T( "#" ) );
	str.Replace( _T( "\\newline" ), _T( "\r\n" ) );

}

