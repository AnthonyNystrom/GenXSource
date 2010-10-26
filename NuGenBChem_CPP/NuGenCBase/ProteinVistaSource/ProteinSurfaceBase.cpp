#include "stdafx.h"
#include "ProteinVista.h"
#include "Interface.h"
#include "pdb.h"
#include "pdbInst.h"


#include "PDBRenderer.h"
#include "ProteinSurfaceBase.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define	SURFACE_CURVATURE_HEADER	(_T("SURFACE_CURVATURE_v06"))

CProteinSurfaceBase::CProteinSurfaceBase()
{
	m_bUsed = FALSE;
	m_surfaceGenMethod = SURFACE_MSMS;

	for ( int i = 0 ; i < MAX_RING ; i++ )
	{
		m_arrayVertexCurvatureMin[i] = 0.0;
		m_arrayVertexCurvatureMax[i] = 0.0;
	}
}

CProteinSurfaceBase::~CProteinSurfaceBase()
{

}

void	CProteinSurfaceBase::Init ( CPDB * pPDB, CChain * pChain, double probeSphere, int surfaceQuality, BOOL bAddHETATM )
{
	m_pPDB = pPDB;
	m_pChain = pChain;

	m_modelID = pChain->m_iModel;
	m_chainID = pChain->m_chainID;

	m_probeSphere = probeSphere;
	m_surfaceQuality = surfaceQuality;

	m_bAddHETATM = bAddHETATM;
}

float CProteinSurfaceBase::GetSurfaceQuality(int quality)
{
	return quality;
}

HRESULT CProteinSurfaceBase::GetSurfaceAtomInst( CChainInst * pChainInst, CSTLArrayAtomInst & arrayAtomInst )
{
	arrayAtomInst.clear();
	arrayAtomInst.reserve(pChainInst->m_arrayAtomInst.size());

	for ( int i = 0 ; i < pChainInst->m_arrayAtomInst.size(); i++ )
	{
		if ( m_bAddHETATM == TRUE )
		{
			arrayAtomInst.push_back(pChainInst->m_arrayAtomInst[i]);
		}
		else
		{
			if ( pChainInst->m_arrayAtomInst[i]->GetAtom()->m_bHETATM == FALSE )
				arrayAtomInst.push_back(pChainInst->m_arrayAtomInst[i]);
		}
	}

	return S_OK;
}

