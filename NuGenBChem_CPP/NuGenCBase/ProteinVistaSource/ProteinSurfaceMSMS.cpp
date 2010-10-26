#include "stdafx.h"
#include "ProteinVista.h"
#include "Interface.h"
#include "ProteinVistaRenderer.h"
#include "pdb.h"
#include "pdbInst.h"

#include "PDBRenderer.h"
#include "ProteinSurfaceMSMS.h"
#include "Utility.h"

#include "Redirect.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define	SURFACE_CURVATURE_HEADER	(_T("SURFACE_CURVATURE_v06"))

CProteinSurfaceMSMS::CProteinSurfaceMSMS()
{
	m_surfaceGenMethod = SURFACE_MSMS;
}

CProteinSurfaceMSMS::~CProteinSurfaceMSMS()
{

}

HRESULT CProteinSurfaceMSMS::CreateSurface()
{
	HRESULT hr;

	GetMainActiveView()->ResetProgress();

	m_arrayAtom.clear();
	m_arrayAtom.reserve(m_pChain->m_arrayAtom.size());

	for ( int i = 0 ; i < m_pChain->m_arrayAtom.size(); i++ )
	{
		if ( m_bAddHETATM == TRUE )
		{
			m_arrayAtom.push_back(m_pChain->m_arrayAtom[i]);
		}
		else
		{
			if ( m_pChain->m_arrayAtom[i]->m_bHETATM == FALSE )
				m_arrayAtom.push_back(m_pChain->m_arrayAtom[i]);
		}
	}

	if ( m_arrayAtom.size() == 0 )
		return E_FAIL;

	char drive[_MAX_DRIVE];
	char dir[_MAX_DIR];
	char fname[_MAX_FNAME];
	char ext[_MAX_EXT];
	_splitpath(m_pPDB->m_strFilename, drive, dir, fname, ext );

	CString strSurfaceDir = GetMainApp()->m_strBaseSurfacePath;

	m_arrayVertex.clear();
	m_arrayNormal.clear();
	m_arrayIndexFace.clear();
	m_arrayIndexAtom.clear();
	m_arrayTypeVertex.clear();
	m_arrayTypeFace.clear();
	m_ArrayArrayFaceIndex.clear();
	m_ArrayArrayAdjacentVertex.clear();

	m_arrayVertex.reserve(m_arrayAtom.size());
	m_arrayNormal.reserve(m_arrayAtom.size());
	m_arrayIndexFace.reserve(m_arrayAtom.size());
	m_arrayIndexAtom.reserve(m_arrayAtom.size());
	m_arrayTypeVertex.reserve(m_arrayAtom.size());
	m_arrayTypeFace.reserve(m_arrayAtom.size());

	CString outputName;
	outputName.Format ( _T("%s%s_%02d_%c_%03d_%.2f_%d_%d_%d") , strSurfaceDir, fname, m_modelID, m_chainID, m_arrayAtom.size(), m_probeSphere, m_surfaceQuality , m_bAddHETATM , GetTypeGenSurface() );

	CString outputFilenameVertex = outputName + _T(".vert");
	CString outputFilenameFace = outputName + _T(".face");

	BOOL bExistSurface = FALSE;
	HANDLE fileVert = CreateFile( outputFilenameVertex, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL );
	HANDLE fileFace = CreateFile( outputFilenameFace, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL );
	if( INVALID_HANDLE_VALUE != fileVert &&  INVALID_HANDLE_VALUE != fileFace )
	{
		bExistSurface = TRUE;
	}
	if ( fileVert != INVALID_HANDLE_VALUE ) CloseHandle(fileVert);
	if ( fileFace != INVALID_HANDLE_VALUE ) CloseHandle(fileFace);
	
	if ( bExistSurface == FALSE )
	{
		long iProgress = GetMainActiveView()->InitProgress(100);
		GetMainActiveView()->SetProgress(0, iProgress );

		//	Make xyzr file
		CString coordFilename;
		coordFilename.Format ( _T("%s%s_%02d_%c_%03d_%d.xyzr") , strSurfaceDir , fname, m_modelID, m_chainID, m_arrayAtom.size() , m_bAddHETATM );

		CString strCoord;
		strCoord.Preallocate(m_arrayAtom.size()*50);
		
		for ( int i = 0 ; i < m_arrayAtom.size() ; i++ )
		{
			CAtom * pAtom = m_arrayAtom[i];

			CString strLine;
			strLine.Format("%.3f %.3f %.3f %.3f\n", pAtom->m_pos.x, pAtom->m_pos.y, pAtom->m_pos.z, pAtom->m_fRadius );

			strCoord += strLine;
		}

		CFile fileMSMS;
		CFileException ex;
		if ( !fileMSMS.Open(coordFilename, CFile::modeWrite|CFile::modeCreate, &ex) )
		{
			TCHAR szError[1024];
			ex.GetErrorMessage(szError, 1024);

			CString strMsg;
			strMsg.Format( _T("Couldn't open source file: %s"), szError );

			OutputTextMsg(strMsg);

			return E_FAIL;
		}

		GetMainActiveView()->SetProgress(20, iProgress );

		TCHAR * buff = strCoord.GetBuffer();
		fileMSMS.Write(buff, strCoord.GetLength());
		strCoord.ReleaseBuffer();
		fileMSMS.Close();

		GetMainActiveView()->SetProgress(60, iProgress );
		//	Run MSMS
		CString strCommand;
		strCommand.Format(_T("%smsms.exe -if \"%s\" -of \"%s\" -no_header -probe_radius %f -density %f") ,  GetMainApp()->m_strBaseExePath , coordFilename, outputName , m_probeSphere, GetSurfaceQuality(m_surfaceQuality) );

		OutputTextMsg(strCommand);
		//	TODO: directory에 공백이 있을 경우 msms 가 실행되는지 조사.
		if ( m_redirector.Open(strCommand) == FALSE )
		{
			m_redirector.Close();
			GetMainActiveView()->EndProgress(iProgress);
			OutputTextMsg(_T("Cannot execute msms.exe. Check file"));
			return E_FAIL;
		}
		m_redirector.Close();
		GetMainActiveView()->EndProgress(iProgress);
	}
	
	long iProgress = GetMainActiveView()->InitProgress(100);
	GetMainActiveView()->SetProgress(0, iProgress );

	//	Gather results
	{
		CFile fileVert;
		CFileException ex;
		if ( !fileVert.Open(outputFilenameVertex , CFile::modeRead, &ex ) )
		{
			TCHAR szError[1024];
			ex.GetErrorMessage(szError, 1024);

			CString strMsg;
			strMsg.Format( _T("Couldn't open source file: %s"), szError );

			 OutputTextMsg(strMsg);

			return E_FAIL;
		}

		DWORD fileLen = fileVert.GetLength()+1;

		TCHAR * pBuff, * pBuffOrig;
		pBuff = pBuffOrig = new TCHAR [fileLen];
		ZeroMemory(pBuff, fileLen);
		fileVert.Read(pBuff, fileLen);

		while(1)
		{
			TCHAR * pFind = _tcschr(pBuff, _T('\n') );
			if ( pFind == NULL )
				break;

			(*pFind) = _T('\0');

			TCHAR * pLine = pBuff;
			pBuff = pFind+1;

			//	"%9.3f %9.3f %9.3f %9.3f %9.3f %9.3f %7d %7d %2d"

			D3DXVECTOR3		pos;
			D3DXVECTOR3		norm;

			pLine[9] = _T('\0');
			pos.x = _tstof(&pLine[0]);		//	x

			pLine[19] = _T('\0');
			pos.y = _tstof(&pLine[10]);		//	y

			pLine[29] = _T('\0');
			pos.z = _tstof(&pLine[20]);		//	z

			pLine[39] = _T('\0');
			norm.x = _tstof(&pLine[30]);		//	nx

			pLine[49] = _T('\0');
			norm.y = _tstof(&pLine[40]);		//	ny

			pLine[59] = _T('\0');
			norm.z = _tstof(&pLine[50]);		//	nz

			m_arrayVertex.push_back(pos);
			m_arrayNormal.push_back(norm);

			//	
			pLine[67] = _T('\0');
			//	_tstoi(&pLine[60]);	skip

			pLine[75] = _T('\0');
			long indexAtom = _tstoi(&pLine[68])-1;
			m_arrayIndexAtom.push_back(indexAtom);
			
			long typeVertex = _tstoi(&pLine[76]);
			m_arrayTypeVertex.push_back(typeVertex);
		};

		fileVert.Close();

		SAFE_DELETE_AR(pBuffOrig);
	}

	GetMainActiveView()->SetProgress(50, iProgress );
	//
	//	face
	//
	{
		CFile fileFace;
		CFileException ex;
		if ( !fileFace.Open(outputFilenameFace, CFile::modeRead, &ex ) )
		{
			TCHAR szError[1024];
			ex.GetErrorMessage(szError, 1024);

			CString strMsg;
			strMsg.Format( _T("Couldn't open source file: %s"), szError );

			OutputTextMsg(strMsg);

			return E_FAIL;
		}

		DWORD fileLen = fileFace.GetLength()+1;

		TCHAR * pBuff, * pBuffOrig;
		pBuff = pBuffOrig = new TCHAR [fileLen];
		ZeroMemory(pBuff, fileLen);
		fileFace.Read(pBuff, fileLen);

		while(1)
		{
			TCHAR * pFind = _tcschr(pBuff, _T('\n') );
			if ( pFind == NULL )
				break;

			(*pFind) = _T('\0');

			TCHAR * pLine = pBuff;
			pBuff = pFind+1;

			//	"%6d %6d %6d %2d %6d"
			int		index[3];

			pLine[6] = _T('\0');
			index[0] = _tstoi(&pLine[0])-1;		//	i1		1 based index

			pLine[13] = _T('\0');
			index[1] = _tstoi(&pLine[7])-1;		//	i2

			pLine[20] = _T('\0');
			index[2] = _tstoi(&pLine[14])-1;		//	i3

			m_arrayIndexFace.push_back(index[0]);
			m_arrayIndexFace.push_back(index[1]);
			m_arrayIndexFace.push_back(index[2]);

			pLine[23] = _T('\0');
			long typeFace = _tstof(&pLine[21]);		//	nx
			m_arrayTypeFace.push_back(typeFace);

		};

		fileFace.Close();

		SAFE_DELETE_AR(pBuffOrig);
	}

	GetMainActiveView()->EndProgress(iProgress );

	//	m_ArrayArrayFaceIndex 를 구함
	//	세로가 m_pChain->m_arrayAtom 의 index.
	//	가로가 atom 이 가리키는 vertex의 index.
	//	atom 이 가지는 triangle face 의 index를 저장
	{
		m_ArrayArrayFaceIndex.clear();
		//	HETATM 때문에 배열의 index를 m_pChain->m_arrayAtom을 사용한다.
		m_ArrayArrayFaceIndex.resize(m_pChain->m_arrayAtom.size());

		long iProgress = GetMainActiveView()->InitProgress(100);
		GetMainActiveView()->SetProgress(0, iProgress );

		long deltaProgress = m_arrayIndexFace.size()/90;
		deltaProgress -= deltaProgress%3;

		for ( long i = 0 ; i < m_arrayIndexFace.size() ; i+= 3 )
		{
			if ( deltaProgress && (i % deltaProgress == 0) )
				GetMainActiveView()->SetProgress(i*100/m_arrayIndexFace.size() , iProgress);

			long index = m_arrayIndexFace[i];
			long iAtom = m_arrayIndexAtom[index];

			//	HETATM 때문에 배열의 index를 m_pChain->m_arrayAtom을 사용한다.
			long arrayIndex = m_arrayAtom[iAtom]->m_arrayIndex;

			CSTLLONGArray & stlLongArray = m_ArrayArrayFaceIndex[arrayIndex];
			if ( stlLongArray.capacity() == 0 )
				stlLongArray.reserve(3*10);

			//	vertexIndex 를 넣어둔다.
			//	
			//	CAtom 에 Surface의 vertex 와 연결된 vertex Index list를 넣어둔다.
			//	vertex Index list = 3* n개
			//
			stlLongArray.push_back(m_arrayIndexFace[i]);
			stlLongArray.push_back(m_arrayIndexFace[i+1]);
			stlLongArray.push_back(m_arrayIndexFace[i+2]);
		}

		GetMainActiveView()->EndProgress(iProgress);
	}

	return S_OK;
}

//	0..10
float CProteinSurfaceMSMS::GetSurfaceQuality(int quality)
{
	return (quality*0.3) + 1.0;		//	enum Quality--> double quality로 변환
}

long	CProteinSurfaceMSMS::GetTypeGenSurface()
{
	return SURFACE_MSMS;
}

