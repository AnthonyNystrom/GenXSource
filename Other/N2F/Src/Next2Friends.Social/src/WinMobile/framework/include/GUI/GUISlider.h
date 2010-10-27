//// =================================================================
/*!	\file GUISlider.h

	Revision History:

	\par [9.8.2007]	13:39 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_SLIDER__
#define __GUI_SLIDER__

#include "GUINumeric.h"
#include "GUIButtonProportional.h"
#include "GUIButtonText.h"

/*! \brief Slider bar control.

Example:

\code
// Create a window
window = guiSystem->CreateWindow(Rect(0, 0, graphics->GetWidth(), graphics->GetHeight()));

// Create a slider bar
// Specify the orientation, it influences the direction in which
// the slider bar will move and to which keys it will react.
slider = new GUISlider(window, Rect(10, 10, 150, 30), EGO_LEFTTORIGHT);

// Specify the minimal limit for the control value
slider->SetMin(0);
// Specify the maximal limit for the control value
slider->SetMax(20);
// Set the value itself
slider->SetValue(10);

// The control value can be scaled to any other limits
int32 v = slider->ScaleToValue(0, 1000);

// Create a progress bar
progressBar = new GUIProgressBar(window, Rect(10, 60, 150, 30), EGO_LEFTTORIGHT);

// Specify the minimal limit for the control value
progressBar->SetMin(0);
// Specify the maximal limit for the control value
progressBar->SetMax(100);
// The control value can be set by scaling the number to the control limits
progressBar->ValueToScale(700, 0, 1000);
// It is possible to set the control; value output format
// The text will be drawn inside of the control
progressBar->SetPrintFormat((char16*)L"%d percent");

// One can get the text in the following way
const char16 * text = progressBar->ValueToString();

// Set focus to the slider
guiSystem->SetFocus(slider);

\endcode
*/
class GUISlider: public GUINumeric
{
public:

	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent			- Parent control.
	//! \param[in]	_rect			- Control rect.
	//! \param[in]	_orientation	- Control orientation.
	//  ***************************************************	
	GUISlider(GUIControl * _parent, const Rect & _rect, eGUIOrientation _orientation);
	virtual ~GUISlider();

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


private:
	HANDLER_PROTOTYPE(OnSkinChange);
	HANDLER_PROTOTYPE(OnIncrease);
	HANDLER_PROTOTYPE(OnDecrease);
	HANDLER_PROTOTYPE(OnUpdate);

	void			Recalculate();

	GUIButtonText * button;
	eGUIOrientation orientation;
	uint32			sliderSize;
};

#endif // __GUI_SLIDER__