HRESULT CProteinSurfaceBase::CreateAdjacentVertex()
{
	char drive[_MAX_DRIVE];
	char dir[_MAX_DIR];
	char fname[_MAX_FNAME];
	char ext[_MAX_EXT];
	_splitpath(m_pPDB->m_strFilename, drive, dir, fname, ext );

	CString strSurfaceDir = GetMainApp()->m_strBaseSurfacePath;

	CString outputName;
	outputName.Format ( _T("%s%s_%02d_%c_%03d_%.2f_%d_%d_%d") , strSurfaceDir, fname, m_modelID, m_chainID, m_arrayAtom.size(), m_probeSphere, m_surfaceQuality, m_bAddHETATM, GetTypeGenSurface() );

	CString outputFilenameAdjacentVertex = outputName + _T(".Adjacent");

	BOOL bExistSurface = FALSE;
	HANDLE fileAdjacent = CreateFile( outputFilenameAdjacentVertex, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL );
	if( INVALID_HANDLE_VALUE != fileAdjacent )
	{
		bExistSurface = TRUE;
	}
	if ( fileAdjacent != INVALID_HANDLE_VALUE ) CloseHandle(fileAdjacent);

	BOOL bReadSuccess = FALSE;
	if ( bExistSurface == TRUE )
	{
		CFile fileAdjacentVertex;
		CFileException ex;
		if ( fileAdjacentVertex.Open(outputFilenameAdjacentVertex, CFile::modeRead, &ex) )
		{
			TCHAR buffHeader[512] = {0,};
			fileAdjacentVertex.Read(buffHeader, _tcslen(SURFACE_CURVATURE_HEADER));
			if ( CString(SURFACE_CURVATURE_HEADER) == CString(buffHeader) )
			{
				long numArray;
				fileAdjacentVertex.Read(&numArray, sizeof(long));

				m_ArrayArrayAdjacentVertex.clear();
				m_ArrayArrayAdjacentVertex.resize(numArray);

				long iProgress = GetMainActiveView()->InitProgress(100);
				for ( int i = 0 ; i < m_ArrayArrayAdjacentVertex.size(); i++ )
				{
					if ( i % (m_ArrayArrayAdjacentVertex.size()/100) == 0 )
						GetMainActiveView()->SetProgress(i*100/m_ArrayArrayAdjacentVertex.size(), iProgress);

					CSTLLONGArray & arrayAdjacentVertex = m_ArrayArrayAdjacentVertex[i];

					long numAdjacentVertex;
					fileAdjacentVertex.Read(&numAdjacentVertex, sizeof(long));
					arrayAdjacentVertex.resize(numAdjacentVertex);

					fileAdjacentVertex.Read(&arrayAdjacentVertex[0], sizeof(long)*numAdjacentVertex);
				}
				GetMainActiveView()->EndProgress(iProgress);

				fileAdjacentVertex.Close();

				if ( m_ArrayArrayAdjacentVertex.size() == i )
					bReadSuccess = TRUE;
			}
		}
	}

	if ( bReadSuccess == FALSE )
	{
		//	setArrayAdjacentVertex 를 구함
		m_ArrayArrayAdjacentVertex.clear();
		m_ArrayArrayAdjacentVertex.resize(m_arrayVertex.size());

		long iProgress = GetMainActiveView()->InitProgress(100);

		long deltaProgress = m_arrayIndexFace.size()/90;
		deltaProgress -= deltaProgress%3;

		//	
		for ( int i = 0 ; i < m_arrayIndexFace.size() ; i+=3 )
		{
			if ( deltaProgress && (i % deltaProgress == 0) )
				GetMainActiveView()->SetProgress(i*100/m_arrayIndexFace.size() , iProgress);

			long index1 = m_arrayIndexFace[i];
			long index2 = m_arrayIndexFace[i+1];
			long index3 = m_arrayIndexFace[i+2];

			if ( m_ArrayArrayAdjacentVertex[index1].capacity() == 0 )
				m_ArrayArrayAdjacentVertex[index1].reserve(20);
			if ( m_ArrayArrayAdjacentVertex[index2].capacity() == 0 )
				m_ArrayArrayAdjacentVertex[index2].reserve(20);
			if ( m_ArrayArrayAdjacentVertex[index3].capacity() == 0 )
				m_ArrayArrayAdjacentVertex[index3].reserve(20);

			m_ArrayArrayAdjacentVertex[index1].push_back(index2);
			m_ArrayArrayAdjacentVertex[index1].push_back(index3);

			m_ArrayArrayAdjacentVertex[index2].push_back(index1);
			m_ArrayArrayAdjacentVertex[index2].push_back(index3);

			m_ArrayArrayAdjacentVertex[index3].push_back(index1);
			m_ArrayArrayAdjacentVertex[index3].push_back(index2);
		}

		//	중복되는것 지움
		for ( int i = 0 ; i < m_ArrayArrayAdjacentVertex.size(); i++ )
		{
			CSTLLONGArray & arrayAdjacentVertex = m_ArrayArrayAdjacentVertex[i];
			for ( int j = 0 ; j < arrayAdjacentVertex.size() ; j++ )
			{
				for ( int k = j+1 ; k < arrayAdjacentVertex.size() ; k++ )
				{
					if ( arrayAdjacentVertex[j] == arrayAdjacentVertex[k] )
					{
						//	delete k.
						arrayAdjacentVertex.erase(arrayAdjacentVertex.begin()+k);
						//	20091010 수정.
						k--;
					}
				}
			}
		}

		//	outputFilenameAdjacentVertex 로 저장.
		CFile fileAdjacentVertex;
		CFileException ex;
		if ( !fileAdjacentVertex.Open(outputFilenameAdjacentVertex, CFile::modeWrite|CFile::modeCreate, &ex) )
		{
			fileAdjacentVertex.Write(SURFACE_CURVATURE_HEADER, _tcslen(SURFACE_CURVATURE_HEADER));

			long numArray = m_ArrayArrayAdjacentVertex.size();
			fileAdjacentVertex.Write(&numArray, sizeof(long));

			for ( int i = 0 ; i < m_ArrayArrayAdjacentVertex.size(); i++ )
			{
				CSTLLONGArray & arrayAdjacentVertex = m_ArrayArrayAdjacentVertex[i];

				long numAdjacentVertex = arrayAdjacentVertex.size();
				fileAdjacentVertex.Write(&numAdjacentVertex, sizeof(long));
				fileAdjacentVertex.Write(&arrayAdjacentVertex[0], sizeof(long)*arrayAdjacentVertex.size());
			}

			fileAdjacentVertex.Close();
		}
		GetMainActiveView()->EndProgress(iProgress);
	}

	return S_OK;
}

