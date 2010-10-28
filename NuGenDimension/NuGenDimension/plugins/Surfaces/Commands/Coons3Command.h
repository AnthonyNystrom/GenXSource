#ifndef __Coons3Command__
#define __Coons3Command__

class Coons3Command : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_object_panels[3];
	
	sgC2DObject*           m_objs[3];

	sgCObject*             m_cur_obj;
	
	int                    m_step;
	CString m_message;

	bool  CheckTwoObjects();
	bool  CheckThreeObjects();

public:
	Coons3Command(IApplicationInterface* appI);
	virtual ~Coons3Command();

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