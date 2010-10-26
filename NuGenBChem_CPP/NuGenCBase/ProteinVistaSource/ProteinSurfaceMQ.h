//	(c) 2000-2002 J. Gans (jdg9@cornell.edu), Shalloway Lab, 
//	Cornell University. You can modify and freely distribute 
//	this code, but please keep this header intact.
//
//	Look-and-feel inspired by Xmol 
//	(http://www.networkcs.com/msc/docs/xmol/).
//
//	OpenGL selection code and quaterion rotation matrix
//	formulation from molview demo program by Mark Kilgard
//	(http://reality.sgi.com/opengl/OpenGLforX.html). PostScript
//	rendering also from Mark Kilgard.
//
//	The printing routines are based on code from
//	Craig Fahrnbach <craignan@home.com> via
//	Uwe Kotyczka <kotyczka@bnm-gmbh.de>.
//
//	Covalent bond determination code based on VMD
//	http://www.ks.uiuc.edu/Research/vmd/
//
//	Stride (http://www.embl-heidelberg.de/stride/stride.html)
//	is used for secondary structure determination and has been 
//	included in Qmol with the kind permission of 
//	Dmitrij Frishman, PhD
//	Institute for Bioinformatics
//	GSF - Forschungszentrum f? Umwelt und Gesundheit, GmbH
//	Ingolst?ter Landstra? 1,
//	D-85764 Neuherberg, Germany
//	Telephone: +49-89-3187-4201
//	Fax: +49-89-31873585
//	e-mail: d.frishman@gsf.de
//	WWW: http://mips.gsf.de/mips/staff/frishman/

//////////////////////////////////////////////////////////////////
// Classes and functions for computing molecular surfaces
//////////////////////////////////////////////////////////////////
#pragma once 

#include "pdb.h"
#include "pdbInst.h"

#include "PDBRenderer.h"
#include "ProteinSurfaceBase.h"

#include "RenderSurfaceArray.h"
#include "RenderSurfaceList.h"

#include <set>

class	CPDBRenderer;

struct	Triangle;

using namespace RenderSurfaceList;
using namespace RenderSurfaceArray;

//	중복되는것을 없애는 컨테이너.
typedef std::set<int> CSTLIntSet;
typedef std::vector<CSTLIntSet> CSTLArraySetInt;

//
struct VertexInfo{
	Triangle* parent;
	long	 index;
};

//===================================================================================================

class CProteinSurfaceMQ: public CProteinSurfaceBase
{
public:
	CProteinSurfaceMQ();
	virtual ~CProteinSurfaceMQ();

	virtual		void Init ( CPDB * pPDB, CChain * pChain, double probeSphere, int surfaceQuality, BOOL bAddHETATM );
	virtual		HRESULT CreateSurface();

	virtual		float	GetSurfaceQuality(int quality);
	virtual		long	GetTypeGenSurface();

	//
	//
	arrayRender< float > m_arrayVertexMQ;
	arrayRender< float > m_arrayNormalMQ;
	arrayRender< int > m_arrayIndexFaceMQ;
	arrayRender< int > m_arrayIndexAtomMQ;	//	index->m_ptrAtoms 로 의 index.

	//	std::vector<CSTLLONGArray>	m_ArrayArrayFaceIndex;		//	3개가 1쌍으로 CAtom이 나타내는 surface의 vertex index를 넣어둔다.

	//	반대는 CMolSurface의 colorIndex[] 이다.
	//	CSTLArraySetInt		m_setArrayAdjacentVertex;

	//
	//	The different surface types
	//	
	enum {HEURISTIC};

	inline BOOL ValidSurface()
	{
		return validSurface;
	};

	inline float GetProbeSphereRadius()
	{
		return probeSphere;
	};

	inline void SetProbeSphereRadius(float m_radius)
	{
		probeSphere = m_radius;

		// Update the box length
		boxLength = 3.0f + probeSphere*probeSphere;
	};


	int GetSurfQuality();
	void SetSurfQuality(int m_quality);

	BOOL BuildSurface(CSTLArrayAtom & arrayAtom );

	inline void ColorByAtom(BOOL m_b)
	{
		color_by_atom = m_b;
	};

	inline int ColorByAtom()
	{
		return color_by_atom;
	};

