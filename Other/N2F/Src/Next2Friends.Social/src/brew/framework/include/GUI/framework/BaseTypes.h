/*!
@file	BaseTypes.h
@brief	Base definitions
*/

#include "Config.h"
#include "Utils.h"

#ifndef __FRAMEWORK_BASETYPES_H__
#define __FRAMEWORK_BASETYPES_H__

#include "Config.h"

#undef	NULL
#define	NULL	0

// Framework data types definition
typedef signed char			int8;
typedef signed short		int16;
typedef signed long			int32;
typedef signed long long	int64;

typedef unsigned char		uint8;
typedef unsigned short		uint16;
typedef unsigned long		uint32;
typedef unsigned long long	uint64;

typedef char				char8;
typedef short				char16;

#ifdef TEST_TYPES
#	include "DefWrongTypes.h"
#endif // TEST_TYPES

#define bool int32
#define true 1
#define false 0

#ifdef __SYMBIAN32__
#undef new
void* operator new(unsigned int size, char8 temp) throw();
void* operator new[](unsigned int size, char8 temp) throw();
#define new new((char8)0)
#endif


#define CLAMP(value, minim, maxim) \
	( ((value) < (minim)) ? ((minim)): ( ((value) > (maxim)) ? ((maxim)) :((value))  ) ) //!< CLAMP macros

#ifndef ABS
#define ABS(a) (((a) < 0) ? (-(a)) : ((a))) //!< ABS macros
#endif // ABS

#ifndef MAX
#define MAX(a, b) ((a) > (b) ? (a) : (b))	//!< MAX macros
#endif // MAX

#ifndef MIN
#define MIN(a, b) ((a) < (b) ? (a) : (b))	//!< MIN macros
#endif // MIN

#ifndef SIGN
#define SIGN(a)((a) < (0) ? (-1) : (1))		//!< SIGN macros
#endif // SING

//! Point structure
struct Point
{
	//! @brief Constructor
	Point(int32 _x = 0, int32 _y = 0): x(_x), y(_y){};
	int32 x;//!< Point X coordinate
	int32 y;//!< Point Y coordinate
};

//! Rectangle structure
struct Rect
{
	//! @brief Constructor
	Rect(int32 _x = 0, int32 _y = 0, int32 _dx = 0, int32 _dy = 0): x(_x), y(_y),dx(_dx),dy(_dy){};
	int32 x, y;//!< Left/top point
	int32 dx, dy;//!< Width/height

	//! @brief Checks if rectangle has area > 0
	//! @return true if area > 0, false if area == 0
	bool IsEmpty() const
	{
		return (!dx || !dy);
	}

	//! @brief Calculates two rectangles intersection: this and _rect
	//!
	//! Resulting Rect is storing in "this"
	//! @param[in] _rect - 2nd operand in intersection operation
	void Intersect(const Rect & _rect)
	{
		int32 xx = x;
		int32 yy = y;
		x = MAX(x, _rect.x);
		y = MAX(y, _rect.y);
		dx = MIN((dx + xx) - x, (_rect.dx + _rect.x) - x);
		dy = MIN((dy + yy) - y, (_rect.dy + _rect.y) - y);
		if (dx <= 0 || dy <= 0)
		{
			x = 0;
			y = 0;
			dx = 0;
			dy = 0;
		}
	}

	//! @brief Calculates two rectangles union: this and _rect
	//!
	//! Resulting Rect is storing in "this"
	//! @param[in] _rect - 2nd operand in union operation
	void Union(const Rect & _rect)
	{
		if (_rect.dx && _rect.dy)
		{
			if (dx && dy)
			{
				int32 xx = x;
				int32 yy = y;
				x = MIN(x, _rect.x);
				y = MIN(y, _rect.y);
				dx = MAX((dx + xx) - x, (_rect.dx + _rect.x) - x);
				dy = MAX((dy + yy) - y, (_rect.dy + _rect.y) - y);
			}
			else
			{
				x = _rect.x;
				y = _rect.y;
				dx = _rect.dx;
				dy = _rect.dy;
			}
		}
	}
	//  ***************************************************
	//! \brief    	Checks if point is inside rect
	//! 
	//! \param[in]	p - point to check
	//! \return		true if point is inside, false otherwise
	//  ***************************************************
	bool IsPointIn(Point &p) const
	{
		return ((p.x >= x)&&(p.y >= y)&&(p.x <= (x + dx))&&(p.y <= (y + dy)));
	}

	//! @brief Operator==
	bool operator==(const Rect & rect) const
	{
		return (x == rect.x)&&(y == rect.y)&&(dx == rect.dx)&&(dy == rect.dy);
	}
};

#endif //__FRAMEWORK_BASETYPES_H__