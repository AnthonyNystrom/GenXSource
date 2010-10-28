#ifndef __POINT_EDIT__
#define __POINT_EDIT__

class CPointEditCommand  : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCPoint*        m_editable_point;
	SG_POINT         m_cur_pnt;
	IGetPointPanel*  m_get_point_panel;
	bool             m_was_started;

	CBitmap*         m_bitmap;
public:
	CPointEditCommand(sgCPoint* edP, IApplicationInterface*  appI);
	virtual ~CPointEditCommand();

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