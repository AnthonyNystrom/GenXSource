//// =================================================================
/*!	\file GUIImageAnimated.h

	Revision History:

	\par [9.8.2007]	13:37 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_IMAGE_ANIMATED_H__
#define __FRAMEWORK_IMAGE_ANIMATED_H__

#include "GUIImage.h"

/*! \brief Class represents the animation.
	
	Use this class to display animated image.
*/
class GUIImageAnimated : public GUIImage
{
public:
	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	rect	- Control rect.
	//! \param[in]	align	- Animation alignment.
	//  ***************************************************
	GUIImageAnimated(GUIControl * _parent, const Rect &rect, bool alwaysPlay = false, uint32 align = EAL_VCENTER + EAL_HCENTER);

	//  ***************************************************
	//! \brief    	Constructor.
	//! \param[in]	_parent	- Parent control.
	//! \param[in]	rect	- Control rect.
	//! \param[in]	img		- Initial animation.
	//! \param[in]	align	- Animation alignment.
	//  ***************************************************
	GUIImageAnimated(GUIControl * _parent, const Rect &rect, Sprite *img, bool alwaysPlay = false, uint32 align = EAL_VCENTER + EAL_HCENTER);
	virtual ~GUIImageAnimated();

	//  ***************************************************
	//! \brief    	Set new image animation.
	//! \param[in]	img - New image animation.
	//  ***************************************************
	virtual void		SetImage( Sprite *img );

	//  ***************************************************
	//! \brief    	Return a copy (clone) of this control.
	//! \return		Control clone.
	//  ***************************************************
	virtual GUIControl*	Clone();

	//  ***************************************************
	//! \brief    	Test if this class is of the given type.
	//! \param[in]	classType	- Class type \ref eClassType. \see eClassType
	//! \return		Test results.
	//  ***************************************************	
	virtual bool		IsClass(uint32 classType) const;

	//  ***************************************************
	//! \brief    	Set animation range in frames.
	//! \param[in]	setStartFrame	- Initial frame.
	//! \param[in]	setFinishFrame	- Last frame.
	//  ***************************************************
	void				SetRange(uint16 setStartFrame, uint16 setFinishFrame);

	//  ***************************************************
	//! \brief    	Set "looping" mode for image animation.
	//! \param[in]	setLooped - Lopping flag.
	//  ***************************************************
	void				Looping(bool setLooped = true);

	//  ***************************************************
	//! \brief    	\todo Description
	//! \param[in]	setPeriod - \todo description
	//! \param[in]	setMsec	- \todo description
	//  ***************************************************
	void				SetPeriodicity(uint16 setPeriod, bool setMsec);

	//  ***************************************************
	//! \brief    	Start image animation playing.
	//  ***************************************************
	bool				Play();

	//  ***************************************************
	//! \brief    	Pause/Resume image animation, depending on its current state.
	//  ***************************************************
	void				PauseResume();

	//  ***************************************************
	//! \brief    	Stop image animation playing.
	//  ***************************************************
	void				Stop();

private:

	bool isPlaying; // not in pause
	bool isLooped;
	uint16 startFrame;
	uint16 finishFrame;

	uint16 period;
	uint32 timeSpent;
	bool isPeriodMsec;
	bool isAlwaysPlay;

	virtual void SetImage( GraphicsSystem::Surface *img );

	void NextFrame();


	HANDLER_PROTOTYPE(OnUpdate);
	HANDLER_PROTOTYPE(OnParentChange);
};

#endif // __FRAMEWORK_IMAGE_ANIMATED_H__