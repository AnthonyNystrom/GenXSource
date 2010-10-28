// MatOnObjectDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "MatOnObjectDlg.h"

#include "..//Drawer.h"
#include "..//RayTracing//RTMaterials.h"
#include ".\matonobjectdlg.h"


// CMatOnObjectDlg dialog

IMPLEMENT_DYNAMIC(CMatOnObjectDlg, CDialog)
CMatOnObjectDlg::CMatOnObjectDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CMatOnObjectDlg::IDD, pParent)
	, m_mat_count(0)
	, m_object(NULL)
	, m_mult_texture(FALSE)
	, m_smooth_texture(FALSE)
	, m_map_type(0)
{
}

CMatOnObjectDlg::~CMatOnObjectDlg()
{
}

void  CMatOnObjectDlg::SetObject(sgC3DObject* Ob)
{
	ASSERT(Ob);
	m_object =Ob;
}

void CMatOnObjectDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_MAT_NAMES_LIST, m_all_mats_names);
	DDX_Text(pDX, IDC_MAT_COUNT, m_mat_count);
	DDX_Control(pDX, IDC_ARMAXCTRL, m_preview_material);
	DDX_Control(pDX, IDC_TEXTURE_PREVIEW, m_texture_preview);

	DDX_Control(pDX, IDC_ANG_SPIN, c_sprotx);
	DDX_Control(pDX, IDC_ANG_EDIT, c_rotx);
	DDX_Control(pDX, IDC_SH_V_SP, c_spshiftV);
	DDX_Control(pDX, IDC_SH_V_ED, c_shiftV);
	DDX_Control(pDX, IDC_SH_U_SP, c_spshiftU);
	DDX_Control(pDX, IDC_SH_U_ED, c_shiftU);
	DDX_Control(pDX, IDC_SC_V_SP, c_spscaleV);
	DDX_Control(pDX, IDC_SC_V_ED, c_scaleV);
	DDX_Control(pDX, IDC_SC_U_SP, c_spscaleU);
	DDX_Control(pDX, IDC_SC_U_ED, c_scaleU);

	DDX_Control(pDX, IDC_MIX_COLOR_COMBO, m_mix_color_combo);
	DDX_Check(pDX, IDC_MULT_TEXT_CHECK, m_mult_texture);
	DDX_Check(pDX, IDC_SMOOTH_TEXT_CHECK, m_smooth_texture);
	DDX_Radio(pDX, IDC_RADIO1, m_map_type);
}


BEGIN_MESSAGE_MAP(CMatOnObjectDlg, CDialog)
	ON_LBN_SELCHANGE(IDC_MAT_NAMES_LIST, OnLbnSelchangeMatNamesList)
	ON_WM_PAINT()
	ON_BN_CLICKED(IDC_TEX_ON_OBJ_BTN, OnBnClickedTexOnObjBtn)
	ON_BN_CLICKED(IDC_RADIO1, OnBnClickedRadio1)
	ON_BN_CLICKED(IDC_RADIO2, OnBnClickedRadio2)
	ON_BN_CLICKED(IDC_RADIO3, OnBnClickedRadio3)
END_MESSAGE_MAP()


// CMatOnObjectDlg message handlers

