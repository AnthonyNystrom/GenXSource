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

//	����.


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



//	������
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

//	������HYDROPATHY
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

#define		MAX_DISPLAY_SELECTION_INDEX		(32*10)			//	32 �� ������ �������� ����.

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

	////	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
	////	parent�� select �Ǹ� child�� ���õǹǷ�, ����Ʈ�� child�� ���� �ʴ´�.
	////	return: ������.
	//virtual		BOOL	GetSelectNode(CSTLArraySelection &selection);
	//virtual		void	GetSelectNodeChild(CSTLArraySelection &selection){GetSelectNode(selection);}

	////	���� ����� child atom�� ���� ���´�. atom �� ���´�.
	//virtual		void	GetChildAtoms(CSTLArrayAtom & atoms) { }	//	������ pure virtual

	////	display style	
	////	Ư���κ��� Ư���� style�� display �ϱ����Ѱ�.
	////	ProteinVistaRenderer�� m_arraySelectionDisplay �� index�� ���
	//ULONG		m_displayStyleIndex[MAX_DISPLAY_SELECTION_INDEX/32];		//	32*10 = 320��(������ 0..319)
	//virtual		inline BOOL	GetDisplayStyle(long index);
	//virtual		inline void	SetDisplayStyle(long index, BOOL bStyle);	
	//virtual		void	SetDisplayStyleChild(long index, BOOL bStyle){SetDisplayStyle(index, bStyle);}

private:
	
};

class CAtomPDB : public	CProteinObject		//	pdb ���� �о�ͼ� �ѹ� �����ϸ� �� ���ϴ� �͵�
{
public:
	CAtomPDB();

	long			m_serial;			//	PDB �ȿ� ����ִ� atom serial
	long			m_arrayIndex;		//	CSTLArrayAtom �� ����ִ� index.

	BOOL			m_bHETATM;

	CString			m_atomName;
	long			m_atomNameIndex;

	TCHAR			m_altLoc;
	CString			m_residueName;
	long			m_residueNameIndex;

	TCHAR			m_chainID;
	long			m_residueNum;				//	pdb ���� residue num
	D3DXVECTOR3		m_posOrig;
	long			m_iModel;

	float			m_occupancy;
	float			m_temperature;
	float			m_hydropathy;

	BOOL			m_bSideChain;
	long			m_typeAtom;

	float			m_fmass;				//	atom�� ����.
	float			m_fRadius;

	CString			m_strPDBID;				//  Atom�� ���� PDB�� ID
	CChain *		m_pChain;				//	atom �� ���ϴ� chain �� poiner.		atom pointer �� ������ chain�� access �Ҽ� �ְ� �Ѵ�.
	CPDB *			m_pPDB;					//	atom �� ���ϴ� PDB �� poiner.		atom pointer �� ������ PDB�� access �Ҽ� �ְ� �Ѵ�.

	CResidue *		m_pResidue;				//	�ڽ��� ���� residue pointer.

	BEGIN("2������ ǥ��")
	public:
		long	m_secondaryStructure;		//	SS_NONE, SS_HELIX, SS_SHEET
		long	m_typeHelix;					//	Helix �϶��� ���: enum { SS_HELIX_DEFAULT, SS_HELIX_PI, SS_HELIX_310 };
	END

	//	DECLARE_DYNAMIC(CAtomPDB)
};

class CAtom : public CAtomPDB
{
public:
	D3DXVECTOR3		m_pos;
	
	HRESULT			SetAtomMass();

	BEGIN("���� �ε���");
		public:
			CSTLArrayDWORD		m_arrayBondIndex;		//	���� �ε��� �̴�. atom ���� m_arrayBond ���� �ε����̴�.
	END;

	BEGIN("�÷� �ε���")
		public:
			HRESULT SetAtomIndex();		//	���÷��� �ӵ��� ������ �ϱ� ����, ��µǴ� �ε����� �����Ѵ�. ���� �ε����� ���� ������� ���. 
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

	long			m_arrayIndex;				//	CResidue�� ����Ǵ� m_arrayResidue �� index.
	CString			m_residueNameOneChar;

