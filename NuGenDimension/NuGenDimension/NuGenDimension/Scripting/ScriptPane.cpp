// ScriptDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "ScriptPane.h"


BEGIN_MESSAGE_MAP(CDockingScriptPane, CEGDockingControlPane)
	ON_COMMAND(ID_RUN_SCRIPT, OnRunScript)
	ON_UPDATE_COMMAND_UI(ID_RUN_SCRIPT, OnUpdateRunScript)
	ON_COMMAND(ID_SCRIPT_NEW, OnNewScript)
	ON_UPDATE_COMMAND_UI(ID_SCRIPT_NEW, OnUpdateNewScript)
	ON_COMMAND(ID_SCRIPT_OPEN, OnOpenScript)
	ON_UPDATE_COMMAND_UI(ID_SCRIPT_OPEN, OnUpdateOpenScript)
	ON_COMMAND(ID_SAVE_SCRIPT, OnSaveScript)
	ON_UPDATE_COMMAND_UI(ID_SAVE_SCRIPT, OnUpdateSaveScript)
	ON_COMMAND(ID_SAVE_AS_SCRIPT, OnSaveAsScript)
	ON_UPDATE_COMMAND_UI(ID_SAVE_AS_SCRIPT, OnUpdateSaveAsScript)
END_MESSAGE_MAP()


void CDockingScriptPane::OnRunScript()
{
	m_wndScriptDlg.OnRun();
}

void CDockingScriptPane::OnUpdateRunScript(CCmdUI *pCmdUI)
{
	// TODO: Add your command update UI handler code here
}


void CDockingScriptPane::OnNewScript()
{
	m_wndScriptDlg.OnNew();
}

void CDockingScriptPane::OnUpdateNewScript(CCmdUI *pCmdUI)
{
	// TODO: Add your command update UI handler code here
}

void CDockingScriptPane::OnOpenScript()
{
	m_wndScriptDlg.OnOpen();
}

void CDockingScriptPane::OnUpdateOpenScript(CCmdUI *pCmdUI)
{
	// TODO: Add your command update UI handler code here
}

void CDockingScriptPane::OnSaveScript()
{
	m_wndScriptDlg.OnSave();	

}

void CDockingScriptPane::OnUpdateSaveScript(CCmdUI *pCmdUI)
{
	// TODO: Add your command update UI handler code here
}

void CDockingScriptPane::OnSaveAsScript()
{
	m_wndScriptDlg.OnSaveAs();
}

void CDockingScriptPane::OnUpdateSaveAsScript(CCmdUI *pCmdUI)
{
	// TODO: Add your command update UI handler code here
}


