#ifndef __COMMONSTRUCTURES__
#define __COMMONSTRUCTURES__

#include "sgCore/sgCore.h"

typedef    enum
{
    PLUGIN_ERROR,
    PLUGIN_TOOLBAR
}  PLUGIN_TYPE;

typedef  struct 
{
	PLUGIN_TYPE       plugin_type;
	CString           menu_string;
	bool              show_after_load;
	bool              in_trial_version;
	unsigned int      plugin_version;
	unsigned int      NuGenDimension_version;
	unsigned int      kernel_version;
} PLUGIN_INFO;

typedef  struct  
{
	int			  nhits;
	unsigned int* buffer;
} SELECT_BUFFER;

typedef enum
{
	SNAP_NO=0,
	SNAP_SYSTEM,
	SNAP_POINTS,
	SNAP_ENDS,
	SNAP_MIDS,
	SNAP_CENTERS
} SNAP_TYPE;

struct  IPainter
{
	virtual void    SetCurColor(float, float,float)					=0;
	virtual void    SetLineWidth(float)								=0;
	virtual void    SetPointWidth(float)							=0;
	virtual void    GetUserColorLines(float&,float&,float&)			=0;
	virtual void    GetUserColorPoints(float&,float&,float&)		=0;

	virtual void    SetTransformMatrix(const sgCMatrix*)            =0;

	virtual void	DrawPoint(const SG_POINT&)						=0;
	virtual void	DrawLine(const SG_LINE&)						=0;
	virtual void	DrawCircle(const SG_CIRCLE&)					=0;
	virtual void	DrawArc(const SG_ARC&)							=0;
	virtual void	DrawSpline(SG_SPLINE*)				            =0;
	virtual void	DrawSplineFrame(SG_SPLINE*)			            =0;
	
	virtual void    DrawSphere(double rad)							=0;
	virtual void    DrawBox(double sz1, double sz2,double sz3)		=0;
	virtual void    DrawCone(double rd1, double rd2, double h)		=0;
	virtual void    DrawCylinder(double rd, double h)				=0;
	virtual void    DrawEllipsoid(double rd1, double rd2, double rd3)  =0;
	virtual void    DrawTorus(double rd1, double rd2)				   =0;
	virtual void    DrawSphericBand(double rd, double begC, double endC) =0;

	virtual void    DrawObject(const sgCObject* objct)                 =0;
};

struct  IViewPort
{
	virtual CWnd*           GetWindow()													=0;
	virtual void			InvalidateViewPort()										=0;

	virtual void			UnProjectScreenPoint(int,int,double,SG_POINT&)				=0;
	virtual void			ProjectWorldPoint(const SG_POINT&, double&, double&, double&)=0;

	typedef enum
	{
		USER_VIEW=0,
		FRONT_VIEW,
		BACK_VIEW,
		TOP_VIEW,
		BOTTOM_VIEW,
		LEFT_VIEW,
		RIGHT_VIEW,
	} VIEW_PORT_VIEW_TYPE;

	virtual VIEW_PORT_VIEW_TYPE   GetViewPortViewType()                                 =0;

	virtual void			GetViewPortNormal(SG_VECTOR&)								=0;

	virtual int             GetSnapSize()												=0;
	virtual SELECT_BUFFER   GetHitsInRect(const CRect&, bool selSubObj=false)			=0;
	virtual sgCObject*	    GetTopObject(SELECT_BUFFER)									=0;
	virtual sgCObject*	    GetTopObjectByType(SELECT_BUFFER, SG_OBJECT_TYPE)	=0;
	virtual bool            IsOnAnyObject(SELECT_BUFFER)								=0;

	virtual float           GetGridSize()												=0;

	typedef struct 
	{
		int        scrX;
		int		   scrY;
		SNAP_TYPE  snapType;
		bool       XFix;
		bool       YFix;
		bool	   ZFix;
		SG_POINT   FixPoint;
	} GET_SNAP_IN;

	typedef struct  
	{
		SG_POINT   result_point;
		bool       isOnWorkPlane;
		double	   snapWorkPlaneD;
		SG_VECTOR  snapWorkPlaneNormal;
	} GET_SNAP_OUT;

	virtual void   GetWorldPointAfterSnap(const GET_SNAP_IN&,
											GET_SNAP_OUT&)=0;

	virtual bool 	        ProjectScreenPointOnPlane(int, int, 
						SG_VECTOR&, double, SG_POINT&)							=0;
	virtual bool            ProjectScreenPointOnLine(int,int,
						const SG_POINT&, const SG_VECTOR&, SG_POINT&)			=0;

	virtual IPainter*       GetPainter()										=0;

	virtual void            SetEditableObject(sgCObject*)								=0;

	virtual void            SetHotObject(sgCObject*)									=0;
	virtual sgCObject*      GetHotObject()						=0;

};

struct  IBaseInterfaceOfGetDialogs
{
	typedef enum
	{
		GET_POINT_DLG,
		GET_NUMBER_DLG,
		GET_VECTOR_DLG,
		GET_OBJECTS_DLG,
		SELECT_POINT_DLG,
		COMBO_DLG,
		USER_DIALOG
	}  DLG_TYPE;

	virtual  DLG_TYPE  GetType() = 0;
	virtual  CWnd*     GetWindow() =0;   

	virtual  void      EnableControls(bool) = 0;

	virtual  ~IBaseInterfaceOfGetDialogs() {};
};


struct IGetPointPanel : public IBaseInterfaceOfGetDialogs
{
	virtual bool  SetPoint(double x, double y, double z)=0;
	virtual bool  GetPoint(double& x, double& y, double& z) =0;
	virtual bool  IsXFixed()=0;
	virtual bool  IsYFixed()=0;
	virtual bool  IsZFixed()=0;
	virtual bool  XFix(bool fix)=0;
	virtual bool  YFix(bool fix)=0;
	virtual bool  ZFix(bool fix)=0;

