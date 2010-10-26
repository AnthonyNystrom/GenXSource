//
//	PDB에서 Instancing 되는것을 저장한다.
//	Instance Class 에서 PDB의 readonly class로의 pointer를 가지고 있다.
//	모든 traversal, write가능한 데이터는 여기에 저장.
//	CPDB에는 read-only만 저장.
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

	//	현재의 m_bSelect값을 bSelect로 assign. Child를 부르지 않음.
	virtual		void	SetSelect(BOOL bSelect) { m_bSelect = bSelect; }		
	//	현재의 m_bSelect값을 bSelect로 assign. Child를 부를때는 SetSelectChildNoCheckStatus로 부름
	virtual		void	SetSelectChild(BOOL bSelect) { m_bSelect = bSelect; }

public:
	//	parent에서 bSelect 값으로 현재의 m_bSelect를 채우고 child의 SetSelectChildNoCheckStatus를 부름
	virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect) { m_bSelect = bSelect; }

public:
	//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
	//	parent가 select 되면 child도 선택되므로, 리스트에 child는 들어가지 않는다.
	//	return: 이전값.
	virtual		BOOL	GetSelectNode(CSTLArraySelectionInst &selection);
	virtual		void	GetSelectNodeChild(CSTLArraySelectionInst &selection){GetSelectNode(selection);}

	//	현재 노드의 child atom을 전부 얻어온다. atom 만 얻어온다.
	virtual		void	GetChildAtoms(CSTLArrayAtomInst & atoms) { }	//	실제로 pure virtual

	//	display style	
	//	특정부분을 특정한 style로 display 하기위한것.
	//	ProteinVistaRenderer의 m_arraySelectionDisplay 의 index로 사용
	ULONG		m_displayStyleIndex[MAX_DISPLAY_SELECTION_INDEX/32];		//	32*10 = 320개(실제로 0..319)
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
	CChainInst *	m_pChainInst;				//	atom 이 속하는 chain 의 poiner.		atom pointer 만 가지고 chain을 access 할수 있게 한다.
	CPDBInst *		m_pPDBInst;					//	atom 이 속하는 PDB 의 poiner.		atom pointer 만 가지고 PDB을 access 할수 있게 한다.

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
	CChainInst *		m_pChainInst;			//	자신이 속한 chain pointer.

	//	child link
	CSTLArrayAtomInst	m_arrayAtomInst;
	

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			void SetSelectChild(BOOL bSelect);

		public:
			virtual		void	SetSelectChildNoCheckStatus(BOOL bSelect);

		public:
			//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
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
			//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
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
			//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
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
	CSTLArrayArrayChainInst		m_arrayarrayChainInst;	//	3차원 배열. 

	BEGIN("")
	public:		//	m_serial로 검색을 빠르게하기위해 Hash table을 만들어둔다.
		CSTLMapAtomInst		m_hashMapAtomInst;
	END;

	//	arg 로 지정된 모델과, chain index 를 가지고 체인을 가져온다.
	CChainInst * GetChainInst(long iModel, long iChain) { return m_arrayarrayChainInst[iModel][iChain]; }

	//	BIOUNIT을 만들때 사용되지 않는 chain을 지운다.
	HRESULT DeleteUnUsedChainInBioUnit(CString & strChain);

	BEGIN("PDB Traversal")
		public:
			void SetSelect(BOOL bSelect);
			//	m_bSelect 를 Traversal 하면서 Setting.
			void SetSelectChild(BOOL bSelect);

		public:
			//	traversal 하면서 선택된 m_bSelect 를 찾아서 selection container 에 넣는다.
			//	현재 select 되어진것을 전부 넣는다. atom과 residue는 중복되어 들어감.
			void GetSelectNodeChild(CSTLArraySelectionInst &selection);

			//	Selection list 에 넣어지는 Index를 설정한다.
			void SetDisplayStyleChild(long index, BOOL bStyle);

			//	child atom을 얻는다.
			void GetChildAtoms(CSTLArrayAtomInst & atoms);

	END;

};

typedef std::vector<CPDBInst * > CSTLArrayPDBInst;

