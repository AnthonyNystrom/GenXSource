#ifndef __PLANES__
#define __PLANES__

#include "3DCamera.h"
typedef struct  
{
	float position;
	float r;
	float g;
	float b;
	float a;
	BOOL  enable;
	BOOL  visible;
	float step;
} WORK_PLANE;

class CPlanes
{
private:
	WORK_PLANE  m_x_WP;
	WORK_PLANE  m_y_WP;
	WORK_PLANE  m_z_WP;

	WORK_PLANE* m_active_work_plane;
	SG_POINT*   m_snap_point;

	int         m_grid_lines_count;

	void        DrawAxes(C3dCamera* cam);
	
	bool        m_use_plane_point;
public:
	BOOL        IsXWorkPlaneEnable() {return m_x_WP.enable;};
	void        EnableXWorkPlane(BOOL enWP)  {m_x_WP.enable=enWP;}; 
	BOOL        IsXWorkPlaneVisible() {return m_x_WP.visible;};
	void        SetXWorkPlaneVisibles(BOOL visWP)  {m_x_WP.visible = visWP;}; 

	BOOL        IsYWorkPlaneEnable() {return m_y_WP.enable;};
	void        EnableYWorkPlane(BOOL enWP)  {m_y_WP.enable=enWP;}; 
	BOOL        IsYWorkPlaneVisible() {return m_y_WP.visible;};
	void        SetYWorkPlaneVisibles(BOOL visWP)  {m_y_WP.visible = visWP;}; 

	BOOL        IsZWorkPlaneEnable() {return m_z_WP.enable;};
	void        EnableZWorkPlane(BOOL enWP)  {m_z_WP.enable=enWP;}; 
	BOOL        IsZWorkPlaneVisible() {return m_z_WP.visible;};
	void        SetZWorkPlaneVisibles(BOOL visWP)  {m_z_WP.visible = visWP;}; 

	void        SetupDialog();

	void        SetUsingPlanePoint(bool upp) {m_use_plane_point=upp;};
	void        GetCurrentWorkPlane(double& d, SG_VECTOR& nrml);

	float       GetActiveWorkPlaneGridSize();
	
public:
	CPlanes();
	~CPlanes();

	void        DrawWorkPlanes(C3dCamera* cam,bool isCommanderRegime);
	SG_POINT*   GetPointOnWorkPlanes(SG_POINT*, SG_VECTOR*);

};
#endif