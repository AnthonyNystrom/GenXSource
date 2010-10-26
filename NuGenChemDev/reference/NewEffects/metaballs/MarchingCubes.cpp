//////////////////////////////////////////////////////////////////////////////
//
//  Author      : Josh Grant
//  Date        : November 26th, 2001
//  File        : MarchingCubes.cpp
//  Description : Header file defining the MarchingCubes class
//
//////////////////////////////////////////////////////////////////////////////

#include <iostream.h>

#include "MarchingCubes.h"

//////////////////////////////////////////////////////////////////////////////
//
// MarchingCubes class
//
// All tables were based off of code written by Cory Bloyd
// (corysama@yahoo.com), from a nice description of the Marching
// Cubes algorithm located at
// http://www.swin.edu.au/astronomy/pbourke/modelling/polygonise/
//
//////////////////////////////////////////////////////////////////////////////

// the blending percentage must be less than 0.5
#define MC_MAX_BLEND_PERCENT 0.499999f

// blending technique added by Gokhan Kisacikoglu
namespace blend
{
    // The blending is nothing but a simple test to the closest voxel 
    //	corner. If we are too close, we accumulate the point and 
    //	average it later. The accumulated point will be kept in the 
    //	'verts' for conservative memory management, but we have to 
    //	save at least its index and the number of accumulated points.
    //	
    class point
    {
    public:
	point() : 
	    m_location(0, 0, 0), 
	    m_normal(0, 0, 0), 
	    m_vertexIndex(-1), m_hits(0) {}
	
	void set( int _vertexIndex, 
		    const SbVec3f &_location, 
		    const SbVec3f &_normal )
	{
	    m_vertexIndex = _vertexIndex;
	    m_location = _location;
	    m_normal = _normal;
	    
	    m_hits = 1;
	}
	
	int add( const SbVec3f &_location, 
		 const SbVec3f &_normal )
	{
	    m_location += _location;
	    m_normal += _normal;
	    ++ m_hits;
	    
	    return m_vertexIndex;
	}
	
	int hits() const { return m_hits; }
	SbVec3f location() const { return m_location / float(m_hits); }
	SbVec3f normal() const { return m_normal / float(m_hits); }
	int vertexIndex() const { return m_vertexIndex; }
	
    private:
	SbVec3f m_location, m_normal;
	int	m_vertexIndex, m_hits;
    };
}

