/* ==========================================================================
	Class :			CUnitConversion

	Author :		Johan Rosengren, Abstrakt Mekanik AB

	Date :			2004-07-25

	Purpose :		

	Description :	

	Usage :			

   ========================================================================*/
#include "stdafx.h"
#include "UnitConversion.h"

void CUnitConversion::Init( int resolution )
/* ============================================================
	Function :		CUnitConversion::Init
	Description :	Initializes the conversion with the
					resolution to use.
	Access :		Public

	Return :		void
	Parameters :	int resolution	-	Resolution to use
					
	Usage :			Call before calling any other conversion 
					functions.

   ============================================================*/
{

	CUnitConversion::s_resolution = resolution;

}

double CUnitConversion::PixelsToInches( int pixels )
/* ============================================================
	Function :		CUnitConversion::PixelsToInches
	Description :	Converts from pixels to inches.
	Access :		Public

	Return :		double		-	Resulting inches
	Parameters :	int pixels	-	Pixels to convert
					
	Usage :			Call to convert from pixels to inches.

   ============================================================*/
{

	return static_cast< double >( pixels ) / static_cast< double >( CUnitConversion::s_resolution );

}

double CUnitConversion::PixelsToCentimeters( int pixels )
/* ============================================================
	Function :		CUnitConversion::PixelsToCentimeters
	Description :	Converts from pixels to centimeters.
	Access :		Public

	Return :		double		-	Resulting centimeters
	Parameters :	int pixels	-	Pixels to convert
					
	Usage :			Call to convert from pixels to centimeters

   ============================================================*/
{

	double pixelspercentimeter = static_cast< double >( CUnitConversion::s_resolution ) / 2.54;
	return pixelspercentimeter / static_cast< double >( pixels );

}

int CUnitConversion::InchesToPixels( double inches )
/* ============================================================
	Function :		CUnitConversion::InchesToPixels
	Description :	Converts from inches to pixels
	Access :		Public

	Return :		int				-	Resulting pixels
	Parameters :	double inches	-	Inches to convert
					
	Usage :			Call to convert from inches to pixels.

   ============================================================*/
{

	return static_cast< int >( static_cast< double >( CUnitConversion::s_resolution ) * inches + .5 );

}

int CUnitConversion::CentimeterToPixels( double centimeters )
/* ============================================================
	Function :		CUnitConversion::CentimeterToPixels
	Description :	Converts from centimeters to pixels.
	Access :		Public

	Return :		int					-	Resulting pixels
	Parameters :	double centimeters	-	Centimeters to 
											convert
					
	Usage :			Call to convert from centimeters to pixels

   ============================================================*/
{

	double pixelspercentimeter = static_cast< double >( CUnitConversion::s_resolution ) / 2.54;
	return static_cast< int >( pixelspercentimeter * centimeters + .5 );

}

int CUnitConversion::PointsToPixels( int points )
/* ============================================================
	Function :		CUnitConversion::PointsToPixels
	Description :	Converts from 10ths of points to pixels
	Access :		

	Return :		int			-	Resulting amount of pixels
	Parameters :	int points	-	Points to convert
					
	Usage :			Call to converts from 10ths of points to 
					pixels.

   ============================================================*/
{

	double point = static_cast< double >( CUnitConversion::s_resolution ) / 72.0;
	return static_cast< int >( ( points / 10.0 ) * point + .5 );

}

int CUnitConversion::s_resolution = 0;