	inline int GetTransparency()
	{
		return transparency;
	};

	inline void GetSurfColor(int &m_r, int &m_g, int &m_b, int &m_a)
	{
		m_r = color[0];
		m_g = color[1];
		m_b = color[2];
		m_a = color[3];
	};

	inline void SetSurfColor(int m_r, int m_g, int m_b, int m_a)
	{
		color[0] = m_r;
		color[1] = m_g;
		color[2] = m_b;
		color[3] = m_a;
	};

	inline int GetSurfType()
	{
		return surfaceType;
	};

	inline void SetSurfType(int m_type)
	{
		surfaceType = m_type;
	};

	void GetAtomSurfaceArea(arrayRender<float> &m_area);
	void GetAdjacentVertexIndex(int vertexIndex);

private:

	BOOL validSurface;

	float fTargetValue;
	float targetValueCutOff;
	float probeSphere;
	float meshSize;

	// Which structure are we generating a surface for?
	// traj_index == -1 if primary structure selected.
	int traj_index;

	int surfaceType; // The type of molecular surface. Currently 
					 // HEURISTIC

	int renderStyle; // The rendering style for the surface. Currently
					 // either SOLID, MESH or POINT
	
	// Should solvent atoms be included when rendering a molecular surface?
	BOOL include_solvent_atoms;

	float boxLength;
	int maxNumBox;
	float maxLength;
	arrayRender< listRender<int>* > atomBox;
	arrayRender< float > cubeBox;
	arrayRender< listRender<VertexInfo>* > vertexBox;
	listRender<Triangle> triangleList;

private:
	float boxSize[3];
	float invBoxSize[3];
	int xytotb, totb;	// Number of grid cells in atom box

	int cubeSize[3];
	int cubeXY;

	float molMin[3];
	float molMax[3];
	int numBox[3];
	
	BOOL color_by_atom;
	BYTE	transparency;
	BYTE	color[4];

	void vMarchCube(float fX, float fY, float fZ, 
		int xIndex, int yIndex, int zIndex, float fScale);

	// fGetOffset finds the approximate point of intersection of the surface
	// between two points with the values fValue1 and fValue2.
	inline float fGetOffset(float fValue1, float fValue2, float fValueDesired)
	{
		float fDelta = fValue2 - fValue1;

        if(fDelta == 0.0f)
        {
                return 0.5f;
        }
        return (fValueDesired - fValue1)/fDelta;
	};

	inline float ScalarField(float fX, float fY, float fZ)
	{
		return ScalarFieldHeuristic(fX, fY, fZ);
	};

	inline void GradientAndColorIndex(float r[3], float grad[3], int &colorIndex)
	{
		GradientAndColorIndexHeuristic(r, grad, colorIndex);
	};

	float ScalarFieldHeuristic(float fX, float fY, float fZ);
	void GradientAndColorIndexHeuristic(float r[3], float grad[3], int &colorIndex);

	void BuildAtomBox();

	void DeleteAtomBox();

	void BuildVertexArray();

	float TriangleArea(int a, int b, int c);

};

struct Triangle
{
	Triangle()
	{
		vertexPtr[0] = vertexData[0];
		vertexPtr[1] = vertexData[1];
		vertexPtr[2] = vertexData[2];

		vertexIndex[0] = -1;
		vertexIndex[1] = -1;
		vertexIndex[2] = -1;
	};

	int vertexIndex[3];
	float* vertexPtr[3];
	float vertexData[3][3];

	void SetVertex(int i, float v[3])
	{
		memcpy(vertexData[i], v, 3*sizeof(float));
	};

	Triangle& operator=(const Triangle &copy)
	{
		vertexIndex[0] = copy.vertexIndex[0];
		vertexIndex[1] = copy.vertexIndex[1];
		vertexIndex[2] = copy.vertexIndex[2];

		memcpy(vertexData[0], copy.vertexData[0], 3*sizeof(float));
		memcpy(vertexData[1], copy.vertexData[1], 3*sizeof(float));
		memcpy(vertexData[2], copy.vertexData[2], 3*sizeof(float));

		// Don't copy the vertexPtr arrayRender -- it always points to 
		// vertexData unless specifically changed.

		return (*this);
	};
};