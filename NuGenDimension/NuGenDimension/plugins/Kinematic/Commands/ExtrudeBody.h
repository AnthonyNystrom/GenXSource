#ifndef __ExtrudeCommand__
#define __ExtrudeCommand__

#include <list>
#include <vector>

class ExtrudeCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_main_object_panel;
	IGetObjectsPanel*      m_get_sub_objects_panel;
	IGetNumberPanel*       m_get_H_panel;
	
	sgCObject*             m_first_obj;
	std::vector<sgCObject*>  m_otv;
	int                    m_step;
	CString m_message;

	SG_VECTOR              m_plane_Normal;
	SG_POINT               m_gab_center;

	bool                   m_inQuestionRegime;
	SG_POINT               m_cur_pnt;

	std::list<SG_LINE>     m_object_lines;
	void                   FillObjectLinesList(sgCObject*);
	
	HO_CORRECT_PATHS_RES   GoodObjectForHole(const sgC2DObject* try_hole);
public:
	ExtrudeCommand(IApplicationInterface* appI);
	virtual ~ExtrudeCommand();

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