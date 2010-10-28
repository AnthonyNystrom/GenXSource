#ifndef __SPLINE_SURFACE_Command__
#define __SPLINE_SURFACE_Command__

#include "LinearSurf.h"

class SplineSurfaceCommand : public ICommander, public IContextMenuInterface
{
	IApplicationInterface* m_app;
private:
	IGetObjectsPanel*      m_get_object_panel;

	std::vector<sgC2DObject*>  m_objs;

	sgCObject*             m_cur_obj;

	
	std::vector< std::vector<SG_POINT> >  m_points_on_curve;
	
	std::vector<POINT_AND_HER_COEF>  m_temp_buffer;

	std::vector<bool>      m_inverse_curve;
	std::vector<double>    m_coef_on_curve;

	int                    m_step;

	CString				   m_message;

	NEED_REGIME            m_need_regime;

	void                   NeedObject(int pX, int pY);
	void                   NeedOneOfEndPoint(int pX, int pY);
	void                   NeedPointOnCurve(int pX, int pY);
	void                   NeedDirection(int pX, int pY);

	bool                   m_inQuestionRegime;

public:
	SplineSurfaceCommand(IApplicationInterface* appI);
	virtual ~SplineSurfaceCommand();

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