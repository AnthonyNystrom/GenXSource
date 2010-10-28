#if !defined(AFX_OPENGLVIEW_H__B54A10FA_6499_4B51_B814_394E26D2D38C__INCLUDED_)
#define AFX_OPENGLVIEW_H__B54A10FA_6499_4B51_B814_394E26D2D38C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// OpenGLView.h : header file
//
#include "3DCamera.h"
#include "background_xD.h"
#include "Planes.h"

#include "..//CommonStructures.h"

#ifdef _DEBUG
	#include "..//Tools//FPSCounter.h"
#endif

typedef enum
{
	HA_NONE,
	HA_MOVE,
	HA_ROTATE,
	HA_ZOOM,
	HA_OBJ_TRANSLATE,
	HA_OBJ_ROTATE
} HAND_ACTION;

/////////////////////////////////////////////////////////////////////////////
// COpenGLView view

class COpenGLView : public CView,public IViewPort, public IPainter
{
protected:
	COpenGLView();           // protected constructor used by dynamic creation
	DECLARE_DYNCREATE(COpenGLView)


	#ifdef _DEBUG
		CFPSCounter m_FPSCounter; // заводим счетчик
	#endif
protected:
	// OpenGL specific
	BOOL  SetWindowPixelFormat(HDC hDC);
	HGLRC m_hGLContext;
	HDC   m_hDC;
	bool  m_need_release_DC;

	CBackground*  m_background;

	void SetClearColor(void) {	glClearColor(m_ClearColorRed,m_ClearColorGreen,m_ClearColorBlue,1.0f); }

	void ZoomRect(const CRect &rcClient,const CRect& rcZoom);

	int ReadPixels(unsigned char** pixels, int* w, int* h, int *rowlen);
	int ReadAndWriteToBmp(CString path);

	void WritePfdToTxt(CString file, PIXELFORMATDESCRIPTOR *pfd, int pixelformat);
	bool IsExtEnable(const char * pName);
	void WriteExtInfoToFile(CString file);
protected:
	HAND_ACTION     m_hand_action;
public:
	HAND_ACTION     GetHandAction() const {return m_hand_action;};

	void            SetCamera(const C3dCamera& newcam) {m_Camera=newcam;};
protected:
	CPoint			m_ScreenLeftButtonDownPoint;	// Screen position of the mouse
	
	C3dCamera		m_Camera;		// Our windows camera

	// Colors
	float m_ClearColorRed;
	float m_ClearColorGreen;
	float m_ClearColorBlue;

	float m_rezPointColorRed;
	float m_rezPointColorGreen;
	float m_rezPointColorBlue;

	float m_rezLineColorRed;
	float m_rezLineColorGreen;
	float m_rezLineColorBlue;

	float m_rezPointSize;
	float m_rezLineWidth;

	CPlanes*  m_planes;
public:
	CPlanes*  GetWorkPlanes()  {ASSERT(m_planes); return m_planes;};

	virtual   void DrawScene(GLenum mode = GL_RENDER, bool selSubObj=false) {};
	virtual   void DrawFromCommander() {};
public:
	void  RotateCamera(CSize sdvig);
	void  TranslateCamera(SG_VECTOR sdvig);
	void  ZoomCamera(CSize sdvig);
public:
	C3dCamera* GetCamera() {return &m_Camera;};

private:
	void       GetPointAfterFixing(int, int, bool xFix,
		bool yFix,
		bool zFix,
		const SG_POINT& fixP,SG_POINT&);
	/************************************************************************/
	/*   IViewPort Interface                                                */
	/************************************************************************/
public:
	CWnd*           GetWindow(){return this;};
	void			InvalidateViewPort();

	void			UnProjectScreenPoint(int,int,double,SG_POINT&);
	void			ProjectWorldPoint(const SG_POINT&, double&, double&, double&);

	VIEW_PORT_VIEW_TYPE   GetViewPortViewType();
	void			GetViewPortNormal(SG_VECTOR&);
	int             GetSnapSize();
	SELECT_BUFFER   GetHitsInRect(const CRect&, bool selSubObj=false);
	sgCObject*	    GetTopObject(SELECT_BUFFER);
	sgCObject*	    GetTopObjectByType(SELECT_BUFFER, SG_OBJECT_TYPE);
	bool            IsOnAnyObject(SELECT_BUFFER);

	float             GetGridSize()
	{
		ASSERT(m_planes);
		return m_planes->GetActiveWorkPlaneGridSize();
	}


	void            GetWorldPointAfterSnap(const GET_SNAP_IN& in_arg,
															GET_SNAP_OUT& out_arg);
    bool    ProjectScreenPointOnPlane(int, int, SG_VECTOR&, double, SG_POINT&);
	bool    ProjectScreenPointOnLine(int,int,const SG_POINT&, const SG_VECTOR&, SG_POINT&);

public:
	IPainter*       GetPainter() {return this;};

