#ifndef __Contour__
#define __Contour__

//#include "..//Dialogs//ContScenarDlg.h"

class CContObjDlg;
class CContScenarDlg;

class Contour : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCObject*          m_start_object;
	int                 m_scenar;
	int                 m_step;
	bool                m_lines;
	IGetObjectsPanel*   m_get_object_panel;

	CContScenarDlg*     m_scenar_panel;
	CContObjDlg*        m_select_obj_type_panel;

	IGetPointPanel*     m_get_first_point;
	IGetPointPanel*     m_get_second_point;
	IGetPointPanel*     m_get_third_point;

	CString m_message;

	SG_POINT            m_cur_point;

	SG_POINT        m_first_point;
	SG_POINT        m_tmp_first_point;
	SG_POINT        m_tmp_second_point;
	bool            m_isFirstPoint;
	bool            m_isSecondPoint;
	bool            m_isLastPointOnArc;
	bool            m_exist_arc_data;
	bool            m_can_close;
	std::vector<sgCObject*>   m_objects;

	SG_ARC          m_arc_geo;


	void    CreateContour(sgCObject*);
	void    CreateContourFromObjects();
public:
	void    SwitchScenario(int newScen);
	void    SwitchObjectType(int newObjType);
public:
	Contour(IApplicationInterface*  appI);
	virtual ~Contour();

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