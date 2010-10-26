#include "stdafx.h"
#include "pdb.h"
#include "pdbInst.h"

#include <math.h>
#include "ColorScheme.h"
#include "ProteinVista.h"
#include "Utils.h"
#include "Interface.h"
using namespace System;
using namespace System::IO;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
 
float g_atomSphereRadius[MAX_ATOM+2] = {
		ATOM_RADIUS_C, ATOM_RADIUS_N, ATOM_RADIUS_O, ATOM_RADIUS_S, ATOM_RADIUS_H, ATOM_RADIUS_P,
		ATOM_RADIUS_CL, ATOM_RADIUS_ZN, ATOM_RADIUS_NA, ATOM_RADIUS_FE, ATOM_RADIUS_MG, ATOM_RADIUS_K,
		ATOM_RADIUS_CA, ATOM_RADIUS_I, ATOM_RADIUS_F, ATOM_RADIUS_B, ATOM_RADIUS_UNKNOWN , 0.25f, 0.5f };

CPDB::CPDB()
{
	m_pCurrentBuffPDB = NULL;

	m_iArrayModel = 0;
	m_iCurrentModel = 0;

	m_pCurrentChain = NULL;
	m_pModel = NULL;

	m_currentChainID = 0;
	
	m_assignBlankChainID = '-';

	m_bUseStride = FALSE;

	m_pPDBRenderer = NULL;

	m_pdbCenter.x = m_pdbCenter.y = m_pdbCenter.z = 0;

	m_strArrayHeader.SetSize(0, 16);
	m_strArrayTitle.SetSize(0, 16);
	m_strArraySource.SetSize(0, 16);
	m_strArrayKeywords.SetSize(0, 16);
	m_strArrayEXPDTA.SetSize(0, 16);
	m_strArrayAuthor.SetSize(0, 16);
	m_strArrayRevisionDate.SetSize(0, 16);
	m_strArrayJournalCitation.SetSize(0, 16);
	m_strBioUnit.SetSize(0, 16);
	m_strArrayBioUnitChain.SetSize(0, 16);

	m_strArrayRemark.SetSize(0, 4000);

	m_arrayHelix.reserve(1000);
	m_arraySheet.reserve(1000);

	D3DXMatrixIdentity(&m_matScale);

	m_bExistBioUnit = FALSE;

	m_pdbMinMaxBB[0] = D3DXVECTOR3(1e20, 1e20, 1e20);
	m_pdbMinMaxBB[1] = D3DXVECTOR3(-1e20, -1e20, -1e20);
}

CPDB::~CPDB()
{
	Destroy();
}

HRESULT CPDB::AnalizeStructure() 
{ 
	//
	//	2차구조가 없을때 stride 실행.
	//	
	if ( m_arrayHelix.size() == 0 && m_arraySheet.size() == 0 )
	{
		//	stride를 돌려서, 2차구조가 없을 경우 만들어 낸다.
		StrideWrapper();
		SetChainSecondaryStructure();
	}
 
	FindBond(); 
	return S_OK; 
}

//
//


HRESULT CPDB::FindBond()
{
	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			(m_arrayarrayChain[i])[j]->FindBond();
		}

	}

	return S_OK;
}


HRESULT CPDB::SetAtomTypeAndResidueIndex()
{
	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			(m_arrayarrayChain[i])[j]->SetAtomTypeAndResidueIndex();
		}

	}

	return S_OK;
}
HRESULT CPDB::SetSecondaryStructureAtomAssign()
{
	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			(m_arrayarrayChain[i])[j]->SetSecondaryStructureAtomAssign();
		}
	}

	return S_OK;
}
HRESULT CPDB::SetChainSecondaryStructure()
{
	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			(m_arrayarrayChain[i])[j]->SetChainSecondaryStructure();
		}
	}

	return S_OK;
}

HRESULT CPDB::SetSequenceData()
{
	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			(m_arrayarrayChain[i])[j]->SetSequenceData();
		}
	}

	return S_OK;
}

HRESULT CPDB::FindCenterRadiusBB()
{
	long nCenter = 0;
	D3DXVECTOR3 center(0,0,0);

	D3DXVECTOR3 centerPDB(0,0,0);
	long nCount = 0;

	D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
	D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);

	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			(m_arrayarrayChain[i])[j]->FindCenterRadius();

			D3DXVECTOR3 pos1 = (m_arrayarrayChain[i])[j]->m_chainMinMaxBB[1];
			D3DXVECTOR3 pos2 = (m_arrayarrayChain[i])[j]->m_chainMinMaxBB[0];

			if (maxBB.x < pos1.x) maxBB.x = pos1.x;
			if (maxBB.y < pos1.y) maxBB.y = pos1.y;
			if (maxBB.z < pos1.z) maxBB.z = pos1.z;
			if (minBB.x > pos1.x) minBB.x = pos1.x;
			if (minBB.y > pos1.y) minBB.y = pos1.y;
			if (minBB.z > pos1.z) minBB.z = pos1.z;

			if (maxBB.x < pos2.x) maxBB.x = pos2.x;
			if (maxBB.y < pos2.y) maxBB.y = pos2.y;
			if (maxBB.z < pos2.z) maxBB.z = pos2.z;
			if (minBB.x > pos2.x) minBB.x = pos2.x;
			if (minBB.y > pos2.y) minBB.y = pos2.y;
			if (minBB.z > pos2.z) minBB.z = pos2.z;
		}
	}

	m_pdbCenter = (minBB + maxBB)/2.0f;
	m_pdbMinMaxBB[0] = minBB;
	m_pdbMinMaxBB[1] = maxBB;
	D3DXVECTOR3 temp = maxBB-m_pdbCenter;
	m_pdbRadius = D3DXVec3Length(&temp) + 4.0f;

	return S_OK;
}

//=====================================================================================
//=====================================================================================
TCHAR * ReadString(TCHAR * buff, CString & str)
{
	TCHAR * pLine = strchr(buff, '\n');
	if ( pLine == NULL )
		return NULL;

	TCHAR * pFind = pLine+1;

	str.GetBufferSetLength(pFind-buff);
	str.LockBuffer();
	TCHAR * pDest = str.GetBuffer(pFind-buff);

	strncpy(pDest, buff, pFind-buff );
	pDest[pFind-buff] = '\0';
	str.UnlockBuffer();
	return pFind;
}

//=====================================================================================
//=====================================================================================

HRESULT BridgeTitleSection(CPDB * pPDB, long index, CString &str)
{
	pPDB->TitleSection(index, str);
	return S_OK;
}

HRESULT BridgeMODELField(CPDB * pPDB, long index, CString &str)
{
	pPDB->MODELField(index, str);
	return S_OK;
}

HRESULT BridgeATOMField(CPDB * pPDB, long index, CString &str)
{
	pPDB->ATOMField(index, str, TRUE);
	return S_OK;
}
HRESULT BridgeSIGATMField(CPDB * pPDB, long index, CString &str)
{
	pPDB->SIGATMField(index, str);
	return S_OK;
}
HRESULT BridgeANISOUField(CPDB * pPDB, long index, CString &str)
{
	pPDB->ANISOUField(index, str);
	return S_OK;
}
HRESULT BridgeSIGUIJField(CPDB * pPDB, long index, CString &str)
{
	pPDB->SIGUIJField(index, str);
	return S_OK;
}
HRESULT BridgeTERField(CPDB * pPDB, long index, CString &str)
{
	pPDB->TERField(index, str);
	return S_OK;
}
HRESULT BridgeHETATMField(CPDB * pPDB, long index, CString &str)
{
	pPDB->ATOMField(index, str, FALSE);
	//    pPDB->HETATMField(index, str);
	return S_OK;
}
HRESULT BridgeENDMDLField(CPDB * pPDB, long index, CString &str)
{
	pPDB->ENDMDLField(index, str);
	return S_OK;
}
HRESULT BridgeENDField(CPDB * pPDB, long index, CString &str)
{
	pPDB->ENDField(index, str);
	return S_OK;
}

HRESULT BridgeHELIXField(CPDB * pPDB, long index, CString &str)
{
	pPDB->HELIXField(index, str);
	return S_OK;
}

HRESULT BridgeSHEETField(CPDB * pPDB, long index, CString &str)
{
	pPDB->SHEETField(index, str);
	return S_OK;
}

HRESULT BridgeSCALEField(CPDB * pPDB, long index, CString &str)
{
	//	pPDB->SCALEField(index, str);
	return S_OK;
}


//=====================================================================================
 
HRESULT CPDB::Load(const TCHAR * filename, BOOL bDebug )
{
	System::IO::FileInfo^ fileInfo = gcnew System::IO::FileInfo(TWCharToMStr(filename));

	if (System::String::Compare(fileInfo->Extension,".pdb",true) == 0||
		System::String::Compare(fileInfo->Extension,".ent",true) == 0)
	{
		return LoadPDB( filename, bDebug );
	}
	return S_OK;
}
 
