#ifndef __FaceCommand__
#define __FaceCommand__

#include <list>
#include <vector>


typedef enum
{
	HO_UNKNOWN_ERROR,
	HO_OUT_NOT_FLAT,
	HO_OUT_BAD_ORIENT,
	HO_IN_NO_CLOSE,
	HO_IN_NO_FLAT,
	HO_NOT_IN_ONE_PLANE,
	HO_INTERSECTS_WITH_OUT,
	HO_IN_NOT_INSIDE_OUT,
	HO_INTERSECTS_WITH_OTHER_HOLE,
	HO_INSIDE_EXIST_HOLE,
	HO_CONTAIN_EXIST_HOLE,
	HO_SUCCESS
} HO_CORRECT_PATHS_RES;


class FaceCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_main_object_panel;
	IGetObjectsPanel*      m_get_sub_objects_panel;
	
	sgCObject*             m_first_obj;
	std::vector<sgCObject*>  m_otv;
	int                    m_step;
	CString m_message;

	SG_VECTOR              m_plane_Normal;
	double                 m_plane_D;

	std::list<SG_LINE>     m_object_lines;
	void                   FillObjectLinesList(sgCObject*);
	bool                   IsIntersect(SG_LINE*);

	HO_CORRECT_PATHS_RES   GoodObjectForHole(const sgC2DObject* try_hole);
public:
	FaceCommand(IApplicationInterface* appI);
	virtual ~FaceCommand();

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