BOOL CMatOnObjectDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	ASSERT(m_object);

	if(Drawer::MatLoader.GetMainHeader())
	{

		m_mat_count = Drawer::MatLoader.GetMainHeader()->nTotalMat;

		if (m_mat_count<1)
			return TRUE;
		
		for (int i=0;i<m_mat_count;i++)
			m_all_mats_names.AddString(Drawer::MatLoader.GetMaterialByIndex(i)->szName);

		c_spscaleU.SetBuddy(&c_scaleU);
		c_spscaleU.SetRange(-10000000.0f,10000000.0f);
		c_spscaleU.SetDelta(0.1f);
		
		c_spscaleV.SetBuddy(&c_scaleV);
		c_spscaleV.SetRange(-10000000.0f,10000000.0f);
		c_spscaleV.SetDelta(0.1f);

		c_spshiftU.SetBuddy(&c_shiftU);
		c_spshiftU.SetRange(-10000000.0f,10000000.0f);
		c_spshiftU.SetDelta(1.f);

		c_spshiftV.SetBuddy(&c_shiftV);
		c_spshiftV.SetRange(-10000000.0f,10000000.0f);
		c_spshiftV.SetDelta(1.f);

		c_sprotx.SetBuddy(&c_rotx);
		c_sprotx.SetRange(-360.0f,360.0f);
		c_sprotx.SetDelta(1.f);

		m_mix_color_combo.ResetContent();
		CString lab;
		lab.LoadString(IDS_MODULATE_COL);
		m_mix_color_combo.AddString(lab);
		lab.LoadString(IDS_BLEND_COL);
		m_mix_color_combo.AddString(lab);
		lab.LoadString(IDS_REPLACE_COL);
		m_mix_color_combo.AddString(lab);
		
		const SG_MATERIAL* mmm = m_object->GetMaterial();
		if (mmm)
		{
			c_spscaleU.SetPos((float)mmm->TextureScaleU);
			c_spscaleV.SetPos((float)mmm->TextureScaleV);
			c_spshiftU.SetPos((float)mmm->TextureShiftU);
			c_spshiftV.SetPos((float)mmm->TextureShiftV);
			c_sprotx.SetPos((float)mmm->TextureAngle);
			m_mult_texture = (mmm->TextureMult)?TRUE:FALSE;
			m_smooth_texture = (mmm->TextureSmooth)?TRUE:FALSE;
			m_map_type = (int)(mmm->TextureUVType)-1;
			m_mix_color_combo.SetCurSel((int)(mmm->MixColorType)-1);
		}
		else
		{
			double optU,optV;
			m_object->CalculateOptimalUV(optU,optV);
			c_spscaleU.SetPos((float)optU);
			c_spscaleV.SetPos((float)optV);
			c_spshiftU.SetPos(0.0f);
			c_spshiftV.SetPos(0.0f);
			c_sprotx.SetPos(0.0f);
			m_mult_texture = TRUE;
			m_smooth_texture = TRUE;
			m_map_type = 0;
			m_mix_color_combo.SetCurSel(2);
		}

		UpdateData(FALSE);

		if (mmm)
		{
			int mI = mmm->MaterialIndex;
			ASSERT(mI>=0);
			if (mI>=m_mat_count)
				m_all_mats_names.SetCurSel(0);
			else
				m_all_mats_names.SetCurSel(mI);
		}
		else
			m_all_mats_names.SetCurSel(0);

		OnLbnSelchangeMatNamesList();
	}
	else
    {
		AfxMessageBox("Material object not loaded");
		EndDialog(IDCANCEL);
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CMatOnObjectDlg::OnLbnSelchangeMatNamesList()
{
	const MAT_ITEM* matrl = Drawer::MatLoader.GetMaterialByIndex(m_all_mats_names.GetCurSel());
	m_preview_material.SetAmbient(matrl->AmbientR,matrl->AmbientG,matrl->AmbientB);
	m_preview_material.SetDiffuse(matrl->DiffuseR,matrl->DiffuseG,matrl->DiffuseB);
	m_preview_material.SetEmission(matrl->EmissionR,matrl->EmissionG,matrl->EmissionB);
	m_preview_material.SetSpecular(matrl->SpecularR,matrl->SpecularG,matrl->SpecularB);
	m_preview_material.SetTransparent(matrl->Transparent);
	m_preview_material.SetShininess(matrl->Shininess);

	ShowHideControls();

	Invalidate(FALSE);
}

void  CMatOnObjectDlg::ShowHideControls()
{
	const MAT_ITEM* matrl = Drawer::MatLoader.GetMaterialByIndex(m_all_mats_names.GetCurSel());
	int sh = SW_SHOW;
	if (matrl->nIdxTexture)
		sh = SW_SHOW;

		GetDlgItem(IDC_STATIC_1)->ShowWindow(sh);
		GetDlgItem(IDC_STATIC_2)->ShowWindow(sh);
		GetDlgItem(IDC_STATIC_3)->ShowWindow(sh);
		GetDlgItem(IDC_STATIC_4)->ShowWindow(sh);
		GetDlgItem(IDC_STATIC_5)->ShowWindow(sh);
		GetDlgItem(IDC_STATIC_6)->ShowWindow(sh);
		GetDlgItem(IDC_STATIC_7)->ShowWindow(sh);
		GetDlgItem(IDC_SC_U_SP)->ShowWindow(sh);
		GetDlgItem(IDC_SC_V_SP)->ShowWindow(sh);
		GetDlgItem(IDC_SC_U_ED)->ShowWindow(sh);
		GetDlgItem(IDC_SC_V_ED)->ShowWindow(sh);
		GetDlgItem(IDC_SH_U_SP)->ShowWindow(sh);
		GetDlgItem(IDC_SH_V_SP)->ShowWindow(sh);
		GetDlgItem(IDC_SH_U_ED)->ShowWindow(sh);
		GetDlgItem(IDC_SH_V_ED)->ShowWindow(sh);
		GetDlgItem(IDC_ANG_SPIN)->ShowWindow(sh);
		GetDlgItem(IDC_ANG_EDIT)->ShowWindow(sh);
		GetDlgItem(IDC_RADIO1)->ShowWindow(sh);
		GetDlgItem(IDC_RADIO2)->ShowWindow(sh);
		GetDlgItem(IDC_RADIO3)->ShowWindow(sh);
		GetDlgItem(IDC_MODE_PIC)->ShowWindow(sh);
		GetDlgItem(IDC_TEX_ON_OBJ_BTN)->ShowWindow(sh);
		GetDlgItem(IDC_MIX_COLOR_COMBO)->ShowWindow(sh);
		GetDlgItem(IDC_TEX_ON_OBJ_BTN)->ShowWindow(sh);
		GetDlgItem(IDC_MULT_TEXT_CHECK)->ShowWindow(sh);
		GetDlgItem(IDC_SMOOTH_TEXT_CHECK)->ShowWindow(sh);
	

}
void CMatOnObjectDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	CDC* pDC = m_texture_preview.GetDC();
	CRect		rc;
	m_texture_preview.GetClientRect(&rc);

	int selMat = m_all_mats_names.GetCurSel();
	if ( Drawer::MatLoader.GetMainHeader())
	{
		ASSERT(selMat>=0 && selMat<Drawer::MatLoader.GetMainHeader()->nTotalMat);

		int matTex = Drawer::MatLoader.GetMaterialByIndex(selMat)->nIdxTexture;
		pDC->FillSolidRect(rc,GetSysColor(COLOR_BTNFACE));	
		if (matTex)
			Drawer::MatLoader.GetTextureByIndex(matTex-1)->image->Draw(pDC->m_hDC,rc);
	}

	m_texture_preview.ReleaseDC(pDC);
}

