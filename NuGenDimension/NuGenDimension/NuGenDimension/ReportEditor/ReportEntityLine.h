#ifndef _CREPORTENTITYLINE_H_9D35E8C_89C8_4D2A_B89FB0442EAF
#define _CREPORTENTITYLINE_H_9D35E8C_89C8_4D2A_B89FB0442EAF

///////////////////////////////////////////////////////////
// File :		ReportEntityLine.h
// Created :	07/14/04
//

#include "DiagramEditor/DiagramLine.h"
#include "ReportLineProperties.h"

class IThumbnailerStorage;

class CReportEntityLine : public CDiagramLine
{
public:
// Construction/destruction
	CReportEntityLine();
	virtual ~CReportEntityLine();

	virtual CDiagramEntity* Clone();
	virtual void Copy( CDiagramEntity * obj );
	virtual BOOL	FromString( const CString& str );
	virtual CString	GetString() const;
	static CDiagramEntity* CreateFromString( const CString & str );

	virtual void    Serialize(CArchive& ar);

	virtual void			Draw( CDC* dc, CRect rect );

// Operations
	unsigned int GetBorderThickness() const;
	void		 SetBorderThickness( unsigned int value );
	unsigned int GetBorderColor() const;
	void		 SetBorderColor( unsigned int value );


// Attributes

private:
	unsigned int m_borderThickness;
	unsigned int m_borderColor;

	CReportLineProperties	m_dlg;

};
#endif //_CREPORTENTITYLINE_H_9D35E8C_89C8_4D2A_B89FB0442EAF
