#include "GUIImage.h"
#include "GUISystem.h"
#include "Sprite.h"
#include "ResourceSystem.h"
#include "GUISkinLocal.h"

GUIImage::GUIImage(Sprite * _sprite/* = NULL*/, GUIControlContainer * _parent/* = NULL*/, const ControlRect &cr/* = ControlRect()*/)
	:	GUIControl		(_parent, cr)
{
	SetDefaults();

	SetImage(_sprite);
}

GUIImage::GUIImage(uint16 resourceID, GUIControlContainer * _parent /*= NULL*/, const ControlRect &cr /*= ControlRect()*/)
	:	GUIControl		(_parent, cr)
{
	SetDefaults();
	
	SetImage(resourceID);
}

GUIImage::~GUIImage()
{
	SAFE_RELEASE(sprite);
}

void GUIImage::SetImage( Sprite* img )
{
	if(sprite == img)
		return;

	surf = NULL;
	SAFE_RELEASE(sprite);

	sprite = img;

	if(sprite)
	{
		sprite->AddReference();
		SetFrame(0);
		UpdateXY();
	}
    Invalidate();
}

void GUIImage::SetImage(uint16 resourceID)
{
	SAFE_RELEASE(sprite);

	surf = NULL;
	sprite = GUISystem::Instance()->GetResourceSystem()->CreateSprite(resourceID);

	SetFrame(0);
	UpdateXY();
	Invalidate();
}

void GUIImage::SetImage( GraphicsSystem::Surface *surface )
{
	SAFE_RELEASE(sprite);
	surf = surface;
	UpdateXY();
	Invalidate();
}

uint16 GUIImage::GetImageWidth() const
{
	if (surf)
	{
		return surf->width;
	}
	return ((sprite) ? sprite->GetWidth() : 0);
}

uint16 GUIImage::GetImageHeight() const
{
	if (surf)
	{
		return surf->height;
	}
	return ((sprite) ? sprite->GetHeight() : 0);
}

void GUIImage::SetFrame( int32 frm )
{
	if (surf)
	{
		return;
	}

	if (frame != frm)
	{
		frame = frm;
		Rect rc;
		GetScreenRect(rc);
		GUISystem::Instance()->InvalidateRect(Rect(rc.x + spriteX, rc.y + spriteY, sprite->GetWidth(), sprite->GetHeight()), this);
	}
}

void GUIImage::SetImageAlign(int32 _align)
{
	imageAlign	=	_align;

	UpdateXY();
}

void GUIImage::Draw()
{
	Rect rc;
	GetScreenRect(rc);

	if (surf)
	{
		GetApplication()->GetGraphicsSystem()->DrawSurface(surf, rc.x + spriteX, rc.y + spriteY
			, true, GetApplication()->GetGraphicsSystem()->Rgb2Native(255, 0, 255), 0);
	}

	if(sprite)
	{
		sprite->Draw(rc.x + spriteX, rc.y + spriteY, frame);
	}

	//if(GUISystem::Instance()->GetFocus() == this)
	//{
	//	borderRect.x	=	rc.x + spriteX;
	//	borderRect.y	=	rc.y + spriteY;

	//	GUISystem::Instance()->GetSkin()->DrawControl(EDT_BORDER, borderRect);
	//}
	GUIControl::Draw();
}

void GUIImage::UpdateXY()
{
	if(!sprite && !surf)
	{
		return;
	}

	Rect	rc = GetRect();

	int32 marRight	= 0;
	int32 marLeft	= 0;
	int32 marBottom = 0;
	int32 marTop	= 0;
	//int32 marRight	= GetMarginRight();
	//int32 marLeft	= GetMarginLeft();
	//int32 marBottom = GetMarginBottom();
	//int32 marTop	= GetMarginTop();

	int32 spriteWidth	= GetImageWidth();
	int32 spriteHeight	= GetImageHeight();



	if(imageAlign & GUIControl::EA_RIGHT)
	{
		spriteX		=	rc.dx -	marRight - spriteWidth;
	}
	else if(imageAlign & GUIControl::EA_HCENTRE)
	{
		int32 width	=	(rc.dx - marRight - marLeft);
		spriteX		=	(width - spriteWidth)>>1;
		spriteX		+=	marLeft;
	}
	else
	{
		spriteX		=	marLeft;
	}

	if(imageAlign & GUIControl::EA_BOTTOM)
	{
		spriteY		=	rc.dy	-	marBottom - spriteHeight;	
	}
	else if(imageAlign & GUIControl::EA_VCENTRE)
	{
		int32 height=	(rc.dy - marTop - marBottom);
		spriteY		=	(height - spriteHeight)>>1;
		spriteY		+=	marTop;
	}
	else
	{
		spriteY		=	marTop;
	}

	borderRect.dx	=	spriteWidth;
	borderRect.dy	=	spriteHeight;
}

void GUIImage::SetDefaults()
{
	frame			=	0;
	sprite			=	NULL;
	spriteX			=	0;
	spriteY			=	0;

	SetDrawType(EDT_IMAGE);
	SetImageAlign(GUIControl::EA_HCENTRE | GUIControl::EA_VCENTRE);
}

void GUIImage::SetRect(const Rect &rc)
{
	GUIControl::SetRect(rc);

	UpdateXY();
}

void GUIImage::Update()
{
	GUIControl::Update();

	if (isAnimate && sprite)
	{
		int32 frm = GetFrame();
		frm++;
		if (frm > lastFrame)
		{
			frm = firstFrame;
		}
		SetFrame(frm);
	}

	//if (GUISystem::Instance()->GetFocus() == this)
	//{
	//	if (GetApplication()->IsKeyUp(Application::EKC_SELECT))
	//	{
	//		EventData ed;
	//		ed.lParam = (uint32)this;
	//		GUISystem::Instance()->SendEventToControl(this, EEI_IMAGE_PRESSED, &ed);
	//	}
	//}
}

void GUIImage::SetAnimationRange( int32 first, int32 last )
{
	firstFrame = first;
	lastFrame = last;
}

uint32 GUIImage::GetFrameCount() const
{
	if (!sprite)
	{
		return 0;
	}
	return sprite->GetFrameCount();
}
//void GUIImage::SetMargins(int32 left, int32 right, int32 top, int32 bottom)
//{
//	GUIControl::SetMargins(left, right, top, bottom);
//
//	UpdateXY();
//}



