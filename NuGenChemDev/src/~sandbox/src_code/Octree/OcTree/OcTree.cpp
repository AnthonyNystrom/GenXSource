#include "StdAfx.h"
#include "OcTree.h"

using namespace OcTree;
using namespace Microsoft::DirectX::Direct3D;

//
//// OcTree::Block
//

/// standard object services ---------------------------------------------------
Block::Block(unsigned int uid)
: uid_m(uid)
, position_m()
, dimensions_m()
{ }

Block::Block(unsigned int uid,
			 const Vector3f& position,
			 const Vector3f& origin,
			 const Vector3f& dimensions,
			 float radius)
: uid_m(uid)
, position_m (position)
, dimensions_m(dimensions)
, radius_m(radius)
, origin_m(origin)
{ }

Block::~Block() { }

Block::Block(const Block& other)
: position_m(other.position_m)
, dimensions_m(other.dimensions_m)
, uid_m(other.uid_m)
, radius_m(other.radius_m)
, origin_m(other.origin_m)
{ }

Block& Block::operator=(const Block& other)
{
	if(&other != this)
	{
		position_m = other.position_m;
		dimensions_m = other.dimensions_m;
	}
	return *this;
}

/// queries --------------------------------------------------------------------
const Vector3f& Block::getPosition() const
{
	return position_m;
}

const Vector3f& Block::getDimensions() const
{
	return dimensions_m;
}

const Vector3f& Block::getOrigin() const
{
	return origin_m;
}

float Block::getRadius() const
{
	return radius_m;
}

unsigned int Block::getUId() const
{
	return uid_m;
}


//
//// OcTree::OctreeAgentBlock
//

/// queries --------------------------------------------------------------------
bool OctreeAgentBlock::isOverlappingCell(const Block& item,
										 const Vector3f& lowerCorner,
										 const Vector3f& upperCorner) const
{
	const Vector3f& itemLower(item.getPosition());
	const Vector3f  itemUpper = item.getPosition() + item.getDimensions();

	/// check the two ranges overlap in every dimension
	return Vector3f::ONE() == (itemLower < upperCorner) &&
	       Vector3f::ONE() == (itemUpper > lowerCorner);
}

dword OctreeAgentBlock::getSubcellOverlaps(const Block& item,
										   const Vector3f& cellsLowerPos,
										   const Vector3f& cellsMiddlePos,
										   const Vector3f& cellsUpperPos) const
{
	/// efficiency could (probably) be improved by doing minimal necessary checks
	/// against the dividing bounds, instead of repeatedly delegating to
	/// isOverlappingCell().

	dword flags = 0;

	const Vector3f* lowMidPoints[] = { &cellsLowerPos, &cellsMiddlePos };
	const Vector3f* midHighPoints[] = { &cellsMiddlePos, &cellsUpperPos };

	for (dword i = 8; i-- > 0;)
	{
		Vector3f lowerCorner( lowMidPoints[ i       & 1]->getX(),
		                      lowMidPoints[(i >> 1) & 1]->getY(),
		                      lowMidPoints[(i >> 2) & 1]->getZ() );
		Vector3f upperCorner( midHighPoints[ i       & 1]->getX(),
		                      midHighPoints[(i >> 1) & 1]->getY(),
		                      midHighPoints[(i >> 2) & 1]->getZ() );
		flags |= dword(isOverlappingCell( item, lowerCorner, upperCorner )) << i;
	}
	return flags;
}

//
//// OcTree::ProximityOctreeVisitor
//

/// standard object services ---------------------------------------------------
ProximityOctreeVisitor::ProximityOctreeVisitor(Vector3f& position, float searchRadius)
: position_m(position)
, searchRadius_m(searchRadius)
{ }

ProximityOctreeVisitor::~ProximityOctreeVisitor() { }

/// commands -------------------------------------------------------------------
void ProximityOctreeVisitor::visitRoot(const OctreeCell* pRootCell,
									 const OctreeData& octreeData)
{
	if(pRootCell != 0)
		pRootCell->visit(octreeData, *this);
}

