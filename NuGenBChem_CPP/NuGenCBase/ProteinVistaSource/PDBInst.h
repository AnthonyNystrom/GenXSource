//
//	PDB���� Instancing �Ǵ°��� �����Ѵ�.
//	Instance Class ���� PDB�� readonly class���� pointer�� ������ �ִ�.
//	��� traversal, write������ �����ʹ� ���⿡ ����.
//	CPDB���� read-only�� ����.
//

#pragma once

#include "PDB.h"

class CProteinObjectInst;
class CAtomInst;
class CResidueInst;
class CChainInst;
class CModelInst;
class CPDBInst;

typedef std::vector < CAtomInst * > CSTLArrayAtomInst;
typedef std::vector <CProteinObjectInst*> CSTLArraySelectionInst;
typedef std::vector <CProteinObjectInst*> CSTLArrayProteinnObjectInst;

class CProteinObjectInst
{
public:
	CProteinObjectInst() { 
		m_bSelect= FALSE;  
		m_hTreeItem = NULL; 
		ZeroMemory(m_displayStyleIndex, 10*sizeof(ULONG)); 
		m_pParent = NULL; }

	CProteinObjectInst *	m_pParent;

	//CString		m_strReadableName;
	//void		SetName(CString &str)	{ m_strReadableName = str; }
	virtual CString		GetName()	{ return ""; }

public:
	HTREEITEM	m_hTreeItem;

	BOOL		m_bSelect;
	virtual		BOOL	GetSelect() { return m_bSelect; }

	//	������ m_bSelect���� bSelect�� assign. Child�� �θ��� ����.
	virtual		void	SetSelect(BOOL bSelect) { m_bSelect = bSelect; }		
	//	������ m_bSelect���� bSelect�� assign. Child�� �θ����� SetSelectChildNoCheckStatus�� �θ�
	virtual		void	SetSelectChild(BOOL bSelect) { m_bSelect = bSelect; }

public:
	//	parent���� bSelect ������ ������ m_bSelect�� ä��� child�� SetSelectChildNoCheckStatus�� �θ�
	virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect) { m_bSelect = bSelect; }

public:
	//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
	//	parent�� select �Ǹ� child�� ���õǹǷ�, ����Ʈ�� child�� ���� �ʴ´�.
	//	return: ������.
	virtual		BOOL	GetSelectNode(CSTLArraySelectionInst &selection);
	virtual		void	GetSelectNodeChild(CSTLArraySelectionInst &selection){GetSelectNode(selection);}

	//	���� ����� child atom�� ���� ���´�. atom �� ���´�.
	virtual		void	GetChildAtoms(CSTLArrayAtomInst & atoms) { }	//	������ pure virtual

	//	display style	
	//	Ư���κ��� Ư���� style�� display �ϱ����Ѱ�.
	//	ProteinVistaRenderer�� m_arraySelectionDisplay �� index�� ���
	ULONG		m_displayStyleIndex[MAX_DISPLAY_SELECTION_INDEX/32];		//	32*10 = 320��(������ 0..319)
	virtual		BOOL	GetDisplayStyle(long index);
	virtual		void	SetDisplayStyle(long index, BOOL bStyle);	
	virtual		void	SetDisplayStyleChild(long index, BOOL bStyle){SetDisplayStyle(index, bStyle);}

private:

};

//	
//	PDB Instance data.
//	
class CAtomInst: public	CProteinObjectInst
{
private:
	CAtom * m_pAtom;

public:
	CAtomInst(CAtom * pAtom) : 
	  m_pAtom(pAtom), m_pResidueInst(NULL), m_pChainInst(NULL), m_pPDBInst(NULL) { }

	CAtom * GetAtom() { return m_pAtom; }

	CString		GetName()	{ return m_pAtom->GetName(); }

	//	parent link
	CResidueInst *	m_pResidueInst;		//	parent.
	CChainInst *	m_pChainInst;				//	atom �� ���ϴ� chain �� poiner.		atom pointer �� ������ chain�� access �Ҽ� �ְ� �Ѵ�.
	CPDBInst *		m_pPDBInst;					//	atom �� ���ϴ� PDB �� poiner.		atom pointer �� ������ PDB�� access �Ҽ� �ְ� �Ѵ�.

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			void SetSelectChild(BOOL bSelect);
			void GetChildAtoms(CSTLArrayAtomInst & atoms) { atoms.push_back(this); }

		public:
			virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect);
	END;
};

class CResidueInst: public	CProteinObjectInst
{
private:
	CResidue * m_pResidue;

public:
	CResidueInst(CResidue * pResidue) : m_pResidue(pResidue), m_pChainInst(NULL) { }

	CResidue * GetResidue() { return m_pResidue; }

	CString		GetName()	{ return m_pResidue->GetName(); }

