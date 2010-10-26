#include "StdAfx.h"


#ifdef min
#undef min
#endif

#ifdef max
#undef max
#endif

#include "localSuperImpose.h"

#define MAX_AA_LENGTH	1500

#include "Jama\\tnt.h"
#include "Jama\\jama_svd.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


using namespace TNT;


double MAtrixDeterminant( TNT::Array2D<double> &mat );

double CalcMatchResidueRmsd(CSTLVectorValueArray & m_atomPosArrayCa1, CSTLLONGArray & matchResidue1, CSTLVectorValueArray & m_atomPosArrayCa2, CSTLLONGArray & matchResidue2 , long nMatch , D3DXMATRIX &transform )
{
	return GetLocalSuperImposeTransform(m_atomPosArrayCa1, matchResidue1, m_atomPosArrayCa2, matchResidue2, nMatch, transform, TRUE );
}

double GetLocalSuperImposeTransform( IN CSTLVectorValueArray & arrayPos1, IN CSTLLONGArray & matchIndex1, 
									 IN CSTLVectorValueArray & arrayPos2, IN CSTLLONGArray & matchIndex2, 
									 long nMatch, OUT D3DXMATRIX &transform , BOOL bCalcRMSD )
{
	long n = nMatch;

	ASSERT(!( n > MAX_AA_LENGTH ));

	int i,j,k;

	double minrmsd = 1e20; // a very big number goes here
	double rmsd;

	static double pi1[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	static double pi2[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double p1[3]={0,}; // The centroid of first point set
	double p2[3]={0,}; // The centroid of second point set
	static double qi1[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of first point set
	static double qi2[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of second point set
	double H[3][3] = {0,};

	ZeroMemory(pi1, sizeof(double)* MAX_AA_LENGTH* 3);
	ZeroMemory(pi2, sizeof(double)* MAX_AA_LENGTH* 3);
	ZeroMemory(qi1, sizeof(double)* MAX_AA_LENGTH* 3);
	ZeroMemory(qi2, sizeof(double)* MAX_AA_LENGTH* 3);

	//	result.
	double rvec[3][3] = {0,};
	double tvec[3] = {0,};

	long	iPi = 0;
	for ( i = 0 ; i < matchIndex1.size() ; i+=2 )
	{
		for ( j = 0 ; j <= matchIndex1[i+1]-matchIndex1[i] ; j++ )
		{
			long iBegin1 = matchIndex1[i];
			pi1[n-1-iPi][0] = arrayPos1[iBegin1+j].x;
			pi1[n-1-iPi][1] = arrayPos1[iBegin1+j].y;
			pi1[n-1-iPi][2] = arrayPos1[iBegin1+j].z;

			long iBegin2 = matchIndex2[i];
			pi2[n-1-iPi][0] = arrayPos2[iBegin2+j].x;
			pi2[n-1-iPi][1] = arrayPos2[iBegin2+j].y;
			pi2[n-1-iPi][2] = arrayPos2[iBegin2+j].z;
			iPi ++;
		}
	}

	//for ( i = 0 ; i < n ; i++ )
	//{
	//	pi1[n-1-i][0] = arrayPos1[iBegin1+i]->x;
	//	pi1[n-1-i][1] = arrayPos1[iBegin1+i]->y;
	//	pi1[n-1-i][2] = arrayPos1[iBegin1+i]->z;

	//	pi2[n-1-i][0] = arrayPos2[iBegin2+i]->x;
	//	pi2[n-1-i][1] = arrayPos2[iBegin2+i]->y;
	//	pi2[n-1-i][2] = arrayPos2[iBegin2+i]->z;
	//}

	// computation of the centroids
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			p1[j]+=pi1[i][j];
			p2[j]+=pi2[i][j];
		}
	}

	for (j=0;j<3;j++)
	{
		p1[j] /= n;
		p2[j] /= n;
	}

	//	difference
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			qi1[i][j]=pi1[i][j]-p1[j];
			qi2[i][j]=pi2[i][j]-p2[j];
		}
	}

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			for (k=0;k<n;k++)
			{
				H[i][j]+=(qi1[k][i]*qi2[k][j]);
			}
		}
	}

	//	
	TNT::Array2D<double> matX(3,3, (double *)H);
	JAMA::SVD <double>  svd (matX);

	TNT::Array2D<double> svdU(3,3);
	TNT::Array2D<double> svdV(3,3);

	svd.getU(svdU);
	svd.getV(svdV);

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			rvec[i][j] =	svdV[i][0] * svdU[j][0] + 
				svdV[i][1] * svdU[j][1] + 
				svdV[i][2] * svdU[j][2];
		}
	}


	double det1 = ::MAtrixDeterminant( svdU );
	double det2 = ::MAtrixDeterminant( svdV );

	if ( det1 * det2 < 0 )
	{
		rvec[0][2]=-rvec[0][2];
		rvec[1][2]=-rvec[1][2];
		rvec[2][2]=-rvec[2][2];
	}


	for (i=0;i<3;i++)
		tvec[i] = p2[i] - (rvec[i][0]*p1[0]+rvec[i][1]*p1[1]+rvec[i][2]*p1[2]);

	transform._11 = rvec[0][0];
	transform._21 = rvec[0][1];
	transform._31 = rvec[0][2];
	transform._41 = tvec[0];
	transform._12 = rvec[1][0];
	transform._22 = rvec[1][1];
	transform._32 = rvec[1][2];
	transform._42 = tvec[1];
	transform._13 = rvec[2][0];
	transform._23 = rvec[2][1];
	transform._33 = rvec[2][2];
	transform._43 = tvec[2];
	transform._14 = 0;
	transform._24 = 0;
	transform._34 = 0;
	transform._44 = 1.0f;

	double newpi1[MAX_AA_LENGTH][3]={0,};

	for ( int i = 0 ; i < n ; i++ )
	{
		D3DXVECTOR3 out;
		D3DXVECTOR3 vec ( pi1[i][0], pi1[i][1], pi1[i][2] );

		D3DXVec3TransformCoord(&out, &vec, &transform);

		newpi1[i][0] = out.x;
		newpi1[i][1] = out.y;
		newpi1[i][2] = out.z;
	}

	double totalErr = 0.0;
	for (int i=0 ; i<n ; i++ )
	{
		totalErr += (pi2[i][0]-newpi1[i][0])*(pi2[i][0]-newpi1[i][0])+(pi2[i][1]-newpi1[i][1])*(pi2[i][1]-newpi1[i][1])+(pi2[i][2]-newpi1[i][2])*(pi2[i][2]-newpi1[i][2]);
	}

	minrmsd = sqrt(totalErr/n);

	//	TRACE("minrmsd: %f\n" , minrmsd);

	return minrmsd;
}

