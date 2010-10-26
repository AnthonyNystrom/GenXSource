#pragma once
#include "pdb.h"
#include "pdbInst.h"
#include "DotNetInterface.h"

#include "Interface.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "ProteinVistaView.h"
#include "SelectionDisplay.h"
#include "MatrixMath.h"
#include "RenderProperty.h"
#include "ColorScheme.h"
#include "Utils.h"

using namespace System;
using namespace NuGenCbaseInterface;
using namespace System::Drawing;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace Microsoft::DirectX;
using namespace Microsoft::DirectX::Direct3D;

using namespace System::ComponentModel;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::IO;
using namespace System::Drawing::Imaging;
using namespace System::Windows::Forms::Design;
using namespace System::ComponentModel::Design;
using namespace System::Drawing::Design;

public ref class PIConverter : public ExpandableObjectConverter
{
public:
	virtual Object^ ConvertTo(ITypeDescriptorContext^ context, System::Globalization::CultureInfo^ culture, Object^ value, Type^ destType ) override
	{
		return ExpandableObjectConverter::ConvertTo(context,culture,value,destType);
	}
};
void ForceRenderScene();

public ref class FileSelectorTypeEditor :public UITypeEditor
{
public:
	virtual UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext^ context) override
	{
		if ( context == nullptr || context->Instance == nullptr )
			return UITypeEditor::GetEditStyle(context);
		return UITypeEditorEditStyle::Modal;
	}

	virtual Object^ EditValue(ITypeDescriptorContext^ context, System::IServiceProvider^ provider, Object^ value) override
	{
		IWindowsFormsEditorService^  editorService;

		if ( context == nullptr || context->Instance == nullptr || provider == nullptr )
			return value;
		try
		{
			// get the editor service, just like in windows forms
			editorService = (IWindowsFormsEditorService^)
				provider->GetService(IWindowsFormsEditorService::typeid);

			OpenFileDialog^ dlg = gcnew OpenFileDialog();
			dlg->Filter = "All Files (*.*)|*.*";
			dlg->CheckFileExists = true;

			String^ filename = (String^)value;
			if ( !File::Exists(filename) )
				filename = nullptr;
			dlg->FileName = filename;

			DialogResult^ res = dlg->ShowDialog();
			if ( res == DialogResult::OK )
		 {
			 filename = dlg->FileName;
		 }
			return filename;

		} finally
		{
			editorService = nullptr;
		}
	}
};
[TypeConverter(PIConverter::typeid)]
public ref class Annotation 
{
private:
	CSelectionDisplay * m_pSelectionDisplay;
	int					m_iAnnotation;	
public:
	Annotation( CSelectionDisplay * pSelectionDisplay , int index )
	{
		m_pSelectionDisplay = pSelectionDisplay; 
		m_iAnnotation = index; 
	}

public:
	property bool Show 
	{ 
		bool get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return Convert::ToBoolean( pPropertyCommon->m_bAnnotation[m_iAnnotation] );
				}
			}
			return false;
		}
		void set(bool _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_bAnnotation[m_iAnnotation]=_value?TRUE:FALSE;
					//pPropertyCommon->m_pItemShowAnnotation[m_iAnnotation]->SetBool(_value);
					//pPropertyCommon->m_pItemShowAnnotation[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemShowAnnotation[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_SHOW);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_FONT);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_SHOW);
				}
				ForceRenderScene();
			}

		}
	}
	property String ^	Text 
	{ 
		String ^ get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return gcnew String(pPropertyCommon->m_strAnnotation[m_iAnnotation]);
				}
			}

			return nullptr;
		}
		void set(String ^ _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_strAnnotation[m_iAnnotation]= MStrToCString(_value);
					//pPropertyCommon->m_pItemstrAnnotation[m_iAnnotation]->SetValue(_value);
					//pPropertyCommon->m_pItemstrAnnotation[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemstrAnnotation[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_SHOW);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_FONT);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_SHOW);
				}
				ForceRenderScene();
			}

		}
	}
	property	String  ^		FontName 
	{ 
		String ^ get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return gcnew String(pPropertyCommon->m_logFont[m_iAnnotation].lfFaceName);
				}
			}

			return nullptr;
		}
		void set(String ^ _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					LOGFONT lf;
					//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->GetFont(&lf);

					CString strFontFaceName; 
					strFontFaceName = MStrToCString(_value);

					_tcscpy( lf.lfFaceName, strFontFaceName);
					pPropertyCommon->m_logFont[m_iAnnotation] = lf;
					//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->SetFont(lf);
					//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT);
				}

				ForceRenderScene();
			}

		}
	}
	property int  FontHeight 
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					HDC hDC = CreateCompatibleDC( NULL );
					int lFontSize = -((pPropertyCommon->m_logFont[m_iAnnotation].lfHeight * 72) / GetDeviceCaps(hDC, LOGPIXELSY));
					DeleteDC(hDC);

					return lFontSize;
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					HDC hDC = CreateCompatibleDC( NULL );
					int heightFont = -MulDiv( _value, (INT)(GetDeviceCaps(hDC, LOGPIXELSY)), 72 );
					DeleteDC(hDC);

					//LOGFONT lf;
					pPropertyCommon->m_logFont[m_iAnnotation].lfHeight =heightFont;
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT);
				}

				ForceRenderScene();
			}
		}
	}


	property IAnnotation::IDisplayMethod		DisplayMethod 
	{ 
		IAnnotation::IDisplayMethod get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return static_cast<IAnnotation::IDisplayMethod>(pPropertyCommon->m_enumTextDisplayTechnique[m_iAnnotation]);
				}
			}

			return IAnnotation::IDisplayMethod::EnableZBuffer;
		}
		void set(IAnnotation::IDisplayMethod _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_enumTextDisplayTechnique[m_iAnnotation] =(int)_value;
				}

				ForceRenderScene();
			}

		}
	}

	property IAnnotation::ITextType	TextType 
	{ 
		IAnnotation::ITextType get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return static_cast<IAnnotation::ITextType>(pPropertyCommon->m_enumAnnotatonType[m_iAnnotation]);
				}
			}

			return IAnnotation::ITextType::ThreeLetter;
		}
		void set(IAnnotation::ITextType _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_enumAnnotatonType[m_iAnnotation]=(int)_value;
					//pPropertyCommon->m_pItemenumAnnotationType[m_iAnnotation]->SetEnum(Convert::ToUInt32(_value));
					//pPropertyCommon->m_pItemenumAnnotationType[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemenumAnnotationType[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					//m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_DESCRIPTION);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_TYPE);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_TYPE);
				}
				ForceRenderScene();
			}

		}
	}

	property IAnnotation::IColorScheme ColorScheme 
	{ 
		IAnnotation::IColorScheme get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return static_cast<IAnnotation::IColorScheme>(pPropertyCommon->m_enumAnnotationColorScheme[m_iAnnotation]);
				}
			}

			return IAnnotation::IColorScheme::FollowAtom;
		}
		void set(IAnnotation::IColorScheme _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_enumAnnotationColorScheme[m_iAnnotation] = (int)_value;
				}
				ForceRenderScene();
			}

		}
	}

	property System::Drawing::Color		Color
	{
		System::Drawing::Color get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return System::Drawing::Color::FromArgb(
						GetRValue(pPropertyCommon->m_annotationColor[m_iAnnotation]),
						GetGValue(pPropertyCommon->m_annotationColor[m_iAnnotation]),
						GetGValue(pPropertyCommon->m_annotationColor[m_iAnnotation])
						);
				}
			}

			return System::Drawing::Color::White;
		}
		void set(System::Drawing::Color _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationColor[m_iAnnotation]=ManagedColor2COLORREF(_value);
					//pPropertyCommon->m_pItemannotationColor[m_iAnnotation]->SetColor(ManagedColor2COLORREF(_value));
					//pPropertyCommon->m_pItemannotationColor[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemannotationColor[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_COLOR);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_COLOR);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_COLOR);
				}
				ForceRenderScene();
			}

		}
	}
	property int RelativeXPos
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationXPos[m_iAnnotation];
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationXPos[m_iAnnotation] =_value;
					if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
					{
						m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_POS_X);
					}
					if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
					{
						m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_POS_X);
					}
					if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
					{
						m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_POS_X);
					}
					ForceRenderScene();
				}
			}

		}
	}
	property int  RelativeYPos 
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationYPos[m_iAnnotation];
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationYPos[m_iAnnotation]=_value;
					//pPropertyCommon->m_pItemAnnotationYPos[m_iAnnotation]->SetNumber(_value);
					//pPropertyCommon->m_pItemAnnotationYPos[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationYPos[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_POS_Y);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_POS_Y);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_POS_Y);
				}
				ForceRenderScene();
			}
		}
	}
	property int	 RelativeZPos 
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationZPos[m_iAnnotation];		
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationZPos[m_iAnnotation]=_value;
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_POS_Z);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_POS_Z);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_POS_Z);
				}
				ForceRenderScene();
			}

		}
	}
	property int	 TextXPos 
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationTextXPos[m_iAnnotation];				
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationTextXPos[m_iAnnotation]=_value;
					//pPropertyCommon->m_pItemAnnotationTextXPos[m_iAnnotation]->SetNumber(_value);
					//pPropertyCommon->m_pItemAnnotationTextXPos[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationTextXPos[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_TEXT_POS_X);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_TEXT_POS_X);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_TEXT_POS_X);
				}
				ForceRenderScene();
			}

		}
	}
	property int	 TextYPos
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationTextYPos[m_iAnnotation];						
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;

			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationTextYPos[m_iAnnotation] =_value;
					//pPropertyCommon->m_pItemAnnotationTextYPos[m_iAnnotation]->SetNumber(_value);
					//pPropertyCommon->m_pItemAnnotationTextYPos[m_iAnnotation]->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationTextYPos[m_iAnnotation]) );
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_VP)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_TEXT_POS_Y);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_ATOM)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_TEXT_POS_Y);
				}
				if(m_iAnnotation==(int)CSelectionDisplay::ANNOTATION_RESIDUE)
				{
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_TEXT_POS_Y);
				}
				ForceRenderScene();
			}

		}
	}
	property int DepthBias
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationDepthBias[m_iAnnotation];						
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationDepthBias[m_iAnnotation]=_value;
				}
				ForceRenderScene();
			}

		}
	}
	property int Transparency 
	{ 
		int get()
		{
			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_annotationTransparency[m_iAnnotation];						
				}
			}

			return 0;
		}
		void set(int _value)
		{
			if(_value <0) _value=0;
			if(_value>100) _value=100;

			if ( m_pSelectionDisplay )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_annotationTransparency[m_iAnnotation]=_value;
				}
				ForceRenderScene();
			}
		}
	}
};

