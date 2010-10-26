#pragma once
 
#include "HTMLListCtrl.h"
class CRenderProperty;
class CSelectionDisplay;

class CSelectionListPane : public CWnd
{
private:
	CProteinVistaRenderer * m_pProteinVistaRenderer;
public:
	CSelectionListPane();
    DECLARE_DYNCREATE(CSelectionListPane)
	void Init(CProteinVistaRenderer * pProteinVistaRenderer)
	{
		this->m_pProteinVistaRenderer=pProteinVistaRenderer;
	}
public:
	CImageList			m_ImageList;
	CHTMLListCtrl *     m_htmlListCtrl;

	virtual CString GetName(){ return "Selection List"; }
	virtual HRESULT OnUpdate();
	virtual void Delete();
	
	void    ChangeSize(UINT nType, int cx, int cy);
	void	MakeSelectionPaneText(long index, CSelectionDisplay * pSelectionDisplay, CString &strHTML);
	void	SelectListItem(long n);
	void	SelectListItem(CSelectionDisplay * pSelectionDisplay);
	void	DeselectPaneItem();
	void	Deselect() { OnDeselect(); }
	int		GetListCtrlIndex(CSelectionDisplay * pSelectionDisplay);

	enum { UNION=1, INTERSECT=2, SUBTRACT=3 };
	long m_selectOperation;
	CSelectionDisplay * m_pBooleanSelectionDisplay;
	void		SelectionOperation(CSelectionDisplay * sel1, CSelectionDisplay * sel2, int op);
public:
	void		SetRenderProperty(CSelectionDisplay * pSelectionDisplay);
	void		DeleteSelectionDisplay(CSelectionDisplay * pSelectionDisplay);
	virtual ~CSelectionListPane()
	{
		SAFE_DELETE(m_htmlListCtrl);
	}
	void UnionPDB(){OnUnion();};
	void IntersectPDB(){OnIntersect();};
	void SubtractPDB(){OnSubtract();};
	void OperateResult(){OnResult();};

	void UpdateSelectFromResidue(CString pdbName);
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnSetFocus(CWnd* pOldWnd);
 
	afx_msg void OnShowAll();
	afx_msg void OnHideAll();
	afx_msg void OnDelete();
	afx_msg void OnDeSelect();
	 
	afx_msg void OnListSelected( NMHDR * pNotifyStruct, LRESULT * result );
	afx_msg void OnListDeSelected( NMHDR * pNotifyStruct, LRESULT * result );
	afx_msg void OnListItemChecked(NMHDR * pNotifyStruct, LRESULT * result );
	afx_msg void OnListItemDBLClick( NMHDR * pNotifyStruct, LRESULT * result );
	afx_msg void OnDeselect();
	afx_msg void OnUnion();
	afx_msg void OnIntersect();
	afx_msg void OnSubtract();
	afx_msg void OnResult();

	afx_msg void OnCurrentUp(); 
	afx_msg void OnCurrentDown();
	afx_msg void OnCenterCurrentVP();
	 
	DECLARE_MESSAGE_MAP()
};

