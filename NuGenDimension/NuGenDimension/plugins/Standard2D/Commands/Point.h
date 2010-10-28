#ifndef __POINTCOMMAND__
#define __POINTCOMMAND__

class PointCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	
	CString m_message;

	IGetPointPanel*   m_get_point_panel;
public:
	PointCommand(IApplicationInterface*  appI);
	virtual ~PointCommand();

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