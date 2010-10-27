#include "GUIPhotoImage.h"
#include "SurfacesPool.h"
#include "ApplicationManager.h"
#include "GUIImage.h"
#include "PhotoLibrary.h"
#include "EncoderObject.h"
#include "LibPhotoItem.h"


GUIPhotoImage::GUIPhotoImage( int16 waitImageID, int16 badImageID, SurfacesPool *surfPool, ApplicationManager *manager, GUIControlContainer *parent /*= NULL*/, const ControlRect &rc /*= ControlRect()*/ )
: GUIControlContainer(parent, rc)
{
	GUIControlContainer::SetStretch(ECS_STRETCH);

	badID = badImageID;
	waitID = waitImageID;
	image = new GUIImage(waitImageID, this);
	int32 w = image->GetSprite()->GetWidth() + (image->GetSprite()->GetWidth() >> 3);
	SetControlRect(ControlRect(0, 0, w, ControlRect::MIN_D, w, ControlRect::MAX_D));
	image->SetAnimationRange(0,image->GetFrameCount() - 1);
	image->StartAnimation();
	image->SetImageAlign(GUIControl::EA_VCENTRE | GUIControl::EA_HCENTRE);

	pool = surfPool;
	pManager = manager;
	oldSize = -1;
	needUnload = true;

}

GUIPhotoImage::~GUIPhotoImage( void )
{
	if (curFile)
	{
		curFile->Release();
	}
}

void GUIPhotoImage::SetPhotoItem( const LibPhotoItem *newPhotoItem )
{
	if (isEncoding)
	{
		return;
	}

	buffer = NULL;
	if (!newPhotoItem || item != newPhotoItem || oldSize != newPhotoItem->GetSize())
	{
		if (surf)
		{
			pool->ReturnSurface(surf);
			surf = NULL;
		}
		image->SetImage(waitID);
		image->SetAnimationRange(0,image->GetFrameCount() - 1);
		image->StartAnimation();

		item = newPhotoItem;
		if (item)
		{
			oldSize = item->GetSize();
		}
		else
		{
			oldSize = -1;
		}
	}





	isError = false;
}

void GUIPhotoImage::SetPhotoBuffer( uint8 *pBuffer, int32 len )
{
	if (isEncoding)
	{
		return;
	}

	{
		if (surf)
		{
			pool->ReturnSurface(surf);
			surf = NULL;
		}
		image->SetImage(waitID);
		image->SetAnimationRange(0,image->GetFrameCount() - 1);
		image->StartAnimation();

		buffer = pBuffer;
		length = len;
		item = NULL;
		oldSize = -1;
	}





	isError = false;
}


