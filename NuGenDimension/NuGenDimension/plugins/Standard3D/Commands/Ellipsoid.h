#ifndef __ELLIPSOIDCOMMAND__
#define __ELLIPSOIDCOMMAND__

class EllipsoidCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	
	IGetPointPanel*   m_base_point_panel;
	IGetNumberPanel*  m_x_size_panel;
	IGetNumberPanel*  m_y_size_panel;
	IGetNumberPanel*  m_z_size_panel;

	SG_POINT         m_first_pnt;
	double           size1;
	double           size2;
	double           size3;
	unsigned int     m_step;
	SG_POINT         m_projection;

	CString m_message;

	void     NewScenar();
public:
	EllipsoidCommand(IApplicationInterface*  appI);
	virtual ~EllipsoidCommand();

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