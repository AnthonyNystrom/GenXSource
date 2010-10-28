#ifndef __SPHERIC_BAND_EDIT__
#define __SPHERIC_BAND_EDIT__

#include "..//Dialogs//MeridiansDlg.h"

class CSphericBandEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCSphericBand*        m_editable_sb;
	SG_POINT         m_cur_pnt;
	
	bool             m_was_started;
	int              m_scenar;
	SG_POINT         m_base_pnt;
	SG_VECTOR        m_dir;

	double           m_rad;
	double           m_cur_coef;
	IGetNumberPanel*   m_number_panel;
	
	sgCMatrix*      m_matr;
	CMeridiansDlg* m_other_params_dlg;
	
	SG_SPHERIC_BAND     m_sp_b_geo;

	CString m_message;
public:
	CSphericBandEditCommand(sgCSphericBand* edSb, IApplicationInterface*  appI);
	virtual ~CSphericBandEditCommand();

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