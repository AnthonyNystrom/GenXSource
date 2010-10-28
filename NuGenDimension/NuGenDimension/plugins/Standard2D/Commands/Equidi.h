#ifndef __Equidi1__
#define __Equidi1__

class Equidi : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCObject*      m_start_object;
	
	IGetObjectsPanel*   m_get_object_panel;
	IGetNumberPanel*    m_get_H_panel;

	unsigned int    m_step;

	CString m_message;

	bool    CheckStartObject();

	SG_POINT    m_end_points[2];
	SG_VECTOR   m_end_vectors[2];

	SG_POINT    m_projection;

	double      m_otstup;


public:
	Equidi(IApplicationInterface*  appI);
	virtual ~Equidi();

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