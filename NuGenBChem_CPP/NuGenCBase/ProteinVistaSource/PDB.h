#ifndef __PDB_H__
#define __PDB_H__

 

#include <afxtempl.h>
#include <vector>
#include <map>
#include "GraphicsObject.h"
#include "ColorScheme.h"

class CPDB;
class CChain;
class CResidue;
class	CPDBRenderer;
typedef std::vector < CPDBRenderer * >	CSTLArrayPDBRenderer;


#define		BEGIN(str)
#define		END

//	아톰.


enum {	ATOM_C,
		ATOM_N,
		ATOM_O,
		ATOM_S,
		ATOM_H,
		ATOM_P,
		ATOM_CL,
		ATOM_ZN,
		ATOM_NA,
		ATOM_FE,
		ATOM_MG,
		ATOM_K,
		ATOM_CA,
		ATOM_I,
		ATOM_F,
		ATOM_B,
		ATOM_UNKNOWN 
	};

#define MAX_ATOM		(ATOM_UNKNOWN+1)	

#define ATOM_RADIUS_C			1.689f
#define ATOM_RADIUS_N			1.527f
#define ATOM_RADIUS_O			1.471f
#define ATOM_RADIUS_S			1.972f
#define ATOM_RADIUS_H			1.20f
#define ATOM_RADIUS_P			2.051f
#define ATOM_RADIUS_CL			1.907f
#define ATOM_RADIUS_ZN			1.2524f
#define ATOM_RADIUS_NA			2.400f
#define ATOM_RADIUS_FE			2.125f
#define ATOM_RADIUS_MG			1.636f
#define ATOM_RADIUS_K			2.605f
#define ATOM_RADIUS_CA			2.125f
#define ATOM_RADIUS_I			1.907f
#define ATOM_RADIUS_F			1.35f
#define ATOM_RADIUS_B			1.95f
#define ATOM_RADIUS_UNKNOWN		1.0f



//	레지듀
enum { 
	RSD_ALA,	// alanine
	RSD_ARG,	// arginine
	RSD_ASN,	// asparagine
	RSD_ASP,	// aspartic acid
	RSD_CYS,	// cysteine
	RSD_GLU,	// glutamic acid
	RSD_GLN,	// glutamine
	RSD_GLY,	// glysine
	RSD_HIS,	// histidine
	RSD_ILE,	// isoleucine
	RSD_LEU,	// leucine
	RSD_LYS,	// lysine
	RSD_MET,	// methionine
	RSD_PHE,	// phenylalanine
	RSD_PRO,	// proline
	RSD_SER,	// serine
	RSD_THR,	// threonine
	RSD_TRP,	// tryptophan 
	RSD_TYR,	// tyrosine
	RSD_VAL		// valine
};

//	레지듀HYDROPATHY
#define			HYDROPATHY_RSD_ALA		1.80f		// alanine
#define			HYDROPATHY_RSD_ARG		-4.50f		// arginine
#define			HYDROPATHY_RSD_ASN		-3.50f		// asparagine
#define			HYDROPATHY_RSD_ASP		-3.50f		// aspartic acid
#define			HYDROPATHY_RSD_CYS		2.50f		// cysteine
#define			HYDROPATHY_RSD_GLU		-3.50f		// glutamic acid
#define			HYDROPATHY_RSD_GLN		-3.50f		// glutamine
#define			HYDROPATHY_RSD_GLY		-0.40f		// glysine
#define			HYDROPATHY_RSD_HIS		2.50f		// histidine
#define			HYDROPATHY_RSD_ILE		4.50f		// isoleucine
#define			HYDROPATHY_RSD_LEU		3.80f		// leucine
#define			HYDROPATHY_RSD_LYS		-3.90f		// lysine
#define			HYDROPATHY_RSD_MET		1.90f		// methionine
#define			HYDROPATHY_RSD_PHE		2.80f		// phenylalanine
#define			HYDROPATHY_RSD_PRO		-1.60f		// proline
#define			HYDROPATHY_RSD_SER		-0.80f		// serine
#define			HYDROPATHY_RSD_THR		-0.70f		// threonine
#define			HYDROPATHY_RSD_TRP		-0.90f		// tryptophan 
#define			HYDROPATHY_RSD_TYR		-1.30f		// tyrosine
#define			HYDROPATHY_RSD_VAL		4.20f		// valine


