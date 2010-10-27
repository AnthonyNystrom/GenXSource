#ifndef __SURFACES_POOL__
#define __SURFACES_POOL__

#include "BaseTypes.h"
#include "Graphics.h"

struct SurfaceNodeData 
{
	GraphicsSystem::Surface *surface;
	bool isFree;
};

class SurfacesPool
{
public:
	SurfacesPool(int32 poolSize, uint16 surfWidth, uint16 surfHeight);
	~SurfacesPool();

	GraphicsSystem::Surface *GetSurface();
	void ReturnSurface(GraphicsSystem::Surface *surf);

protected:
	int32 size;

	SurfaceNodeData *pSurfArray;

};


#endif//__GUI_BUTTON__