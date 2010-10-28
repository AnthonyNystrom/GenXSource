#ifndef __RotationCommand__
#define __RotationCommand__

#include <list>

class RotationCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_contour_panel;
	IGetPointPanel*        m_get_first_point_panel;
	IGetPointPanel*        m_get_second_point_panel;
	IGetNumberPanel*       m_get_angle_panel;

	SG_POINT               m_obj_gab_center;
	
	SG_POINT               m_cur_pnt;

	SG_ARC                 m_draw_arc;
	SG_POINT               m_arc_center;
	bool                   m_exist_draw_arc;

	sgCObject*             m_first_obj;
	SG_POINT               m_axe_p1;
	SG_POINT               m_axe_p2;
	double                 m_angle;
	int                    m_step;
	CString m_message;

	SG_VECTOR              m_plane_Normal;
	double                 m_plane_D;

	bool                   m_want_close;
	bool                   m_need_third_pnt_for_plane;
	bool                   m_need_fourth_pnt_for_plane;
	bool                   m_bad_point_on_plane;
	bool                   m_inQuestionRegime;

	std::list<SG_LINE>     m_object_lines;
	void                   FillObjectLinesList(sgCObject*);
	bool                   IsIntersect(SG_LINE*);
public:
	RotationCommand(IApplicationInterface* appI);
	virtual ~RotationCommand();

	virtual void            Start()	;
	virtual bool            PreTranslateMessage(MSG* pMsg);
	virtual void            Draw();
	IContextMenuInterface*    GetContextMenuInterface() {return this;};

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*);

private:	
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

	void            MouseMove(unsigned int,int,int);
	void            LeftClick(unsigned int,int,int);
	void            OnEnter();
};

#endif