HRESULT CPDB::LoadPDB(const TCHAR * filename, BOOL bDebug )
{
//	TRACE("Load Filename: %s\n", filename );

	m_strFilename = filename;
	
	{
		char drive[_MAX_DRIVE];
		char dir[_MAX_DIR];
		char fname[_MAX_FNAME];
		char ext[_MAX_EXT];
		
		_splitpath( filename, drive, dir, fname, ext );

		m_strPDBID = CString(fname);
	}

	m_pCurrentBuffPDB = NULL;

	try
	{
		TCHAR * pBuffOrigin;
		CFile file(filename, CFile::modeRead|CFile::typeBinary|CFile::shareDenyNone );
		DWORD fileLen = file.GetLength()+1;

		pBuffOrigin = m_pCurrentBuffPDB = new TCHAR [fileLen];
		ZeroMemory(m_pCurrentBuffPDB, fileLen);
		file.Read(m_pCurrentBuffPDB, fileLen);

		//	순서가 바뀌어도 되지만, 성능이 떨어진다.
		static CString		 strFieldName[] =  {		
			//	1				2			3				4			5				6			7				8			9
			_T("HEADER"), _T("TITLE"), _T("SOURCE"), _T("KEYWDS"), _T("EXPDTA"), _T("AUTHOR"), _T("REVDAT"), _T("JRNL"), _T("REMARK"), 
			_T("DBREF"), _T("SEQADV"), _T("SEQRES"), _T("MODRES"), _T("HELIX"), _T("SHEET"), _T("TURN"), _T("SCALE1"), _T("SCALE2"), _T("SCALE3"),
			_T("MODEL"), _T("ATOM"), _T("SIGATM"), _T("ANISOU"), _T("SIGUIJ"), _T("TER"), _T("HETATM"), _T("ENDMDL"),
			_T("CONECT"), _T("MASTER"), _T("END"),

		};

		static PDBFunction PDBFunctionList[] = {
			BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, BridgeTitleSection, 
				NULL,	NULL,	NULL,	NULL,	BridgeHELIXField ,	BridgeSHEETField ,	NULL, BridgeSCALEField, BridgeSCALEField, BridgeSCALEField,  
				BridgeMODELField,	BridgeATOMField,	BridgeSIGATMField,	BridgeANISOUField,	BridgeSIGUIJField,	BridgeTERField,	BridgeHETATMField,	BridgeENDMDLField,
				NULL, NULL, BridgeENDField
		};

		BOOL	bEndField = FALSE;
		long currentStartIndex = 0;
		while(bEndField == FALSE)
		{
			CString strField;
			m_pCurrentBuffPDB = ::ReadString(m_pCurrentBuffPDB, strField);
			if ( m_pCurrentBuffPDB == NULL )
				break;

			CString strCurrentFieldName = strField.Mid(0,6);
			strCurrentFieldName.TrimLeft();
			strCurrentFieldName.TrimRight();

			long nTypeField = sizeof(strFieldName)/sizeof(CString *);
			long iCurrent;

			//	field 가 차례대로 나온다.
			for ( iCurrent = currentStartIndex ; iCurrent < currentStartIndex+nTypeField ; iCurrent++ )
			{
				long index = iCurrent%nTypeField;
				if ( strCurrentFieldName == strFieldName[index] )
				{
					if ( PDBFunctionList[index] )
						PDBFunctionList[index](this, index, strField);
					currentStartIndex = index;
					if (CString (_T("END")) == strFieldName[index] )
						bEndField = TRUE;
					break;
				}
			}
		}

		file.Close();
		delete [] pBuffOrigin;
	}
	catch(CFileException *e)
	{
		if( e->m_cause == CFileException::fileNotFound )
		{
				//	AfxMessageBox("ERROR: File not found");
		}
		else
		{
				//	AfxMessageBox("ERROR: File Open Error");
		}

		return E_FAIL;
	}

	//	마지막에 END, ENDMDL, TER 아무것도 없이 그냥 끝났을 경우.
	//	예전 파일 호환성을 위해서 만듬 
	if ( m_pCurrentChain != NULL )
	{
		if ( (m_pCurrentChain->m_arrayAtom.size() != 0) || (m_pCurrentChain->m_arrayHETATM.size() != 0) )
		{
			BOOL bRet = ValidateChain(m_pCurrentChain);
			if ( bRet == TRUE )
			{
				//	MODEL field가 없는 PDB를 위해서 공간을 만들어준다.
				while ( m_arrayarrayChain.size() <= m_iArrayModel )
				{
					CSTLArrayChain arrayChainTemp;
					m_arrayarrayChain.push_back(arrayChainTemp);
				}

				/*
				if ( m_iArrayModel == 0 && m_arrayarrayChain.size() == 0 ) 
				{	//	MODEL field가 없는 PDB를 위해서 하나를 넣어준다.
					CSTLArrayChain arrayChainTemp;
					m_arrayarrayChain.push_back(arrayChainTemp);
				}
				*/

				BOOL	bAlreadyExist = FALSE;
				for ( int iChain1= 0 ; iChain1 < m_arrayarrayChain[m_iArrayModel].size() ; iChain1++ )
				{
					CChain * pChain = m_arrayarrayChain[m_iArrayModel][iChain1];
					if ( pChain == m_pCurrentChain )
						bAlreadyExist = TRUE;
				}

				if ( bAlreadyExist == FALSE )
				{
					m_arrayarrayChain[m_iArrayModel].push_back(m_pCurrentChain);
					m_pCurrentChain->m_arrayIndex = m_arrayarrayChain[m_iArrayModel].size()-1;
					if ( m_pModel )
					{
						m_pModel->m_arrayChain.push_back(m_pCurrentChain);
						m_pCurrentChain->m_arrayIndexModel = m_pModel->m_arrayChain.size()-1;
						m_arrayModel.push_back(m_pModel);
					}
				}
			}
			else
			{
				//	버린다.
				delete m_pCurrentChain;
				if ( m_pModel ) 
					delete m_pModel;
			}

			m_pCurrentChain = NULL;
			m_pModel = NULL;
		}
	}

	//	
	//	MODEL 이 없는 PDB 일 경우에 MODEL 을 1개 만들어 chain을 모두 넣는다.
	if ( m_arrayModel.size() == 0 )
	{
		CModel * pModel = new CModel;

		pModel->m_bValidTreeCtrl = FALSE;
		pModel->m_iModel = -1;
		pModel->m_pPDB = this;

		for ( int i = 0 ; i < m_arrayarrayChain[0].size() ; i++ )
		{
			pModel->m_arrayChain.push_back(m_arrayarrayChain[0][i]);
			m_arrayarrayChain[0][i]->m_arrayIndexModel = pModel->m_arrayChain.size()-1;
		}

		m_arrayModel.push_back(pModel);
	}

	long iProgress = GetMainActiveView()->InitProgress(100);

	GetMainActiveView()->SetProgress(0, iProgress );
	//	
	//	chain id 를 정리한다. 
	//	PDB 에 들어있는 경우는 대문자 'A', .. 이고,
	//	chain ID 가 1개이어서 ' ' 인 경우에는 소문자 'a', 'b', ... 로 사용
	//
	ModifyPDBChain();

	//	BioUnit Transform을 생성한다.
	MakeBioUnitTransform();

	GetMainActiveView()->SetProgress(10, iProgress);
	if ( IsExecuteStride() == TRUE )
	{
		m_arrayHelix.clear();
		m_arraySheet.clear();
	}

	GetMainActiveView()->SetProgress(30, iProgress );
	SetAtomTypeAndResidueIndex();

	GetMainActiveView()->SetProgress(40, iProgress );
	SetSequenceData();

	GetMainActiveView()->SetProgress(50, iProgress );
	FindCenterRadiusBB();

	//	arrayAtom 에 2차구조를 표시
	GetMainActiveView()->SetProgress(80, iProgress );
	SetSecondaryStructureAtomAssign();

	//	arrayAtom 을 가지고 chain의 SS를 재구성
	GetMainActiveView()->SetProgress(99, iProgress );
	SetChainSecondaryStructure();

	SetAtomParentPointer();

	GetMainActiveView()->SetProgress(100, iProgress );

	GetMainActiveView()->EndProgress(iProgress);

	return S_OK;
}
 
HRESULT	CPDB::TitleSection(long index, CString &strField)
{
	//										0					1				2					3					4					5					6							7						8
	CStringArray * arraySection[] = { &m_strArrayHeader, &m_strArrayTitle, &m_strArraySource, &m_strArrayKeywords, &m_strArrayEXPDTA, &m_strArrayAuthor, &m_strArrayRevisionDate, &m_strArrayJournalCitation, &m_strArrayRemark };
	long		 contentsIndex[]  ={		11-1,				11-1,			11-1,					11-1,				11-1,				11-1,			 11-1, 						13-1,						12-1	};	

	CString strFieldContents = strField.Mid(contentsIndex[index]);
	strFieldContents.TrimLeft();
	strFieldContents.TrimRight();
	arraySection[index]->Add(strFieldContents);

/*
0		  1         2         3         4         5         6         7      
012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
REMARK 350 APPLY THE FOLLOWING TO CHAINS: A, B                      
REMARK 350                    AND CHAINS: J, K, L, M, N, O, P, Q, T
REMARK 350   BIOMT1   1  1.000000  0.000000  0.000000        0.00000            
REMARK 350   BIOMT2   1  0.000000  1.000000  0.000000        0.00000            
REMARK 350   BIOMT3   1  0.000000  0.000000  1.000000        0.00000            
*/
	if ( index == 8 && strField.Mid(7,3) == _T("350"))	//	REMARK 350
	{
		if ( strField.Mid(11, 41-11) == _T("APPLY THE FOLLOWING TO CHAINS:" ) )
		{
			m_strBioUnit.Add( strField );
		}
		else
		if ( strField.Mid(11, 41-11) == _T("                   AND CHAINS:" ) )
		{
			m_strBioUnit.Add( strField );
		}
		else
		if ( strField.Mid(13, 5) == _T("BIOMT" ) )
		{
			m_strBioUnit.Add( strField );
		}
	}

	return S_OK;
}

//=====================================================================================
//=====================================================================================

HRESULT CPDB::MODELField(long , CString &str)
{
	CString strFieldContents = str.Mid(10, 14-11+1);
	long index = atoi(strFieldContents);
	//	PDB 안의 모델 번호
	m_iCurrentModel = index;

	if ( m_pModel != NULL )
	{
		m_arrayModel.push_back(m_pModel);
		m_pModel = NULL;
	}

	m_pModel = new CModel;
	m_pModel->m_iModel = m_iCurrentModel;
	m_pModel->m_pPDB = this;

	//	새로운 체인 하나 추가.
	CSTLArrayChain	arrayChainTemp;
	m_arrayarrayChain.push_back(arrayChainTemp);
	m_iArrayModel = m_arrayarrayChain.size()-1;				//	어레이의 index.

	return S_OK;
}

