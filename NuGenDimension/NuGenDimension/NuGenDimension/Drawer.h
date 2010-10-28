#ifndef __DRAWER__
#define __DRAWER__

#include "Tools//MatLoader.h"

class Drawer
{
public:
	static   bool				  DrawObject(GLenum,sgCObject*,bool selSubObjects,
												bool asHot = false);
	static   const float*		  GetColorByIndex(unsigned short);
	static   const unsigned short GetLineTypeByIndex(unsigned short);

	static   void     DrawStylingLine(unsigned short styl,
											CDC* pDC, 
											const CPoint& startP, 
											int wid);

	static   void  DrawGabariteBox(const SG_POINT& pMin,const SG_POINT& pMax, const float* col);

	typedef enum
	{
		PROJECT_LINES,
		PROJECT_FACES
	} WMF_PROJECT_TYPE;

	static   bool                 ProjectObjectOnMetaDC(sgCObject*,
									CDC *pDC,
									double *modelMatrix,
									double *projMatrix,
									int *viewport,
									int height,
									WMF_PROJECT_TYPE projType = PROJECT_LINES,
									double ratio = 30.0,
									float RatioNbFace = 1.0f);

	static   sgCObject*           CurrentHotObject;
	static   sgCObject*           TopParentOfHotObject;
	static   sgCObject*           CurrentEditableObject;
	static   float                HotObjectColor[3];
	static   float                ColorOfObjectInHotGroup[3];
	static   float                SelectedObjectColor[3];

	static   bool                 is_VBO_Supported;

	static   CMatLoader           MatLoader;
private:
	static   bool    DrawPoint(GLenum,sgCPoint*,bool,
									bool asHot = false);
	static   bool    DrawLine(GLenum,sgCLine*,bool,
									bool asHot = false);
	static   bool    DrawCircle(GLenum,sgCCircle*,bool,
									bool asHot = false);
	static   bool    DrawArc(GLenum,sgCArc*,bool,
									bool asHot = false);
	static   bool    DrawSpline(GLenum,sgCSpline*,bool,
									bool asHot = false);
	static   bool    DrawText(GLenum,sgCText*,bool,
									bool asHot = false);
	static   bool    DrawDimensions(GLenum,sgCDimensions*,bool,
									bool asHot = false);
	static   bool    DrawGroup(GLenum,sgCGroup*,bool,
									bool asHot = false);
	static   bool    DrawContour(GLenum,sgCContour*,bool,
									bool asHot = false);
	static   bool    DrawBREP(GLenum,sgC3DObject*,bool,
		bool asHot = false);

private:
	static   bool                 ProjectPointOnMetaDC(sgCPoint*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectLineOnMetaDC(sgCLine*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectCircleOnMetaDC(sgCCircle*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectArcOnMetaDC(sgCArc*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectSplineOnMetaDC(sgCSpline*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectTextOnMetaDC(sgCText*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectDimensionsOnMetaDC(sgCDimensions*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectGroupOnMetaDC(sgCGroup*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectContourOnMetaDC(sgCContour*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);
	static   bool                 ProjectBREPOnMetaDC(sgC3DObject*,
											CDC *pDC,
											double *modelMatrix,
											double *projMatrix,
											int *viewport,
											int height,
											WMF_PROJECT_TYPE projType,
											double ratio,
											float RatioNbFace);

};

#endif