public enum class VisualMode{ Wireframe,Stick,SpaceFill,BallnStick,Ribbon,Surface,NGRealistic};
 

[TypeConverter(PIConverter::typeid)]
public ref class CTexture
{
private:
	CSelectionDisplay * m_pSelectionDisplay;
public :
	CTexture(CSelectionDisplay * pSelectionDisplay)
	{
		m_pSelectionDisplay = pSelectionDisplay;
	}
public:
	property bool ShowTexture 
	{
		bool get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bTextureHelix);
			}

			return true;
		}
		void set(bool value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_bTextureHelix = value? TRUE:FALSE;// Convert::ToBoolean();
			}
			m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_COIL_SHOW_ON_SHEET);
			ForceRenderScene();
		}
	}


	[Editor(FileSelectorTypeEditor::typeid,System::Drawing::Design::UITypeEditor::typeid)]
	property String ^ Filename 
	{
		String ^ get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return gcnew String (pProperty->m_strTextureFilenameHelix);
			}

			return nullptr;
		}
		void set(String ^ value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_strTextureFilenameHelix = MStrToCString(value);
				//m_pSelectionDisplay->SetPropertyChanged(XTP_PGN_ITEMVALUE_CHANGED,pProperty->m_strTextureFilenameHelix);
				//pProperty->m_pTextureFilenameHelix->SetValue( _value );
				//pProperty->m_pTextureFilenameHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureFilenameHelix) );

				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_COIL_TEXTURE_FILENAME);
				ForceRenderScene();
			}
		}
	}
	property	int	 CoordU 
	{
		int get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return pProperty->m_textureCoordUHelix;
			}

			return 0;
		}
		void set(int value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_textureCoordUHelix = value;
				// m_pSelectionDisplay->SetPropertyChanged(XTP_PGN_ITEMVALUE_CHANGED);
				//pProperty->m_pTextureCoordUHelix->SetNumber ( _value );
				//pProperty->m_pTextureCoordUHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordUHelix) );
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_SHEET_TEXTURE_COORD_U);
				ForceRenderScene();
			}
		}
	}
	property	int	 CoordV 
	{
		int get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return pProperty->m_textureCoordVHelix;
			}

			return 0;
		}
		void set(int value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_textureCoordVHelix =value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_SHEET_TEXTURE_COORD_V);
				ForceRenderScene();
			}
		}
	}			
};
////////////////////////////////////////////////
[TypeConverter(PIConverter::typeid)]
public ref class Clipping
{
private:
	CSelectionDisplay * m_pSelectionDisplay;
	long index ;
public :
	Clipping(CSelectionDisplay * pSelectionDisplay,long index)
	{
		m_pSelectionDisplay = pSelectionDisplay;
		this->index = index;
	}
public:
	[Category("Clipping")]
	property bool	Enable 
	{
		virtual bool get()
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return Convert::ToBoolean(pPropertyCommon->m_bClipping1);
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
					return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bClipping0);
			}

			return true;
		}
		virtual void set(bool enable)
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_bClipping1 = enable?TRUE:FALSE;
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
				{
					pProteinVistaRenderer->m_pPropertyScene->m_bClipping0 = enable?TRUE:FALSE;
					GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_ENABLE_CLIPPING0,"",NULL);
				}
				ForceRenderScene(); 
			}

		}
	}

	[Category("Clipping")]
	property bool	Show
	{
		virtual bool get()
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return Convert::ToBoolean(pPropertyCommon->m_bShowClipPlane1);
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
					return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bShowClipPlane0);
			}

			return true;
		}
		virtual void set(bool show)
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_bShowClipPlane1 = show?TRUE:FALSE;

				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
				{
					pProteinVistaRenderer->m_pPropertyScene->m_bShowClipPlane0= show?TRUE:FALSE;
					GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_CLIPPING0_SHOW,"",NULL);
				}
			}

			ForceRenderScene(); 
		}
	}
	[Category("Clipping")]
	property System::Drawing::Color	Color 
	{ 
		virtual System::Drawing::Color get()
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return System::Drawing::Color::FromArgb(
						GetRValue(pPropertyCommon->m_clipPlaneColor1),
						GetGValue(pPropertyCommon->m_clipPlaneColor1),
						GetGValue(pPropertyCommon->m_clipPlaneColor1)
						);
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
					return System::Drawing::Color::FromArgb(
							GetRValue(pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneColor0),
							GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneColor0),
							GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneColor0)
					);
			}

			return System::Drawing::Color::Aqua;
		}
		virtual void set(System::Drawing::Color color)
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_clipPlaneColor1=ManagedColor2COLORREF(color);

				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
				{
					pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneColor0=ManagedColor2COLORREF(color);
					GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_CLIPPING0_COLOR,"",NULL);
				}
			}
			ForceRenderScene();
		}
	}
	[Category("Clipping")]
	property int	Transparency 
	{ 
		virtual int get()
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return pPropertyCommon->m_clipPlaneTransparency1;
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
					return pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneTransparency0;
			}

			return 0;
		}
		virtual void set(int transparency)
		{
			if(transparency <0) transparency=0;
			if(transparency>100) transparency=100;
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_clipPlaneTransparency1 = transparency;

				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
				{
					pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneTransparency0 = transparency;
					GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_CLIPPING0_TRANSPARENCY,"",NULL);
				}
			}

			ForceRenderScene(); 
		}
	}
	[Category("Clipping")]
	property bool	Direction 
	{ 
		virtual bool get()
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					return Convert::ToBoolean(pPropertyCommon->m_bClipDirection1);
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
					return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bClipDirection0);
			}
			return true;
		}
		virtual void set(bool direction)
		{
			if ( m_pSelectionDisplay != NULL )
			{
				CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
				if ( pPropertyCommon )
				{
					pPropertyCommon->m_bClipDirection1 = direction?TRUE:FALSE;
				}
			}
			else
			{
				CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
				if ( pProteinVistaRenderer )
				{
					pProteinVistaRenderer->m_pPropertyScene->m_bClipDirection0 = direction?TRUE:FALSE;
					GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_CLIPPING0_DIRECTION,"",NULL);
				}
			}
			ForceRenderScene(); 
		}
	}
	[Category("Clipping")]
	property String ^ Equation 
	{ 
		virtual String ^ get();
		virtual void set(String ^ equ);
	}

};

