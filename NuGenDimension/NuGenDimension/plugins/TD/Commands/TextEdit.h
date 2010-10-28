#ifndef __TextEditCommand__
#define __TextEditCommand__

#include "..//TextParamsDlg.h"
#include "..//TextTextDlg.h"

class TextEditCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	sgCText*        m_editable_text;
	CString m_message;

	SG_TEXT_STYLE    m_text_style;

	CTextParamsDlg*        m_text_panel;
	IComboPanel*           m_orient_panel;
	CTextTextDlg*          m_text_text_panel;

	CString                m_text;
	sgCMatrix              m_matrix;
	unsigned int           m_fnt;

	short                 m_scenario;
public:
	TextEditCommand(sgCText* edT, IApplicationInterface* appI);
	virtual ~TextEditCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*  GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:	
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP         GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();
};

#endif