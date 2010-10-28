#ifndef _CREPORTENTITYPICTURE_H_C3863707_6C7A_43F6_B33FFC4BE4D5
#define _CREPORTENTITYPICTURE_H_C3863707_6C7A_43F6_B33FFC4BE4D5

///////////////////////////////////////////////////////////
// File :		ReportEntityPicture.h
// Created :	07/14/04
//

#include "DiagramEditor/DiagramEntity.h"
#include "ReportPictureProperties.h"

class IThumbnailerStorage;

class CReportEntityPicture : public CDiagramEntity
{
public:
// Construction/destruction
	CReportEntityPicture();
	CReportEntityPicture(HENHMETAFILE hMF);
	virtual ~CReportEntityPicture();

	virtual DIAGRAM_OBJECT_TYPE  GetEntityType() {return DIAGRAM_PICTURE;};


	virtual CDiagramEntity* Clone();
	virtual void Copy( CDiagramEntity * obj );
	virtual BOOL	FromString( const CString& str );
	virtual CString	GetString() const;
	static CDiagramEntity* CreateFromString( const CString & str );

	virtual void    Serialize(CArchive& ar);

	virtual void  Draw( CDC* dc, CRect rect );

// Operations
	unsigned int GetBorderThickness() const;
	void SetBorderThickness( unsigned int value );
	unsigned int GetBorderStyle() const;
	void SetBorderStyle( unsigned int value );
	unsigned  GetBorderColor() const;
	void SetBorderColor( unsigned int value );
	void SetFilename( const CString& filename );
	CString GetFilename() const;

	void AdjustSize();

// Attributes

private:
	unsigned int m_borderThickness;
	unsigned int m_borderStyle;
	unsigned int m_borderColor;
	BOOL m_fill;
	unsigned int m_fillColor;

	CString m_filename;

	int            m_image_type;
	unsigned int   m_image_size;
	BYTE*          m_image_bits;

	CxImage*  m_bitmap;

	CReportPictureProperties	m_dlg;
public:
	CSize  GetPictureSizes() 
	{
		CSize resSz(0,0);
		if (m_bitmap)
		{
			resSz.cx=m_bitmap->GetWidth();
			resSz.cy=m_bitmap->GetHeight();
		}
		return resSz;
	}
};
#endif //_CREPORTENTITYPICTURE_H_C3863707_6C7A_43F6_B33FFC4BE4D5
