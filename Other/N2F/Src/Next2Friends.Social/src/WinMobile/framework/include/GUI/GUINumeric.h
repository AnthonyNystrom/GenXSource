//// =================================================================
/*!	\file GUINumeric.h

	Revision History:

	\par [9.8.2007]	13:38 by Anton Lobanov
	File created.
*/// ==================================================================

#ifndef __GUI_NUMERIC__
#define __GUI_NUMERIC__

#include "Vector.h"
#include "GUIControl.h"

/*! \brief Numeric control.

It is a basic control for all the derivative controls that display a state with a numbers.
*/
class GUINumeric: public GUIControl
{
public:
	//  ***************************************************
	//! \brief    	Constructor
	//! \param[in]	_parent			- Parent control.
	//! \param[in]	_rect			- Control rect.
	//  ***************************************************	
	GUINumeric(GUIControl * _parent, const Rect & _rect);
	virtual ~GUINumeric();

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

	//  ***************************************************
	//! \brief    	Set a control value
	//! \param[in]	_value	- value
	//  ***************************************************	
	void			SetValue(int32 _value);

	//  ***************************************************
	//! \brief    	Get the control value
	//! \return		Control value
	//  ***************************************************	
	int32			GetValue() { return value; }

	//  ***************************************************
	//! \brief    	Set the maximal value for the control
	//! \param[in]	_max	- maximal value
	//  ***************************************************	
	void			SetMax(int32 _max);

	//  ***************************************************
	//! \brief    	Get the maximal value for the control
	//! \return		Maximal value.
	//  ***************************************************	
	int32			GetMax();

	//  ***************************************************
	//! \brief    	Set the minimal value for the control
	//! \param[in]	_min	- minimal value
	//  ***************************************************	
	void			SetMin(int32 _min);

	//  ***************************************************
	//! \brief    	Get the minimal value for the control
	//! \return		Minimal value.
	//  ***************************************************	
	int32			GetMin();

	//  ***************************************************
	//! \brief    	Increase the control value by the specified step
	//! \param[in]	_step	- step
	//  ***************************************************	
	void			Inc(int32 _step = 1);

	//  ***************************************************
	//! \brief    	Decrease the control value by the specified step
	//! \param[in]	_step	- step
	//  ***************************************************	
	void			Dec(int32 _step = 1);

	//  ***************************************************
	//! \brief    	Scale the control value to the set limits
	//! \param[in]	_scaleMin	- new minimal limit
	//! \param[in]	_scaleMax	- new maximal limit
	//! \return		new scaled value
	//  ***************************************************	
	int32			ScaleToValue(int32 _scaleMin, int32 _scaleMax);

	//  ***************************************************
	//! \brief    	Scale the given number to the control limits and specify the control value
	//! \param[in]	_scale		- given number
	//! \param[in]	_scaleMin	- given minimal limit
	//! \param[in]	_scaleMax	- given maximal limit
	//  ***************************************************	
	void			ValueToScale(int32 _scale, int32 _scaleMin, int32 _scaleMax);

	//  ***************************************************
	//! \brief    	Set the ValueToString function output format
	//! \param[in]	_printFormat - a string in printf() style but with some restrictions !!! \n
	//!				restriction 1 - can be only %%d \n (может быть только %%d \n)
	//!				restriction 2 - %%d can be only once in the string
	//  ***************************************************	
	void			SetPrintFormat(const char16 * _printFormat);

	//  ***************************************************
	//! \brief    	Get the ValueToString function output format.
	//! \return		A string in printf() style but.
	//  ***************************************************	
	const char16 *	GetPrintFormat();

	//  ***************************************************
	//! \brief    	Get the control value in accordance with the string input format 
	//!				default output format is "%d"
	//! \return		The control value in accordance with the string input format.
	//  ***************************************************	
	const char16 *	ValueToString();

	//  ***************************************************
	//! \brief    	Set the control value in accordance with the string input format 
	//!				default output format is "%d"
	//! \param[in]	_text - New value string.
	//  ***************************************************	
	void StringToValue(const char16 * _text);

private:

	int32 value;
	int32 max;
	int32 min;

	Vector<char16> printString;
	Vector<char16> printFormat;
};


#endif // __GUI_NUMERIC__