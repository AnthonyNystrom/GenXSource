#include "SurfacesPool.h"

SurfacesPool::SurfacesPool( int32 poolSize, uint16 surfWidth, uint16 surfHeight )
{
	size = poolSize;
	pSurfArray = new SurfaceNodeData[size];
	for(int i = 0; i < size; i++)
	{
		pSurfArray[i].surface = GetApplication()->GetGraphicsSystem()->CreateNativeSurface(surfWidth, surfHeight);
		pSurfArray[i].isFree = true;
	}
}

SurfacesPool::~SurfacesPool()
{
	for(int i = 0; i < size; i++)
	{
		GetApplication()->GetGraphicsSystem()->ReleaseNativeSurface(pSurfArray[i].surface);
	}
	SAFE_DELETE(pSurfArray);
}

GraphicsSystem::Surface * SurfacesPool::GetSurface()
{
	for(int i = 0; i < size; i++)
	{
		if (pSurfArray[i].isFree == true)
		{
			pSurfArray[i].isFree = false;
			return pSurfArray[i].surface;
		}
	}

	return NULL;
}

void SurfacesPool::ReturnSurface( GraphicsSystem::Surface *surf )
{
	for(int i = 0; i < size; i++)
	{
		if (pSurfArray[i].surface == surf)
		{
			FASSERT(!pSurfArray[i].isFree);
			pSurfArray[i].isFree = true;
			return;
		}
	}
	FASSERT(false);
}

