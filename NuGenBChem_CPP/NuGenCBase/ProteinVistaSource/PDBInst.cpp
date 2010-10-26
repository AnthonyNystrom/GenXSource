#include "stdafx.h"
#include "pdb.h"
#include "pdbInst.h"
#include "PDBRenderer.h"


void CPDBInst::SetSelect(BOOL bSelect)
{
	if ( bSelect == TRUE )
		m_pPDBRenderer->m_bIsSelectionExist = TRUE;

	m_bSelect = bSelect;
}

void CModelInst::SetSelect(BOOL bSelect)
{
	if ( bSelect == TRUE )
		m_pPDBInst->m_pPDBRenderer->m_bIsSelectionExist = TRUE;

	m_bSelect = bSelect;
}

void CChainInst::SetSelect(BOOL bSelect)
{
	if ( bSelect == TRUE )
		m_pPDBInst->m_pPDBRenderer->m_bIsSelectionExist = TRUE;

	m_bSelect = bSelect;
}

void CResidueInst::SetSelect(BOOL bSelect)
{
	if ( bSelect == TRUE )
		m_pChainInst->m_pPDBInst->m_pPDBRenderer->m_bIsSelectionExist = TRUE;

	m_bSelect = bSelect;
}

void CAtomInst::SetSelect(BOOL bSelect)
{ 
	if ( bSelect == TRUE )
		m_pPDBInst->m_pPDBRenderer->m_bIsSelectionExist = TRUE;

	m_bSelect = bSelect;
}

//
//
void CPDBInst::SetSelectChild(BOOL bSelect)
{
	if ( bSelect == FALSE && m_pPDBRenderer->m_bIsSelectionExist == FALSE )
		return;

	//	�ڽ��� select ��
	SetSelect(bSelect);

	long iModel = 0;
	for ( int iModel = 0 ; iModel < m_arrayModelInst.size(); iModel++ )
	{
		m_arrayModelInst[iModel]->SetSelectChildNoCheckStatus(bSelect);
	}

	//	PDB���� bSelect�� false �̸� PDB ��ü�� deselect �Ȱ���.
	if ( bSelect == FALSE )
		m_pPDBRenderer->m_bIsSelectionExist = FALSE;
}

void CModelInst::SetSelectChild(BOOL bSelect)
{
	if ( bSelect == FALSE && m_pPDBInst->m_pPDBRenderer->m_bIsSelectionExist == FALSE )
		return;

	SetSelect(bSelect);
	for ( int iChain = 0 ; iChain < m_arrayChainInst.size(); iChain++ )
	{
		m_arrayChainInst[iChain]->SetSelectChildNoCheckStatus(bSelect);
	}
}

void CChainInst::SetSelectChild(BOOL bSelect)
{
	if ( bSelect == FALSE && m_pPDBInst->m_pPDBRenderer->m_bIsSelectionExist == FALSE )
		return;

	//	�ڽ��� select ��
	SetSelect(bSelect);

	for ( int k = 0 ; k < m_arrayResidueInst.size() ; k++ )
	{
		CResidueInst * pResidue = m_arrayResidueInst[k];
		pResidue->SetSelectChildNoCheckStatus(bSelect);
	}

	for ( k = 0 ; k < m_arrayHETATMInst.size() ; k++ )
	{
		m_arrayHETATMInst[k]->SetSelectChildNoCheckStatus(bSelect);
	}
}

void CResidueInst::SetSelectChild(BOOL bSelect)
{
	//	�ڽ��� select ��
	SetSelect(bSelect);

	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size(); iAtom ++ )
	{
		m_arrayAtomInst[iAtom]->SetSelectChildNoCheckStatus(bSelect);
	}
}

void CAtomInst::SetSelectChild(BOOL bSelect)
{ 
	SetSelect(bSelect); 
}

//
//	Hierarchy chain���� �Ҹ��°�����, �� ó�� �θ������� m_bIsSelectionExist �� ������, �߰��� m_bIsSelectionExist�� ���� ����
//	so, void CPDBInst::SetSelectChildNoCheckStatus(BOOL bSelect) �� ����.
//
void CModelInst::SetSelectChildNoCheckStatus(BOOL bSelect)
{
	m_bSelect = bSelect;

	for ( int iChain = 0 ; iChain < m_arrayChainInst.size(); iChain++ )
	{
		m_arrayChainInst[iChain]->SetSelectChildNoCheckStatus(bSelect);
	}
}

void CChainInst::SetSelectChildNoCheckStatus(BOOL bSelect)
{
	//	�ڽ��� select ��
	m_bSelect = bSelect;

	for ( int k = 0 ; k < m_arrayResidueInst.size() ; k++ )
	{
		CResidueInst * pResidue = m_arrayResidueInst[k];
		pResidue->SetSelectChildNoCheckStatus(bSelect);
	}

	for ( k = 0 ; k < m_arrayHETATMInst.size() ; k++ )
	{
		m_arrayHETATMInst[k]->SetSelectChildNoCheckStatus(bSelect);
	}
}

