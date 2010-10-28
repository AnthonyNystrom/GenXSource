#ifndef __SPHERECOMMAND__
#define __SPHERECOMMAND__

class SphereCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	SG_POINT         m_first_pnt;
	double           m_rad;
	unsigned int     m_step;
	
	IGetPointPanel*   m_base_point_panel;
	IGetNumberPanel*  m_r_panel;
	
	sgCMatrix*       m_matrix;
	CString m_message;

	void     NewScenar();
public:
	SphereCommand(IApplicationInterface*  appI);
	virtual ~SphereCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:	
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP    GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();

};

#endif