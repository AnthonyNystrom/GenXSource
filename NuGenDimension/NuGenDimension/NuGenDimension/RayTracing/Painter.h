#ifndef __PAINTER__
#define __PAINTER__

class Painter
{
public:
	static   bool         draw_triangles_regime;
	static   bool				  DrawObject(GLenum,sgCObject*,bool selSubObjects,
												bool asHot = false);
	static   const float*		  GetColorByIndex(unsigned short);
	static   const unsigned short GetLineTypeByIndex(unsigned short);

	static   void    DrawGabariteBox(const SG_POINT& pMin,const SG_POINT& pMax);

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
	static   bool    DrawGroup(GLenum,sgCGroup*,bool,
									bool asHot = false);
	static   bool    DrawContour(GLenum,sgCContour*,bool,
									bool asHot = false);
	static   bool    Draw3D(GLenum,sgC3DObject*,bool,
		bool asHot = false);
};

#endif