void CResidueInst::SetSelectChildNoCheckStatus(BOOL bSelect)
{
	//	�ڽ��� select ��
	m_bSelect = bSelect;

	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size(); iAtom ++ )
	{
		m_arrayAtomInst[iAtom]->SetSelectChildNoCheckStatus(bSelect);
	}
}

void CAtomInst::SetSelectChildNoCheckStatus(BOOL bSelect)
{ 
	m_bSelect = bSelect;
}

//
//
//
BOOL CProteinObjectInst::GetSelectNode(CSTLArraySelectionInst &selection)
{
	if ( m_bSelect == TRUE )
		selection.push_back(this);

	return m_bSelect;
}

//========================================================================================
//========================================================================================
static ULONG	bitmask[32] = {
	0x0001,
	0x0002,
	0x0004,
	0x0008,
	0x0010,
	0x0020,
	0x0040,
	0x0080,
	0x0100,
	0x0200,
	0x0400,
	0x0800,
	0x1000,
	0x2000,
	0x4000,
	0x8000,
	0x00010000,
	0x00020000,
	0x00040000,
	0x00080000,
	0x00100000,
	0x00200000,
	0x00400000,
	0x00800000,
	0x01000000,
	0x02000000,
	0x04000000,
	0x08000000,
	0x10000000,
	0x20000000,
	0x40000000,
	0x80000000
};

BOOL CProteinObjectInst::GetDisplayStyle(long index)
{
	ULONG Bit = m_displayStyleIndex[index/32];
	return !(!( Bit & ( bitmask[index%32] ) ));
}

void CProteinObjectInst::SetDisplayStyle(long index, BOOL bStyle) 
{
	ULONG & Bit = m_displayStyleIndex[index/32];
	if ( bStyle == TRUE )
		Bit = Bit | ( bitmask[index%32] ) ;
	else
		Bit = Bit & ~( bitmask[index%32] ) ;
}

//	old version. compare speed
//BOOL CProteinObjectInst::GetDisplayStyle(long index)
//{
//	ULONG Bit = m_displayStyleIndex[index/32];
//	return !(!( Bit & ( 1<<(index%32) ) ));
//}
//
//void CProteinObjectInst::SetDisplayStyle(long index, BOOL bStyle) 
//{
//	ULONG & Bit = m_displayStyleIndex[index/32];
//	if ( bStyle == TRUE )
//		Bit = Bit | ( 1<<(index%32) ) ;
//	else
//		Bit = Bit & ~( 1<<(index%32) ) ;
//}

//////////////////////////////////////////////////////////////////////////

void CPDBInst::SetDisplayStyleChild(long index, BOOL bStyle)
{
	SetDisplayStyle(index, bStyle);

	long iModel = 0;
	for ( int iModel = 0 ; iModel < m_arrayModelInst.size(); iModel++ )
	{
		m_arrayModelInst[iModel]->SetDisplayStyleChild(index, bStyle);
	}
}

void CModelInst::SetDisplayStyleChild(long index, BOOL bStyle)
{
	SetDisplayStyle(index, bStyle);

	for ( int iChain = 0 ; iChain < m_arrayChainInst.size(); iChain++ )
	{
		m_arrayChainInst[iChain]->SetDisplayStyleChild(index, bStyle);
	}
}

void CChainInst::SetDisplayStyleChild(long index, BOOL bStyle)
{
	SetDisplayStyle(index, bStyle);

	for ( int k = 0 ; k < m_arrayResidueInst.size() ; k++ )
	{
		m_arrayResidueInst[k]->SetDisplayStyleChild(index, bStyle);
	}

	for ( k = 0 ; k < m_arrayHETATMInst.size() ; k++ )
	{
		m_arrayHETATMInst[k]->SetDisplayStyle(index, bStyle);
	}
}

void CResidueInst::SetDisplayStyleChild(long index, BOOL bStyle)
{
	SetDisplayStyle(index, bStyle);

	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size(); iAtom ++ )
	{
		m_arrayAtomInst[iAtom]->SetDisplayStyle(index, bStyle);
	}
}

//===============================================================================
//===============================================================================
//===============================================================================

CChainInst::~CChainInst()
{
	for ( int i = 0 ; i < m_arrayResidueInst.size() ; i++ )
	{
		SAFE_DELETE(m_arrayResidueInst[i]);
	}

	for ( int i = 0 ; i < m_arrayAtomInst.size() ; i++ )
	{
		SAFE_DELETE(m_arrayAtomInst[i]);
	}
}

