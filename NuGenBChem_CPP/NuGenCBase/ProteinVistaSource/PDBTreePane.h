//////////////////////////////////////////////////////////////////////////////////////////////////
// PDBTreePane.h																		//
//////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once

#include "PDB.h"
#include "PDBInst.h"
#include "Interface.h"
//#include "MltiTree.h"
class CProteinVistaRenderer;

//////////////////////////////////////////////////////////////////////////////////////////////////
// CPDBTreePane
//
class CPDBTreePane: public CWnd
{
private:
	CProteinVistaRenderer * m_pProteinVistaRenderer;
public:
	CPDBTreePane();
	~CPDBTreePane();
	enum {	BITMAP_INDEX_PDB, BITMAP_INDEX_PDB_HIDE,	
			BITMAP_INDEX_MODEL, BITMAP_INDEX_MODEL_HIDE,
			BITMAP_INDEX_CHAIN, BITMAP_INDEX_CHAIN_HIDE,	
			BITMAP_INDEX_RESIDUE, BITMAP_INDEX_RESIDUE_HIDE,	
			BITMAP_INDEX_ATOM, BITMAP_INDEX_ATOM_HIDE,
			BITMAP_INDEX_HETATM, BITMAP_INDEX_HETATM_HIDE,
	};

	CTreeCtrl*		m_residueTreeCtrl;
	CImageList		m_residueImageList;
	CTreeCtrl	* GetTreeCtrl() { return m_residueTreeCtrl; }

	void Init(CProteinVistaRenderer * pProteinVistaRenderer);

	virtual void	Delete();
	virtual CString GetName(){ return "PDB Tree"; }

	void     ChangeSize(UINT nType, int cx, int cy);
	void	InitialUpdateTreeCtrl();
	void	ResetTreeItem();
	virtual HRESULT	OnUpdate();
	void	UpdatePDBSelectionFromTreeCtrl();
	void	UpdateTreeCtrlFromPDBSelection();		 
	BOOL	CheckTreeItem(CProteinObjectInst * pObjectInst);
	void	InsertChild(HTREEITEM hItem);
	void	ClickTreeCtrl(HTREEITEM hTreeItemClick);
	void	SaveSelection(CSTLArraySelectionInst &selection);

	void	SetParentUnselect (CProteinObjectInst * pObjectClick);
	void	ItemExpandToChain (HTREEITEM hItem);		 
	void	ShowSelectionOptionMenu(POINT pt);

private:
	BOOL	m_bSelectionChanged;		 
	void	UpdateTreeCtrl(CProteinObjectInst * pObjectExpanding);

protected:
	//{{AFX_MSG(CPDBTreePane)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnRclickTreeCtrl(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDblclickTreeCtrl(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnClickTreeCtrl(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnItemExpanded(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnItemExpanding(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnSelectionMarking();
	afx_msg void OnSyncPDB();
	afx_msg void OnFullSyncPDB();
	afx_msg void OnSaveSelection();
	afx_msg void OnSelectionOption();
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnDestroy();
};

//////////////////////////////////////////////////////////////////////////////////////////////////



