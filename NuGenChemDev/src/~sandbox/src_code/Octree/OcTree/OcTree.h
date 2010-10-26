// OcTree.h

#pragma once
#include "Octree.hpp"
#include <vector>

using namespace std;

using namespace System;
using namespace Microsoft::DirectX;
using namespace System::Collections::Generic;

using namespace hxa7241_graphics;

namespace OcTree {

	//// Native ---------------------------------------------------------------------
	class Block
	{
	/// standard object services ---------------------------------------------------
	public:
	    Block(unsigned int uid);
	    Block(unsigned int uid,
			  const Vector3f& position,
			  const Vector3f& origin,
	          const Vector3f& dimension,
			  float radius);

	    ~Block();
	    Block( const Block& );
		Block& operator=( const Block& );

	/// queries --------------------------------------------------------------------
	    const Vector3f& getPosition() const;
		const Vector3f& getOrigin() const;
	    const Vector3f& getDimensions() const;
		float getRadius() const;
		unsigned int getUId() const;

	/// fields ---------------------------------------------------------------------
	private:
		Vector3f position_m;
		Vector3f dimensions_m;
		Vector3f origin_m;
		float radius_m;
		unsigned int uid_m;
	};

	class OctreeAgentBlock
		: public OctreeAgent<Block>
	{
	/// standard object services ---------------------------------------------------
	public:
		OctreeAgentBlock() {};

		virtual ~OctreeAgentBlock() {};
	private:
		OctreeAgentBlock( const OctreeAgentBlock& );
		OctreeAgentBlock& operator=( const OctreeAgentBlock& );

	/// queries --------------------------------------------------------------------
	/// octree agent overrides
	protected:
		virtual bool isOverlappingCell(const Block&    item,
									   const Vector3f& lowerCorner,
									   const Vector3f& upperCorner) const;
		virtual dword getSubcellOverlaps(const Block&    item,
										 const Vector3f& lower,
										 const Vector3f& middle,
										 const Vector3f& upper) const;
	};

	class ProximityOctreeVisitor
		: public OctreeVisitor<Block>
	{
	/// standard object services ---------------------------------------------------
	public:
		ProximityOctreeVisitor(Vector3f& position, float searchRadius);
		virtual ~ProximityOctreeVisitor();

	private:
		ProximityOctreeVisitor(const ProximityOctreeVisitor&);
		ProximityOctreeVisitor& operator=(const ProximityOctreeVisitor&);

	/// commands -------------------------------------------------------------------
	/// octree visitor overrides
	protected:
		virtual void visitRoot(const OctreeCell* pRootCell,
							   const OctreeData& octreeData);
		virtual void visitBranch(const OctreeCell* subCells[8],
								 const OctreeData& octreeData);
		virtual void visitLeaf(const hxa7241_general::Array<const Block*>& items,
							   const OctreeData& octreeData);

	/// queries --------------------------------------------------------------------

		const vector<const Block*>& getResults();

	/// fields ---------------------------------------------------------------------
	private:
		Vector3f position_m;
		float searchRadius_m;
		vector<const Block*> results_m;
	};

	class RayTestOctreeVisitor
		: public OctreeVisitor<Block>
	{
		/// standard object services ---------------------------------------------------
	public:
		RayTestOctreeVisitor(Vector3& rayOrigin, Vector3& rayDir);
		virtual ~RayTestOctreeVisitor();

	private:
		RayTestOctreeVisitor(const RayTestOctreeVisitor&);
		RayTestOctreeVisitor& operator=(const RayTestOctreeVisitor&);

	/// commands -------------------------------------------------------------------
	/// octree visitor overrides
	protected:
		virtual void visitRoot(const OctreeCell* pRootCell,
							   const OctreeData& octreeData);
		virtual void visitBranch(const OctreeCell* subCells[8],
								 const OctreeData& octreeData);
		virtual void visitLeaf(const hxa7241_general::Array<const Block*>& items,
							   const OctreeData& octreeData);

	/// queries --------------------------------------------------------------------
	public:
		const vector<const Block*>& getResults();
		const Block* getNearestResult();

	/// fields ---------------------------------------------------------------------
	public:
		Vector3 rayOrigin_m;
		Vector3 rayDir_m;
		vector<const Block*> results_m;
		const Block* nearestResult_m;
	};
ref struct Frustum;
	class FrustumTestOctreeVisitor
		: public OctreeVisitor<Block>
	{
		/// standard object services ---------------------------------------------------
	public:
		FrustumTestOctreeVisitor(Plane left, Plane right, Plane top, Plane bottom, Plane near, Plane far, Vector3 offset);
		virtual ~FrustumTestOctreeVisitor();

	private:
		FrustumTestOctreeVisitor(const FrustumTestOctreeVisitor&);
		FrustumTestOctreeVisitor& operator=(const FrustumTestOctreeVisitor&);

	/// commands -------------------------------------------------------------------
	/// octree visitor overrides
	protected:
		virtual void visitRoot(const OctreeCell* pRootCell,
							   const OctreeData& octreeData);
		virtual void visitBranch(const OctreeCell* subCells[8],
								 const OctreeData& octreeData);
		virtual void visitLeaf(const hxa7241_general::Array<const Block*>& items,
							   const OctreeData& octreeData);

	/// queries --------------------------------------------------------------------
	public:
		const vector<const Block*>& getResults();

	/// fields ---------------------------------------------------------------------
	public:
		Plane leftPlane_m, rightPlane_m, topPlane_m, bottomPlane_m, nearPlane_m, farPlane_m;
		vector<const Block*> results_m;
		Vector3 offset;
	};