HRESULT CPDB::ATOMField(long , CString &strField, BOOL bAtom )
{
	CAtom * pAtom = new CAtom;

	pAtom->m_serial = atoi(strField.Mid(6, 5));

	CString temp = strField.Mid(12,4);
	pAtom->m_atomName = temp;

	pAtom->m_altLoc = strField[16];

	temp = strField.Mid(17, 3);
	temp.TrimLeft();
	temp.TrimRight();
	pAtom->m_residueName = temp;

	pAtom->m_chainID = strField[21];
	pAtom->m_residueNum = atoi(strField.Mid(22, 4));

	pAtom->m_pos.x = (FLOAT)atof(strField.Mid(30,8));
	pAtom->m_pos.y = (FLOAT)atof(strField.Mid(38,8));
	pAtom->m_pos.z = -(FLOAT)atof(strField.Mid(46,8));
	pAtom->m_posOrig = pAtom->m_pos;
	pAtom->m_occupancy = (FLOAT)atof(strField.Mid(54,6));
	pAtom->m_temperature = (FLOAT)atof(strField.Mid(60,6));

	if ( bAtom == FALSE )
	{
		pAtom->m_bHETATM = TRUE;

		//	HETATM --> ATOM으로 변경시키는 조건에 따라 HETATM을 ATOM으로 바꾼다.
		//	조건: serial+1 == 현재 serial
		if ( m_pCurrentChain )
		{
			if ( m_pCurrentChain->m_arrayAtom.size() > 0 )
			{	
				CAtom * pAtomPrev = m_pCurrentChain->m_arrayAtom[m_pCurrentChain->m_arrayAtom.size()-1];
				if ( pAtomPrev->m_serial	+ 1 == pAtom->m_serial )
				{
					pAtom->m_bHETATM = FALSE;
					bAtom = TRUE;
				}
			}
		}
	}
	else
	{
		pAtom->m_bHETATM = FALSE;
	}

	pAtom->SetAtomIndex();
	pAtom->SetAtomMass();

	//	Atom Radius
	pAtom->m_fRadius = g_atomSphereRadius[pAtom->m_atomNameIndex];
	
	pAtom->m_strPDBID = m_strPDBID;

	if ( m_currentChainID != pAtom->m_chainID )
	{
		//	새로운 체인이 시작.

		if ( m_pCurrentChain != NULL )		
		{	//	기존에 체인이 있을 경우에..
			CChain * pFindChain = NULL;
			BOOL	bAlreadyExist = FALSE;

			//    같은 모델번호와 chain-id를 가진 chain이 이미 존재하면 그것을 사용.
			for ( int iModel= 0 ; iModel < m_arrayarrayChain.size() ; iModel++ )
			{
				for ( int iChain1= 0 ; iChain1 < m_arrayarrayChain[iModel].size() ; iChain1++ )
				{
					CChain * pChain = m_arrayarrayChain[iModel][iChain1];

					if ( pChain->m_iModel != m_iCurrentModel ) 
								break;

					if ( pChain == m_pCurrentChain )
						bAlreadyExist = TRUE;
					if ( pChain->m_chainID == pAtom->m_chainID )
					{	//	모델 번호 && 체인 ID 가 같은것이 있다.
						pFindChain = pChain;
					}
				}
			}

			BOOL bRet = ValidateChain(m_pCurrentChain);
			if ( bRet == TRUE )
			{
				if ( bAlreadyExist == FALSE )
				{
					//	MODEL field가 없는 PDB를 위해서 공간을 만들어준다.
					while ( m_arrayarrayChain.size() <= m_iArrayModel )
					{
						CSTLArrayChain arrayChainTemp;
						m_arrayarrayChain.push_back(arrayChainTemp);
					}

					
					//if ( m_iArrayModel == 0 && m_arrayarrayChain.size() == 0 ) 
					//{	//	MODEL field이 없는 PDB를 위해서 하나를 넣어준다.
					//	CSTLArrayChain arrayChainTemp;
					//	m_arrayarrayChain.push_back(arrayChainTemp);
					//}

					m_arrayarrayChain[m_iArrayModel].push_back(m_pCurrentChain);
					m_pCurrentChain->m_arrayIndex = m_arrayarrayChain[m_iArrayModel].size()-1;
					if ( m_pModel )
					{
						m_pModel->m_arrayChain.push_back(m_pCurrentChain);
						m_pCurrentChain->m_arrayIndexModel = m_pModel->m_arrayChain.size()-1;
					}
				}
			}
			else
			{
				//	버린다.
				delete m_pCurrentChain;
			}

			m_pCurrentChain = pFindChain;

		}
	}

	if ( m_pCurrentChain == NULL )
	{
		m_pCurrentChain = new CChain();
		m_pCurrentChain->m_pPDB = this;
		m_pCurrentChain->m_iModel = m_iCurrentModel;
		m_pCurrentChain->m_chainID = pAtom->m_chainID;
		m_pCurrentChain->m_strPDBID = m_strPDBID + _T("_") + m_pCurrentChain->m_chainID;
	}

	if ( bAtom == TRUE )
		m_pCurrentChain->m_arrayAtom.push_back(pAtom);
	else
		m_pCurrentChain->m_arrayHETATM.push_back(pAtom);

	m_currentChainID = pAtom->m_chainID;

	return S_OK;
}

//	
//    obsolete....!!!!
//
HRESULT CPDB::HETATMField(long , CString &strField)
{
	/*
	CAtom * pAtom = new CAtom;

	pAtom->m_serial = atoi(strField.Mid(6, 5));

	CString temp = strField.Mid(12,4);
	pAtom->m_atomName = temp;
	pAtom->m_bHETATM = TRUE;

	temp = strField.Mid(17, 3);
	temp.TrimLeft();
	temp.TrimRight();
	pAtom->m_residueName = temp;

	pAtom->m_chainID = strField[21];
	pAtom->m_residueNum = atoi(strField.Mid(22, 4));
	pAtom->m_pos.x = (FLOAT)atof(strField.Mid(30,8));
	pAtom->m_pos.y = (FLOAT)atof(strField.Mid(38,8));
	pAtom->m_pos.z = -(FLOAT)atof(strField.Mid(46,8));
	pAtom->m_posOrig = pAtom->m_pos;
	pAtom->m_occupancy = (FLOAT)atof(strField.Mid(54,6));
	pAtom->m_temperature = (FLOAT)atof(strField.Mid(60,6));

	pAtom->SetAtomIndex();
	//	pAtom->SetDefaultAtomColor();
	pAtom->SetAtomMass();

	pAtom->m_strPDBID = m_strPDBID;

	//
	if ( m_currentChainID != pAtom->m_chainID )
	{
		//	새로운 체인이 시작.
		if ( m_pCurrentChain != NULL )
		{
			CChain * pFindChain = NULL;
			BOOL	bAlreadyExist = FALSE;

			//    같은 모델번호와 chain-id를 가진 chain이 이미 존재하면 그것을 사용.
			long iModel = m_iCurrentModel;
			for ( int iChain1= 0 ; iChain1 < m_arrayarrayChain[iModel].size() ; iChain1++ )
			{
				CChain * pChain = m_arrayarrayChain[iModel][iChain1];
				if ( pChain == m_pCurrentChain )
					bAlreadyExist = TRUE;
				if ( pChain->m_chainID == pAtom->m_chainID )
				{	//	모델 번호 && 체인 ID 가 같은것이 있다.
					pFindChain = pChain;
				}
			}

			//
			BOOL bRet = ValidateChain(m_pCurrentChain);
			if ( bRet == TRUE )
			{
				if ( bAlreadyExist == FALSE )
				{
					m_arrayarrayChain[m_iArrayModel].push_back(m_pCurrentChain);
					m_pCurrentChain->m_arrayIndex = m_arrayarrayChain[m_iArrayModel].size()-1;
					if ( m_pModel )
					{
						m_pModel->m_arrayChain.push_back(m_pCurrentChain);
						m_pCurrentChain->m_arrayIndexModel = m_pModel->m_arrayChain.size()-1;
					}
				}
			}
			else
			{
				//	버린다.
				delete m_pCurrentChain;
			}
			m_pCurrentChain = pFindChain;

		}
	}

	if ( m_pCurrentChain == NULL )
	{
		m_pCurrentChain = new CChain();
		m_pCurrentChain->m_pPDB = this;
		m_pCurrentChain->m_iArrayModel = m_iCurrentModel;
		m_pCurrentChain->m_chainID = pAtom->m_chainID;
		m_pCurrentChain->m_strPDBID = m_strPDBID + _T("_") + m_pCurrentChain->m_chainID;
	}
	
	m_pCurrentChain->m_arrayHETATM.push_back(pAtom);
	m_currentChainID = pAtom->m_chainID;
	*/

	return S_OK;
}

//
//	ATOM 과 HETATM 사이에 반드시 들어간다.
//	ATOM 과 ATOM 사이에도 들어간다.
//	
HRESULT CPDB::TERField(long , CString &)
{
	if ( m_pCurrentChain == NULL )
	{
		//	AfxMessageBox("Error CPDB::TERField");
		return S_OK;
	}

	//	의미 없다. TER field 를 불분명 해서 사용하지 않는다.
	//	한 chain 안에 ATOM. HETATM 사이에도 TER 가 들어간다.
	//	m_arrayChain[m_iArrayModel].Add(m_pCurrentChain);
	//	m_pCurrentChain = NULL;

	return S_OK;
}


HRESULT CPDB::ENDMDLField(long, CString &str)
{
	if ( m_pCurrentChain != NULL )
	{	//	HETATM 뒤에 TER 없이 ENDMDL 이 나온다.

		BOOL bRet = ValidateChain(m_pCurrentChain);
		if ( bRet == TRUE )
		{
			//	MODEL field가 없는 PDB를 위해서 공간을 만들어준다.
			while ( m_arrayarrayChain.size() <= m_iArrayModel )
			{
				CSTLArrayChain arrayChainTemp;
				m_arrayarrayChain.push_back(arrayChainTemp);
			}

			BOOL	bAlreadyExist = FALSE;
			for ( int iChain1= 0 ; iChain1 < m_arrayarrayChain[m_iArrayModel].size() ; iChain1++ )
			{
				CChain * pChain = m_arrayarrayChain[m_iArrayModel][iChain1];
				if ( pChain == m_pCurrentChain )
					bAlreadyExist = TRUE;
			}

			if ( bAlreadyExist == FALSE )
			{
				m_arrayarrayChain[m_iArrayModel].push_back(m_pCurrentChain);
				m_pCurrentChain->m_arrayIndex = m_arrayarrayChain[m_iArrayModel].size()-1;
				if ( m_pModel )
				{
					m_pModel->m_arrayChain.push_back(m_pCurrentChain);
					m_pCurrentChain->m_arrayIndexModel = m_pModel->m_arrayChain.size()-1;
					m_arrayModel.push_back(m_pModel);
				}
			}
		}
		else
		{
			//	버린다.
			delete m_pCurrentChain;
			if( m_pModel )
				delete m_pModel;
		}
		m_pCurrentChain = NULL;
		m_pModel = NULL;
	}

	return S_OK;
}

HRESULT CPDB::SIGATMField(long, CString &)
{

	return S_OK;
}

HRESULT CPDB::ANISOUField(long, CString &)
{
	return S_OK;
}

HRESULT CPDB::SIGUIJField(long, CString &)
{
	return S_OK;
}

