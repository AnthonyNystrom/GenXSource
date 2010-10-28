#ifndef __CONE_EDIT__
#define __CONE_EDIT__

#include "..//Dialogs//MeridiansDlg.h"

class CConeEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCCone*        m_editable_cone;
	SG_POINT         m_cur_pnt;

	int              m_scenar;

	sgCMatrix*       m_matr;

	CMeridiansDlg* m_other_params_dlg;

	SG_POINT         m_base_pnt;
	SG_VECTOR        m_dir;

	IGetNumberPanel*   m_r_panel;

	double          m_rad;
	double          m_height;

	
	bool             m_was_started;

	SG_CONE     m_cone_geo;
	CString         m_message;
public:
	CConeEditCommand(sgCCone* edC, IApplicationInterface*  appI);
	virtual ~CConeEditCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*) {};

private:	
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP    GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();
};

#endif