	long			GetResidueNum() { return m_arrayAtom[0]->m_residueNum; }			//	pdb ���� residue index.
	CString			GetResidueName() { return m_arrayAtom[0]->m_residueName; }
	long			GetSS() { return m_arrayAtom[0]->m_secondaryStructure; }
	long			GetTypeHelix() { return m_arrayAtom[0]->m_typeHelix; }

	CSTLArrayAtom	m_arrayAtom;

	D3DXVECTOR3		m_center;

	//	index�� enum { MAINCHAIN_N, MAINCHAIN_CA, RESIDUE_CB, MAINCHAIN_C, MAINCHAIN_O, RESIDUE_R }; �� ���.
	CAtom *			m_arrayAtomSpecial[5];		//	N, Ca, Cb, C, O �� ���ؼ� ���� ����.
	CAtom *			m_pDNAC5;

	CAtom *			GetNAtom() { return m_arrayAtomSpecial[MAINCHAIN_N]; }
	CAtom *			GetCAAtom() { return m_arrayAtomSpecial[MAINCHAIN_CA]; }
	CAtom *			GetCBAtom() { return m_arrayAtomSpecial[RESIDUE_CB]; }
	CAtom *			GetCAtom() { return m_arrayAtomSpecial[MAINCHAIN_C]; }
	CAtom *			GetOAtom() { return m_arrayAtomSpecial[MAINCHAIN_O]; }
	CAtom *			GetC5Atom() { return m_pDNAC5; }

	CChain *		m_pChain;			//	�ڽ��� ���� chain pointer.
	BOOL			m_bExistMainChain;	//	N, Ca, C �� ��� ������ ��츸 TRUE �� �ȴ�.
	BOOL			m_bHETATM;			
	BOOL			m_bDNA;

	//BEGIN("PDB Traversal")
	//	public:
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	//	DECLARE_DYNAMIC(CResidue)
};

class CResidueRangeInfo
{
public:
	long		m_beginResidue;		//	�迭�� index.
	long		m_endResidue;		//	�迭�� index.

	CSTLArrayResidue	m_arrayResidue;

	CResidueRangeInfo() { m_beginResidue = m_endResidue = 0; }
};

typedef CResidueRangeInfo	CSSInfo;
typedef std::vector < CSSInfo * > CSTLArraySSInfo;
typedef std::vector < CResidueRangeInfo * > CSTLArrayResidueRangeInfo;

class CAtomRangeInfo
{
public:
	long		m_beginAtom;		//	�迭�� index.
	long		m_endAtom;		//	�迭�� index.

	CSTLArrayAtom	m_arrayAtom;

	CAtomRangeInfo() { m_beginAtom = m_endAtom = 0; }
};

typedef std::vector < CAtomRangeInfo * > CSTLArrayAtomRangeInfo;

//	�ƹ̳���� ����� �ϳ��� ü���� ��Ÿ��.
class CChain : public CProteinObject
{
public:
	enum { ATOM, HETATM };

	long			m_iModel;		//	���� chain�� MODEL #

	TCHAR			m_chainID;		//	ü�� ID �� ���� ��쿡 A,B,C,D �̰�, ���� ��쿡 '-'�� �ٿ�����.
	CString			m_strPDBID;		//	1a33_A �� ���� ������ PDBID �̴�. 
	long			m_arrayIndex;		//	m_arrayChain �� ���° index�� ��Ÿ��.
	long			m_arrayIndexModel;		//	m_arrayChain �� ���° index�� ��Ÿ��.

	BOOL			m_bDNA;

	//	���� �迭.
	CSTLArrayAtom	m_arrayAtom;
	CSTLArrayAtom	m_arrayHETATM;

	//	residue �迭
	CSTLArrayResidue	m_arrayResidue;

	//	visualization

	//	Init. Destroy
	CPDB * m_pPDB;
	CChain();
	~CChain();

	HRESULT Destroy();

	BEGIN("������ �а� ó���Ǵ� �۾���")
		public:
			HRESULT FindBond();							//	�ڽ��� amino acid �ȿ��� ������ ã�´�.
			//	HRESULT FindDotSurfaceBond();