enum { SS_NONE, SS_HELIX, SS_SHEET };
enum { SS_HELIX_DEFAULT, SS_HELIX_PI, SS_HELIX_310 };

enum { MAINCHAIN_N, MAINCHAIN_CA, RESIDUE_CB, MAINCHAIN_C, MAINCHAIN_O, RESIDUE_R };

class CProteinObject;
typedef std::vector <CProteinObject*> CSTLArraySelection;
typedef std::vector <CProteinObject*> CSTLArrayProteinnObject;

class CAtom;
typedef		std::vector <CAtom *>			CSTLArrayAtom;

typedef std::map < LONGLONG , CAtom * >		CSTLMapAtom;
typedef std::pair < LONGLONG, CAtom * >		CMapAtomPair;

typedef std::vector <DWORD>					CSTLArrayDWORD;

//
//

#define		MAX_DISPLAY_SELECTION_INDEX		(32*10)			//	32 로 나눠서 떨어지는 숫자.

class CProteinObject 
{
public:
	CProteinObject() { /*m_bSelect= FALSE;  m_hTreeItem = NULL; ZeroMemory(m_displayStyleIndex, 10*sizeof(ULONG));*/ m_pParent = NULL; }

	CProteinObject *	m_pParent;

	CString				m_strReadableName;
	void		SetName(CString &str)	{ m_strReadableName = str; }
	CString	&	GetName()	{ return m_strReadableName; }

public:
	//HTREEITEM	m_hTreeItem;

	//BOOL		m_bSelect;
	//virtual		BOOL	GetSelect() { return m_bSelect; }
	//virtual		void	SetSelect(BOOL bSelect) { m_bSelect = bSelect; }
	//virtual		void	SetSelectChild(BOOL bSelect) { m_bSelect = bSelect; }

	////	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
	////	parent가 select 되면 child도 선택되므로, 리스트에 child는 들어가지 않는다.
	////	return: 이전값.
	//virtual		BOOL	GetSelectNode(CSTLArraySelection &selection);
	//virtual		void	GetSelectNodeChild(CSTLArraySelection &selection){GetSelectNode(selection);}

	////	현재 노드의 child atom을 전부 얻어온다. atom 만 얻어온다.
	//virtual		void	GetChildAtoms(CSTLArrayAtom & atoms) { }	//	실제로 pure virtual

	////	display style	
	////	특정부분을 특정한 style로 display 하기위한것.
	////	ProteinVistaRenderer의 m_arraySelectionDisplay 의 index로 사용
	//ULONG		m_displayStyleIndex[MAX_DISPLAY_SELECTION_INDEX/32];		//	32*10 = 320개(실제로 0..319)
	//virtual		inline BOOL	GetDisplayStyle(long index);
	//virtual		inline void	SetDisplayStyle(long index, BOOL bStyle);	
	//virtual		void	SetDisplayStyleChild(long index, BOOL bStyle){SetDisplayStyle(index, bStyle);}

private:
	
};

class CAtomPDB : public	CProteinObject		//	pdb 에서 읽어와서 한번 설정하면 안 변하는 것들
{
public:
	CAtomPDB();

	long			m_serial;			//	PDB 안에 들어있는 atom serial
	long			m_arrayIndex;		//	CSTLArrayAtom 에 들어있는 index.

	BOOL			m_bHETATM;

	CString			m_atomName;
	long			m_atomNameIndex;

	TCHAR			m_altLoc;
	CString			m_residueName;
	long			m_residueNameIndex;

	TCHAR			m_chainID;
	long			m_residueNum;				//	pdb 안의 residue num
	D3DXVECTOR3		m_posOrig;
	long			m_iModel;

