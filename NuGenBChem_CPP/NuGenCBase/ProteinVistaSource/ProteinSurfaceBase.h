#pragma once

#include "PDB.h"
#include "PDBInst.h"

#include <set>

class	CPDB;
class	CChain;
class	CAtom;
class	CPDBRenderer;

//	중복되는것을 없애는 컨테이너.
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

	CSTLVectorValueArray	m_arrayVertex;			//	vertex를 저장, MSMS 에서 구해짐
	CSTLVectorValueArray	m_arrayNormal;			//	normal, MSMS 에서 구해짐
	CSTLIntArray			m_arrayIndexFace;		//	face. 3개가 1set, MSMS 에서 구해짐

	CSTLIntArray			m_arrayIndexAtom;		//	index->m_arrayAtom 로 의 index, MSMS 에서 구해짐
	CSTLIntArray			m_arrayTypeVertex;		//	MSMS에서 나오는데이타	
	CSTLIntArray			m_arrayTypeFace;		//	MSMS에서 나오는데이타	

	std::vector<CSTLLONGArray>	m_ArrayArrayFaceIndex;			//	atom 이 가지는 triangle face 의 index를 저장
	std::vector<CSTLLONGArray>	m_ArrayArrayAdjacentVertex;		//	vertex 주변의 vertex들의 index 를 저장

	CSTLFLOATArray	m_arrayVertexCurvature[MAX_RING];
	FLOAT			m_arrayVertexCurvatureMin[MAX_RING];
	FLOAT			m_arrayVertexCurvatureMax[MAX_RING];

	enum {SURFACE_MQ, SURFACE_MSMS};
	long			m_surfaceGenMethod;

public:
	virtual void	Init ( CPDB * pPDB, CChain * pChain, double probeSphere, int surfaceQuality, BOOL bAddHETATM );

	virtual float	GetSurfaceQuality(int quality);		//	enum Quality--> vertex에서 사용되는 float quality로 변환
	virtual long	GetTypeGenSurface() = 0;

	//	surface에 참여하는 atomInst를 얻는다.
	virtual HRESULT GetSurfaceAtomInst( CChainInst * pChainInst, CSTLArrayAtomInst & arrayAtomInst );

	virtual HRESULT CreateSurface() = 0;
	virtual HRESULT CreateAdjacentVertex();

	virtual void	CalculateCurvature(int depthLimit );
	virtual float	CalcAdjacentCurvature(int iVertex, float &sumCurvature, int depth, int depthLimit, CSTLLONGArray & arrayPoint );
};


