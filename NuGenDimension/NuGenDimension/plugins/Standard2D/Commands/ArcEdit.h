#ifndef __ARC_EDIT__
#define __ARC_EDIT__

class CArcEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCArc*        m_editable_arc;
	unsigned int     m_scenario;

	SG_POINT         m_cur_pnt;
	SG_ARC           m_tmp_arc;
	
	IGetPointPanel*       m_get_point_panel;

	bool             m_was_started;
	bool             m_invert;
	bool             m_exist_arc_data;
	void           NewScenario();
public:
	CArcEditCommand(sgCArc* edA, IApplicationInterface*  appI);
	virtual ~CArcEditCommand();

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