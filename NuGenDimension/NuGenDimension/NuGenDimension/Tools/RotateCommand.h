#ifndef __ROTATE_COMMAND__
#define __ROTATE_COMMAND__

#include "..//Dialogs//RotatePanelDlg.h"

class RotateCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	SG_POINT			  m_cur_pnt;
	IGetObjectsPanel*     m_get_objects_panel;
	CRotatePanelDlg*      m_panel;

	CString m_message;

	SG_POINT m_selMinP,m_selMaxP;
	void     drawRotateCircles(bool sel=false);

	int      m_step;

	int      m_active_axe;

	SG_VECTOR  m_start_drag_vector;
	SG_POINT   m_rot_plane_pnt;

	SG_VECTOR m_rot_angles;

	unsigned int  GetRotateHandleInRect(CRect&);
public:
	RotateCommand(IApplicationInterface*  appI);
	virtual ~RotateCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:
	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void		    LeftUp(int,int);
	void            OnEnter();
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP  GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);
public:
	void    RedrawFromPanel();
};


#endif