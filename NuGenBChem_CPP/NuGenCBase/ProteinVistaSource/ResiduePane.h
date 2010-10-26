#pragma once

#include "afxwin.h"
#include "GridCtrl\GridCtrl.h"
#include <vector>

class CProteinVistaRenderer;
class CResiduePane : public CWnd
{
private:
	CProteinVistaRenderer * m_pProteinVistaRenderer;

	CString rowHeaderName;
public:
	CResiduePane();
	CGridCtrl*			m_residueGridCtrl;
	virtual CString GetName() { return "Residue Pane"; }
	void Init(CProteinVistaRenderer * pProteinVistaRenderer)
	{
		this->m_pProteinVistaRenderer=pProteinVistaRenderer;
		rowHeaderName ="";
	}
public:
	virtual HRESULT OnUpdate();
	virtual void	Delete();
	void UpdateResidueFromPDBSelection();

	enum { ONE_RESIDUE, ONE_RESIDUE_INDEX, ONE_RESIDUE_INDEX_NUMBER, THREE_RESIDUE, THREE_RESIDUE_INDEX, THREE_RESIDUE_INDEX_NUMBER };
	long	m_displayStyle;
	void	SetGridCtrlText();
public:
	virtual ~CResiduePane();
	BOOL	m_bSelectionChanged;
	void	UpdatePDBFromGridCtrl();
	void    ChangeSize(UINT nType, int cx, int cy);
	void    DisplaySelected(){OnDisplaySelected();}
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	DECLARE_MESSAGE_MAP()
	afx_msg void OnDisplaySelected();
	afx_msg void OnSelectionChanged(NMHDR* pNMHDR, LRESULT* pResult);
public:
	afx_msg void OnDestroy();
	afx_msg void OnTimer(UINT_PTR nIDEvent);
};