	float			m_occupancy;
	float			m_temperature;
	float			m_hydropathy;

	BOOL			m_bSideChain;
	long			m_typeAtom;

	float			m_fmass;				//	atom의 질량.
	float			m_fRadius;

	CString			m_strPDBID;				//  Atom이 속한 PDB의 ID
	CChain *		m_pChain;				//	atom 이 속하는 chain 의 poiner.		atom pointer 만 가지고 chain을 access 할수 있게 한다.
	CPDB *			m_pPDB;					//	atom 이 속하는 PDB 의 poiner.		atom pointer 만 가지고 PDB을 access 할수 있게 한다.

	CResidue *		m_pResidue;				//	자신이 속한 residue pointer.

	BEGIN("2차구조 표시")
	public:
		long	m_secondaryStructure;		//	SS_NONE, SS_HELIX, SS_SHEET
		long	m_typeHelix;					//	Helix 일때만 사용: enum { SS_HELIX_DEFAULT, SS_HELIX_PI, SS_HELIX_310 };
	END

	//	DECLARE_DYNAMIC(CAtomPDB)
};

class CAtom : public CAtomPDB
{
public:
	D3DXVECTOR3		m_pos;
	
	HRESULT			SetAtomMass();

	BEGIN("본드 인덱스");
		public:
			CSTLArrayDWORD		m_arrayBondIndex;		//	본드 인덱스 이다. atom 에서 m_arrayBond 로의 인덱스이다.
	END;

	BEGIN("컬러 인덱스")
		public:
			HRESULT SetAtomIndex();		//	디스플레이 속도를 빠르게 하기 위해, 출력되는 인덱스를 지정한다. 같은 인덱스는 같은 모양으로 출력. 
	END

	//BEGIN("PDB Traversal")
	//	public:
	//		void SetSelectChild(BOOL bSelect) { SetSelect(bSelect); }
	//		void GetChildAtoms(CSTLArrayAtom & atoms) { atoms.push_back(this); }
	//END;

public:
	CAtom();
	~CAtom();

	HRESULT Destroy() { return S_OK; }
	//	DECLARE_DYNAMIC(CAtom)
};

typedef std::vector <CResidue *>  CSTLArrayResidue;

class CHelix
{
public:
	CString			m_helixID;
	TCHAR			m_chainID;
	long			m_beginSeqNum;
	long			m_endSeqNum;
	long			m_helixClass;
	CString			m_strHelixClass;

	CSTLArrayResidue	m_arrayResidue;

public:
	CHelix(): m_chainID(0), m_beginSeqNum(0), m_endSeqNum(0), m_helixClass(1) { }
	~CHelix() { }
};

class CSheet 
{
public:
	CString			m_sheetID;		//	"A" , "B" , "C", ...
	
	long			m_indexStrand;

	long			m_numStrand;

	TCHAR			m_chainID;		//	'A' , 'B' , 'C', ...

	long			m_beginSeqNum;
	long			m_endSeqNum;

	long			m_hydronBondIndex1;
	long			m_hydronBondIndex2;

	CSTLArrayResidue	m_arrayResidue;

public:
	CSheet():m_chainID(0), m_indexStrand(0), m_numStrand(0), m_beginSeqNum(0), m_endSeqNum(0), m_hydronBondIndex1(0), m_hydronBondIndex2(0) {  }
	~CSheet() {}
};

typedef		std::vector <CHelix *>		CSTLArrayHelix;
typedef		std::vector <CSheet *>		CSTLArraySheet;

class CResidue	: public	CProteinObject
{
public:
	CResidue() { m_pChain = NULL; ZeroMemory(m_arrayAtomSpecial, sizeof(CAtom*)*5);  m_bExistMainChain = FALSE;  m_bHETATM = FALSE; m_bDNA = FALSE; m_pDNAC5 = NULL; m_center = D3DXVECTOR3(0,0,0); }
	~CResidue() { m_arrayAtom.clear(); }

