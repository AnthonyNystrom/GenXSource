/*!
@file FixedMath.h
@brief Fixed Point mathematic.
*/
#ifndef __FRAMEWORK_FIXED_MATH__
#define __FRAMEWORK_FIXED_MATH__

#include "BaseTypes.h"

#define	FX_PRECISION	16					//!< fixed point accuracy, bit
#define FX_ONE			(1 << FX_PRECISION) //!< 1.0
#define FX_HALF_ONE		FX_ONE >> 1			//!< 0.5
#define FX_ZERO			0					//!< 0
#define FX_PI			205887				//!< PI		
#define	FX_2PI			411774				//!< 2*PI
#define	FX_ONE_DIV_PI	20860				//!< 1/PI
#define	FX_ONE_DIV_2PI	10430				//!< 1/(2*PI)
#define FX_PI_DIV_180	1143				//!< PI/180


typedef int32 Fixed;

//! @brief Converts int32 to Fixed
inline Fixed Int2Fx(int32 value)
{
	return (value << FX_PRECISION);
}
//! @brief Converts Fixed to int32
inline int32 Fx2Int(Fixed value)
{
	return (value >> FX_PRECISION);
}
//! @brief Gets fractional part.
inline Fixed FxGetFractional(Fixed value)
{
	return (value & ((1 << FX_PRECISION) - 1));
}
//! @brief Round to the nearest integer.
//! @return rounded integer.
inline int32 FxRound(Fixed value)
{
	return (value + (FX_ONE >> 1)) >> FX_PRECISION;
}
//! @brief Round to the nearest integer.
//! @return rounded fixed point.
inline Fixed FxNearest(Fixed value) 
{
	return (value + ((FX_ONE >> 1) - 1)) & 0xffff0000;
}
//! @brief Returns a*b.
inline Fixed FxMul(Fixed a, Fixed b)
{
#ifndef __SYMBIAN32__
	return (Fixed)(((int64)a * (int64)b) >> FX_PRECISION);
#else
	return (Fixed)((a * b) >> FX_PRECISION);
#endif
}

//! @brief Returns 1/a
Fixed FxInverse(Fixed a);

//! @brief Returns 1/sqrt(a)
Fixed FxInverseSqrt(Fixed a);

//! @brief Returns sin(a)
Fixed FxSin(Fixed a);

//! @brief Returns cos(a)
Fixed FxCos(Fixed a);

//! @brief Returns x^y, where 0 < x < 1, y > 1
Fixed FxInvXpowY(Fixed x, Fixed y);

//! @brief Returns x^y, where x > 1, y > 1
inline Fixed FxXpowY(Fixed x, Fixed y)
{
	return FxInverse((FxInvXpowY(FxInverse(x), y)));
}
//! @brief Returns a/b
inline Fixed FxDiv(Fixed a, Fixed b)
{
	if ((b >> 24) && (b >> 24) + 1) {
		return FxMul(a >> 8, FxInverse(b >> 8));
	} else {
		return FxMul(a, FxInverse(b));
	}
}
//! @brief Returns sqrt(a)
inline Fixed FxSqrt(Fixed a) 
{
	return a <= 0 ? 0 : FxInverse(FxInverseSqrt(a));
}
//! @brief Returns sign of the value a.
inline Fixed FxSign(Fixed a)
{
	return a < 0 ? -FX_ONE : FX_ONE; 
}

#ifndef __SYMBIAN32__
//! @brief Returns sqrt(a), where a is 64-bit fixed point value.
int64 FxSqrt64(int64 a);
#endif

#endif // __FRAMEWORK_FIXED_MATH__