HRESULT	CPDB::ENDField(long index, CString &str)
{
	if ( m_pCurrentChain != NULL )
	{	//	HETATM 뒤에 TER 이 없이 다음 필드가 나오는 경우가 있다.
		BOOL bRet = ValidateChain(m_pCurrentChain);
		if ( bRet == TRUE )
		{
			//	MODEL field가 없는 PDB를 위해서 공간을 만들어준다.
			while ( m_arrayarrayChain.size() <= m_iArrayModel )
			{
				CSTLArrayChain arrayChainTemp;
				m_arrayarrayChain.push_back(arrayChainTemp);
			}

			//
			BOOL	bAlreadyExist = FALSE;
			for ( int iChain1= 0 ; iChain1 < m_arrayarrayChain[m_iArrayModel].size() ; iChain1++ )
			{
				CChain * pChain = m_arrayarrayChain[m_iArrayModel][iChain1];
				if ( pChain == m_pCurrentChain )
					bAlreadyExist = TRUE;
			}

			if ( bAlreadyExist == FALSE )
			{
				m_arrayarrayChain[m_iArrayModel].push_back(m_pCurrentChain);
				m_pCurrentChain->m_arrayIndex = m_arrayarrayChain[m_iArrayModel].size()-1;
				if ( m_pModel )
				{
					m_pModel->m_arrayChain.push_back(m_pCurrentChain);
					m_pCurrentChain->m_arrayIndexModel = m_pModel->m_arrayChain.size()-1;
					m_arrayModel.push_back(m_pModel);
				}
			}
		}
		else
		{
			//	버린다.
			delete m_pCurrentChain;
			if ( m_pModel )
				delete m_pModel;
		}
		m_pCurrentChain = NULL;
		m_pModel = NULL;
	}
	return S_OK;
}

//	secondary structure.
HRESULT	CPDB::HELIXField(long index, CString &strFiled)
{
	CHelix	* pHelix = new CHelix;

	pHelix->m_helixID = strFiled.Mid(11, 14-12+1);
	pHelix->m_helixID.TrimLeft(); 
	pHelix->m_helixID.TrimRight();

	pHelix->m_chainID = strFiled.Mid(19).GetAt(0);

	pHelix->m_beginSeqNum = atoi(strFiled.Mid(21, 25-22+1));
	pHelix->m_endSeqNum  = atoi(strFiled.Mid(33, 37-34+1));

	pHelix->m_helixClass = atoi(strFiled.Mid(38, 40-39+1));

	static TCHAR * helixClassName[100] = {  _T(""), _T("Right-handed alpha"), _T("Right-handed omega"), _T("Right-handed pi"), _T("Right-handed gamma"), _T("Right-handed 310"),
									_T("Left-handed alpha"), _T("Left-handed omega"), _T("Left-handed gamma"), _T("27 ribbon/helix"), _T("Polyproline") , };
	pHelix->m_strHelixClass = helixClassName[pHelix->m_helixClass];

	m_arrayHelix.push_back(pHelix);
	return S_OK;
}

HRESULT	CPDB::SHEETField(long index, CString &strFiled)
{
	CSheet	* pSheet = new CSheet;
	pSheet->m_sheetID = strFiled.Mid(11, 14-12+1);

	pSheet->m_indexStrand = atoi(strFiled.Mid( 7 , 10-8+1));

	pSheet->m_numStrand = atoi(strFiled.Mid(14,16-15+1));

	pSheet->m_chainID = strFiled.Mid(21).GetAt(0);

	pSheet->m_beginSeqNum = atoi(strFiled.Mid(22,4));
	pSheet->m_endSeqNum = atoi(strFiled.Mid(33,4));

	pSheet->m_hydronBondIndex1 = atoi(strFiled.Mid(50,4));
	pSheet->m_hydronBondIndex2 = atoi(strFiled.Mid(65,4));

	m_arraySheet.push_back(pSheet);

	return S_OK;
}

/*
0		  1         2         3         4         5         6         7      
012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
SCALE1      0.007886  0.004553  0.000000        0.00000                         
SCALE2      0.000000  0.009106  0.000000        0.00000                         
SCALE3      0.000000  0.000000  0.007172        0.00000     

COLUMNS         DATA  TYPE    FIELD              DEFINITION
------------------------------------------------------------------
1 -  6         Record name   "SCALEn" n=1,  2, or 3
11 - 20         Real(10.6)    s[n][1]            Sn1
21 - 30         Real(10.6)    s[n][2]            Sn2
31 - 40         Real(10.6)    s[n][3]            Sn3
46 - 55         Real(10.5)    u[n]               Un

*/
HRESULT	CPDB::SCALEField(long index, CString &strFiled)
{
	/*
	CString value1 = strFiled.Mid(10,10);
	CString value2 = strFiled.Mid(20,10);
	CString value3 = strFiled.Mid(30,10);
	CString value4 = strFiled.Mid(45,10);

	FLOAT	fVal1 = atof(value1);
	FLOAT	fVal2 = atof(value2);
	FLOAT	fVal3 = atof(value3);
	FLOAT	fVal4 = atof(value4);

	if ( strFiled.Mid(5,1) == "1" )
	{
		m_matScale._11 = fVal1;
		m_matScale._12 = fVal2;
		m_matScale._13 = fVal3;
		m_matScale._14 = fVal4;
	}
	
	if ( strFiled.Mid(5,1) == "2" )
	{
		m_matScale._21 = fVal1;
		m_matScale._22 = fVal2;
		m_matScale._23 = fVal3;
		m_matScale._24 = fVal4;
	}

	if ( strFiled.Mid(5,1) == "3" )
	{
		m_matScale._31 = fVal1;
		m_matScale._32 = fVal2;
		m_matScale._33 = fVal3;
		m_matScale._34 = fVal4;

		D3DXMatrixTranspose(&m_matScale, &m_matScale);

		m_matScale._43 = -m_matScale._43;
	}
	*/

	return S_OK;
}

//
//
//

HRESULT CPDB::Destroy()
{
	for ( int i = 0 ; i < m_arrayHelix.size() ; i++ )
	{
		SAFE_DELETE(m_arrayHelix[i]);
	}
	m_arrayHelix.clear();

	for ( int i = 0 ; i < m_arraySheet.size() ; i++ )
	{
		SAFE_DELETE(m_arraySheet[i]);
	}
	m_arraySheet.clear();

	m_strArrayHeader.RemoveAll();
	m_strArrayTitle.RemoveAll();
	m_strArraySource.RemoveAll();
	m_strArrayKeywords.RemoveAll();
	m_strArrayEXPDTA.RemoveAll();
	m_strArrayAuthor.RemoveAll();
	m_strArrayRevisionDate.RemoveAll();
	m_strArrayJournalCitation.RemoveAll();
	m_strArrayRemark.RemoveAll();
	
	for ( int i = 0 ; i < m_arrayModel.size() ; i++ )
	{
		SAFE_DELETE( m_arrayModel[i] );
	}

	m_arrayModel.clear();

	for ( int i = 0 ; i < m_arrayarrayChain.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChain[i].size() ; j++ )
		{
			SAFE_DELETE( (m_arrayarrayChain[i])[j] );
		}

		m_arrayarrayChain[i].clear();
	}
	m_arrayarrayChain.clear();

	return S_OK;
}

//=====================================================================================
//=====================================================================================
CChain::CChain()
{
	m_iModel = 0;
	m_chainID = 0;
	m_arrayIndex = 0;
	m_arrayIndexModel = 0;

	m_pPDB = NULL;

	m_bDNA = FALSE;

	m_bondLengthMin = 0.4f;
	m_bondLengthMax = 1.9f;		//	실험으로 알아냄.

	m_bondVanDerWaalsMin = 0.4f;
	m_bondVanDerWaalsMax = 1.2f;

	m_pD3DRotationMatrix1 = NULL;
	m_pD3DRotationMatrix2 = NULL;

	m_arrayBondPos1 = NULL;
	m_arrayBondPos2 = NULL;
	m_arrayBondRotation1 = NULL;
	m_arrayBondRotation2 = NULL;
	m_arrayBondScale = NULL;

	m_arrayAtom.reserve(1000);
	m_arrayHETATM.reserve(1000);

	m_chainRadius = 0.0f;
	m_chainCenter.x = m_chainCenter.y = m_chainCenter.z = 0.0f;
	m_chainMinMaxBB[0] = D3DXVECTOR3(1e20, 1e20, 1e20);
	m_chainMinMaxBB[1] = D3DXVECTOR3(-1e20, -1e20, -1e20);
}

CChain::~CChain()
{
	Destroy();
}

HRESULT CChain::Destroy()
{
	for ( int i = 0 ; i < m_arrayAtom.size() ; i++ )
	{
		SAFE_DELETE( m_arrayAtom[i] );
	}

	for ( i = 0 ; i < m_arrayHETATM.size() ; i++ )
	{
		//    m_arrayHETATM 이 m_arrayAtom 에 concat 된다.
		//    delete m_arrayHETATM[i];
	}

	for ( i = 0 ; i < m_arrayResidue.size() ; i++ )
	{
		SAFE_DELETE( m_arrayResidue[i] );
	}

	for ( i = 0 ; i < m_arrayHelix.size() ; i++ )
	{
		SAFE_DELETE( m_arrayHelix[i] );
	}
	m_arrayHelix.clear();

	for ( i = 0 ; i < m_arraySheet.size() ; i++ )
	{
		SAFE_DELETE( m_arraySheet[i] );
	}
	m_arraySheet.clear();

	m_arrayAtom.clear();
	m_arrayHETATM.clear();

	m_arrayBond.clear();

	m_arrayResidueIndex.clear();
	m_arrayResidueIndexBegin.clear();
	m_arrayResidueIndexEnd.clear();	

	SAFE_DELETE_ARRAY(m_pD3DRotationMatrix1);
	SAFE_DELETE_ARRAY(m_pD3DRotationMatrix2);

	SAFE_DELETE_ARRAY(m_arrayBondPos1);
	SAFE_DELETE_ARRAY(m_arrayBondPos2);
	SAFE_DELETE_ARRAY(m_arrayBondRotation1);
	SAFE_DELETE_ARRAY(m_arrayBondRotation2);
	SAFE_DELETE_ARRAY(m_arrayBondScale);

	return S_OK;
}

//	거리가 가까운것에 대해, bond를 구한다.
 
