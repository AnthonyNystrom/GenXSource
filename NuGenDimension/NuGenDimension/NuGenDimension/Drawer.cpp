#include "stdafx.h"
#include "Drawer.h"
#include "NuGenDimension.h"

static float SYSTEM_COLORS[1024] =
//              0               1               2               3
{
  0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f, 128.0f/255.0f,0.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f, 128.0f/255.0f,0.0f/255.0f,
    128.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f, 0.0f/255.0f, 128.0f/255.0f,0.0f/255.0f, 128.0f/255.0f,128.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f, 192.0f/255.0f,192.0f/255.0f,192.0f/255.0f,0.0f/255.0f,
    128.0f/255.0f,128.0f/255.0f,128.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f,0.0f/255.0f, 0.0f/255.0f,  255.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,
    255.0f/255.0f, 0.0f/255.0f,  0.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f, 0.0f/255.0f, 0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,
    // 1/18
    255.0f/255.0f,240.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,226.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,212.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,198.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f,184.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,170.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,170.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,146.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f,122.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f, 98.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f, 74.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f, 50.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  8
    // 2/18
    255.0f/255.0f,227.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,199.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,143.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f,115.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 87.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 85.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f, 73.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f, 61.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f, 49.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f, 37.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f, 25.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  8
    // 3/18
    255.0f/255.0f,212.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f, 72.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  8
    // 4/18
    255.0f/255.0f,212.0f/255.0f,227.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,199.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,143.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f, 72.0f/255.0f,115.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f, 87.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f, 85.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f, 73.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f,  0.0f/255.0f, 61.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f, 49.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f, 25.0f/255.0f,0.0f/255.0f, //  8
    // 5/18
    255.0f/255.0f,212.0f/255.0f,240.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,226.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,198.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f, 72.0f/255.0f,184.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f,170.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f,170.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f,146.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f,  0.0f/255.0f,122.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f, 98.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f, 74.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f, 50.0f/255.0f,0.0f/255.0f, //  8
    // 6/18
    255.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  80.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f, //  8
    // 7/18
    240.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 226.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 212.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 198.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    184.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 170.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 170.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 146.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f, //  4
    122.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f,  98.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f,  74.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  50.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f, //  8
    // 8/18
    227.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 199.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 171.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 143.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    115.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  87.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  85.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  73.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f, //  4
    61.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f,  49.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  25.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f,  //  8
    // 9/18
    212.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,177.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,142.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,107.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f, 72.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  37.0f/255.0f, 37.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,220.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f,  0.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,  0.0f/255.0f, 80.0f/255.0f,0.0f/255.0f, //  0
    // 10/18
    212.0f/255.0f,227.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,199.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,171.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,143.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f,115.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  34.0f/255.0f, 87.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 85.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 73.0f/255.0f,220.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f, 61.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 49.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 37.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 25.0f/255.0f, 80.0f/255.0f,0.0f/255.0f, //  8
    // 11/18
    212.0f/255.0f,240.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,226.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,212.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,198.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f,184.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,170.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,170.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,146.0f/255.0f,220.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f,122.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 98.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 74.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 50.0f/255.0f, 80.0f/255.0f,0.0f/255.0f, //  8
    // 12/18
    212.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f,255.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f,220.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f,185.0f/255.0f,185.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f,150.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f,115.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f, 80.0f/255.0f,0.0f/255.0f, //  8
    // 13/18
    212.0f/255.0f,255.0f/255.0f,240.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,226.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,198.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f,255.0f/255.0f,184.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f,170.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f,170.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f,146.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f,185.0f/255.0f,122.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f, 98.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f, 74.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f, 50.0f/255.0f,0.0f/255.0f, //  8
    // 14/18
    212.0f/255.0f,255.0f/255.0f,227.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,199.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,143.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f,255.0f/255.0f,115.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f, 87.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f, 85.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f, 73.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f,185.0f/255.0f, 61.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f, 49.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f, 25.0f/255.0f,0.0f/255.0f, //  8
    // 15/18
    212.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 142.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 107.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    72.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  //  4
    0.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,   0.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  8
    // 15/18
    227.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 199.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 177.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 143.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    115.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f,  87.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f,  85.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  73.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  4
    51.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  49.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  37.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  25.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  //  8
    // 17/18
    240.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 226.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 212.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 198.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    184.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 170.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 170.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 146.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  4
    122.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  98.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  74.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  50.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  8
    // 18/18
    255.0f/255.0f,255.0f/255.0f,212.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,177.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,142.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,107.0f/255.0f,0.0f/255.0f, //  0
    255.0f/255.0f,255.0f/255.0f, 72.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f, 37.0f/255.0f,0.0f/255.0f, 255.0f/255.0f,255.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,220.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  4
    185.0f/255.0f,185.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 150.0f/255.0f,150.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, 115.0f/255.0f,115.0f/255.0f,  0.0f/255.0f,0.0f/255.0f,  80.0f/255.0f, 80.0f/255.0f,  0.0f/255.0f,0.0f/255.0f, //  8
    // gray
    238.0f/255.0f,238.0f/255.0f,238.0f/255.0f,0.0f/255.0f, 220.0f/255.0f,220.0f/255.0f,220.0f/255.0f,0.0f/255.0f, 202.0f/255.0f,202.0f/255.0f,202.0f/255.0f,0.0f/255.0f, 184.0f/255.0f,184.0f/255.0f,184.0f/255.0f,0.0f/255.0f, //  0
    166.0f/255.0f,166.0f/255.0f,166.0f/255.0f,0.0f/255.0f, 148.0f/255.0f,148.0f/255.0f,148.0f/255.0f,0.0f/255.0f, 130.0f/255.0f,130.0f/255.0f,130.0f/255.0f,0.0f/255.0f, 112.0f/255.0f,112.0f/255.0f,112.0f/255.0f,0.0f/255.0f, //  4
};

sgCObject*  Drawer::CurrentHotObject=NULL;
sgCObject*  Drawer::TopParentOfHotObject=NULL;
sgCObject*  Drawer::CurrentEditableObject=NULL;

float  Drawer::HotObjectColor[3] = {1.0f, 0.0f, 0.0f};
float  Drawer::ColorOfObjectInHotGroup[3] = {0.0f, 0.7f, 0.0f};
float  Drawer::SelectedObjectColor[3] = {1.0f, 1.0f, 1.0f};

bool   Drawer::is_VBO_Supported = false;

CMatLoader    Drawer::MatLoader;

bool  Drawer::DrawObject(GLenum mode,sgCObject* obj,bool selSubObjects,bool asHot)
{
  if ((obj->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_HIDE) ||
    (obj==CurrentEditableObject))
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
  case SG_OT_TEXT:
    return DrawText(mode, reinterpret_cast<sgCText*>(obj),selSubObjects,asHot);
  case SG_OT_DIM:
    return DrawDimensions(mode, reinterpret_cast<sgCDimensions*>(obj),selSubObjects,asHot);
  case SG_OT_GROUP:
    return DrawGroup(mode, reinterpret_cast<sgCGroup*>(obj),selSubObjects,asHot);
  case SG_OT_CONTOUR:
    return DrawContour(mode, reinterpret_cast<sgCContour*>(obj),selSubObjects,asHot);
  case SG_OT_3D:
    return DrawBREP(mode, reinterpret_cast<sgC3DObject*>(obj),selSubObjects, asHot);
  default:
    return false;
  }
  return true;
}

