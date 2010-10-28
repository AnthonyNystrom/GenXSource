#ifndef _UNITCONVERSION_H_
#define _UNITCONVERSION_H_

class CUnitConversion {
public:
	static void Init( int resolution );
	static double PixelsToInches( int pixels );
	static double PixelsToCentimeters( int pixels );
	static int InchesToPixels( double inches );
	static int CentimeterToPixels( double centimeters );
	static int PointsToPixels( int points );

	static int s_resolution;

};

#endif //_UNITCONVERSION_H_