double CalcMatchResidueRmsd(CSTLVectorValueArray & arrayPos1, CSTLVectorValueArray & arrayPos2, D3DXMATRIX &transform )
{
	ASSERT(arrayPos1.size() == arrayPos2.size());

	long n = arrayPos1.size();

	ASSERT(!( n > MAX_AA_LENGTH ));

	int i,j,k;

	double minrmsd = 1e20; // a very big number goes here
	double rmsd;

	double pi1[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double pi2[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double p1[3]={0,}; // The centroid of first point set
	double p2[3]={0,}; // The centroid of second point set
	double qi1[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of first point set
	double qi2[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of second point set
	double H[3][3] = {0,};

	//	result.
	double rvec[3][3] = {0,};
	double tvec[3] = {0,};

	for ( i = 0 ; i < n ; i++ )
	{
		pi1[n-1-i][0] = arrayPos1[i].x;
		pi1[n-1-i][1] = arrayPos1[i].y;
		pi1[n-1-i][2] = arrayPos1[i].z;

		pi2[n-1-i][0] = arrayPos2[i].x;
		pi2[n-1-i][1] = arrayPos2[i].y;
		pi2[n-1-i][2] = arrayPos2[i].z;
	}

	// computation of the centroids
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			p1[j]+=pi1[i][j];
			p2[j]+=pi2[i][j];
		}
	}

	for (j=0;j<3;j++)
	{
		p1[j] /= n;
		p2[j] /= n;
	}

	//	difference
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			qi1[i][j]=pi1[i][j]-p1[j];
			qi2[i][j]=pi2[i][j]-p2[j];
		}
	}

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			for (k=0;k<n;k++)
			{
				H[i][j]+=(qi1[k][i]*qi2[k][j]);
			}
		}
	}

	//	
	TNT::Array2D<double> matX(3,3, (double *)H);
	JAMA::SVD <double>  svd (matX);

	TNT::Array2D<double> svdU(3,3);
	TNT::Array2D<double> svdV(3,3);

	svd.getU(svdU);
	svd.getV(svdV);

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			rvec[i][j] =	svdV[i][0] * svdU[j][0] + 
				svdV[i][1] * svdU[j][1] + 
				svdV[i][2] * svdU[j][2];
		}
	}


	double det1 = ::MAtrixDeterminant( svdU );
	double det2 = ::MAtrixDeterminant( svdV );

	if ( det1 * det2 < 0 )
	{
		rvec[0][2]=-rvec[0][2];
		rvec[1][2]=-rvec[1][2];
		rvec[2][2]=-rvec[2][2];
	}


	for (i=0;i<3;i++)
		tvec[i] = p2[i] - (rvec[i][0]*p1[0]+rvec[i][1]*p1[1]+rvec[i][2]*p1[2]);

	transform._11 = rvec[0][0];
	transform._21 = rvec[0][1];
	transform._31 = rvec[0][2];
	transform._41 = tvec[0];
	transform._12 = rvec[1][0];
	transform._22 = rvec[1][1];
	transform._32 = rvec[1][2];
	transform._42 = tvec[1];
	transform._13 = rvec[2][0];
	transform._23 = rvec[2][1];
	transform._33 = rvec[2][2];
	transform._43 = tvec[2];
	transform._14 = 0;
	transform._24 = 0;
	transform._34 = 0;
	transform._44 = 1.0f;

	double newpi1[MAX_AA_LENGTH][3]={0,};

	for ( int i = 0 ; i < n ; i++ )
	{
		D3DXVECTOR3 out;
		D3DXVECTOR3 vec ( pi1[i][0], pi1[i][1], pi1[i][2] );

		D3DXVec3TransformCoord(&out, &vec, &transform);

		newpi1[i][0] = out.x;
		newpi1[i][1] = out.y;
		newpi1[i][2] = out.z;
	}

	double totalErr = 0.0;
	for (int i=0 ; i<n ; i++ )
	{
		totalErr += (pi2[i][0]-newpi1[i][0])*(pi2[i][0]-newpi1[i][0])+(pi2[i][1]-newpi1[i][1])*(pi2[i][1]-newpi1[i][1])+(pi2[i][2]-newpi1[i][2])*(pi2[i][2]-newpi1[i][2]);
	}

	minrmsd = sqrt(totalErr/n);

	//	TRACE("minrmsd: %f\n" , minrmsd);

	return minrmsd;
}