	virtual ~IGetPointPanel() {};
};

struct IGetNumberPanel  : public IBaseInterfaceOfGetDialogs
{
	virtual double GetNumber()=0;
	virtual void   SetNumber(double nmbr)=0;

	virtual ~IGetNumberPanel() {};
};

struct  IGetVectorPanel : public IBaseInterfaceOfGetDialogs
{
	typedef enum
	{
		X_VECTOR,
		Y_VECTOR,
		Z_VECTOR,
		USER_VECTOR
	} VECTOR_TYPE;

	virtual VECTOR_TYPE   GetVector(double& x, double& y, double& z)=0;
	virtual void          SetVector(VECTOR_TYPE n_t, 
		double x, double y, double z)=0;
	virtual bool   IsXFixed()=0;
	virtual bool   IsYFixed()=0;
	virtual bool   IsZFixed()=0;
	virtual bool   XFix(bool fix)=0;
	virtual bool   YFix(bool fix)=0;
	virtual bool   ZFix(bool fix)=0;

	virtual ~IGetVectorPanel() {};
};

struct IGetObjectsPanel : public IBaseInterfaceOfGetDialogs
{
	typedef bool(*LPFUNC_FILL_OBJECTS_LIST)(sgCObject*);

	virtual void  RemoveAllObjects()    =0;
	virtual void  SetMultiselectMode(bool)   =0;
	virtual void  FillList(LPFUNC_FILL_OBJECTS_LIST isAdd=NULL) =0;

	virtual void  AddObject(sgCObject*,bool)				=0;
	virtual void  RemoveObject(sgCObject*)				=0;
	virtual void  SelectObject(sgCObject*, bool)      =0;

	virtual ~IGetObjectsPanel() {};
};

struct IComboPanel  : public IBaseInterfaceOfGetDialogs
{
	virtual void            AddString(const char*)     =0;
	virtual void            RemoveAllStrings()	       =0;
	virtual void            SetCurString(unsigned int) =0;
	virtual unsigned int    GetCurString()             =0;

	virtual ~IComboPanel() {};
};

struct ISelectPointPanel  : public IBaseInterfaceOfGetDialogs
{
	virtual void   AddPoint(double,double,double)=0;
	virtual void   RemoveAllPoints()	   =0;
	virtual void   SetCurrentPoint(unsigned int) =0;
	virtual unsigned int    GetCurrentPoint()    =0;

	virtual ~ISelectPointPanel() {};
};

struct  ICommandPanel
{
	virtual  CWnd* GetDialogsContainerWindow() = 0;

	virtual  bool	AddDialog(IBaseInterfaceOfGetDialogs*,const char*, bool) = 0;
	virtual  IBaseInterfaceOfGetDialogs*   AddDialog(IBaseInterfaceOfGetDialogs::DLG_TYPE, 
		const char*, bool) = 0;

	virtual  bool   RemoveDialog(IBaseInterfaceOfGetDialogs*)       =0;
	virtual  bool   RemoveDialog(unsigned int)						=0;

	virtual  bool   RenameRadio(unsigned int, const char*)			=0;

	virtual  void   EnableRadio(unsigned int,bool) = 0;
	virtual  void   SetActiveRadio(unsigned int) = 0;

	virtual  bool   RemoveAllDialogs() = 0;

	virtual  void   DrawGroupFrame(CDC* pDC, const CRect& rct, 
										const int leftLab, const int rightLab)   =0;
};

struct IContextMenuInterface 
{
	virtual unsigned int    GetItemsCount()									=0;
	virtual void            GetItem(unsigned int, CString&)					=0;
	virtual void            GetItemState(unsigned int, bool&, bool&)		=0;
	virtual HBITMAP         GetItemBitmap(unsigned int)						=0;
	virtual void            Run(unsigned int)								=0;
};

#define GET_X_LPARAM(lp)                        ((int)(short)LOWORD(lp))
#define GET_Y_LPARAM(lp)                        ((int)(short)HIWORD(lp))

struct ICommander
{
	virtual void            Start()														=0;
	virtual bool            PreTranslateMessage(MSG* pMsg)								=0;
	virtual void            Draw()														=0;
	virtual IContextMenuInterface*    GetContextMenuInterface()							=0;

	typedef enum
	{
		CM_UPDATE_COMMAND_PANEL,   // void* == NULL
		CM_SWITCH_ROLLUP_DIALOG,    // void* == int* - clicking dialog number
		CM_SELECT_OBJECT,           // void* = sgCObject* - clicking object pointer
		CM_CHANGE_COMBO	,           // void* = IComboPanel* - clicking combo
	} COMMANDER_MESSAGE;

	virtual void            SendCommanderMessage(COMMANDER_MESSAGE, void*)				=0;

	virtual					~ICommander(){};
};

struct  IApplicationInterface
{
	typedef enum
	{
		MT_MESSAGE,
		MT_WARNING,
		MT_ERROR
	} MESSAGE_TYPE;
	virtual void PutMessage(MESSAGE_TYPE,const char* mes_str)=0;

	virtual IViewPort*		GetViewPort()=0;
	virtual ICommandPanel*	GetCommandPanel()=0;
	virtual double          ApplyPrecision(double)=0;
	virtual void            CopyAttributes(sgCObject& where_obj, 
									const sgCObject& from_obj)  =0;
	virtual void            ApplyAttributes(sgCObject*)         =0;
	virtual void            StartCommander(const char*)=0;
	virtual void            StopCommander() = 0;
};

#endif