const float*   Drawer::GetColorByIndex(unsigned short ind)
{
  return SYSTEM_COLORS+4*(ind%239);
}

const unsigned short Drawer::GetLineTypeByIndex(unsigned short ind)
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

void  Drawer::DrawStylingLine(unsigned short styl,
                CDC* pDC,
                const CPoint& startP,
                int wid)
{
  COLORREF wc = ::GetSysColor(COLOR_WINDOW);
  for (int i=0;i<wid;i++)
  {
    COLORREF col = (TEST_NTH_BIT(styl, i%(sizeof(styl)*8)))?
      RGB(0,0,0):wc;
    pDC->SetPixel(startP.x+i,startP.y,col);
  }
}


void  Drawer::DrawGabariteBox(const SG_POINT& pMin,const SG_POINT& pMax, const float* col)
{
  glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

  glDisable(GL_LIGHTING);
  glDisable(GL_TEXTURE_2D);
  glLineWidth(1);

  glColor3fv(col);

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

bool  Drawer::DrawPoint(GLenum mode,sgCPoint* objP,bool selSubObjects,bool asHot)
{
  const SG_POINT*  pnt = objP->GetGeometry();
  double pnts[3];
  pnts[0]  = pnt->x;
  pnts[1]  = pnt->y;
  pnts[2]  = pnt->z;

  if(mode == GL_SELECT)
  {
    if (objP->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objP));///Номер в режиме выбора


    glPushAttrib(GL_ENABLE_BIT|GL_POINT_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);
    glPointSize(static_cast<float>(objP->GetAttribute(SG_OA_LINE_THICKNESS)+3));
    glDisable(GL_POINT_SMOOTH);
    glBegin(GL_POINTS);
      glVertex3dv(pnts);
    glEnd();

    glPopAttrib();
  }
  if (mode == GL_RENDER)
  {
      glPushAttrib(GL_ENABLE_BIT|GL_POINT_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

      glDisable(GL_LIGHTING);
      glDisable(GL_TEXTURE_2D);

      sgCObject* topPar = GetObjectTopParent(objP);

      if (objP==CurrentHotObject || asHot)
      {
        glPointSize(static_cast<float>(objP->GetAttribute(SG_OA_LINE_THICKNESS)+5));
        glColor3fv(HotObjectColor);
      }
      else
        if (objP->GetParent()!=NULL && topPar==TopParentOfHotObject)
        {
          glPointSize(static_cast<float>(objP->GetAttribute(SG_OA_LINE_THICKNESS)+3));
          glColor3fv(ColorOfObjectInHotGroup);
        }
        else
            if (objP->IsSelect() || (topPar && topPar->IsSelect()))
            {
              glPointSize(static_cast<float>(objP->GetAttribute(SG_OA_LINE_THICKNESS)+3));
              glColor3fv(SelectedObjectColor);
            }
            else
            {
              glPointSize(static_cast<float>(objP->GetAttribute(SG_OA_LINE_THICKNESS)+3));
              glColor3fv(GetColorByIndex(objP->GetAttribute(SG_OA_COLOR)));
            }
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

bool  Drawer::DrawLine(GLenum mode,sgCLine* objL,bool selSubObjects,bool asHot)
{
  const SG_LINE*  ln = objL->GetGeometry();
  double pnts[6];
  pnts[0]  = ln->p1.x;
  pnts[1]  = ln->p1.y;
  pnts[2]  = ln->p1.z;
  pnts[3]  = ln->p2.x;
  pnts[4]  = ln->p2.y;
  pnts[5]  = ln->p2.z;


  if(mode == GL_SELECT)
  {
    if (objL->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objL));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);
    glLineWidth(static_cast<float>(objL->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);
    glBegin(GL_LINES);
      glVertex3dv(pnts);
      glVertex3dv(pnts+3);
    glEnd();

    glPopAttrib();
  }
  if (mode == GL_RENDER)
  {
        glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

        glDisable(GL_LIGHTING);
        glDisable(GL_TEXTURE_2D);

        sgCObject* topPar = GetObjectTopParent(objL);

        GLushort pattern = GetLineTypeByIndex(objL->GetAttribute(SG_OA_LINE_TYPE));
        if (pattern>0)
        {
          glEnable(GL_LINE_STIPPLE);
          glLineStipple(1, pattern);
        }

        if (objL==CurrentHotObject || asHot)
        {
          glLineWidth(static_cast<float>(objL->GetAttribute(SG_OA_LINE_THICKNESS)+3));
          glColor3fv(HotObjectColor);
        }
        else
          if (objL->GetParent()!=NULL && topPar==TopParentOfHotObject)
          {
            glLineWidth(static_cast<float>(objL->GetAttribute(SG_OA_LINE_THICKNESS)+2));
            glColor3fv(ColorOfObjectInHotGroup);
          }
          else
                        if (objL->IsSelect() || (topPar && topPar->IsSelect()))
            {
              glLineWidth(static_cast<float>(objL->GetAttribute(SG_OA_LINE_THICKNESS)+1));
              glColor3fv(SelectedObjectColor);
            }
            else
            {
              glLineWidth(static_cast<float>(objL->GetAttribute(SG_OA_LINE_THICKNESS)+1));
              glColor3fv(GetColorByIndex(objL->GetAttribute(SG_OA_COLOR)));
            }
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

bool   Drawer::DrawCircle(GLenum mode,sgCCircle* objC,bool selSubObjects,bool asHot)
{
  const int  pnts_cnt = objC->GetPointsCount();
  ASSERT(pnts_cnt);
  const SG_POINT*  pnts = objC->GetPoints();
  ASSERT(pnts);


  if(mode == GL_SELECT)
  {
    if (objC->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objC));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    glLineWidth(static_cast<float>(objC->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);
    glBegin(GL_LINE_LOOP);
    for (int i=0;i<pnts_cnt;i++)
      glVertex3dv(&pnts[i].x);
    glEnd();

    glPopAttrib();

  }
  if (mode == GL_RENDER)
  {
        glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

        glDisable(GL_LIGHTING);
        glDisable(GL_TEXTURE_2D);

        GLushort pattern = GetLineTypeByIndex(objC->GetAttribute(SG_OA_LINE_TYPE));
        if (pattern>0)
        {
          glEnable(GL_LINE_STIPPLE);
          glLineStipple(1, pattern);
        }

        sgCObject* topPar = GetObjectTopParent(objC);

        if (objC==CurrentHotObject || asHot)
        {
          glLineWidth(static_cast<float>(objC->GetAttribute(SG_OA_LINE_THICKNESS)+3));
          glColor3fv(HotObjectColor);
        }
        else
          if (objC->GetParent()!=NULL && topPar==TopParentOfHotObject)
          {
            glLineWidth(static_cast<float>(objC->GetAttribute(SG_OA_LINE_THICKNESS)+2));
            glColor3fv(ColorOfObjectInHotGroup);
          }
          else
            if (objC->IsSelect() || (topPar && topPar->IsSelect()))
            {
              glLineWidth(static_cast<float>(objC->GetAttribute(SG_OA_LINE_THICKNESS)+1));
              glColor3fv(SelectedObjectColor);
            }
            else
            {
              glLineWidth(static_cast<float>(objC->GetAttribute(SG_OA_LINE_THICKNESS)+1));
              glColor3fv(GetColorByIndex(objC->GetAttribute(SG_OA_COLOR)));
            }
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

bool   Drawer::DrawArc(GLenum mode,sgCArc* objA,bool selSubObjects,bool asHot)
{
  const int  pnts_cnt = objA->GetPointsCount();
  ASSERT(pnts_cnt);
  const SG_POINT*  pnts = objA->GetPoints();
  ASSERT(pnts);


  if(mode == GL_SELECT)
  {
    if (objA->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objA));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    glLineWidth(static_cast<float>(objA->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);
    glBegin(GL_LINE_STRIP);
    for (int i=0;i<pnts_cnt;i++)
      glVertex3dv(&pnts[i].x);
    glEnd();

    glPopAttrib();

  }
  if (mode == GL_RENDER)
  {
    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    GLushort pattern = GetLineTypeByIndex(objA->GetAttribute(SG_OA_LINE_TYPE));
    if (pattern>0)
    {
      glEnable(GL_LINE_STIPPLE);
      glLineStipple(1, pattern);
    }

    sgCObject* topPar = GetObjectTopParent(objA);

    if (objA==CurrentHotObject || asHot)
    {
      glLineWidth(static_cast<float>(objA->GetAttribute(SG_OA_LINE_THICKNESS)+3));
      glColor3fv(HotObjectColor);
    }
    else
      if (objA->GetParent()!=NULL && topPar==TopParentOfHotObject)
      {
        glLineWidth(static_cast<float>(objA->GetAttribute(SG_OA_LINE_THICKNESS)+2));
        glColor3fv(ColorOfObjectInHotGroup);
      }
      else
        if (objA->IsSelect() || (topPar && topPar->IsSelect()))
        {
          glLineWidth(static_cast<float>(objA->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(SelectedObjectColor);
        }
        else
        {
          glLineWidth(static_cast<float>(objA->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(GetColorByIndex(objA->GetAttribute(SG_OA_COLOR)));
        }

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

bool  Drawer::DrawSpline(GLenum mode,sgCSpline* splObj,bool selSubObjects, bool asHot)
{
  const unsigned int  pnts_cnt = splObj->GetGeometry()->GetPointsCount();
  ASSERT(pnts_cnt);
  const SG_POINT*  pnts = splObj->GetGeometry()->GetPoints();
  ASSERT(pnts);


  if(mode == GL_SELECT)
  {
    if (splObj->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(splObj));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    glLineWidth(static_cast<float>(splObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);
    glBegin(GL_LINE_STRIP);
    for (unsigned int i=0;i<pnts_cnt;i++)
      glVertex3dv(&pnts[i].x);
    glEnd();

    glPopAttrib();

  }
  if (mode == GL_RENDER)
  {
    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    GLushort pattern = GetLineTypeByIndex(splObj->GetAttribute(SG_OA_LINE_TYPE));
    if (pattern>0)
    {
      glEnable(GL_LINE_STIPPLE);
      glLineStipple(1, pattern);
    }

    sgCObject* topPar = GetObjectTopParent(splObj);

    if (splObj==CurrentHotObject || asHot)
    {
      glLineWidth(static_cast<float>(splObj->GetAttribute(SG_OA_LINE_THICKNESS)+3));
      glColor3fv(HotObjectColor);
    }
    else
      if (splObj->GetParent()!=NULL && topPar==TopParentOfHotObject)
      {
        glLineWidth(static_cast<float>(splObj->GetAttribute(SG_OA_LINE_THICKNESS)+2));
        glColor3fv(ColorOfObjectInHotGroup);
      }
      else
        if (splObj->IsSelect() || (topPar && topPar->IsSelect()))
        {
          glLineWidth(static_cast<float>(splObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(SelectedObjectColor);
        }
        else
        {
          glLineWidth(static_cast<float>(splObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(GetColorByIndex(splObj->GetAttribute(SG_OA_COLOR)));
        }
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

bool  Drawer::DrawText(GLenum mode,sgCText* textObj,bool selSubObjects, bool asHot)
{
  const int  lns_cnt = textObj->GetLinesCount();
  //ASSERT(lns_cnt);
  const SG_LINE*  lns = textObj->GetLines();
  //ASSERT(lns);


  if(mode == GL_SELECT)
  {
    if (textObj->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(textObj));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    glLineWidth(static_cast<float>(textObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);
    glBegin(GL_LINES);
    for (int i=0;i<lns_cnt;i++)
    {
      glVertex3dv(&lns[i].p1.x);
      glVertex3dv(&lns[i].p2.x);
    }
    glEnd();

    glPopAttrib();

  }
  if (mode == GL_RENDER)
  {
    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    GLushort pattern = GetLineTypeByIndex(textObj->GetAttribute(SG_OA_LINE_TYPE));
    if (pattern>0)
    {
      glEnable(GL_LINE_STIPPLE);
      glLineStipple(1, pattern);
    }

    sgCObject* topPar = GetObjectTopParent(textObj);

    if (textObj==CurrentHotObject || asHot)
    {
      glLineWidth(static_cast<float>(textObj->GetAttribute(SG_OA_LINE_THICKNESS)+3));
      glColor3fv(HotObjectColor);
    }
    else
      if (textObj->GetParent()!=NULL && topPar==TopParentOfHotObject)
      {
        glLineWidth(static_cast<float>(textObj->GetAttribute(SG_OA_LINE_THICKNESS)+2));
        glColor3fv(ColorOfObjectInHotGroup);
      }
      else
        if (textObj->IsSelect() || (topPar && topPar->IsSelect()))
        {
          glLineWidth(static_cast<float>(textObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(SelectedObjectColor);
        }
        else
        {
          glLineWidth(static_cast<float>(textObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(GetColorByIndex(textObj->GetAttribute(SG_OA_COLOR)));
        }

    glEnable(GL_LINE_SMOOTH);

    glPushMatrix();
    if (textObj->GetTempMatrix()!=0)
      glMultMatrixd(textObj->GetTempMatrix()->GetTransparentData());

    glBegin(GL_LINES);
    for (int i=0;i<lns_cnt;i++)
    {
      glVertex3dv(&lns[i].p1.x);
      glVertex3dv(&lns[i].p2.x);
    }
    glEnd();

    glPopMatrix();
    glPopAttrib();
  }


  return true;
}

bool  Drawer::DrawDimensions(GLenum mode,sgCDimensions* dimObj,bool selSubObjects, bool asHot)
{
  const int  lns_cnt = dimObj->GetLinesCount();
  //ASSERT(lns_cnt);
  const SG_LINE*  lns = dimObj->GetLines();
  //ASSERT(lns);


  if(mode == GL_SELECT)
  {
    if (dimObj->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(dimObj));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    glLineWidth(static_cast<float>(dimObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);
    glBegin(GL_LINES);
    for (int i=0;i<lns_cnt;i++)
    {
      glVertex3dv(&lns[i].p1.x);
      glVertex3dv(&lns[i].p2.x);
    }
    glEnd();

    glPopAttrib();

  }
  if (mode == GL_RENDER)
  {
    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    GLushort pattern = GetLineTypeByIndex(dimObj->GetAttribute(SG_OA_LINE_TYPE));
    if (pattern>0)
    {
      glEnable(GL_LINE_STIPPLE);
      glLineStipple(1, pattern);
    }

    sgCObject* topPar = GetObjectTopParent(dimObj);

    if (dimObj==CurrentHotObject || asHot)
    {
      glLineWidth(static_cast<float>(dimObj->GetAttribute(SG_OA_LINE_THICKNESS)+3));
      glColor3fv(HotObjectColor);
    }
    else
      if (dimObj->GetParent()!=NULL && topPar==TopParentOfHotObject)
      {
        glLineWidth(static_cast<float>(dimObj->GetAttribute(SG_OA_LINE_THICKNESS)+2));
        glColor3fv(ColorOfObjectInHotGroup);
      }
      else
        if (dimObj->IsSelect() || (topPar && topPar->IsSelect()))
        {
          glLineWidth(static_cast<float>(dimObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(SelectedObjectColor);
        }
        else
        {
          glLineWidth(static_cast<float>(dimObj->GetAttribute(SG_OA_LINE_THICKNESS)+1));
          glColor3fv(GetColorByIndex(dimObj->GetAttribute(SG_OA_COLOR)));
        }

    glEnable(GL_LINE_SMOOTH);

    glPushMatrix();
    if (dimObj->GetTempMatrix()!=0)
      glMultMatrixd(dimObj->GetTempMatrix()->GetTransparentData());

    glBegin(GL_LINES);
    for (int i=0;i<lns_cnt;i++)
    {
      glVertex3dv(&lns[i].p1.x);
      glVertex3dv(&lns[i].p2.x);
    }
    glEnd();

    glPopMatrix();
    glPopAttrib();
  }


  return true;
}

bool   Drawer::DrawGroup(GLenum mode, sgCGroup* objGr,bool selSubObjects,bool asHot)
{
  if(mode == GL_SELECT)
  {
    if (objGr->GetParent()==NULL && !selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objGr));///Номер в режиме выбора
  }

  glPushMatrix();
  if (objGr->GetTempMatrix()!=0)
    glMultMatrixd(objGr->GetTempMatrix()->GetTransparentData());

  sgCObject*  curObj = objGr->GetChildrenList()->GetHead();
  while (curObj)
  {
    DrawObject(mode,curObj,selSubObjects,(objGr==CurrentHotObject || asHot));
    curObj = objGr->GetChildrenList()->GetNext(curObj);
  }
  glPopMatrix();
  return true;
}

bool   Drawer::DrawContour(GLenum mode, sgCContour* objCnt,bool selSubObjects,bool asHot)
{
  if(mode == GL_SELECT)
  {
    if (objCnt->GetParent()==NULL && !selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objCnt));///Номер в режиме выбора
  }

  glPushMatrix();
  if (objCnt->GetTempMatrix()!=0)
    glMultMatrixd(objCnt->GetTempMatrix()->GetTransparentData());

  sgCObject*  curObj = objCnt->GetChildrenList()->GetHead();
  while (curObj)
  {
    DrawObject(mode,curObj,selSubObjects,(objCnt==CurrentHotObject || asHot));
    curObj = objCnt->GetChildrenList()->GetNext(curObj);
  }
  glPopMatrix();

  return true;
}

bool  Drawer::DrawBREP(GLenum mode, sgC3DObject* objBREP,bool selSubObjects,bool asHot)
{
  const SG_ALL_TRIANGLES* trngls = objBREP->GetTriangles();
//  ASSERT(trngls);

  bool Tex = false;

  if(mode == GL_SELECT)
  {
    if (objBREP->GetParent()==NULL || selSubObjects)
      glLoadName(reinterpret_cast<GLuint>(objBREP));///Номер в режиме выбора

    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT);

    glDisable(GL_LIGHTING);
    glDisable(GL_TEXTURE_2D);

    glLineWidth(static_cast<float>(objBREP->GetAttribute(SG_OA_LINE_THICKNESS)+1));
    glDisable(GL_LINE_SMOOTH);

    glPushMatrix();
    glMultMatrixd(objBREP->GetWorldMatrixData());

    if (trngls && (objBREP->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FULL))
    {
      glBegin(GL_TRIANGLES);
      for(int i = 0; i < 3*trngls->nTr; i += 3)
      {
        glVertex3dv(&trngls->allVertex[i].x);
        glVertex3dv(&trngls->allVertex[i+1].x);
        glVertex3dv(&trngls->allVertex[i+2].x);
      }
      glEnd();
    }
    else
    {
      if (trngls==NULL || (objBREP->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FRAME))
      {
        glBegin(GL_LINES);

     	const sgCBRep* br= objBREP->GetBRep();
		ASSERT(br);

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
    }

    glPopMatrix();

    glPopAttrib();

  }
  if (mode == GL_RENDER)
  {
    glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_CURRENT_BIT|GL_LIGHTING_BIT|GL_TEXTURE_BIT);

    glPushMatrix();
    if (objBREP->GetTempMatrix()!=0)
      glMultMatrixd(objBREP->GetTempMatrix()->GetTransparentData());
    glMultMatrixd(objBREP->GetWorldMatrixData());

    if (trngls && (objBREP->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FULL))
    {
      
      //glEnable(GL_POLYGON_OFFSET_FILL);
      //glPolygonOffset(1,1);
      //glColorMask(0,0,0,0);
      
            GLenum Face;
            if(objBREP->Get3DObjectType() ==SG_BODY)
            {
              Face = GL_FRONT;
              glEnable(GL_CULL_FACE);
              glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_FALSE);
            }
            else if(objBREP->Get3DObjectType() == SG_SURFACE)
            {
              Face = GL_FRONT_AND_BACK;
              glDisable(GL_CULL_FACE);
              glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);
            }

            GLfloat     c_mat[4]; 
			const SG_MATERIAL *pM = objBREP->GetMaterial();
			bool bIsAtt  = MatLoader.IsAttached();
			int iNTotMat=-1,iMatIdx=0;
			if (bIsAtt)
				iNTotMat = MatLoader.GetMainHeader()->nTotalMat;
			if (pM)
				iMatIdx  = pM->MaterialIndex;
            if (pM==NULL || !bIsAtt || iNTotMat<=iMatIdx)
			//if (pM==NULL)
            {
                const float* clr = GetColorByIndex(objBREP->GetAttribute(SG_OA_COLOR));
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
                c_mat[0] = (GLfloat) 0.2f;//0.2f;
                c_mat[1] = (GLfloat) 0.2f;//0.2f;
                c_mat[2] = (GLfloat) 0.2f;//0.2f;
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
              const int matId = objBREP->GetMaterial()->MaterialIndex;
              const MAT_ITEM* mI = MatLoader.GetMaterialByIndex(matId);
              c_mat[0] = (GLfloat) mI->AmbientR;
              c_mat[1] = (GLfloat) mI->AmbientG;//0.5f;
              c_mat[2] = (GLfloat) mI->AmbientB;//0.75f;
              c_mat[3] = (GLfloat) 1.0f-mI->Transparent;
              glMaterialfv(Face,GL_AMBIENT,c_mat);
              c_mat[0] = (GLfloat) mI->DiffuseR;//0.0f;
              c_mat[1] = (GLfloat) mI->DiffuseG;//0.5f;
              c_mat[2] = (GLfloat) mI->DiffuseB;//1.0f;
              c_mat[3] = (GLfloat) 1.0f-mI->Transparent;
              glMaterialfv(Face,GL_DIFFUSE,c_mat);
              c_mat[0] = (GLfloat) mI->SpecularR;
              c_mat[1] = (GLfloat) mI->SpecularG;
              c_mat[2] = (GLfloat) mI->SpecularB;
              c_mat[3] = (GLfloat) 1.0f-mI->Transparent;
              glMaterialfv(Face,GL_SPECULAR,c_mat);
              glMaterialf(Face,GL_SHININESS,mI->Shininess);
              c_mat[0] = (GLfloat) mI->EmissionR;
              c_mat[1] = (GLfloat) mI->EmissionG;
              c_mat[2] = (GLfloat) mI->EmissionB;
              c_mat[3] = (GLfloat) 1.0f-mI->Transparent;
              glMaterialfv(Face,GL_EMISSION,c_mat);

              if (mI->nIdxTexture && MatLoader.GetMainHeader()->nTotalTex>=mI->nIdxTexture)
              {
                const TEXTURE_ITEM*  texture = MatLoader.GetTextureByIndex(mI->nIdxTexture-1);
                ASSERT(texture->opengl_texture);
                ASSERT(texture->opengl_texture->pPicBits);
                glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
                glTexImage2D(GL_TEXTURE_2D, 0, 3, texture->opengl_texture->Width,
                  texture->opengl_texture->Height,
                  0, GL_RGB, GL_UNSIGNED_BYTE,
                  texture->opengl_texture->pPicBits);

                glEnable(GL_TEXTURE_2D);

                const SG_MATERIAL* mat = objBREP->GetMaterial();
                ASSERT(mat);

                if(mat->TextureSmooth)
                {
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

                }
                else
                {
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
                }

                if(mat->TextureMult)
                {
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
                }
                else
                {
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP );
                  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP );
                }

                if(mat->MixColorType == SG_REPLACE_MIX_TYPE)
                  glTexEnvi( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);
                else if(mat->MixColorType == SG_BLEND_MIX_TYPE)
                  glTexEnvi( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_BLEND);
                else if (mat->MixColorType == SG_MODULATE_MIX_TYPE)
                  glTexEnvi( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
                else
                  glTexEnvi( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);

                glMatrixMode(GL_TEXTURE );
                glLoadIdentity ();
                glScaled(1.0/mat->TextureScaleU, 1.0/mat->TextureScaleV, 1.0);
                glRotated(mat->TextureAngle,0,0,1);
                glTranslated(mat->TextureShiftU,mat->TextureShiftV,0.0);
                glMatrixMode(GL_MODELVIEW);

                Tex = true;
              }

            }

            glHint(GL_LINE_SMOOTH_HINT,GL_NICEST);
			


            glEnable(GL_NORMALIZE);
            glEnable(GL_LIGHTING);

            //glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

            glBegin(GL_TRIANGLES);
            for(int i = 0, j=0; i < 3*trngls->nTr; i += 3, j+=6)
            {
              glNormal3dv(&trngls->allNormals[i].x);
              if(Tex)
                glTexCoord2dv(&trngls->allUV[j]);//-------
              glVertex3dv(&trngls->allVertex[i].x);

              glNormal3dv(&trngls->allNormals[i+1].x);
              if(Tex)
                glTexCoord2dv(&trngls->allUV[j+2]);//-------
              glVertex3dv(&trngls->allVertex[i+1].x);

              glNormal3dv(&trngls->allNormals[i+2].x);
              if(Tex)
                glTexCoord2dv(&trngls->allUV[j+4]);//-------
              glVertex3dv(&trngls->allVertex[i+2].x);

            }
            glEnd();

			/**********************************/
		/*	glEnableClientState(GL_NORMAL_ARRAY_EXT);
			if (Tex)
				glEnableClientState(GL_TEXTURE_COORD_ARRAY_EXT);
			glEnableClientState(GL_VERTEX_ARRAY_EXT);

			glNormalPointer(GL_DOUBLE, 0,   (double*)trngls->allNormals);
			if (Tex)
				glTexCoordPointer(2,GL_DOUBLE, 0, (double*)trngls->allUV);
			glVertexPointer(3, GL_DOUBLE, 0,(double*)trngls->allVertex);

			glDrawArrays(GL_TRIANGLES, 0, 3*trngls->nTr);

			glDisableClientState(GL_VERTEX_ARRAY_EXT);
			glDisableClientState(GL_NORMAL_ARRAY_EXT);
			if (Tex)
				glDisableClientState(GL_TEXTURE_COORD_ARRAY_EXT);*/
			/****************************************************/

            //glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
    }

    sgCObject* topPar = GetObjectTopParent(objBREP);

    if (trngls==NULL || objBREP==CurrentHotObject || asHot || objBREP->IsSelect() ||
      (topPar && topPar->IsSelect()) ||
      (objBREP->GetParent()!=NULL && topPar==TopParentOfHotObject) ||
      (objBREP->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FRAME))
    {
      //glColorMask(1,1,1,1);
      //glDisable(GL_POLYGON_OFFSET_FILL);
      
      if (objBREP==CurrentHotObject || asHot)
      {
        glLineWidth(4.0f);
        glColor3fv(HotObjectColor);
      }
      else
        if (objBREP->GetParent()!=NULL && topPar==TopParentOfHotObject)
        {
          glLineWidth(2.0f);
          glColor3fv(ColorOfObjectInHotGroup);
        }
        else
          if (objBREP->IsSelect() || (topPar && topPar->IsSelect()))
          {
            glLineWidth(2.0f);
            glColor3fv(SelectedObjectColor);
          }
          else
            if (trngls==NULL || (objBREP->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_FRAME))
            {
              glLineWidth(static_cast<GLfloat>(objBREP->GetAttribute(SG_OA_LINE_THICKNESS)+1));
              glColor3fv(GetColorByIndex(objBREP->GetAttribute(SG_OA_COLOR)));
            }

      glDisable(GL_LIGHTING);
      glDisable(GL_TEXTURE_2D);

      //glLineWidth(static_cast<GLfloat>(objBREP->GetLineThickness()+1));

      GLushort pattern = GetLineTypeByIndex(objBREP->GetAttribute(SG_OA_LINE_TYPE));
      if (pattern>0)
      {
        glEnable(GL_LINE_STIPPLE);
        glLineStipple(1, pattern);
      }
      glEnable(GL_LINE_SMOOTH);
      glBegin(GL_LINES);

  	  const sgCBRep* br= objBREP->GetBRep();
	  ASSERT(br);

	  for (unsigned int i=0;i<br->GetPiecesCount();i++)
	  {
		  const sgCBRepPiece* brPiece= br->GetPiece(i);
		  //glColor3fv(GetColorByIndex(i%200));
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













bool      Drawer::ProjectObjectOnMetaDC(sgCObject* obj,
                          CDC *pDC,
                          double *modelMatrix,
                          double *projMatrix,
                          int *viewport,
                          int height,
                          WMF_PROJECT_TYPE projType,
                          double ratio,
                          float RatioNbFace)
{
  if ((obj->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_HIDE) ||
    (obj==CurrentEditableObject))
    return false;

  switch(obj->GetType())
  {
  case SG_OT_POINT:
    return ProjectPointOnMetaDC(reinterpret_cast<sgCPoint*>(obj),
                      pDC,
                      modelMatrix,
                      projMatrix,
                      viewport,
                      height,
                      projType,
                      ratio,
                      RatioNbFace);
  case SG_OT_LINE:
    return ProjectLineOnMetaDC(reinterpret_cast<sgCLine*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_CIRCLE:
    return ProjectCircleOnMetaDC(reinterpret_cast<sgCCircle*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_ARC:
    return ProjectArcOnMetaDC(reinterpret_cast<sgCArc*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_SPLINE:
    return ProjectSplineOnMetaDC( reinterpret_cast<sgCSpline*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_TEXT:
    return ProjectTextOnMetaDC( reinterpret_cast<sgCText*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_DIM:
    return ProjectDimensionsOnMetaDC( reinterpret_cast<sgCDimensions*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_GROUP:
    return ProjectGroupOnMetaDC( reinterpret_cast<sgCGroup*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_CONTOUR:
    return ProjectContourOnMetaDC( reinterpret_cast<sgCContour*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  case SG_OT_3D:
    return ProjectBREPOnMetaDC( reinterpret_cast<sgC3DObject*>(obj),
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace);
  default:
    return false;
  }
  return true;
}

static void  swap(int& a, int& b)
{
  int c=a;
  a=b; b=c;
}

static int clip_code(const int& clipRectX1,
             const int& clipRectY1,
             const int& clipRectX2,
             const int& clipRectY2,
             const int& xP,
             const int& yP)
{
  int cd = 0;

  if (xP<clipRectX1) cd|=0x01;
  if (yP<clipRectY1) cd|=0x02;
  if (xP>clipRectX2) cd|=0x04;
  if (yP>clipRectY2) cd|=0x08;

  return cd;
}

static  bool  clip_line(const int& clipRectX1,
            const int& clipRectY1,
            const int& clipRectX2,
            const int& clipRectY2,
            int& xP1,
            int& yP1,
            int& xP2,
            int& yP2)
{
  int code1 = clip_code(clipRectX1,clipRectY1,clipRectX2,clipRectY2,xP1,yP1);
  int code2 = clip_code(clipRectX1,clipRectY1,clipRectX2,clipRectY2,xP2,yP2);

  int inside = (code1 | code2)==0;
  int outside = (code1 & code2)!=0;

  if (outside) return false;

  while (!outside && !inside)
  {
    if (code1==0)
    {
      swap(xP1,xP2); swap(yP1,yP2); swap(code1,code2);
    }
    if (code1 & 0x01)  // clip left
    {
      yP1+=(long)(yP2-yP1)*(clipRectX1-xP1)/(xP2-xP1);
      xP1 = clipRectX1;
    }
    else
      if (code1 & 0x02)  // clip above
      {
        xP1+=(long)(xP2-xP1)*(clipRectY1-yP1)/(yP2-yP1);
        yP1 = clipRectY1;
      }
      else
        if (code1 & 0x04)  // clip right
        {
          yP1+=(long)(yP2-yP1)*(clipRectX2-xP1)/(xP2-xP1);
          xP1 = clipRectX2;
        }
        else
          if (code1 & 0x08)  // clip below
          {
            xP1+=(long)(xP2-xP1)*(clipRectY2-yP1)/(yP2-yP1);
            yP1 = clipRectY2;
          }
    code1 = clip_code(clipRectX1,clipRectY1,clipRectX2,clipRectY2,xP1,yP1);
    code2 = clip_code(clipRectX1,clipRectY1,clipRectX2,clipRectY2,xP2,yP2);
    inside = (code1 | code2)==0;
    outside = (code1 & code2)!=0;
  }
  return true;
}

bool   Drawer::ProjectPointOnMetaDC(sgCPoint* objP,
                           CDC *pDC,
                           double *modelMatrix,
                           double *projMatrix,
                           int *viewport,
                           int height,
                           WMF_PROJECT_TYPE projType,
                           double ratio,
                           float RatioNbFace)
{
  const SG_POINT*  pnt = objP->GetGeometry();

  double x1,y1,z1;

  COLORREF ColorLine = RGB((int)(GetColorByIndex(objP->GetAttribute(SG_OA_COLOR))[0]*255.0f),
               (int)(GetColorByIndex(objP->GetAttribute(SG_OA_COLOR))[1]*255.0f),
              (int)(GetColorByIndex(objP->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  gluProject(pnt->x,pnt->z,pnt->z,
            modelMatrix,
            projMatrix,
      viewport,&x1,&y1,&z1);
    // Crop to window
  if(x1 >= viewport[0] && y1 >= viewport[1]  &&
    x1 <= viewport[2] && y1 <= viewport[3])
      pDC->SetPixel((int)(ratio*x1),(int)(ratio*((float)height-y1)),ColorLine);

  return true;
}

bool   Drawer::ProjectLineOnMetaDC(sgCLine* objL,
                          CDC *pDC,
                          double *modelMatrix,
                          double *projMatrix,
                          int *viewport,
                          int height,
                          WMF_PROJECT_TYPE projType,
                          double ratio,
                          float RatioNbFace)
{
  const SG_LINE*  ln = objL->GetGeometry();

  COLORREF ColorLine = RGB((int)(GetColorByIndex(objL->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(objL->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(objL->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  double x1,y1,x2,y2,z;

    gluProject(ln->p1.x,ln->p1.y,ln->p1.z,
      modelMatrix,
      projMatrix,
      viewport,&x1,&y1,&z);

    gluProject(ln->p2.x,ln->p2.y,ln->p2.z,
      modelMatrix,
      projMatrix,
      viewport,&x2,&y2,&z);

    int x1_i = (int)x1;
    int y1_i = (int)y1;
    int x2_i = (int)x2;
    int y2_i = (int)y2;

    // Crop to window
    if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
      x1_i,y1_i,x2_i,y2_i))
    {
      x1 = (double)x1_i;
      y1 = (double)y1_i;
      x2 = (double)x2_i;
      y2 = (double)y2_i;
      pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
      pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
    }

  pDC->SelectObject(pOldPen);
  return true;
}

bool   Drawer::ProjectCircleOnMetaDC(sgCCircle* objC,
                          CDC *pDC,
                          double *modelMatrix,
                          double *projMatrix,
                          int *viewport,
                          int height,
                          WMF_PROJECT_TYPE projType,
                          double ratio,
                          float RatioNbFace)
{
  const int  pnts_cnt = objC->GetPointsCount();
  ASSERT(pnts_cnt);
  const SG_POINT*  pnts = objC->GetPoints();
  ASSERT(pnts);

  COLORREF ColorLine = RGB((int)(GetColorByIndex(objC->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(objC->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(objC->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  double x1,y1,x2,y2,z;

  for (int i=0;i<pnts_cnt;i++)
  {
      gluProject(pnts[i].x,pnts[i].y,pnts[i].z,
        modelMatrix,
        projMatrix,
        viewport,&x1,&y1,&z);

      int secPInd = (i==(pnts_cnt-1))?0:(i+1);

      gluProject(pnts[secPInd].x,pnts[secPInd].y,pnts[secPInd].z,
        modelMatrix,
        projMatrix,
        viewport,&x2,&y2,&z);

      int x1_i = (int)x1;
      int y1_i = (int)y1;
      int x2_i = (int)x2;
      int y2_i = (int)y2;

      // Crop to window
      if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
        x1_i,y1_i,x2_i,y2_i))
      {
        x1 = (double)x1_i;
        y1 = (double)y1_i;
        x2 = (double)x2_i;
        y2 = (double)y2_i;
        pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
        pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
      }
  }

  pDC->SelectObject(pOldPen);

  return true;
}

bool   Drawer::ProjectArcOnMetaDC(sgCArc* objA,
                         CDC *pDC,
                         double *modelMatrix,
                         double *projMatrix,
                         int *viewport,
                         int height,
                         WMF_PROJECT_TYPE projType,
                         double ratio,
                         float RatioNbFace)
{
  const int  pnts_cnt = objA->GetPointsCount();
  ASSERT(pnts_cnt);
  const SG_POINT*  pnts = objA->GetPoints();
  ASSERT(pnts);

  COLORREF ColorLine = RGB((int)(GetColorByIndex(objA->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(objA->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(objA->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  double x1,y1,x2,y2,z;

  for (int i=0;i<pnts_cnt-1;i++)
  {
    gluProject(pnts[i].x,pnts[i].y,pnts[i].z,
      modelMatrix,
      projMatrix,
      viewport,&x1,&y1,&z);

    gluProject(pnts[i+1].x,pnts[i+1].y,pnts[i+1].z,
      modelMatrix,
      projMatrix,
      viewport,&x2,&y2,&z);

    int x1_i = (int)x1;
    int y1_i = (int)y1;
    int x2_i = (int)x2;
    int y2_i = (int)y2;

    // Crop to window
    if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
      x1_i,y1_i,x2_i,y2_i))
    {
      x1 = (double)x1_i;
      y1 = (double)y1_i;
      x2 = (double)x2_i;
      y2 = (double)y2_i;
      pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
      pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
    }
  }

  pDC->SelectObject(pOldPen);

  return true;
}

bool Drawer::ProjectSplineOnMetaDC(sgCSpline* splObj,
                          CDC *pDC,
                          double *modelMatrix,
                          double *projMatrix,
                          int *viewport,
                          int height,
                          WMF_PROJECT_TYPE projType,
                          double ratio,
                          float RatioNbFace)
{
  const unsigned int  pnts_cnt = splObj->GetGeometry()->GetPointsCount();
  ASSERT(pnts_cnt);
  const SG_POINT*  pnts = splObj->GetGeometry()->GetPoints();
  ASSERT(pnts);

  COLORREF ColorLine = RGB((int)(GetColorByIndex(splObj->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(splObj->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(splObj->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  double x1,y1,x2,y2,z;

  for (unsigned int i=0;i<pnts_cnt-1;i++)
  {
    gluProject(pnts[i].x,pnts[i].y,pnts[i].z,
      modelMatrix,
      projMatrix,
      viewport,&x1,&y1,&z);

    gluProject(pnts[i+1].x,pnts[i+1].y,pnts[i+1].z,
      modelMatrix,
      projMatrix,
      viewport,&x2,&y2,&z);

    int x1_i = (int)x1;
    int y1_i = (int)y1;
    int x2_i = (int)x2;
    int y2_i = (int)y2;

    // Crop to window
    if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
      x1_i,y1_i,x2_i,y2_i))
    {
      x1 = (double)x1_i;
      y1 = (double)y1_i;
      x2 = (double)x2_i;
      y2 = (double)y2_i;
      pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
      pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
    }
  }

  pDC->SelectObject(pOldPen);

  return true;
}

bool Drawer::ProjectTextOnMetaDC(sgCText* textObj,
                   CDC *pDC,
                   double *modelMatrix,
                   double *projMatrix,
                   int *viewport,
                   int height,
                   WMF_PROJECT_TYPE projType,
                   double ratio,
                   float RatioNbFace)
{
  const int  lns_cnt = textObj->GetLinesCount();
    ASSERT(lns_cnt);
  const SG_LINE*  lns = textObj->GetLines();
    ASSERT(lns);


  COLORREF ColorLine = RGB((int)(GetColorByIndex(textObj->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(textObj->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(textObj->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  double x1,y1,x2,y2,z;

  for (int i=0;i<lns_cnt;i++)
  {
    gluProject(lns[i].p1.x,lns[i].p1.y,lns[i].p1.z,
      modelMatrix,
      projMatrix,
      viewport,&x1,&y1,&z);

    gluProject(lns[i].p2.x,lns[i].p2.y,lns[i].p2.z,
      modelMatrix,
      projMatrix,
      viewport,&x2,&y2,&z);

    int x1_i = (int)x1;
    int y1_i = (int)y1;
    int x2_i = (int)x2;
    int y2_i = (int)y2;

    // Crop to window
    if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
      x1_i,y1_i,x2_i,y2_i))
    {
      x1 = (double)x1_i;
      y1 = (double)y1_i;
      x2 = (double)x2_i;
      y2 = (double)y2_i;
      pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
      pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
    }
  }

  pDC->SelectObject(pOldPen);

  return true;
}

bool Drawer::ProjectDimensionsOnMetaDC(sgCDimensions* dimObj,
                 CDC *pDC,
                 double *modelMatrix,
                 double *projMatrix,
                 int *viewport,
                 int height,
                 WMF_PROJECT_TYPE projType,
                 double ratio,
                 float RatioNbFace)
{
  const int  lns_cnt = dimObj->GetLinesCount();
  ASSERT(lns_cnt);
  const SG_LINE*  lns = dimObj->GetLines();
  ASSERT(lns);


  COLORREF ColorLine = RGB((int)(GetColorByIndex(dimObj->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(dimObj->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(dimObj->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  double x1,y1,x2,y2,z;

  for (int i=0;i<lns_cnt;i++)
  {
    gluProject(lns[i].p1.x,lns[i].p1.y,lns[i].p1.z,
      modelMatrix,
      projMatrix,
      viewport,&x1,&y1,&z);

    gluProject(lns[i].p2.x,lns[i].p2.y,lns[i].p2.z,
      modelMatrix,
      projMatrix,
      viewport,&x2,&y2,&z);

    int x1_i = (int)x1;
    int y1_i = (int)y1;
    int x2_i = (int)x2;
    int y2_i = (int)y2;

    // Crop to window
    if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
      x1_i,y1_i,x2_i,y2_i))
    {
      x1 = (double)x1_i;
      y1 = (double)y1_i;
      x2 = (double)x2_i;
      y2 = (double)y2_i;
      pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
      pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
    }
  }

  pDC->SelectObject(pOldPen);

  return true;
}

bool  Drawer::ProjectGroupOnMetaDC(sgCGroup* objGr,
                           CDC *pDC,
                           double *modelMatrix,
                           double *projMatrix,
                           int *viewport,
                           int height,
                           WMF_PROJECT_TYPE projType,
                           double ratio,
                           float RatioNbFace)
{
  sgCObject*  curObj = objGr->GetChildrenList()->GetHead();
  while (curObj)
  {
    if (!ProjectObjectOnMetaDC(curObj,
                pDC,
                modelMatrix,
                projMatrix,
                viewport,
                height,
                projType,
                ratio,
                RatioNbFace))
        return false;
    curObj = objGr->GetChildrenList()->GetNext(curObj);
  }

  return true;
}

bool  Drawer::ProjectContourOnMetaDC(sgCContour* objCnt,
                   CDC *pDC,
                   double *modelMatrix,
                   double *projMatrix,
                   int *viewport,
                   int height,
                   WMF_PROJECT_TYPE projType,
                   double ratio,
                   float RatioNbFace)
{
  sgCObject*  curObj = objCnt->GetChildrenList()->GetHead();
  while (curObj)
  {
    if (!ProjectObjectOnMetaDC(curObj,
      pDC,
      modelMatrix,
      projMatrix,
      viewport,
      height,
      projType,
      ratio,
      RatioNbFace))
      return false;
    curObj = objCnt->GetChildrenList()->GetNext(curObj);
  }

  return true;
}

bool  Drawer::ProjectBREPOnMetaDC(sgC3DObject* objBREP,
                          CDC *pDC,
                          double *modelMatrix,
                          double *projMatrix,
                          int *viewport,
                          int height,
                          WMF_PROJECT_TYPE projType,
                          double ratio,
                          float RatioNbFace)
{

  COLORREF ColorLine = RGB((int)(GetColorByIndex(objBREP->GetAttribute(SG_OA_COLOR))[0]*255.0f),
    (int)(GetColorByIndex(objBREP->GetAttribute(SG_OA_COLOR))[1]*255.0f),
    (int)(GetColorByIndex(objBREP->GetAttribute(SG_OA_COLOR))[2]*255.0f));

  CPen pen(PS_SOLID,0,ColorLine);
  CPen *pOldPen = pDC->SelectObject(&pen);

  //double x1,y1,x2,y2,z;#WARNING


  //glMultMatrixd(objBREP->GetWorldMatrixData());

  sgCMatrix matr(objBREP->GetWorldMatrixData());
  matr.Transparent();

  //ASSERT(0); // old code

	const sgCBRep* br= objBREP->GetBRep();
		ASSERT(br);
	for (unsigned int i=0;i<br->GetPiecesCount();i++)
	{
		const sgCBRepPiece* brPiece= br->GetPiece(i);
		for (unsigned int j=0;j<brPiece->GetEdgesCount();j++)
		{
			if (brPiece->GetEdges()[j].edge_type & (SG_EDGE_1_LEVEL|SG_EDGE_3_LEVEL|SG_EDGE_2_LEVEL))
			{
				SG_POINT P_1 = brPiece->GetVertexes()[brPiece->GetEdges()[j].begin_vertex_index];
				matr.ApplyMatrixToPoint(P_1);

				GLdouble x1,x2,y1,y2,z;
				gluProject(P_1.x,P_1.y,P_1.z,
				  modelMatrix,
				  projMatrix,
				  viewport,&x1,&y1,&z);

				SG_POINT P_2 = brPiece->GetVertexes()[brPiece->GetEdges()[j].end_vertex_index ];
				matr.ApplyMatrixToPoint(P_2);

				gluProject(P_2.x,P_2.y,P_2.z,
				  modelMatrix,
				  projMatrix,
				  viewport,&x2,&y2,&z);

				int x1_i = (int)x1;
				int y1_i = (int)y1;
				int x2_i = (int)x2;
				int y2_i = (int)y2;

				// Crop to window
				if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
				  x1_i,y1_i,x2_i,y2_i))
				{
				  x1 = (double)x1_i;
				  y1 = (double)y1_i;
				  x2 = (double)x2_i;
				  y2 = (double)y2_i;
				  pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
				  pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
				}
			}
		}
	}

	glEnd();
  /*for (long i=0;i<Edgs->nEdges;i++)
  {
    if (Edgs->allEdges[i].edge_type & (SG_EDGE_1_LEVEL|SG_EDGE_3_LEVEL|SG_EDGE_2_LEVEL))
    {
        SG_POINT P_1 = Edgs->allVertex[Edgs->allEdges[i].begin_vertex_index];
        matr.ApplyMatrixToPoint(P_1);

        gluProject(P_1.x,P_1.y,P_1.z,
          modelMatrix,
          projMatrix,
          viewport,&x1,&y1,&z);

        SG_POINT P_2 = Edgs->allVertex[Edgs->allEdges[i].end_vertex_index];
        matr.ApplyMatrixToPoint(P_2);

        gluProject(P_2.x,P_2.y,P_2.z,
          modelMatrix,
          projMatrix,
          viewport,&x2,&y2,&z);

        int x1_i = (int)x1;
        int y1_i = (int)y1;
        int x2_i = (int)x2;
        int y2_i = (int)y2;

        // Crop to window
        if(clip_line(viewport[0],viewport[1],viewport[2],viewport[3],
          x1_i,y1_i,x2_i,y2_i))
        {
          x1 = (double)x1_i;
          y1 = (double)y1_i;
          x2 = (double)x2_i;
          y2 = (double)y2_i;
          pDC->MoveTo((int)(ratio*x1),(int)(ratio*((float)height-y1)));
          pDC->LineTo((int)(ratio*x2),(int)(ratio*((float)height-y2)));
        }
    }
  }
*/
  pDC->SelectObject(pOldPen);

  return true;
}