[Serializable]
public ref class CPIProperty  
{
protected:
	CSelectionDisplay * m_pSelectionDisplay;
public:
	virtual void Init() 
	{
	}
	virtual CPIProperty^ GetInstance()
	{
		return this;
	}
	CPIProperty()
	{
	}
	CPIProperty( CSelectionDisplay * pSelectionDisplay ) 
	{
		this->m_pSelectionDisplay = pSelectionDisplay; 
	}
};
[Serializable]
public ref class CPICommonProperty :public CPIProperty
{
protected:
	Annotation^ m_annotationVP;
	Annotation^ m_annotationAtom;
	Annotation^ m_annotationResidue;

	Clipping^ m_clipping0;
	Clipping^ m_clipping1;
public:
	[Category("Common Option"),Browsable(true)]
	property Annotation ^ DisplayVPAnnotation
	{  
		virtual Annotation ^ get() 
		{
			return this->m_annotationVP;
		} 
		virtual void set(Annotation ^ value) 
		{
			m_annotationVP = value;
		} 
	}

	[Category("Common Option")]
	property	Annotation ^ DisplayAnnotationAtom
	{
		virtual Annotation ^ get()
		{ 
			return this->m_annotationAtom;
		} 
		virtual void set(Annotation ^ value) 
		{
			m_annotationAtom = value;
		} 
	}

	[Category("Common Option")]
	property Annotation^ DisplayResidueName
	{ 
		virtual Annotation^ get() 
		{
			return this->m_annotationResidue;
		} 
		virtual void set(Annotation ^ value) 
		{
			m_annotationResidue = value;
		} 
	}
public:
 	[Category("Common Option")]
	property Clipping^ Clipping0
	{ 
		virtual Clipping^ get() 
		{
			return this->m_clipping0;
		} 
		virtual void set(Clipping ^ value) 
		{
			m_clipping0 = value;
		} 
	}
	[Category("Common Option")]
	property Clipping^ Clipping1
	{ 
		virtual Clipping^ get() 
		{
			return this->m_clipping1;
		} 
		virtual void set(Clipping ^ value) 
		{
			m_clipping1 = value;
		} 
	}
public:
	CPICommonProperty()
	{
	}
	CPICommonProperty( CSelectionDisplay * pSelectionDisplay ) 
	{
		m_pSelectionDisplay = pSelectionDisplay; 
		this->Init();
	}
	virtual void Init() override
	{	
		m_annotationVP = gcnew  Annotation(m_pSelectionDisplay, CSelectionDisplay::ANNOTATION_VP );
		m_annotationAtom = gcnew Annotation(m_pSelectionDisplay, CSelectionDisplay::ANNOTATION_ATOM );
		m_annotationResidue = gcnew Annotation(m_pSelectionDisplay, CSelectionDisplay::ANNOTATION_RESIDUE );

		m_clipping0 = gcnew  Clipping(m_pSelectionDisplay,0);
		m_clipping1 = gcnew Clipping(m_pSelectionDisplay,1);

	}
	virtual CPIProperty^ GetInstance() override{return this;};

	[Category("Common Option")]
	property bool Show 
	{
		virtual bool get()
		{
			if ( this->m_pSelectionDisplay )
			{
				return  Convert::ToBoolean(this->m_pSelectionDisplay->m_bShow);
			}
			return true;

		}
		virtual void set(bool value)
		{
			if ( this->m_pSelectionDisplay )
			{
				this->m_pSelectionDisplay->m_bShow=value ?TRUE:FALSE;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_SHOW); 
				ForceRenderScene();
			}
		}
	}

	
	[Category("Common Option")]
	property VisualMode VisualizationMode 
	{
		virtual VisualMode get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return (VisualMode)pPropertyCommon->m_enumDisplayMode;;
			}
			return VisualMode::Ribbon;
		}
		virtual void set(VisualMode value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_enumDisplayMode = (int)value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_DISPLAY_MODE);  
				ForceRenderScene();
			}
		}
	}
	[Category("Common Option")]
	property String ^ VPName 
	{ 
		virtual String ^ get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return gcnew String(pPropertyCommon->m_strSelectionName);
			}

			return "";
		}
		virtual void set(String ^ value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_strSelectionName =MStrToCString(value);

				ForceRenderScene();
				//pPropertyCommon->m_pItempSelectionName->SetValue(name);
				//pPropertyCommon->m_pItempSelectionName->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempSelectionName) );
			}

		}
	}
	[Category("Common Option")]
	property bool DisplaySideChain 
	{
		virtual bool get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return Convert::ToBoolean(pPropertyCommon->m_bDisplaySideChain);
			}

			return true;
		}
		virtual void set(bool value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_bDisplaySideChain =value?TRUE:FALSE;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_DISPLAY_SIDECHAIN);
				ForceRenderScene();
				//pPropertyCommon->m_pItempDisplaySideChain->SetBool(display);
				//pPropertyCommon->m_pItempDisplaySideChain->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempDisplaySideChain) );
			}

		}
	}


	[Category("Common Option")]
	property System::Drawing::Color BackGround 
	{
		virtual System::Drawing::Color get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor));
			}
			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor =ManagedColor2COLORREF(value);
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_BACKGROUND_COLOR,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Common Option")]
	property IProperty::IColorScheme ColorScheme 
	{ 
		virtual IProperty::IColorScheme get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return static_cast<IProperty::IColorScheme>(pPropertyCommon->m_enumColorScheme);
			}

			return IProperty::IColorScheme::CPK;
		}
		virtual void set(IProperty::IColorScheme colorScheme)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_enumColorScheme = (int)colorScheme;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_COLOR_SCHEME);
				ForceRenderScene();
				//pPropertyCommon->m_pItemColorScheme->SetEnum(Convert::ToInt32(colorScheme));
				//pPropertyCommon->m_pItemColorScheme->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemColorScheme) );
			}

		}
	}
	[Category("Common Option")]
	property List<System::Drawing::Color> ^ CustomizeColors 
	{
		virtual List<System::Drawing::Color> ^ get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				int iScheme = pPropertyCommon->m_enumColorScheme;
				CArrayColorRow & listColorRow = pPropertyCommon->m_arrayColorScheme[iScheme];

				List<System::Drawing::Color> ^ listColor = gcnew List<System::Drawing::Color>(listColorRow.size());

				for ( int i = 0 ; i < listColorRow.size() ; i++ )
				{
					//	TODO:
					listColor->Add( System::Drawing::Color::FromArgb(
						GetRValue(listColorRow[i]->m_color),
						GetGValue(listColorRow[i]->m_color),
						GetGValue(listColorRow[i]->m_color)	) );
				}

				return listColor;
			}
			return nullptr;
		}
		virtual void set(List<System::Drawing::Color> ^ listColor)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				int iScheme = pPropertyCommon->m_enumColorScheme;
				CArrayColorRow & arrayColorRow = pPropertyCommon->m_arrayColorScheme[iScheme];

				if ( iScheme == COLOR_SCHEME_OCCUPANCY || iScheme == COLOR_SCHEME_TEMPARATURE || iScheme == COLOR_SCHEME_PROGRESSIVE || iScheme == COLOR_SCHEME_HYDROPATHY )
				{	//	변동길이. arrayColor 대로 추가한다.
					for ( int i = 0 ; i < arrayColorRow.size() ; i++ )
					{
						SAFE_DELETE(arrayColorRow[i]);
					}

					arrayColorRow.clear();
					arrayColorRow.reserve(listColor->Count);

					for ( int i = 0 ; i < listColor->Count ; i++ )
					{
						CString strColorName;
						strColorName.Format("Color %d", i+1 );
						D3DXCOLOR diffuse = D3DCOLOR_ARGB(0, listColor[i].R, listColor[i].G, listColor[i].B);

						CColorRow * pColorRow = new CColorRow(strColorName, diffuse );
						arrayColorRow.push_back( pColorRow );
					}
				}
				else
				{
					//	고정길이 overwrite 하고, 남거나 모자르는것 무시.
					for ( int i = 0; i < listColor->Count ; i++ )
					{
						if ( i >= arrayColorRow.size() ) 
							break;

						arrayColorRow[i]->m_color = D3DCOLOR_ARGB(0, listColor[i].R, listColor[i].G, listColor[i].B);
					}
				}

				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				m_pSelectionDisplay->UpdateAnnotation();
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_COLOR_SCHEME_CUSTOMIZE); 

			}
		}
	}

	[Category("Common Option")]
	property IProperty::IGeometryQuality	GeometryQuality 
	{ 
		virtual IProperty::IGeometryQuality get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return static_cast<IProperty::IGeometryQuality>(pPropertyCommon->m_modelQuality);
			}

			return IProperty::IGeometryQuality::High;
		}
		virtual void set(IProperty::IGeometryQuality value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_modelQuality =(long)value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_MODEL_QUALITY);  			 
			}
			ForceRenderScene(); 

		}
	}
	[Category("Common Option")]
	property IProperty::IShaderQuality ShaderQuality 
	{
		virtual IProperty::IShaderQuality get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return static_cast<IProperty::IShaderQuality>(pPropertyCommon->m_shaderQuality);
			}
			return IProperty::IShaderQuality::High;
		}
		virtual void set(IProperty::IShaderQuality value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_shaderQuality =(int) value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_SHADER_QUALITY,"",NULL);
				ForceRenderScene();
			}

		}
	}
	[Category("Common Option")]
	property bool ShowSelectionMark
	{
		virtual bool get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return Convert::ToBoolean ( pPropertyCommon->m_bShowSelectionMark ) ;
			}

			return true;
		}
		virtual void set(bool value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_bShowSelectionMark =value?TRUE:FALSE;
				ForceRenderScene();
			}

		}
	}

	[Category("Common Option")]
	property	System::Drawing::Color IndicateSelectionMarkColor
	{
		virtual System::Drawing::Color get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pPropertyCommon->m_indicateColor),
					GetGValue(pPropertyCommon->m_indicateColor),
					GetGValue(pPropertyCommon->m_indicateColor)
					);
			}

			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_indicateColor =ManagedColor2COLORREF(value); 
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_INDICATE_SELECTION_COLOR);
				ForceRenderScene();
				//pPropertyCommon->m_pItemindicateColor->SetColor(ManagedColor2COLORREF(_value));
				//pPropertyCommon->m_pItemindicateColor->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemindicateColor) );
			}

		}
	}

	[Category("Common Option")]
	property	bool ShowIndicateSelectionMark
	{
		virtual bool get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return Convert::ToBoolean(pPropertyCommon->m_bIndicate);
			}
			return true;
		}
		virtual void set(bool value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
		 {
			 pPropertyCommon->m_bIndicate = value?TRUE:FALSE;
			 m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_INDICATE_SELECTION);
			 ForceRenderScene();
			 //pPropertyCommon->m_pItempItemIndicate->SetBool(value);
			 //pPropertyCommon->m_pItempItemIndicate->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemIndicate) );
		 }

		}
	}

	[Category("Common Option")]
	property int IntensityAmbient 
	{ 
		virtual int get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return pPropertyCommon->m_intensityAmbient;
			}
			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;

			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_intensityAmbient =value;
				ForceRenderScene(); 
				//pPropertyCommon->m_pItemIntensityAmbient->SetNumber(_value);
				//pPropertyCommon->m_pItemIntensityAmbient->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemIntensityAmbient));
			}
		}
	}
	[Category("Common Option")]
	property int DiffuseIntensity
	{  
		virtual int get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return pPropertyCommon->m_intensiryDiffuse;
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_intensiryDiffuse = value;
				ForceRenderScene(); 
				//pPropertyCommon->m_pItemIntensityDiffuse->SetNumber(_value);
				//pPropertyCommon->m_pItemIntensityDiffuse->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemIntensityDiffuse));
			}

		}
	}
	[Category("Common Option")]
	property int SpecularIntensity
	{ 
		virtual int get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return pPropertyCommon->m_intensitySpecular;
			}

			return 0;
		}
		virtual void set(int value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_intensitySpecular = value;
				ForceRenderScene();
				//pPropertyCommon->m_pItemIntensitySpecular->SetNumber(_value);
				//pPropertyCommon->m_pItemIntensitySpecular->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemIntensitySpecular));
			}

		}
	}

	[Category("Common Option")]
	property	bool  DisplayAxis 
	{
		virtual bool get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return  Convert::ToBoolean(pPropertyCommon->m_bDisplayAxisLocalCoord);
			}
			return TRUE;
		}
		virtual void set(bool value)
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_bDisplayAxisLocalCoord= value?TRUE:FALSE;
				ForceRenderScene();
			}
		}
	}
	[Category("Common Option")]
	property	int		AxisSize 
	{
		virtual int get()
		{
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				return pPropertyCommon->m_axisScaleLocalCoord;
			}
			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
			if ( pPropertyCommon )
			{
				pPropertyCommon->m_axisScaleLocalCoord= value?TRUE:FALSE;
				ForceRenderScene();
			}
		}
	}

	[Category("Common Option")]
	property	bool  ShowTorch  
	{
		virtual bool get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bLight1Show) &&
					   Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bLight2Show);
			}
			return true;
			 
		}
		virtual void set(bool light)
		{
		    CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bLight1Show = light?TRUE:FALSE;
				pProteinVistaRenderer->m_pPropertyScene->m_bLight2Show = light?TRUE:FALSE;
			    GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_ENABLE,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	/*[Category("Common Option")]
	property	bool  ShowLog 
	{
		virtual bool get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_bShowLog);
			}
			return true;

		}
		virtual void set(bool value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_bShowLog = value?TRUE:FALSE; 
				ForceRenderScene(); 
			}
		}
	}*/
};