	//
	//// CLR -----------------------------------------------------------------------
	//

	public ref class OcTreeItem
	{
	public:
		OcTreeItem(Vector3^ position, Vector3^ dimensions, float radius)
		{
			this->Position = position;
			this->Dimensions = dimensions;
			this->Radius = radius;
		}
		~OcTreeItem()
		{
			delete treeBlock;
		}

		const property Vector3^ Position3D
		{
			const Vector3^ get()
			{
				return Position;
			}
		}
		const property Vector3^ Dimensions3D
		{
			const Vector3^ get()
			{
				return Dimensions;
			}
		}
		property float ItemRadius
		{
			float get()
			{
				return Radius;
			}
		}
		property unsigned int UId
		{
			unsigned int get()
			{
				return treeBlock->getUId();
			}
		}

	internal:
		Vector3^ Position;
		float Radius;
		Vector3^ Dimensions;
		Block* treeBlock;
	};

	public ref struct Frustum
	{
	public:
		array<Plane^>^ Planes;
		Plane ^Left, ^Right, ^Top, ^Bottom, ^Near, ^Far;
		Frustum()
		{
			Planes = gcnew array<Plane^>(6);
			Left = Planes[0] = gcnew Plane();
			Right = Planes[1] = gcnew Plane();
			Top = Planes[2] = gcnew Plane();
			Bottom = Planes[3] = gcnew Plane();
			Near = Planes[4] = gcnew Plane();
			Far = Planes[5] = gcnew Plane();
		}

		Frustum(Matrix^ viewProjection)
		{
			Planes = gcnew array<Plane^>(6);
			Left = Planes[0] = gcnew Plane();
			Right = Planes[1] = gcnew Plane();
			Top = Planes[2] = gcnew Plane();
			Bottom = Planes[3] = gcnew Plane();
			Near = Planes[4] = gcnew Plane();
			Far = Planes[5] = gcnew Plane();

			Left->A = viewProjection->M14 + viewProjection->M11;
			Left->B = viewProjection->M24 + viewProjection->M21;
			Left->C = viewProjection->M34 + viewProjection->M31;
			Left->D = viewProjection->M44 + viewProjection->M41;

			Right->A = viewProjection->M14 - viewProjection->M11;
			Right->B = viewProjection->M24 - viewProjection->M21;
			Right->C = viewProjection->M34 - viewProjection->M31;
			Right->D = viewProjection->M44 - viewProjection->M41;

			Top->A = viewProjection->M14 - viewProjection->M12;
			Top->B = viewProjection->M24 - viewProjection->M22;
			Top->C = viewProjection->M34 - viewProjection->M32;
			Top->D = viewProjection->M44 - viewProjection->M42;

			Bottom->A = viewProjection->M14 + viewProjection->M12;
			Bottom->B = viewProjection->M24 + viewProjection->M22;
			Bottom->C = viewProjection->M34 + viewProjection->M32;
			Bottom->D = viewProjection->M44 + viewProjection->M42;

			// Near plane
			Near->A = viewProjection->M13;  
			Near->B = viewProjection->M23;
			Near->C = viewProjection->M33;
			Near->D = viewProjection->M43;

			// Far plane
			Far->A = viewProjection->M14 - viewProjection->M13;  
			Far->B = viewProjection->M24 - viewProjection->M23;
			Far->C = viewProjection->M34 - viewProjection->M33;
			Far->D = viewProjection->M44 - viewProjection->M43;

			for (int i = 0; i < 6; i++)
			{
				Planes[i]->Normalize();
			} 
		}
	};