	long			m_arrayIndex;				//	CResidue가 저장되는 m_arrayResidue 의 index.
	CString			m_residueNameOneChar;

	long			GetResidueNum() { return m_arrayAtom[0]->m_residueNum; }			//	pdb 안의 residue index.
	CString			GetResidueName() { return m_arrayAtom[0]->m_residueName; }
	long			GetSS() { return m_arrayAtom[0]->m_secondaryStructure; }
	long			GetTypeHelix() { return m_arrayAtom[0]->m_typeHelix; }

	CSTLArrayAtom	m_arrayAtom;

	D3DXVECTOR3		m_center;

	//	index로 enum { MAINCHAIN_N, MAINCHAIN_CA, RESIDUE_CB, MAINCHAIN_C, MAINCHAIN_O, RESIDUE_R }; 를 사용.
	CAtom *			m_arrayAtomSpecial[5];		//	N, Ca, Cb, C, O 에 대해서 따로 관리.
	CAtom *			m_pDNAC5;

	CAtom *			GetNAtom() { return m_arrayAtomSpecial[MAINCHAIN_N]; }
	CAtom *			GetCAAtom() { return m_arrayAtomSpecial[MAINCHAIN_CA]; }
	CAtom *			GetCBAtom() { return m_arrayAtomSpecial[RESIDUE_CB]; }
	CAtom *			GetCAtom() { return m_arrayAtomSpecial[MAINCHAIN_C]; }
	CAtom *			GetOAtom() { return m_arrayAtomSpecial[MAINCHAIN_O]; }
	CAtom *			GetC5Atom() { return m_pDNAC5; }

	CChain *		m_pChain;			//	자신이 속한 chain pointer.
	BOOL			m_bExistMainChain;	//	N, Ca, C 가 모두 존재할 경우만 TRUE 가 된다.
	BOOL			m_bHETATM;			
	BOOL			m_bDNA;

	//BEGIN("PDB Traversal")
	//	public:
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	//	DECLARE_DYNAMIC(CResidue)
};

class CResidueRangeInfo
{
public:
	long		m_beginResidue;		//	배열의 index.
	long		m_endResidue;		//	배열의 index.

	CSTLArrayResidue	m_arrayResidue;

	CResidueRangeInfo() { m_beginResidue = m_endResidue = 0; }
};

typedef CResidueRangeInfo	CSSInfo;
typedef std::vector < CSSInfo * > CSTLArraySSInfo;
typedef std::vector < CResidueRangeInfo * > CSTLArrayResidueRangeInfo;

class CAtomRangeInfo
{
public:
	long		m_beginAtom;		//	배열의 index.
	long		m_endAtom;		//	배열의 index.

	CSTLArrayAtom	m_arrayAtom;

	CAtomRangeInfo() { m_beginAtom = m_endAtom = 0; }
};

typedef std::vector < CAtomRangeInfo * > CSTLArrayAtomRangeInfo;

//	아미노산이 연결된 하나의 체인을 나타냄.
class CChain : public CProteinObject
{
public:
	enum { ATOM, HETATM };

	long			m_iModel;		//	현재 chain의 MODEL #

	TCHAR			m_chainID;		//	체인 ID 가 있을 경우에 A,B,C,D 이고, 없을 경우에 '-'로 붙여진다.
	CString			m_strPDBID;		//	1a33_A 와 같은 형태의 PDBID 이다. 
	long			m_arrayIndex;		//	m_arrayChain 에 몇번째 index를 나타냄.
	long			m_arrayIndexModel;		//	m_arrayChain 에 몇번째 index를 나타냄.

	BOOL			m_bDNA;

	//	아톰 배열.
	CSTLArrayAtom	m_arrayAtom;
	CSTLArrayAtom	m_arrayHETATM;

	//	residue 배열
	CSTLArrayResidue	m_arrayResidue;

	//	visualization

	//	Init. Destroy
	CPDB * m_pPDB;
	CChain();
	~CChain();

	HRESULT Destroy();

