#ifndef __RD_DIM_Command__
#define __RD_DIM_Command__


#include "..//LinearDimDlg.h"
#include "..//TextParamsDlg.h"

class RDDimCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*       m_get_object_panel;
	IGetPointPanel*        m_get_point_panel;

	IComboPanel*           m_precision_combo;
	CLinearDimDlg*         m_props_panel;
	IComboPanel*           m_text_align_panel;
	CTextParamsDlg*        m_text_params_panel;


	int                    m_step;

	CString m_message;

	sgC2DObject*           m_obj;
	sgCObject*             m_cur_obj;

	SG_POINT               m_cur_pnt;
	SG_POINT               m_first_pnt;
	SG_POINT               m_second_pnt;
	SG_POINT               m_last_pnt;
	
	SG_DIMENSION_STYLE        m_dim_style;

	CString                m_text;

	void                   NewScenar();

	bool                   m_rad_regime;
public:
	RDDimCommand(bool rad, IApplicationInterface* appI);
	virtual ~RDDimCommand();

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