			//	�Ʒ� 4���� ����Ʈ�� �����
			HRESULT SetAtomTypeAndResidueIndex();		//	Atom�� type�� assign �ϰ�, m_arrayResidue�� ����
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

	BEGIN("2�� ���� ����Ÿ ")
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

	BEGIN("Ca, Cb �� ���� pseudoAngle");
		public:
			CSTLFLOATArray			m_arrayPseudoAngleCa;
			CSTLFLOATArray			m_arrayPseudoAngleCb;

			CSTLArrayResidue		m_arrayResiduePseudoAngle;		//	pseudo angle �� ������ residue�� ���� list �� �ִ´�.

			CSTLVectorValueArray	m_atomPosArrayCa; 
			CSTLVectorValueArray	m_atomPosArrayCb;
			CString					m_aaID;
			CSTLLONGArray			m_sseArray;					//	--HH--SS---	 �̷����·� ����
			CSTLLONGArray			m_sseIndexArray;			//	2���� 1������ 2�������� index �� ����. H, S�� �������� �ʴ´�.
	END

	BEGIN("Visualization");
		CSTLArrayDWORD		m_arrayBond;				//	���带 ����.
		
		D3DXMATRIX * m_pD3DRotationMatrix1;		//	������ transform �� �̸� ���. �ε����� m_arrayBond �� ����.
		D3DXMATRIX * m_pD3DRotationMatrix2;
		D3DXVECTOR4	*	m_arrayBondPos1;
		D3DXVECTOR4	*	m_arrayBondPos2;
		D3DXVECTOR2	*	m_arrayBondRotation1;
		D3DXVECTOR2	*	m_arrayBondRotation2;
		FLOAT	*		m_arrayBondScale;

		//	�Ʒ� 3���� obsolete. ����...
		//	m_arrayResidue �� ��ġ
		CSTLArrayDWORD	m_arrayResidueIndex;			//	�ƹ̳�꺰�� m_arrayAtom �� �ε����� ����. ù��° ���� 0 �� ���Ե��� �ʴ´�.
		CSTLArrayDWORD	m_arrayResidueIndexBegin;		//	�ƹ̳�꺰�� m_arrayAtom �� ���� �ε����� ����.
		CSTLArrayDWORD	m_arrayResidueIndexEnd;			//	�ƹ̳�꺰�� m_arrayAtom �� �� �ε����� ����.
	END;

	//BEGIN("PDB Traversal")
	//	public:
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
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

	//		//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�� �ִ´�.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	CPDB *			m_pPDB;
};
typedef		std::vector < CModel * > CSTLArrayModel;