void CMatOnObjectDlg::OnOK()
{
	UpdateData();
	ASSERT(m_object);
	SG_MATERIAL objMat;
	objMat.MaterialIndex = m_all_mats_names.GetCurSel();
	objMat.MixColorType = (SG_MIX_COLOR_TYPE)(m_mix_color_combo.GetCurSel()+1);
	objMat.TextureAngle =  c_rotx.GetValueX();
	objMat.TextureMult = (m_mult_texture)?true:false;
	objMat.TextureScaleU = c_scaleU.GetValueX();
	objMat.TextureScaleV =  c_scaleV.GetValueX();
	objMat.TextureShiftU =  c_shiftU.GetValueX();
	objMat.TextureShiftV = c_shiftV.GetValueX();
	objMat.TextureSmooth = (m_smooth_texture)?true:false;
	objMat.TextureUVType = (SG_UV_TYPE)(m_map_type+1);
	m_object->SetMaterial(objMat);
	CDialog::OnOK();
}

void CMatOnObjectDlg::OnBnClickedTexOnObjBtn()
{
	double optU,optV;
	m_object->CalculateOptimalUV(optU,optV);
	c_spscaleU.SetPos((float)optU);
	c_spscaleV.SetPos((float)optV);
}

void CMatOnObjectDlg::OnBnClickedRadio1()
{
	// TODO: Add your control notification handler code here
}

void CMatOnObjectDlg::OnBnClickedRadio2()
{
	// TODO: Add your control notification handler code here
}

void CMatOnObjectDlg::OnBnClickedRadio3()
{
	// TODO: Add your control notification handler code here
}