#pragma managed(push,off)
HRESULT CChain::FindBond()
{
	//	예외 : Residue에, A,B 가 붙은것이 있다.
	//	ATOM   1100  N   ARG E 151      35.732  37.983 102.903  1.00 17.62   3  3APR1312
	//	ATOM   1101  CA  ARG E 151      36.160  38.466 101.555  1.00 17.56   3  3APR1313
	//	ATOM   1102  C   ARG E 151      36.877  39.781 101.904  1.00 14.67   3  3APR1314
	//	ATOM   1103  O   ARG E 151      36.355  40.507 102.762  1.00 13.16   3  3APR1315
	//	ATOM   1104  CB  ARG E 151      35.046  38.578 100.537  1.00 19.70   3  3APR1316
	//	ATOM   1105  CG AARG E 151      34.697  37.223  99.883  0.50 21.37   3  3APR1317
	//	ATOM   1106  CG BARG E 151      34.187  37.338 100.300  0.50 20.48   3  3APR1318
	//	ATOM   1107  CD AARG E 151      34.190  37.411  98.489  0.50 22.79   3  3APR1319
	//	ATOM   1108  CD BARG E 151      32.856  37.705  99.742  0.50 21.25   3  3APR1320
	//	ATOM   1109  NE AARG E 151      35.159  38.137  97.670  0.50 24.34   3  3APR1321
	//	ATOM   1110  NE BARG E 151      32.049  38.447 100.717  0.50 22.30   3  3APR1322
	//	ATOM   1111  CZ AARG E 151      34.937  38.760  96.518  0.50 24.85   3  3APR1323
	//	ATOM   1112  CZ BARG E 151      30.765  38.778 100.530  0.50 22.38   3  3APR1324
	//	ATOM   1113  NH1AARG E 151      33.754  38.789  95.914  0.50 25.68   3  3APR1325
	//	ATOM   1114  NH1BARG E 151      30.085  38.471  99.421  0.50 22.36   3  3APR1326
	//	ATOM   1115  NH2AARG E 151      35.938  39.423  95.933  0.50 25.41   3  3APR1327
	//	ATOM   1116  NH2BARG E 151      30.132  39.413 101.517  0.50 22.15   3  3APR1328
	//
	//	

	m_arrayBond.reserve( m_arrayResidue.size() * 20); 

	for ( int i = 0 ; i < m_arrayResidue.size() ; i++ )
	{
		CResidue * pResidue = m_arrayResidue[i];

		for ( int j = 0 ; j < pResidue->m_arrayAtom.size() ; j++ )
		{
			//	
			//	하나의 residue 안에서의 공유결합을 찾는다.
			//	
			for ( int k = j+1 ; k < pResidue->m_arrayAtom.size() ; k++ )
			{
				CAtom * atom1 = pResidue->m_arrayAtom[j];
				CAtom * atom2 = pResidue->m_arrayAtom[k];

				D3DXVECTOR3 vecLen = atom1->m_pos-atom2->m_pos;
				FLOAT len = D3DXVec3Length(&vecLen);

				if ( atom1->m_atomName[1] == 'H' || atom2->m_atomName[1] == 'H' )
				{
					if ( len > m_bondVanDerWaalsMin && len < m_bondVanDerWaalsMax )
					{
						//	3apr.ent 의 151 번 residue.
						//	occupancy가 다른 두개의 amino-acid 가 있다.
						if ( atom1->m_altLoc == _T(' ') || atom1->m_altLoc == atom2->m_altLoc )
						{
							m_arrayBond.push_back((DWORD)MAKELONG(atom1->m_arrayIndex, atom2->m_arrayIndex));
							long iBond= m_arrayBond.size()-1;
							atom1->m_arrayBondIndex.push_back(iBond);
						}
					}
				}
				else
				{
					//	특정 길이 안에서만 결합이 만들어짐. 
					if ( len > m_bondVanDerWaalsMin && len < m_bondLengthMax )
					{
						//	3apr.ent 의 151 번 residue.
						//	occupancy가 다른 두개의 amino-acid 가 있다.
						if ( atom1->m_altLoc == _T(' ') || atom1->m_altLoc == atom2->m_altLoc )
						{
							m_arrayBond.push_back((DWORD)MAKELONG(atom1->m_arrayIndex, atom2->m_arrayIndex));
							long iBond= m_arrayBond.size()-1;
							atom1->m_arrayBondIndex.push_back(iBond);
						}
					}
				}
			}
		}

		//	마지막꺼가 아니라면 다음 레지듀와의 bond를 지정
		if ( pResidue->m_bHETATM == FALSE )		//	일반 residue
		{
			if ( i < m_arrayResidue.size()-1 )
			{
				CAtom * atom1 = pResidue->GetCAtom();
				if ( atom1 != NULL ) 
				{
					CResidue * pResidue2 = m_arrayResidue[i+1];
					if ( pResidue2->m_bHETATM == FALSE )
					{
						CAtom * atom2 = pResidue2->GetNAtom();
						if ( atom2 )
						{
							m_arrayBond.push_back((DWORD)MAKELONG(atom1->m_arrayIndex, atom2->m_arrayIndex));
							long iBond= m_arrayBond.size()-1;
							atom1->m_arrayBondIndex.push_back(iBond);
						}
					}
				}
			}
		}
	}

	//	
	//	본드의 매트릭스 를 미리 계산해둠.
	//	precomputation matrix (matScale*matRot2*matRot1*matTrans)
	//	
	//	m_pD3DRotationMatrix1 = new D3DXMATRIX [m_arrayBond.GetSize()];
	//	m_pD3DRotationMatrix2 = new D3DXMATRIX [m_arrayBond.GetSize()];
	m_arrayBondPos1 = new D3DXVECTOR4 [m_arrayBond.size()];
	m_arrayBondPos2 = new D3DXVECTOR4 [m_arrayBond.size()];
	m_arrayBondRotation1 = new D3DXVECTOR2 [m_arrayBond.size()];
	m_arrayBondRotation2 = new D3DXVECTOR2 [m_arrayBond.size()];
	m_arrayBondScale = new FLOAT [m_arrayBond.size()];

	for ( long iBond = 0 ; iBond < m_arrayBond.size() ; iBond ++ )
	{
		DWORD bond = m_arrayBond[iBond];
		long iBond1 = LOWORD(bond);
		long iBond2 = HIWORD(bond);

		CAtom * pAtom1 = m_arrayAtom[iBond1];
		CAtom * pAtom2 = m_arrayAtom[iBond2];

		D3DXVECTOR3		delta = pAtom2->m_pos - pAtom1->m_pos;
		D3DXVECTOR3		Center = delta / 2.0 + pAtom1->m_pos;
		double distance1 = sqrt(delta.x*delta.x + delta.z*delta.z);
		double distance2 = sqrt(delta.x*delta.x + delta.y*delta.y +delta.z*delta.z);
		double Theta = acos(delta.z / distance1) * 180.0 / D3DX_PI;
		if (delta.x < 0) Theta = -Theta;
		double Pi = acos(distance1 / distance2) * 180.0 / D3DX_PI;
		if (delta.y > 0) Pi = -Pi;

		D3DXMATRIXA16 matTrans1, matTrans2, matRot1, matRot2, matScale;
		D3DXMatrixTranslation(&matTrans1, pAtom1->m_pos.x, pAtom1->m_pos.y, pAtom1->m_pos.z );
		D3DXMatrixTranslation(&matTrans2, Center.x, Center.y, Center.z );
		D3DXMatrixRotationY( &matRot1, (float)(D3DX_PI*Theta/180.0f) );
		D3DXMatrixRotationX( &matRot2, (float)(D3DX_PI*Pi/180.0f) );
		D3DXMatrixScaling( &matScale, 1.0f, 1.0f, (float)(distance2/2.0f) );

		m_arrayBondPos1[iBond] = D3DXVECTOR4(pAtom1->m_pos.x, pAtom1->m_pos.y, pAtom1->m_pos.z, 0);
		m_arrayBondPos2[iBond] = D3DXVECTOR4(Center.x, Center.y, Center.z, 0);
		m_arrayBondRotation1[iBond].x = (float)(D3DX_PI*Theta/180.0f);
		m_arrayBondRotation1[iBond].y = (float)(D3DX_PI*Pi/180.0f);
		m_arrayBondRotation2[iBond].x = (float)(D3DX_PI*Theta/180.0f);
		m_arrayBondRotation2[iBond].y = (float)(D3DX_PI*Pi/180.0f);
		m_arrayBondScale[iBond] = (float)(distance2/2.0f);
		//	m_pD3DRotationMatrix1[iBond] = matScale*matRot2*matRot1*matTrans1;
		//	m_pD3DRotationMatrix2[iBond] = matScale*matRot2*matRot1*matTrans2;
	}

	return S_OK;
}
#pragma managed(pop)


