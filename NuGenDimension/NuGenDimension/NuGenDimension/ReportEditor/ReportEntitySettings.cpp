/* ==========================================================================
	Class :			CReportEntitySettings

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		"CReportEntitySettings" is holding default values for new 
					instances of different drawing objects.

	Description :	The class is a singleton class that is updated from the 
					property dialogs to make all newly created objects get 
					the attributes of the last created or edited one.

	Usage :			Call the getters to set default data to newly created 
					object. Call the setters as soon as an object has been 
					edited.

   ========================================================================*/
#include "stdafx.h"
#include "ReportEntitySettings.h"

CReportEntitySettings::CReportEntitySettings()
/* ============================================================
	Function :		CReportEntitySettings::CReportEntitySettings
	Description :	Constructor
	Access :		Public

	Return :		void
	Parameters :	none

	Usage :			

   ============================================================*/
{

	ZeroMemory( &m_lf, sizeof( m_lf ) );

	m_lf.lfHeight = 360;
	m_lf.lfWeight = FW_NORMAL;
	lstrcpy( m_lf.lfFaceName, _T( "Times New Roman" ) );

	m_color = 0;
	m_justification = DT_LEFT;

	m_text_angle = 0;

	m_linecolor = 0;
	m_linestyle = DIAGRAM_FRAME_STYLE_LEFT|DIAGRAM_FRAME_STYLE_RIGHT|
				DIAGRAM_FRAME_STYLE_TOP|DIAGRAM_FRAME_STYLE_BOTTOM;
	m_linethickness = 0;

	m_fill = FALSE;
	m_fillcolor = 0;

	m_border = TRUE;
	m_bordercolor = 0;
	m_borderstyle = DIAGRAM_FRAME_STYLE_LEFT|DIAGRAM_FRAME_STYLE_RIGHT|
		DIAGRAM_FRAME_STYLE_TOP|DIAGRAM_FRAME_STYLE_BOTTOM;
	m_borderthickness = 0;

	m_measurementunits = 0;

}

CReportEntitySettings* CReportEntitySettings::GetRESInstance()        
/* ============================================================
	Function :		CReportEntitySettings::GetRESInstance
	Description :	Get the singleton instance of this class.
	Access :		Public

	Return :		CReportEntitySettings*	-	Instance
	Parameters :	none

	Usage :			Call to get access to this singleton.

   ============================================================*/
{

    return &m_reportEntitySettings;

}

void CReportEntitySettings::SetLogFont( LOGFONT& lf )
/* ============================================================
	Function :		CReportEntitySettings::SetLogFont
	Description :	Sets the font of the object.
	Access :		Public

	Return :		void
	Parameters :	LOGFONT& lf	-	Font to set
					
	Usage :			Call to set the font of the settings 
					singleton.

   ============================================================*/
{

	memcpy( &m_lf, &lf, sizeof( m_lf ) );

}

void CReportEntitySettings::SetColor( unsigned int color )
/* ============================================================
	Function :		CReportEntitySettings::SetColor
	Description :	Sets the color of the singleton
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	Color to set
					
	Usage :			Call to set the color of the settings 
					singleton.

   ============================================================*/
{

	m_color = color;

}

void CReportEntitySettings::SetJustification( unsigned int justification )
/* ============================================================
	Function :		CReportEntitySettings::SetJustification
	Description :	Sets the justification of the singleton
	Access :		Public

	Return :		void
	Parameters :	int justification	-	Justification to set
					
	Usage :			Call to set the justification of the settings 
					singleton.

   ============================================================*/
{

	m_justification = justification;

}

void CReportEntitySettings::GetFontSettings( CReportEntityLabel* obj )
/* ============================================================
	Function :		CReportEntitySettings::GetFontSettings
	Description :	Sets the font in "obj" to the font 
					settings in the singleton.
	Access :		Public

	Return :		void
	Parameters :	CReportEntityLabel* obj	-	Object to set
												data for
					
	Usage :			Call to set default values for "obj".

   ============================================================*/
{

	obj->SetFontName( m_lf.lfFaceName );
	obj->SetCharSet(m_lf.lfCharSet);
	obj->SetFontSize( m_lf.lfHeight );
	obj->SetFontBold( ( m_lf.lfWeight != FW_NORMAL ) );
	obj->SetFontItalic( m_lf.lfItalic );
	obj->SetFontStrikeout( m_lf.lfStrikeOut );
	obj->SetFontUnderline( m_lf.lfUnderline );

	obj->SetFontColor( m_color );
	obj->SetJustification( m_justification );

}

void CReportEntitySettings::GetBorderSettings( CReportEntityBox* obj )
/* ============================================================
	Function :		CReportEntitySettings::GetBorderSettings
	Description :	Sets the borders in "obj" to the border 
					settings in the singleton.
	Access :		Public

	Return :		void
	Parameters :	CReportEntityBox* obj	-	Object to set
												data for	
					
	Usage :			Call to set default values for "obj".

   ============================================================*/
{

	obj->SetBorderColor( m_bordercolor );
	obj->SetBorderStyle( m_borderstyle );
	obj->SetBorderThickness( m_borderthickness );

}

void CReportEntitySettings::GetBorderSettings( CReportEntityLabel* obj )
{

	obj->SetBorderColor( m_bordercolor );
	obj->SetBorderStyle( m_borderstyle );
	obj->SetBorderThickness( m_borderthickness );

}

