#ifndef __GROUP_COMMAND__
#define __GROUP_COMMAND__

class GroupCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*     m_get_objects_panel;
	
	CString m_message;

public:
	GroupCommand(IApplicationInterface*  appI);
	virtual ~GroupCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:
	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP  GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);
};


#endif