//	
HRESULT CChain::SetAtomTypeAndResidueIndex()
{
	long	iCurrentAtom;
	long	currentResidueNum = -324534;		//	-1 하면 안된다. residue index 가 -1 인것이 있다. pdb1a5r.ent

	m_arrayResidueIndex.reserve(m_arrayResidue.size()+10);
	m_arrayResidueIndexBegin.reserve(m_arrayResidue.size()+10);
	m_arrayResidueIndexEnd.reserve(m_arrayResidue.size()+10);

	//    
	long	nMaxAtoms = m_arrayAtom.size();
	for ( iCurrentAtom = 0 ; iCurrentAtom < nMaxAtoms; iCurrentAtom++ )
	{
		m_arrayAtom[iCurrentAtom]->m_arrayIndex = iCurrentAtom;

		if ( m_arrayAtom[iCurrentAtom]->m_residueNum != currentResidueNum )
		{
			currentResidueNum = m_arrayAtom[iCurrentAtom]->m_residueNum;
			
			long	iStartAtom = iCurrentAtom;
			while ( (iStartAtom<nMaxAtoms) && m_arrayAtom[iStartAtom]->m_residueNum == currentResidueNum )
			{
				if ( m_arrayAtom[iStartAtom]->m_atomName == _T(" N  ") )
				{
					m_arrayAtom[iStartAtom]->m_bSideChain = FALSE;
					m_arrayAtom[iStartAtom]->m_typeAtom = MAINCHAIN_N;
				}
				else
				if ( m_arrayAtom[iStartAtom]->m_atomName == _T(" CA ") )
				{
					m_arrayAtom[iStartAtom]->m_bSideChain = FALSE;
					m_arrayAtom[iStartAtom]->m_typeAtom = MAINCHAIN_CA;
				}
				else
				if ( m_arrayAtom[iStartAtom]->m_atomName == _T(" CB ") )
				{
					m_arrayAtom[iStartAtom]->m_bSideChain = TRUE;
					m_arrayAtom[iStartAtom]->m_typeAtom = RESIDUE_CB;
				}
				else
				if ( m_arrayAtom[iStartAtom]->m_atomName == _T(" C  ") )
				{
					m_arrayAtom[iStartAtom]->m_bSideChain = FALSE;
					m_arrayAtom[iStartAtom]->m_typeAtom = MAINCHAIN_C;
				}
				else
				if ( m_arrayAtom[iStartAtom]->m_atomName == _T(" O  ") )
				{
					m_arrayAtom[iStartAtom]->m_bSideChain = FALSE;
					m_arrayAtom[iStartAtom]->m_typeAtom = MAINCHAIN_O;
				}
				
				iStartAtom ++;
			}

			currentResidueNum = m_arrayAtom[iCurrentAtom]->m_residueNum;
			if ( iCurrentAtom != 0 )
				m_arrayResidueIndex.push_back(iCurrentAtom);

			m_arrayResidueIndexBegin.push_back(iCurrentAtom);
			if ( iCurrentAtom != 0 )
				m_arrayResidueIndexEnd.push_back(iCurrentAtom-1);
		}
	}

	//	마지막꺼를 하나 더 넣어준다. 실제 아미노산 숫자보다 +1 더 큰 숫자이다.
	m_arrayResidueIndex.push_back(iCurrentAtom);
	m_arrayResidueIndexEnd.push_back(iCurrentAtom-1);

	//
	//	CResidue 를 설정.
	//	
	for ( int i = 0 ; i < m_arrayResidueIndexBegin.size(); i++ )
	{
		long begin = m_arrayResidueIndexBegin[i];
		long end = m_arrayResidueIndexEnd[i];

		CResidue * pResidue = new CResidue;
		pResidue->m_pChain = this;

		for ( int j = begin; j <= end ; j++ )
		{
			CAtom * pAtom = m_arrayAtom[j];
			pAtom->m_pResidue = pResidue;	
			pResidue->m_arrayAtom.push_back(pAtom);
			pResidue->m_arrayIndex = i;

			if ( pAtom->m_typeAtom == MAINCHAIN_N )
				pResidue->m_arrayAtomSpecial[MAINCHAIN_N] = pAtom;
			else if ( pAtom->m_typeAtom == MAINCHAIN_CA )
				pResidue->m_arrayAtomSpecial[MAINCHAIN_CA] = pAtom;
			else if ( pAtom->m_typeAtom == RESIDUE_CB )
				pResidue->m_arrayAtomSpecial[RESIDUE_CB] = pAtom;
			else if ( pAtom->m_typeAtom == MAINCHAIN_C )
				pResidue->m_arrayAtomSpecial[MAINCHAIN_C] = pAtom;
			else if ( pAtom->m_typeAtom == MAINCHAIN_O )
				pResidue->m_arrayAtomSpecial[MAINCHAIN_O] = pAtom;
		}

		if ( pResidue->GetNAtom() != NULL && pResidue->GetCAAtom() != NULL && pResidue->GetCAtom() != NULL )
			pResidue->m_bExistMainChain = TRUE;

		m_arrayResidue.push_back(pResidue);
	}

	//
	//    HETATM 을 atom array에 넣어준다.
	//    
	if ( m_arrayHETATM.size() > 0 )
	{
		CResidue * pResidue = new CResidue;
		pResidue->m_pChain = this;
		pResidue->m_bHETATM = TRUE;
		pResidue->m_arrayIndex = m_arrayResidue.size();
		for ( int i = 0 ; i < m_arrayHETATM.size(); i++ )
		{
			pResidue->m_arrayAtom.push_back(m_arrayHETATM[i]);
			m_arrayHETATM[i]->m_pResidue = pResidue;

			m_arrayHETATM[i]->m_arrayIndex = m_arrayAtom.size();
			m_arrayAtom.push_back(m_arrayHETATM[i]);
		}
		m_arrayResidue.push_back(pResidue);
	}

	return S_OK;
}

//	pdb 에서 읽은 SSE 의 데이타를 CAtom 에 설정한다.
HRESULT CChain::SetSecondaryStructureAtomAssign()
{
	CSTLArrayAtom &arrayAtom = m_arrayAtom;

	if ( arrayAtom.size() < 10 )
	{
		return E_ABORT;
	}

	CSTLArrayHelix	& helix = m_pPDB->m_arrayHelix;
	CSTLArraySheet	& sheet = m_pPDB->m_arraySheet;

	for ( int iHelix = 0 ; iHelix < helix.size() ; iHelix ++ )
	{
		if ( m_chainID == helix[iHelix]->m_chainID )
		{
			for ( int i = 0 ; i < m_arrayAtom.size() ; i++ )
			{
				if ( m_arrayAtom[i]->m_residueNum >= helix[iHelix]->m_beginSeqNum && m_arrayAtom[i]->m_residueNum <= helix[iHelix]->m_endSeqNum )
				{
					m_arrayAtom[i]->m_secondaryStructure = SS_HELIX;
					if ( helix[iHelix]->m_helixClass == 3 )
						m_arrayAtom[i]->m_typeHelix = SS_HELIX_PI;
					else if ( helix[iHelix]->m_helixClass == 5 )
						m_arrayAtom[i]->m_typeHelix = SS_HELIX_310;
					else
						m_arrayAtom[i]->m_typeHelix = SS_HELIX_DEFAULT;
				}
			}
		}
	}

	for ( int iSheet = 0 ; iSheet < sheet.size() ; iSheet ++ )
	{
		if ( m_chainID == sheet[iSheet]->m_chainID )
		{
			for ( int i = 0 ; i < m_arrayAtom.size() ; i++ )
			{
				if ( m_arrayAtom[i]->m_residueNum >= sheet[iSheet]->m_beginSeqNum && m_arrayAtom[i]->m_residueNum <= sheet[iSheet]->m_endSeqNum )
				{
					m_arrayAtom[i]->m_secondaryStructure = SS_SHEET;
				}
			}
		}
	}

	return S_OK;
}

//	
//	CAtom 에 SS 의 데이타를 가지고 Chain 안에 CSSInfo 데이타 구조를 만든다
//
HRESULT CChain::SetChainSecondaryStructure()
{
	CSSInfo * pSSInfoHelix = NULL;
	CSSInfo * pSSInfoSheet = NULL;

	BOOL	helixRange = FALSE;
	BOOL	sheetRange = FALSE;

	long ssOld = -1;
	for ( int i = 0 ; i < m_arrayResidue.size() ; i++ )
	{
		CResidue * pResidue = m_arrayResidue[i];
		long ssCurrent = pResidue->GetSS();

		if ( ssCurrent != ssOld )
		{
			if ( ssCurrent == SS_NONE )
			{	//	helix->none. sheet->none, 시작시->none 으로
				if ( ssOld == SS_HELIX )
				{	//	helix->none	, helix 저장
					pSSInfoHelix->m_endResidue =  m_arrayResidue[i-1]->m_arrayIndex;
					helixRange = FALSE;
					m_arrayHelix.push_back(pSSInfoHelix);
					pSSInfoHelix = NULL;
				}
				else
				if ( ssOld == SS_SHEET )
				{	//	sheet->none, sheet 저장
					pSSInfoSheet->m_endResidue =  m_arrayResidue[i-1]->m_arrayIndex;
					sheetRange = FALSE;
					m_arraySheet.push_back(pSSInfoSheet);
					pSSInfoSheet = NULL;
				}
			}
			else
			if ( ssCurrent == SS_HELIX )
			{	//	none->helix(시작시 helix로), sheet->helix, 
				if ( ssOld == SS_SHEET )
				{	//	sheet->helix, Sheet(기존꺼) 저장,
					pSSInfoSheet->m_endResidue =  m_arrayResidue[i-1]->m_arrayIndex;
					sheetRange = FALSE;
					m_arraySheet.push_back(pSSInfoSheet);
					pSSInfoSheet = NULL;
				}

				pSSInfoHelix = new CSSInfo;
				pSSInfoHelix->m_beginResidue =  m_arrayResidue[i]->m_arrayIndex;
				helixRange = TRUE;
			}
			else
			if ( ssCurrent == SS_SHEET )
			{
				if ( ssOld == SS_HELIX )
				{	//	helix->sheet, helix(기존꺼) 저장,
					pSSInfoHelix->m_endResidue =  m_arrayResidue[i-1]->m_arrayIndex;
					helixRange = FALSE;
					m_arrayHelix.push_back(pSSInfoHelix);
					pSSInfoHelix = NULL;
				}

				pSSInfoSheet = new CSSInfo;
				pSSInfoSheet->m_beginResidue =  m_arrayResidue[i]->m_arrayIndex;
				sheetRange = TRUE;
			}
		}

		if ( helixRange == TRUE )
			pSSInfoHelix->m_arrayResidue.push_back(m_arrayResidue[i]);

		if ( sheetRange == TRUE )
			pSSInfoSheet->m_arrayResidue.push_back(m_arrayResidue[i]);

		ssOld = ssCurrent;
	}

	if ( pSSInfoHelix )
		m_arrayHelix.push_back(pSSInfoHelix);

	if ( pSSInfoSheet )
		m_arrayHelix.push_back(pSSInfoSheet);

	return S_OK;
}