//////////////////////////////////////////////
[Serializable]
public ref class CPIPropertyScene: public CPIProperty 
{
private :  Clipping^ m_ClippingPanel;
public:
	[Category("Rendering Option"),DisplayName("Clipping Panel")]
	property	Clipping^  ClippingPanel 
	{
		virtual Clipping^ get() 
		{
			 return this->m_ClippingPanel;
		}
		virtual void set(Clipping^ value) 
		{
			this->m_ClippingPanel = value; 
		}
	}
public:
	CPIPropertyScene(CSelectionDisplay * pSelectionDisplay)
	{
		m_ClippingPanel = gcnew Clipping(NULL, 0);

		m_arrayLights = gcnew array <CDotNetLight ^> (2);

		CDotNetLight ^ light1 = gcnew CDotNetLight(0);
		light1->Init();
		m_arrayLights[0] = light1;

		CDotNetLight ^ light2 = gcnew CDotNetLight(1);
		light2->Init();
		m_arrayLights[1] = light2;

		this->Init();
	}


	virtual CPIProperty^ GetInstance() override{return this;};


	[Category("Rendering Option")]
	property	bool  DisplayAxis 
	{
		virtual bool get() 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bDisplayAxis);
			}
			return true;

		}
		virtual void set(bool value) 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bDisplayAxis = value?TRUE:FALSE;
				//
				ForceRenderScene(); 
			}

		}
	}
	[Category("Rendering Option")]
	property	int		AxisSize 
	{
		virtual int get() 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_pPropertyScene->m_axisScale;
			}
			return 50;
		}
		virtual void set(int value) 
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_axisScale = value;

				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property System::Drawing::Color BackgroundColor 
	{
		virtual System::Drawing::Color get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor)	);
			}
			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor =ManagedColor2COLORREF(value);
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_BACKGROUND_COLOR,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property System::Drawing::Color SelectionColor 
	{
		virtual System::Drawing::Color get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProteinVistaRenderer->m_pPropertyScene->m_selectionColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_selectionColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_selectionColor)

					);
			}
			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_selectionColor =ManagedColor2COLORREF(value);
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_SELECTION_COLOR,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	bool  ShowBackgroundTexture 
	{
		virtual bool get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bUseBackgroundTexture);
			}
			return true;
		}
		virtual void set(bool value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bUseBackgroundTexture =value?TRUE:FALSE;
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	[Editor(FileSelectorTypeEditor::typeid,System::Drawing::Design::UITypeEditor::typeid)]
	property	String^ BackgroundTextureFilename 
	{
		virtual String ^ get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return gcnew String (pProteinVistaRenderer->m_pPropertyScene->m_strBackgroundTextureFilename);
			}
			return nullptr;
		}
		virtual void set(String ^ value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_strBackgroundTextureFilename =MStrToCString(value);
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_BACKGROUND_FILENAME,"",NULL);
				ForceRenderScene(); 
			}
		}
	}


	[Category("Rendering Option")]
	property	IPropertyScene::IShaderQuality	ShaderQuality 
	{ 
		virtual IPropertyScene::IShaderQuality get() 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return static_cast<IPropertyScene::IShaderQuality>(pProteinVistaRenderer->m_pPropertyScene->m_shaderQuality);
			}
			return IPropertyScene::IShaderQuality::Low;
		}
		virtual void set(IPropertyScene::IShaderQuality value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_shaderQuality=(int)value;	
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_SHADER_QUALITY,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	IPropertyScene::IGeometryQuality	GeometryQuality 
	{ 
		virtual IPropertyScene::IGeometryQuality get() 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return static_cast<IPropertyScene::IGeometryQuality>(pProteinVistaRenderer->m_pPropertyScene->m_modelQuality);
			}

			return IPropertyScene::IGeometryQuality::Low;
		}
		virtual void set(IPropertyScene::IGeometryQuality value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_modelQuality=(int)value;	
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_MODEL_QUALITY,"",NULL);
				ForceRenderScene(); 
			}
		}
	}


	
	[Category("Rendering Option")]
	[TypeConverter(PIConverter::typeid)]
	property	CDotNetLight ^ Light1
	{
		virtual CDotNetLight ^ get() 
		{
			return m_arrayLights[0];
		} 
	}


	
	[Category("Rendering Option")]
	[TypeConverter(PIConverter::typeid)]
	property	CDotNetLight ^ Light2
	{
		virtual CDotNetLight ^ get() 
		{
			return m_arrayLights[1];
		} 
	}

	[Category("Rendering Option")]
	property	double	ClipPlaneNear 
	{
		virtual double get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_fNearClipPlane;
			}
			return 0.0;
		}
		virtual void set(double value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_fNearClipPlane =value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_NEAR_CLIP_PLANE,"",NULL);
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	double	ClipPlaneFar
	{
		virtual double get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_fFarClipPlane;
			}

			return 1000.0;
		}
		virtual void set(double value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_fFarClipPlane =value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_FAR_CLIP_PLANE,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	Vector3	CameraPosition 
	{
		virtual Vector3 get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				Vector3 pos;
				pos.X = pProteinVistaRenderer->m_FromVec.x;
				pos.Y = pProteinVistaRenderer->m_FromVec.y;
				pos.Z = pProteinVistaRenderer->m_FromVec.z;
				return pos;
			}

			return Vector3(0,0,0);
		}
		virtual void set(Vector3 pos)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				D3DXVECTOR3 cameraPos(pos.X, pos.Y, pos.Z);
				pProteinVistaRenderer->SetCameraPos(cameraPos);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	IPropertyScene::ICameraType	CameraType 
	{
		virtual IPropertyScene::ICameraType get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return static_cast<IPropertyScene::ICameraType>(pProteinVistaRenderer->m_pPropertyScene->m_cameraType);
			}

			return IPropertyScene::ICameraType::Perspective;
		}
		virtual void set(IPropertyScene::ICameraType value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_cameraType =(int)value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_CAMERA_TYPE,"",NULL);
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	int	FOV 
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_pPropertyScene->m_lFOV;
			}
			return 50;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_lFOV =value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_FOV,"",NULL);
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property int	SizeViewVol
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_pPropertyScene-> m_othoCameraViewVol;
			}
			return 50;
		}
		virtual void set(int value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_othoCameraViewVol =value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_CAMERA_SIZE_VIEW_VOL,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property int	DISPLAY_HETATM
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_pPropertyScene->m_iDisplayHETATM;
			}
			return 50;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_iDisplayHETATM =value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_DISPLAY_HETATM,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	IPropertyScene::IAntiAliasing	 AntiAliasing	
	{
		virtual IPropertyScene::IAntiAliasing get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return static_cast<IPropertyScene::IAntiAliasing>(pProteinVistaRenderer->m_pPropertyScene->m_iAntialiasing);
			}
			return IPropertyScene::IAntiAliasing::None;
		}
		virtual void set(IPropertyScene::IAntiAliasing value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_iAntialiasing =(int)value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_DISPLAY_ANTIALIASING,"",NULL);
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	bool	 ShowSelectionMark
	{
		virtual bool get() 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bShowSelectionMark);
			}

			return false;
		}
		virtual void set(bool value) 
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bShowSelectionMark =value?TRUE:FALSE;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_SELECTION_SHOW,"",NULL); 
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	bool	 EnableAO
	{
		virtual bool get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene-> m_bUseSSAO);
			}

			return true;
		}
		virtual void set(bool value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bUseSSAO =value?TRUE:FALSE;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_SSAO_ENABLE,"",NULL);
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	int	 AORange
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToInt32(pProteinVistaRenderer->m_pPropertyScene->m_ssaoRange);
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_ssaoRange =value ;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_SSAO_HALF_SIZE_BLUR,"",NULL);
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	int		 AOSampling
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToInt32(pProteinVistaRenderer->m_pPropertyScene->m_numSSAOSampling);
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_numSSAOSampling =value ;
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	int	 AOIntensity
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToInt32(pProteinVistaRenderer->m_pPropertyScene->m_ssaoIntensity);
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_ssaoIntensity =value ;
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	IPropertyScene::IAOBlurType	 AOBlurType 
	{
		virtual IPropertyScene::IAOBlurType get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return static_cast<IPropertyScene::IAOBlurType>(pProteinVistaRenderer->m_pPropertyScene->m_ssaoBlurType);
			}

			return IPropertyScene::IAOBlurType::None;
		}
		virtual void set(IPropertyScene::IAOBlurType value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_ssaoBlurType =(int)value ;
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property bool AOFullSizeBuffer
	{
		virtual bool get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bUseFullSizeBlur);
			}

			return true;
		}
		virtual void set(bool value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bUseFullSizeBlur =value?TRUE:FALSE ;
				ForceRenderScene(); 
			}
		}
	}

	[Category("Rendering Option")]
	property	bool  DepthOfField 
	{
		virtual bool get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bDepthOfField);
			}

			return true;
		}
		virtual void set(bool value)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_bDepthOfField =value?TRUE:FALSE ;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_DEPTH_OF_FIELD,"",NULL);
				ForceRenderScene(); 
			}
		}
	}
	[Category("Rendering Option")]
	property	System::Drawing::Color FogColor 
	{
		virtual System::Drawing::Color get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProteinVistaRenderer->m_pPropertyScene->m_fogColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_fogColor),
					GetGValue(pProteinVistaRenderer->m_pPropertyScene->m_fogColor));
			}
			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color color)
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_fogColor =ManagedColor2COLORREF(color);
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_FOG_COLOR,"",NULL);
				ForceRenderScene(); 
			} 
		} 
	}
	[Category("Rendering Option")]
	property	int	  FogStart 
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_pPropertyScene->m_fogStart;
			}
			return 0;
		}
		virtual void set(int start)
		{
			if(start <0) start=0;
			if(start>100) start=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_fogStart =start;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_FOG_START,"",NULL);
				ForceRenderScene(); 
			} 
		}
	}
	[Category("Rendering Option")]
	property	int	  FogEnd   
	{
		virtual int get()
		{
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				return pProteinVistaRenderer->m_pPropertyScene->m_fogEnd;
			}
			return 1000;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
			if ( pProteinVistaRenderer )
			{
				pProteinVistaRenderer->m_pPropertyScene->m_fogEnd =value;
				GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_FOG_END,"",NULL);
				ForceRenderScene(); 
			} 
		}
	}


	void CameraAnimation(IAtom ^ atom, float time)
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			pProteinVistaRenderer->SetCameraAnimation((dynamic_cast<CDotNetAtom ^> (atom))->GetUnManagedAtom(), time);
		}
	}

	void CameraAnimation(IResidue ^ residue, float time)
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			pProteinVistaRenderer->SetCameraAnimation((dynamic_cast<CDotNetResidue ^> (residue))->GetUnManagedResidue(), time);
		}
	}

	void CameraAnimation(Vector3 pos , float time)
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			D3DXVECTOR3 endPos(pos.X, pos.Y, pos.Z);
			pProteinVistaRenderer->SetCameraAnimation(endPos, time);
		}
	}

	void CameraAnimation(IVP ^ vp, float time)
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			CDotNetSelection ^ selection = dynamic_cast<CDotNetSelection ^> (vp);
			if ( selection != nullptr )
			{
				pProteinVistaRenderer->SetCameraAnimation( selection->m_pSelectionDisplay->m_arrayAtomInst , time );
			}
		}
	}

	void CameraAnimation()
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			pProteinVistaRenderer->SetCameraAnimation();
		}
	}
