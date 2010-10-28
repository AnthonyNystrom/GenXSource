#ifndef __CONECOMMAND__
#define __CONECOMMAND__

class ConeCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	SG_POINT         m_first_pnt;
	SG_POINT         m_second_pnt;
	SG_VECTOR        m_dir;
	double           m_length;
	double           m_rad_1;
	double           m_rad_2;
	unsigned int     m_step;
	
	IGetPointPanel*  m_get_first_point_panel;
	IGetPointPanel*  m_get_second_point_panel;
	IGetNumberPanel*  m_get_r1_panel;
	IGetNumberPanel*  m_get_r2_panel;

	CString m_message;

	void     NewScenar();
public:
	ConeCommand(IApplicationInterface*  appI);
	virtual ~ConeCommand();

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