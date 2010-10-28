#ifndef _DIAGRAMLINE_H_
#define _DIAGRAMLINE_H_

#include "DiagramEntity.h"
#include "HitParams.h"
#include "HitParamsRect.h"

class IThumbnailerStorage;

class CDiagramLine : public CDiagramEntity
{
public:
	CDiagramLine();
	virtual ~CDiagramLine();

	virtual DIAGRAM_OBJECT_TYPE  GetEntityType() {return DIAGRAM_LINE;};


	virtual CDiagramEntity* Clone();
	static	CDiagramEntity* CreateFromString( const CString& str );
	virtual void			Draw( CDC* dc, CRect rect );
	virtual int				GetHitCode( CPoint point ) const;
	virtual int				GetHitCode( const CPoint& point, const CRect& rect ) const;
	virtual HCURSOR			GetCursor( int hit ) const;
	virtual void			SetRect( CRect rect );
	virtual BOOL			BodyInRect( CRect rect ) const;

protected:

	virtual void	DrawSelectionMarkers( CDC* dc, CRect rect ) const;
	
};

#endif // _DIAGRAMLINE_H_
