#include "stdafx.h"
#include "Painter.h"

#include "RTMaterials.h"
#include "RTImage.h"

static float SYSTEM_COLORS[1024] =
//              0               1               2               3
{
	0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f, 128.0f/255.0f,0.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f, 128.0f/255.0f,0.0f/255.0f,
		128.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f,0.0f/255.0f, 128.0f/255.0f,128.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f, 192.0f/255.0f,192.0f/255.0f,192.0f/255.0f,0.0f/255.0f,
		128.0f/255.0f,128.0f/255.0f,128.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f,0.0f/255.0f, 0.0f/255.0f,  255.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,
		255.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,
		// 1/18
		255.0f/255.0f,240.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,226.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,212.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,198.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f,184.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,170.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,170.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,146.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f,122.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f, 98.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f, 74.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f, 50.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// 2/18
		255.0f/255.0f,227.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,199.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,143.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f,115.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 87.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 85.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f, 73.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f, 61.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f, 49.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f, 37.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f, 25.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// 3/18
		255.0f/255.0f,212.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f, 72.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// 4/18
		255.0f/255.0f,212.0f/255.0f,227.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,199.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,143.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f, 72.0f/255.0f,115.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f, 87.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f, 85.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f, 73.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f,  0.0f/255.0f, 61.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f, 49.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f, 25.0f/255.0f,0.0f/255.0f,	//  8
		// 5/18
		255.0f/255.0f,212.0f/255.0f,240.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,226.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,198.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f, 72.0f/255.0f,184.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f,170.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f,170.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f,146.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f,  0.0f/255.0f,122.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f, 98.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f, 74.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f, 50.0f/255.0f,0.0f/255.0f,	//  8
		// 6/18
		255.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  8
		// 7/18
		240.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 226.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 212.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 198.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		184.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 170.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 170.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 146.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		122.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f,  98.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f,  74.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  50.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  8
		// 8/18
		227.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 199.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 171.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 143.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		115.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  87.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  85.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  73.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		61.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f,  49.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  25.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  8
		// 9/18
		212.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  37.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  0
		// 10/18
		212.0f/255.0f,227.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,199.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,171.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,143.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f,115.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  34.0f/255.0f, 87.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 85.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 73.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f, 61.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 49.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 37.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 25.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  8
		// 11/18
		212.0f/255.0f,240.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,226.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,198.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f,184.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,170.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,170.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,146.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f,122.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 98.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 74.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 50.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  8
		// 12/18
		212.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f,220.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f,185.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,	//  8
		// 13/18
		212.0f/255.0f,255.0f/255.0f,240.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,226.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,198.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f,255.0f/255.0f,184.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f,170.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f,170.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f,146.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f,185.0f/255.0f,122.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f, 98.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f, 74.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f, 50.0f/255.0f,0.0f/255.0f,	//  8
		// 14/18
		212.0f/255.0f,255.0f/255.0f,227.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,199.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,143.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f,255.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f, 87.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f, 85.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f, 73.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f,185.0f/255.0f, 61.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f, 49.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f, 25.0f/255.0f,0.0f/255.0f,	//  8
		// 15/18
		212.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		72.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		0.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// 15/18
		227.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 199.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 143.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		115.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f,  87.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,  85.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  73.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		51.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  49.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  25.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// 17/18
		240.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 226.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 212.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 198.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		184.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 170.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 170.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 146.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		122.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  98.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  74.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  50.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// 18/18
		255.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f,	//  0
		255.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  4
		185.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,	//  8
		// gray
		238.0f/255.0f,238.0f/255.0f,238.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,220.0f/255.0f,220.0f/255.0f,0.0f/255.0f, 202.0f/255.0f,202.0f/255.0f,202.0f/255.0f,0.0f/255.0f, 184.0f/255.0f,184.0f/255.0f,184.0f/255.0f,0.0f/255.0f,	//  0
		166.0f/255.0f,166.0f/255.0f,166.0f/255.0f,0.0f/255.0f, 148.0f/255.0f,148.0f/255.0f,148.0f/255.0f,0.0f/255.0f, 130.0f/255.0f,130.0f/255.0f,130.0f/255.0f,0.0f/255.0f, 112.0f/255.0f,112.0f/255.0f,112.0f/255.0f,0.0f/255.0f,	//  4
};

