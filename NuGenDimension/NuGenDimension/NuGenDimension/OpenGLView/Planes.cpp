#include "stdafx.h"
#include "..//NuGenDimension.h"

#include "Planes.h"

#include "..//Dialogs//WorkPlanesSetupsDlg.h"

CPlanes::CPlanes()
{
	m_x_WP.enable = m_y_WP.enable = m_z_WP.enable = FALSE;
	m_x_WP.visible = m_y_WP.visible = m_z_WP.visible = TRUE;
	m_z_WP.enable = TRUE;
	m_x_WP.position = m_y_WP.position = m_z_WP.position = 0.0f;
	m_x_WP.r = 1.0f;
	m_x_WP.g = 0.0f;
	m_x_WP.b = 0.0f;
	m_x_WP.a = 0.2f;
	m_y_WP.r = 0.0f;
	m_y_WP.g = 1.0f;
	m_y_WP.b = 0.0f;
	m_y_WP.a = 0.2f;
	m_z_WP.r = 0.0f;
	m_z_WP.g = 0.0f;
	m_z_WP.b = 1.0f;
	m_z_WP.a = 0.2f;
	m_x_WP.step = m_y_WP.step = m_z_WP.step = 1.0f;

	m_active_work_plane = NULL;
	m_snap_point = NULL;

	m_grid_lines_count = 2;

	m_use_plane_point = false;
}

CPlanes::~CPlanes()
{
	if (m_snap_point)
		delete m_snap_point;
}

void  CPlanes::SetupDialog()
{
	CWorkPlanesSetupsDlg dlg;
	dlg.m_x_pos = m_x_WP.position;
	dlg.m_y_pos = m_y_WP.position;
	dlg.m_z_pos = m_z_WP.position;
	dlg.m_xy_vis = m_z_WP.visible;
	dlg.m_xz_vis = m_y_WP.visible;
	dlg.m_yz_vis = m_x_WP.visible;
	if (m_z_WP.enable)
		dlg.m_cur_work_plane = 0;
	else
		if (m_y_WP.enable)
			dlg.m_cur_work_plane = 1;
		else
			if (m_x_WP.enable)
				dlg.m_cur_work_plane = 2;
			else
			{
				ASSERT(0);
			}
	if (dlg.DoModal()==IDOK)
	{
		m_x_WP.position = dlg.m_x_pos ;
		m_y_WP.position = dlg.m_y_pos ;
		m_z_WP.position = dlg.m_z_pos ;
		m_x_WP.visible = dlg.m_yz_vis ;
		m_y_WP.visible = dlg.m_xz_vis ;
		m_z_WP.visible = dlg.m_xy_vis ;
		m_x_WP.enable = m_y_WP.enable = m_z_WP.enable = FALSE;
		if (dlg.m_cur_work_plane == 0)
			m_z_WP.enable = TRUE;
		else
			if (dlg.m_cur_work_plane == 1)
				m_y_WP.enable = TRUE;
			else
				if (dlg.m_cur_work_plane == 2)
					m_x_WP.enable = TRUE;
				else
				{
					ASSERT(0);
				}
	}
}

//#include "glMath.h"
#include <float.h>

void  CPlanes::DrawAxes(C3dCamera* cam)
{
}

double  EyeToGridStep(double dist)
{
	if (dist<=0.001 && dist>=-0.001)
		return 0.0001;
	if (dist<=0.01 && dist>=-0.01)
		return 0.001;
	if (dist<=0.1 && dist>=-0.1)
		return 0.01;
	if (dist<=1.0 && dist>=-1.0)
		return 0.1;
	if (dist<=10.0 && dist>=-10.0)
		return 1.0;
	if (dist<=100.0 && dist>=-100.0)
		return 10.0;
	if (dist<=1000.0 && dist>=-1000.0)
		return 100.0;
	if (dist<=10000.0 && dist>=-10000.0)
		return 1000.0;
	if (dist<=100000.0 && dist>=-100000.0)
		return 10000.0;
	return 100000.0;
}