private:
	array < CDotNetLight ^ > ^	m_arrayLights;
};

////////////////////////////////////////////////
[Serializable]
public ref class CPIPropertyWireframe: public CPICommonProperty 
{
public:
	CPIPropertyWireframe( CSelectionDisplay * pSelectionDisplay ) {
		m_pSelectionDisplay = pSelectionDisplay; 
		this->Init();
	}

	virtual CPIProperty^ GetInstance() override {return this;};

	[Category("Wireframe Display")]
	property IPropertyWireframe::IDisplayMode DisplayMode 
	{
		virtual IPropertyWireframe::IDisplayMode get()
		{
			CPropertyWireframe * pProperty = m_pSelectionDisplay->GetPropertyWireframe();
			if ( pProperty )
			{
				return static_cast<IPropertyWireframe::IDisplayMode>(pProperty->m_enumDisplayMethod);
			}

			return IPropertyWireframe::IDisplayMode::All;
		}
		virtual void set(IPropertyWireframe::IDisplayMode value)
		{
			CPropertyWireframe * pProperty= m_pSelectionDisplay->GetPropertyWireframe();
			if ( pProperty)
			{
				pProperty->m_enumDisplayMethod =(int) value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_WIREFRAME_DISPLAY_METHOD);
				ForceRenderScene();
				//pProperty->m_penumDisplayMethod->SetEnum( Convert::ToInt32(_value) );
				//pProperty->m_penumDisplayMethod->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumDisplayMethod) );
			}

		}
	}

	[Category("Wireframe Display")]
	property int LineWidth 
	{
		virtual int get()
		{
			CPropertyWireframe * pProperty = m_pSelectionDisplay->GetPropertyWireframe();
			if ( pProperty )
			{
				return pProperty->m_lineWidth;
			}

			return 1;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertyWireframe * pProperty= m_pSelectionDisplay->GetPropertyWireframe();
			if ( pProperty)
			{
				pProperty->m_lineWidth = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_WIREFRAME_LINE_WIDTH,"");
				ForceRenderScene();
				//pProperty->m_pItemLineWidth->SetNumber(_value);
				//pProperty->m_pItemLineWidth->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemLineWidth) );
			}

		}
	}

};
//////////////////////////////////////////////
[Serializable]
public ref class CPIPropertyStick: public CPICommonProperty 
{
public:
	CPIPropertyStick( CSelectionDisplay * pSelectionDisplay ) {
		m_pSelectionDisplay = pSelectionDisplay; 
		this->Init();
	}

	virtual CPIProperty^ GetInstance() override {return this;};
	[Category("Stick Display")]
	property int SphereResolution 
	{
		virtual int get()
		{
			CPropertyStick *	pProperty = m_pSelectionDisplay->GetPropertyStick();
			if ( pProperty )
			{
				return pProperty->m_sphereResolution;
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertyStick * pProperty= m_pSelectionDisplay->GetPropertyStick();
			if ( pProperty )
			{
				pProperty->m_sphereResolution = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SPACEFILL_SPHERE_RESOLUTION);
				ForceRenderScene();
				//pProperty->m_pItemSphereResolution->SetNumber ( Convert::ToInt32(_value) );
				//pProperty->m_pItemSphereResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereResolution) );
			}
		}
	}
	[Category("Stick Display")]
	property int CylinderResolution
	{
		virtual int get()
		{
			CPropertyStick *	pProperty = m_pSelectionDisplay->GetPropertyStick();
			if ( pProperty )
			{
				return pProperty->m_cylinderResolution;
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertyStick * pProperty= m_pSelectionDisplay->GetPropertyStick();
			if ( pProperty )
			{
				pProperty->m_cylinderResolution = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_STICK_CYLINDER_RESOLUTION);
				ForceRenderScene();
				//pProperty->m_pItemCylinderResolution->SetNumber ( Convert::ToInt32(_value) );
				//pProperty->m_pItemCylinderResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemCylinderResolution) );
			}
		}
	}
	[Category("Stick Display")]
	property double	StickSize
	{
		virtual double get()
		{
			CPropertyStick *	pProperty = m_pSelectionDisplay->GetPropertyStick();
			if ( pProperty )
			{
				return pProperty->m_stickSize;
			}

			return 0.0;
		}
		virtual void set(double value)
		{
			CPropertyStick * pProperty= m_pSelectionDisplay->GetPropertyStick();
			if ( pProperty )
			{
				pProperty->m_stickSize = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_STICK_STICK_SIZE);
				ForceRenderScene();
				//pProperty->m_pItemStickSize->SetDouble ( _value );
				//pProperty->m_pItemStickSize->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemStickSize) );
			}
		}
	}

};
////////////////////////////////////////////////////////////
[Serializable]
public ref class CPIPropertySpaceFill: public CPICommonProperty 
{
public:
	CPIPropertySpaceFill( CSelectionDisplay * pSelectionDisplay ) {
		m_pSelectionDisplay = pSelectionDisplay; 
		this->Init();
	}

	virtual CPIProperty^ GetInstance() override {return this;};
	[Category("Spacefill Display")]
	property int	SphereResolution 
	{
		virtual int get()
		{
			CPropertySpaceFill *	pProperty = m_pSelectionDisplay->GetPropertySpaceFill();
			if ( pProperty )
			{
				return pProperty->m_sphereResolution;
			}

			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertySpaceFill * pProperty= m_pSelectionDisplay->GetPropertySpaceFill();
			if ( pProperty )
			{
				pProperty->m_sphereResolution = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SPACEFILL_SPHERE_RESOLUTION,"");
				ForceRenderScene();
				//pProperty->m_pItemSphereResolution->SetNumber( _value );
				//pProperty->m_pItemSphereResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereResolution) );
			}
		}
	}
};
////////////////////////////////////////
[Serializable]
public ref class CPIPropertyBallnStick: public CPICommonProperty
{
public:
	CPIPropertyBallnStick( CSelectionDisplay * pSelectionDisplay ) {
		m_pSelectionDisplay = pSelectionDisplay;
		this->Init();
	}
	virtual CPIProperty^ GetInstance() override {return this;};

	[Category("Ball & Stick Display")]
	property int SphereResolution 
	{
		virtual int get()
		{
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				return pProperty->m_sphereResolution;
			}
			return 0;
		}
		virtual void set(int value)
		{
			if(value <0) value=0;
			if(value>100) value=100;
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				pProperty->m_sphereResolution = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_BALL_STICK_SPHERE_RESOLUTION);
				ForceRenderScene();
				//pProperty->m_pItemSphereResolution->SetNumber( _value );
				//pProperty->m_pItemSphereResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereResolution) );
			}
		}
	}
	[Category("Ball & Stick Display")]
	property int CylinderResolution 
	{
		virtual int get()
		{

			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				return pProperty->m_cylinderResolution;
			}

			return 0;
		}
		virtual void set(int value)
		{
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				pProperty->m_cylinderResolution = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_BALL_STICK_CYLINDER_RESOLUTION);
				ForceRenderScene();
				//pProperty->m_pItemCylinderResolution->SetNumber( _value );
				//pProperty->m_pItemCylinderResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemCylinderResolution) );
			}
		}
	}

	[Category("Ball & Stick Display")]
	property double SphereRadius
	{
		virtual double get()
		{
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				return pProperty->m_sphereRadius;
			}

			return 0.0;
		}
		virtual void set(double value)
		{
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{

				pProperty->m_sphereRadius = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_BALL_STICK_SPHERE_RADIUS);
				ForceRenderScene();
				//pProperty->m_pItemSphereRadius->SetDouble( _value );
				//pProperty->m_pItemSphereRadius->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereRadius) );
			}
		}
	}
	[Category("Ball & Stick Display")]
	property double CylinderSize 
	{
		virtual double get()
		{
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				return pProperty->m_cylinderSize;
			}

			return 0.0;
		}
		virtual void set(double value)
		{
			CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
			if ( pProperty )
			{
				pProperty->m_cylinderSize = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_BALL_STICK_CYLINDER_RADIUS);
				ForceRenderScene();
				//pProperty->m_pItemCylinderSize->SetDouble( _value );
				//pProperty->m_pItemCylinderSize->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemCylinderSize) );
			}
		}
	}
};
////////////////////////////////////////////////////////////////
[Serializable]
public ref class CPIPropertyRibbon: public CPICommonProperty 
{
private:
	CTexture^ m_HelixTexture;
	CTexture^ m_SheetTexture;
	CTexture^ m_CoilTexture;
public:
	[Category("Helix Display")]
	property bool ShowHelix
	{ 
		bool get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
		 {
			 return Convert::ToBoolean(pProperty->m_bDisplayHelix);
		 }

			return true;
		}
		void set(bool value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
		 {
			 pProperty->m_bDisplayHelix = value?TRUE:False;
			 m_pSelectionDisplay->UpdateAtomPosColorChanged();
			 ForceRenderScene();
			 //m_pSelectionDisplay->SetPropertyChanged(XTP_PGN_ITEMVALUE_CHANGED);
			 //pProperty->m_pDisplayHelix->SetBool ( _value );
			 //pProperty->m_pDisplayHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplayHelix) );
		 }

		}
	}
	[Category("Helix Display")]
	property System::Drawing::Color HelixColor 
	{ 
		virtual System::Drawing::Color get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProperty->m_colorHelix),
					GetGValue(pProperty->m_colorHelix),
					GetGValue(pProperty->m_colorHelix)
					);
			}

			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_colorHelix=ManagedColor2COLORREF(value);
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pcolorHelix->SetColor (  );
				//pProperty->m_pcolorHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pcolorHelix) );
			}
		}
	}

	[Category("Helix Display")]
	property IPropertyHelix::IFitting Fitting 
	{ 
		IPropertyHelix::IFitting get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return static_cast<IPropertyHelix::IFitting>(pProperty->m_fittingMethodHelix);
			}

			return IPropertyHelix::IFitting::Optimal;
		}
		void set(IPropertyHelix::IFitting value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_fittingMethodHelix =(int)value;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_penumFittingMethodHelix->SetEnum ( Convert::ToUInt32(_value) );
				//pProperty->m_penumFittingMethodHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumFittingMethodHelix) );
			}

		}
	}
	[Category("Helix Display")]
	property	System::Drawing::Size HelixSize 
	{ 
		System::Drawing::Size get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return System::Drawing::Size(pProperty->m_sizeHelix.cx, pProperty->m_sizeHelix.cy);
			}

			return System::Drawing::Size(0,0);
		} 
		void set(System::Drawing::Size value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_sizeHelix.cx =value.Width;
				pProperty->m_sizeHelix.cy = value.Height;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_psizeHelix->SetSize ( CSize(_value.Width, _value.Height) );
				//pProperty->m_psizeHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_psizeHelix) );
			}
		}
	}
	[Category("Helix Display")]
	property IPropertyHelix::IShape	HelixShape 
	{ 
		IPropertyHelix::IShape get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return static_cast<IPropertyHelix::IShape>(pProperty->m_shapeHelix);
			}

			return IPropertyHelix::IShape::_30Poly;
		}
		void set(IPropertyHelix::IShape value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_shapeHelix =(int)value;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene();
				//pProperty->m_penumShapeHelix->SetEnum ( Convert::ToInt32(_value) );
				//pProperty->m_penumShapeHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumShapeHelix) );
			}

		}
	}
	[Category("Helix Display")]
	property bool ShowCoilOnHelix 
	{ 
		bool get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bShowCoilOnHelix);
			}

			return true;
		}
		void set(bool value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_bShowCoilOnHelix = value?TRUE:FALSE;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pShowCoilOnHelix->SetBool ( _value );
				//pProperty->m_pShowCoilOnHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pShowCoilOnHelix) );
			}
		}
	}
	[Category("Helix Display")]
	property CTexture^ HelixTexture
	{
		CTexture^ get()
		{
			return m_HelixTexture;
		}
		void set(CTexture^ value)
		{
			if(value==nullptr)
				return;
			this->m_HelixTexture =value; 

		}
	}
	////////////////////////////////////////////////
	[Category("Sheet Display")]
	property	bool ShowSheet
	{
		bool get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bDisplaySheet);
			}

			return true;
		}
		void set(bool value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_bDisplaySheet =value?TRUE:FALSE;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pDisplaySheet->SetBool ( _value );
				//pProperty->m_pDisplaySheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplaySheet) );
			}
		}
	}
	[Category("Sheet Display")]
	property System::Drawing::Color SheetColor 
	{ 
		virtual System::Drawing::Color get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProperty->m_colorSheet),
					GetGValue(pProperty->m_colorSheet),
					GetGValue(pProperty->m_colorSheet)
					);
			}

			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_colorSheet =ManagedColor2COLORREF(value) ;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pcolorSheet->SetColor ( ManagedColor2COLORREF(_value) );
				//pProperty->m_pcolorSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pcolorSheet) );
			}

		}
	}


	[Category("Sheet Display")]
	property	System::Drawing::Size SheetSize 
	{ 
		System::Drawing::Size get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return System::Drawing::Size(pProperty->m_sizeSheet.cx, pProperty->m_sizeSheet.cy);
			}

			return System::Drawing::Size(0,0);
		} 
		void set(System::Drawing::Size value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_sizeSheet.cx =value.Width; 
				pProperty->m_sizeSheet.cy =value.Height;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_psizeSheet->SetSize ( CSize(_value.Width, _value.Height) );
				//pProperty->m_psizeSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_psizeSheet) );
			}

		}
	}
	[Category("Sheet Display")]
	property IPropertySheet::IShape	SheetShape 
	{ 
		IPropertySheet::IShape get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return static_cast<IPropertySheet::IShape>(pProperty->m_shapeSheet);
			}

			return IPropertySheet::IShape::_30Poly;
		}
		void set(IPropertySheet::IShape value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_shapeSheet = (int)value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_SHEET_TEXTURE_COORD_U,"");
				ForceRenderScene(); 
				//pProperty->m_penumShapeSheet->SetEnum ( Convert::ToInt32(_value) );
				//pProperty->m_penumShapeSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumShapeSheet) );
			}
		}
	}
	[Category("Sheet Display")]
	property bool ShowCoilOnSheet 
	{ 
		bool get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bShowCoilOnSheet);
			}

			return true;
		}
		void set(bool value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_bShowCoilOnSheet = value?TRUE:FALSE;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pShowCoilOnSheet->SetBool ( _value );
				//pProperty->m_pShowCoilOnSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pShowCoilOnSheet) );
			}

		}
	}



	[Category("Sheet Display")]
	property CTexture^ SheetTexture
	{
		CTexture^ get()
		{
			return m_SheetTexture;
		}
		void set(CTexture^ value)
		{
			if(value==nullptr)
				return;
			this->m_SheetTexture =value;
			m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_COIL_COLOR);
		}
	}
	///////////////////////////////////////////////
	[Category("Coil Display")]
	property	bool ShowCoil
	{
		bool get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bDisplayCoil);
			}

			return true;
		}
		void set(bool value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_bDisplayCoil = value?TRUE:FALSE;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pDisplayCoil->SetBool ( _value );
				//pProperty->m_pDisplayCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplayCoil) );
			}
		}
	}
	[Category("Coil Display")]
	property System::Drawing::Color CoilColor 
	{ 
		virtual System::Drawing::Color get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return System::Drawing::Color::FromArgb(
					GetRValue(pProperty->m_colorCoil),
					GetGValue(pProperty->m_colorCoil),
					GetGValue(pProperty->m_colorCoil)
					);
			}

			return System::Drawing::Color::White;
		}
		virtual void set(System::Drawing::Color value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_colorCoil=ManagedColor2COLORREF(value);
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene(); 
				//pProperty->m_pcolorCoil->SetColor ( ManagedColor2COLORREF(_value) );
				//pProperty->m_pcolorCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pcolorCoil) );
			}
		}
	}


	[Category("Coil Display")]
	property	System::Drawing::Size CoilSize 
	{ 
		System::Drawing::Size get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return System::Drawing::Size(pProperty->m_sizeCoil.cx, pProperty->m_sizeCoil.cy);
			}

			return System::Drawing::Size(0,0);
		} 
		void set(System::Drawing::Size value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_sizeCoil.cx =value.Width;
				pProperty->m_sizeCoil.cy = value.Height;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene();
				//pProperty->m_psizeCoil->SetSize ( CSize(_value.Width, _value.Height) );
				//pProperty->m_psizeCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_psizeCoil) );
			}

		}
	}
	[Category("Coil Display")]
	property IPropertyCoil::IShape	CoilShape 
	{ 
		IPropertyCoil::IShape get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return static_cast<IPropertyCoil::IShape>(pProperty->m_shapeCoil);
			}

			return IPropertyCoil::IShape::_30Poly;
		}
		void set(IPropertyCoil::IShape value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_shapeCoil = (int)value;
				m_pSelectionDisplay->UpdateAtomPosColorChanged();
				ForceRenderScene();
				//pProperty->m_penumShapeCoil->SetEnum ( Convert::ToInt32(_value) );
				//pProperty->m_penumShapeCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumShapeCoil) );
			}
		}
	}


	[Category("Coil Display")]
	property CTexture^ CoilTexture
	{
		CTexture^ get()
		{
			return m_CoilTexture;
		}
		void set(CTexture^ value)
		{
			if(value==nullptr)
				return;
			this->m_CoilTexture =value; 
			m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_COIL_COLOR);

		}
	}
