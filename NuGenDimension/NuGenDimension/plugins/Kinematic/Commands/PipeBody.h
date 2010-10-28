#ifndef __PipeCommand__
#define __PipeCommand__

#include <list>
#include <vector>

class PipeCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_main_object_panel;
	IGetObjectsPanel*      m_get_sub_objects_panel;
	IGetObjectsPanel*      m_get_pipe_profile;
	
	sgCObject*             m_first_obj;
	std::vector<sgCObject*>  m_otv;
	int                    m_step;
	CString m_message;

	SG_POINT               m_zero_point_on_cont;

	sgC2DObject*           m_pipe_cont;

	bool                   m_inQuestionRegime;
	
	std::list<SG_LINE>     m_object_lines;
	void                   FillObjectLinesList(sgCObject*);
	
	HO_CORRECT_PATHS_RES   GoodObjectForHole(const sgC2DObject* try_hole);
public:
	PipeCommand(IApplicationInterface* appI);
	virtual ~PipeCommand();

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