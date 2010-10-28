#ifndef __SPHERICBANDCOMMAND__
#define __SPHERICBANDCOMMAND__

class SphericBandCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	SG_POINT         m_cur_pnt;
	SG_POINT         m_first_pnt;
	SG_POINT         m_fir_projection;
	SG_POINT         m_projection;
	unsigned int     m_step;
	
	IGetPointPanel*   m_base_point_panel;
	IGetNumberPanel*  m_r_panel;
	IGetNumberPanel*  m_k1_panel;
	IGetNumberPanel*  m_k2_panel;

	double           m_rad;
	double           m_coef[2];

	sgCMatrix*       m_matrix;

	CString m_message;

	void     NewScenar();
public:
	SphericBandCommand(IApplicationInterface*  appI);
	virtual ~SphericBandCommand();
	
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