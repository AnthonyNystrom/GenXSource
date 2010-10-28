#ifndef __TORUSCOMMAND__
#define __TORUSCOMMAND__

class TorusCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	
	IGetPointPanel*   m_base_point_panel;
	IGetVectorPanel*  m_normal_panel;
	IGetNumberPanel*  m_r1_panel;
	IGetNumberPanel*  m_r2_panel;

	unsigned int     m_step;
	SG_POINT         m_first_pnt;
	SG_VECTOR        m_dir;
	double           m_rad_1;
	double           m_rad_2;

	sgCMatrix*       m_matrix;

	void NewScenar();
	
	CString m_message;
public:
	TorusCommand(IApplicationInterface*  appI);
	virtual ~TorusCommand();

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