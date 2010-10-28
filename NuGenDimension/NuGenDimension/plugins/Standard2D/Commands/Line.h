#ifndef __LINECOMMAND__
#define __LINECOMMAND__

typedef struct 
{
	IGetPointPanel*  m_get_first_point_panel;
	IGetPointPanel*  m_get_second_point_panel;
} FIRST_SC;

typedef struct 
{
	IGetPointPanel*  m_get_first_point_panel;
	IGetVectorPanel*  m_get_dir_panel;
	IGetNumberPanel*  m_get_length_panel;
} THIRD_SC;

typedef struct 
{
	IGetObjectsPanel*  m_get_obj_panel;
	ISelectPointPanel*  m_sel_point_panel;
	IGetNumberPanel*  m_get_length_panel;
} FOURTH_SC;

typedef struct 
{
	IGetObjectsPanel*  m_get_obj_panel;
	ISelectPointPanel*  m_sel_point_panel;
	IGetPointPanel*  m_get_point_panel;
} FIVETH_SC;

typedef struct 
{
	IGetObjectsPanel*  m_get_obj_panel;
	ISelectPointPanel*  m_sel_point_panel;
	IGetNumberPanel*  m_get_length_panel;
} SIXTH_SC;

typedef struct 
{
	IGetObjectsPanel*  m_get_obj_panel;
	ISelectPointPanel*  m_sel_point_panel;
	IGetNumberPanel*  m_get_length_panel;
} SEVENTH_SC;



class LineCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	unsigned int     m_scenario;
	unsigned int     m_step;
	SG_POINT       m_cur_pnt;
	SG_POINT       m_first_pnt;
	SG_POINT       m_second_pnt;
	SG_VECTOR      m_dir;
	
	CString        m_message;

	CBitmap*        m_bitmaps;

	void LineTwoPointsScenario();
	void LinePointDxdydzScenario();
	void LinePointDirLenScenario();

	sgCLine*   m_other_line;
	void LineLineExtScenario();
	void LineLinePerpScenario();
	sgCArc*    m_other_arc;
	SG_VECTOR  m_plN;
	void LineArcExtScenario();
	void LineArcPerpScenario();

	FIRST_SC    m_1_panels;
	FIRST_SC	m_2_panels;
	THIRD_SC    m_3_panels;
	FOURTH_SC   m_4_panels;
	FIVETH_SC   m_5_panels;
	SIXTH_SC    m_6_panels;
	SEVENTH_SC  m_7_panels;
public:
	LineCommand(IApplicationInterface*  appI);
	virtual ~LineCommand();

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