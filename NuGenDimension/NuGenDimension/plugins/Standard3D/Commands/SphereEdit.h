#ifndef __SPHERE_EDIT__
#define __SPHERE_EDIT__

#include "..//Dialogs//SphereParamsDlg.h"

class CSphereEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCSphere*        m_editable_sphere;
	SG_POINT         m_cur_pnt;
	SG_POINT         m_center;
	double           m_rad;
	IGetNumberPanel*   m_r_panel;
	bool             m_was_started;

	CSphereParamsDlg* m_other_params_dlg;

	sgCMatrix*      m_matr;
	SG_SPHERE       m_sph_geo;
public:
	CSphereEditCommand(sgCSphere* edP, IApplicationInterface*  appI);
	virtual ~CSphereEditCommand();

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