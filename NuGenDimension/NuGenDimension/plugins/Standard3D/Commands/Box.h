#ifndef __BOXCOMMAND__
#define __BOXCOMMAND__

class BoxCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	
	IGetPointPanel*   m_base_point_panel;
	IGetNumberPanel*  m_x_size_panel;
	IGetNumberPanel*  m_y_size_panel;
	IGetNumberPanel*  m_z_size_panel;

	sgCMatrix*       m_matrix;

	SG_POINT         m_first_pnt;
	SG_POINT         m_projection;
	double           size1;
	double           size2;
	double           size3;
	unsigned int     m_step;

	CString m_message;

	void     NewScenar();
public:
	BoxCommand(IApplicationInterface*  appI);
	virtual ~BoxCommand();

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