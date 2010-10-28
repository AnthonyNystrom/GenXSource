#ifndef __TRANSLATE_COMMAND__
#define __TRANSLATE_COMMAND__

#include "..//Dialogs//TranslatePanelDlg.h"

class TranslateCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT			  m_cur_pnt;
	IGetObjectsPanel*     m_get_objects_panel;
	CTranslatePanelDlg*   m_panel;

	CString m_message;

	SG_POINT m_selMinP,m_selMaxP;
	void     drawTransArrows(bool sel=false);

	int      m_step;

	int      m_active_axe;

	SG_POINT  m_start_drag_point;

	SG_VECTOR m_trans_vector;

	unsigned int  GetTransHandleInRect(CRect&);
public:
	TranslateCommand(IApplicationInterface* appI);
	virtual ~TranslateCommand();

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
	void		    LeftUp(int,int);
	void            OnEnter();
};


#endif