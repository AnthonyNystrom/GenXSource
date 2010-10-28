#ifndef __ELLIPSOID_EDIT__
#define __ELLIPSOID_EDIT__

#include "..//Dialogs//SphereParamsDlg.h"

class CEllipsoidEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCEllipsoid*        m_editable_ellipsoid;
	SG_POINT         m_cur_pnt;
	
	IGetNumberPanel*  m_size_panel;
	bool             m_was_started;

	int             m_scenar;

	CString         m_message;

	CSphereParamsDlg* m_other_params_dlg;

	sgCMatrix*      m_matr;

	SG_BOX       m_box_geo;

	SG_POINT      m_base_pnt;

	SG_VECTOR     m_dir;

	void          ReCalcDir();

	SG_POINT      m_projection;

	SG_ELLIPSOID     m_ell_geo;
public:
	CEllipsoidEditCommand(sgCEllipsoid* edE, IApplicationInterface*  appI);
	virtual ~CEllipsoidEditCommand();

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