void ProximityOctreeVisitor::visitBranch(const OctreeCell* subCells[8],
									   const OctreeData& octreeData)
{
	/// step through subcells (can be in any order)
	/// subcell numbering:
	///    y z       6 7
	///    |/   2 3  4 5
	///     -x  0 1
	///
	/// (in binary:)
	///    y z           110 111
	///    |/   010 011  100 101
	///     -x  000 001
	///
	for (dword i = 0; i < 8;  ++i)
	{
		const OctreeCell* pSubCell = subCells[i];
		/// avoid null subcells
		if (pSubCell != 0)
		{
			const OctreeData subCellData(octreeData, i);
			// check to see if sub-cell is in radius
			float distance = (subCellData.getBound().getCenter() - position_m).length() - subCellData.getBound().getRadius();

			if (distance < searchRadius_m)
			{
				/// continue visit traversal
				pSubCell->visit(subCellData, *this);
			}
		}
	}
}

void ProximityOctreeVisitor::visitLeaf(const hxa7241_general::Array<const Block*>& items,
									   const OctreeData& octreeData)
{
	// check items
	for (dword i = 0, end = items.getLength(); i < end; ++i)
	{
		// see if in radius
		float distance = (items[i]->getOrigin() - position_m).length() - items[i]->getRadius();
		if (distance < searchRadius_m)
		{
			// add to match results
			results_m.push_back(items[i]);
		}
	}
}

const vector<const Block*>& ProximityOctreeVisitor::getResults()
{
	return results_m;
}

//
//// OcTree::RayTestOctreeVisitor
//

/// standard object services ---------------------------------------------------
RayTestOctreeVisitor::RayTestOctreeVisitor(Vector3& rayOrigin, Vector3& rayDir)
: rayOrigin_m(rayOrigin.X, rayOrigin.Y, rayOrigin.Z)
, rayDir_m(rayDir.X, rayDir.Y, rayDir.Z)
, nearestResult_m(NULL)
{ }

RayTestOctreeVisitor::~RayTestOctreeVisitor() { }

/// commands -------------------------------------------------------------------
void RayTestOctreeVisitor::visitRoot(const OctreeCell* pRootCell,
									 const OctreeData& octreeData)
{
	if(pRootCell != 0)
		pRootCell->visit(octreeData, *this);
}

void RayTestOctreeVisitor::visitBranch(const OctreeCell* subCells[8],
									   const OctreeData& octreeData)
{
	/// step through subcells (can be in any order)
	/// subcell numbering:
	///    y z       6 7
	///    |/   2 3  4 5
	///     -x  0 1
	///
	/// (in binary:)
	///    y z           110 111
	///    |/   010 011  100 101
	///     -x  000 001
	///
	for (dword i = 0; i < 8;  ++i)
	{
		const OctreeCell* pSubCell = subCells[i];
		/// avoid null subcells
		if (pSubCell != 0)
		{
			const OctreeData subCellData(octreeData, i);
			// check to see if ray is in sub-cell radius
			const OctreeBound& bounds = subCellData.getBound();
			const Vector3f& center = bounds.getCenter();
			if (Geometry::SphereBoundProbe(Vector3(center.x_m, center.y_m, center.z_m),
										   bounds.getRadius(), rayOrigin_m, rayDir_m))
			{
				/// continue visit traversal
				pSubCell->visit(subCellData, *this);
			}
		}
	}
}

void RayTestOctreeVisitor::visitLeaf(const hxa7241_general::Array<const Block*>& items,
									 const OctreeData& octreeData)
{
	// check items
	for (dword i = 0, end = items.getLength(); i < end; ++i)
	{
		// see if in radius
		const Block* blk = items[i];
		Vector3f pos = items[i]->getOrigin();
		if (Geometry::SphereBoundProbe(Vector3(pos.x_m, pos.y_m, pos.z_m),
									   items[i]->getRadius(), rayOrigin_m, rayDir_m))
		{
			// check item box bounds
			//Vector3f lower = items[i]->getDimensions();
			//if (Geometry::BoxBoundProbe(Vector3(lower.x_m, lower.y_m, lower.z_m), )
			nearestResult_m = items[i];
		}
	}
}