void  CPlanes::DrawWorkPlanes(C3dCamera* cam,bool isCommanderRegime)
{
	glPushAttrib(GL_ENABLE_BIT);
	glEnable(GL_BLEND);
	glDisable(GL_CULL_FACE);
	glDisable(GL_LIGHTING);
	glDisable(GL_TEXTURE_2D);
	glDisable(GL_LINE_SMOOTH);
	glDisable(GL_DEPTH_TEST);

	SG_POINT EyePos;
	cam->GetEyePos(&EyePos.x,&EyePos.y,&EyePos.z);
	SG_POINT LookAtPos;
	cam->GetLookAtPos(&LookAtPos.x,&LookAtPos.y,&LookAtPos.z);

	if (m_x_WP.enable && m_x_WP.visible)
		{
			double plD = 0.0;
			SG_POINT plP = {m_x_WP.position, 0.0, 0.0};
			SG_VECTOR plN = {1.0, 0.0, 0.0};
			sgSpaceMath::PlaneFromNormalAndPoint(plP, plN, plD);
			SG_POINT resPnt;
			if (cam->ProjectCenterOfScreenOnPlane(plD,plN, resPnt))
			{
				bool  actP = m_active_work_plane==&m_x_WP && m_use_plane_point && isCommanderRegime;
					if (actP)
						glColor4f(m_x_WP.r, m_x_WP.g, m_x_WP.b, m_x_WP.a+0.4f);
					else
						glColor4f(m_x_WP.r, m_x_WP.g, m_x_WP.b, m_x_WP.a+0.1f);

					double curPos, tmpDbl;

					m_x_WP.step = (float)EyeToGridStep(sgSpaceMath::PointsDistance(resPnt,EyePos));

					double  begVal1 = FloorGrid(resPnt.z-m_x_WP.step*7.0,(float)m_x_WP.step);
					double  endVal1 = FloorGrid(resPnt.z+m_x_WP.step*7.0,(float)m_x_WP.step);
					double  begVal2 = FloorGrid(resPnt.y-m_x_WP.step*7.0,(float)m_x_WP.step);
					double  endVal2 = FloorGrid(resPnt.y+m_x_WP.step*7.0,(float)m_x_WP.step);

					glBegin(GL_LINES);
					for (curPos = begVal1; curPos<=endVal1; curPos+=m_x_WP.step)
					{
						bool isCel = false;
						if (fabs(modf(curPos/(double)(m_x_WP.step*10.0),&tmpDbl))<0.00001)
						{
							if (actP)
								glColor4f(0.0f, 0.0f, 0.0f, m_x_WP.a+0.6f);
							else
								glColor4f(0.0f, 0.0f, 0.0f, m_x_WP.a+0.3f);
							isCel = true;
						}
						glVertex3d(resPnt.x, begVal2, curPos);
						glVertex3d(resPnt.x, endVal2, curPos);
						if (isCel)
						{
							if (actP)
								glColor4f(m_x_WP.r, m_x_WP.g, m_x_WP.b, m_x_WP.a+0.4f);
							else
								glColor4f(m_x_WP.r, m_x_WP.g, m_x_WP.b, m_x_WP.a+0.1f);
						}
					}
					for (curPos = begVal2; curPos<=endVal2; curPos+=m_x_WP.step)
					{
						bool isCel = false;
						if (fabs(modf(curPos/(double)(m_x_WP.step*10.0),&tmpDbl))<0.00001)
						{
							if (actP)
								glColor4f(0.0f, 0.0f, 0.0f, m_x_WP.a+0.6f);
							else
								glColor4f(0.0f, 0.0f, 0.0f, m_x_WP.a+0.3f);
							isCel = true;
						}
						glVertex3d(resPnt.x,curPos,	 begVal1 );
						glVertex3d(resPnt.x,curPos,	 endVal1 );
						if (isCel)
						{
							if (actP)
								glColor4f(m_x_WP.r, m_x_WP.g, m_x_WP.b, m_x_WP.a+0.4f);
							else
								glColor4f(m_x_WP.r, m_x_WP.g, m_x_WP.b, m_x_WP.a+0.1f);
						}
					}
					glEnd();

		}

		}

	if (m_y_WP.enable && m_y_WP.visible)
		{
			double plD = 0.0;
			SG_POINT plP = {0.0, m_y_WP.position, 0.0};
			SG_VECTOR plN = {0.0, 1.0, 0.0};
			sgSpaceMath::PlaneFromNormalAndPoint(plP, plN, plD);
			SG_POINT resPnt;
			if (cam->ProjectCenterOfScreenOnPlane(plD,plN, resPnt))
			{
				bool  actP = m_active_work_plane==&m_y_WP && m_use_plane_point && isCommanderRegime;
					if (actP)
						glColor4f(m_y_WP.r, m_y_WP.g, m_y_WP.b, m_y_WP.a+0.4f);
					else
						glColor4f(m_y_WP.r, m_y_WP.g, m_y_WP.b, m_y_WP.a+0.1f);

					double curPos, tmpDbl;

					m_y_WP.step = (float)EyeToGridStep(sgSpaceMath::PointsDistance(resPnt,EyePos));

					double  begVal1 = FloorGrid(resPnt.z-m_y_WP.step*7.0,(float)m_y_WP.step);
					double  endVal1 = FloorGrid(resPnt.z+m_y_WP.step*7.0,(float)m_y_WP.step);
					double  begVal2 = FloorGrid(resPnt.x-m_y_WP.step*7.0,(float)m_y_WP.step);
					double  endVal2 = FloorGrid(resPnt.x+m_y_WP.step*7.0,(float)m_y_WP.step);

					glBegin(GL_LINES);
					for (curPos = begVal1; curPos<=endVal1; curPos+=m_y_WP.step)
					{
						bool isCel = false;
						if (fabs(modf(curPos/(double)(m_y_WP.step*10.0),&tmpDbl))<0.00001)
						{
							if (actP)
								glColor4f(0.0f, 0.0f, 0.0f, m_y_WP.a+0.6f);
							else
								glColor4f(0.0f, 0.0f, 0.0f, m_y_WP.a+0.3f);
							isCel = true;
						}
						glVertex3d(begVal2,	resPnt.y, curPos);
						glVertex3d(endVal2,	resPnt.y, curPos);
						if (isCel)
						{
							if (actP)
								glColor4f(m_y_WP.r, m_y_WP.g, m_y_WP.b, m_y_WP.a+0.4f);
							else
								glColor4f(m_y_WP.r, m_y_WP.g, m_y_WP.b, m_y_WP.a+0.1f);
						}
					}
					for (curPos = begVal2; curPos<=endVal2; curPos+=m_y_WP.step)
					{
						bool isCel = false;
						if (fabs(modf(curPos/(double)(m_y_WP.step*10.0),&tmpDbl))<0.00001)
						{
							if (actP)
								glColor4f(0.0f, 0.0f, 0.0f, m_y_WP.a+0.6f);
							else
								glColor4f(0.0f, 0.0f, 0.0f, m_y_WP.a+0.3f);
							isCel = true;
						}
						glVertex3d(curPos,	resPnt.y, begVal1 );
						glVertex3d(curPos,	resPnt.y, endVal1 );
						if (isCel)
						{
							if (actP)
								glColor4f(m_y_WP.r, m_y_WP.g, m_y_WP.b, m_y_WP.a+0.4f);
							else
								glColor4f(m_y_WP.r, m_y_WP.g, m_y_WP.b, m_y_WP.a+0.1f);
						}
					}
					glEnd();

					
			}
		}

	if (m_z_WP.enable && m_z_WP.visible)
		{
			double plD = 0.0;
			SG_POINT plP = {0.0,0.0,m_z_WP.position};
			SG_VECTOR plN = {0.0, 0.0, 1.0};
			sgSpaceMath::PlaneFromNormalAndPoint(plP, plN, plD);
			SG_POINT resPnt;
			if (cam->ProjectCenterOfScreenOnPlane(plD,plN, resPnt))
			{
					bool  actP = m_active_work_plane==&m_z_WP && m_use_plane_point && isCommanderRegime;
					if (actP)
						glColor4f(m_z_WP.r, m_z_WP.g, m_z_WP.b, m_z_WP.a+0.4f);
					else
						glColor4f(m_z_WP.r, m_z_WP.g, m_z_WP.b, m_z_WP.a+0.1f);

					double curPos, tmpDbl;
										
					m_z_WP.step = (float)EyeToGridStep(sgSpaceMath::PointsDistance(resPnt,EyePos));
					
					double  begVal1 = FloorGrid(resPnt.y-m_z_WP.step*7.0,(float)m_z_WP.step);
					double  endVal1 = FloorGrid(resPnt.y+m_z_WP.step*7.0,(float)m_z_WP.step);
					double  begVal2 = FloorGrid(resPnt.x-m_z_WP.step*7.0,(float)m_z_WP.step);
					double  endVal2 = FloorGrid(resPnt.x+m_z_WP.step*7.0,(float)m_z_WP.step);

					glBegin(GL_LINES);
					
					for (curPos = begVal1; curPos<=endVal1; curPos+=m_z_WP.step)
					{
						bool isCel = false;
						if (fabs(modf(curPos/(double)(m_z_WP.step*10.0),&tmpDbl))<0.00001)
						{
							if (actP)
								glColor4f(0.0f, 0.0f, 0.0f, m_z_WP.a+0.6f);
							else
								glColor4f(0.0f, 0.0f, 0.0f, m_z_WP.a+0.3f);
							isCel = true;
						}
						glVertex3d(begVal2,	curPos,	resPnt.z);
						glVertex3d(endVal2,	curPos,	resPnt.z);
						if (isCel)
						{
							if (actP)
								glColor4f(m_z_WP.r, m_z_WP.g, m_z_WP.b, m_z_WP.a+0.4f);
							else
								glColor4f(m_z_WP.r, m_z_WP.g, m_z_WP.b, m_z_WP.a+0.1f);
						}
					}
					for (curPos = begVal2; curPos<=endVal2; curPos+=m_z_WP.step)
					{
						bool isCel = false;
						if (fabs(modf(curPos/(double)(m_z_WP.step*10.0),&tmpDbl))<0.00001)
						{
							if (actP)
								glColor4f(0.0f, 0.0f,0.0f, m_z_WP.a+0.6f);
							else
								glColor4f(0.0f, 0.0f, 0.0f, m_z_WP.a+0.3f);
							isCel = true;
						}
						glVertex3d(curPos,	begVal1, resPnt.z);
						glVertex3d(curPos,	endVal1, resPnt.z);
						if (isCel)
						{
							if (actP)
								glColor4f(m_z_WP.r, m_z_WP.g, m_z_WP.b, m_z_WP.a+0.4f);
							else
								glColor4f(m_z_WP.r, m_z_WP.g, m_z_WP.b, m_z_WP.a+0.1f);
						}
					}
					glEnd();

			}
		}

	DrawAxes(cam);

	if (isCommanderRegime && m_snap_point && m_active_work_plane->step>FLT_EPSILON && m_grid_lines_count>0
						 && m_use_plane_point)
	{
		glBegin(GL_LINES);
		glColor4f(m_active_work_plane->r, m_active_work_plane->g, m_active_work_plane->b, 1.0f);
		if (m_active_work_plane==&m_x_WP)
		{
			glVertex3d(m_snap_point->x,m_snap_point->y-m_active_work_plane->step*0.5,m_snap_point->z);
			glVertex3d(m_snap_point->x,m_snap_point->y+m_active_work_plane->step*0.5,m_snap_point->z);
			glVertex3d(m_snap_point->x,m_snap_point->y,m_snap_point->z-m_active_work_plane->step*0.5);
			glVertex3d(m_snap_point->x,m_snap_point->y,m_snap_point->z+m_active_work_plane->step*0.5);
		}


		if (m_active_work_plane==&m_y_WP)
		{
			glVertex3d(m_snap_point->x-m_active_work_plane->step*0.5,m_snap_point->y,m_snap_point->z);
			glVertex3d(m_snap_point->x+m_active_work_plane->step*0.5,m_snap_point->y,m_snap_point->z);
			glVertex3d(m_snap_point->x,m_snap_point->y,m_snap_point->z-m_active_work_plane->step*0.5);
			glVertex3d(m_snap_point->x,m_snap_point->y,m_snap_point->z+m_active_work_plane->step*0.5);
		}

		if (m_active_work_plane==&m_z_WP)
		{
			glVertex3d(m_snap_point->x-m_active_work_plane->step*0.5,m_snap_point->y,m_snap_point->z);
			glVertex3d(m_snap_point->x+m_active_work_plane->step*0.5,m_snap_point->y,m_snap_point->z);
			glVertex3d(m_snap_point->x,m_snap_point->y-m_active_work_plane->step*0.5,m_snap_point->z);
			glVertex3d(m_snap_point->x,m_snap_point->y+m_active_work_plane->step*0.5,m_snap_point->z);
		}	
		glEnd();


	}

	glPopAttrib();	

}

