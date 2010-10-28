#pragma once

// ArMaXCtrl.h : Declaration of the CArMaXCtrl ActiveX Control class.


// CArMaXCtrl : See ArMaXCtrl.cpp for implementation.

class CArMaXCtrl : public COleControl
{
	DECLARE_DYNCREATE(CArMaXCtrl)

// Constructor
public:
	CArMaXCtrl();

private:
	HDC m_hDC;        // Handle to DC
	HGLRC m_hRC;      // Handle to RC

	BOOL SetWindowPixelFormat(HDC hDC);

	  virtual void DrawScene();
  virtual void KillScene();
  virtual void InitScene();

// Overrides
public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();

// Implementation
protected:
	~CArMaXCtrl();

	DECLARE_OLECREATE_EX(CArMaXCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CArMaXCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CArMaXCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CArMaXCtrl)		// Type name and misc status

// Message maps
	DECLARE_MESSAGE_MAP()

// Dispatch maps
	DECLARE_DISPATCH_MAP()

// Event maps
	DECLARE_EVENT_MAP()

// Dispatch and event IDs
public:
	enum {
		dispidCopyToClipboard = 7L,		dispidSetShininess = 6L,		dispidSetTransparent = 5L,		dispidSetSpecular = 4L,		dispidSetEmission = 3L,		dispidSetDiffuse = 2L,		dispidSetAmbient = 1L
	};
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnSize(UINT nType, int cx, int cy);
protected:
	void SetAmbient(FLOAT r, FLOAT g, FLOAT b);
	void SetDiffuse(FLOAT r, FLOAT g, FLOAT b);
	void SetEmission(FLOAT r, FLOAT g, FLOAT b);
	void SetSpecular(FLOAT r, FLOAT g, FLOAT b);
	void SetTransparent(FLOAT transparent);
	void SetShininess(FLOAT shininess);
	void CopyToClipboard(void);
public:
};