CPDBInst::~CPDBInst()
{
	for ( int i = 0 ; i < m_arrayModelInst.size() ; i++ )
	{
		SAFE_DELETE( m_arrayModelInst[i] );
	}

	for ( int i = 0 ; i < m_arrayarrayChainInst.size() ; i++ )
	{
		for ( int j = 0 ; j < m_arrayarrayChainInst[i].size() ; j++ )
		{
			SAFE_DELETE( (m_arrayarrayChainInst[i])[j] );
		}
		m_arrayarrayChainInst[i].clear();
	}
}

void CPDBInst::GetSelectNodeChild(CSTLArraySelectionInst &selection)
{
	if ( m_pPDBRenderer->m_bIsSelectionExist == TRUE )
	{
		if ( GetSelectNode(selection) == TRUE )
			return;

		long iModel = 0;
		for ( int iModel = 0 ; iModel < m_arrayModelInst.size(); iModel++ )
		{
			m_arrayModelInst[iModel]->GetSelectNodeChild(selection);
		}
	}
}

void CModelInst::GetSelectNodeChild(CSTLArraySelectionInst &selection)
{
	if ( GetSelectNode(selection) == TRUE )
		return;

	for ( int iChain = 0 ; iChain < m_arrayChainInst.size(); iChain++ )
	{
		m_arrayChainInst[iChain]->GetSelectNodeChild(selection);
	}
}

void CChainInst::GetSelectNodeChild(CSTLArraySelectionInst &selection)
{
	if ( GetSelectNode(selection) == TRUE )
		return;

	for ( int k = 0 ; k < m_arrayResidueInst.size() ; k++ )
	{
		m_arrayResidueInst[k]->GetSelectNodeChild(selection);
	}

	for ( k = 0 ; k < m_arrayHETATMInst.size() ; k++ )
	{
		m_arrayHETATMInst[k]->GetSelectNode(selection);
	}
}

void CResidueInst::GetSelectNodeChild(CSTLArraySelectionInst &selection)
{
	if ( GetSelectNode(selection) == TRUE )
		return;

	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size(); iAtom ++ )
	{
		m_arrayAtomInst[iAtom]->GetSelectNode(selection);
	}
}

//////////////////////////////////////////////////////////////////////////

void CPDBInst::GetChildAtoms(CSTLArrayAtomInst & atoms)
{
	long iModel = 0;
	for ( int iModel = 0 ; iModel < m_arrayModelInst.size(); iModel++ )
	{
		m_arrayModelInst[iModel]->GetChildAtoms(atoms);
	}
}

void CModelInst::GetChildAtoms(CSTLArrayAtomInst & atoms)
{
	for ( int iChain = 0 ; iChain < m_arrayChainInst.size(); iChain++ )
	{
		m_arrayChainInst[iChain]->GetChildAtoms(atoms);
	}
}

void CChainInst::GetChildAtoms(CSTLArrayAtomInst & atoms)
{
	for ( int k = 0 ; k < m_arrayResidueInst.size() ; k++ )
	{
		CResidueInst * pResidue = m_arrayResidueInst[k];
		pResidue->GetChildAtoms(atoms);
	}
}

void CResidueInst::GetChildAtoms(CSTLArrayAtomInst & atoms)
{
	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size(); iAtom ++ )
	{
		m_arrayAtomInst[iAtom]->GetChildAtoms(atoms);
	}
}

//	
//
//	BioUnit Molecule�� ���鶧, duplicate ���� �ʴ� chain���� �����.
//	�ԷµǴ� chain ID�� ��ȿ�Ѱ�.. �̰� �̿��ǰ͵��� �����.
//	
HRESULT CPDBInst::DeleteUnUsedChainInBioUnit(CString & strChain)
{
	//	��� chain�� biounit���� ���
	if ( strChain == _T("*") )
		return S_OK;

	for ( int iModel = 0 ; iModel < m_arrayModelInst.size() ; iModel ++ )
	{
		CModelInst * pModelInst = m_arrayModelInst[iModel];

		for ( int iChain= 0 ; iChain < pModelInst->m_arrayChainInst.size() ; iChain++ )
		{
			CChainInst * pChainInst = pModelInst->m_arrayChainInst[iChain];
			TCHAR chainID = pChainInst->GetChain()->m_chainID;

			BOOL bFindID = FALSE;
			for ( int iID = 0 ; iID < strChain.GetLength() ; iID ++ )
			{
				if ( strChain[iID] == chainID )
				{
					bFindID = TRUE;
					break;
				}
			}

			if ( bFindID == FALSE )
			{
				//	strChain ���� �߰ߵ��� ������ ����� ���̴�.
				pModelInst->m_arrayChainInst.erase(pModelInst->m_arrayChainInst.begin()+iChain);

				//	���⵵ ���� �����.
				m_arrayarrayChainInst[iModel].erase(m_arrayarrayChainInst[iModel].begin()+iChain);

				delete pChainInst;

				iChain--;
			}
		}
	}

	return S_OK;
}