bool Painter::draw_triangles_regime = false;

bool  Painter::DrawObject(GLenum mode,sgCObject* obj,bool selSubObjects,bool asHot)
{
	if ((obj->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_HIDE))
			return false;

	switch(obj->GetType()) 
	{
	case SG_OT_POINT:
		return DrawPoint(mode,reinterpret_cast<sgCPoint*>(obj),selSubObjects,asHot);
	case SG_OT_LINE:
		return DrawLine(mode,reinterpret_cast<sgCLine*>(obj),selSubObjects,asHot);
	case SG_OT_CIRCLE:
		return DrawCircle(mode,reinterpret_cast<sgCCircle*>(obj),selSubObjects,asHot);
	case SG_OT_ARC:
		return DrawArc(mode,reinterpret_cast<sgCArc*>(obj),selSubObjects,asHot);
	case SG_OT_SPLINE:
		return DrawSpline(mode, reinterpret_cast<sgCSpline*>(obj),selSubObjects,asHot);
	case SG_OT_GROUP:
		return DrawGroup(mode, reinterpret_cast<sgCGroup*>(obj),selSubObjects,asHot);
	case SG_OT_CONTOUR:
		return DrawContour(mode, reinterpret_cast<sgCContour*>(obj),selSubObjects,asHot);
	case SG_OT_3D:
		return Draw3D(mode, reinterpret_cast<sgC3DObject*>(obj),selSubObjects, asHot);
	default:
		return false;
	}
	return true;
}

const float*   Painter::GetColorByIndex(unsigned short ind)
{
	return SYSTEM_COLORS+4*(ind%239);
}

const unsigned short Painter::GetLineTypeByIndex(unsigned short ind)
{
	switch (ind)
	{
	case 0:
		return BIN16(11111111,11111111);
	case 1:
		return BIN16(11111100,11111100);
	case 2:
		return BIN16(00000111,11111100);
	case 3:
		return BIN16(00011111,11111111);
	case 4:
		return BIN16(11111111,11001100);
	case 5:
		return BIN16(11110000,11110000);
	case 6:
		return BIN16(11000000,11000000);
	case 7:
		return BIN16(01010101,01010101);
	default:
		return BIN16(11111111,11111111);
	}
}

void  Painter::DrawGabariteBox(const SG_POINT& pMin,const SG_POINT& pMax)
{
	glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

	glDisable(GL_LIGHTING);
	glDisable(GL_TEXTURE_2D);
	glLineWidth(1);

	glColor3f(1.0f, 1.0f, 1.0f);

	glEnable(GL_LINE_SMOOTH);
	glBegin(GL_LINE_STRIP);
		glVertex3d(pMin.x,pMin.y,pMin.z);
		glVertex3d(pMin.x,pMin.y,pMax.z);
		glVertex3d(pMin.x,pMax.y,pMax.z);
		glVertex3d(pMin.x,pMax.y,pMin.z);
		glVertex3d(pMin.x,pMin.y,pMin.z);
		glVertex3d(pMax.x,pMin.y,pMin.z);
		glVertex3d(pMax.x,pMin.y,pMax.z);
		glVertex3d(pMax.x,pMax.y,pMax.z);
		glVertex3d(pMax.x,pMax.y,pMin.z);
		glVertex3d(pMax.x,pMin.y,pMin.z);
	glEnd();
	glBegin(GL_LINES);
		glVertex3d(pMin.x,pMin.y,pMax.z);
		glVertex3d(pMax.x,pMin.y,pMax.z);
		glVertex3d(pMin.x,pMax.y,pMax.z);
		glVertex3d(pMax.x,pMax.y,pMax.z);
		glVertex3d(pMin.x,pMax.y,pMin.z);
		glVertex3d(pMax.x,pMax.y,pMin.z);
	glEnd();


	glPopAttrib();
}