//	Protein �����̳�.
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
			void		SetUseStride(BOOL bUse) { m_bUseStride = bUse; }		//	2�������� ����־ ������ stride�� ���
			BOOL		IsExecuteStride() { return m_bUseStride; }
			CSTLArrayHelix		m_arrayHelix;		//	PDB ���� �︯�� ������ ���� ���� ����. (ü�ΰ� ���о��� ���� ��)
			CSTLArraySheet		m_arraySheet;		//	PDB ���� Sheet ����.
	END;

	BEGIN("Coordinate Section");		//	�ƹ̳�� �迭 - ����. ���׷� ����. ��
		public: 
			//	SCALE FILED in the pdb
			D3DXMATRIX		m_matScale;

		public:
			CSTLArrayArrayChain		m_arrayarrayChain;	//	3���� �迭. 

		private:
			CChain	*		m_pCurrentChain;
			BYTE			m_currentChainID;
	END;

	BEGIN("MODEL")
		public:
			//	MODEL �ʵ尡 ���°��� m_arrayModel �� size() �� 1 �̴�. -> ������ �ʴ´�.
			//	m_arrayChain[] �� �����ؼ� ���.
			CSTLArrayModel	m_arrayModel;
			long			m_iCurrentModel;	//	PDB ���� ���� Model #
			long			m_iArrayModel;			//	�迭�� ���� �� index.
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
			//	bDebug �� TRUE �̸� load �� ������ debug.pdb �� �Ȱ��� write �Ѵ�.
			HRESULT		Load(const TCHAR * filename, BOOL bDebug = FALSE);
			HRESULT		LoadPDB(const TCHAR * filename, BOOL bDebug = FALSE);
			HRESULT		LoadXML(const TCHAR * filename, BOOL bDebug = FALSE);

			//	HRESULT		DeleteUselessChain(CString & strChain);

			//	chain �� ���ؼ� ���ܻ�Ȳ ó��
		private:
			TCHAR	m_assignBlankChainID;
			BOOL	ValidateChain(CChain * m_pChain);
			void	MakeBioUnitTransform();
			void	ModifyPDBChain();
			void	SetAtomParentPointer();

		public:	//	��ǻ� private
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
		public:		//	m_serial�� �˻��� �������ϱ����� Hash table�� �����д�.
			//	CSTLMapAtom		m_hashMapAtom;
	END;
	//
	//
	//	
	BEGIN("������ ���� ã��")
		public:
			HRESULT AnalizeStructure();		//	2������ ������ stride ����.

			BOOL	StrideWrapper();
			HRESULT FindBond();
			HRESULT	FindCenterRadiusBB();
						
		private:
			//	�Ʒ� 4���� load �� �� �ڵ����� �����
			HRESULT SetAtomTypeAndResidueIndex();			//	type�� define �ϰ�, m_arrayResidue�� ����
			HRESULT	SetSequenceData();						//	sequence �� ����.

			HRESULT SetSecondaryStructureAtomAssign();		//	arrayAtom �� 2�������� ǥ��
			HRESULT SetChainSecondaryStructure();			//	arrayAtom �� ������ chain�� SS�� �籸��
			
	END;


	public:
	//	Init. Destroy
		CPDB();
		~CPDB();
		HRESULT Destroy();
		
		//	utility function...
		//	arg �� ������ �𵨾ȿ� ü���� ��ΰ��� �˾Ƴ���.
		long	GetNumChain(long iModel) { return m_arrayarrayChain[iModel].size(); }

		//	arg �� ������ �𵨰�, chain index �� ������ ü���� �����´�.
		CChain * GetChain(long iModel, long iChain) { return m_arrayarrayChain[iModel][iChain]; }

		//	atom pointer array �� HETATM pointer array �� ���´�.
		CSTLArrayAtom & GetAtomPtrArray(long iModel, long iChain) { return  (m_arrayarrayChain[iModel][iChain])->m_arrayAtom; }
		CSTLArrayAtom & GetHETATMPtrArray(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayHETATM; }

		//	��, ü�ξȿ� ������ ����� �˾Ƴ�.
		long	GetNumAtom(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayAtom.size(); }
		long	GetNumHETATM(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayHETATM.size(); }

		//	ü�ξȿ� residue �� ����� �˾Ƴ�.
		long	GetNumResidue(long iModel, long iChain) { return (m_arrayarrayChain[iModel][iChain])->m_arrayResidueIndex.size(); }

		//	2�� ����.
		CSTLArrayHelix & GetHelixArray() { return m_arrayHelix; }
		CSTLArraySheet & GetSheetArray() { return m_arraySheet; }

	//BEGIN("PDB Traversal")
	//	public:
	//		//	m_bSelect �� Traversal �ϸ鼭 Setting.
	//		void SetSelectChild(BOOL bSelect);

	//		//	traversal �ϸ鼭 ���õ� m_bSelect �� ã�Ƽ� selection container �� �ִ´�.
	//		//	���� select �Ǿ������� ���� �ִ´�. atom�� residue�� �ߺ��Ǿ� ��.
	//		void GetSelectNodeChild(CSTLArraySelection &selection);

	//		//	Selection list �� �־����� Index�� �����Ѵ�.
	//		void SetDisplayStyleChild(long index, BOOL bStyle);

	//		//	child atom�� ��´�.
	//		void GetChildAtoms(CSTLArrayAtom & atoms);
	//END;

	//	DECLARE_DYNAMIC(CPDB)
};

typedef		std::vector < CPDB * > CSTLArrayPDB;

typedef HRESULT (*PDBFunction)(CPDB * , long , CString &);

#endif