const vector<const Block*>& RayTestOctreeVisitor::getResults()
{
	return results_m;
}

const Block* RayTestOctreeVisitor::getNearestResult()
{
	return nearestResult_m;
}

//
//// OcTree::FrustumTestOctreeVisitor
//

/// standard object services ---------------------------------------------------
FrustumTestOctreeVisitor::FrustumTestOctreeVisitor(Plane left, Plane right, Plane top, Plane bottom, Plane near, Plane far, Vector3 offset)
{
	leftPlane_m = left;
	rightPlane_m = right;
	topPlane_m = top;
	bottomPlane_m = bottom;
	nearPlane_m = near;
	farPlane_m = far;
	this->offset = offset;
}

FrustumTestOctreeVisitor::~FrustumTestOctreeVisitor() { }

/// commands -------------------------------------------------------------------
void FrustumTestOctreeVisitor::visitRoot(const OctreeCell* pRootCell,
									     const OctreeData& octreeData)
{
	if(pRootCell != 0)
		pRootCell->visit(octreeData, *this);
}

void FrustumTestOctreeVisitor::visitBranch(const OctreeCell* subCells[8],
									       const OctreeData& octreeData)
{
	/// step through subcells (can be in any order)
	/// subcell numbering:
	///    y z       6 7
	///    |/   2 3  4 5
	///     -x  0 1
	///
	/// (in binary:)
	///    y z           110 111
	///    |/   010 011  100 101
	///     -x  000 001
	///
	for (dword i = 0; i < 8;  ++i)
	{
		const OctreeCell* pSubCell = subCells[i];
		/// avoid null subcells
		if (pSubCell != 0)
		{
			const OctreeData subCellData(octreeData, i);
			// check to see if ray is in sub-cell radius
			const OctreeBound& bounds = subCellData.getBound();
			const Vector3f& center = bounds.getCenter();

			// test each plane
			Vector3 c = Vector3(center.x_m, center.y_m, center.z_m) - this->offset;
			float radius = bounds.getRadius();
			if (leftPlane_m.Dot(c) + radius < 0/* &&
				rightPlane_m.Dot(c) + radius < 0 &&
				topPlane_m.Dot(c) + radius < 0 &&
				bottomPlane_m.Dot(c) + radius < 0 &&
				nearPlane_m.Dot(c) + radius < 0 &&
				farPlane_m.Dot(c) + radius < 0*/)
			{
				// passed all planes so visit cell
				pSubCell->visit(subCellData, *this);
			}
		}
	}
}

void FrustumTestOctreeVisitor::visitLeaf(const hxa7241_general::Array<const Block*>& items,
									     const OctreeData& octreeData)
{
	// check items
	for (dword i = 0, end = items.getLength(); i < end; ++i)
	{
		// see if in radius
		const Block* blk = items[i];
		Vector3f pos = items[i]->getOrigin();
		// test planes
		Vector3 c = Vector3(pos.x_m, pos.y_m, pos.z_m) - this->offset;
		float radius = items[i]->getRadius();
		if (leftPlane_m.Dot(c) + radius < 0/* &&
				rightPlane_m.Dot(c) + radius < 0 &&
				topPlane_m.Dot(c) + radius < 0 &&
				bottomPlane_m.Dot(c) + radius < 0 &&
				nearPlane_m.Dot(c) + radius < 0 &&
				farPlane_m.Dot(c) + radius < 0*/)
		{
			results_m.push_back(items[i]);
		}
	}
}

const vector<const Block*>& FrustumTestOctreeVisitor::getResults()
{
	return results_m;
}