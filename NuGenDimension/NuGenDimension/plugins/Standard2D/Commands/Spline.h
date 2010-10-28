#ifndef __SPLINECOMMAND__
#define __SPLINECOMMAND__

#include "..//Dialogs//SplinePointsDlg.h"


class SplineCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;

	IGetPointPanel*   m_get_point_panel;
	CSplinePointsDlg* m_existing_points;

	SG_POINT         m_first_point;
	SG_POINT         m_last_point;
	SG_POINT         m_temp_pnt;
	bool             m_ex_temp_pnt;
    
	SG_SPLINE*   m_geo;

	CString m_message;
public:
	SplineCommand(IApplicationInterface*  appI);
	virtual ~SplineCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*) {};

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