HRESULT CChain::SetSequenceData()
{
	long	nAmino = 0;
	long	nDNA = 0;

	for ( int i = 0 ; i < m_arrayResidue.size() ; i++ )
	{
		CResidue * pResidue = m_arrayResidue[i];
		CString strResdiue = pResidue->GetResidueName();
		CString strResdiueLast;
		if ( strResdiue.GetLength() > 0 )
			strResdiueLast = strResdiue[strResdiue.GetLength()-1];

		if(strResdiue == _T("ALA")) { strResdiue = _T("A");	nAmino++; }
		else if(strResdiue == _T("ARG")) { strResdiue = _T("R");	nAmino++; }
		else if(strResdiue == _T("ASN")) { strResdiue = _T("N");	nAmino++; }
		else if(strResdiue == _T("ASP")) { strResdiue = _T("D");	nAmino++; }
		else if(strResdiue == _T("CYS")) { strResdiue = _T("C");	nAmino++; }
		else if(strResdiue == _T("GLU")) { strResdiue = _T("E");	nAmino++; }
		else if(strResdiue == _T("GLN")) { strResdiue = _T("Q");	nAmino++; }
		else if(strResdiue == _T("GLY")) { strResdiue = _T("G");	nAmino++; }
		else if(strResdiue == _T("HIS")) { strResdiue = _T("H");	nAmino++; }
		else if(strResdiue == _T("ILE")) { strResdiue = _T("I");	nAmino++; }
		else if(strResdiue == _T("LEU")) { strResdiue = _T("L");	nAmino++; }
		else if(strResdiue == _T("LYS")) { strResdiue = _T("K");	nAmino++; }
		else if(strResdiue == _T("MET")) { strResdiue = _T("M");	nAmino++; }
		else if(strResdiue == _T("PHE")) { strResdiue = _T("F");	nAmino++; }
		else if(strResdiue == _T("PRO")) { strResdiue = _T("P");	nAmino++; }
		else if(strResdiue == _T("SER")) { strResdiue = _T("S");	nAmino++; }
		else if(strResdiue == _T("THR")) { strResdiue = _T("T");	nAmino++; }
		else if(strResdiue == _T("TRP")) { strResdiue = _T("W");	nAmino++; }
		else if(strResdiue == _T("TYR")) { strResdiue = _T("Y");	nAmino++; }
		else if(strResdiue == _T("VAL")) { strResdiue = _T("V");	nAmino++; }
		else if(strResdiue == _T("A")) { strResdiue = _T("A");		pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiue == _T("G")) { strResdiue = _T("G");		pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiue == _T("C")) { strResdiue = _T("C");		pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiue == _T("T")) { strResdiue = _T("T");		pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiue == _T("U")) { strResdiue = _T("U");		pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiueLast == _T("A")) { strResdiue = _T("A");	pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiueLast == _T("G")) { strResdiue = _T("G");	pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiueLast == _T("C")) { strResdiue = _T("C");	pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiueLast == _T("T")) { strResdiue = _T("T");	pResidue->m_bDNA = TRUE; nDNA++; }
		else if(strResdiueLast == _T("U")) { strResdiue = _T("U");	pResidue->m_bDNA = TRUE; nDNA++; }
		else strResdiue = _T("-");

		//	
		if ( pResidue->m_bDNA == TRUE )
		{	//	DNA 일때, C5 atom 을 찾는다.
			for ( int i = 0 ; i < pResidue->m_arrayAtom.size() ; i++ )
			{
				CAtom * pAtom = pResidue->m_arrayAtom[i];
				if ( pAtom->m_atomName.Find("C5'") != -1 )
				{
					pResidue->m_pDNAC5 = pAtom;
					break;
				}
			}
		}

		m_strSequenceData += strResdiue;

		pResidue->m_residueNameOneChar = strResdiue;

		//	residue의 center를 구한다.
		D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
		D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);
		for ( int i = 0 ; i < pResidue->m_arrayAtom.size() ; i++ )
		{
			CAtom * pAtom = pResidue->m_arrayAtom[i];

			if (maxBB.x < pAtom->m_pos.x) maxBB.x = pAtom->m_pos.x;
			if (maxBB.y < pAtom->m_pos.y) maxBB.y = pAtom->m_pos.y;
			if (maxBB.z < pAtom->m_pos.z) maxBB.z = pAtom->m_pos.z;
			if (minBB.x > pAtom->m_pos.x) minBB.x = pAtom->m_pos.x;
			if (minBB.y > pAtom->m_pos.y) minBB.y = pAtom->m_pos.y;
			if (minBB.z > pAtom->m_pos.z) minBB.z = pAtom->m_pos.z;
		}

		pResidue->m_center = (minBB + maxBB)/2.0f;
	}

	if ( nAmino > nDNA )
		m_bDNA = FALSE;
	else
		m_bDNA = TRUE;

	return S_OK;
}

//	
//	CSTLArrayAtom &arrayAtom, long &nCount 은 PDB center를 구하기 위해서 사용.
//
HRESULT CChain::FindCenterRadius()
{
	if ( m_arrayAtom.size() == 0 )
		return E_FAIL;

	D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
	D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);

	for ( int i = 0 ; i < m_arrayAtom.size() ; i++ )
	{
		CAtom * pAtom = m_arrayAtom[i];

		if (maxBB.x < pAtom->m_pos.x) maxBB.x = pAtom->m_pos.x;
		if (maxBB.y < pAtom->m_pos.y) maxBB.y = pAtom->m_pos.y;
		if (maxBB.z < pAtom->m_pos.z) maxBB.z = pAtom->m_pos.z;
		if (minBB.x > pAtom->m_pos.x) minBB.x = pAtom->m_pos.x;
		if (minBB.y > pAtom->m_pos.y) minBB.y = pAtom->m_pos.y;
		if (minBB.z > pAtom->m_pos.z) minBB.z = pAtom->m_pos.z;
	}

	m_chainCenter = (minBB + maxBB)/2.0f;
	m_chainRadius = D3DXVec3Length( &D3DXVECTOR3(m_chainCenter-minBB) ) + 4.0f;

	m_chainMinMaxBB[0] = minBB;
	m_chainMinMaxBB[1] = maxBB;

	return S_OK;
}

//=====================================================================================
//=====================================================================================

HRESULT CAtom::SetAtomIndex()
{
	TCHAR chAtom = m_atomName.GetAt(1);

	switch (chAtom)
	{
		case 'C':
			if ( m_atomName[2] == 'L' )
				m_atomNameIndex = ATOM_CL;
			else
				m_atomNameIndex = ATOM_C;
			break;
		case 'N':
			if ( m_atomName[2] == 'A' )
				m_atomNameIndex = ATOM_NA;
			else
				m_atomNameIndex = ATOM_N;
			break;
		case 'O':
			m_atomNameIndex = ATOM_O;
			break;
		case 'S':
			m_atomNameIndex = ATOM_S;
			break;
		case 'H':
			m_atomNameIndex = ATOM_H;
			break;
		case 'P':
			m_atomNameIndex = ATOM_P;
			break;
		case 'F':
			if ( m_atomName[2] == 'E' )
				m_atomNameIndex = ATOM_FE;
			else
				m_atomNameIndex = ATOM_F;
			break;
		case 'B':
			m_atomNameIndex = ATOM_B;
			break;
		case 'I':
			m_atomNameIndex = ATOM_I;
			break;
		case 'Z':
			m_atomNameIndex = ATOM_ZN;
			break;
		case 'M':
			m_atomNameIndex = ATOM_MG;
			break;
		case 'K':
			m_atomNameIndex = ATOM_K;
			break;
		default:
			m_atomNameIndex = ATOM_UNKNOWN;
				break;
	}

	if ( m_residueName == _T("ALA") ) m_residueNameIndex = RSD_ALA;
	else
	if ( m_residueName == _T("ARG") ) m_residueNameIndex = RSD_ARG;
	else
	if ( m_residueName == _T("ASN") ) m_residueNameIndex = RSD_ASN;
	else
	if ( m_residueName == _T("ASP") ) m_residueNameIndex = RSD_ASP;
	else
	if ( m_residueName == _T("CYS") ) m_residueNameIndex = RSD_CYS;
	else
	if ( m_residueName == _T("GLU") ) m_residueNameIndex = RSD_GLU;
	else
	if ( m_residueName == _T("GLN") ) m_residueNameIndex = RSD_GLN;
	else
	if ( m_residueName == _T("GLY") ) m_residueNameIndex = RSD_GLY;
	else
	if ( m_residueName == _T("HIS") ) m_residueNameIndex = RSD_HIS;
	else
	if ( m_residueName == _T("ILE") ) m_residueNameIndex = RSD_ILE;
	else
	if ( m_residueName == _T("LEU") ) m_residueNameIndex = RSD_LEU;
	else
	if ( m_residueName == _T("LYS") ) m_residueNameIndex = RSD_LYS;
	else
	if ( m_residueName == _T("MET") ) m_residueNameIndex = RSD_MET;
	else
	if ( m_residueName == _T("PHE") ) m_residueNameIndex = RSD_PHE;
	else
	if ( m_residueName == _T("PRO") ) m_residueNameIndex = RSD_PRO;
	else
	if ( m_residueName == _T("SER") ) m_residueNameIndex = RSD_SER;
	else
	if ( m_residueName == _T("THR") ) m_residueNameIndex = RSD_THR;
	else
	if ( m_residueName == _T("TRP") ) m_residueNameIndex = RSD_TRP;
	else
	if ( m_residueName == _T("TYR") ) m_residueNameIndex = RSD_TYR;
	else
	if ( m_residueName == _T("VAL") ) m_residueNameIndex = RSD_VAL;
	else
		m_residueNameIndex = 0;

	static float hydropathy[] = {	HYDROPATHY_RSD_ALA,HYDROPATHY_RSD_ARG,HYDROPATHY_RSD_ASN,HYDROPATHY_RSD_ASP,HYDROPATHY_RSD_CYS,HYDROPATHY_RSD_GLU,HYDROPATHY_RSD_GLN,HYDROPATHY_RSD_GLY,HYDROPATHY_RSD_HIS,HYDROPATHY_RSD_ILE,
		HYDROPATHY_RSD_LEU,HYDROPATHY_RSD_LYS,HYDROPATHY_RSD_MET,HYDROPATHY_RSD_PHE,HYDROPATHY_RSD_PRO,HYDROPATHY_RSD_SER,HYDROPATHY_RSD_THR,HYDROPATHY_RSD_TRP,HYDROPATHY_RSD_TYR,HYDROPATHY_RSD_VAL,0,0,0 };

	m_hydropathy = hydropathy[m_residueNameIndex];

	return S_OK;
}

HRESULT CAtom::SetAtomMass()
{
	switch(m_atomNameIndex)
	{
		case ATOM_C:	m_fmass =  12.011f;		break;
		case ATOM_N:	m_fmass =  14.0067f;	break;
		case ATOM_O:	m_fmass =  15.9994f;	break;
		case ATOM_S:	m_fmass =  32.06f;		break;
		case ATOM_H:	m_fmass =  1.0079f;		break;
		case ATOM_P:	m_fmass =  30.97376f;	break;
		case ATOM_CL:	m_fmass =  35.453f;		break;
		case ATOM_ZN:	m_fmass =  65.38f;		break;
		case ATOM_NA:	m_fmass =  22.98977f;	break;
		case ATOM_FE:	m_fmass =  55.847f;		break;
		case ATOM_MG:	m_fmass =  24.305f;		break;
		case ATOM_K:	m_fmass =  39.0983f;	break;
		case ATOM_CA:	m_fmass =  40.08f;		break;
		case ATOM_I:	m_fmass =  125.9045f;	break;
		case ATOM_UNKNOWN:	m_fmass =  12.011f;	break;
	}
	return S_OK;
}

 
CAtomPDB::CAtomPDB()
{
	m_bHETATM = FALSE;
	m_bSideChain = TRUE; 	

	m_posOrig = D3DXVECTOR3(0,0,0);

	m_atomNameIndex = -1; 
	m_residueNameIndex = -1;
	m_arrayIndex = 0;

	m_atomNameIndex = m_residueNameIndex = -1;

	m_serial = 0;
	m_chainID = 0;
	m_residueNum = 0;
	m_iModel = 0;

	m_occupancy = m_temperature = 0.0f;

	m_secondaryStructure = SS_NONE;
	m_typeHelix = 0;

	m_typeAtom = RESIDUE_R;

	//	m_bShow		= TRUE;

	m_fRadius = 1.0f;

	m_fmass = 10.0f;

	m_pChain = NULL;
	m_pPDB = NULL;

	m_hydropathy = 0.0f;
}

CAtom::CAtom() 
{
	//	m_posTransformed = D3DXVECTOR3(0,0,0);
	m_pos = D3DXVECTOR3(0,0,0);

	//	m_bAnnotation = FALSE;
	//	m_fDistance = 0.0f;
	//	m_bIndicated = FALSE;
}
 