public:
	virtual void SelectSugarInDNA()
	{
		CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
		if ( pProperty )
		{
			//pProperty->m_pDNASelectSugar->OnClick();
		}
		ForceRenderScene(); 

	}
	virtual void	SelectBackBoneInDNA()
	{
		CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
		if ( pProperty )
		{
			//pProperty->m_pDNASelectBase->OnClick();
		}
		ForceRenderScene(); 

	}
	virtual void	SelectInnerAtomsInDNA()
	{
		CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
		if ( pProperty )
		{
			//pProperty->m_pDNASelectInnerAtom->OnClick();
		}
		ForceRenderScene(); 
	}

	virtual CPIProperty^ GetInstance() override {return this;};


	CPIPropertyRibbon( CSelectionDisplay * pSelectionDisplay )
	{
		m_pSelectionDisplay = pSelectionDisplay; 
		m_HelixTexture =gcnew CTexture(m_pSelectionDisplay);
		m_SheetTexture =gcnew CTexture(m_pSelectionDisplay);
		m_CoilTexture =gcnew CTexture(m_pSelectionDisplay);
		this->Init();
	}

	//////////////////////////////////////////////////////////////////
	[Category("Ribbon Display")] 
	property int	CurveTension 
	{
		virtual int get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return pProperty->m_curveTension;
			}

			return 0;
		}
		virtual void set(int value)
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				pProperty->m_curveTension = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_CURVE_TENSION);
				ForceRenderScene();
				//pProperty->m_pCurveTension->SetNumber(_value);
				//pProperty->m_pCurveTension->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pCurveTension) );
			}
		}
	}
	[Category("Ribbon Display")] 
	property int	CurveResolution 
	{
		virtual int get()
		{
			CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
			if ( pProperty )
			{
				return pProperty->m_resolution;
			}

			return 0;
		}
		virtual void set(int value)
		{
			try
			{
				CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
				if ( pProperty )
				{
					pProperty->m_resolution = value;
					m_pSelectionDisplay->SetPropertyChanged(PROPERTY_RIBBON_RESOLUTION);
					ForceRenderScene();
				}
			}
			catch (System::Runtime::InteropServices::SEHException^ ef)
			{
			}
			 
		}
	}


};

