#ifndef __GUI_IMAGE_H_
#define __GUI_IMAGE_H_

#include "GUIControl.h"
#include "Graphics.h"

class Sprite;

class GUIImage : public GUIControl
{
public:

	GUIImage(Sprite	* _sprite = NULL, GUIControlContainer * _parent = NULL, const ControlRect &cr = ControlRect());
	GUIImage(uint16	resourceID, GUIControlContainer * _parent = NULL, const ControlRect &cr = ControlRect());

	virtual ~GUIImage();

	virtual void Draw();
	virtual void Update();

	void SetImage(Sprite *img);
	void SetImage(uint16 resourceID);
	void SetImage(GraphicsSystem::Surface *surface);

	uint16 GetImageWidth() const; 
	uint16 GetImageHeight() const;
	uint32 GetFrameCount() const;

	virtual void SetImageAlign(int32 _align);
	virtual void SetRect(const Rect &rc);

	void SetFrame(int32 frm);
	int32 GetFrame()			const	{	return frame;	}

	Sprite *GetSprite()			const	{	return sprite;	}

	void SetAnimationRange(int32 first, int32 last);
	void StartAnimation(){isAnimate = true;};
	void StopAnimation(){isAnimate = false;};

	//virtual void	SetMargins(int32 left, int32 right, int32 top, int32 bottom);


private:
	int32		imageAlign;
	uint32		frame;
	Sprite	*	sprite;		//!< Image sprite.
	
	int32		spriteX;
	int32		spriteY;

	int32		firstFrame;
	int32		lastFrame;
	bool		isAnimate;

	void SetDefaults();
	void UpdateXY();

	GraphicsSystem::Surface *surf;

	Rect		borderRect;
};

#endif // __GUI_IMAGE_H_