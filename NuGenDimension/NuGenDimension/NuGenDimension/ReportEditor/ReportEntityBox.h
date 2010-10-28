#ifndef _CREPORTENTITYBOX_H_C3863707_6C7A_43F6_B33FFC4BE4D5
#define _CREPORTENTITYBOX_H_C3863707_6C7A_43F6_B33FFC4BE4D5

///////////////////////////////////////////////////////////
// File :		ReportEntityBox.h
// Created :	07/14/04
//

#include "DiagramEditor/DiagramEntity.h"
#include "ReportBoxProperties.h"

class IThumbnailerStorage;

class CReportEntityBox : public CDiagramEntity
{
public:
// Construction/destruction
	CReportEntityBox();
	virtual ~CReportEntityBox();

	virtual DIAGRAM_OBJECT_TYPE  GetEntityType() {return DIAGRAM_RECT;};


	virtual CDiagramEntity* Clone();
	virtual void Copy( CDiagramEntity * obj );
	virtual BOOL	FromString( const CString& str );
	virtual CString	GetString() const;
	static CDiagramEntity* CreateFromString( const CString & str );

	virtual void    Serialize(CArchive& ar);

	virtual void			Draw( CDC* dc, CRect rect );

// Operations
	unsigned int GetBorderThickness() const;
	void SetBorderThickness( unsigned int value );
	unsigned int GetBorderStyle() const;
	void SetBorderStyle( unsigned int value );
	unsigned int GetBorderColor() const;
	void SetBorderColor( unsigned int value );
	BOOL GetFill() const;
	void SetFill( BOOL value );
	unsigned int GetFillColor() const;
	void SetFillColor( unsigned int value );

// Attributes

private:
	unsigned int m_borderThickness;
	unsigned int m_borderStyle;
	unsigned int m_borderColor;
	BOOL m_fill;
	unsigned int m_fillColor;

	CReportBoxProperties	m_dlg;

};
#endif //_CREPORTENTITYBOX_H_C3863707_6C7A_43F6_B33FFC4BE4D5