	generic<class T> where T : OcTreeItem
	public ref class OcTree
	{
	internal:
		int worldSize;
		Octree<Block>* octree;
		OctreeAgentBlock* agent;
		Dictionary<unsigned int,T>^ itemCache;
		unsigned int uid_count;
		Vector3^ offset;

	public:
		OcTree(int worldSize, Vector3^ offset)
		{
			this->offset = offset;
			this->uid_count = 0;
			this->worldSize = worldSize;
			octree = new Octree<Block>(Vector3f::ZERO(), worldSize, 4, 4);
			agent = new OctreeAgentBlock();
			itemCache = gcnew System::Collections::Generic::Dictionary<unsigned int,T>();
		}

		~OcTree()
		{
			if (octree)
				delete octree;
			if (agent)
				delete agent;
		}

		void Insert(T item)
		{
			itemCache->Add(uid_count, item);
			item->treeBlock = new Block(uid_count++,
										Vector3f(item->Position->X - (item->Dimensions->X / 2) + offset->X,
												 item->Position->Y - (item->Dimensions->Y / 2) + offset->Y,
												 item->Position->Z - (item->Dimensions->Z / 2) + offset->Z),
										Vector3f(item->Position->X + offset->X, item->Position->Y + offset->Y, item->Position->Z + offset->Z),
										Vector3f(item->Dimensions->X, item->Dimensions->Y, item->Dimensions->Z),
										item->Radius);
			if (!octree->insertItem(*item->treeBlock, *agent))
				throw gcnew Exception("Failed to add to tree");
		}

		void Update(T item, Vector3^ position, float radius)
		{
			// just do remove and re-add for now
			octree->removeItem(*item->treeBlock, *agent);
		}

		void Remove(T item)
		{
			octree->removeItem(*item->treeBlock, *agent);
			itemCache->Remove(item->treeBlock->getUId());
		}

		array<T>^ RayIntersectAll(Vector3^ ray)
		{
			return nullptr;
		}

		OcTreeItem^ RayIntersectFirst(Vector3^ rayOrigin, Vector3^ rayDir)
		{
			// walk tree with ray
			RayTestOctreeVisitor rayTester(Vector3(rayOrigin->X + offset->X, rayOrigin->Y + offset->Y, rayOrigin->Z + offset->Z),
										   Vector3(rayDir->X, rayDir->Y, rayDir->Z));
			octree->visit(rayTester);

			const Block* block = rayTester.getNearestResult();

			if (block)
				return itemCache[block->getUId()];
			return nullptr;
		}

		array<T>^ GetAllInsideFrustum(Frustum^ frustum)
		{
			FrustumTestOctreeVisitor fTester(*frustum->Left, *frustum->Right,
											 *frustum->Top, *frustum->Bottom,
											 *frustum->Near, *frustum->Far,
											 *this->offset);
			octree->visit(fTester);

			// match all blocks found to tree items
			if (fTester.results_m.size() > 0)
			{
				array<T>^ items = gcnew array<T>(fTester.results_m.size());
				for (int i = 0; i < fTester.results_m.size(); i++)
				{
					items[i] = itemCache[fTester.results_m[i]->getUId()];
				}
				return items;
			}
			return nullptr;
		}

		array<T>^ FindAllNear(T item, float radius)
		{
			return nullptr;
		}

		void StreamAllInFrustum(/*FRUSTUM,STREAMDEST*/)
		{
		}

		property bool IsEmpty
		{
			bool get() { return octree->isEmpty(); }
		}

		property float Size
		{
			float get() { return octree->getSize(); }
		}

		property int LeafCount
		{
			int get()
			{
				dword byteSize, leafCount, itemRefCount, maxDepth;
				octree->getInfo(byteSize, leafCount, itemRefCount, maxDepth);
				return leafCount;
			}
		}

		property int ByteSize
		{
			int get()
			{
				dword byteSize, leafCount, itemRefCount, maxDepth;
				octree->getInfo(byteSize, leafCount, itemRefCount, maxDepth);
				return byteSize;
			}
		}

		property int ItemsCount
		{
			int get()
			{
				dword byteSize, leafCount, itemRefCount, maxDepth;
				octree->getInfo(byteSize, leafCount, itemRefCount, maxDepth);
				return itemRefCount;
			}
		}

		property Dictionary<unsigned int,T>^ SceneItems
		{
			Dictionary<unsigned int,T>^ get()
			{
				return itemCache;
			}
		}

		property Vector3^ Offset
		{
			Vector3^ get()
			{
				return offset;
			}
		}
	};
}