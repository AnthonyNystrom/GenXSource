#pragma once

//
//	하나의 PDB 에 대해서 Rendering 오브젝트를 만든다.
//	마우스 움직임 과 local Transform 을 관리.
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
//	사용자의 마우스에 의해 움직이는 최소 객체.
//
class	CPDBRenderer:public CMoleculeRenderObject
{
private:	
	//	CPDB *					m_pPDB;				//	디스플레이 할 PDB
	CPDBInst *				m_pPDBInst;				//	디스플레이 할 PDB

public:
	//	임시적으로 있는것. 지워짐.-> CSelectionDislay로 옮겨짐
	//	enum	{ WIREFRAME, STICKS, SPACEFILL, BALLANDSTICK, RIBBON, SURFACE };
	public:

		//	pdbinst, chainInst, residueInst, atomInst중 어느 하나라도 선택된것이 있으면 m_bIsSelectionExist 가 TRUE.
		//	m_bIsSelectionExist 가 fasle 이면, child 중에서 선택된것이 하나도 없다.
		BOOL	m_bIsSelectionExist;

	public:
		BOOL	m_bSelected;					//	현재 pdb 가 선택되었는가를 표시. 선택된 pdb 만 R,T 이 사용가능함. 

		CPDBInst	*	GetPDBInst() { return m_pPDBInst; }
		BOOL	IsSelected() { return m_bSelected; }

		//	pdb 중에서 현재 선택된 node 를 돌려준다.
		void	GetSelectedObject(CSTLArraySelectionInst& selection){ if ( m_bIsSelectionExist == TRUE )	GetPDBInst()->GetSelectNodeChild(selection); }

	BEGIN("트랜스폼");
		public:
			D3DXMATRIXA16 *		GetWorldMatrix() { return &m_matWorld; }

			D3DXMATRIXA16		m_matWorld;

			D3DXVECTOR3         m_selectionCenterTransformed;			//	Center of bounding sphere of selected object : transformed 된것임.

			//	PDB 에 대한 center, radius, BB 는 pdb꺼를 사용
			D3DXVECTOR3			m_selectionCenter;						//	rotation center of selected object. transform 되지 않은 좌표임.
			D3DXVECTOR3			m_selectionMinMaxBB[2];								//	selection 된것의 MinMax BB 의 값
			FLOAT				m_selectionRadius;

			BOOL				m_bSelectionRotCenter;					//	selection한 부분을 rotation의 center로 하는지, pdb의 center를 rotation의 center로하는지 flag
			void				GetCenterRadiusBB();					//	m_bSelectionRotCenter에 따라서, model의 center 나 selection의 center를 돌려준다.
			void				SetTransformCenter();

			void				CenterPDBRenderer();	//	현재 PDBRenderer를 center 로 옮겨놓는다.

			void				UpdatePDBRendererCenter();

			CPDBRenderer *			m_pPDBRendererParentBioUnit;
			CSTLArrayPDBRenderer	m_arrayPDBRendererChildBioUnit;
			
			long				m_iBioUnit;				//	child 일때 child 의 index
			BOOL				m_bAttatchBioUnit;		//	parent biomolecule에 대해서 child를 자동으로 attatch 시키는지의 flag.
			D3DXVECTOR3			m_biounitCenter;		//	bioMolecule 에서 parent에 대해서 전체 biomolecule의 center를 넣어둔다.
			D3DXVECTOR3			m_bioUnitMinMaxBB[2];
			FLOAT				m_biounitRadius;
			D3DXMATRIXA16		m_matTransformBioUnit;
			void				AddChildPDBRendererBioUnit(CPDBRenderer * pPDBRenderer) { m_arrayPDBRendererChildBioUnit.push_back(pPDBRenderer); }
			void				SetBioUnitTransform(CPDBRenderer * pPDBRendererParent, long index, D3DXMATRIX &matTransform);
	END;

	BEGIN("트랜스폼");
		public:
			BOOL	m_bDrag;
			long	m_posOldX, m_posOldY;
			
		public:
			//	transform을 위해서 새로 추가되는 것들.
			D3DXMATRIXA16	m_matWorldMouseMoveRot;
			D3DXMATRIXA16	m_matWorldMouseMoveTrans;

			//	m_matWorldUserInput, m_matWorldUserInputOrig 은 user mouse move에 의해서 생성된 transform
			D3DXMATRIXA16	m_matWorldUserInputOrig;
			D3DXMATRIXA16	m_matWorldUserInput;

			D3DXMATRIXA16	m_matPDBCenter;
			D3DXMATRIXA16	m_matPDBCenterInv;

			D3DXMATRIXA16	m_matWorldPrevious;

		public:
			LRESULT HandleMessages( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam );
	END;

	BEGIN("HETATM 렌더링");
		//	CSTLArrayRenderHETATM				m_arrayRenderHETATM;
	END;
	
	BEGIN("DOT SURFACE 렌더링");
		public:
			//	각각의 chain에 대한 surface 데이타 저장.
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
	
	//	현재 모델을 나타내는 index. 처음에 0으로 가정. 현재 model 에 대한 m_pModel이 만들어져 사용됨.
	//	이 변수 사용되는것 전부 수정필요
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