	BEGIN("파일을 읽고 처리되는 작업들")
		public:
			HRESULT FindBond();							//	자신의 amino acid 안에서 연결을 찾는다.
			//	HRESULT FindDotSurfaceBond();

			//	아래 4개는 디폴트로 수행됨
			HRESULT SetAtomTypeAndResidueIndex();		//	Atom에 type의 assign 하고, m_arrayResidue를 생성
			HRESULT SetSecondaryStructureAtomAssign();
			HRESULT SetChainSecondaryStructure();
			HRESULT	SetSequenceData();
			HRESULT	FindCenterRadius();
			
		private:
			float	m_bondLengthMin;
			float	m_bondLengthMax;

			float	m_bondVanDerWaalsMin;
			float	m_bondVanDerWaalsMax;
	END

	BEGIN("2차 구조 데이타 ")
		public:
			CSTLArraySSInfo		m_arrayHelix;
			CSTLArraySSInfo		m_arraySheet;
	END

	public:
		D3DXVECTOR3		m_chainCenter;
		FLOAT			m_chainRadius;
		D3DXVECTOR3		m_chainMinMaxBB[2];

	public:
		CString		m_strSequenceData;

	BEGIN("Ca, Cb 에 대한 pseudoAngle");
		public:
			CSTLFLOATArray			m_arrayPseudoAngleCa;
			CSTLFLOATArray			m_arrayPseudoAngleCb;

			CSTLArrayResidue		m_arrayResiduePseudoAngle;		//	pseudo angle 이 구해진 residue를 여기 list 에 넣는다.

			CSTLVectorValueArray	m_atomPosArrayCa; 
			CSTLVectorValueArray	m_atomPosArrayCb;
			CString					m_aaID;
			CSTLLONGArray			m_sseArray;					//	--HH--SS---	 이런형태로 구성
			CSTLLONGArray			m_sseIndexArray;			//	2개가 1쌍으로 2차구조의 index 를 저장. H, S를 구분하지 않는다.
	END

	BEGIN("Visualization");
		CSTLArrayDWORD		m_arrayBond;				//	본드를 지정.
		
		D3DXMATRIX * m_pD3DRotationMatrix1;		//	본드의 transform 을 미리 계산. 인덱스는 m_arrayBond 와 같다.
		D3DXMATRIX * m_pD3DRotationMatrix2;
		D3DXVECTOR4	*	m_arrayBondPos1;
		D3DXVECTOR4	*	m_arrayBondPos2;
		D3DXVECTOR2	*	m_arrayBondRotation1;
		D3DXVECTOR2	*	m_arrayBondRotation2;
		FLOAT	*		m_arrayBondScale;

		//	아래 3개는 obsolete. 예정...
		//	m_arrayResidue 로 대치
		CSTLArrayDWORD	m_arrayResidueIndex;			//	아미노산별로 m_arrayAtom 의 인덱스를 지정. 첫번째 값에 0 은 포함되지 않는다.
		CSTLArrayDWORD	m_arrayResidueIndexBegin;		//	아미노산별로 m_arrayAtom 의 시작 인덱스를 지정.
		CSTLArrayDWORD	m_arrayResidueIndexEnd;			//	아미노산별로 m_arrayAtom 의 끝 인덱스를 지정.
	END;

	//BEGIN("PDB Traversal")
	//	public:
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	//	DECLARE_DYNAMIC(CChain)
};

typedef		std::vector < CChain * >	CSTLArrayChain;
typedef		std::vector < CSTLArrayChain >	CSTLArrayArrayChain;

class CModel : public	CProteinObject
{
public:
	CModel() { m_iModel = 0; m_pPDB = NULL; m_bValidTreeCtrl = TRUE; }

	BOOL			m_bValidTreeCtrl;
	long			m_iModel;
	CSTLArrayChain	m_arrayChain;

	//BEGIN("PDB Traversal")
	//	public:
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal 하면서 선택된 m_bSelect 를 찾아 넣는다.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	CPDB *			m_pPDB;
};
typedef		std::vector < CModel * > CSTLArrayModel;

