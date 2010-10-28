#ifndef __TextCommand__
#define __TextCommand__

#include "..//TextParamsDlg.h"
#include "..//TextTextDlg.h"

class TextCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetPointPanel*        m_get_point_panel;
	CTextTextDlg*          m_text_text_panel;
	IComboPanel*           m_orient_combo_panel;
	CTextParamsDlg*        m_text_panel;

	CString m_message;

	SG_POINT               m_cur_pnt;

	sgCMatrix        m_text_rot_matr;

	SG_TEXT_STYLE    m_text_style;

	CTextParamsDlg*        m_params_dlg;

	CString                m_text;
public:
	TextCommand(IApplicationInterface* appI);
	virtual ~TextCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:	
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP  GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();
};

#endif