void GUIPhotoImage::Update()
{
	GUIControlContainer::Update();
	if (isError)
	{
		return;
	}
	if (surf && appearCounter < 16)
	{
		appearCounter += 3;
		Invalidate();
	}
		
	Rect r;
	GetScreenRect(r);
	int32 sh = GetApplication()->GetGraphicsSystem()->GetHeight();
	if (r.y + r.dy > 0 - (sh >> 2) && r.y < sh + (sh >> 2))
	{
		if (!surf && (item || buffer) && pManager->GetEncoder()->IsFree())
		{
			surf = pool->GetSurface();
			if (surf)
			{
				GraphicsSystem *gr = GetApplication()->GetGraphicsSystem();
				gr->SetCurrentSurface(surf);
				gr->SetColor(255, 0, 255);
				gr->FillRect(0, 0, 1000, 1000);
				gr->SetCurrentSurface(NULL);
				if (buffer)
				{
					pManager->GetEncoder()->SetListener(this);
					ImageInfo inf = *pManager->GetEncoder()->GetInfo((char8*)buffer, length);
					if (!inf.width || !inf.height)
					{
						isError = true;
						pManager->GetEncoder()->SetListener(NULL);
						buffer = NULL;
						if (surf)
						{
							pool->ReturnSurface(surf);
							surf = NULL;
						}
						return;
					}
					int32 w = inf.width;
					int32 h = inf.height;
					w = surf->width;
					h = inf.height * w / inf.width;
					if (h > surf->height)
					{
						h = surf->height;
						w = inf.width * h / inf.height;
					}
					inf.width = w;
					inf.height = h;

					isEncoding = true;
					pManager->GetEncoder()->GetSurface((char8*)buffer, length, surf, &inf);
				}
				else
				{
					curFile = pManager->GetPhotoLib()->GetPhotoFile(item);
					if (curFile)
					{
						pManager->GetEncoder()->SetListener(this);
						ImageInfo inf = *pManager->GetEncoder()->GetInfo(curFile);
						if (!inf.width || !inf.height)
						{
							isError = true;
							pManager->GetEncoder()->SetListener(NULL);
							if (curFile)
							{
								curFile->Release();
								curFile = NULL;
							}
							if (surf)
							{
								pool->ReturnSurface(surf);
								surf = NULL;
							}
							return;
						}
						int32 w = inf.width;
						int32 h = inf.height;
						w = surf->width;
						h = inf.height * w / inf.width;
						if (h > surf->height)
						{
							h = surf->height;
							w = inf.width * h / inf.height;
						}
						inf.width = w;
						inf.height = h;

						isEncoding = true;
						pManager->GetEncoder()->GetSurface(curFile, surf, &inf);
					}
					else
					{
						isError = true;
						if (surf)
						{
							pool->ReturnSurface(surf);
							surf = NULL;
						}
						image->SetImage(badID);
						image->SetAnimationRange(0, image->GetFrameCount() - 1);
						image->StartAnimation();
					}
				}
			}
		}
	}
	else
	{
		if (!needUnload)
		{
			return;
		}
		if (surf)
		{
			if (!isEncoding)
			{
				pool->ReturnSurface(surf);
				surf = NULL;
				image->SetImage(waitID);
				image->SetAnimationRange(0,image->GetFrameCount() - 1);
				image->StartAnimation();
			}
			else
			{
				pManager->GetEncoder()->Cancel();
			}
		}
	}
}

void GUIPhotoImage::OnEncodingSuccess(int32 size)
{
	image->StopAnimation();
	image->SetImage(surf);
	if (curFile)
	{
		curFile->Release();
		curFile = NULL;
	}
	isEncoding = false;
	appearCounter = 1;
	buffer = NULL;
}

void GUIPhotoImage::OnEncodingCanceled()
{
	if (curFile)
	{
		curFile->Release();
		curFile = NULL;
	}
	pool->ReturnSurface(surf);
	surf = NULL;
	image->SetImage(waitID);
	image->SetAnimationRange(0,image->GetFrameCount() - 1);
	image->StartAnimation();
	isEncoding = false;
	buffer = NULL;
}

void GUIPhotoImage::OnEncodingFailed()
{
	isError = true;
	isEncoding = false;
	pManager->GetEncoder()->SetListener(NULL);
	if (curFile)
	{
		curFile->Release();
		curFile = NULL;
	}
	if (surf)
	{
		pool->ReturnSurface(surf);
		surf = NULL;
	}
	image->SetImage(badID);
	//image->SetImage(badID);
	image->SetAnimationRange(0, image->GetFrameCount() - 1);
	//image->SetAnimationRange(5, 5);
	image->StartAnimation();
	buffer = NULL;
	return;
}

void GUIPhotoImage::Draw()
{
	if (surf && appearCounter < 15)
	{
		GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_ALPHA);
		GetApplication()->GetGraphicsSystem()->SetAlpha((uint8)appearCounter);
	}
}

void GUIPhotoImage::DrawFinished()
{
	if (surf && appearCounter < 15)
	{
		GetApplication()->GetGraphicsSystem()->SetBlendMode(GraphicsSystem::EBM_NONE);
	}
}

void GUIPhotoImage::CancelEncoding()
{
	if (isEncoding)
	{
		pManager->GetEncoder()->Cancel();
	}
}

void GUIPhotoImage::NeedUnload( bool isNeed )
{
	needUnload = isNeed;
}


