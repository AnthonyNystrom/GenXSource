#pragma once

#include <vector>

class CPickAtomInst
{
public:
	CAtomInst * m_pAtomInst;
	FLOAT		m_len;

	CPickAtomInst() { m_pAtomInst = NULL; m_len = 1e20; }
};

typedef std::vector < CPickAtomInst > CSTLArrayPickedAtomInst;

class CPickResidueInst
{
public:
	CPickResidueInst() { m_pos = D3DXVECTOR3(0,0,0);	m_pResidueInst= NULL; m_len = 1e20; }

	D3DXVECTOR3			m_pos;
	CResidueInst *		m_pResidueInst;
	FLOAT	m_len;
};

typedef std::vector < CPickResidueInst > CSTLArrayPickedResidueInst;

//	한점과 직선간의 길이를 구한다.
FLOAT CalcLenPointToLine(D3DXVECTOR3 &p, D3DXVECTOR3 &d , D3DXVECTOR3 &p1 );

//	한 직선이 Triangle을 관통하는지 구한다.
BOOL IntersectTriangle( const D3DXVECTOR3& orig, const D3DXVECTOR3& dir, D3DXVECTOR3& v0, D3DXVECTOR3& v1, D3DXVECTOR3& v2, FLOAT* t, FLOAT* u, FLOAT* v );

//	Picking 에서 사용되는 Pick Ray를 구한다.
void GetPickRay ( /*IN*/ D3DXMATRIXA16 &matWorld, /*OUT*/ D3DXVECTOR3 & vPickRayDir, /*OUT*/ D3DXVECTOR3 & vPickRayOrig );
void GetPickRay ( /*IN*/ POINT & ptCursor, /*IN*/ D3DXMATRIXA16 &matWorld, /*OUT*/ D3DXVECTOR3 & vPickRayDir, /*OUT*/ D3DXVECTOR3 & vPickRayOrig );