void CReportEntitySettings::GetLineSettings( CReportEntityLine* obj )
/* ============================================================
	Function :		CReportEntitySettings::GetLineSettings
	Description :	Sets the line in "obj" to the line
					settings in the singleton.
	Access :		Public

	Return :		void
	Parameters :	CReportEntityLine* obj	-	Object to set
												data for	
					
	Usage :			Call to set default values for "obj".

   ============================================================*/
{

	obj->SetBorderColor( m_linecolor );
	obj->SetBorderThickness( m_linethickness );

}

void CReportEntitySettings::GetFillSettings( CReportEntityBox* obj )
/* ============================================================
	Function :		CReportEntitySettings::GetFillSettings
	Description :	Sets the fill flag and -color from the 
					default members.
	Access :		Public

	Return :		void
	Parameters :	CReportEntityBox* obj	-	Object to set 
												default data for.
					
	Usage :			Call to set default values for "obj"

   ============================================================*/
{

	obj->SetFill( m_fill );
	obj->SetFillColor( m_fillcolor );

}

void CReportEntitySettings::SetLineColor( unsigned int color )
/* ============================================================
	Function :		CReportEntitySettings::SetLineColor
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_linecolor = color;

}

void CReportEntitySettings::SetLineStyle( unsigned int style )
/* ============================================================
	Function :		CReportEntitySettings::SetLineStyle
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int style	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_linestyle = style;

}

void CReportEntitySettings::SetLineThickness( unsigned int thickness )
/* ============================================================
	Function :		CReportEntitySettings::SetLineThickness
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int thickness	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_linethickness = thickness;

}

void CReportEntitySettings::SetFill( BOOL fill )
/* ============================================================
	Function :		CReportEntitySettings::SetFill
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	BOOL fill	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_fill = fill;

}

void CReportEntitySettings::SetFillColor( unsigned int color )
/* ============================================================
	Function :		CReportEntitySettings::SetFillColor
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_fillcolor = color;

}

void CReportEntitySettings::SetBorder( BOOL border )
/* ============================================================
	Function :		CReportEntitySettings::SetBorder
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	BOOL border	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_border = border;

}

void CReportEntitySettings::SetBorderColor( unsigned int color )
/* ============================================================
	Function :		CReportEntitySettings::SetBorderColor
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_bordercolor = color;

}

void CReportEntitySettings::SetBorderStyle( unsigned int style  )
/* ============================================================
	Function :		CReportEntitySettings::SetBorderStyle
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int style	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_borderstyle = style;

}

void CReportEntitySettings::SetBorderThickness( unsigned int thickness )
/* ============================================================
	Function :		CReportEntitySettings::SetBorderThickness
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int thickness	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_borderthickness = thickness;

}

void CReportEntitySettings::SetColumn( BOOL column )
/* ============================================================
	Function :		CReportEntitySettings::SetColumn
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	BOOL column	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_column = column;

}

void CReportEntitySettings::SetColumnColor( unsigned int color )
/* ============================================================
	Function :		CReportEntitySettings::SetColumnColor
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_columncolor = color;

}

void CReportEntitySettings::SetColumnStyle( unsigned int style  )
/* ============================================================
	Function :		CReportEntitySettings::SetColumnStyle
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int style	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_columnstyle = style;

}

void CReportEntitySettings::SetColumnThickness( unsigned int thickness )
/* ============================================================
	Function :		CReportEntitySettings::SetColumnThickness
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int thickness	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_columnthickness = thickness;

}

void CReportEntitySettings::SetRow( BOOL row )
/* ============================================================
	Function :		CReportEntitySettings::SetRow
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	BOOL row	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_row = row;

}

void CReportEntitySettings::SetRowColor( unsigned int color )
/* ============================================================
	Function :		CReportEntitySettings::SetRowColor
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	COLORREF color	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_rowcolor = color;

}

void CReportEntitySettings::SetRowStyle( unsigned int style  )
/* ============================================================
	Function :		CReportEntitySettings::SetRowStyle
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int style	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_rowstyle = style;

}

void CReportEntitySettings::SetRowThickness( unsigned int thickness )
/* ============================================================
	Function :		CReportEntitySettings::SetRowThickness
	Description :	Attribute setter.
	Access :		Public

	Return :		void
	Parameters :	int thickness	-	New value
					
	Usage :			Call to set the attribute

   ============================================================*/
{

	m_rowthickness = thickness;

}

void CReportEntitySettings::GetBorderSettings( CReportEntityPicture* obj )
/* ============================================================
	Function :		CReportEntitySettings::GetBorderSettings
	Description :	Sets the border in "obj" to the border 
					settings in the singleton.
	Access :		Public
	Access :		Public

	Return :		void
	Parameters :	CReportEntityPicture* obj	-	
					
	Usage :			Call to set default values for "obj".

   ============================================================*/
{

//	obj->SetBorder( m_border );
	obj->SetBorderColor( m_bordercolor );
	obj->SetBorderStyle( m_borderstyle );
	obj->SetBorderThickness( m_borderthickness );

}

void CReportEntitySettings::SetMeasurementUnits( unsigned int measurementunits )
/* ============================================================
	Function :		CReportEntitySettings::SetMeasurementUnits
	Description :	Sets the current measurement.
	Access :		Public

	Return :		void
	Parameters :	int measurementunits	-	Measurements to use
					
	Usage :			Accessor.

   ============================================================*/
{

	m_measurementunits = measurementunits;

}

int CReportEntitySettings::GetMeasurementUnits()
/* ============================================================
	Function :		CReportEntitySettings::GetMeasurementUnits
	Description :	Gets the current measurements.
	Access :		Public

	Return :		int	-	Current measurements
	Parameters :	none

	Usage :			Accessor.

   ============================================================*/
{

	return m_measurementunits;

}

CReportEntitySettings CReportEntitySettings::m_reportEntitySettings;

