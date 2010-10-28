#pragma once

#include "EGDocking.h"

#include "..//Dialogs//SceneTreeDlg.h"
#include "ScriptDlg.h"


class CDockingScriptPane : public  CEGDockingControlPane
{
	friend class CMainFrame;
public:

	DECLARE_MESSAGE_MAP();
protected:
	CScriptDlg   m_wndScriptDlg;
	bool         m_isVisible;

	afx_msg void OnRunScript();
	afx_msg void OnUpdateRunScript(CCmdUI *pCmdUI);

	afx_msg void OnNewScript();
	afx_msg void OnUpdateNewScript(CCmdUI *pCmdUI);

	afx_msg void OnOpenScript();
	afx_msg void OnUpdateOpenScript(CCmdUI *pCmdUI);

	afx_msg void OnSaveScript();
	afx_msg void OnUpdateSaveScript(CCmdUI *pCmdUI);

	afx_msg void OnSaveAsScript();
	afx_msg void OnUpdateSaveAsScript(CCmdUI *pCmdUI);

	virtual HWND CreateControl() 
	{
		m_wndScriptDlg.Create(IDD_SCRIPT_DLG,this);
		m_isVisible = true;
		return m_wndScriptDlg.GetSafeHwnd();
	}
};


