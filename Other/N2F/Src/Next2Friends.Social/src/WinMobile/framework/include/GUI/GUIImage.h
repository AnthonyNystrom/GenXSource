//// =================================================================
/*!	\file GUIImage.h

	Revision History:

	\par [9.8.2007]	13:37 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_IMAGE_H__
#define __FRAMEWORK_IMAGE_H__

#include "Graphics.h"
#include "GUIControl.h"

class Sprite;

//  ***************************************************
//! \brief Class represents an image control.
//!
//! Use this class to display a sprite or a surface with the required alignment.
//  ***************************************************
class GUIImage : public GUIControl
{
private:
	uint32	align;
	uint32	frame;
	Rect	frameRect;

	HANDLER_PROTOTYPE(OnImageChanged);
	HANDLER_PROTOTYPE(OnSizeChange);
	
	void	RecalcFrameRect();

protected:
	Sprite						*sprite;		//!< Image sprite.
	GraphicsSystem::Surface		*surface;		//!< Image surface.

public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	rect	- Control rect.
	//! \param[in]	align	- Image alignment.
	//  ***************************************************
	GUIImage(GUIControl * _parent, const Rect &rect, uint32 align = EAL_VCENTER + EAL_HCENTER);

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	rect	- Control rect.
	//! \param[in]	img		- Initial image sprite.
	//! \param[in]	align	- Image alignment.
	//  ***************************************************
	GUIImage(GUIControl * _parent, const Rect &rect, Sprite *img, uint32 align = EAL_VCENTER + EAL_HCENTER);

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	rect	- Control rect.
	//! \param[in]	img		- Initial image surface.
	//! \param[in]	align	- Image alignment.
	//  ***************************************************
	GUIImage(GUIControl * _parent, const Rect &rect, GraphicsSystem::Surface *img, uint32 align = EAL_VCENTER + EAL_HCENTER);

	virtual ~GUIImage();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Clone itself.
	//! \return		Its clone.
	//  ***************************************************
	virtual GUIControl * Clone();

	// ***************************************************
	//! \brief    	SetImage - set image to be displayed.
	//! It will clear (displace) the pointer to the previous image.
	//! \param      img - image to set.
	// ***************************************************
	virtual void SetImage(Sprite *img);

	// ***************************************************
	//! \brief    	SetImage - set surface to be displayed.
	//! It will clear (displace) the pointer to the previous surface.
	//! \param      img - surface to set.
	virtual void SetImage(GraphicsSystem::Surface *img);

	//  ***************************************************
	//! \brief    	Get image width.
	//! \return		Image width.
	//  ***************************************************
	uint16 GetImageWidth() const; 

	//  ***************************************************
	//! \brief    	Get image height.
	//! \return		Image height.
	//  ***************************************************
	uint16 GetImageHeight() const; 

	// ***************************************************
	//! \brief    	Set the alignment \ref eALignment.
	//! \param      newAlign - alignment \ref eALignment to set.
	// ***************************************************
	virtual void SetAlign(uint32 newAlign);

	//  ***************************************************
	//! \brief    	Get current image alignment.
	//! \return		Current image alignment.
	//  ***************************************************
	uint32 GetAlign() const;

	// ***************************************************
	//! \brief    	SetFrame - set a frame for drawing.
	//! \param      frm - Sprite frame number.
	// ***************************************************
	void SetFrame(int32 frm);

	//  ***************************************************
	//! \brief    	Get current frame number.
	//! \return		Current frame number.
	//  ***************************************************
	int32 GetFrame() const;

	//  ***************************************************
	//! \brief    	Get current image sprite.
	//! \return		Current image sprite.
	//  ***************************************************
	Sprite *GetSprite();

	//  ***************************************************
	//! \brief    	Get current image surface.
	//! \return		Current image surface.
	//  ***************************************************
	GraphicsSystem::Surface *GetSurface();

	//  ***************************************************
	//! \brief    	Get current image frame rect according to its alignment.
	//! \return		Current image frame rect according to its alignment.
	//  ***************************************************
	Rect GetFrameRect();
};

#endif // __FRAMEWORK_IMAGE_H__