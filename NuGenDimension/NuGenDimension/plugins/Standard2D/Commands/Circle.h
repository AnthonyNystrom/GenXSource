#ifndef __CIRCLECOMMAND__
#define __CIRCLECOMMAND__

class CircleCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	unsigned int     m_scenario;
	unsigned int     m_step;
	SG_POINT       m_cur_pnt;
	SG_POINT       m_first_pnt;
	SG_POINT       m_second_pnt;
	SG_CIRCLE      m_circle_geo_data;
	SG_VECTOR      m_dir;
	
	IGetPointPanel*  m_get_center_panel;
	IGetVectorPanel* m_get_normal_panel;
	IGetNumberPanel*  m_get_r_panel;

	bool            m_exist_circ_data;

	CString        m_message;

	CBitmap*        m_bitmaps;

	void CircleRadCenNormScenario();
	void CircleThreePointsScenario();


public:
	CircleCommand(IApplicationInterface*  appI);
	virtual ~CircleCommand();

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
	void            OnEnter();
};

#endif