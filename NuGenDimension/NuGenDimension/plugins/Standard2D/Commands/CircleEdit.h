#ifndef __CIRCLE_EDIT__
#define __CIRCLE_EDIT__

class CircleEditCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCCircle*       m_editable_circle;
	unsigned int     m_scenario;

	SG_POINT       m_cur_pnt;
	SG_VECTOR      m_dir;
	
	IGetPointPanel*  m_get_center_panel;
	IGetVectorPanel* m_get_normal_panel;
	IGetNumberPanel*  m_get_r_panel;

	SG_CIRCLE      m_tmp_circ;

	bool           m_was_started;

	CBitmap*        m_bitmaps;

	void           NewScenario();
public:
	CircleEditCommand(sgCCircle* eC, IApplicationInterface*  appI);
	virtual ~CircleEditCommand();

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