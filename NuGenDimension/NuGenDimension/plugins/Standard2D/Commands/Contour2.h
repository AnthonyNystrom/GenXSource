#ifndef __Contour2__
#define __Contour2__

class Contour2 : public ICommander, public IContextMenuInterface
{
	IApplicationInterface*  m_app;
private:
	sgCObject*      m_start_object;
	ICommandPanel*   m_panel;

	SG_POINT        m_cur_point;
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

	bool            m_line_regime;

	CString m_message;

	void    CreateContour();
public:
	Contour2(IApplicationInterface*  appI);
	virtual ~Contour2();

	virtual void            Start()	;
	virtual void            MouseMove(unsigned int,int,int);
	virtual void            LeftClick(unsigned int,int,int);
	virtual void            Draw();	
	virtual void            OnEnter();
	virtual unsigned int    GetItemsCount();
	virtual void            GetItem(unsigned int, CString&);
	virtual void            GetItemState(unsigned int, bool&, bool&);
	virtual HBITMAP  GetItemBitmap(unsigned int);
	virtual void            Run(unsigned int);

};

#endif