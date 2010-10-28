#pragma once

#include "..//Controls//RollupCtrl.h"
// CCommandPanelDlg dialog

class CCommandPanelDlg : public CDialog, public ICommandPanel
{
	DECLARE_DYNAMIC(CCommandPanelDlg)

public:
	CCommandPanelDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CCommandPanelDlg();

// Dialog Data
	enum { IDD = IDD_COMMAND_PANEL_DLG };

private:
	CRollupCtrl										    m_wndRollupCtrl;
	void       RecalcPlaces();
public:
	
	virtual  CWnd*                   GetDialogsContainerWindow();

	virtual  bool	AddDialog(IBaseInterfaceOfGetDialogs*,const char*, bool);
	virtual  IBaseInterfaceOfGetDialogs*   AddDialog(IBaseInterfaceOfGetDialogs::DLG_TYPE, 
		const char*, bool);

	virtual  bool   RemoveDialog(IBaseInterfaceOfGetDialogs*);
	virtual  bool   RemoveDialog(unsigned int);

	virtual  bool   RenameRadio(unsigned int, const char*);


	virtual  void   EnableRadio(unsigned int,bool);
	virtual  void   SetActiveRadio(unsigned int);

	virtual  bool   RemoveAllDialogs();

	virtual  void   DrawGroupFrame(CDC* pDC, const CRect& rct, 
		const int leftLab, const int rightLab);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL DestroyWindow();
protected:
	virtual void OnOK();
	virtual void OnCancel();
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
