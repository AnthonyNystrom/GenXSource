using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Objects;
using sgCoreWrapper.Structs;
using sgCoreWrapper.Interfaces;

namespace Demo
{
	public static class Painter
	{
		static float[] SYSTEM_COLORS = new float[]
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

		public enum RenderingMode
		{ 
			GL_RENDER, 
			GL_FEEDBACK, 
			GL_SELECT 
		}
		
		public static bool draw_triangles_regime;
		
		public static void DrawObject(RenderingMode mode, msgObject obj, bool selSubObjects, bool asHot)
		{
            if (obj is msg3DObject)
            {
                Draw3DObject((msg3DObject)obj, mode, selSubObjects, asHot);
            }
            else
                if (obj is msgGroup)
                {
                    DrawGroup((msgGroup)obj, mode, selSubObjects, asHot);
                }
                else
                    if (obj is msgLine)
                    {
                        DrawLine((msgLine)obj, mode, selSubObjects, asHot);
                    }
                    else
                        if (obj is msgArc)
                        {
                            DrawArc((msgArc)obj, mode, selSubObjects, asHot);
                        }
                        else
                            if (obj is msgContour)
                            {
                                DrawContour((msgContour)obj, mode, selSubObjects, asHot);
                            }
		}

		public static ushort GetLineTypeByIndex(ushort index)
		{
			return 0;
		}

		public static float[] GetColorByIndex(ushort ind)
		{
			float[] colors = new float[3];
			unsafe
			{
				fixed (float* sysColsTemp = SYSTEM_COLORS)
				{
					float* sysCols = sysColsTemp + 4 * (ind % 239);
					colors[0] = sysCols[0];
					colors[1] = sysCols[1];
					colors[2] = sysCols[2];
				}
			}
			return colors;
		}

		public static void DrawGabariteBox(msgPointStruct pMin, msgPointStruct pMax)
		{
		}

        private static void DrawLine(msgLine objL, RenderingMode mode, bool selSubObjects, bool asHot)
        {
            msgLineStruct ln = objL.GetGeometry();
            double[] pnts1 = new double[3];
            pnts1[0] = ln.P1.x;
            pnts1[1] = ln.P1.y;
            pnts1[2] = ln.P1.z;
            double[] pnts2 = new double[3];
            pnts2[0] = ln.P2.x;
            pnts2[1] = ln.P2.y;
            pnts2[2] = ln.P2.z;

            if (mode == RenderingMode.GL_RENDER)
            {
                OpenGLControl.glPushAttrib(OpenGLControl.GL_ENABLE_BIT |
                    OpenGLControl.GL_LINE_BIT |
                    OpenGLControl.GL_CURRENT_BIT | 
                    OpenGLControl.GL_LIGHTING_BIT);

                OpenGLControl.glDisable(OpenGLControl.GL_LIGHTING);
                OpenGLControl.glDisable(OpenGLControl.GL_TEXTURE_2D);
                OpenGLControl.glLineWidth((float)(objL.GetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS) + 1));

                /*GLushort pattern = GetLineTypeByIndex(objL->GetAttribute(SG_OA_LINE_TYPE));
                if (pattern>0)
                {
                  glEnable(GL_LINE_STIPPLE);
                  glLineStipple(1, pattern);
                }*/

                if (asHot)
                    OpenGLControl.glColor3f(1.0f, 0.0f, 0.0f);
                else
                    if (objL.IsSelect())
                        OpenGLControl.glColor3f(1.0f, 0.0f, 0.0f);
                    else
                        OpenGLControl.glColor3fv(GetColorByIndex(objL.GetAttribute(msgObjectAttrEnum.SG_OA_COLOR)));
                OpenGLControl.glEnable(OpenGLControl.GL_LINE_SMOOTH);
                OpenGLControl.glPushMatrix();
                if (objL.GetTempMatrix() != null)
                {
                    double[] transData = GetDoubles(objL.GetTempMatrix().GetTransparentData().values);
                    unsafe
                    {
                        fixed (double* matrixPtr = transData)
                        {
                            OpenGLControl.glMultMatrixd(matrixPtr);
                        }
                    }
                }
                OpenGLControl.glBegin(OpenGLControl.GL_LINES);
                OpenGLControl.glVertex3dv(pnts1);
                OpenGLControl.glVertex3dv(pnts2);
                OpenGLControl.glEnd();

                OpenGLControl.glPopMatrix();
                OpenGLControl.glPopAttrib();
            }
        }


