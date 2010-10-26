#pragma once

#include "OcTree.h"

using namespace System::Collections::Generic;

namespace OcTree
{
	/// NATIVE

	class BoxBuilderOctreeVisitor
		: public OctreeVisitor<Block>
	{
		/// standard object services ---------------------------------------------------
	public:
		BoxBuilderOctreeVisitor();
		virtual ~BoxBuilderOctreeVisitor();

	private:
		BoxBuilderOctreeVisitor(const BoxBuilderOctreeVisitor&);
		BoxBuilderOctreeVisitor& operator=(const BoxBuilderOctreeVisitor&);

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
		vector<float>& getResults();

	/// fields ---------------------------------------------------------------------
	private:
		vector<float> results_m;
	};

	/// CLR

	generic<class T> where T : OcTreeItem
	public ref class OcTreeVisualizer
	{
	public:
		OcTreeVisualizer(void);

		void Visualize(OcTree<T>^ tree, array<Vector3>^% boxes);
	};

}