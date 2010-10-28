// ChildFrm.h : interface of the CChildFrame class
//
#pragma once

#include "MainFrm.h"

class CChildFrame : public CMDIChildWnd, public IApplicationInterface
{
	friend class CNuGenDimensionView;

	DECLARE_DYNCREATE(CChildFrame)
	
public:
	CChildFrame();

// Attributes
public:
	void     SetView(CNuGenDimensionView* v);

	virtual void PutMessage(MESSAGE_TYPE mes_type,
		const char* mes_str);
	
// Operations
public:
	
// Overrides
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);

// Implementation
public:
	virtual ~CChildFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif


	
private:
	CNuGenDimensionView* m_view;
	SNAP_TYPE	  m_snap_type;

	//CEGButtonsBar	  m_wndPanelsContainer;
	//CCommandPanelDlg  m_command_panel;
	//CSceneTreeDlg     m_scene_tree_dlg;
	
public:
	ICommander*  m_commander;
private:
	UINT         m_active_plugin;
	INT_PTR      m_command_panel_page_index;
	void         FreeCommander();
	
public:
	virtual IViewPort*           GetViewPort();
	virtual ICommandPanel*		 GetCommandPanel();
	virtual void				  StopCommander(){FreeCommander();};
	virtual void            StartCommander(const char* str);
	virtual double          ApplyPrecision(double);
	virtual void            CopyAttributes(sgCObject& where_obj, 
		const sgCObject& from_obj);
	virtual void            ApplyAttributes(sgCObject*);

	const int   GetSnapSize() const;
	SNAP_TYPE   GetSnapType() const {return m_snap_type;};
// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);
	afx_msg int  OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnWorkPlanesSetups();
	afx_msg void OnXWpShow();
	afx_msg void OnUpdateXWpShow(CCmdUI *pCmdUI);
	afx_msg void OnYWpShow();
	afx_msg void OnUpdateYWpShow(CCmdUI *pCmdUI);
	afx_msg void OnZWpShow();
	afx_msg void OnUpdateZWpShow(CCmdUI *pCmdUI);
	virtual BOOL DestroyWindow();
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnEditUndo();
	afx_msg void OnUpdateEditUndo(CCmdUI *pCmdUI);
	afx_msg void OnEditRedo();
	afx_msg void OnUpdateEditRedo(CCmdUI *pCmdUI);
	afx_msg void OnSnapNo();
	afx_msg void OnUpdateSnapNo(CCmdUI *pCmdUI);
	afx_msg void OnSnapPoints();
	afx_msg void OnUpdateSnapPoints(CCmdUI *pCmdUI);
	afx_msg void OnSnapEnds();
	afx_msg void OnUpdateSnapEnds(CCmdUI *pCmdUI);
	afx_msg void OnSnapMids();
	afx_msg void OnUpdateSnapMids(CCmdUI *pCmdUI);

	afx_msg void OnDeleteHotObject();
	afx_msg void OnFitHotObject();
	afx_msg void OnThisProjectionOn2D();

	afx_msg void OnSetMaterialToObj();
	afx_msg void OnUpdateSetMaterialToObj(CCmdUI *pCmdUI);
	afx_msg void OnUnSetMaterialToObj();
	afx_msg void OnUpdateUnSetMaterialToObj(CCmdUI *pCmdUI);

	afx_msg void OnEndCommander();

	void    CommanderContextMenu(int x,int y);
	void    EditCommanderContextMenu(int x,int y);
	afx_msg void OnObjectProperties();
	afx_msg void OnSnapCenters();
	afx_msg void OnUpdateSnapCenters(CCmdUI *pCmdUI);
	afx_msg void OnLayers();
	afx_msg void OnAttachMatLib();
	afx_msg void OnMatEditor();
	afx_msg void OnDetachMatLib();
	afx_msg void OnUpdateDetachMatLib(CCmdUI *pCmdUI);

	bool    CreateTransformCommander();
	afx_msg void OnFonts();
	afx_msg void OnFontsPreview();
	afx_msg void OnUpdateFontsPreview(CCmdUI *pCmdUI);
	afx_msg void OnBreakHotGroup();
	afx_msg void OnBreakHotContour();
	afx_msg void OnMDIActivate(BOOL bActivate, CWnd* pActivateWnd, CWnd* pDeactivateWnd);

	bool    CreateGroupCommander();
	afx_msg void OnRayTraceStart();
};