        private static void DrawArc(msgArc objA, RenderingMode mode, bool selSubObjects, bool asHot)
        {
            msgPointStruct[] pnts = objA.GetPoints();

            if (mode == RenderingMode.GL_RENDER)
            {
                OpenGLControl.glPushAttrib(OpenGLControl.GL_ENABLE_BIT |
                    OpenGLControl.GL_LINE_BIT |
                    OpenGLControl.GL_CURRENT_BIT |
                    OpenGLControl.GL_LIGHTING_BIT);

                OpenGLControl.glDisable(OpenGLControl.GL_LIGHTING);
                OpenGLControl.glDisable(OpenGLControl.GL_TEXTURE_2D);
                OpenGLControl.glLineWidth((float)(objA.GetAttribute(msgObjectAttrEnum.SG_OA_LINE_THICKNESS) + 1));


                /*GLushort pattern = GetLineTypeByIndex(objA->GetAttribute(SG_OA_LINE_TYPE));
                if (pattern > 0)
                {
                    glEnable(GL_LINE_STIPPLE);
                    glLineStipple(1, pattern);
                }*/

                if (asHot)
                    OpenGLControl.glColor3f(1.0f, 0.0f, 0.0f);
                else
                    if (objA.IsSelect())
                        OpenGLControl.glColor3f(1.0f, 0.0f, 0.0f);
                    else
                        OpenGLControl.glColor3fv(GetColorByIndex(objA.GetAttribute(msgObjectAttrEnum.SG_OA_COLOR)));
                OpenGLControl.glEnable(OpenGLControl.GL_LINE_SMOOTH);
                OpenGLControl.glPushMatrix();
                if (objA.GetTempMatrix() != null)
                {
                    double[] transData = GetDoubles(objA.GetTempMatrix().GetTransparentData().values);
                    unsafe
                    {
                        fixed (double* matrixPtr = transData)
                        {
                            OpenGLControl.glMultMatrixd(matrixPtr);
                        }
                    }
                }
                OpenGLControl.glBegin(OpenGLControl.GL_LINE_STRIP);
                for (int i = 0; i < pnts.Length; i++)
                    OpenGLControl.glVertex3d(pnts[i].x, pnts[i].y, pnts[i].z);
                OpenGLControl.glEnd();

                OpenGLControl.glPopMatrix();
                OpenGLControl.glPopAttrib();
            }

        }

        private static void DrawContour(msgContour objCont, RenderingMode mode, bool selSubObjects, bool asHot)
        {
            mIObjectsList objSubList = objCont.GetChildrenList();

            msgObject curSubObj = objSubList.GetHead();
            while (curSubObj != null)
            {
                DrawObject(Painter.RenderingMode.GL_RENDER, curSubObj, false, false);
                curSubObj = objSubList.GetNext(curSubObj);
            }
        }