//	Protein 컨테이너.
class CPDB : public	CProteinObject
{
private:	TCHAR *			m_pCurrentBuffPDB;

public:
			CPDBRenderer *	m_pPDBRenderer;

public:
			CString			m_strPDBID;
			CString			m_strFilename;

	BEGIN("BioUnit")
		public:
			BOOL				m_bExistBioUnit;

			CStringArray		m_strBioUnit;

			CStringArray		m_strArrayBioUnitChain;
			CSTLArrayMatrixValue	m_bioUnitMatrix;
	END;


	BEGIN("title section")
		public:
			CStringArray	m_strArrayHeader;
			CStringArray	m_strArrayTitle;
			CStringArray	m_strArraySource;
			CStringArray	m_strArrayKeywords;
			CStringArray	m_strArrayEXPDTA;
			CStringArray	m_strArrayAuthor;
			CStringArray	m_strArrayRevisionDate;
			CStringArray	m_strArrayJournalCitation;
			CStringArray	m_strArrayRemark;
	END;

	//	structure section
	
	BEGIN("Secondary Structure");
			BOOL		m_bUseStride;		
			void		SetUseStride(BOOL bUse) { m_bUseStride = bUse; }		//	2차구조가 들어있어도 무조건 stride를 사용
			BOOL		IsExecuteStride() { return m_bUseStride; }
			CSTLArrayHelix		m_arrayHelix;		//	PDB 안의 헬릭스 구조에 대한 정보 보관. (체인과 구분없이 전부 다)
			CSTLArraySheet		m_arraySheet;		//	PDB 안의 Sheet 구조.
	END;

	BEGIN("Coordinate Section");		//	아미노산 배열 - 아톰. 헤테로 아톰. 등
		public: 
			//	SCALE FILED in the pdb
			D3DXMATRIX		m_matScale;

		public:
			CSTLArrayArrayChain		m_arrayarrayChain;	//	3차원 배열. 

		private:
			CChain	*		m_pCurrentChain;
			BYTE			m_currentChainID;
	END;

	BEGIN("MODEL")
		public:
			//	MODEL 필드가 없는것은 m_arrayModel 의 size() 가 1 이다. -> 사용되지 않는다.
			//	m_arrayChain[] 과 병행해서 사용.
			CSTLArrayModel	m_arrayModel;
			long			m_iCurrentModel;	//	PDB 에서 읽은 Model #
			long			m_iArrayModel;			//	배열에 들어가는 모델 index.
		private:
			CModel	*		m_pModel;

	END;

	BEGIN("");
		public:
			D3DXVECTOR3			m_pdbCenter;
			D3DXVECTOR3			m_pdbMinMaxBB[2];
			FLOAT				m_pdbRadius;
	END;

	BEGIN("Load");
		public:
			//	load.
			//	bDebug 을 TRUE 이면 load 된 파일을 debug.pdb 에 똑같이 write 한다.
			HRESULT		Load(const TCHAR * filename, BOOL bDebug = FALSE);
			HRESULT		LoadPDB(const TCHAR * filename, BOOL bDebug = FALSE);
			HRESULT		LoadXML(const TCHAR * filename, BOOL bDebug = FALSE);

			//	HRESULT		DeleteUselessChain(CString & strChain);

			//	chain 에 대해서 예외상황 처리
		private:
			TCHAR	m_assignBlankChainID;
			BOOL	ValidateChain(CChain * m_pChain);
			void	MakeBioUnitTransform();
			void	ModifyPDBChain();
			void	SetAtomParentPointer();

		public:	//	사실상 private
			//	Title section
			HRESULT		TitleSection(long index, CString &str);

			//	Coordinate section
			HRESULT		MODELField(long index, CString &str);
			HRESULT		ATOMField(long index, CString &str, BOOL bAtom);
			HRESULT		ATOMField(IXMLDOMNode*, long index, CString &str);

