#ifndef __ScrewCommand__
#define __ScrewCommand__

#include <list>
#include <vector>

class ScrewCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_main_object_panel;
	IGetObjectsPanel*      m_get_sub_objects_panel;
	IGetPointPanel*        m_get_f_p_panel;
	IGetPointPanel*        m_get_s_p_panel;
	IGetNumberPanel*        m_get_step_panel;
	IGetNumberPanel*        m_get_len_panel;
	
	sgCObject*             m_first_obj;
	std::vector<sgCObject*>  m_otv;
	int                    m_step;

	CString                m_message;

	SG_POINT               m_obj_gab_center;
	SG_POINT               m_project_on_axe_obj_gab_center;

	SG_VECTOR              m_plane_Normal;
	double                 m_plane_D;

	SG_POINT               m_axe_p1;
	SG_POINT               m_axe_p2;

	SG_VECTOR              m_axe_dir_no_Normal;

	bool                   m_good_step_of_screw;
	double                 m_cur_step_of_screw;

	bool                   m_good_len_of_screw;
	double                 m_cur_len_of_screw;
	bool                   m_invert_length;

	bool                   m_need_third_pnt_for_plane;
	bool                   m_need_fourth_pnt_for_plane;
	bool                   m_bad_point_on_plane;
	bool                   m_inQuestionRegime;

	SG_POINT               m_cur_pnt;

	std::list<SG_LINE>     m_object_lines;
	void                   FillObjectLinesList(sgCObject*);
	
	HO_CORRECT_PATHS_RES   GoodObjectForHole(const sgC2DObject* try_hole);
public:
	ScrewCommand(IApplicationInterface* appI);
	virtual ~ScrewCommand();

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