//
//    
void CProteinSurfaceBase::CalculateCurvature(int depthLimit )
{
	if ( m_arrayVertexCurvature[depthLimit].size() > 0 )
		return;

	//	저장된것이 있으면 load 한다.
	char drive[_MAX_DRIVE];
	char dir[_MAX_DIR];
	char fname[_MAX_FNAME];
	char ext[_MAX_EXT];
	_splitpath(m_pPDB->m_strFilename, drive, dir, fname, ext );

	CString strSurfaceDir = GetMainApp()->m_strBaseSurfacePath;

	CString outputName;
	outputName.Format ( _T("%s%s_%02d_%c_%03d_%.2f_%d_%d_%d_%d") , strSurfaceDir, fname, m_modelID, m_chainID, m_arrayAtom.size(), m_probeSphere, m_surfaceQuality, m_bAddHETATM , depthLimit, GetTypeGenSurface() );

	CString outputFilenameCurvature = outputName + _T(".Curvature");

	BOOL	bLoadSuccess = FALSE;
	CFile fileCurvature;
	CFileException ex;
	if ( fileCurvature.Open(outputFilenameCurvature, CFile::modeRead, &ex) )
	{
		TCHAR buffHeader[512] = {0,};
		fileCurvature.Read(buffHeader, _tcslen(SURFACE_CURVATURE_HEADER));
		if ( CString(SURFACE_CURVATURE_HEADER) == CString(buffHeader) )
		{
			FLOAT min, max;
			fileCurvature.Read(&min, sizeof(FLOAT));
			fileCurvature.Read(&max, sizeof(FLOAT));

			m_arrayVertexCurvatureMin[depthLimit] = min;
			m_arrayVertexCurvatureMax[depthLimit] = max;

			long	nArray;
			fileCurvature.Read(&nArray, sizeof(long));

			m_arrayVertexCurvature[depthLimit].clear();
			m_arrayVertexCurvature[depthLimit].resize(nArray);

			fileCurvature.Read(&m_arrayVertexCurvature[depthLimit][0], sizeof(FLOAT)*nArray);
			fileCurvature.Close();

			bLoadSuccess = TRUE;
		}
	}

	if ( bLoadSuccess == FALSE )
	{
		m_arrayVertexCurvature[depthLimit].reserve(m_arrayVertex.size());

		if ( m_arrayVertexCurvature[depthLimit].size() == 0 )
		{
			long iProgress = GetMainActiveView()->InitProgress(100);
			for ( long vertexIndex = 0 ; vertexIndex < m_arrayVertex.size() ; vertexIndex++ )
			{
				if ( vertexIndex % (m_arrayVertex.size()/100) == 0 )
					GetMainActiveView()->SetProgress( ((vertexIndex+1)*100)/(m_arrayVertex.size()) , iProgress );

				int		depth = 0;
				float	sumCurvature = 0.0f;
				CSTLLONGArray arrayPoint;
				arrayPoint.reserve(100);

				CalcAdjacentCurvature(vertexIndex, sumCurvature, depth, depthLimit, arrayPoint );

				m_arrayVertexCurvature[depthLimit].push_back(sumCurvature);
			}

			//    apply color to real vertex.
			//    find min,max
			FLOAT minValue = 1e20;
			FLOAT maxValue = -1e20;
			for ( int i = 0 ; i < m_arrayVertexCurvature[depthLimit].size(); i++ )
			{
				if ( m_arrayVertexCurvature[depthLimit][i] > maxValue )
					maxValue = m_arrayVertexCurvature[depthLimit][i];
				if ( m_arrayVertexCurvature[depthLimit][i] < minValue )
					minValue = m_arrayVertexCurvature[depthLimit][i];
			}

			m_arrayVertexCurvatureMin[depthLimit] = minValue;
			m_arrayVertexCurvatureMax[depthLimit] = maxValue;

			GetMainActiveView()->EndProgress(iProgress);
		}

		//
		//	outputFilenameCurvature 로 저장.
		//	
		CFile fileCurvature;
		CFileException ex;
		if ( fileCurvature.Open(outputFilenameCurvature, CFile::modeWrite|CFile::modeCreate, &ex) )
		{
			fileCurvature.Write(SURFACE_CURVATURE_HEADER, _tcslen(SURFACE_CURVATURE_HEADER));

			fileCurvature.Write(&m_arrayVertexCurvatureMin[depthLimit], sizeof(FLOAT));
			fileCurvature.Write(&m_arrayVertexCurvatureMax[depthLimit], sizeof(FLOAT));

			long numArray = m_arrayVertexCurvature[depthLimit].size();
			fileCurvature.Write(&numArray, sizeof(long));
			fileCurvature.Write(&m_arrayVertexCurvature[depthLimit][0], sizeof(FLOAT)*m_arrayVertexCurvature[depthLimit].size());

			fileCurvature.Close();
		}
	}
}

