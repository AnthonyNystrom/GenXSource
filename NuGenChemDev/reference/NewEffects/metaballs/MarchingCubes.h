//////////////////////////////////////////////////////////////////////////////
//
//  Author      : Josh Grant
//  Date        : November 26th, 2001
//  File        : MarchingCubes.h
//  Description : Header file defining the MarchingCubes class
//
//////////////////////////////////////////////////////////////////////////////

#ifndef _MARCHING_CUBES_
#define _MARCHING_CUBES_

#include <Inventor/engines/SoSubEngine.h>
#include <Inventor/fields/SoSFFloat.h>
#include <Inventor/fields/SoMFVec3f.h>
#include <Inventor/fields/SoMFInt32.h>

#include "SFScalarField.h"

//////////////////////////////////////////////////////////////////////////////
//
//  Class: MarchingCubes
//
//  A subclass of the SoEngine class.  This node computes a surface using the
//  Marching Cubes algorithm developed by William Lorensen.  One of the input
//  fields is an SFScalarField which describes the data used by marching
//  cubes.
//
//  This particular implementation does not 'March' through the data cube by
//  cube, but rather edge by edge.  This is to ensure every point generated
//  is unique.  In addition, this accelerates the computation time by not
//  searching through the data more than once.
//
//  All tables were based off of code written by Cory Bloyd
//  (corysama@yahoo.com), for a nice description of the Marching
//  Cubes algorithm located at
//  http://www.swin.edu.au/astronomy/pbourke/modelling/polygonise/
//
//////////////////////////////////////////////////////////////////////////////

class MarchingCubes : public SoEngine {

  // Required Inventor macro
  SO_ENGINE_HEADER(MarchingCubes);

public:
  // scalar field data
  SFScalarField data;
  // isovalue to draw surface at
  SoSFFloat     isoValue;
  // percentage of edge to blend vertices together with [0.0, 0.5)
  SoSFFloat     blendPercent;

  SoEngineOutput  points;    // (SoMFVec3f)
  SoEngineOutput  normals;   // (SoMFVec3f)
  // every 4 values are used to describe a triangle, the first 3 are indexes
  // into the points field, and the 4th is always -1
  SoEngineOutput  indexes;   // (SoMFInt32)
  SoEngineOutput  numTriangles;  // (SoSFFloat)

  // Inventor stuff
  static void   initClass          ();

                MarchingCubes      ();

protected:
  virtual void  inputChanged (SoField *which);

private:
  virtual       ~MarchingCubes     ();

  // Inventor method, called when the engine is to be evaluated
  virtual void  evaluate ();

  // internal variables to hold data information instead of constantly
  // calling the getValue() routine from the class. The actual data is not
  // copied only a pointer to the data array.
  SbVec3f       min, max, voxel;
  float        *values, isoval, bperc;
  int           dims[3];
  
  // internal variable for storing the gradients at each grid point
  SoMFVec3f     gradients;

  // data structures for marching cubes, this implementation creates an
  // isosurface first by checking each edge to see if it intersects the
  // surface.  If so then the the point is created and stored in the 'verts'
  // field and the index of the point is stored in the 'edgeIndex' array.
  // The 'cubes' array holds the index into the Marching Cubes table for the
  // given cube.
  SoMFInt32     cubes, edgeIndex;
  int           cubeDims[3];
  int           triNum;

  // output buffer variables.  an output field cannot be set directly
  // (efficiently), it is more efficient to store everything in a separate
  // variable and write all data out at once when done.
  SoMFInt32     inds;
  SoMFVec3f     verts, norms;
  
  bool          computeGradients;

  // calculate all the points that will make up the surface
  void          calcPoints    ();
  // create the index triples to used to make up each triangle of the surface
  void          calcIndexes   ();
  // calculate the gradient at each grid point
  void          calcGradients ();

};

#endif /* _MARCHING_CUBES_ */