CAtom::~CAtom() 
{
	m_arrayBondIndex.clear(); 
}

//=====================================================================================
//	return TRUE: chain 이 유효	
//	return FALSE: chain 이 무효

//	chain 에 ATOMs 가 6 보다 작으면.. 체인으로 사용하지 않는다.
//	chain 에 ATOM 은 없고 HETATM 만 있는 경우도 chain으로 본다.
//	
BOOL	CPDB::ValidateChain(CChain * m_pChain)
{
	//if ( m_pChain->m_arrayAtom.GetSize() < 6 )
	//	return FALSE;	

	return TRUE;
}

//
//	chain id 가 ' ' 인것을 수정한다.
//	
void CPDB::ModifyPDBChain()
{
	for ( int iModel = 0 ; iModel < m_arrayarrayChain.size() ; iModel ++ )
	{
		for ( int iChain= 0 ; iChain < m_arrayarrayChain[iModel].size() ; iChain++ )
		{
			CChain * pChain = m_arrayarrayChain[iModel][iChain];
			
			//	m_assignBlankChainID = pChain->m_chainID;

			//	chain id 가 ' ' 일때를 조사.
			if ( pChain->m_chainID == ' ' )
			{
				//	새로운 chain id 를 찾는다.
				//	BOOL bFind = FALSE;
				//for ( int id = 'A' ; id < 'Z' ; id ++ )
				//{
				//	m_assignBlankChainID = id;

				//	for ( int i = 0 ; i < m_arrayarrayChain[m_iModel].GetSize(); i++ )
				//	{
				//		if( m_arrayarrayChain[m_iModel].GetAt(i)->m_chainID == id )
				//		{
				//			bFind = TRUE;
				//			break;
				//		}
				//	}

				//	if ( bFind == FALSE )
				//	{
				//		m_assignBlankChainID = tolower(id);

				//		break;
				//	}
				//}
				
				pChain->m_chainID = m_assignBlankChainID;

				//	체인 id 를 전부 바꾼다.
				for ( int i = 0 ; i < pChain->m_arrayAtom.size(); i++ )
				{
					pChain->m_arrayAtom[i]->m_chainID = m_assignBlankChainID;
				}

				//	helix, sheet 도 마찬가지로 바꾼다.

				for ( i = 0 ; i < m_arrayHelix.size(); i++ )
				{
					if ( m_arrayHelix[i]->m_chainID == ' ' )
						m_arrayHelix[i]->m_chainID = m_assignBlankChainID;
				}

				for ( i = 0 ; i < m_arraySheet.size(); i++ )
				{
					if ( m_arraySheet[i]->m_chainID == ' ' )
						m_arraySheet[i]->m_chainID = m_assignBlankChainID;
				}				
			}

			//	HETATOM 의 chain id 를 ATOM 것으로 맞춘다.
			if ( pChain->m_arrayHETATM.size() > 0 )
			{
				if ( pChain->m_arrayHETATM[0]->m_chainID == ' ' )
				{
					for ( int i = 0 ; i < pChain->m_arrayHETATM.size(); i++ )
					{
						pChain->m_arrayHETATM[i]->m_chainID = m_assignBlankChainID;
					}
				}
			}
		}
	}
}

#pragma managed(push,off)
void CPDB::MakeBioUnitTransform()
{
	//	m_strBioUnit
	//	CStringArray		m_strArrayBioUnitChain;
	//	CSTLArrayMatrix		m_bioUnitMatrix;

	/*
0		  1         2         3         4         5         6         7      
012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
REMARK 350 APPLY THE FOLLOWING TO CHAINS: A, B, C, D                                  
REMARK 350 APPLY THE FOLLOWING TO CHAINS: NULL,L                                
REMARK 350   BIOMT1   1  1.000000  0.000000  0.000000        0.00000            
REMARK 350   BIOMT2   1  0.000000  1.000000  0.000000        0.00000            
REMARK 350   BIOMT3   1  0.000000  0.000000  1.000000        0.00000            

REMARK 350   BIOMT1   2 -0.500000 -0.866025  0.000000      126.80000            
REMARK 350   BIOMT2   2  0.866025 -0.500000  0.000000        0.00000            
REMARK 350   BIOMT3   2  0.000000  0.000000  1.000000        0.00000            

	*/

	D3DXMATRIXA16	matBioUnit;
	D3DXMatrixIdentity(&matBioUnit);

	CString		strBioUnitChain(_T("*"));
	for ( int i = 0 ; i < m_strBioUnit.GetSize() ; i++ )
	{
		if ( m_strBioUnit[i].Mid(34, 7) == _T("CHAINS:") )
		{
			if ( strBioUnitChain == _T("*") )
				strBioUnitChain.Empty();

			CString strChainList = m_strBioUnit[i].Mid(42);

			long	index = 0;
			while(1)
			{
				long indexCurrent = strChainList.Find(',', index);
				if ( indexCurrent == -1 )
					break;

				CString strChainID = strChainList.Mid(index, indexCurrent-index);
				strChainID.TrimLeft();
				strChainID.TrimRight();

				if (strChainID == _T("NULL") )
				{
					strChainID = _T("-");
				}

				strBioUnitChain += strChainID;
				index = indexCurrent+1;
			}

			CString strChainID = strChainList.Mid(index);
			strChainID.TrimLeft();
			strChainID.TrimRight();

			if (strChainID == _T("NULL") )
			{
				strChainID = _T("-");
			}

			strBioUnitChain += strChainID;
		}
		else
		{
			CString strfloat1 = m_strBioUnit[i].Mid(24, 10);
			CString strfloat2 = m_strBioUnit[i].Mid(34, 10);
			CString strfloat3 = m_strBioUnit[i].Mid(44, 10);
			CString strfloat4 = m_strBioUnit[i].Mid(54, 15);

			FLOAT float1 = atof(strfloat1);
			FLOAT float2 = atof(strfloat2);
			FLOAT float3 = atof(strfloat3);
			FLOAT float4 = atof(strfloat4);

			long indexMatrix = atoi( m_strBioUnit[i].Mid(18, 1) );
			if ( indexMatrix == 1 )
			{
				matBioUnit._11 = float1;
				matBioUnit._12 = float2;
				matBioUnit._13 = float3;
				matBioUnit._14 = float4;
			}
			else
			if ( indexMatrix == 2 )
			{
				matBioUnit._21 = float1;
				matBioUnit._22 = float2;
				matBioUnit._23 = float3;
				matBioUnit._24 = float4;
			}
			else
			if ( indexMatrix == 3 )
			{
				matBioUnit._31 = float1;
				matBioUnit._32 = float2;
				matBioUnit._33 = float3;
				matBioUnit._34 = float4;
			}

			if ( m_strBioUnit[i].Mid(13, 6) == _T("BIOMT3") )
			{
				if ( D3DXMatrixIsIdentity(&matBioUnit) == FALSE )
				{
					//	OpenGL --> DX transform (http://wonwoolee.tistory.com/99)
					D3DXMatrixTranspose(&matBioUnit, &matBioUnit);
					matBioUnit._13 = -matBioUnit._13;
					matBioUnit._23 = -matBioUnit._23;
					matBioUnit._31 = -matBioUnit._31;
					matBioUnit._32 = -matBioUnit._32;
					matBioUnit._43 = -matBioUnit._43;

					m_bioUnitMatrix.push_back(matBioUnit);
					D3DXMatrixIdentity(&matBioUnit);

					m_strArrayBioUnitChain.Add(strBioUnitChain);
				}
			}
		}
	}

	if ( m_strArrayBioUnitChain.GetSize() > 0 )
	{
		m_bExistBioUnit = TRUE;
	}
}

#pragma managed(pop)
//	atom 안에 m_pChain, m_pPDB 를 설정한다.
void CPDB::SetAtomParentPointer()
{
	CString strName = _T("PDB:") +m_strPDBID;
	SetName(strName);

	for ( int iModel = 0 ; iModel < m_arrayModel.size() ; iModel ++ )
	{
		CModel * pModel = m_arrayModel[iModel];
		pModel->m_pParent = this;

		strName.Format(_T("MODEL:%d") , pModel->m_iModel);
		pModel->SetName(strName);

		for ( int iChain= 0 ; iChain < m_arrayarrayChain[iModel].size() ; iChain++ )
		{
			CChain * pChain = m_arrayarrayChain[iModel][iChain];
			pChain->m_pParent = pModel;

			strName.Format(_T("CHAIN:%c") , pChain->m_chainID);
			pChain->SetName(strName);

			TCHAR chainID = ' ';
			if ( pChain->m_arrayAtom.size() > 0 )
				chainID = pChain->m_arrayAtom[0]->m_chainID;
			else
			if ( pChain->m_arrayHETATM.size() > 0 )
				chainID = pChain->m_arrayHETATM[0]->m_chainID;

			pChain->m_chainID = chainID;
			pChain->m_strPDBID = m_strPDBID + _T("_") + chainID;

			//	Residue에 속하지 않는 ATOM 을 설정
			for ( int iAtom = 0 ; iAtom < pChain->m_arrayAtom.size(); iAtom++ )
			{
				CAtom * pAtom = pChain->m_arrayAtom[iAtom];
				pAtom->m_pChain = pChain;
				pAtom->m_pPDB = this;
				pAtom->m_pParent = pChain;
				pAtom->m_iModel = pChain->m_iModel;

				LARGE_INTEGER li;
				li.LowPart = pAtom->m_serial; 
				li.HighPart = pAtom->m_iModel;

				//	TODO: 수정
				//	m_hashMapAtom.insert( CMapAtomPair( li.QuadPart , pAtom));
			}

			for ( int iResidue = 0 ; iResidue < pChain->m_arrayResidue.size() ; iResidue++ )
			{
				CResidue * pResidue = pChain->m_arrayResidue[iResidue];
				pResidue->m_pParent = pChain;

				strName.Format(_T("RESIDUE:%s") , pResidue->m_residueNameOneChar);
				pResidue->SetName(strName);

				for ( int iAtom = 0 ; iAtom < pResidue->m_arrayAtom.size(); iAtom++ )
				{
					CAtom * pAtom = pResidue->m_arrayAtom[iAtom];
					pAtom->m_pChain = pChain;
					pAtom->m_pPDB = this;
					pAtom->m_pParent = pResidue;
					pAtom->m_iModel = pChain->m_iModel;

					LARGE_INTEGER li;
					li.LowPart = pAtom->m_serial; 
					li.HighPart = pAtom->m_iModel;

					//	TODO: 수정
					//	m_hashMapAtom.insert( CMapAtomPair( li.QuadPart , pAtom));
				}
			}
		}
	}
}