		private static void Draw3DObject(msg3DObject obj3D, RenderingMode mode, bool selSubObjects, bool asHot)
		{
			msgAllTrianglesStruct trngls = obj3D.GetTriangles();
			
			bool Tex = false;

			if (mode == RenderingMode.GL_RENDER)
			{
				OpenGLControl.glPushAttrib(OpenGLControl.GL_ENABLE_BIT |
					OpenGLControl.GL_LINE_BIT |
					OpenGLControl.GL_CURRENT_BIT |
					OpenGLControl.GL_LIGHTING_BIT |
					OpenGLControl.GL_TEXTURE_BIT);

				OpenGLControl.glPushMatrix();
				if (obj3D.GetTempMatrix() != null)
				{
					double[] transData = GetDoubles(obj3D.GetTempMatrix().GetTransparentData().values);
					unsafe
					{
						fixed (double* matrixPtr = transData)
						{
							OpenGLControl.glMultMatrixd(matrixPtr);
						}
					}
				}

				unsafe
				{
					double[] worldData = GetDoubles(obj3D.GetWorldMatrixData().values);
					fixed (double* matrixPtr = worldData)
					{
						OpenGLControl.glMultMatrixd(matrixPtr);
					}
				}

				if (trngls != null &&
					(draw_triangles_regime || (obj3D.GetAttribute(msgObjectAttrEnum.SG_OA_DRAW_STATE) & (ushort)SG_OA_DRAW_STATEValuesEnum.SGDS_FULL) > 0))
				{
					uint Face = 0;
					if (obj3D.Get3DObjectType() == msg3DObjectTypeEnum.SG_BODY)
					{
						Face = OpenGLControl.GL_FRONT;
						OpenGLControl.glEnable(OpenGLControl.GL_CULL_FACE);
						OpenGLControl.glLightModelf(OpenGLControl.GL_LIGHT_MODEL_TWO_SIDE,
							OpenGLControl.GL_FALSE);
					}
					else if (obj3D.Get3DObjectType() == msg3DObjectTypeEnum.SG_SURFACE)
					{
						Face = OpenGLControl.GL_FRONT_AND_BACK;
						OpenGLControl.glDisable(OpenGLControl.GL_CULL_FACE);
						OpenGLControl.glLightModelf(OpenGLControl.GL_LIGHT_MODEL_TWO_SIDE,
							OpenGLControl.GL_TRUE);
					}

					float[] c_mat = new float[4];

					float[] clrs = GetColorByIndex(obj3D.GetAttribute(msgObjectAttrEnum.SG_OA_COLOR));

					c_mat[0] = clrs[0];//0.0f;
					c_mat[1] = clrs[1];//0.5f;
					c_mat[2] = clrs[2];//0.75f;
					c_mat[3] = 1.0f;
					OpenGLControl.glMaterialfv(Face, OpenGLControl.GL_AMBIENT, c_mat);
					c_mat[0] = clrs[0];//0.0f;
					c_mat[1] = clrs[1];//0.5f;
					c_mat[2] = clrs[2];//1.0f;
					c_mat[3] = 1.0f;
					OpenGLControl.glMaterialfv(Face, OpenGLControl.GL_DIFFUSE, c_mat);
					c_mat[0] = 0.6f;//0.2f;
					c_mat[1] = 0.6f;//0.2f;
					c_mat[2] = 0.6f;//0.2f;
					c_mat[3] = 1.0f;
					OpenGLControl.glMaterialfv(Face, OpenGLControl.GL_SPECULAR, c_mat);
					OpenGLControl.glMaterialf(Face, OpenGLControl.GL_SHININESS, 128);
					c_mat[0] = 0.0f;
					c_mat[1] = 0.0f;
					c_mat[2] = 0.0f;
					c_mat[3] = 1.0f;
					OpenGLControl.glMaterialfv(Face, OpenGLControl.GL_EMISSION, c_mat);

					OpenGLControl.glHint(OpenGLControl.GL_LINE_SMOOTH_HINT,
						OpenGLControl.GL_NICEST);

					OpenGLControl.glEnable(OpenGLControl.GL_NORMALIZE);
					OpenGLControl.glEnable(OpenGLControl.GL_LIGHTING);

					if (draw_triangles_regime)
					{
						OpenGLControl.glPolygonMode(OpenGLControl.GL_FRONT_AND_BACK, OpenGLControl.GL_LINE);
					}

					OpenGLControl.glBegin(OpenGLControl.GL_TRIANGLES);

					OpenGLControl.glBegin(OpenGLControl.GL_TRIANGLES);
					for (int i = 0; i < trngls.allVertex.Length; i += 3)
					{
						OpenGLControl.glNormal3d(trngls.allNormals[i].x, trngls.allNormals[i].y, trngls.allNormals[i].z);
						OpenGLControl.glVertex3d(trngls.allVertex[i].x, trngls.allVertex[i].y, trngls.allVertex[i].z);

						OpenGLControl.glNormal3d(trngls.allNormals[i + 1].x, trngls.allNormals[i + 1].y, trngls.allNormals[i + 1].z);
						OpenGLControl.glVertex3d(trngls.allVertex[i + 1].x, trngls.allVertex[i + 1].y, trngls.allVertex[i + 1].z);

						OpenGLControl.glNormal3d(trngls.allNormals[i + 2].x, trngls.allNormals[i + 2].y, trngls.allNormals[i + 2].z);
						OpenGLControl.glVertex3d(trngls.allVertex[i + 2].x, trngls.allVertex[i + 2].y, trngls.allVertex[i + 2].z);

					}
					OpenGLControl.glEnd();

					if (draw_triangles_regime)
					{
						OpenGLControl.glPolygonMode(OpenGLControl.GL_FRONT_AND_BACK, OpenGLControl.GL_FILL);
					}
				}
			}
		}

        private static void DrawGroup(msgGroup objGr, RenderingMode mode, bool selSubObjects, bool asHot)
        {
            mIObjectsList objSubList = objGr.GetChildrenList();

            msgObject curSubObj = objSubList.GetHead();
            while (curSubObj != null)
            {
                DrawObject(Painter.RenderingMode.GL_RENDER, curSubObj, false, false);
                curSubObj = objSubList.GetNext(curSubObj);
            }
        }

		private static T[] GetVertexes<T>(T[] trngs, int start) where T : new()
		{
			T[] vertexes = new T[3];
			vertexes[0] = trngs[start];
			vertexes[1] = trngs[start + 1];
			vertexes[2] = trngs[start + 2];
			return vertexes;
		}

		private static double[] GetDoubles(msgDoubleStruct[] doublesStruct)
		{
			double[] doubles = new double[16];
			for (int i = 0; i < doubles.Length; i++)
			{
				doubles[i] = doublesStruct[i].value;
			}
			return doubles;
		}
	}
}
