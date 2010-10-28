/* ==========================================================================
	Class :			CReportControlFactory

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportControlFactory" is a utility class to create 
					drawing objects from text strings.

	Description :	The single static function calls corresponding static 
					functions in the different drawing objects to try to 
					create an object.

	Usage :			Call from the loading functionality.

   ========================================================================*/
#include "stdafx.h"
#include "ReportControlFactory.h"

#include "ReportEntityBox.h"
#include "ReportEntityLine.h"
#include "ReportEntityLabel.h"
#include "ReportEntityPicture.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CReportControlFactory

CDiagramEntity* CReportControlFactory::CreateFromString( const CString& str )
/* ============================================================
	Function :		CReportControlFactory::CreateFromString
	Description :	Tries to create an object from "str"
	Access :		Public

	Return :		CDiagramEntity*		-	Resulting object or 
											NULL
	Parameters :	const CString& str	-	String representing 
											an object.
					
	Usage :			Call from a load-function.

   ============================================================*/
{
	CDiagramEntity* obj;

	obj = CReportEntityBox::CreateFromString( str );

	if( !obj )
		obj = CReportEntityLine::CreateFromString( str );
	if( !obj )
		obj = CReportEntityLabel::CreateFromString( str );
	if( !obj )
		obj = CReportEntityPicture::CreateFromString( str );

	return obj;
}