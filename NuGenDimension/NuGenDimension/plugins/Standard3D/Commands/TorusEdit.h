#ifndef __TORUS_EDIT__
#define __TORUS_EDIT__

#include "..//Dialogs//TorusEditDlg.h"

class CTorusEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCTorus*        m_editable_torus;
	SG_POINT         m_cur_pnt;
	
	bool             m_was_started;

	int              m_scenar;

	sgCMatrix*       m_matr;

	CTorusParamsDlg* m_other_params_dlg;

	SG_POINT         m_base_pnt;
	SG_VECTOR        m_dir;
	double           m_plD;

	IGetNumberPanel*   m_r_panel;

	double          m_rad_1;
	double          m_rad_2;


	SG_TORUS         m_tor_geo;
	CString         m_message;
public:
	CTorusEditCommand(sgCTorus* edT, IApplicationInterface*  appI);
	virtual ~CTorusEditCommand();

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