bool  Painter::DrawPoint(GLenum mode,sgCPoint* objP,bool selSubObjects,bool asHot)
{
	const SG_POINT*  pnt = objP->GetGeometry();
	double pnts[3];
	pnts[0]  = pnt->x;
	pnts[1]  = pnt->y;
	pnts[2]  = pnt->z;

	if (mode == GL_RENDER)
	{
			glPushAttrib(GL_ENABLE_BIT|GL_POINT_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

			glDisable(GL_LIGHTING);
			glDisable(GL_TEXTURE_2D);
			glPointSize(static_cast<float>(objP->GetAttribute(SG_OA_LINE_THICKNESS)+2));
			if (asHot)
				glColor3f(1.0f, 0.0f, 0.0f);
			else
				if (objP->IsSelect())
					glColor3f(1.0f, 0.0f, 0.0f);
				else
					glColor3fv(GetColorByIndex(objP->GetAttribute(SG_OA_COLOR)));
			glEnable(GL_POINT_SMOOTH);
			glPushMatrix();
			if (objP->GetTempMatrix()!=0)
				glMultMatrixd(objP->GetTempMatrix()->GetTransparentData());
			glBegin(GL_POINTS);
			glVertex3dv(pnts);
			glEnd();

			glPopMatrix();
			glPopAttrib();
	}

	return true;
}

bool  Painter::DrawLine(GLenum mode,sgCLine* objL,bool selSubObjects,bool asHot)
{
	const SG_LINE*  ln = objL->GetGeometry();
	double pnts[6];
	pnts[0]  = ln->p1.x;
	pnts[1]  = ln->p1.y;
	pnts[2]  = ln->p1.z;
	pnts[3]  = ln->p2.x;
	pnts[4]  = ln->p2.y;
	pnts[5]  = ln->p2.z;

	if (mode == GL_RENDER)
	{
				glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

				glDisable(GL_LIGHTING);
				glDisable(GL_TEXTURE_2D);
				glLineWidth(static_cast<float>(objL->GetAttribute(SG_OA_LINE_THICKNESS)+1));
				
				GLushort pattern = GetLineTypeByIndex(objL->GetAttribute(SG_OA_LINE_TYPE));
				if (pattern>0)
				{
					glEnable(GL_LINE_STIPPLE);
					glLineStipple(1, pattern);
				}
				
				if (asHot)
					glColor3f(1.0f, 0.0f, 0.0f);
				else
					if (objL->IsSelect())
						glColor3f(1.0f, 0.0f, 0.0f);
					else
						glColor3fv(GetColorByIndex(objL->GetAttribute(SG_OA_COLOR)));
				glEnable(GL_LINE_SMOOTH);
				glPushMatrix();
				if (objL->GetTempMatrix()!=0)
					glMultMatrixd(objL->GetTempMatrix()->GetTransparentData());
				glBegin(GL_LINES);
					glVertex3dv(pnts);
					glVertex3dv(pnts+3);
				glEnd();

				glPopMatrix();
				glPopAttrib();
	}

	return true;
}

bool   Painter::DrawCircle(GLenum mode,sgCCircle* objC,bool selSubObjects,bool asHot)
{
	const int  pnts_cnt = objC->GetPointsCount();
	const SG_POINT*  pnts = objC->GetPoints();
	

	if (mode == GL_RENDER)
	{
				glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

				glDisable(GL_LIGHTING);
				glDisable(GL_TEXTURE_2D);

				glLineWidth(static_cast<float>(objC->GetAttribute(SG_OA_LINE_THICKNESS)+1));

				GLushort pattern = GetLineTypeByIndex(objC->GetAttribute(SG_OA_LINE_TYPE));
				if (pattern>0)
				{
					glEnable(GL_LINE_STIPPLE);
					glLineStipple(1, pattern);
				}

				if (asHot)
					glColor3f(1.0f, 0.0f, 0.0f);
				else
					if (objC->IsSelect())
						glColor3f(1.0f, 0.0f, 0.0f);
					else
						glColor3fv(GetColorByIndex(objC->GetAttribute(SG_OA_COLOR)));
				glEnable(GL_LINE_SMOOTH);
				glPushMatrix();
				if (objC->GetTempMatrix()!=0)
					glMultMatrixd(objC->GetTempMatrix()->GetTransparentData());
				glBegin(GL_LINE_LOOP);
					for (int i=0;i<pnts_cnt;i++)
						glVertex3dv(&pnts[i].x);
				glEnd();

				glPopMatrix();
				glPopAttrib();
	}

		
	return true;
}

bool   Painter::DrawArc(GLenum mode,sgCArc* objA,bool selSubObjects,bool asHot)
{
	const int  pnts_cnt = objA->GetPointsCount();
	const SG_POINT*  pnts = objA->GetPoints();
	
	if (mode == GL_RENDER)
	{
		glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

		glDisable(GL_LIGHTING);
		glDisable(GL_TEXTURE_2D);

		glLineWidth(static_cast<float>(objA->GetAttribute(SG_OA_LINE_THICKNESS)+1));

		GLushort pattern = GetLineTypeByIndex(objA->GetAttribute(SG_OA_LINE_TYPE));
		if (pattern>0)
		{
			glEnable(GL_LINE_STIPPLE);
			glLineStipple(1, pattern);
		}

		if (asHot)
			glColor3f(1.0f, 0.0f, 0.0f);
		else
			if (objA->IsSelect())
				glColor3f(1.0f, 0.0f, 0.0f);
			else
				glColor3fv(GetColorByIndex(objA->GetAttribute(SG_OA_COLOR)));
		glEnable(GL_LINE_SMOOTH);
		glPushMatrix();
		if (objA->GetTempMatrix()!=0)
			glMultMatrixd(objA->GetTempMatrix()->GetTransparentData());
		glBegin(GL_LINE_STRIP);
		for (int i=0;i<pnts_cnt;i++)
			glVertex3dv(&pnts[i].x);
		glEnd();

		glPopMatrix();
		glPopAttrib();
	}


	return true;
}

bool  Painter::DrawSpline(GLenum mode,sgCSpline* splObj,bool selSubObjects, bool asHot)
{
	const unsigned int  pnts_cnt = splObj->GetGeometry()->GetPointsCount();
	const SG_POINT*  pnts = splObj->GetGeometry()->GetPoints();
	
	if (mode == GL_RENDER)
	{
		glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

		glDisable(GL_LIGHTING);
		glDisable(GL_TEXTURE_2D);

		glLineWidth(static_cast<float>(splObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));

		GLushort pattern = GetLineTypeByIndex(splObj->GetAttribute(SG_OA_LINE_TYPE));
		if (pattern>0)
		{
			glEnable(GL_LINE_STIPPLE);
			glLineStipple(1, pattern);
		}

		if (asHot)
			glColor3f(1.0f, 0.0f, 0.0f);
		else
			if (splObj->IsSelect())
				glColor3f(1.0f, 0.0f, 0.0f);
			else
				glColor3fv(GetColorByIndex(splObj->GetAttribute(SG_OA_COLOR)));
		glEnable(GL_LINE_SMOOTH);
		glPushMatrix();
		if (splObj->GetTempMatrix()!=0)
			glMultMatrixd(splObj->GetTempMatrix()->GetTransparentData());
		glBegin(GL_LINE_STRIP);
		for (unsigned int i=0;i<pnts_cnt;i++)
			glVertex3dv(&pnts[i].x);
		glEnd();

		glPopMatrix();
		glPopAttrib();
	}


	return true;
}

bool   Painter::DrawGroup(GLenum mode, sgCGroup* objGr,bool selSubObjects,bool asHot)
{
	glPushMatrix();
	if (objGr->GetTempMatrix()!=0)
		glMultMatrixd(objGr->GetTempMatrix()->GetTransparentData());

	sgCObject*  curObj = objGr->GetChildrenList()->GetHead();
	while (curObj) 
	{
		if (!DrawObject(mode,curObj,selSubObjects,asHot))
			return false;
		curObj = objGr->GetChildrenList()->GetNext(curObj);
	}
	glPopMatrix();
	return true;
}

bool   Painter::DrawContour(GLenum mode, sgCContour* objCnt,bool selSubObjects,bool asHot)
{
	glPushMatrix();
	if (objCnt->GetTempMatrix()!=0)
		glMultMatrixd(objCnt->GetTempMatrix()->GetTransparentData());

	sgCObject*  curObj = objCnt->GetChildrenList()->GetHead();
	while (curObj) 
	{
		if (!DrawObject(mode,curObj,selSubObjects,asHot))
			return false;
		curObj = objCnt->GetChildrenList()->GetNext(curObj);
	}
	glPopMatrix();
	return true;
}

bool  Painter::Draw3D(GLenum mode, sgC3DObject* obj3D,bool selSubObjects,bool asHot)
{
	const SG_ALL_TRIANGLES* trngls = obj3D->GetTriangles();

	bool Tex = false;

	if (mode == GL_RENDER)
	{
		glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT|GL_TEXTURE_BIT);

		glPushMatrix();
		if (obj3D->GetTempMatrix()!=0)
			glMultMatrixd(obj3D->GetTempMatrix()->GetTransparentData());
		glMultMatrixd(obj3D->GetWorldMatrixData());

		if ( trngls && 
			(Painter::draw_triangles_regime || (obj3D->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FULL)))
		{
			GLenum Face;
			if(obj3D->Get3DObjectType() ==SG_BODY)
			{
				Face = GL_FRONT;
				::glEnable(GL_CULL_FACE);
				glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_FALSE);
			}
			else if(obj3D->Get3DObjectType() == SG_SURFACE)
			{
				Face = GL_FRONT_AND_BACK;
				::glDisable(GL_CULL_FACE);
				glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);
			}

			GLfloat     c_mat[4]; 
			if (obj3D->GetMaterial()==NULL ||
				(obj3D->GetMaterial()!=NULL && 
				!IsImageMaterial(obj3D->GetMaterial()->MaterialIndex)))
			{ 
				const float* clr = GetColorByIndex(obj3D->GetAttribute(SG_OA_COLOR));
				c_mat[0] = (GLfloat) clr[0];//0.0f;
				c_mat[1] = (GLfloat) clr[1];//0.5f;
				c_mat[2] = (GLfloat) clr[2];//0.75f;
				c_mat[3] = (GLfloat) 1.0f;
				glMaterialfv(Face,GL_AMBIENT,c_mat);
				c_mat[0] = (GLfloat) clr[0];//0.0f;
				c_mat[1] = (GLfloat) clr[1];//0.5f;
				c_mat[2] = (GLfloat) clr[2];//1.0f;
				c_mat[3] = (GLfloat) 1.0f;
				glMaterialfv(Face,GL_DIFFUSE,c_mat);
				c_mat[0] = (GLfloat) 0.6f;//0.2f;
				c_mat[1] = (GLfloat) 0.6f;//0.2f;
				c_mat[2] = (GLfloat) 0.6f;//0.2f;
				c_mat[3] = (GLfloat) 1.0f;
				glMaterialfv(Face,GL_SPECULAR,c_mat);
				glMaterialf(Face,GL_SHININESS,128);
				c_mat[0] = (GLfloat) 0.0f;
				c_mat[1] = (GLfloat) 0.0f;
				c_mat[2] = (GLfloat) 0.0f;
				c_mat[3] = (GLfloat) 1.0f;
				glMaterialfv(Face,GL_EMISSION,c_mat);
			}
			else
			{
				{
					const int matId = obj3D->GetMaterial()->MaterialIndex;
					rtCMaterial* mater = GetMaterial(matId);
					CImageMaterial* im_mat = reinterpret_cast<CImageMaterial*>(mater);

					glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
					glTexImage2D(GL_TEXTURE_2D, 0, 3, im_mat->m_texture.GetSizes().cx,
						im_mat->m_texture.GetSizes().cy,
						0, GL_RGB, GL_UNSIGNED_BYTE,
						im_mat->m_texture.GetPictureBits());

					glEnable(GL_TEXTURE_2D);

					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

					glTexEnvi( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);


					glMatrixMode(GL_TEXTURE );
					glLoadIdentity ();
					glScaled(1.0/im_mat->m_texture.GetScale(), 
						1.0/im_mat->m_texture.GetScale(), 1.0);
					glRotated(im_mat->m_texture.GetAngle(),0,0,1);
					glTranslated(im_mat->m_texture.GetShift(),
						im_mat->m_texture.GetShift(),0.0);
					glMatrixMode(GL_MODELVIEW);

					Tex = true;
				}

			}

			glHint(GL_LINE_SMOOTH_HINT,GL_NICEST);

			glEnable(GL_NORMALIZE);
			glEnable(GL_LIGHTING);

			if (Painter::draw_triangles_regime)
				glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

			glBegin(GL_TRIANGLES);
			for(int i = 0, j=0; i < 3*trngls->nTr; i += 3, j+=6)
			{
				glNormal3dv(&trngls->allNormals[i].x);
				if(Tex)
					glTexCoord2dv(&trngls->allUV[j]);
				glVertex3dv(&trngls->allVertex[i].x);

				glNormal3dv(&trngls->allNormals[i+1].x);
				if(Tex)
					glTexCoord2dv(&trngls->allUV[j+2]);
				glVertex3dv(&trngls->allVertex[i+1].x);

				glNormal3dv(&trngls->allNormals[i+2].x);
				if(Tex)
					glTexCoord2dv(&trngls->allUV[j+4]);
				glVertex3dv(&trngls->allVertex[i+2].x);

			}
			glEnd();

			/*
			glEnableClientState(GL_NORMAL_ARRAY_EXT);
			if (Tex)
			glEnableClientState(GL_TEXTURE_COORD_ARRAY_EXT);
			glEnableClientState(GL_VERTEX_ARRAY_EXT);

			::glNormalPointer(GL_DOUBLE, 0,   (double*)trngls->allNormals);
			if (Tex)
			::glTexCoordPointer(2,GL_DOUBLE, 0, (double*)trngls->allUV);
			::glVertexPointer(3, GL_DOUBLE, 0,(double*)trngls->allVertex);

			::glDrawArrays(GL_TRIANGLES, 0, 3*trngls->nTr);

			glDisableClientState(GL_VERTEX_ARRAY_EXT);
			glDisableClientState(GL_NORMAL_ARRAY_EXT);
			if (Tex)
			glDisableClientState(GL_TEXTURE_COORD_ARRAY_EXT);

			if (Tex)
			glBindTexture(GL_TEXTURE_2D, 0);
			*/

			if (Painter::draw_triangles_regime)
				glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
		}

		if (trngls==NULL || (!Painter::draw_triangles_regime  && (asHot || obj3D->IsSelect() ||
			(obj3D->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FRAME))))
		{
			if (obj3D->IsSelect())
			{
				glLineWidth(3.0f);
				glColor3f(1.0f, 0.0f, 0.0f);
			}
			else
			{
				if (trngls==NULL || ((obj3D->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FRAME)&& 
					!asHot))
				{
					glLineWidth(static_cast<GLfloat>(obj3D->GetAttribute(SG_OA_LINE_THICKNESS)+1));
					glColor3fv(GetColorByIndex(obj3D->GetAttribute(SG_OA_COLOR)));
				}
				else
				{
					glLineWidth(3.0f);
					glColor3f(1.0f, 0.0f, 0.0f);
				}
			}
			glDisable(GL_LIGHTING);
			glDisable(GL_TEXTURE_2D);

			//glLineWidth(static_cast<GLfloat>(obj3D->GetLineThickness()+1));

			GLushort pattern = GetLineTypeByIndex(obj3D->GetAttribute(SG_OA_LINE_TYPE));
			if (pattern>0)
			{
				glEnable(GL_LINE_STIPPLE);
				glLineStipple(1, pattern);
			}
			glEnable(GL_LINE_SMOOTH);
			glBegin(GL_LINES);

			const sgCBRep* br= obj3D->GetBRep();
			
			for (unsigned int i=0;i<br->GetPiecesCount();i++)
			{
				const sgCBRepPiece* brPiece= br->GetPiece(i);
				for (unsigned int j=0;j<brPiece->GetEdgesCount();j++)
				{
					if (brPiece->GetEdges()[j].edge_type & (SG_EDGE_1_LEVEL|SG_EDGE_3_LEVEL|SG_EDGE_2_LEVEL))
					{
						glVertex3dv(&(brPiece->GetVertexes()[brPiece->GetEdges()[j].begin_vertex_index]).x);
						glVertex3dv(&(brPiece->GetVertexes()[brPiece->GetEdges()[j].end_vertex_index]).x);
					}
				}
			}
			glEnd();
		}

		glPopMatrix();

		glPopAttrib();
	}


	return true;
}


