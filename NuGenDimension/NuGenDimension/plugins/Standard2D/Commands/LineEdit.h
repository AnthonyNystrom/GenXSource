#ifndef __LINE_EDIT__
#define __LINE_EDIT__


class LineEditCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCLine*         m_editable_line;
	unsigned int     m_choise_pnt;
	unsigned int     m_step;

	SG_POINT       m_cur_pnt;
	
	ISelectPointPanel*    m_sel_point_panel;
	IGetPointPanel*       m_get_point_panel;

	bool           m_was_started;

	void           NewPanels();

	CString        m_message;

	CBitmap*         m_bitmap;
public:
	LineEditCommand(sgCLine* el, IApplicationInterface*  appI);
	virtual ~LineEditCommand();

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