float CProteinSurfaceBase::CalcAdjacentCurvature(int iVertex, float &sumCurvature, int depth, int depthLimit, CSTLLONGArray & arrayPoint )
{
	if ( m_ArrayArrayAdjacentVertex.size() == 0 )
		CreateAdjacentVertex();

	CSTLLONGArray & adjacentIndexArray = m_ArrayArrayAdjacentVertex[iVertex];

	int lengthOfAdjacentIndexArrayLength = adjacentIndexArray.size();

	CSTLLONGArray::iterator iterator;

	arrayPoint.push_back(iVertex);

	float dotProduct = 0.0;
	for ( iterator= adjacentIndexArray.begin( ); iterator != adjacentIndexArray.end( ); iterator++ )
	{
		long iAdjacentVertex = (*iterator);
		D3DXVECTOR3	edgeVec( m_arrayVertex[iAdjacentVertex] - m_arrayVertex[iVertex] );
		D3DXVec3Normalize( &edgeVec, &edgeVec );

		D3DXVECTOR3	vecNormal(m_arrayNormal[iVertex] );
		D3DXVec3Normalize( &vecNormal, &vecNormal );

		dotProduct += D3DXVec3Dot(&edgeVec, &vecNormal);

		if ( depth < depthLimit )
		{
			//    이미 계산했는지를 조사.
			BOOL bFind = FALSE;
			for ( int i = 0 ; i < arrayPoint.size() ; i++ )
			{
				if ( arrayPoint[i] == iAdjacentVertex )
				{
					bFind = TRUE;
					break;
				}
			}

			if ( bFind == FALSE )
				CalcAdjacentCurvature ( iAdjacentVertex, sumCurvature, depth+1, depthLimit, arrayPoint );
		}
	}

	sumCurvature += dotProduct;
	return sumCurvature;
}
