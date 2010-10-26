#pragma once

//
//	�ϳ��� PDB �� ���ؼ� Rendering ������Ʈ�� �����.
//	���콺 ������ �� local Transform �� ����.
//
#include "pdb.h"
#include "pdbInst.h"
#include "GraphicsObject.h"
#include "pick.h"
#include "ColorScheme.h"
#include "CoordinateAxis.h"

class	CProteinVistaRenderer;

//	class	CProteinSurface;
//	typedef std::vector <CProteinSurface *>		CSTLArrayProteinSurface;

//	class	CProteinSurfaceMSMS;
//	typedef std::vector <CProteinSurfaceMSMS *>		CSTLArrayProteinSurfaceMSMS;

class	CPDBRenderer;
typedef std::vector < CPDBRenderer * >	CSTLArrayPDBRenderer;

//
//	������� ���콺�� ���� �����̴� �ּ� ��ü.
//
class	CPDBRenderer:public CMoleculeRenderObject
{
private:	
	//	CPDB *					m_pPDB;				//	���÷��� �� PDB
	CPDBInst *				m_pPDBInst;				//	���÷��� �� PDB

public:
	//	�ӽ������� �ִ°�. ������.-> CSelectionDislay�� �Ű���
	//	enum	{ WIREFRAME, STICKS, SPACEFILL, BALLANDSTICK, RIBBON, SURFACE };
	public:

		//	pdbinst, chainInst, residueInst, atomInst�� ��� �ϳ��� ���õȰ��� ������ m_bIsSelectionExist �� TRUE.
		//	m_bIsSelectionExist �� fasle �̸�, child �߿��� ���õȰ��� �ϳ��� ����.
		BOOL	m_bIsSelectionExist;

	public:
		BOOL	m_bSelected;					//	���� pdb �� ���õǾ��°��� ǥ��. ���õ� pdb �� R,T �� ��밡����. 

		CPDBInst	*	GetPDBInst() { return m_pPDBInst; }
		BOOL	IsSelected() { return m_bSelected; }

		//	pdb �߿��� ���� ���õ� node �� �����ش�.
		void	GetSelectedObject(CSTLArraySelectionInst& selection){ if ( m_bIsSelectionExist == TRUE )	GetPDBInst()->GetSelectNodeChild(selection); }

	BEGIN("Ʈ������");
		public:
			D3DXMATRIXA16 *		GetWorldMatrix() { return &m_matWorld; }

			D3DXMATRIXA16		m_matWorld;

			D3DXVECTOR3         m_selectionCenterTransformed;			//	Center of bounding sphere of selected object : transformed �Ȱ���.

			//	PDB �� ���� center, radius, BB �� pdb���� ���
			D3DXVECTOR3			m_selectionCenter;						//	rotation center of selected object. transform ���� ���� ��ǥ��.
			D3DXVECTOR3			m_selectionMinMaxBB[2];								//	selection �Ȱ��� MinMax BB �� ��
			FLOAT				m_selectionRadius;

			BOOL				m_bSelectionRotCenter;					//	selection�� �κ��� rotation�� center�� �ϴ���, pdb�� center�� rotation�� center���ϴ��� flag
			void				GetCenterRadiusBB();					//	m_bSelectionRotCenter�� ����, model�� center �� selection�� center�� �����ش�.
			void				SetTransformCenter();

			void				CenterPDBRenderer();	//	���� PDBRenderer�� center �� �Űܳ��´�.

			void				UpdatePDBRendererCenter();

			CPDBRenderer *			m_pPDBRendererParentBioUnit;
			CSTLArrayPDBRenderer	m_arrayPDBRendererChildBioUnit;
			
			long				m_iBioUnit;				//	child �϶� child �� index
			BOOL				m_bAttatchBioUnit;		//	parent biomolecule�� ���ؼ� child�� �ڵ����� attatch ��Ű������ flag.
			D3DXVECTOR3			m_biounitCenter;		//	bioMolecule ���� parent�� ���ؼ� ��ü biomolecule�� center�� �־�д�.
			D3DXVECTOR3			m_bioUnitMinMaxBB[2];
			FLOAT				m_biounitRadius;
			D3DXMATRIXA16		m_matTransformBioUnit;
			void				AddChildPDBRendererBioUnit(CPDBRenderer * pPDBRenderer) { m_arrayPDBRendererChildBioUnit.push_back(pPDBRenderer); }
			void				SetBioUnitTransform(CPDBRenderer * pPDBRendererParent, long index, D3DXMATRIX &matTransform);
	END;

	BEGIN("Ʈ������");
		public:
			BOOL	m_bDrag;
			long	m_posOldX, m_posOldY;
			
		public:
			//	transform�� ���ؼ� ���� �߰��Ǵ� �͵�.
			D3DXMATRIXA16	m_matWorldMouseMoveRot;
			D3DXMATRIXA16	m_matWorldMouseMoveTrans;

			//	m_matWorldUserInput, m_matWorldUserInputOrig �� user mouse move�� ���ؼ� ������ transform
			D3DXMATRIXA16	m_matWorldUserInputOrig;
			D3DXMATRIXA16	m_matWorldUserInput;

			D3DXMATRIXA16	m_matPDBCenter;
			D3DXMATRIXA16	m_matPDBCenterInv;

			D3DXMATRIXA16	m_matWorldPrevious;

		public:
			LRESULT HandleMessages( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam );
	END;

	BEGIN("HETATM ������");
		//	CSTLArrayRenderHETATM				m_arrayRenderHETATM;
	END;
	
	BEGIN("DOT SURFACE ������");
		public:
			//	������ chain�� ���� surface ����Ÿ ����.
			//	CSTLArrayProteinSurface			m_arrayProteinSurface;
			//	CSTLArrayProteinSurfaceMSMS		m_arrayProteinSurfaceMSMS;
	END;

	BEGIN("Pick")
		public:
			D3DXVECTOR3 m_vPickRayDir;
			D3DXVECTOR3 m_vPickRayOrig;

			void	SetPickRay(POINT pt);
			void	Pick(CSTLArrayPickedAtomInst & pickAtomArray);
	END;
	
	//	���� ���� ��Ÿ���� index. ó���� 0���� ����. ���� model �� ���� m_pModel�� ������� ����.
	//	�� ���� ���Ǵ°� ���� �����ʿ�
	long		m_iDisplayModel;

public:

	CPDBRenderer();
	~CPDBRenderer();

	//	
	void Init(CProteinVistaRenderer * pProteinVistaRenderer, CPDB * pPDB );

    virtual HRESULT OneTimeSceneInit();
    virtual HRESULT InitDeviceObjects();
    virtual HRESULT RestoreDeviceObjects();
    virtual HRESULT FrameMove();
    virtual HRESULT Render();
    virtual HRESULT InvalidateDeviceObjects();
    virtual HRESULT DeleteDeviceObjects();
    virtual HRESULT FinalCleanup();

};