double GetLocalSuperImposeTransform( IN CSTLArrayVector & arrayPos1, IN long iBegin1 , IN CSTLArrayVector	& arrayPos2, IN long iBegin2, IN long nAtom , OUT D3DXMATRIX &transform )
{
	long n = nAtom;

	int i,j,k;

	double minrmsd = 1e20; // a very big number goes here
	double rmsd;

	double pi1[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double pi2[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double p1[3]={0,}; // The centroid of first point set
	double p2[3]={0,}; // The centroid of second point set
	double qi1[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of first point set
	double qi2[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of second point set
	double H[3][3] = {0,};

	//	result.
	double rvec[3][3] = {0,};
	double tvec[3] = {0,};

	for ( i = 0 ; i < n ; i++ )
	{
		pi1[n-1-i][0] = arrayPos1[iBegin1+i]->x;
		pi1[n-1-i][1] = arrayPos1[iBegin1+i]->y;
		pi1[n-1-i][2] = arrayPos1[iBegin1+i]->z;

		pi2[n-1-i][0] = arrayPos2[iBegin2+i]->x;
		pi2[n-1-i][1] = arrayPos2[iBegin2+i]->y;
		pi2[n-1-i][2] = arrayPos2[iBegin2+i]->z;
	}

	// computation of the centroids
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			p1[j]+=pi1[i][j];
			p2[j]+=pi2[i][j];
		}
	}

	for (j=0;j<3;j++)
	{
		p1[j] /= n;
		p2[j] /= n;
	}

	//	difference
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			qi1[i][j]=pi1[i][j]-p1[j];
			qi2[i][j]=pi2[i][j]-p2[j];
		}
	}

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			for (k=0;k<n;k++)
			{
				H[i][j]+=(qi1[k][i]*qi2[k][j]);
			}
		}
	}

	//	
	TNT::Array2D<double> matX(3,3, (double *)H);
	JAMA::SVD <double>  svd (matX);

	TNT::Array2D<double> svdU(3,3);
	TNT::Array2D<double> svdV(3,3);

	svd.getU(svdU);
	svd.getV(svdV);

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			rvec[i][j] =	svdV[i][0] * svdU[j][0] + 
				svdV[i][1] * svdU[j][1] + 
				svdV[i][2] * svdU[j][2];
		}
	}


	double det1 = ::MAtrixDeterminant( svdU );
	double det2 = ::MAtrixDeterminant( svdV );

	if ( det1 * det2 < 0 )
	{
		rvec[0][2]=-rvec[0][2];
		rvec[1][2]=-rvec[1][2];
		rvec[2][2]=-rvec[2][2];
	}


	for (i=0;i<3;i++)
		tvec[i] = p2[i] - (rvec[i][0]*p1[0]+rvec[i][1]*p1[1]+rvec[i][2]*p1[2]);

	transform._11 = rvec[0][0];
	transform._21 = rvec[0][1];
	transform._31 = rvec[0][2];
	transform._41 = tvec[0];
	transform._12 = rvec[1][0];
	transform._22 = rvec[1][1];
	transform._32 = rvec[1][2];
	transform._42 = tvec[1];
	transform._13 = rvec[2][0];
	transform._23 = rvec[2][1];
	transform._33 = rvec[2][2];
	transform._43 = tvec[2];
	transform._14 = 0;
	transform._24 = 0;
	transform._34 = 0;
	transform._44 = 1.0f;

	double newpi1[MAX_AA_LENGTH][3]={0,};

	for ( int i = 0 ; i < n ; i++ )
	{
		D3DXVECTOR3 out;
		D3DXVECTOR3 vec ( pi1[i][0], pi1[i][1], pi1[i][2] );

		D3DXVec3TransformCoord(&out, &vec, &transform);

		newpi1[i][0] = out.x;
		newpi1[i][1] = out.y;
		newpi1[i][2] = out.z;
	}

	double totalErr = 0.0;
	for (int i=0 ; i<n ; i++ )
	{
		totalErr += (pi2[i][0]-newpi1[i][0])*(pi2[i][0]-newpi1[i][0])+(pi2[i][1]-newpi1[i][1])*(pi2[i][1]-newpi1[i][1])+(pi2[i][2]-newpi1[i][2])*(pi2[i][2]-newpi1[i][2]);
	}

	minrmsd = sqrt(totalErr/n);

	//	TRACE("minrmsd: %f\n" , minrmsd);

	return minrmsd;
}

