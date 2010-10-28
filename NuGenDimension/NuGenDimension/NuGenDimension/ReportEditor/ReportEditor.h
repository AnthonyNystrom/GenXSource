#ifndef _REPORTEDITOR_H_
#define _REPORTEDITOR_H_

#include "DiagramEditor/DiagramEditor.h"
class CReportEditor : public CDiagramEditor
{
public:
	CReportEditor();
	~CReportEditor();

	virtual void SetZoom( double zoom );

protected:

	virtual afx_msg void OnMouseMove(UINT nFlags, CPoint point);

	virtual void SetHScroll( int pos );
	virtual void SetVScroll( int pos );

};

extern UINT UWM_HSCROLL;
extern UINT UWM_VSCROLL;
extern UINT UWM_ZOOM;
extern UINT UWM_MOUSE;

#endif //_REPORTEDITOR_H_