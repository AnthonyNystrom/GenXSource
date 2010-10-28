#ifndef _REPORTENTITYLABEL_H_
#define _REPORTENTITYLABEL_H_

#include "DiagramEditor/DiagramEntity.h"
#include "ReportLabelProperties.h"

class CReportEntityLabel : public CDiagramEntity
{
public:
	CReportEntityLabel();
	~CReportEntityLabel();

	virtual DIAGRAM_OBJECT_TYPE  GetEntityType() {return DIAGRAM_LABEL;};


	virtual	CDiagramEntity* Clone();
	virtual void	Copy( CDiagramEntity* obj );
	virtual BOOL	FromString( const CString& str );
	virtual CString	GetString() const;
	static CDiagramEntity* CreateFromString( const CString & str );

	virtual void    Serialize(CArchive& ar);

	virtual void			Draw( CDC* dc, CRect rect );

	CString GetFontName() const;
	void SetFontName( CString value );

	void SetCharSet(BYTE nCS) {m_fontCharSet=nCS;};
	BYTE GetCharSet() const {return m_fontCharSet;};

	int GetFontSize() const;
	void SetFontSize( int value );
	BOOL GetFontBold() const;
	void SetFontBold( BOOL value );
	BOOL GetFontItalic() const;
	void SetFontItalic( BOOL value );
	BOOL GetFontUnderline() const;
	void SetFontUnderline( BOOL value );
	BOOL GetFontStrikeout() const;
	void SetFontStrikeout( BOOL value );
	COLORREF GetFontColor() const;
	void SetFontColor( COLORREF value );
	void SetJustification( unsigned int justification );
	unsigned int GetJustification() const;

	int GetAngle() {return m_angle;};
	void SetAngle(int nA) {m_angle=nA;};

	unsigned int GetBorderThickness() const;
	void SetBorderThickness( unsigned int value );
	unsigned int GetBorderStyle() const;
	void SetBorderStyle( unsigned int value );
	unsigned int GetBorderColor() const;
	void SetBorderColor( unsigned int value );

private:

	CString		m_fontName;
	int			m_fontSize;
	BYTE        m_fontCharSet;
	BOOL		m_fontBold;
	BOOL		m_fontItalic;
	BOOL		m_fontUnderline;
	BOOL		m_fontStrikeout;
	COLORREF	m_fontColor;
	unsigned int	m_justification;

	unsigned int m_borderThickness;
	unsigned int m_borderStyle;
	unsigned int m_borderColor;

	int          m_angle;

	CReportLabelProperties	m_dlg;

};

#endif //_REPORTENTITYLABEL_H_