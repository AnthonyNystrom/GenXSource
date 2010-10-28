#ifndef __ARCCOMMAND__
#define __ARCCOMMAND__


class ArcCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	unsigned int     m_scenario;
	unsigned int     m_step;
	SG_POINT       m_cur_pnt;
	SG_POINT       m_first_pnt;
	SG_POINT       m_second_pnt;
	SG_POINT       m_projection;
	double         m_dist;
	SG_ARC         m_arc_geo_data;
	SG_VECTOR      m_dir;
	
	IGetPointPanel*  m_get_first_point_panel;
	IGetPointPanel*  m_get_second_point_panel;
	IGetPointPanel*  m_get_third_point_panel;

	bool            m_invert;
	bool            m_exist_arc_data;

	CString        m_message;

	void Arc_b_e_m();
	void Arc_c_b_e();

	sgCLine*   m_other_line;
	void ArcLineExtScenario();
	void ArcLinePerpScenario();
	sgCArc*    m_other_arc;
	SG_VECTOR  m_plN;
	SG_VECTOR  m_v_dir;
	void ArcArcExtScenario();
	void ArcArcPerpScenario();

public:
	ArcCommand(IApplicationInterface* appI);
	virtual ~ArcCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:	
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP			GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();

};


#endif