	void    SetCurColor(float, float,float);
	void    SetLineWidth(float);
	void    SetPointWidth(float);
	void    GetUserColorLines(float&,float&,float&)	;
	void    GetUserColorPoints(float&,float&,float&);
private:
	sgCMatrix*   m_transforming_matrix;
public:

	void    SetTransformMatrix(const sgCMatrix*);

	void	DrawPoint(const SG_POINT&);
	void	DrawLine(const SG_LINE&);
	void	DrawCircle(const SG_CIRCLE&);
	void	DrawArc(const SG_ARC&);
	void	DrawSpline(SG_SPLINE*);
	void	DrawSplineFrame(SG_SPLINE*);

	void    DrawSphere(double rad);
	void    DrawBox(double sz1, double sz2,double sz3);
	void    DrawCone(double rd1, double rd2, double h);
	void    DrawCylinder(double rd, double h);
	void    DrawEllipsoid(double rd1, double rd2, double rd3);
	void    DrawTorus(double rd1, double rd2);
	void    DrawSphericBand(double rd, double begC, double endC);

	void    DrawObject(const sgCObject*);

	void            SetEditableObject(sgCObject*);
	void            SetHotObject(sgCObject*);
	sgCObject*      GetHotObject();

public:
// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(COpenGLView)
	protected:
	virtual void OnActivateView(BOOL bActivate, CView* pActivateView, CView* pDeactiveView);
	virtual void OnDraw(CDC* pDC);
	//}}AFX_VIRTUAL
// Implementation
protected:
	virtual ~COpenGLView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	// Generated message map functions
protected:
	//{{AFX_MSG(COpenGLView)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnPaint();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	afx_msg LRESULT OnEnterSizeMove (WPARAM, LPARAM); 
	afx_msg LRESULT OnExitSizeMove (WPARAM, LPARAM);
	afx_msg void OnParallProj();
	afx_msg void OnUpdateParallProj(CCmdUI *pCmdUI);
	afx_msg void OnPerspectProj();
	afx_msg void OnUpdatePerspectProj(CCmdUI *pCmdUI);
	afx_msg void OnFrontView();
	afx_msg void OnUpdateFrontView(CCmdUI *pCmdUI);
	afx_msg void OnBackView();
	afx_msg void OnUpdateBackView(CCmdUI *pCmdUI);
	afx_msg void OnTopView();
	afx_msg void OnUpdateTopView(CCmdUI *pCmdUI);
	afx_msg void OnBottomView();
	afx_msg void OnUpdateBottomView(CCmdUI *pCmdUI);
	afx_msg void OnLeftView();
	afx_msg void OnUpdateLeftView(CCmdUI *pCmdUI);
	afx_msg void OnRightView();
	afx_msg void OnUpdateRightView(CCmdUI *pCmdUI);
	afx_msg void OnIsoFrontView();
	afx_msg void OnUpdateIsoFrontView(CCmdUI *pCmdUI);
	afx_msg void OnIsoBackView();
	afx_msg void OnUpdateIsoBackView(CCmdUI *pCmdUI);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnMoveEyeHand();
	afx_msg void OnUpdateMoveEyeHand(CCmdUI *pCmdUI);
	afx_msg void OnRotateEyeHand();
	afx_msg void OnUpdateRotateEyeHand(CCmdUI *pCmdUI);
	afx_msg void OnZoomEyeHand();
	afx_msg void OnUpdateZoomEyeHand(CCmdUI *pCmdUI);
	afx_msg void OnMoveLeftAuto();
	afx_msg void OnMoveRightAuto();
	afx_msg void OnMoveUpAuto();
	afx_msg void OnMoveDownAuto();
	afx_msg void OnRotateXAuto();
	afx_msg void OnRotateYAuto();
	afx_msg void OnRotateZAuto();
	afx_msg void OnZoomPlusAuto();
	afx_msg void OnZoomMinusAuto();
	afx_msg void OnCopyAsRastr();
	afx_msg void OnCopyAsVector();
	afx_msg void OnSaveAsRastr();
	afx_msg void OnSaveAsVector();
	afx_msg void OnTranslate();
	afx_msg void OnUpdateTranslate(CCmdUI *pCmdUI);
	afx_msg void OnRotate();
	afx_msg void OnUpdateRotate(CCmdUI *pCmdUI);
	afx_msg void OnAllSceneView();
	afx_msg void OnUpdateAllSceneView(CCmdUI *pCmdUI);
	afx_msg void OnCreateGroup();
	afx_msg void OnUpdateCreateGroup(CCmdUI *pCmdUI);
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_OPENGLVIEW_H__B54A10FA_6499_4B51_B814_394E26D2D38C__INCLUDED_)