#include <float.h>

SG_POINT*  CPlanes::GetPointOnWorkPlanes( SG_POINT* begAxePnt, SG_VECTOR* naprCos)
{
	SG_VECTOR planeNorm;
	SG_POINT  tmpPnt;
	double    dist = DBL_MAX;
	SG_POINT  resPnt;
	double    tmpDst ;
	
	m_active_work_plane = NULL;
	if (m_snap_point)
	{
		delete m_snap_point;
		m_snap_point = NULL;
	}

#define   COSINUS_60    0.5

	bool   second_prohod = false;

sec_pr:
	if (m_x_WP.enable)
	{
		planeNorm.x = 1.0;
		planeNorm.y = 0.0; 
		planeNorm.z = 0.0;

		if ((second_prohod || fabs(naprCos->x)>COSINUS_60) && sgSpaceMath::IntersectPlaneAndLine(planeNorm, -m_x_WP.position, 
																				*begAxePnt, *naprCos,
																				tmpPnt)==1)
		{
			if (((tmpPnt.x-begAxePnt->x)*naprCos->x+
				 (tmpPnt.y-begAxePnt->y)*naprCos->y+
				 (tmpPnt.z-begAxePnt->z)*naprCos->z)>0.00001)
			{
				tmpDst = sgSpaceMath::PointsDistance(tmpPnt,*begAxePnt);
				if (tmpDst>0.00001)
					if (tmpDst<dist)
					{
						dist = tmpDst;
						m_active_work_plane = &m_x_WP;
						resPnt = tmpPnt;
					}
			}
		}
	}

	if (m_y_WP.enable)
	{
		planeNorm.x = 0.0;
		planeNorm.y = 1.0; 
		planeNorm.z = 0.0;

		if ((second_prohod || fabs(naprCos->y)>COSINUS_60) && sgSpaceMath::IntersectPlaneAndLine(planeNorm, -m_y_WP.position, 
																				*begAxePnt, *naprCos,
																				tmpPnt)==1)
		{
			if (((tmpPnt.x-begAxePnt->x)*naprCos->x+
				 (tmpPnt.y-begAxePnt->y)*naprCos->y+
				 (tmpPnt.z-begAxePnt->z)*naprCos->z)>0.00001)
			{
				tmpDst = sgSpaceMath::PointsDistance(tmpPnt,*begAxePnt);
				if (tmpDst>0.00001)
					if (tmpDst<dist)
					{
						dist = tmpDst;
						m_active_work_plane = &m_y_WP;
						resPnt = tmpPnt;
					}
			}
		}
	}

	if (m_z_WP.enable)
	{
		planeNorm.x = 0.0;
		planeNorm.y = 0.0; 
		planeNorm.z = 1.0;

		if ((second_prohod || fabs(naprCos->z)>COSINUS_60) && sgSpaceMath::IntersectPlaneAndLine(planeNorm, -m_z_WP.position, 
																				*begAxePnt, *naprCos,
																				tmpPnt)==1)
		{
			if (((tmpPnt.x-begAxePnt->x)*naprCos->x+
				(tmpPnt.y-begAxePnt->y)*naprCos->y+
				(tmpPnt.z-begAxePnt->z)*naprCos->z)>0.00001)
			{
				tmpDst = sgSpaceMath::PointsDistance(tmpPnt,*begAxePnt);
				if (tmpDst>0.00001)
					if (tmpDst<dist)
					{
						dist = tmpDst;
						m_active_work_plane = &m_z_WP;
						resPnt = tmpPnt;
					}
			}
		}
	}

	if (!second_prohod && m_active_work_plane==NULL)
	{
		second_prohod = true;
		goto sec_pr;
	}

	if (m_active_work_plane)
	{
		m_snap_point = new SG_POINT;
		*m_snap_point = resPnt;
	}
	return m_snap_point;
}

void   CPlanes::GetCurrentWorkPlane(double& d, SG_VECTOR& nrml)
{
	if (!m_active_work_plane)
	{
		ASSERT(0);
		return;
	}
	d = m_active_work_plane->position;
	if (m_active_work_plane==&m_x_WP)
	{
		nrml.x = 1.0;
		nrml.y = 0.0;
		nrml.z = 0.0;
	}
	else
		if (m_active_work_plane==&m_y_WP)
		{
			nrml.x = 0.0;
			nrml.y = 1.0;
			nrml.z = 0.0;
		}
		else
			if (m_active_work_plane==&m_z_WP)
			{
				nrml.x = 0.0;
				nrml.y = 0.0;
				nrml.z = 1.0;
			}
			else
			{
				ASSERT(0);
			}
}

float  CPlanes::GetActiveWorkPlaneGridSize()
{
	if (m_z_WP.enable)
		return m_z_WP.step;
	if (m_y_WP.enable)
		return m_y_WP.step;
	if (m_x_WP.enable)
		return m_x_WP.step;
	ASSERT(0);
	return 0.0f;
}