/////////////////////////////////////////////////////////////
[Serializable]
public ref class CPIPropertySurface: public CPICommonProperty 
{
public:
	CPIPropertySurface( CSelectionDisplay * pSelectionDisplay ) 
	{
		m_pSelectionDisplay = pSelectionDisplay; 
		this->Init();
	}

	virtual CPIProperty^ GetInstance() override {return this;};

	[Category("Surface Display")] 
	property	IPropertySurface::IDisplayMethod	DisplayMethod
	{
		virtual IPropertySurface::IDisplayMethod get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return static_cast<IPropertySurface::IDisplayMethod>(pProperty->m_enumSurfaceDisplayMethod);
			}

			return IPropertySurface::IDisplayMethod::Solid;
		}
		virtual void set(IPropertySurface::IDisplayMethod value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_enumSurfaceDisplayMethod = (int)value;
				ForceRenderScene();
				//pProperty->m_pSurfaceDisplayMethod->SetEnum(Convert::ToInt32(_value));
				//pProperty->m_pSurfaceDisplayMethod->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceDisplayMethod) );
			}
		}
	}
	[Category("Surface Display")] 
	property	int		Transparency 
	{
		virtual int get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return pProperty->m_transparency;
			}

			return 0;
		}
		virtual void set(int value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_transparency = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_TRANSPARENCY,"");
				ForceRenderScene();
				//pProperty->m_ptransparency->SetNumber(_value);
				//pProperty->m_ptransparency->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_ptransparency) );
			}
		}
	}

	[Category("Surface Display")] 
	property	double	ProbeSphereRadius 
	{
		virtual double get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return pProperty->m_probeSphere;
			}

			return 0.0;
		}
		virtual void set(double value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_probeSphere = value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_TRANSPARENCY_SURFACE_PROBE_SPHERE);
				ForceRenderScene();
				//pProperty->m_pProbeSphere->SetDouble(_value);
				//pProperty->m_pProbeSphere->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pProbeSphere) );
			}
		}
	}

	[Category("Surface Display")] 
	property	IPropertySurface::IAlgorithm Algorithm
	{
		virtual IPropertySurface::IAlgorithm get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return static_cast<IPropertySurface::IAlgorithm> (pProperty->m_surfaceGenMethod);
			}
			return IPropertySurface::IAlgorithm::MQ;
		}
		virtual void set(IPropertySurface::IAlgorithm value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_surfaceGenMethod =(int) value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_GEN_METHOD);
				ForceRenderScene();
				//pProperty->m_pSurfaceGenMethod->SetEnum(Convert::ToInt32(algorithm));
				//pProperty->m_pSurfaceGenMethod->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceGenMethod) );
			}
		}
	}

	[Category("Surface Display")] 
	property	bool				AddHETATM 
	{
		virtual bool get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bAddHETATM);
			}
			return true;
		}
		virtual void set(bool value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_bAddHETATM = value?TRUE:FALSE;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_INCLUDE_HETATM);
				ForceRenderScene();
				//pProperty->m_pAddHETATM->SetBool(Convert::ToInt32(_value));
				//pProperty->m_pAddHETATM->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pAddHETATM) );
			}
		}
	}

	[Category("Surface Display")] 
	property	bool ShowCurvature 
	{
		virtual bool get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bDisplayCurvature);
			}
			return true;
		}
		virtual void set(bool value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_bDisplayCurvature = value?TRUE:FALSE;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_SHOW_CURVATURE);

				ForceRenderScene();
				//pProperty->m_pDisplayCurvate->SetBool(Convert::ToInt32(_value));
				//pProperty->m_pDisplayCurvate->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplayCurvate) );
			}
		}
	}
	[Category("Surface Display")] 
	property	IPropertySurface::ICurvatureRingSize	CurvatureRingSize 
	{
		virtual IPropertySurface::ICurvatureRingSize get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return static_cast<IPropertySurface::ICurvatureRingSize>(pProperty->m_curvatureRingSize);
			}

			return IPropertySurface::ICurvatureRingSize::_1;
		}
		virtual void set(IPropertySurface::ICurvatureRingSize value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_curvatureRingSize =(int) value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_CURVATURE_RINGSIZE);
				ForceRenderScene();
				//pProperty->m_pCurvatureRingSize->SetEnum(Convert::ToInt32(_value));
				//pProperty->m_pCurvatureRingSize->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pCurvatureRingSize) );
			}
		}
	}

	[Category("Surface Display")] 
	property	IPropertySurface::IColorSmoothing	ColorSmoothing 
	{
		virtual IPropertySurface::IColorSmoothing get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return static_cast<IPropertySurface::IColorSmoothing>(pProperty->m_iSurfaceBlurring);
			}

			return	IPropertySurface::IColorSmoothing::_1;
		}
		virtual void set(IPropertySurface::IColorSmoothing value)
		{

			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_iSurfaceBlurring =(int)value;
				m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_SHOW_CURVATURE);
				ForceRenderScene();
				//pProperty->m_pSurfaceBlurring->SetEnum(Convert::ToInt32(_value));
				//pProperty->m_pSurfaceBlurring->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceBlurring) );
			}
		}
	}
	[Category("Surface Display")] 
	property	bool				DepthSort 
	{
		virtual bool get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_bSurfaceDepthSort);
			}

			return true;
		}
		virtual void set(bool value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_iSurfaceBlurring = value?TRUE:FALSE;
				ForceRenderScene();
				//pProperty->m_pSurfaceDepthSort->SetBool(Convert::ToInt32(_value));
				//pProperty->m_pSurfaceDepthSort->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceDepthSort) );
			}
		}
	}

	[Category("Surface Display")] 
	property bool UseInnerFaceColor 
	{
		virtual bool get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return Convert::ToBoolean(pProperty->m_useInnerFaceColor);
			}
			return true;
		}
		virtual void set(bool value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_useInnerFaceColor = value?TRUE:FALSE;
				ForceRenderScene();
				//pProperty->m_pUseInnerFaceColor->SetBool(Convert::ToInt32(_value));
				//pProperty->m_pUseInnerFaceColor->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pUseInnerFaceColor) );
			}
		}
	}
	[Category("Surface Display")] 
	property	int	 InnerFaceColorBlend 
	{
		virtual int get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return pProperty->m_blendFactor;
			}

			return 0;
		}
		virtual void set(int value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_blendFactor = value;
				ForceRenderScene();
				//pProperty->m_pBlendInnerFace->SetNumber(_value);
				//pProperty->m_pBlendInnerFace->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pBlendInnerFace) );
			}
		}
	}
	[Category("Surface Display")] 
	property	Color InnerFaceColor 
	{
		virtual Color get()
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				return Color::FromArgb(
					GetRValue(pProperty->m_colorInnerFace),
					GetGValue(pProperty->m_colorInnerFace),
					GetGValue(pProperty->m_colorInnerFace)
					);
			}

			return System::Drawing::Color::White;
		}
		virtual void set(Color value)
		{
			CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
			if ( pProperty )
			{
				pProperty->m_blendFactor = ManagedColor2COLORREF(value);
				ForceRenderScene();
				//pProperty->m_pColorInnerFace->SetColor( ManagedColor2COLORREF(_value) );
				//pProperty->m_pColorInnerFace->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pColorInnerFace) );
			}
		}
	}


	virtual	 void	 SelectSurfaceAtoms()
	{
		CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
		if ( pProperty )
		{
			m_pSelectionDisplay->SetPropertyChanged(PROPERTY_SURFACE_SELECT_ATOM_SURFACE);
			ForceRenderScene(); 
			//pProperty->m_pSelectSurfaceAtom->OnClick();		//	SetBool(TRUE);
		}

	}
};


