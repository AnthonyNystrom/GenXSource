/***************************************************************************
 *   Copyright (C) 2005 by Abderrahman Taha                                *
 *                                                                         *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

#pragma once
#include "NuGenPSurfaceModel3D.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;

/// <newCode>
using namespace Microsoft::DirectX;
using namespace Microsoft::DirectX::Direct3D;
#define LOCK(object) if(gclock _lock_=(object))
/// </newCode>

namespace Genetibase
{
namespace MathX {

	/// <newcode>

	/// <summary>
	/// Hardware Display settings
	/// </summary>
	ref struct HwDSettingsGeneral
	{
	public:
		bool MultiSample;
	};

	ref struct HwDSettingsDirectX
	{
	public:
		AdapterInformation^ AdapterInf;
		bool HardwareDevice;
		bool HwTnL;
		bool PureDevice;
	};

	/// </newCode>

	/// <summary>
	/// Summary for NuGenPSurface
	/// </summary>
	public ref class NuGenPSurface : public System::Windows::Forms::UserControl
	{
	// define class members
protected:
		virtual void OnPaint( PaintEventArgs^ ) override;
		virtual void OnMouseDown( MouseEventArgs^) override;
		virtual void OnMouseUp( MouseEventArgs^) override;
		virtual void OnMouseMove( MouseEventArgs^) override;
		virtual void OnKeyPress ( KeyPressEventArgs ^) override;
		virtual void OnResize(EventArgs ^) override;
		virtual void OnPaintBackground( PaintEventArgs ^e) override;
		/// <newCode>
		virtual void OnMouseDoubleClick(MouseEventArgs^) override;
		virtual void OnBackgroundImageChanged(EventArgs^) override;
		virtual void OnBackgroundImageLayoutChanged(EventArgs^) override;
		/// </newCode>

public: // slots:
		void valueChanged();
		void newFile();
		void setFunction(int);
		void cutline(int);
		void cutcolumn(int);
		void linecolumn(int);
		void boxok();
		void anim();
		void addcondt();
		void meshcondt();
	    
		void anim4xy();  //4D rotation
		void anim4xz();  //4D rotation
		void anim4yz();  //4D rotation
		void anim4xw();  //4D rotation
		void anim4yw();  //4D rotation
		void anim4zw();  //4D rotation    
    
		void anglexy(int);  //4D rotation
		void anglexz(int);  //4D rotation
		void angleyz(int);  //4D rotation
		void anglexw(int);  //4D rotation
		void angleyw(int);  //4D rotation
		void anglezw(int);  //4D rotation
		void anglext(int);  //5D rotation
		void angleyt(int);  //5D rotation
		void anglezt(int);  //5D rotation
		void anglewt(int);  //5D rotation
		void anglexs(int);  //6D rotation
		void angleys(int);  //6D rotation
		void anglezs(int);  //6D rotation
		void anglews(int);  //6D rotation
		void anglets(int);  //6D rotation
	    
		void anim5xy();  //5D rotation
		void anim5xt();  //5D rotation    
		void anim5xz();  //5D rotation
		void anim5yz();  //5D rotation
		void anim5xw();  //5D rotation
		void anim5yw();  //5D rotation
		void anim5zw();  //5D rotation     
		void anim5yt();  //5D rotation
		void anim5zt();  //5D rotation
		void anim5wt();  //5D rotation
	    
		void anim6xs();  //6D rotation
		void anim6ys();  //6D rotation
		void anim6zs();  //6D rotation
		void anim6ws();  //6D rotation
		void anim6ts();  //6D rotation     
    
		void animND(int );  // Multi purpose fct for rotational plans
		void help();                
		void morph();
		void start();
		void wait();
		void clipok();
		void infosok();
		void latence_change(int);
		void step_morph(int);
		void red(int);
		void green(int);
		void blue(int);
		void transparence();
		void interior();
		void exterior();
		void videorecord();
		void screenshot();
		void jpg();
		void bmp();
		void png();
		void quality(int);
		void jpg2();
		void bmp2();
		void png2();
		void quality2(int);
		void activate_frame();
		void frame_name_short();
		void frame_name_big();
		void zbuffer_activate();
		void zbuffer_quality_change(int);
    
		void scalex(int);
		void scaley(int);
		void scalez(int);
		void activescalex();
		void activescaley();
		void activescalez();
	    
		void nbtwistez_changed(int);
		void coeffrayonz_changed(int);
	    
		void nbtwistey_changed(int);
		void coeffrayony_changed(int);    
	    
		void nbtwistex_changed(int);
		void coeffrayonx_changed(int);    
	    
		void save_changes();

		void SaveImageToFile(String^ , ImageFormat^);
		void OpenSaveImageDialog();

		/// <newCode rv="2">
		void XamlExport();
		/// </newCode>
	    
public:
		array<Point> ^points;				// point array
		array<Color> ^colors;				// color array
		int		count, anim_ok, anim4_ok, anim5_ok, morph_ok, taillechanged;// count = number of points
		bool	down;				// TRUE if mouse down
		Model3D  ^objet;
		Bitmap^ pixmap;
		int btgauche, btdroit, btmilieu;
		int width_; 
		int height_;
		int latence, colortype, video_ok;
	//    Image^ alpha;

		int png_ok, jpg_ok, bmp_ok,
			png2_ok, jpg2_ok, bmp2_ok,
			quality_image, quality_image2, 
			counter, frames_ok, short_names, big_names,
			oldScale_x, oldScale_y, oldScale_z, 
			scalexactivated, scaleyactivated, scalezactivated,
			twistezactivated,
			coeff_rayonz, nb_twistez, oldcoeff_rayonz, oldnb_twistez,
			coeff_rayony, nb_twistey, oldcoeff_rayony, oldnb_twistey,
			coeff_rayonx, nb_twistex, oldcoeff_rayonx, oldnb_twistex;

	//    QFile *f;
		//    QPNGImagePacker *pngfile;
		String^ directory;
		int condition_mesh, add_condition;
	//    QRegion  *r;    //MAKE IT GLOBAL VARIABLE
	//    QPaintEvent *me;

		[Description("Background Color"),Category("Colors")]
		property Color BackgroundColor
		{
			Color get()
			{
				return MyBackgroundColor;
			}
			void set(Color value)
			{
				MyBackgroundColor = value;
				//Invalidate();
				Refresh();
			}
		}

	   [Description("Front Surface Color"),Category("Colors")]
		property Color FrontSurfaceColor
		{
			Color get()
			{
				return MyFrontSurfaceColor;
			}
			void set(Color value)
			{
				MyFrontSurfaceColor = value;
				UpdateObjectColors();
				//Invalidate();
				Refresh();
			}
		}

		[Description("Back Surface Color"),Category("Colors")]
		property Color BackSurfaceColor
		{
			Color get()
			{
				return MyBackSurfaceColor;
			}
			void set(Color value)
			{
				MyBackSurfaceColor = value;
				UpdateObjectColors();
				//Invalidate();
				Refresh();
			}
		}

		[Description("Gridline Color"),Category("Colors")]
		property Color GridLineColor
		{
			Color get()
			{
				return MyGridLineColor;
			}
			void set(Color value)
			{
				MyGridLineColor = value;
				UpdateObjectColors();
				//Invalidate();
				Refresh();
			}
		}

	   [Description("Indicates if the boundary around the object is drawn."),Category("Appearance")]
		property bool DrawBorder
		{
			bool get()
			{
				return MyDrawBorder;
			}
			void set(bool value)
			{
				MyDrawBorder = value;
				//Invalidate();
				Refresh();
			}
		}

		enum class QualityEnum {
			Lowest,
			Low,
			Medium,
			High,
			Highest
		};

		[Description("Sets the smoothness of the model."),Category("Quality"),DefaultValue(QualityEnum::Lowest)]
		property QualityEnum Quality
		{
			QualityEnum get()
			{
				return MyQuality;
			}
			void set(QualityEnum value)
			{
				MyQuality = value;
				UpdateQuality();
				//Invalidate();
				Refresh();
			}
		}

		[Description("Sets the Z Buffer Level."),Category("Quality")]
		property int OptimizeLevel
		{
			int get()
			{
				return MyOptimizeLevel;
			}
			void set(int value)
			{
				if (value > 0 && value < 6)
				{
					MyOptimizeLevel = value;
					UpdateQuality();
					//Invalidate();
					Refresh();
				}
				else
				{
					System::Windows::Forms::MessageBox::Show("OptimizeLevel must be between 1 and 5.", "Invalid Value");
				}
			}
		}

		[Description("Indicates if antialiasing is used."),Category("Quality")]
		property bool UseAntialiasing
		{
			bool get()
			{
				return MyAntialiasing;
			}
			void set(bool value)
			{
				MyAntialiasing = value;

				/// <newCode>
				// reset device with new PP
				if (generalHwDSettings.MultiSample && MyAntialiasing)
					presentParams->MultiSample = MultiSampleType::FourSamples;
				else
					presentParams->MultiSample = MultiSampleType::None;

				if (graphicsDevice != nullptr)
					graphicsDevice->Reset(presentParams);
				/// </newCode>

				Refresh();
			}
		}

		[Category("Expression")]
		property String^ Expression_X
		{
			String^ get()
			{
				return objet->expression_X;
			}
			void set(String^ value)
			{
				objet->expression_X = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		[Category("Expression")]
		property String^ Expression_Y
		{
			String^ get()
			{
				return objet->expression_Y;
			}
			void set(String^ value)
			{
				objet->expression_Y = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		[Category("Expression")]
		property String^ Expression_Z
		{
			String^ get()
			{
				return objet->expression_Z;
			}
			void set(String^ value)
			{
				objet->expression_Z = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		/*[Category("Expression")]
		property String^ Expression_W
		{
			String^ get()
			{
				return objet->expression_W;
			}

			void set(String^ value)
			{
				objet->expression_W = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		[Category("Expression")]
		property String^ Expression_T
		{
			String^ get()
			{
				return objet->expression_T;
			}

			void set(String^ value)
			{
				objet->expression_T = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		[Category("Expression")]
		property String^ Expression_S
		{
			String^ get()
			{
				return objet->expression_S;
			}

			void set(String^ value)
			{
				objet->expression_S = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		[Category("Expression")]
		property bool FiveDSupport
		{
			bool get()
			{
				return objet->fivedimshapes == 1;
			}

			void set(bool value)
			{
				if (value)
				{
					objet->fivedimshapes = 1;
					objet->fourdimshapes = 1;
					if (objet->expression_T == "" || objet->expression_W == "")
						MessageBox::Show("Expression_T or Expression_W is not set for 5dSupport");
				}
				else
				{
					objet->fivedimshapes = -1;
					objet->fourdimshapes = -1;
				}
			}
		}*/

		[Category("Expression")]
		property String^ uMin
		{
			String^ get()
			{
				return objet->inf_u;
			}
			void set(String^ value)
			{
				objet->inf_u = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}
	
		[Category("Expression")]
		property String^ uMax
		{
			String^ get()
			{
				return objet->sup_u;
			}
			void set(String^ value)
			{
				objet->sup_u = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		[Category("Expression")]
		property String^ vMin
		{
			String^ get()
			{
				return objet->inf_v;
			}
			void set(String^ value)
			{
				objet->inf_v = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}
		
		[Category("Expression")]
		property String^ vMax
		{
			String^ get()
			{
				return objet->sup_v;
			}
			void set(String^ value)
			{
				objet->sup_v = value;
				if (reDraw)
				{
					valueChanged();
					Update();
					Refresh();
				}
			}
		}

		enum class Shapes {
			Prism,
			Cube,
			Hexagon,
			Cone,
			Diamond,
			Shape_10,
			Star_7,
			Shape_8,
			Shape_9,
			Star,
			Implicit_Lemniscape,
			Twisted_heart,
			Folium,
			Heart,
			Bow_Tie,
			Triaxial_Hexatorus,
			Ghost_Plane,
			Bent_Horns,
			Richmond,
			Kidney,
			Kinky_Torus,
			Snail,
			Limpet_Torus,
			Twisted_Triaxial,
			Apple,
			Boy,
			Maeders_Owl,
			Cone_2,
			Eight,
			Drop,
			Plan,
			Ellipsoide,
			EightSurface,
			Dini,
			Flower,
			Cosinus,
			Shell,
			Sphere,
			Steiner,
			Cross_cap,
			Boys,
			Torus,
			Klein,
			Moebius,
			Riemann,
			Klein_2,
			Henneberg,
			Enneper,
			Helix,
			Hexaedron,
			Sphere_1,
			Sphere_2,
			Sphere_3,
			Sphere_4,
			Sphere_5,
			Sphere_6,
			Sphere_7,
			Sphere_8,
			Sphere_9,
			Sphere_10,
			Catalan,
			Toupie,
			Bonbon,
			Curve,
			Trumpet,
			Helice_Curve,
			Cresent,
			Shoe,
			Snake,
			Roman,
			Hyperhelicoidal,
			Horn,
			Helicoidal,
			Catenoid,
			Kuen,
			Hellipticparaboloid,
			Enneper_2,
			Stereosphere,
			Cliffordtorus,
			Fresnel_1,
			Fresnel_2,
		};

		[Category("Expression")]
		property Shapes Shape
		{
			Shapes get()
			{
				return myShape;
			}
			void set(Shapes value)
			{
				myShape = value;
				UpdateShape();
				Refresh();
			}
		}

private:
		void dumpMovie (); 
		void UpdateObjectColors();
		void UpdateQuality();
		void UpdateShape();

private:
		Color MyBackgroundColor;
		Color MyFrontSurfaceColor;
		Color MyBackSurfaceColor;
		Color MyGridLineColor;
		bool MyDrawBorder;
		QualityEnum MyQuality;
		int MyOptimizeLevel;
		bool MyAntialiasing;
		Shapes myShape;
		bool reDraw;
		//bool MyDoubleBuffering;

public:
		NuGenPSurface(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
			width_  = 650;
			height_ = 650;

			//setBackgroundColor( black );	
			
			//SetStyle(ControlStyles::UserPaint | ControlStyles::AllPaintingInWmPaint | ControlStyles::DoubleBuffer, true);
		//    setWindowOpacity ( 100);
			pixmap = gcnew Bitmap(this->Width, this->Height);
			Width = width_;
			Height = height_;
			
			objet = gcnew Model3D;
			/// <newCode>
			presentParams = gcnew PresentParameters();
			DetectAdapter();
			CreateGraphicsDevice();
			CreateMatrices();

			objet->SetGraphicsDevice(graphicsDevice);
			this->DoubleBuffered = true;
			matStack = gcnew array<Microsoft::DirectX::Matrix>(3);
			selectPolygons = true;
			bgVBuffer = gcnew VertexBuffer(CustomVertex::PositionTextured::typeid, 6,
										   graphicsDevice, Usage::None,
										   CustomVertex::PositionTextured::Format,
										   Pool::Managed);
			/// </newCode>
			taillechanged = -1;
			anim_ok  = -1;
			anim4_ok = -1;
			anim5_ok = -1;	
			morph_ok = -1;
			latence = 30;
			colortype = 0;
			video_ok = -1;
		    
			counter = 0;
		    
			png_ok =  1;
			jpg_ok = -1;
			bmp_ok = -1;
			quality_image = 50;  
		    
			jpg2_ok =  1;
			png2_ok = -1;
			bmp2_ok = -1;
			quality_image2 = 50;
			frames_ok   = -1;
			short_names = -1;
			big_names   =  1;   
			directory = "frames";
			condition_mesh = 1;
			add_condition  =-1;

			oldScale_x = oldScale_y = oldScale_z = 10;
			scalexactivated = -1; scaleyactivated = -1; scalezactivated = -1;
			twistezactivated = -1;
			coeff_rayonz = 10; nb_twistez = 0; oldcoeff_rayonz = 10; oldnb_twistez = 0;
			coeff_rayony = 10; nb_twistey = 0; oldcoeff_rayony = 10; oldnb_twistey = 0;    
			coeff_rayonx = 10; nb_twistex = 0; oldcoeff_rayonx = 10; oldnb_twistex = 0;    

			objet->Init();

			this->Shape = Shapes::Trumpet;

			OptimizeLevel = 5;
			FrontSurfaceColor = Color::FromArgb(0, 210, 0);
			BackSurfaceColor = Color::FromArgb(249, 170, 0);
			GridLineColor = Color::FromArgb(0, 100, 4);
			
			reDraw = true;

			Refresh();
		}

protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~NuGenPSurface()
		{
			if (components)
			{
				delete components;
			}
		}

private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->SuspendLayout();
			// 
			// NuGenPSurface
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->Name = L"NuGenPSurface";
			this->Size = System::Drawing::Size(650, 650);
			this->ResumeLayout(false);
		}
#pragma endregion

		/// <newCode>
public:
		enum class RenderingMode {
			Software,
			DirectX
		};

		enum class ContentType {
			PSurface,
			Avalon
		};
private:
		RenderingMode MyRenderMode;
		Device^ graphicsDevice;
		PresentParameters^ presentParams;
		Matrix matrixProjDx, matrixViewDx, matrixWorldDx;
		HwDSettingsGeneral generalHwDSettings;
		HwDSettingsDirectX directxHwDSettings;
		array<Point3F>^ shapePointCache;
		array<Microsoft::DirectX::Matrix>^ matStack;
		bool selectPolygons;
		Texture^ bgTex;
		SizeF bgImgSize;
		VertexBuffer^ bgVBuffer;
		ContentType contentType;

		void CreateGraphicsDevice();
		void CreateMatrices();
		void DrawDxFrame(PaintEventArgs ^e);
		// Creates a points list for the current structure
		array<Point3F>^ CreateCurrentPointsList();
		// Creates a points list for eash level of the structure
		array<array<Point3F>^>^ CreatePointsListByLevel();
		void DetectAdapter();
		void TrySelectFrom2DScreen(int x, int y);
		void CancelResize(System::Object^ sender, System::ComponentModel::CancelEventArgs^ e);
		void ReBuildBgBuffers();
		array<CustomVertex::PositionNormal>^ IndexRawTriangles(array<CustomVertex::PositionNormal>^ triangles, array<int>^ oIndices);

public:
		[Description("The type of content current being rendered")]
		property ContentType CurrentContentType
		{
			ContentType get()
			{
				return contentType;
			}
		}

		[Description("Sets the prefered rendering method"),Category("Quality"),DefaultValue(RenderingMode::Software)]
		property RenderingMode RenderMode
		{
			RenderingMode get()
			{
				return MyRenderMode;
			}
			void set(RenderingMode value)
			{
				MyRenderMode = value;
				this->DoubleBuffered = (value == RenderingMode::Software);
				//UpdateQuality();
				//Invalidate();
				Refresh();
			}
		}

		[Description("Sets the type of grid that will be displayed"),Category("DirectX"),DefaultValue(Model3D::GridMode::TwoDimensions)]
		property Model3D::GridMode GridType
		{
			Model3D::GridMode get()
			{
				return objet->gridMode;
			}
			void set(Model3D::GridMode value)
			{
				objet->gridMode = value;
				Refresh();
			}
		}

		[Description("If the point coordinates for the selected polygon are displayed"),Category("DirectX"),DefaultValue(true)]
		property bool SelectionText
		{
			bool get()
			{
				return objet->drawSelectedPolyText;
			}
			void set(bool value)
			{
				objet->drawSelectedPolyText = value;
				Refresh();
			}
		}

		[Description("The color of selected polygon text"),Category("DirectX")]
		property Color SelectionTextColor
		{
			Color get()
			{
				return objet->selectedPolyTextClr;
			}
			void set(Color value)
			{
				objet->selectedPolyTextClr = value;
				Refresh();
			}
		}

		[Description("The color of the selected polygon outline"),Category("DirectX")]
		property Color SelectionOutlineColor
		{
			Color get()
			{
				return objet->selectedPolyOutlineClr;
			}
			void set(Color value)
			{
				objet->selectedPolyOutlineClr = value;
				Refresh();
			}
		}

		[Description("The number of decimal places to show for the point value text on the selected polygon (-1 for all)"),Category("DirectX"),DefaultValue(1)]
		property int SelectionTextDP
		{
			int get()
			{
				return objet->decimalPoints;
			}
			void set(int value)
			{
				if (value > 0 || value == -1)
				{
					objet->decimalPoints = value;
					Refresh();
				}
			}
		}

		[Description("The points that the current shape consists of"),Category("Expression")]
		property array<Point3F>^ ShapePoints
		{
			array<Point3F>^ get()
			{
				if (shapePointCache == nullptr)
					shapePointCache = objet->CreateStructurePointsList();
				return shapePointCache;
			}
		}

		[Description("The spacing inbetween grid lines"),Category("DirectX"),DefaultValue(50)]
		property unsigned int GridSpacing
		{
			unsigned int get()
			{
				return objet->gridSpacing;
			}
			void set(unsigned int value)
			{
				if (value > 5)
				{
					objet->gridSpacing = value;
					objet->BuildDxGrids();
					Refresh();
				}
			}
		}

		[Description("The points for the selected polygon"),Category("DirectX")]
		property array<Point3F>^ SelectedPolygon
		{
			array<Point3F>^ get()
			{
				if (objet->PolyIsSelected)
					return objet->SelectedPolygon;
				return nullptr;
			}
		}

		[Description("If double-clicking should select/deselect polygons"),Category("DirectX")]
		property bool PolygonSelection
		{
			bool get()
			{
				return selectPolygons;
			}
			void set(bool value)
			{
				selectPolygons = value;
			}
		}

		/*[Category("DirectX")]
		property bool Wireframe
		{
			bool get()
			{
				return (objet->DxFillMode == FillMode::WireFrame);
			}
			void set(bool value)
			{
				if (value)
					objet->DxFillMode = FillMode::WireFrame;
				else
					objet->DxFillMode = FillMode::Solid;

				Refresh();
			}
		}*/

		/// </newCode>
	};
}
}