#ifndef _REPORTENTITYSETTINGS_H_
#define _REPORTENTITYSETTINGS_H_

#include "ReportEntityLabel.h"
#include "ReportEntityBox.h"
#include "ReportEntityLine.h"
#include "ReportEntityPicture.h"

class CReportEntitySettings
{

public:

    static CReportEntitySettings * GetRESInstance();

	void SetLogFont( LOGFONT& logfont );
	void SetColor( unsigned int color );
	void SetJustification( unsigned int justification );
	void SetTextAngle(int newAng) {m_text_angle = newAng;};
	int  GetTextAngle() {return m_text_angle;};

	void GetFontSettings( CReportEntityLabel* obj );
	
	void GetBorderSettings( CReportEntityLabel* obj );
	void GetBorderSettings( CReportEntityBox* obj );
	void GetBorderSettings( CReportEntityPicture* obj );
	
	void GetLineSettings( CReportEntityLine* obj );

	void GetFillSettings( CReportEntityBox* obj );
	
	void SetLineColor( unsigned int color );
	void SetLineStyle( unsigned int style );
	void SetLineThickness( unsigned int thickness );
	void SetFill( BOOL fill );
	void SetFillColor( unsigned int color );
	void SetBorder( BOOL border );
	void SetBorderColor( unsigned int color );
	void SetBorderStyle( unsigned int style  );
	void SetBorderThickness( unsigned int thickness );

	void SetColumn( BOOL border );
	void SetColumnColor( unsigned int color );
	void SetColumnStyle( unsigned int style  );
	void SetColumnThickness( unsigned int thickness );

	void SetRow( BOOL border );
	void SetRowColor( unsigned int color );
	void SetRowStyle( unsigned int style  );
	void SetRowThickness( unsigned int thickness );

	void SetMeasurementUnits( unsigned int measurementunits );
	int GetMeasurementUnits();

    virtual ~CReportEntitySettings(){};

protected:

    CReportEntitySettings();
    static CReportEntitySettings m_reportEntitySettings;

private:

	LOGFONT		m_lf;
	unsigned int			m_justification;
	int             m_text_angle;
	unsigned int	m_color;

	unsigned int	m_linecolor;
	unsigned int			m_linestyle;
	unsigned int			m_linethickness;

	BOOL		m_fill;
	unsigned int	m_fillcolor;

	BOOL		m_border;
	unsigned int	m_bordercolor;
	unsigned int			m_borderstyle;
	unsigned int			m_borderthickness;

	BOOL		m_column;
	unsigned int	m_columncolor;
	unsigned int			m_columnstyle;
	unsigned int			m_columnthickness;

	BOOL		m_row;
	unsigned int	m_rowcolor;
	unsigned int	m_rowstyle;
	unsigned int	m_rowthickness;

	unsigned int	m_measurementunits;

};

#endif //_REPORTENTITYSETTINGS_H_