			HRESULT		SIGATMField(long index, CString &str);
			HRESULT		ANISOUField(long index, CString &str);
			HRESULT		SIGUIJField(long index, CString &str);
			HRESULT		TERField(long index, CString &str);
			HRESULT		HETATMField(long index, CString &str);
			HRESULT		ENDMDLField(long index, CString &str);

			//	second structure
			HRESULT		HELIXField(long index, CString &str);
			HRESULT	    HELIXField(IXMLDOMNode* pNode,CString &strField);

			HRESULT		SHEETField(long index, CString &str);
			HRESULT		SHEETField1(IXMLDOMNode*, CString &str);
			HRESULT		SHEETField2(IXMLDOMNode*, CString &str);
			HRESULT		SHEETField3(IXMLDOMNode*, CString &str);
			HRESULT		SHEETField4(IXMLDOMNode*, CString &str);

			HRESULT		SCALEField(long index, CString &str);

			//	BookKeeping Section
			HRESULT		ENDField(long index, CString &str);
	END;

	BEGIN("")
		public:		//	m_serial로 검색을 빠르게하기위해 Hash table을 만들어둔다.
			//	CSTLMapAtom		m_hashMapAtom;
	END;
	//
	//
	//	
	BEGIN("구조적 결합 찾기")
		public:
			HRESULT AnalizeStructure();		//	2차구조 없을때 stride 실행.

			BOOL	StrideWrapper();
			HRESULT FindBond();
			HRESULT	FindCenterRadiusBB();
						
		private:
			//	아래 4개는 load 된 후 자동으로 실행됨
			HRESULT SetAtomTypeAndResidueIndex();			//	type을 define 하고, m_arrayResidue를 구성
			HRESULT	SetSequenceData();						//	sequence 를 설정.

			HRESULT SetSecondaryStructureAtomAssign();		//	arrayAtom 에 2차구조를 표시
			HRESULT SetChainSecondaryStructure();			//	arrayAtom 을 가지고 chain의 SS를 재구성
			
	END;


	public:
	//	Init. Destroy
		CPDB();
		~CPDB();
		HRESULT Destroy();
		
		//	utility function...
		//	arg 로 지정된 모델안에 체인이 몇개인가를 알아낸다.
		long	GetNumChain(long iModel) { return m_arrayarrayChain[iModel].size(); }

		//	arg 로 지정된 모델과, chain index 를 가지고 체인을 가져온다.
		CChain * GetChain(long iModel, long iChain) { return m_arrayarrayChain[iModel][iChain]; }

		//	atom pointer array 와 HETATM pointer array 를 얻어온다.
		CSTLArrayAtom & GetAtomPtrArray(long iModel, long iChain) { return  (m_arrayarrayChain[iModel][iChain])->m_arrayAtom; }
		CSTLArrayAtom & GetHETATMPtrArray(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayHETATM; }

		//	모델, 체인안에 아톰이 몇개인지 알아냄.
		long	GetNumAtom(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayAtom.size(); }
		long	GetNumHETATM(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayHETATM.size(); }

		//	체인안에 residue 가 몇개인지 알아냄.
		long	GetNumResidue(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayResidueIndex.size(); }

		//	2차 구조.
		CSTLArrayHelix & GetHelixArray() { return m_arrayHelix; }
		CSTLArraySheet & GetSheetArray() { return m_arraySheet; }

	//BEGIN("PDB Traversal")
	//	public:
	//		//	m_bSelect 를 Traversal 하면서 Setting.
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal 하면서 선택된 m_bSelect 를 찾아서 selection container 에 넣는다.
	//		//	현재 select 되어진것을 전부 넣는다. atom과 residue는 중복되어 들어감.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);

	//		//	Selection list 에 넣어지는 Index를 설정한다.
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		//	child atom을 얻는다.
	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	//	DECLARE_DYNAMIC(CPDB)
};

typedef		std::vector < CPDB * > CSTLArrayPDB;

typedef HRESULT (*PDBFunction)(CPDB * , long , CString &);

#endif