// For each of the possible vertex states listed in this table there is a
// specific triangulation of the edge intersection points.  The table lists
// all of them in the form of 0-5 edge triples with the list terminated by
// the invalid value -1.  For example: triTable[3] list the 2 triangles
// formed when cube[0] and cube[1] are inside of the surface, but the rest of
// the cube is not. 
static const short triTable[256][16] = {
  {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  1,  9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  8,  3,  9,  8,  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  3,  1,  2, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 9,  2, 10,  0,  2,  9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 2,  8,  3,  2, 10,  8, 10,  9,  8, -1, -1, -1, -1, -1, -1, -1},
  { 3, 11,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0, 11,  2,  8, 11,  0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  9,  0,  2,  3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1, 11,  2,  1,  9, 11,  9,  8, 11, -1, -1, -1, -1, -1, -1, -1},
  { 3, 10,  1, 11, 10,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0, 10,  1,  0,  8, 10,  8, 11, 10, -1, -1, -1, -1, -1, -1, -1},
  { 3,  9,  0,  3, 11,  9, 11, 10,  9, -1, -1, -1, -1, -1, -1, -1},
  { 9,  8, 10, 10,  8, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  7,  8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  3,  0,  7,  3,  4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  1,  9,  8,  4,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  1,  9,  4,  7,  1,  7,  3,  1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 10,  8,  4,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 3,  4,  7,  3,  0,  4,  1,  2, 10, -1, -1, -1, -1, -1, -1, -1},
  { 9,  2, 10,  9,  0,  2,  8,  4,  7, -1, -1, -1, -1, -1, -1, -1},
  { 2, 10,  9,  2,  9,  7,  2,  7,  3,  7,  9,  4, -1, -1, -1, -1},
  { 8,  4,  7,  3, 11,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {11,  4,  7, 11,  2,  4,  2,  0,  4, -1, -1, -1, -1, -1, -1, -1},
  { 9,  0,  1,  8,  4,  7,  2,  3, 11, -1, -1, -1, -1, -1, -1, -1},
  { 4,  7, 11,  9,  4, 11,  9, 11,  2,  9,  2,  1, -1, -1, -1, -1},
  { 3, 10,  1,  3, 11, 10,  7,  8,  4, -1, -1, -1, -1, -1, -1, -1},
  { 1, 11, 10,  1,  4, 11,  1,  0,  4,  7, 11,  4, -1, -1, -1, -1},
  { 4,  7,  8,  9,  0, 11,  9, 11, 10, 11,  0,  3, -1, -1, -1, -1},
  { 4,  7, 11,  4, 11,  9,  9, 11, 10, -1, -1, -1, -1, -1, -1, -1},
  { 9,  5,  4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 9,  5,  4,  0,  8,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  5,  4,  1,  5,  0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 8,  5,  4,  8,  3,  5,  3,  1,  5, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 10,  9,  5,  4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 3,  0,  8,  1,  2, 10,  4,  9,  5, -1, -1, -1, -1, -1, -1, -1},
  { 5,  2, 10,  5,  4,  2,  4,  0,  2, -1, -1, -1, -1, -1, -1, -1},
  { 2, 10,  5,  3,  2,  5,  3,  5,  4,  3,  4,  8, -1, -1, -1, -1},
  { 9,  5,  4,  2,  3, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0, 11,  2,  0,  8, 11,  4,  9,  5, -1, -1, -1, -1, -1, -1, -1},
  { 0,  5,  4,  0,  1,  5,  2,  3, 11, -1, -1, -1, -1, -1, -1, -1},
  { 2,  1,  5,  2,  5,  8,  2,  8, 11,  4,  8,  5, -1, -1, -1, -1},
  {10,  3, 11, 10,  1,  3,  9,  5,  4, -1, -1, -1, -1, -1, -1, -1},
  { 4,  9,  5,  0,  8,  1,  8, 10,  1,  8, 11, 10, -1, -1, -1, -1},
  { 5,  4,  0,  5,  0, 11,  5, 11, 10, 11,  0,  3, -1, -1, -1, -1},
  { 5,  4,  8,  5,  8, 10, 10,  8, 11, -1, -1, -1, -1, -1, -1, -1},
  { 9,  7,  8,  5,  7,  9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 9,  3,  0,  9,  5,  3,  5,  7,  3, -1, -1, -1, -1, -1, -1, -1},
  { 0,  7,  8,  0,  1,  7,  1,  5,  7, -1, -1, -1, -1, -1, -1, -1},
  { 1,  5,  3,  3,  5,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 9,  7,  8,  9,  5,  7, 10,  1,  2, -1, -1, -1, -1, -1, -1, -1},
  {10,  1,  2,  9,  5,  0,  5,  3,  0,  5,  7,  3, -1, -1, -1, -1},
  { 8,  0,  2,  8,  2,  5,  8,  5,  7, 10,  5,  2, -1, -1, -1, -1},
  { 2, 10,  5,  2,  5,  3,  3,  5,  7, -1, -1, -1, -1, -1, -1, -1},
  { 7,  9,  5,  7,  8,  9,  3, 11,  2, -1, -1, -1, -1, -1, -1, -1},
  { 9,  5,  7,  9,  7,  2,  9,  2,  0,  2,  7, 11, -1, -1, -1, -1},
  { 2,  3, 11,  0,  1,  8,  1,  7,  8,  1,  5,  7, -1, -1, -1, -1},
  {11,  2,  1, 11,  1,  7,  7,  1,  5, -1, -1, -1, -1, -1, -1, -1},
  { 9,  5,  8,  8,  5,  7, 10,  1,  3, 10,  3, 11, -1, -1, -1, -1},
  { 5,  7,  0,  5,  0,  9,  7, 11,  0,  1,  0, 10, 11, 10,  0, -1},
  {11, 10,  0, 11,  0,  3, 10,  5,  0,  8,  0,  7,  5,  7,  0, -1},
  {11, 10,  5,  7, 11,  5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {10,  6,  5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  3,  5, 10,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 9,  0,  1,  5, 10,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  8,  3,  1,  9,  8,  5, 10,  6, -1, -1, -1, -1, -1, -1, -1},
  { 1,  6,  5,  2,  6,  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  6,  5,  1,  2,  6,  3,  0,  8, -1, -1, -1, -1, -1, -1, -1},
  { 9,  6,  5,  9,  0,  6,  0,  2,  6, -1, -1, -1, -1, -1, -1, -1},
  { 5,  9,  8,  5,  8,  2,  5,  2,  6,  3,  2,  8, -1, -1, -1, -1},
  { 2,  3, 11, 10,  6,  5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {11,  0,  8, 11,  2,  0, 10,  6,  5, -1, -1, -1, -1, -1, -1, -1},
  { 0,  1,  9,  2,  3, 11,  5, 10,  6, -1, -1, -1, -1, -1, -1, -1},
  { 5, 10,  6,  1,  9,  2,  9, 11,  2,  9,  8, 11, -1, -1, -1, -1},
  { 6,  3, 11,  6,  5,  3,  5,  1,  3, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8, 11,  0, 11,  5,  0,  5,  1,  5, 11,  6, -1, -1, -1, -1},
  { 3, 11,  6,  0,  3,  6,  0,  6,  5,  0,  5,  9, -1, -1, -1, -1},
  { 6,  5,  9,  6,  9, 11, 11,  9,  8, -1, -1, -1, -1, -1, -1, -1},
  { 5, 10,  6,  4,  7,  8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  3,  0,  4,  7,  3,  6,  5, 10, -1, -1, -1, -1, -1, -1, -1},
  { 1,  9,  0,  5, 10,  6,  8,  4,  7, -1, -1, -1, -1, -1, -1, -1},
  {10,  6,  5,  1,  9,  7,  1,  7,  3,  7,  9,  4, -1, -1, -1, -1},
  { 6,  1,  2,  6,  5,  1,  4,  7,  8, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2,  5,  5,  2,  6,  3,  0,  4,  3,  4,  7, -1, -1, -1, -1},
  { 8,  4,  7,  9,  0,  5,  0,  6,  5,  0,  2,  6, -1, -1, -1, -1},
  { 7,  3,  9,  7,  9,  4,  3,  2,  9,  5,  9,  6,  2,  6,  9, -1},
  { 3, 11,  2,  7,  8,  4, 10,  6,  5, -1, -1, -1, -1, -1, -1, -1},
  { 5, 10,  6,  4,  7,  2,  4,  2,  0,  2,  7, 11, -1, -1, -1, -1},
  { 0,  1,  9,  4,  7,  8,  2,  3, 11,  5, 10,  6, -1, -1, -1, -1},
  { 9,  2,  1,  9, 11,  2,  9,  4, 11,  7, 11,  4,  5, 10,  6, -1},
  { 8,  4,  7,  3, 11,  5,  3,  5,  1,  5, 11,  6, -1, -1, -1, -1},
  { 5,  1, 11,  5, 11,  6,  1,  0, 11,  7, 11,  4,  0,  4, 11, -1},
  { 0,  5,  9,  0,  6,  5,  0,  3,  6, 11,  6,  3,  8,  4,  7, -1},
  { 6,  5,  9,  6,  9, 11,  4,  7,  9,  7, 11,  9, -1, -1, -1, -1},
  {10,  4,  9,  6,  4, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4, 10,  6,  4,  9, 10,  0,  8,  3, -1, -1, -1, -1, -1, -1, -1},
  {10,  0,  1, 10,  6,  0,  6,  4,  0, -1, -1, -1, -1, -1, -1, -1},
  { 8,  3,  1,  8,  1,  6,  8,  6,  4,  6,  1, 10, -1, -1, -1, -1},
  { 1,  4,  9,  1,  2,  4,  2,  6,  4, -1, -1, -1, -1, -1, -1, -1},
  { 3,  0,  8,  1,  2,  9,  2,  4,  9,  2,  6,  4, -1, -1, -1, -1},
  { 0,  2,  4,  4,  2,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 8,  3,  2,  8,  2,  4,  4,  2,  6, -1, -1, -1, -1, -1, -1, -1},
  {10,  4,  9, 10,  6,  4, 11,  2,  3, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  2,  2,  8, 11,  4,  9, 10,  4, 10,  6, -1, -1, -1, -1},
  { 3, 11,  2,  0,  1,  6,  0,  6,  4,  6,  1, 10, -1, -1, -1, -1},
  { 6,  4,  1,  6,  1, 10,  4,  8,  1,  2,  1, 11,  8, 11,  1, -1},
  { 9,  6,  4,  9,  3,  6,  9,  1,  3, 11,  6,  3, -1, -1, -1, -1},
  { 8, 11,  1,  8,  1,  0, 11,  6,  1,  9,  1,  4,  6,  4,  1, -1},
  { 3, 11,  6,  3,  6,  0,  0,  6,  4, -1, -1, -1, -1, -1, -1, -1},
  { 6,  4,  8, 11,  6,  8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 7, 10,  6,  7,  8, 10,  8,  9, 10, -1, -1, -1, -1, -1, -1, -1},
  { 0,  7,  3,  0, 10,  7,  0,  9, 10,  6,  7, 10, -1, -1, -1, -1},
  {10,  6,  7,  1, 10,  7,  1,  7,  8,  1,  8,  0, -1, -1, -1, -1},
  {10,  6,  7, 10,  7,  1,  1,  7,  3, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2,  6,  1,  6,  8,  1,  8,  9,  8,  6,  7, -1, -1, -1, -1},
  { 2,  6,  9,  2,  9,  1,  6,  7,  9,  0,  9,  3,  7,  3,  9, -1},
  { 7,  8,  0,  7,  0,  6,  6,  0,  2, -1, -1, -1, -1, -1, -1, -1},
  { 7,  3,  2,  6,  7,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 2,  3, 11, 10,  6,  8, 10,  8,  9,  8,  6,  7, -1, -1, -1, -1},
  { 2,  0,  7,  2,  7, 11,  0,  9,  7,  6,  7, 10,  9, 10,  7, -1},
  { 1,  8,  0,  1,  7,  8,  1, 10,  7,  6,  7, 10,  2,  3, 11, -1},
  {11,  2,  1, 11,  1,  7, 10,  6,  1,  6,  7,  1, -1, -1, -1, -1},
  { 8,  9,  6,  8,  6,  7,  9,  1,  6, 11,  6,  3,  1,  3,  6, -1},
  { 0,  9,  1, 11,  6,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 7,  8,  0,  7,  0,  6,  3, 11,  0, 11,  6,  0, -1, -1, -1, -1},
  { 7, 11,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 7,  6, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 3,  0,  8, 11,  7,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  1,  9, 11,  7,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 8,  1,  9,  8,  3,  1, 11,  7,  6, -1, -1, -1, -1, -1, -1, -1},
  {10,  1,  2,  6, 11,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 10,  3,  0,  8,  6, 11,  7, -1, -1, -1, -1, -1, -1, -1},
  { 2,  9,  0,  2, 10,  9,  6, 11,  7, -1, -1, -1, -1, -1, -1, -1},
  { 6, 11,  7,  2, 10,  3, 10,  8,  3, 10,  9,  8, -1, -1, -1, -1},
  { 7,  2,  3,  6,  2,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 7,  0,  8,  7,  6,  0,  6,  2,  0, -1, -1, -1, -1, -1, -1, -1},
  { 2,  7,  6,  2,  3,  7,  0,  1,  9, -1, -1, -1, -1, -1, -1, -1},
  { 1,  6,  2,  1,  8,  6,  1,  9,  8,  8,  7,  6, -1, -1, -1, -1},
  {10,  7,  6, 10,  1,  7,  1,  3,  7, -1, -1, -1, -1, -1, -1, -1},
  {10,  7,  6,  1,  7, 10,  1,  8,  7,  1,  0,  8, -1, -1, -1, -1},
  { 0,  3,  7,  0,  7, 10,  0, 10,  9,  6, 10,  7, -1, -1, -1, -1},
  { 7,  6, 10,  7, 10,  8,  8, 10,  9, -1, -1, -1, -1, -1, -1, -1},
  { 6,  8,  4, 11,  8,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 3,  6, 11,  3,  0,  6,  0,  4,  6, -1, -1, -1, -1, -1, -1, -1},
  { 8,  6, 11,  8,  4,  6,  9,  0,  1, -1, -1, -1, -1, -1, -1, -1},
  { 9,  4,  6,  9,  6,  3,  9,  3,  1, 11,  3,  6, -1, -1, -1, -1},
  { 6,  8,  4,  6, 11,  8,  2, 10,  1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 10,  3,  0, 11,  0,  6, 11,  0,  4,  6, -1, -1, -1, -1},
  { 4, 11,  8,  4,  6, 11,  0,  2,  9,  2, 10,  9, -1, -1, -1, -1},
  {10,  9,  3, 10,  3,  2,  9,  4,  3, 11,  3,  6,  4,  6,  3, -1},
  { 8,  2,  3,  8,  4,  2,  4,  6,  2, -1, -1, -1, -1, -1, -1, -1},
  { 0,  4,  2,  4,  6,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  9,  0,  2,  3,  4,  2,  4,  6,  4,  3,  8, -1, -1, -1, -1},
  { 1,  9,  4,  1,  4,  2,  2,  4,  6, -1, -1, -1, -1, -1, -1, -1},
  { 8,  1,  3,  8,  6,  1,  8,  4,  6,  6, 10,  1, -1, -1, -1, -1},
  {10,  1,  0, 10,  0,  6,  6,  0,  4, -1, -1, -1, -1, -1, -1, -1},
  { 4,  6,  3,  4,  3,  8,  6, 10,  3,  0,  3,  9, 10,  9,  3, -1},
  {10,  9,  4,  6, 10,  4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  9,  5,  7,  6, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  3,  4,  9,  5, 11,  7,  6, -1, -1, -1, -1, -1, -1, -1},
  { 5,  0,  1,  5,  4,  0,  7,  6, 11, -1, -1, -1, -1, -1, -1, -1},
  {11,  7,  6,  8,  3,  4,  3,  5,  4,  3,  1,  5, -1, -1, -1, -1},
  { 9,  5,  4, 10,  1,  2,  7,  6, 11, -1, -1, -1, -1, -1, -1, -1},
  { 6, 11,  7,  1,  2, 10,  0,  8,  3,  4,  9,  5, -1, -1, -1, -1},
  { 7,  6, 11,  5,  4, 10,  4,  2, 10,  4,  0,  2, -1, -1, -1, -1},
  { 3,  4,  8,  3,  5,  4,  3,  2,  5, 10,  5,  2, 11,  7,  6, -1},
  { 7,  2,  3,  7,  6,  2,  5,  4,  9, -1, -1, -1, -1, -1, -1, -1},
  { 9,  5,  4,  0,  8,  6,  0,  6,  2,  6,  8,  7, -1, -1, -1, -1},
  { 3,  6,  2,  3,  7,  6,  1,  5,  0,  5,  4,  0, -1, -1, -1, -1},
  { 6,  2,  8,  6,  8,  7,  2,  1,  8,  4,  8,  5,  1,  5,  8, -1},
  { 9,  5,  4, 10,  1,  6,  1,  7,  6,  1,  3,  7, -1, -1, -1, -1},
  { 1,  6, 10,  1,  7,  6,  1,  0,  7,  8,  7,  0,  9,  5,  4, -1},
  { 4,  0, 10,  4, 10,  5,  0,  3, 10,  6, 10,  7,  3,  7, 10, -1},
  { 7,  6, 10,  7, 10,  8,  5,  4, 10,  4,  8, 10, -1, -1, -1, -1},
  { 6,  9,  5,  6, 11,  9, 11,  8,  9, -1, -1, -1, -1, -1, -1, -1},
  { 3,  6, 11,  0,  6,  3,  0,  5,  6,  0,  9,  5, -1, -1, -1, -1},
  { 0, 11,  8,  0,  5, 11,  0,  1,  5,  5,  6, 11, -1, -1, -1, -1},
  { 6, 11,  3,  6,  3,  5,  5,  3,  1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 10,  9,  5, 11,  9, 11,  8, 11,  5,  6, -1, -1, -1, -1},
  { 0, 11,  3,  0,  6, 11,  0,  9,  6,  5,  6,  9,  1,  2, 10, -1},
  {11,  8,  5, 11,  5,  6,  8,  0,  5, 10,  5,  2,  0,  2,  5, -1},
  { 6, 11,  3,  6,  3,  5,  2, 10,  3, 10,  5,  3, -1, -1, -1, -1},
  { 5,  8,  9,  5,  2,  8,  5,  6,  2,  3,  8,  2, -1, -1, -1, -1},
  { 9,  5,  6,  9,  6,  0,  0,  6,  2, -1, -1, -1, -1, -1, -1, -1},
  { 1,  5,  8,  1,  8,  0,  5,  6,  8,  3,  8,  2,  6,  2,  8, -1},
  { 1,  5,  6,  2,  1,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  3,  6,  1,  6, 10,  3,  8,  6,  5,  6,  9,  8,  9,  6, -1},
  {10,  1,  0, 10,  0,  6,  9,  5,  0,  5,  6,  0, -1, -1, -1, -1},
  { 0,  3,  8,  5,  6, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {10,  5,  6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {11,  5, 10,  7,  5, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {11,  5, 10, 11,  7,  5,  8,  3,  0, -1, -1, -1, -1, -1, -1, -1},
  { 5, 11,  7,  5, 10, 11,  1,  9,  0, -1, -1, -1, -1, -1, -1, -1},
  {10,  7,  5, 10, 11,  7,  9,  8,  1,  8,  3,  1, -1, -1, -1, -1},
  {11,  1,  2, 11,  7,  1,  7,  5,  1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  3,  1,  2,  7,  1,  7,  5,  7,  2, 11, -1, -1, -1, -1},
  { 9,  7,  5,  9,  2,  7,  9,  0,  2,  2, 11,  7, -1, -1, -1, -1},
  { 7,  5,  2,  7,  2, 11,  5,  9,  2,  3,  2,  8,  9,  8,  2, -1},
  { 2,  5, 10,  2,  3,  5,  3,  7,  5, -1, -1, -1, -1, -1, -1, -1},
  { 8,  2,  0,  8,  5,  2,  8,  7,  5, 10,  2,  5, -1, -1, -1, -1},
  { 9,  0,  1,  5, 10,  3,  5,  3,  7,  3, 10,  2, -1, -1, -1, -1},
  { 9,  8,  2,  9,  2,  1,  8,  7,  2, 10,  2,  5,  7,  5,  2, -1},
  { 1,  3,  5,  3,  7,  5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  7,  0,  7,  1,  1,  7,  5, -1, -1, -1, -1, -1, -1, -1},
  { 9,  0,  3,  9,  3,  5,  5,  3,  7, -1, -1, -1, -1, -1, -1, -1},
  { 9,  8,  7,  5,  9,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 5,  8,  4,  5, 10,  8, 10, 11,  8, -1, -1, -1, -1, -1, -1, -1},
  { 5,  0,  4,  5, 11,  0,  5, 10, 11, 11,  3,  0, -1, -1, -1, -1},
  { 0,  1,  9,  8,  4, 10,  8, 10, 11, 10,  4,  5, -1, -1, -1, -1},
  {10, 11,  4, 10,  4,  5, 11,  3,  4,  9,  4,  1,  3,  1,  4, -1},
  { 2,  5,  1,  2,  8,  5,  2, 11,  8,  4,  5,  8, -1, -1, -1, -1},
  { 0,  4, 11,  0, 11,  3,  4,  5, 11,  2, 11,  1,  5,  1, 11, -1},
  { 0,  2,  5,  0,  5,  9,  2, 11,  5,  4,  5,  8, 11,  8,  5, -1},
  { 9,  4,  5,  2, 11,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 2,  5, 10,  3,  5,  2,  3,  4,  5,  3,  8,  4, -1, -1, -1, -1},
  { 5, 10,  2,  5,  2,  4,  4,  2,  0, -1, -1, -1, -1, -1, -1, -1},
  { 3, 10,  2,  3,  5, 10,  3,  8,  5,  4,  5,  8,  0,  1,  9, -1},
  { 5, 10,  2,  5,  2,  4,  1,  9,  2,  9,  4,  2, -1, -1, -1, -1},
  { 8,  4,  5,  8,  5,  3,  3,  5,  1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  4,  5,  1,  0,  5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 8,  4,  5,  8,  5,  3,  9,  0,  5,  0,  3,  5, -1, -1, -1, -1},
  { 9,  4,  5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4, 11,  7,  4,  9, 11,  9, 10, 11, -1, -1, -1, -1, -1, -1, -1},
  { 0,  8,  3,  4,  9,  7,  9, 11,  7,  9, 10, 11, -1, -1, -1, -1},
  { 1, 10, 11,  1, 11,  4,  1,  4,  0,  7,  4, 11, -1, -1, -1, -1},
  { 3,  1,  4,  3,  4,  8,  1, 10,  4,  7,  4, 11, 10, 11,  4, -1},
  { 4, 11,  7,  9, 11,  4,  9,  2, 11,  9,  1,  2, -1, -1, -1, -1},
  { 9,  7,  4,  9, 11,  7,  9,  1, 11,  2, 11,  1,  0,  8,  3, -1},
  {11,  7,  4, 11,  4,  2,  2,  4,  0, -1, -1, -1, -1, -1, -1, -1},
  {11,  7,  4, 11,  4,  2,  8,  3,  4,  3,  2,  4, -1, -1, -1, -1},
  { 2,  9, 10,  2,  7,  9,  2,  3,  7,  7,  4,  9, -1, -1, -1, -1},
  { 9, 10,  7,  9,  7,  4, 10,  2,  7,  8,  7,  0,  2,  0,  7, -1},
  { 3,  7, 10,  3, 10,  2,  7,  4, 10,  1, 10,  0,  4,  0, 10, -1},
  { 1, 10,  2,  8,  7,  4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  9,  1,  4,  1,  7,  7,  1,  3, -1, -1, -1, -1, -1, -1, -1},
  { 4,  9,  1,  4,  1,  7,  0,  8,  1,  8,  7,  1, -1, -1, -1, -1},
  { 4,  0,  3,  7,  4,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 4,  8,  7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 9, 10,  8, 10, 11,  8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 3,  0,  9,  3,  9, 11, 11,  9, 10, -1, -1, -1, -1, -1, -1, -1},
  { 0,  1, 10,  0, 10,  8,  8, 10, 11, -1, -1, -1, -1, -1, -1, -1},
  { 3,  1, 10, 11,  3, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  2, 11,  1, 11,  9,  9, 11,  8, -1, -1, -1, -1, -1, -1, -1},
  { 3,  0,  9,  3,  9, 11,  1,  2,  9,  2, 11,  9, -1, -1, -1, -1},
  { 0,  2, 11,  8,  0, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 3,  2, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 2,  3,  8,  2,  8, 10, 10,  8,  9, -1, -1, -1, -1, -1, -1, -1},
  { 9, 10,  2,  0,  9,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 2,  3,  8,  2,  8, 10,  0,  1,  8,  1, 10,  8, -1, -1, -1, -1},
  { 1, 10,  2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 1,  3,  8,  9,  1,  8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  9,  1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  { 0,  3,  8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
  {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}
};

// offsets used to determine which vertex of a cube has a value >= to the
// isoval.
static const int cubeVertOffset[8][3] = {
  { 0,  0,  0},
  {-1,  0,  0},
  {-1, -1,  0},
  { 0, -1,  0},
  { 0,  0, -1},
  {-1,  0, -1},
  {-1, -1, -1},
  { 0, -1, -1}
};

// Inventor stuff
SO_ENGINE_SOURCE(MarchingCubes);

// Inventor stuff
void
MarchingCubes::initClass ()
{
  SO_ENGINE_INIT_CLASS(MarchingCubes, SoEngine, "Engine");
}

//////////////////////////////////////////////////////////////////////////////
//
//  Description:
//    Default constructor initializes all class fields
//
//////////////////////////////////////////////////////////////////////////////
MarchingCubes::MarchingCubes ()
{
  SO_ENGINE_CONSTRUCTOR(MarchingCubes);

  dims[0] = dims[1] = dims[2] = 0;
  SO_ENGINE_ADD_INPUT(data,               (dims, 0, false));
  SO_ENGINE_ADD_INPUT(isoValue,                      (1.0));
  SO_ENGINE_ADD_INPUT(blendPercent, (MC_MAX_BLEND_PERCENT));

  SO_ENGINE_ADD_OUTPUT(points,                   SoMFVec3f);
  SO_ENGINE_ADD_OUTPUT(normals,                  SoMFVec3f);
  SO_ENGINE_ADD_OUTPUT(indexes,                  SoMFInt32);
  SO_ENGINE_ADD_OUTPUT(numTriangles,             SoSFFloat);

  computeGradients = true;

  // surface will be created with coordinates between 0 and 1, making it
  // easier to translate and scale
  min = SbVec3f(0.0, 0.0, 0.0);
  max = SbVec3f(1.0, 1.0, 1.0);
}

MarchingCubes::~MarchingCubes ()
{

}

//////////////////////////////////////////////////////////////////////////////
//
//  Description:
//    Whenever the data field changes set the flag to recompute the
//    gradients.
//
//////////////////////////////////////////////////////////////////////////////
void
MarchingCubes::inputChanged (SoField *which)
{
  if (which == &data)
    computeGradients = true;

}

//////////////////////////////////////////////////////////////////////////////
//
//  Description:
//    Called by Inventor each time an output has been called and an input has
//    been modified.  The points and the indexes will be computed here.
//
//////////////////////////////////////////////////////////////////////////////
void
MarchingCubes::evaluate ()
{

  //currVert  = 0;

  // reset the arrays holding the vertices and indexes before they are
  // modified.
  verts.setNum(0);
  norms.setNum(0);
  inds.setNum(0);

  // grab the current value of isoValue so that only one value will be used
  // during the computation.
  isoval = isoValue.getValue();
  bperc  = fabs(blendPercent.getValue());  
  bperc  = (bperc < 0.5) ? bperc : MC_MAX_BLEND_PERCENT;

  // calculate the gradient at each grid point, this is called whenever the
  // data has been modified.
  if (computeGradients) {
    // get the latest values
    values = data.getValue(dims);

    // compute the size of each voxel (dependent on the dimensions of the
    // data)
    SbVec3f size = max - min;
    voxel[0] = (dims[0] > 1) ? size[0] / (float)(dims[0] - 1) : 0.0;
    voxel[1] = (dims[1] > 1) ? size[1] / (float)(dims[1] - 1) : 0.0;
    voxel[2] = (dims[2] > 1) ? size[2] / (float)(dims[2] - 1) : 0.0;

    // there are actually more than the standard (nx - 1)*(ny - 1)*(nz - 1)
    // number of cubes for the dataset.  To speed computation up by
    // eliminating constant index bounds checking, a cube buffer is placed
    // around the actual cubes being used.  This way if the indexing goes one
    // under zero or over the max it won't core dump :)
    cubeDims[0] = dims[0] + 1;
    cubeDims[1] = dims[1] + 1;
    cubeDims[2] = dims[2] + 1;

    // set the total number of cubes
    cubes.setNum(cubeDims[0] * cubeDims[1] * cubeDims[2]);
    // set the total number of edges being looked at.
    edgeIndex.setNum(3 * dims[0] * dims[1] * dims[2]);

    // calculate the gradients at each grid point. these gradients will be
    // used to interpolate the values at the surface points.
    calcGradients();
    computeGradients = false;
  }

  // calculate the points on the surface
  calcPoints();
  // create the triangle triples defining the triangles of the surface
  calcIndexes();

  // send the results to the connected fields
  SO_ENGINE_OUTPUT(points, SoMFVec3f,
		   setValues(0, verts.getNum(), verts.getValues(0)));

  SO_ENGINE_OUTPUT(normals, SoMFVec3f,
		   setValues(0, norms.getNum(), norms.getValues(0)));

  int i = inds.getNum();
  SO_ENGINE_OUTPUT(indexes, SoMFInt32, setNum(i));
  SO_ENGINE_OUTPUT(indexes, SoMFInt32, setValues(0, i, inds.getValues(0)));

  SO_ENGINE_OUTPUT(numTriangles, SoSFFloat, setValue(triNum));
}

//////////////////////////////////////////////////////////////////////////////
//
//  Description:
//    To avoid calculating points multiple times a "Marching Edges"
//    technique is used (this may as well be called something else by someone
//    else, I do not know.  I did not research on this technique, just
//    thought of it one day while waiting in an airport.)   Instead of
//    searching each cube of data one by one and accessing 8 different points
//    at once only 2 are looked at each time, and less assigments are made.
//    The two pulled out make up an edge on a cube.  If the isoValue is
//    inbetween the two edge values then a point is computed for the
//    surface.  To eliminate additional calls to the same data point the
//    order of operation is done in the following manner...
//                                     
//                V2                     
//                |   V1                
//               y|  /                
//                | / z                
//                |/              
//                V0-------V3
//                     x
//
//    V0 and V1 are checked,
//    V0 and V2 are checked,
//    V0 and V3 are checked
//
//    Every time a point is added to the 'verts' array its index is stored in
//    the 'edgeIndex' array.  This way when building the triangles with the
//    Marching Cubes tables the proper edge can point to the proper point.
//
//////////////////////////////////////////////////////////////////////////////
void
MarchingCubes::calcPoints ()
{
  int i, j, k, ci, cj, ck, cv;
    int currCube, currEdge, currVert, lastVert;
  int offset[3], checkDims[3];
  int cyOffset, czOffset;
  int32_t *cptr, *eptr;
  int v1, v2, index;
  float delta, percent;
  SbVec3f basePt, pt, norm;
  const SbVec3f *gptr;

    // the point blending: we are allocating the voxel structure
    //	in order to process as locate the blended points as possible later
    //	
    blend :: point *bTableP, *bPntP;
    bool nearCorner;
    
    bTableP = new blend :: point[ dims[0] * dims[1] * dims[2] ];
    
// add a point to the vertice list.  A point is added when it is determined
// that the isosurface goes through an edge.  Sets the normal as well.
// (accumulate if too close to the voxel corner except for the boundaries)
//
#define MC_ADD_POINT(comp, v1, v2)                                    		\
    {										\
	pt    = basePt;                                                     	\
	delta = values[v2] - values[v1];                                    	\
	percent = (delta == 0.0) ? 0.5 : (isoval - values[v1]) / delta;     	\
	pt[comp] += voxel[comp] * percent;                                  	\
	norm = gptr[v1] + (gptr[v2] - gptr[v1]) * percent;                  	\
	norm.normalize();                                                   	\
										\
	nearCorner = ( fabs( percent ) < bperc);                                \
	if ( i && j && k &&							\
	    i < checkDims[0] && j < checkDims[1] && k < checkDims[2] &&		\
	    ( nearCorner || ( fabs( 1.0f - percent ) < bperc ) ) )              \
	{									\
	    bPntP = bTableP + ((nearCorner) ? v1 : v2);				\
	    if ( bPntP->hits() )						\
	    {									\
		lastVert = bPntP->add( pt, norm );				\
	    }									\
	    else								\
	    {									\
		bPntP->set( currVert, pt, norm );				\
		lastVert = currVert;						\
		++ currVert;							\
	    }									\
	}									\
	else									\
	{									\
	    verts.set1Value(currVert, pt);					\
	    norms.set1Value(currVert, norm);                                    \
	    lastVert = currVert;						\
	    ++ currVert;							\
	}									\
	eptr[currEdge] = lastVert;						\
    }

// check an edge to see if v2 is > the isoval.  This is called
// when it is already known that the v1 is <= to the isoval
#define MC_CHECK_EDGE_1(comp)                                           \
  {								        \
    v2 = v1 + offset[comp];                                             \
    if (values[v2] > isoval) {                                          \
      MC_ADD_POINT(comp, v1, v2);                                       \
    }                                                                   \
  } 								        \
  currEdge++;

// check an edge to see if v2 is <= to the isoval.  This is called when it
// is already know that v1 is > than the isoval.
#define MC_CHECK_EDGE_2(comp)                                           \
  {								        \
    v2 = v1 + offset[comp];                                             \
    if (values[v2] <= isoval) {                                         \
      MC_ADD_POINT(comp, v1, v2);                                       \
    }                                                                   \
  } 								        \
  currEdge++;


  // set offsets for each direction
  offset[0] = 1;
  offset[1] = dims[0];
  offset[2] = dims[0]*dims[1];

  // just a temporary variable to avoid checking (dims[x] - 1) against a
  // number, the less CPU cycles the better.
  checkDims[0] = dims[0] - 1;
  checkDims[1] = dims[1] - 1;
  checkDims[2] = dims[2] - 1;

  // set offsets for each direction when computing the index of surrounding
  // cubes  
  cyOffset  = cubeDims[0];
  czOffset  = cubeDims[0] * cubeDims[1];

  // initialize all variables
  gptr = gradients.getValues(0);
  eptr = edgeIndex.startEditing();
  cptr = cubes.startEditing();
  memset(cptr, 0, sizeof(int32_t)*cubeDims[0]*cubeDims[1]*cubeDims[2]);
  for (i = 0; i < dims[0]*dims[1]*dims[2]; i++)
    eptr[i] = -1;

  currCube = 0;
  currVert = 0;
  currEdge = 0;
  v1       = 0;

  // for each vertice, check the edges it is connected to in the positive
  // x,y,z direction.  
  for (k = 0, ck = 1; k < dims[2]; k++, ck++) {
    basePt[2] = min[2] + k*voxel[2];
    for (j = 0, cj = 1; j < dims[1]; j++, cj++) {
      basePt[1] = min[1] + j*voxel[1];
      for (i = 0, ci = 1; i < dims[0]; i++, ci++) {
        basePt[0] = min[0] + i*voxel[0];

	// most of the time all edges in each direction will be checked
        if (k < checkDims[2] && j < checkDims[1] && i < checkDims[0]) {

	  // if the value at data point 'v1' is <= isoval then the cubes that
	  // contain the point as a vertice need to have the bits toggled.
	  if (values[v1] <= isoval) {
	    // compute index of the current cube, this is always the cube
	    // where 'v1' is vertex 0
	    currCube = ck*czOffset + cj*cyOffset + ci;
	    for (cv = 0; cv < 8; cv++) {
	      index = currCube + cubeVertOffset[cv][2]*czOffset +
	                         cubeVertOffset[cv][1]*cyOffset +
	                         cubeVertOffset[cv][0];
	      cptr[index] |= (1 << cv);
	    }
	  
	    MC_CHECK_EDGE_1(2); 
	    MC_CHECK_EDGE_1(1);
	    MC_CHECK_EDGE_1(0);
	  }
	  else {
	    MC_CHECK_EDGE_2(2);
	    MC_CHECK_EDGE_2(1);
	    MC_CHECK_EDGE_2(0);
	  }
	}
	else {
	  if (values[v1] <= isoval) {

	    currCube = ck*czOffset + cj*cyOffset + ci;
	    for (cv = 0; cv < 8; cv++) {
	      index = currCube + cubeVertOffset[cv][2]*czOffset +
	                         cubeVertOffset[cv][1]*cyOffset +
	                         cubeVertOffset[cv][0];
	      cptr[index] |= (1 << cv);
	    }
	    
	    if (k < checkDims[2])
	      MC_CHECK_EDGE_1(2);
	    if (j < checkDims[1])
	      MC_CHECK_EDGE_1(1);
	    if (i < checkDims[0])
	      MC_CHECK_EDGE_1(0);
	  }
	  else {
	    if (k < checkDims[2])
	      MC_CHECK_EDGE_2(2);
	    if (j < checkDims[1])
	      MC_CHECK_EDGE_2(1);
	    if (i < checkDims[0])
	      MC_CHECK_EDGE_2(0);
	  }
	}
	  
	v1++;
      }
    }
  }

    bPntP = bTableP;
    for (k = 0; k < dims[2]; ++k)
    {
	for (j = 0; j < dims[1]; ++j)
	{
	    for (i = 0; i < dims[0]; ++i, ++ bPntP)
	    {
		if ( bPntP->hits() )
		{
		    verts.set1Value(bPntP->vertexIndex(), bPntP->location());
		    norms.set1Value(bPntP->vertexIndex(), bPntP->normal());
		}
	    }
	}
    }
    
    delete [] bTableP;
}

//////////////////////////////////////////////////////////////////////////////
//
//  Description:
//    Create the index triples which will define the triangles used to create
//    the surface.  This is done by iterating through each cube in 'cubes' (not
//    including the buffer cubes) and using the value as an index into the
//    'triTable' array.  The 'triTable' array is used to determine which
//    edges the surface is intersecting.  Based on this information the edge
//    index is computed and then the index of the point is found.
//
//////////////////////////////////////////////////////////////////////////////
void
MarchingCubes::calcIndexes ()
{
  int c, e, i, j, k, t;
  int currIndex;
  int v1, v2, v3;
  const int32_t *cptr, *eptr;

  // these offsets are used to index into the 'edgeIndex' array.  The edges
  // of a cube according to the Marching Cubes table 'triTable' are different
  // then how the edges are stored in the 'edgeIndex' array.  The offset of
  // an edge is found by indexing into the array by the edge number (0-11).
  // Notice these offsets are a function of the dimensions of the data.
  const int32_t offset[12] = {
    2,
    4,
    3*dims[0] + 2,
    1,
    3*dims[1]*dims[0] + 2,
    3*dims[1]*dims[0] + 4,
    3*dims[1]*dims[0] + 3*dims[0] + 2,
    3*dims[1]*dims[0] + 1,
    0,
    3,
    3*dims[0] + 3,
    3*dims[0]
  };

  // get pointers to the cubes and edges
  cptr = cubes.getValues(0);
  eptr = edgeIndex.getValues(0);
  
  e = currIndex = triNum = 0;
  // for each cube use the value as an index into 'triTable' and determine if
  // the cube should have any triangles in it.
  for (k = 1; k < cubeDims[2] - 1; k++) {
    for (j = 1; j < cubeDims[1] - 1; j++) {
      for (i = 1; i < cubeDims[0] - 1; i++) {

	// create the cube index
	c = cubeDims[0] * (k*cubeDims[1] + j) + i;
	// at most there will be 5 triangles in a cube, cycle through until a
	// -1 is found (signifying no more triangles)
	for (t = 0; t < 15; t += 3) {
	  if (triTable[cptr[c]][t] < 0)
	    break;

	  // extract the indexes by...
	  //   - look up triangle combination in 'triTable'
	  //   - use 't' to index into 'triTable' and determine which edges
	  //       make up the triangle
	  //   - use edge value as an index into the 'offset' array
	  //   - add offset to the current edge (always the lower left edge
	  //     of the current cube pointing in the z-direction)
	  //   - index into the 'edgeIndex' array and get the index of the
	  //     point corresponding to that edge.
	  v1 = eptr[offset[triTable[cptr[c]][t + 0]] + e];
	  v2 = eptr[offset[triTable[cptr[c]][t + 1]] + e];
	  v3 = eptr[offset[triTable[cptr[c]][t + 2]] + e];

	  // the blending of vertices may have edges pointing to the same
	  // vertex, this would be a waste of a triangle so it is ignored.
	  if ( v1 == v2 || v2 == v3 || v1 == v3 ) continue;
	  
	  // just incase something went wrong a check can be made
	  //if (v1 < 0 || v2 < 0 || v3 < 0) {
	  //  cout << "no point " << v1 << " " << v2 << " " << v3 << endl;
	  //  continue;
	  //}

	  // set the values to form a triangle
	  inds.set1Value(currIndex++, v1);
	  inds.set1Value(currIndex++, v2);
	  inds.set1Value(currIndex++, v3);
	  inds.set1Value(currIndex++,  -1);

	  triNum++;
	}

	c++;
	e += 3;
      }
      e += 3;
    }
    e += 3*dims[0];
  }
}

//////////////////////////////////////////////////////////////////////////////
//
//  Description:
//    Computes the gradients at each grid point.  This is called only when
//    the data has been changed.  This way when computing normals at each
//    triangle vertex, tri-linear interpolation will be used to efficiently
//    calculate the values.
//
//////////////////////////////////////////////////////////////////////////////
void
MarchingCubes::calcGradients ()
{
  int i, j, k;
  int index, xOffset, yOffset, zOffset;
  SbVec3f *grad;

  // set the total number of gradient values
  gradients.setNum(dims[0] * dims[1] * dims[2]);

  // macro used to determine normal for a given component
#define MC_COMPONENT(c, i, offset)                                         \
  if (i == 0)                                                              \
    grad[index][c] = (values[index + offset] - values[index]) / voxel[c];  \
  else if (i == dims[c] - 1)                                               \
    grad[index][c] = (values[index] - values[index - offset]) / voxel[c];  \
  else                                                                     \
    grad[index][c] = (values[index + offset] - values[index - offset]) /   \
	             (2.0 * voxel[c]);

  // get pointer to start editing gradients
  grad    = gradients.startEditing();
  index   = 0;
  // set offsets for each direction
  xOffset = 1;
  yOffset = dims[0];
  zOffset = dims[0]*dims[1];
  
  for (k = 0; k < dims[2]; k++) {
    for (j = 0; j < dims[1]; j++) {
      for (i = 0; i < dims[0]; i++) {

	MC_COMPONENT(0, i, xOffset);
	MC_COMPONENT(1, j, yOffset);
	MC_COMPONENT(2, k, zOffset);

	grad[index].normalize();

	index++;
      }
    }
  }
  
  gradients.finishEditing();
}

  

  
	


