#include "stdafx.h"
#include "ProteinVista.h"

#include "MatrixMath.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


//////////////////////////////////////////////////////////////////////////
//	Matrix utility function
//////////////////////////////////////////////////////////////////////////

//
//	translation은 없다.
//	matRot은 원점에서의 rotation matrix 이다.
//	
D3DXMATRIX * GetRotMatrixFromVec ( D3DXVECTOR3 * vecDir, D3DXVECTOR3 * upDir , D3DXMATRIX * matRot )
{
	D3DXMatrixIdentity ( matRot );
	D3DXVec3Normalize ( vecDir, vecDir );
	D3DXVec3Normalize ( upDir, upDir );

	D3DXVECTOR3	WorldUpVec = *upDir;

	FLOAT	epsilonx = (vecDir->x) - upDir->x;
	FLOAT	epsilony = (vecDir->y) - upDir->y;
	FLOAT	epsilony2 = (vecDir->y) + upDir->y;
	FLOAT	epsilonz = (vecDir->z) - upDir->z;
	epsilonx = (epsilonx>0)? epsilonx: -epsilonx;
	epsilony = (epsilony>0)? epsilony: -epsilony;
	epsilonz = (epsilonz>0)? epsilonz: -epsilonz;
	if ( epsilonx < 1.0e-5 && epsilonz < 1.0e-5 )
	{
		if ( epsilony < 1.0e-5 || epsilony2 < 1.0e-5 )
		{
			WorldUpVec.x += 0.001f; 
			WorldUpVec.y += 0.001f; 
			WorldUpVec.z += 0.001f;

			TRACE("Warning: Upvector == dirVector\n");
		}
	}

	//	LOS
	matRot->_31 = vecDir->x;
	matRot->_32 = vecDir->y;
	matRot->_33 = vecDir->z;

	FLOAT d = D3DXVec3Dot(&WorldUpVec, vecDir);
	D3DXVECTOR3	upVec = WorldUpVec-(*vecDir*d);
	D3DXVec3Normalize ( &upVec, &upVec );

	//	UP
	matRot->_21 = upVec.x;
	matRot->_22 = upVec.y;
	matRot->_23 = upVec.z;

	D3DXVECTOR3 cross2;
	D3DXVec3Cross(&cross2, &upVec, vecDir);
	D3DXVec3Normalize ( &cross2 , &cross2 );

	//	RIGHT
	matRot->_11 = cross2.x;
	matRot->_12 = cross2.y;
	matRot->_13 = cross2.z;

	return matRot;
}

//	
//	vector의 transform 매트릭스를 구한다.
//	pEye=> direction vector
//
D3DXMATRIX * GetMatrixFromVec ( CONST D3DXVECTOR3 *pEye, CONST D3DXVECTOR3 *pAt, CONST D3DXVECTOR3 *pUp, D3DXMATRIX * pMatTransform )
{
	D3DXMATRIX matOut;
	D3DXMATRIX matOutInverse;

	D3DXMatrixLookAtLH( &matOut, pEye, pAt, pUp );

	D3DXMatrixInverse(pMatTransform, NULL, &matOut);

	return pMatTransform;
}

//
//	eye1 을 eye2 에 맞추는 transform
//
D3DXMATRIX * GetMatrixFromTwoVec ( CONST D3DXVECTOR3 *pEye1, CONST D3DXVECTOR3 *pAt1, CONST D3DXVECTOR3 *pUp1, 
								   CONST D3DXVECTOR3 *pEye2, CONST D3DXVECTOR3 *pAt2, CONST D3DXVECTOR3 *pUp2 , 
								   D3DXMATRIX * pMatTransformResult )
{
	D3DXMATRIX matOut;
	D3DXMatrixLookAtLH( &matOut, pEye1, pAt1, pUp1 );

	D3DXMATRIX matTransform;
	GetMatrixFromVec ( pEye2, pAt2, pUp2 , &matTransform );

	*pMatTransformResult = matOut * matTransform;

	return pMatTransformResult;
}

