#ifndef __CYLINDERCOMMAND__
#define __CYLINDERCOMMAND__

class CylinderCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	SG_POINT         m_first_pnt;
	SG_POINT         m_second_pnt;
	SG_VECTOR        m_dir;
	double           m_length;
	unsigned int     m_step;
	
	IGetPointPanel*  m_get_first_point_panel;
	IGetPointPanel*  m_get_second_point_panel;
	IGetNumberPanel*  m_get_r_panel;
	
	double           m_rad;

	CString m_message;

	void     NewScenar();
public:
	CylinderCommand(IApplicationInterface*  appI);
	virtual ~CylinderCommand();

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