	//	parent link
	CChainInst *		m_pChainInst;			//	�ڽ��� ���� chain pointer.

	//	child link
	CSTLArrayAtomInst	m_arrayAtomInst;
	

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			void SetSelectChild(BOOL bSelect);

		public:
			virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect);

		public:
			//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
			void GetSelectNodeChild(CSTLArraySelectionInst &selection);
			void SetDisplayStyleChild(long index, BOOL bStyle);

			void GetChildAtoms(CSTLArrayAtomInst & atoms);
	END;

};

typedef std::vector<CResidueInst * > CSTLArrayResidueInst;

class CChainInst: public	CProteinObjectInst
{
private:
	CChain * m_pChain;

public:
	CChainInst(CChain * pChain) : m_pChain(pChain), m_pPDBInst(NULL), m_pModelInst(NULL)  { }
	~CChainInst();

	CChain * GetChain() { return m_pChain; }

	CString		GetName()	{ return m_pChain->GetName(); }

	//	parent link
	CPDBInst	* m_pPDBInst;
	CModelInst	* m_pModelInst;

	//	child link.
	CSTLArrayResidueInst	m_arrayResidueInst;
	CSTLArrayAtomInst		m_arrayAtomInst;
	CSTLArrayAtomInst		m_arrayHETATMInst;

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			void SetSelectChild(BOOL bSelect);

		public:
			virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect);

		public:
			//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
			void GetSelectNodeChild(CSTLArraySelectionInst &selection);
			void SetDisplayStyleChild(long index, BOOL bStyle);

			void GetChildAtoms(CSTLArrayAtomInst & atoms);
	END;

};

typedef		std::vector<CChainInst * > CSTLArrayChainInst;
typedef		std::vector < CSTLArrayChainInst >	CSTLArrayArrayChainInst;


class CModelInst: public	CProteinObjectInst
{
private:
	CModel * m_pModel;

public:
	CModelInst(CModel * pModel) : m_pModel(pModel), m_pPDBInst(NULL) { }

	CModel * GetModel() { return m_pModel; }

	CString		GetName()	{ return m_pModel->GetName(); }

	//	parent link
	CPDBInst * m_pPDBInst;

	//	child link
	CSTLArrayChainInst	m_arrayChainInst;

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			void SetSelectChild(BOOL bSelect);

		public:
			virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect);

		public:
			//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
			void GetSelectNodeChild(CSTLArraySelectionInst &selection);
			void SetDisplayStyleChild(long index, BOOL bStyle);

			void GetChildAtoms(CSTLArrayAtomInst & atoms);
	END;

};

typedef std::vector<CModelInst * > CSTLArrayModelInst;
typedef std::map < LONGLONG , CAtomInst * >		CSTLMapAtomInst;
typedef std::pair < LONGLONG, CAtomInst * >		CMapAtomInstPair;

class CPDBInst: public	CProteinObjectInst
{
private:
	CPDB *	m_pPDB;

public:
	CPDBInst(CPDB * pPDB) : m_pPDB(pPDB), m_pPDBRenderer(NULL){ }
	~CPDBInst();

	CPDB * GetPDB() { return m_pPDB; }

	CString		GetName()	{ return m_pPDB->GetName(); }

	//	parent link
	CPDBRenderer *			m_pPDBRenderer;

	//	child link
	CSTLArrayModelInst			m_arrayModelInst;
	CSTLArrayArrayChainInst		m_arrayarrayChainInst;	//	3���� �迭. 

	BEGIN("")
	public:		//	m_serial�� �˻��� �������ϱ����� Hash table�� �����д�.
		CSTLMapAtomInst		m_hashMapAtomInst;
	END;

	//	arg �� ������ �𵨰�, chain index �� ������ ü���� �����´�.
	CChainInst * GetChainInst(long iModel, long iChain) { return m_arrayarrayChainInst[iModel][iChain]; }

	//	BIOUNIT�� ���鶧 ������ �ʴ� chain�� �����.
	HRESULT DeleteUnUsedChainInBioUnit(CString & strChain);

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			//	m_bSelect �� Traversal �ϸ鼭 Setting.
			void SetSelectChild(BOOL bSelect);

		public:
			//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�Ƽ� selection container �� �ִ´�.
			//	���� select �Ǿ������� ���� �ִ´�. atom�� residue�� �ߺ��Ǿ� ��.
			void GetSelectNodeChild(CSTLArraySelectionInst &selection);

			//	Selection list �� �־����� Index�� �����Ѵ�.
			void SetDisplayStyleChild(long index, BOOL bStyle);

			//	child atom�� ��´�.
			void GetChildAtoms(CSTLArrayAtomInst & atoms);

	END;

};

typedef std::vector<CPDBInst * > CSTLArrayPDBInst;