double GetLocalSuperImposeTransform( IN CVectorArray & arrayPos1, IN long iBegin1 , IN CVectorArray	& arrayPos2, IN long iBegin2, IN long nAtom , OUT D3DXMATRIX &transform )
{
	long n = nAtom;

	int i,j,k;

	double minrmsd = 1e20; // a very big number goes here
	double rmsd;

	double pi1[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double pi2[MAX_AA_LENGTH][3]={0,};  // The list of 3-d points
	double p1[3]={0,}; // The centroid of first point set
	double p2[3]={0,}; // The centroid of second point set
	double qi1[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of first point set
	double qi2[MAX_AA_LENGTH][3]={0,}; // The difference vector (coord - centroid) of second point set
	double H[3][3] = {0,};

	//	result.
	double rvec[3][3] = {0,};
	double tvec[3] = {0,};

	for ( i = 0 ; i < n ; i++ )
	{
		pi1[n-1-i][0] = arrayPos1[iBegin1+i].x;
		pi1[n-1-i][1] = arrayPos1[iBegin1+i].y;
		pi1[n-1-i][2] = arrayPos1[iBegin1+i].z;

		pi2[n-1-i][0] = arrayPos2[iBegin2+i].x;
		pi2[n-1-i][1] = arrayPos2[iBegin2+i].y;
		pi2[n-1-i][2] = arrayPos2[iBegin2+i].z;
	}

	// computation of the centroids
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			p1[j]+=pi1[i][j];
			p2[j]+=pi2[i][j];
		}
	}

	for (j=0;j<3;j++)
	{
		p1[j] /= n;
		p2[j] /= n;
	}
	
	//	difference
	for (i=0;i<n;i++)
	{
		for (j=0;j<3;j++)
		{
			qi1[i][j]=pi1[i][j]-p1[j];
			qi2[i][j]=pi2[i][j]-p2[j];
		}
	}

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			for (k=0;k<n;k++)
			{
				H[i][j]+=(qi1[k][i]*qi2[k][j]);
			}
		}
	}

	//	
	TNT::Array2D<double> matX(3,3, (double *)H);
	JAMA::SVD <double>  svd (matX);

	TNT::Array2D<double> svdU(3,3);
	TNT::Array2D<double> svdV(3,3);

	svd.getU(svdU);
	svd.getV(svdV);

	for (i=0;i<3;i++)
	{
		for (j=0;j<3;j++)
		{
			rvec[i][j] =	svdV[i][0] * svdU[j][0] + 
							svdV[i][1] * svdU[j][1] + 
							svdV[i][2] * svdU[j][2];
		}
	}


	double det1 = ::MAtrixDeterminant( svdU );
	double det2 = ::MAtrixDeterminant( svdV );

	if ( det1 * det2 < 0 )
	{
		rvec[0][2]=-rvec[0][2];
		rvec[1][2]=-rvec[1][2];
		rvec[2][2]=-rvec[2][2];
	}


	for (i=0;i<3;i++)
		tvec[i] = p2[i] - (rvec[i][0]*p1[0]+rvec[i][1]*p1[1]+rvec[i][2]*p1[2]);

	transform._11 = rvec[0][0];
	transform._21 = rvec[0][1];
	transform._31 = rvec[0][2];
	transform._41 = tvec[0];
	transform._12 = rvec[1][0];
	transform._22 = rvec[1][1];
	transform._32 = rvec[1][2];
	transform._42 = tvec[1];
	transform._13 = rvec[2][0];
	transform._23 = rvec[2][1];
	transform._33 = rvec[2][2];
	transform._43 = tvec[2];
	transform._14 = 0;
	transform._24 = 0;
	transform._34 = 0;
	transform._44 = 1.0f;

	double newpi1[MAX_AA_LENGTH][3]={0,};

	for ( int i = 0 ; i < n ; i++ )
	{
		D3DXVECTOR3 out;
		D3DXVECTOR3 vec ( pi1[i][0], pi1[i][1], pi1[i][2] );

		D3DXVec3TransformCoord(&out, &vec, &transform);

		newpi1[i][0] = out.x;
		newpi1[i][1] = out.y;
		newpi1[i][2] = out.z;
	}

	double totalErr = 0.0;
	for (int i=0 ; i<n ; i++ )
	{
		totalErr += (pi2[i][0]-newpi1[i][0])*(pi2[i][0]-newpi1[i][0])+(pi2[i][1]-newpi1[i][1])*(pi2[i][1]-newpi1[i][1])+(pi2[i][2]-newpi1[i][2])*(pi2[i][2]-newpi1[i][2]);
	}

	minrmsd = sqrt(totalErr/n);
	
	//	TRACE("minrmsd: %f\n" , minrmsd);

	return minrmsd;
}

double MAtrixDeterminant( TNT::Array2D<double> &mat )
{
	//3x3 matrix determinant ±¸ÇÏ±â.	
	double r =  (mat[0][0]*(mat[1][1]*mat[2][2]-mat[1][2]*mat[2][1]))-
				(mat[0][1]*(mat[1][0]*mat[2][2]-mat[1][2]*mat[2][0]))+
				(mat[0][2]*(mat[1][0]*mat[2][1]-mat[1][1]*mat[2][0]));
	return r;
}
