#pragma once

#include "PDB.h"
#include "PDBInst.h"

#include <set>

class	CPDB;
class	CChain;
class	CAtom;
class	CPDBRenderer;

//	�ߺ��Ǵ°��� ���ִ� �����̳�.
typedef std::set<int> CSTLIntSet;
typedef std::set<int> CSTLSetArray;
typedef std::vector<CSTLIntSet> CSTLIntSetArray;

#define		MAX_RING		10

class CProteinSurfaceBase
{
public:
	CProteinSurfaceBase();
	virtual ~CProteinSurfaceBase();

	BOOL				m_bUsed;

	CPDB *				m_pPDB;
	CChain *			m_pChain;

	long				m_chainID;
	long				m_modelID;

	double				m_probeSphere;
	int					m_surfaceQuality;
	BOOL				m_bAddHETATM;

	//
	//	
	CSTLArrayAtom			m_arrayAtom;

	CSTLVectorValueArray	m_arrayVertex;			//	vertex�� ����, MSMS ���� ������
	CSTLVectorValueArray	m_arrayNormal;			//	normal, MSMS ���� ������
	CSTLIntArray			m_arrayIndexFace;		//	face. 3���� 1set, MSMS ���� ������

	CSTLIntArray			m_arrayIndexAtom;		//	index->m_arrayAtom �� �� index, MSMS ���� ������
	CSTLIntArray			m_arrayTypeVertex;		//	MSMS���� �����µ���Ÿ	
	CSTLIntArray			m_arrayTypeFace;		//	MSMS���� �����µ���Ÿ	

	std::vector<CSTLLONGArray>	m_ArrayArrayFaceIndex;			//	atom �� ������ triangle face �� index�� ����
	std::vector<CSTLLONGArray>	m_ArrayArrayAdjacentVertex;		//	vertex �ֺ��� vertex���� index �� ����

	CSTLFLOATArray	m_arrayVertexCurvature[MAX_RING];
	FLOAT			m_arrayVertexCurvatureMin[MAX_RING];
	FLOAT			m_arrayVertexCurvatureMax[MAX_RING];

	enum {SURFACE_MQ, SURFACE_MSMS};
	long			m_surfaceGenMethod;

public:
	virtual void	Init ( CPDB * pPDB, CChain * pChain, double probeSphere, int surfaceQuality, BOOL bAddHETATM );

	virtual float	GetSurfaceQuality(int quality);		//	enum Quality--> vertex���� ���Ǵ� float quality�� ��ȯ
	virtual long	GetTypeGenSurface() = 0;

	//	surface�� �����ϴ� atomInst�� ��´�.
	virtual HRESULT GetSurfaceAtomInst( CChainInst * pChainInst, CSTLArrayAtomInst & arrayAtomInst );

	virtual HRESULT CreateSurface() = 0;
	virtual HRESULT CreateAdjacentVertex();

	virtual void	CalculateCurvature(int depthLimit );
	virtual float	CalcAdjacentCurvature(int iVertex, float &sumCurvature, int depth, int depthLimit, CSTLLONGArray & arrayPoint );
};


