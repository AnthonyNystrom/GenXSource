#pragma once

#include "..//Controls//BirchCtrl.h"
// CSceneTreeDlg dialog

class CGlobalTree : public CBirch, public ISceneTreeControl
{
	HTREENODE     m_work_planes_node;
	HTREENODE     m_layers_node;
	HTREENODE     m_objects_node;

	bool          m_editing_regime;
	HTREENODE     m_editing_object_node;	

	void          SetIconsToNode(HTREENODE nd);

	CBitmap32*    m_vis_obj_bitmap;
	CBitmap32*    m_invis_obj_bitmap;
	CBitmap32*    m_objects_bmp;
	CBitmap32*    m_layers_bmp;
	CBitmap32*    m_wp_bmp;

	CBitmap32*    m_radio_check_bmp;
	CBitmap32*    m_radio_uncheck_bmp;

	CBitmap32*    m_unck_obj_bmp;

	std::vector<HTREENODE>   m_wp_nodes;
	
	CBitmap32*    m_wp_bmps[3];
public:
	CGlobalTree();
	~CGlobalTree();

	virtual  ISceneTreeControl::TREENODEHANDLE  AddNode(sgCObject**, 
		ISceneTreeControl::TREENODEHANDLE);
	virtual  bool            ShowNode(sgCObject*);
	virtual  bool            HideNode(TREENODEHANDLE);
	virtual  bool            UpdateNode(ISceneTreeControl::TREENODEHANDLE);
	virtual  bool            RemoveNode(ISceneTreeControl::TREENODEHANDLE);
	virtual  bool            ClearTree();

	void     SetCurrentWorkPlane(size_t nmbr);
	void     ChangeWorkPlaneVisible(size_t nmbr);
	void     ChangeObjectVisible(sgCObject* ob,bool vis);
};

class  CMyBirchCtrl : public CBirchCtrl
{
public:
	virtual   void  StartEditLabel(HTREENODE pNode, CString& newLabel);
	virtual   void  FinishEditLabel(HTREENODE pNode, CString& newLabel);
	virtual   void  ClickOnIcon(HTREENODE pNode, unsigned int iconNumb);
};

class CSceneTreeDlg : public CDialog
{
	DECLARE_DYNAMIC(CSceneTreeDlg)

public:
	CSceneTreeDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSceneTreeDlg();

// Dialog Data
	enum { IDD = IDD_SCENE_TREE_CTRL };
private:
	CMyBirchCtrl   m_tree;
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnOK();
	virtual void OnCancel();
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
};
