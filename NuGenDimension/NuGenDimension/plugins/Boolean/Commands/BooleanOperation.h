#ifndef __BooleanCommand__
#define __BooleanCommand__

typedef enum
{
	BO_INTSCT,
	BO_UNION,
	BO_SUB,
	BO_IL
} BO_TYPE;

class BooleanCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	SG_POINT         m_cur_pnt;

	BO_TYPE          m_bo_type;

	IGetObjectsPanel*  m_get_first_obj_panel;
	IGetObjectsPanel*  m_get_second_obj_panel;
	
	int              m_step;

	sgC3DObject*     m_first_obj;
	sgC3DObject*     m_second_obj;
	bool             m_sec_obj_sel;

	CString m_message;
private:	

	void                    BuildBoolean();

public:
	BooleanCommand(IApplicationInterface* appI,BO_TYPE bt);
	virtual ~BooleanCommand();

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