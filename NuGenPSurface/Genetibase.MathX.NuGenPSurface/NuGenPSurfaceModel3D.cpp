// This is the main DLL file.

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
 
//#include <qpixmap.h>
//#include <Q3PointArray>
#include "NuGenPSurfaceModel3D.h"

//using std::vector;
//using std::list;
using namespace System::Drawing::Drawing2D;

using namespace Microsoft::DirectX;

Model3D::Model3D( )
{
	Tre2 = gcnew array<double,2>(300,100);

	Tre = gcnew array<double,2>(300, 100);
	Tre2_save = gcnew array<double,2>(300,100);
	HL = gcnew array<double,2>(300,100);
	HR = gcnew array<double,2>(300,100); 
	DL = gcnew array<double,2>(300,100); 
	DR = gcnew array<double,2>(300,100);//to replace hidden points
	HL2 = gcnew array<double,2>(300,100);
	HR2 = gcnew array<double,2>(300,100);
	DL2 = gcnew array<double,2>(300,100);
	DR2 = gcnew array<double,2>(300,100); 
	HL3 = gcnew array<double,2>(300,100); 
	HR3 = gcnew array<double,2>(300,100); 
	DL3 = gcnew array<double,2>(300,100);
	DR3 = gcnew array<double,2>(300,100);
	Nor = gcnew array<double,2>(300,100); 
	Nor2 = gcnew array<double,2>(300,100);
	Nor3 = gcnew array<double,2>(300,100);
	Tre3 = gcnew array<double,2>(300,100);
	shape4D = gcnew array<double,2>(400,100); 
	shape5D = gcnew array<double,2>(500,100); 
	shape6D = gcnew array<double,2>(600,100);

	zbuffer = gcnew array<int,2>(11, 2*10000);
	rayon   = gcnew array<double>(2*10000);
	hidden_points = gcnew array<int,2>(100, 100);
	tmp = gcnew array<double>(300);
	tmp2 = gcnew array<double>(300);

	v1 = gcnew array<double>(3);
	v2 = gcnew array<double>(3);
	Obser = gcnew array<double>(3);
	norm  = gcnew array<double>(3);
	Oprime = gcnew array<double>(3);
	vec    = gcnew array<double>(3);

	center = gcnew array<double>(3);
	boiteenglobante = gcnew array<double>(24);
	boiteenglobante2 = gcnew array<double>(24);

	polys_ = gcnew List<Polygon^>;

	myParser = gcnew FunctionParser; 
	myParserX = gcnew FunctionParser;
	myParserY = gcnew FunctionParser;
		myParserZ = gcnew FunctionParser;
		myParserW  = gcnew FunctionParser;
		myParserT = gcnew FunctionParser;
		myParserS= gcnew FunctionParser;
        myParserX_2 = gcnew FunctionParser;
		myParserY_2 = gcnew FunctionParser;
		myParserZ_2= gcnew FunctionParser;
		f1= gcnew FunctionParser;
        myParser_spherical= gcnew FunctionParser;
		myParser_cylindrical= gcnew FunctionParser;
        myParser_condition= gcnew FunctionParser;

		nb_licol = 25;
		largeur_fenetre = hauteur_fenetre = 650;

	backtrans  = -1; 
    fronttrans = -1;

    tableaureferences = gcnew array<Q3PointArray^>(2*(100)*(100));
    for (int j=0; j < 20000; j++)
    {
        tableaureferences[j] = gcnew Q3PointArray(4);
    }
    
    tableau = gcnew array<Polygon^>(2* 100 * 100);
    for (int i = 0; i < 2 * 100 * 100; i++)
    {
        tableau[i] = gcnew Polygon;
    }

    expression_X = "(3*(1+sin(v)) + 2*(1-cos(v)/2)*cos(u))*cos(v)"; 
    expression_Y = "(4+2*(1-cos(v)/2)*cos(u))*sin(v)";
    expression_Z = "-2*(1-cos(v)/2) * sin(u)";	
    inf_u        = "0";
    sup_u        = "2*pi";
    inf_v        = "0";
    sup_v        = "pi";
        
    two_system = 1;  //risky 
	
    expression_X_2 = "3*(1+sin(v))*cos(v) - 2*(1-cos(v)/2)*cos(u)";
    expression_Y_2 = "4*sin(v)";
    expression_Z_2 = "-2*(1-cos(v)/2)* sin(u)";
    inf_u_2        = "0";
    sup_u_2        = "2*pi";
    inf_v_2        = "pi";
    sup_v_2        = "2*pi";

	/// <newCode>
	structFirstHalfMat = gcnew Material();
	structSecondHalfMat = gcnew Material();
	gridMode = GridMode::TwoDimensions;
	dxMatrix = Microsoft::DirectX::Matrix::Identity;
	selectedTri = -1;

	selectedPolyPoints = gcnew array<Vector3>(4);
	SelectedPolygon = gcnew array<Point3F>(4);
	PolyIsSelected = false;

	selectedPolyTextClr = Color::Black;
	drawSelectedPolyText = true;
	selectedPolyOutlineClr = Color::Red;
	gridSpacing = 50;
	decimalPoints = 1;
	//DxFillMode = Microsoft::DirectX::Direct3D::FillMode::Solid;
	/// </newCode>
}

void Model3D::Init()
{
    //=========== La premiere figure " klein" a dessiner au lancement======

	keyboard ="";
	coupure_ligne = coupure_col = 0;
	
 //   backsurfr = 249;
	//backsurfg = 170;
 //   backsurfb = 0;
	//
 //  frontsurfr = 0;
 //  frontsurfg = 210;
 //  frontsurfb = 0;

 //   gridliner = 0;
 //   gridlineg = 100;
 //   gridlineb = 4;

    initialiser_palette();
 	initialiser_parametres3();
	initialiser_parseur3();
	parse_expression();
	calcul_objet3();  // Utilisation du parseur..
    boite_englobante3();
	normalisation3(); // Calcul des normales
	
	mat->unit(); // On applique la matrice unite a la matrice principale 3D
	mat4D->unit(); // On applique la matrice unite a la matrice principale 4D
	rotation3();      // rotation de l'objet....
	homothetie3();    // pour faire entrer l'objet ds la boite englobante..
	calcul_points3(); // On applique la rotation et l'homothetie	

	/// <newCode>
	BuildDxBuffers();
	/// </newCode>
}

void Model3D::twistex(double nb_twiste, double coeff_rayon)
{
    double rayon = coeff_rayon;

	expression_Y = rayon.ToString() + " * (" + expression_Y_save+
        " * cos(("+expression_X_save+" - "+
        MINX_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFX_save.ToString()+") - "+
        expression_Z_save+" * sin(("+expression_X_save+" - "+
        MINX_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFX_save.ToString()+"))  ";
    
	expression_Z = rayon.ToString()+" * ("+expression_Y_save+
        " * sin(("+expression_X_save+" - "+
        MINX_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFX_save.ToString()+") + "+
        expression_Z_save+" * cos(("+expression_X_save+" - "+
        MINX_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFX_save.ToString()+"))  ";    
    
    expression_X = expression_X_save;
	fct_calcul3();	
}  

void Model3D::twistey(double nb_twiste, double coeff_rayon)
{
    double rayon = coeff_rayon;

    expression_X = rayon.ToString()+" * ("+expression_X_save+
        " * cos(("+expression_Y_save+" - "+
        MINY_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFY_save.ToString()+") - "+
        expression_Z_save+" * sin(("+expression_Y_save+" - "+
        MINY_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFY_save.ToString()+"))  ";
    
    expression_Z = rayon.ToString()+" * ("+expression_X_save+
        " * sin(("+expression_Y_save+" - "+
        MINY_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFY_save.ToString()+") + "+
        expression_Z_save+" * cos(("+expression_Y_save+" - "+
        MINY_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFY_save.ToString()+"))  ";    
    
    expression_Y = expression_Y_save;
    fct_calcul3();	
}  

  
void Model3D::twistez(double nb_twiste, double coeff_rayon)
{
    double rayonz = coeff_rayon;

	expression_X = rayonz.ToString()+" * ("+expression_X_save+
        " * cos(("+expression_Z_save+" - "+
        MINZ_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFZ_save.ToString()+") - "+
        expression_Y_save+" * sin(("+expression_Z_save+" - "+
        MINZ_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFZ_save.ToString()+"))  ";
    
	expression_Y = rayonz.ToString()+" * ("+expression_X_save+
        " * sin(("+expression_Z_save+" - "+
        MINZ_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFZ_save.ToString()+") + "+
        expression_Y_save+" * cos(("+expression_Z_save+" - "+
        MINZ_save.ToString()+")*2*pi*"+nb_twiste.ToString()+"/"+
        DIFZ_save.ToString()+"))  ";    
    
    expression_Z = expression_Z_save;
    fct_calcul3();	
}  

void Model3D::scalex(int coeff)
{
    newscalex = (double)coeff/10.0; 
    
    if(newscalex != 1)
        expression_X = newscalex.ToString()+"*("+expression_X_save+")";
    else expression_X = expression_X_save;
	
    if(newscaley != 1)
        expression_Y = newscaley.ToString()+"*("+expression_Y_save+")";
    else expression_Y = expression_Y_save;
    
    if(newscalez != 1)
        expression_Z = newscalez.ToString()+"*("+expression_Z_save+")";
    else expression_Z = expression_Z_save;
        
    fct_calcul3();        
}

void Model3D::scaley(int coeff)
{
    newscaley = (double)coeff/10.0; 
    
    if(newscalex != 1)
        expression_X = newscalex.ToString()+"*("+expression_X_save+")";
    else expression_X = expression_X_save;
	
    if(newscaley != 1)
        expression_Y = newscaley.ToString()+"*("+expression_Y_save+")";
    else expression_Y = expression_Y_save;
    
    if(newscalez != 1)
        expression_Z = newscalez.ToString()+"*("+expression_Z_save+")";
    else expression_Z = expression_Z_save;
  
    fct_calcul3();
}

void Model3D::scalez(int coeff)
{
    newscalez = (double)coeff/10.0; 
    
    if(newscalex != 1)
		expression_X = newscalex.ToString()+"*("+expression_X_save+")";
    else expression_X = expression_X_save;
	
    if(newscaley != 1)
        expression_Y = newscaley.ToString()+"*("+expression_Y_save+")";
    else expression_Y = expression_Y_save;
    
    if(newscalez != 1)
        expression_Z = newscalez.ToString()+"*("+expression_Z_save+")";
    else expression_Z = expression_Z_save;
    
       
    fct_calcul3();
}

void Model3D::initialisecoeffscale()
{
    newscalex = newscaley = newscalez = 1; 
}
      

	   
void Model3D::initialiser_parametres3()
{
    nb_ligne = nb_licol; 
    nb_colone = nb_licol;
	//largeur_fenetre = hauteur_fenetre = 650; 
	demi_hauteur = hauteur_fenetre/2;
	demi_largeur = largeur_fenetre/2;

    Oprime[0]=(double)0.0;
    Oprime[1]=(double)0.0;
    Oprime[2]=(double)800.0;
    D=460; 
    
	coefficient =  1.0f ;
	mesh = 1, box = 1; clipping =1; morph_param = 1; step = 0.1;
	DefineNewFct = -1, infos =1, latence = 30; implicitdef = -1; 
	fourdimshapes = -1; fivedimshapes = -1; sixdimshapes = -1; 
	sphericaldef = -1; cylindricaldef = -1;
	//zbuffer_active_ok = -1;
	zbuffer_active_ok = 1;
//	zbuffer_quality   = 5; //maximum

//	zbuffer_quality   = 5; //maximum
	
    there_is_condition = -1; // No condition imposed to the ploting
	condition_expression = "((x^2 + y^2) > 0.05) & ((t*x+y+z) > -1) ";
    precision = 10; //used for calculating points to replace hidden points
	                //equivalent to a grid with (nb_ligne * precision) points
	there_is_hidden_points = -1; // there is no hidden points
	draw_hidden_poly_and_nonhidden = -1;
	draw_cond_mesh = 1;
	draw_poly_normals    = -1;
	two_separate_objects = -1;  // we have one object
	change_first_object  =  1;  // draw first object
	change_second_object =  1;  // draw second object
	showhelp = -1;
	supershape = -1;
	
	center[0] = center[1] = center[2] = 0;
    exterior_surface = 1; 
	interior_surface = 1;
	independantwindow = -1;
	tetaxy = 3; tetaxz = 3; tetayz = 3; 
	tetaxw = 3; tetayw = 3; tetazw = 3; 
	tetaxt = 3; tetayt = 3; tetazt = 3; tetawt = 3;
	tetaxs = 3; tetays = 3; tetazs = 3; tetaws = 3; tetats = 3; 
	tetaxy_ok = -1; tetaxz_ok = -1; tetayz_ok = -1;
	tetaxw_ok = -1; tetayw_ok = -1; tetazw_ok = -1;
	tetaxt_ok = -1; tetayt_ok = -1; tetazt_ok = -1; tetawt_ok = -1;
	tetaxs_ok = -1; tetays_ok = -1; tetazs_ok = -1; tetaws_ok = -1; tetats_ok = -1;

	// initialisation des matrices 3D
    mat = gcnew Matrix3D();
	mat_first_obj  = gcnew Matrix3D();
	mat_second_obj = gcnew Matrix3D();
	mat_rotation_first_save   = gcnew Matrix3D();
	mat_rotation_second_save  = gcnew Matrix3D();
        
	mat_rotation      = gcnew Matrix3D();
    mat_rotation_save = gcnew Matrix3D();
    mat_homothetie    = gcnew Matrix3D();
    mat_translation   = gcnew Matrix3D();
    mat_inversetranslation = gcnew Matrix3D();
	
	// initialisation des matrices 4D
    mat4D                    = gcnew Matrix4D();
    mat_rotation4D           = gcnew Matrix4D();
    mat_rotation_save4D      = gcnew Matrix4D();
    mat_homothetie4D         = gcnew Matrix4D();
    mat_translation4D        = gcnew Matrix4D();
    mat_inversetranslation4D = gcnew Matrix4D();
	
	// initialisation des matrices 5D
    mat5D                    = gcnew Matrix5D();
    mat_rotation5D           = gcnew Matrix5D();
    mat_rotation_save5D      = gcnew Matrix5D();
    mat_homothetie5D         = gcnew Matrix5D();
    mat_translation5D        = gcnew Matrix5D();
    mat_inversetranslation5D = gcnew Matrix5D();	
	
	// initialisation des matrices 6D
    mat6D                    = gcnew Matrix6D();
    mat_rotation6D           = gcnew Matrix6D();
    mat_rotation_save6D      = gcnew Matrix6D();
    mat_homothetie6D         = gcnew Matrix6D();
    mat_translation6D        = gcnew Matrix6D();
    mat_inversetranslation6D = gcnew Matrix6D();	
}

void Model3D::initialiser_palette()
{
    palette_front_face = gcnew array<Brush^>(300);
	palette_back_face  = gcnew array<Brush^>(300);
    palette_cond_face  = gcnew array<Brush^>(300);

    for(int i=0; i<256; i++)
    {
        double coef = (double)i/256;

        if(fronttrans == 1)
		{
         
			palette_front_face[i] = gcnew HatchBrush(HatchStyle::DottedDiamond, 
				Color::FromArgb((int)((double)(frontsurfr)*coef),
				(int)((double)(frontsurfg)*coef),
				(int)((double)(frontsurfb)*coef)));
		}
		else
		{
			palette_front_face[i] = gcnew SolidBrush(
				Color::FromArgb((int)((double)(frontsurfr)*coef),
				(int)((double)(frontsurfg)*coef),
				(int)((double)(frontsurfb)*coef)));
		}

        if(backtrans  == 1)
		{
			palette_back_face[i] =  gcnew HatchBrush(HatchStyle::DottedDiamond,
				Color::FromArgb((int)((double)(backsurfr)*coef),
				(int)((double)(backsurfg)*coef),
				(int)((double)(backsurfb)*coef)));
		}
		else
		{
			palette_back_face[i] =  gcnew SolidBrush(Color::FromArgb((int)((double)(backsurfr)*coef),
                                              (int)((double)(backsurfg)*coef),
                                              (int)((double)(backsurfb)*coef)));
		}

		palette_cond_face[i] = gcnew SolidBrush(Color::FromArgb((int)((double)(250)*coef),0,0));
    }

	/// <newCode>
	structFirstHalfMat->Ambient =Color::FromArgb(frontsurfr, frontsurfg, frontsurfb);
	structFirstHalfMat->Diffuse = Color::FromArgb(frontsurfr, frontsurfg, frontsurfb);
	structSecondHalfMat->Ambient = Color::FromArgb(backsurfr, backsurfg, backsurfb);
	structSecondHalfMat->Diffuse = Color::FromArgb(backsurfr, backsurfg, backsurfb);
	outlineClrBlend = Color::FromArgb(backgroundr, backgroundg, backgroundb);
	/// </newCode>
}
 
void Model3D::fct_bouton_gauche3 ()
{
    rotation3();
    calcul_points3();
}

void Model3D::fct_bouton_milieu3 ()
{
    translation3();
    calcul_points3();	
}

void Model3D::fct_bouton_droit3 ()
{
    homothetie3();
    calcul_points3();	
}

void Model3D::fct_bouton_Anim3 ()
{
    rotation3();
    calcul_points3(); // On applique la rotation et l'homothetie	
}

void Model3D::fct_bouton_Morph3()
{
	initialiser_parseur3();
	calcul_objet3();  // Utilisation du parseur..
	boite_englobante3(); 
	normalisation3(); // Calcul des normales
	calcul_points3(); // On applique la rotation et l'homothetie

	/// <newCode>
	BuildDxBuffers();
	/// </newCode>
}

void Model3D::fct_bouton_AnimMorph()
{
	initialiser_parseur3();
    rotation3();
	calcul_objet3();  // Utilisation du parseur..
	boite_englobante3(); 
	normalisation3(); // Calcul des normales
	calcul_points3(); // On applique la rotation et l'homothetie	

	/// <newCode>
	BuildDxBuffers();
	/// </newCode>
}

void Model3D::fct_calcul3 ()
{
	initialiser_parseur3();
	parse_expression();
	calcul_objet3();  // Utilisation du parseur..
	boite_englobante3(); 
	normalisation3(); // Calcul des normales
	calcul_points3(); // On applique la rotation et l'homothetie	

	/// <newCode>
	BuildDxBuffers();
	/// </newCode>
}

void Model3D::homothetie3()
{
    mat_homothetie->unit();
    mat_homothetie->xx = mat_homothetie->yy = mat_homothetie->zz = coefficient;

    // Application a la matrice principale:

    mat->mult(mat_homothetie);

	/// <newCode>
	UpdateDxMatrix();
	/// </newcode>
}

void Model3D::translation3()
{
    mat_translation->unit();
    translatex = mat_translation->xo = angley;
    translatey = mat_translation->yo = anglex;
    mat_translation->zo = 0 ;

    // Application a la matrice principale 
    mat->mult(mat_translation);

	/// <newCode>
	UpdateDxMatrix();
	/// </newCode>
}

void Model3D::rotation3()
{
    mat_rotation->unit();

    // Construction de la matrice de trnsformation
    mat_rotation->xrot(anglex);
    mat_rotation->yrot(angley);
    //we have to invert the translation before applaying the rotation

    mat_inversetranslation->unit();
    mat_inversetranslation->xo = -center[0];
    mat_inversetranslation->yo = -center[1];
    mat_inversetranslation->zo = -center[2];

    mat->mult(mat_inversetranslation);
    // On applique cette transformation a la matrice principale "mat"
    mat->mult(mat_rotation);
    
    // Now we return the object to it's first place..
    mat_inversetranslation->unit();
    mat_inversetranslation->xo = center[0];
    mat_inversetranslation->yo = center[1];
    mat_inversetranslation->zo = center[2];

    mat->mult(mat_inversetranslation);
    mat_rotation_save->mult(mat_rotation);

	/// <newCode>
	UpdateDxMatrix();
	/// </newCode>
}

/*****************************************************************************/
/*****************************************************************************/ 
void  Model3D::rotation4()
{
    mat_rotation4D->unit();         
    //tetaxy = 4;tetaxz = 4;tetayz = 4;// for testing
    //tetaxw = 4;tetayw = 4;tetazw = 4;// for testing
    
    // Construction de la matrice de trnsformation
    if(tetaxy_ok == 1)    mat_rotation4D->xyrot(tetaxy);
    if(tetaxz_ok == 1)    mat_rotation4D->xzrot(tetaxz);
    if(tetayz_ok == 1)    mat_rotation4D->yzrot(tetayz);
    
    if(tetaxw_ok == 1)    mat_rotation4D->xwrot(tetaxw);
    if(tetayw_ok == 1)    mat_rotation4D->ywrot(tetayw);
    if(tetazw_ok == 1)    mat_rotation4D->zwrot(tetazw);    

    // On applique cette transformation a la matrice principale "mat"
    mat4D->mult(mat_rotation4D);		
}
	
void  Model3D::rotation5()
{	
    // Second version of 5D rotation
    // here we don't use special mat_rotation5D
    if(tetaxy_ok == 1)    mat5D->xyrot(tetaxy);
    if(tetaxz_ok == 1)    mat5D->xzrot(tetaxz);
    if(tetayz_ok == 1)    mat5D->yzrot(tetayz);
    
    if(tetaxw_ok == 1)    mat5D->xwrot(tetaxw);
    if(tetayw_ok == 1)    mat5D->ywrot(tetayw);
    if(tetazw_ok == 1)    mat5D->zwrot(tetazw); 
      
    if(tetaxt_ok == 1)    mat5D->xtrot(tetaxt);
    if(tetayt_ok == 1)    mat5D->ytrot(tetayt);   
    if(tetazt_ok == 1)    mat5D->ztrot(tetazt);
    if(tetawt_ok == 1)    mat5D->wtrot(tetawt);
	
}

void  Model3D::rotation6()
{	
    // Second version of 5D rotation
    // here we don't use special mat_rotation5D
    if(tetaxy_ok == 1)    mat6D->xyrot(tetaxy);
    if(tetaxz_ok == 1)    mat6D->xzrot(tetaxz);
    if(tetayz_ok == 1)    mat6D->yzrot(tetayz);
    
    if(tetaxw_ok == 1)    mat6D->xwrot(tetaxw);
    if(tetayw_ok == 1)    mat6D->ywrot(tetayw);
    if(tetazw_ok == 1)    mat6D->zwrot(tetazw); 
      
    if(tetaxt_ok == 1)    mat6D->xtrot(tetaxt);
    if(tetayt_ok == 1)    mat6D->ytrot(tetayt);   
    if(tetazt_ok == 1)    mat6D->ztrot(tetazt);
    if(tetawt_ok == 1)    mat6D->wtrot(tetawt);
      
    if(tetaxs_ok == 1)    mat6D->xsrot(tetaxs);
    if(tetays_ok == 1)    mat6D->ysrot(tetays);   
    if(tetazs_ok == 1)    mat6D->zsrot(tetazs);
    if(tetaws_ok == 1)    mat6D->wsrot(tetaws);    
    if(tetats_ok == 1)    mat6D->tsrot(tetats);	  
}

void Model3D::fct_bouton_Anim4 ()
{
    rotation4();
    calcul_points4();         // On applique la rotation 4D
    //boite_englobante4D();// pas necessaire...donne resultat bizarre!!!            
    project_4D_to_3D();  
    boite_englobante3(); //<--try to find something else more convenient
    //normalisation3(); // Calcul des normales...doit etre fait prochainement en 4D
    calcul_points3(); // On applique la rotation et l'homothetie            	
    mat4D->unit(); 
}

void Model3D::fct_bouton_Anim5 ()
{
    rotation5();
    calcul_points5();         // On applique la rotation 5D          
    project_5D_to_4D();  
    project_4D_to_3D();
    boite_englobante3(); //<--try to find something else more convenien   
    calcul_points3(); // On applique la rotation et l'homothetie  (en 3D)          	
    mat5D->unit(); 
}

void Model3D::fct_bouton_Anim6 ()
{
    rotation6();
    calcul_points6(); // On applique la rotation 5D  
    project_6D_to_5D();                
    project_5D_to_4D();  
    project_4D_to_3D();
    boite_englobante3(); //<--try to find something else more convenien   
    calcul_points3(); // On applique la rotation et l'homothetie  (en 3D)          	
    mat6D->unit();                   
}

void  Model3D::calcul_points4()
{
    int i,j;

    // Changement de coordonnees des points selon les
    // angles angex et angley

    for (i=0; i < nb_ligne  ; i++)
        for (j=0; j < nb_colone   ; j++)
        {      
            tp1 = shape4D[i*4    , j];
            tp2 = shape4D[i*4 + 1, j];
            tp3 = shape4D[i*4 + 2, j];
            tp4 = shape4D[i*4 + 3, j];
        
            shape4D[i*4  , j] = mat4D->xx*tp1 + mat4D->xy*tp2 + mat4D->xz*tp3 + mat4D->xw*tp4 + mat4D->xo;
            shape4D[i*4+1, j] = mat4D->yx*tp1 + mat4D->yy*tp2 + mat4D->yz*tp3 + mat4D->yw*tp4 + mat4D->yo;
            shape4D[i*4+2, j] = mat4D->zx*tp1 + mat4D->zy*tp2 + mat4D->zz*tp3 + mat4D->zw*tp4 + mat4D->zo;
            shape4D[i*4+3, j] = mat4D->wx*tp1 + mat4D->wy*tp2 + mat4D->wz*tp3 + mat4D->ww*tp4 + mat4D->wo;       
        }
}

void  Model3D::calcul_points5()
{
    int i,j;

    // Changement de coordonnees des points selon les
    // angles angex et angley

    for (i=0; i < nb_ligne  ; i++)
        for (j=0; j < nb_colone   ; j++)
        {
            tp1 = shape5D[i*5    , j];
            tp2 = shape5D[i*5 + 1, j];
            tp3 = shape5D[i*5 + 2, j];
            tp4 = shape5D[i*5 + 3, j];
            tp5 = shape5D[i*5 + 4, j];
	
            shape5D[i*5  , j] = mat5D->xx*tp1 + mat5D->xy*tp2 + mat5D->xz*tp3 + mat5D->xw*tp4 + mat5D->xt*tp5 + mat5D->xo;
            shape5D[i*5+1, j] = mat5D->yx*tp1 + mat5D->yy*tp2 + mat5D->yz*tp3 + mat5D->yw*tp4 + mat5D->yt*tp5 + mat5D->yo;
            shape5D[i*5+2, j] = mat5D->zx*tp1 + mat5D->zy*tp2 + mat5D->zz*tp3 + mat5D->zw*tp4 + mat5D->zt*tp5 + mat5D->zo;
            shape5D[i*5+3, j] = mat5D->wx*tp1 + mat5D->wy*tp2 + mat5D->wz*tp3 + mat5D->ww*tp4 + mat5D->wt*tp5 + mat5D->wo;
            shape5D[i*5+4, j] = mat5D->tx*tp1 + mat5D->ty*tp2 + mat5D->tz*tp3 + mat5D->tw*tp4 + mat5D->tt*tp5 + mat5D->to;       
        }
}

void  Model3D::calcul_points6()
{
    int i,j;

    // Changement de coordonnees des points selon les
    // angles angex et angley

    for (i=0; i < nb_ligne  ; i++)
        for (j=0; j < nb_colone   ; j++)
        {
            tp1 = shape6D[i*6    , j];
            tp2 = shape6D[i*6 + 1, j];
            tp3 = shape6D[i*6 + 2, j];
            tp4 = shape6D[i*6 + 3, j];
            tp5 = shape6D[i*6 + 4, j];
            tp6 = shape6D[i*6 + 5, j];
	
            shape6D[i*6  , j] = mat6D->xx*tp1 + mat6D->xy*tp2 + mat6D->xz*tp3 + mat6D->xw*tp4 + mat6D->xt*tp5 + mat6D->xs*tp6 + mat6D->xo;
            shape6D[i*6+1, j] = mat6D->yx*tp1 + mat6D->yy*tp2 + mat6D->yz*tp3 + mat6D->yw*tp4 + mat6D->yt*tp5 + mat6D->ys*tp6 + mat6D->yo;
            shape6D[i*6+2, j] = mat6D->zx*tp1 + mat6D->zy*tp2 + mat6D->zz*tp3 + mat6D->zw*tp4 + mat6D->zt*tp5 + mat6D->zs*tp6 + mat6D->zo;
            shape6D[i*6+3, j] = mat6D->wx*tp1 + mat6D->wy*tp2 + mat6D->wz*tp3 + mat6D->ww*tp4 + mat6D->wt*tp5 + mat6D->ws*tp6 + mat6D->wo;
            shape6D[i*6+4, j] = mat6D->tx*tp1 + mat6D->ty*tp2 + mat6D->tz*tp3 + mat6D->tw*tp4 + mat6D->tt*tp5 + mat6D->ts*tp6 + mat6D->to;
            shape6D[i*6+5, j] = mat6D->sx*tp1 + mat6D->sy*tp2 + mat6D->sz*tp3 + mat6D->sw*tp4 + mat6D->st*tp5 + mat6D->ss*tp6 + mat6D->so;      
        }
}

void  Model3D::normalisation3()
{
    int i,j;	
    // Construction des normales : (premiere methode )


    for (j=0; j < nb_colone -1   ; j++)
        for (i=0; i < nb_ligne -1  ; i++) {
     	     	
            caa  = Tre2[(i+1)*3+1, j  ] - Tre2[i*3+1, j]; 
            bab  = Tre2[i*3    +2, j+1] - Tre2[i*3+2, j];
            cab  = Tre2[(i+1)*3+2, j  ] - Tre2[i*3+2, j];
            baa  = Tre2[i*3    +1, j+1] - Tre2[i*3+1, j];
            ba   = Tre2[i*3      , j+1] - Tre2[i*3  , j];
            ca   = Tre2[(i+1)*3  , j  ] - Tre2[i*3  , j];
    	
            Nor2[i*3  , j] = caa*bab - cab*baa;
            Nor2[i*3+1, j] = cab*ba  - ca*bab;
            Nor2[i*3+2, j] = ca*baa  - caa*ba;


			double b4 = (double)Math::Sqrt((Nor2[i*3  , j]*Nor2[i*3  , j]) + 
                                     (Nor2[i*3+1, j]*Nor2[i*3+1, j]) +
                                     (Nor2[i*3+2, j]*Nor2[i*3+2, j])  );


            // This must be changed...
            if(b4 > 0.00000001) 
			{
				Nor2[i*3  , j] /= b4;
                Nor[i*3  , j]= Nor2[i*3  , j];

				Nor2[i*3+1, j]/=b4;
                Nor[i*3+1, j]= Nor2[i*3+1, j];

 				Nor2[i*3+2, j]/=b4;
                Nor[i*3+2, j]= Nor2[i*3+2, j];
            }

        }


    i = nb_ligne -1;
    for (j=0; j < nb_colone -1   ; j++) 
	{
		Nor2[i*3  , j] = Nor2[(i-1)*3  , j];
        Nor[i*3  , j] = Nor2[i*3  , j];

		Nor2[i*3+1, j] = Nor2[(i-1)*3+1, j];
        Nor[i*3+1, j] = Nor2[i*3+1, j];

		Nor2[i*3+2, j] = Nor2[(i-1)*3+2, j];
        Nor[i*3+2, j] = Nor2[i*3+2, j];
    }

    j =nb_colone -1;
    for (i=0; i < nb_ligne -1  ; i++) 
	{
		Nor2[i*3  , j]= Nor2[i*3  , j-1];
        Nor[i*3  , j]= Nor2[i*3  , j];
    
		Nor2[i*3+1, j]= Nor2[i*3+1, j-1];
		Nor[i*3+1, j]= Nor2[i*3+1, j];

		Nor2[i*3+2, j]= Nor2[i*3+2, j-1];
        Nor[i*3+2, j]= Nor2[i*3+2, j];
    }
}

void  Model3D::calcul_points3()
{
    int i,j;
    // pour savoir ou se trouve le centre de l'objet

    moitie_colone = (int)(nb_colone/2);
 
 
    center[0] =  mat->xo;
    center[1] =  mat->yo;
    center[2] =  mat->zo;

    // Changement de coordonnees des points selon les
    // angles angex et angley
    if(two_separate_objects == -1 ) {
        mat_first_obj = mat_second_obj = mat;
        mat_rotation_first_save= mat_rotation_second_save = mat_rotation_save;


        for (i=0; i < nb_ligne  ; i++)
            for (j=0; j < nb_colone   ; j++){
                tp1 = Tre2[i*3    , j];
                tp2 = Tre2[i*3 + 1, j];
                tp3 = Tre2[i*3 + 2, j];
                Tre[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                Tre[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                Tre[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;        
            }
                                 
        // here we are testing if there is much more points to transforme
        // in the case we have a conditional equation...
        if( there_is_condition == 1) {
     
            for (i=0; i < nb_ligne  ; i++)
                for (j=0; j < nb_colone   ; j++){
                    if(hidden_points[i, j] == 0){
                        //DR
                        tp1 = DR2[i*3    , j];
                        tp2 = DR2[i*3 + 1, j];
                        tp3 = DR2[i*3 + 2, j];
                        DR[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                        DR[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                        DR[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;        
                                 
                        //DL
                        tp1 = DL2[i*3    , j];
                        tp2 = DL2[i*3 + 1, j];
                        tp3 = DL2[i*3 + 2, j];
                        DL[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                        DL[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                        DL[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;                                  
                                 
                        //HR
                        tp1 = HR2[i*3    , j];
                        tp2 = HR2[i*3 + 1, j];
                        tp3 = HR2[i*3 + 2, j];
                        HR[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                        HR[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                        HR[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;                                  
                                 
                        //HL
                        tp1 = HL2[i*3    , j];
                        tp2 = HL2[i*3 + 1, j];
                        tp3 = HL2[i*3 + 2, j];
                        HL[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                        HL[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                        HL[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;                                 
                    }    
                } // End if(hidden_points[i, j] == 0)... 
        } // End  if( there_is_condition == 1)
     
     
 
 
 
        //Normales
        for (i=0; i < nb_ligne  ; i++)                                                                       
            for (j=0; j < nb_colone    ; j++) {
                tp1 = Nor2[i*3    , j];
                tp2 = Nor2[i*3 + 1, j];
                tp3 = Nor2[i*3 + 2, j];
                Nor[i*3  , j] = mat_rotation_save->xx*tp1 + mat_rotation_save->xy*tp2 +mat_rotation_save->xz*tp3 + mat_rotation_save->xo;
                Nor[i*3+1, j] = mat_rotation_save->yx*tp1 + mat_rotation_save->yy*tp2 +mat_rotation_save->yz*tp3 + mat_rotation_save->yo;
                Nor[i*3+2, j] = mat_rotation_save->zx*tp1 + mat_rotation_save->zy*tp2 +mat_rotation_save->zz*tp3 + mat_rotation_save->zo;   

            }
 
 
 
 
    }
    else  {
 
        if(change_first_object == 1) {

            for (i=0; i < nb_ligne  ; i++)
                for (j=0; j < moitie_colone -1  ; j++){
                    tp1 = Tre2[i*3    , j];
                    tp2 = Tre2[i*3 + 1, j];
                    tp3 = Tre2[i*3 + 2, j];
                    Tre[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                    Tre[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                    Tre[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;        
                }

            // Normales 
            for (i=0; i < nb_ligne  ; i++)                                                                       
                for (j=0; j < moitie_colone -1   ; j++) {
                    tp1 = Nor2[i*3    , j];
                    tp2 = Nor2[i*3 + 1, j];
                    tp3 = Nor2[i*3 + 2, j];
                    Nor[i*3  , j] = mat_rotation_save->xx*tp1 + mat_rotation_save->xy*tp2 +mat_rotation_save->xz*tp3 + mat_rotation_save->xo;
                    Nor[i*3+1, j] = mat_rotation_save->yx*tp1 + mat_rotation_save->yy*tp2 +mat_rotation_save->yz*tp3 + mat_rotation_save->yo;
                    Nor[i*3+2, j] = mat_rotation_save->zx*tp1 + mat_rotation_save->zy*tp2 +mat_rotation_save->zz*tp3 + mat_rotation_save->zo;   

                }				 
				 
			 
				 	
            //mat_first_obj = mat;
            //mat_rotation_first_save= mat_rotation_save;

        }
        if(change_second_object == 1){
            for (i=0; i < nb_ligne  ; i++)
                for (j=moitie_colone -1; j < nb_colone ; j++){
                    tp1 = Tre2[i*3    , j];
                    tp2 = Tre2[i*3 + 1, j];
                    tp3 = Tre2[i*3 + 2, j];
                    Tre[i*3  , j] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
                    Tre[i*3+1, j] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
                    Tre[i*3+2, j] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;        
                }
	
            // Normales 
            for (i=0; i < nb_ligne  ; i++)                                                                       
                for (j = moitie_colone -1; j < nb_colone    ; j++) {
                    tp1 = Nor2[i*3    , j];
                    tp2 = Nor2[i*3 + 1, j];
                    tp3 = Nor2[i*3 + 2, j];
                    Nor[i*3  , j] = mat_rotation_save->xx*tp1 + mat_rotation_save->xy*tp2 +mat_rotation_save->xz*tp3 + mat_rotation_save->xo;
                    Nor[i*3+1, j] = mat_rotation_save->yx*tp1 + mat_rotation_save->yy*tp2 +mat_rotation_save->yz*tp3 + mat_rotation_save->yo;
                    Nor[i*3+2, j] = mat_rotation_save->zx*tp1 + mat_rotation_save->zy*tp2 +mat_rotation_save->zz*tp3 + mat_rotation_save->zo;
                }				 
				 				 
            //mat_second_obj = mat;
            //mat_rotation_second_save = mat_rotation_save;
	
        }		

    }  
      
                                 
    // Boite englobante
    
    for (i=0; i < 8  ; i++) {

        tp1 = boiteenglobante2[i*3    ];
        tp2 = boiteenglobante2[i*3 + 1];
        tp3 = boiteenglobante2[i*3 + 2];
        boiteenglobante[i*3    ] = mat->xx*tp1 + mat->xy*tp2 +mat->xz*tp3 + mat->xo;
        boiteenglobante[i*3 + 1] = mat->yx*tp1 + mat->yy*tp2 +mat->yz*tp3 + mat->yo;
        boiteenglobante[i*3 + 2] = mat->zx*tp1 + mat->zy*tp2 +mat->zz*tp3 + mat->zo;        

    }                         
}


void Model3D::initialiser_parseur3()
{
    myParser->AddConstant ("pi", 3.14159265);
    myParserX->AddConstant("pi", 3.14159265);
    myParserY->AddConstant("pi", 3.14159265);
    myParserZ->AddConstant("pi", 3.14159265);
    myParserW->AddConstant("pi", 3.14159265);
    myParserT->AddConstant("pi", 3.14159265);
    myParserS->AddConstant("pi", 3.14159265);
    
    
    myParserX_2->AddConstant("pi", 3.14159265);
    myParserY_2->AddConstant("pi", 3.14159265);
    myParserZ_2->AddConstant("pi", 3.14159265);  
    
    myParser_spherical->AddConstant("pi", 3.14159265);
    myParser_cylindrical->AddConstant("pi", 3.14159265);
    
    myParser_condition->AddConstant("pi", 3.14159265);    
}


/*****************************************************************************/
/** This function must be rewriten for more clarity..*************************/
/*****************************************************************************/ 

void  Model3D::parse_expression()
{
    array<double>^ vals = { 0,0,0,0,0,0}; // arbitrary values

    if(there_is_condition == 1)
    {
        myParser_condition->AddFunction("k", f1);
        myParser_condition->Parse(condition_expression, "u,v,t,x,y,z", false);                    
    }

    if ( supershape == 1)
    {
        myParser->Parse(sm1, "u,v,t,x,y,z", false);
        m1 = myParser->Eval(vals); 
     
        myParser->Parse(sm2, "u,v,t,x,y,z", false);
        m2 = myParser->Eval(vals);     
     
        myParser->Parse(sa_1, "u,v,t,x,y,z", false);
        a_1 = myParser->Eval(vals);
    
        myParser->Parse(sa_2, "u,v,t,x,y,z", false);
        a_2 = myParser->Eval(vals);    
         
        myParser->Parse(sb_1, "u,v,t,x,y,z", false);
        b_1 = myParser->Eval(vals);
    
        myParser->Parse(sb_2, "u,v,t,x,y,z", false);
        b_2 = myParser->Eval(vals);

        myParser->Parse(sn1_1, "u,v,t,x,y,z", false);
        n1_1 = myParser->Eval(vals);
    
        myParser->Parse(sn1_2, "u,v,t,x,y,z", false);
        n1_2 = myParser->Eval(vals);
    
        myParser->Parse(sn2_1, "u,v,t,x,y,z", false);
        n2_1 = myParser->Eval(vals);
    
        myParser->Parse(sn2_2, "u,v,t,x,y,z", false);
        n2_2 = myParser->Eval(vals);    
    
        myParser->Parse(sn3_1, "u,v,t,x,y,z", false);
        n3_1 = myParser->Eval(vals);
    
        myParser->Parse(sn3_2, "u,v,t,x,y,z", false);
        n3_2 = myParser->Eval(vals);
    

        myParser->Parse(inf_u, "u,v,t,x,y", false);
        u_inf = myParser->Eval(vals);
        myParser->Parse(sup_u, "u,v,t,x,y", false);
        u_sup = myParser->Eval(vals);
        dif_u = u_sup - u_inf;

        myParser->Parse(inf_v, "u,v,t,x,y", false);
        v_inf = myParser->Eval(vals);
        myParser->Parse(sup_v, "u,v,t,x,y", false);
        v_sup = myParser->Eval(vals);
        dif_v = v_sup - v_inf;
    }
    else if(  implicitdef != 1 )
    {     
        if(DefineNewFct ==1){
            f1->Parse(newfct, "u,v,t,x,y", false);
            myParserX->AddFunction("k", f1);
            myParserY->AddFunction("k", f1);
            myParserZ->AddFunction("k", f1);
    
            myParserX_2->AddFunction("k", f1);
            myParserY_2->AddFunction("k", f1);
            myParserZ_2->AddFunction("k", f1);
    
            myParser_spherical->AddFunction("k", f1);
            myParser_cylindrical->AddFunction("k", f1);      
        }      
        myParser->Parse(inf_u, "u,v,t,x,y", false);
        u_inf = myParser->Eval(vals);
        myParser->Parse(sup_u, "u,v,t,x,y", false);
        u_sup = myParser->Eval(vals);
        dif_u = u_sup - u_inf;

        myParser->Parse(inf_v, "u,v,t,x,y", false);
        v_inf = myParser->Eval(vals);
        myParser->Parse(sup_v, "u,v,t,x,y", false);
        v_sup = myParser->Eval(vals);
        dif_v = v_sup - v_inf;
     
        myParserX->Parse(expression_X, "u,v,t,x,y", false);
        myParserY->Parse(expression_Y, "u,v,t,x,y", false);
        myParserZ->Parse(expression_Z, "u,v,t,x,y", false);

        // if two system activated
 
        if(two_system == 1) {
            myParser->Parse(inf_u_2, "u,v,t,x,y", false);
            u_inf_2 = myParser->Eval(vals);
            myParser->Parse(sup_u_2, "u,v,t,x,y", false);
            u_sup_2 = myParser->Eval(vals);
            dif_u_2 = u_sup_2 - u_inf_2;

            myParser->Parse(inf_v_2, "u,v,t,x,y", false);
            v_inf_2 = myParser->Eval(vals);
            myParser->Parse(sup_v_2, "u,v,t,x,y", false);
            v_sup_2 = myParser->Eval(vals);
            dif_v_2 = v_sup_2 - v_inf_2;
  
            myParserX_2->Parse(expression_X_2, "u,v,t,x,y", false);
            myParserY_2->Parse(expression_Y_2, "u,v,t,x,y", false);
            myParserZ_2->Parse(expression_Z_2, "u,v,t,x,y", false);
        }
 
 
        // 6D shapes 
        if (sixdimshapes == 1 ) {     
            myParserW->AddFunction("k", f1);    
            myParserW->Parse(expression_W, "u,v,t,x,y", false); 
    
            myParserT->AddFunction("k", f1);    
            myParserT->Parse(expression_T, "u,v,t,x,y", false);  
    
            myParserS->AddFunction("k", f1);    
            myParserS->Parse(expression_S, "u,v,t,x,y", false);              
        }
     
        // 5D shapes 
        if (fivedimshapes == 1 ) {     
            myParserW->AddFunction("k", f1);    
            myParserW->Parse(expression_W, "u,v,t,x,y", false); 
    
            myParserT->AddFunction("k", f1);    
            myParserT->Parse(expression_T, "u,v,t,x,y", false);                
        }
        // 4D shapes 
        else if (fourdimshapes == 1 ) {     
            myParserW->AddFunction("k", f1);    
            myParserW->Parse(expression_W, "u,v,t,x,y", false);          
        }
 

        else if(sphericaldef == 1) // spherical equation  r^n = f(u,v,x,y,z)
        { 
            // we are looking for the parity of the coefficient n
            myParser->Parse(coefficient_n, "u,v,t,x,y", false);
            coefficent_fct_implicite = myParser->Eval(vals);
			coefficent_fct_implicite_parity = Math::IEEERemainder(coefficent_fct_implicite, 2);       
            myParser_spherical->Parse(spherical_expression, "u,v,t,x,y,z", false); // we have one more variable z    
        }
        else if(cylindricaldef == 1) // spherical equation  r^n = f(u,v,x,y,z)
        { 
            // we are looking for the parity of the coefficient n
            myParser->Parse(coefficient_n, "u,v,t,x,y", false);
            coefficent_fct_implicite = myParser->Eval(vals);
			coefficent_fct_implicite_parity = Math::IEEERemainder (coefficent_fct_implicite, 2);       
            myParser_cylindrical->Parse(cylindrical_expression, "u,v,t,x,y,z", false); // we have one more variable z    
        }

    
    }
    // this part is for the implicit equation...
    else
    {

        // We use the borders of the second system to delimite x and y
        myParser->Parse(inf_u, "u,v,t,x,y", false);
        u_inf = myParser->Eval(vals);
        myParser->Parse(sup_u, "u,v,t,x,y", false);
        u_sup = myParser->Eval(vals);
        dif_u = u_sup - u_inf;

        myParser->Parse(inf_v, "u,v,t,x,y", false);
        v_inf = myParser->Eval(vals);
        myParser->Parse(sup_v, "u,v,t,x,y", false);
        v_sup = myParser->Eval(vals);
        dif_v = v_sup - v_inf;
    
        // we are looking for the parity of the coefficient n
        myParser->Parse(coefficient_n, "u,v,t,x,y", false);
        coefficent_fct_implicite = myParser->Eval(vals);
		coefficent_fct_implicite_parity = Math::IEEERemainder (coefficent_fct_implicite, 2);
        // we set the first and second equation to u and v    
    
        myParserX->Parse("u", "u,v,t,x,y", false);
        myParserY->Parse("v", "u,v,t,x,y", false); 
        // Now we construct the third equation for Implicit equation z^n = f(x,y)...
        expression_Z = expression_implicite;
        myParserZ->Parse(expression_Z, "u,v,t,x,y", false);
                        
        // just for more security....
        two_system = -1;
    } 
} 
 
void  Model3D::calcul_objet3()
{
    array<double>^ vals = {0,0,0,0,0,0}; 
    array<double>^ vals2 = {0,0,0,0,0,0};
    double iprime, jprime, valeur_de_fct_implicite;
    double spherical_val, cylindrical_val, U_step, V_step;
    
    // we initialise the hidden_points array so all points are ready to be drawn    
    
    for(int j=0; j < nb_ligne   ; j++)
        for (int i=0; i < nb_colone  ; i++)
            hidden_points[j, i]  = 1;     
    
    there_is_hidden_points = -1;    
        
    // this is for the morph effect...
    if(morph_param >= 0.0)
    {
        vals[2] = vals2[2] = morph_param ;
    }
    else
    {
        vals[2] = vals2[2] = -morph_param ;
    }

    morph_param -= step ;
    if( morph_param <= -1)
        morph_param = 1;

    if ( supershape == 1)
    {
        double r1, r2;
        // Begin First case : n = 1, 3, 5.....  
        //  if(coefficent_fct_implicite_parity == 1)   
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;

            for (int i=0; i < nb_colone   ; i++)
            {
                iprime = (double)i/(double)(nb_colone-1 ) ;
                iprime = iprime * dif_v + v_inf;
				r1 = Math::Pow(
					Math::Pow(Math::Abs(Math::Cos(m1*jprime/4)/a_1), n2_1) + 
					Math::Pow(Math::Abs(Math::Sin(m1*jprime/4)/b_1), n3_1)
                         , -1/n1_1);
   

				r2 = Math::Pow(
					Math::Pow(Math::Abs(Math::Cos(m2*iprime/4)/a_2), n2_2) + 
					Math::Pow(Math::Abs(Math::Sin(m2*iprime/4)/b_2), n3_2)
                         , -1/n1_2);
   
				Tre2[j*3    , i]  = r1 * Math::Cos(jprime) * r2 * Math::Cos(iprime);
				Tre2[j*3 + 1, i]  = r1 * Math::Sin(jprime) * r2 * Math::Cos(iprime);
				Tre2[j*3 + 2, i]  = r2 * Math::Sin(iprime);
            }
        }

        // End of first case : n = 1, 3, 5.....  
    }

    else if (cylindricaldef == 1)
    {   
        // Begin First case : n = 1, 3, 5.....  
        //  if(coefficent_fct_implicite_parity == 1)   
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;
            for (int i=0; i < nb_colone   ; i++)
            {
                iprime = (double)i/(double)(nb_colone-1 ) ;
                iprime = iprime * dif_v + v_inf;
   
                vals[0]=jprime; vals[1]=iprime;   // u = jprime ; v = iprime;

				vals[3] = Math::Cos(jprime) ; // because  X = cos(u)
				vals[4] = Math::Sin(jprime);  // because  Y = sin(u)
                vals[5] = iprime  ;     // because  Z = v
                cylindrical_val = myParser_cylindrical->Eval(vals) ; 
   
                (cylindrical_val  >= 0 ) ?
					cylindrical_val  =  Math::Pow(cylindrical_val, 1/coefficent_fct_implicite) : 
				cylindrical_val  = -Math::Pow(-cylindrical_val, 1/coefficent_fct_implicite);
   
                Tre2[j*3    , i]  = vals[3] * cylindrical_val;
                Tre2[j*3 + 1, i]  = vals[4] * cylindrical_val;
                Tre2[j*3 + 2, i]  = vals[5] ;
            }
        }

        // End of first case : n = 1, 3, 5.....  
    }
    else if (sphericaldef == 1)
    {   
        // Begin First case : n = 1, 3, 5.....  
        //  if(coefficent_fct_implicite_parity == 1)   
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;
            for (int i=0; i < nb_colone   ; i++)
            {
                iprime = (double)i/(double)(nb_colone-1 ) ;
                iprime = iprime * dif_v + v_inf;
   
                vals[0]=jprime; vals[1]=iprime;   // u = jprime ; v = iprime;

   
                //double supershape(double n1, double n2, double n3, int m, double a, double b, double teta)   
				vals[3] = Math::Cos(iprime)*Math::Cos(jprime) ; // because  X = cos(v)*cos(u)
				vals[4] = Math::Sin(iprime)*Math::Cos(jprime);  // because  Y = sin(v)*cos(u)
				vals[5] = Math::Sin(jprime)  ;            // because  Z = sin(u)
                spherical_val = myParser_spherical->Eval(vals) ; 
   
                (spherical_val  >= 0 ) ?
					spherical_val  =  Math::Pow(spherical_val, 1/coefficent_fct_implicite) : 
				spherical_val  = -Math::Pow(-spherical_val, 1/coefficent_fct_implicite);
   
                Tre2[j*3    , i]  = vals[3] * spherical_val;
                Tre2[j*3 + 1, i]  = vals[4] * spherical_val;
                Tre2[j*3 + 2, i]  = vals[5] * spherical_val;
            }
        }
        // End of first case : n = 1, 3, 5.....   
    }
    else if(sixdimshapes == 1)
    {
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;    
            for (int i=0; i < nb_colone   ; i++)
            {
                iprime = (double)i/(double)(nb_colone-1 ) ;
                iprime = iprime * dif_v + v_inf;
   
                vals[0]=jprime; vals[1]=iprime;

                shape6D[j*6    , i]  = vals[3] = myParserX->Eval(vals);
                shape6D[j*6 + 1, i]  = vals[4] = myParserY->Eval(vals);  
                shape6D[j*6 + 2, i]            = myParserZ->Eval(vals);
                shape6D[j*6 + 3, i]            = myParserW->Eval(vals);
                shape6D[j*6 + 4, i]            = myParserT->Eval(vals);
                shape6D[j*6 + 5, i]            = myParserS->Eval(vals);
            }
        }                
   
        boite_englobante6D();
        project_6D_to_5D();
        project_5D_to_4D();                
        project_4D_to_3D();               
                  
    }
    else if(fivedimshapes == 1)
    {
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;    
            for (int i=0; i < nb_colone   ; i++)
            {
                iprime = (double)i/(double)(nb_colone-1 ) ;
                iprime = iprime * dif_v + v_inf;
   
                vals[0]=jprime; vals[1]=iprime;

                shape5D[j*5    , i]  = vals[3] = myParserX->Eval(vals);
                shape5D[j*5 + 1, i]  = vals[4] = myParserY->Eval(vals);  
                shape5D[j*5 + 2, i]            = myParserZ->Eval(vals);
                shape5D[j*5 + 3, i]            = myParserW->Eval(vals);
                shape5D[j*5 + 4, i]            = myParserT->Eval(vals);
            }
        }                
   
        boite_englobante5D();
        project_5D_to_4D();               
        project_4D_to_3D();               
    }
    else if(fourdimshapes == 1)
    {
        for(int j=0; j < nb_ligne   ; j++) {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;    
            for (int i=0; i < nb_colone   ; i++)
            {
                iprime = (double)i/(double)(nb_colone-1 ) ;
                iprime = iprime * dif_v + v_inf;
   
                vals[0]=jprime; vals[1]=iprime;

				shape4D[j*4    , i]  = myParserX->Eval(vals);
                vals[3] = shape4D[j*4    , i];

				shape4D[j*4 + 1, i]  = myParserY->Eval(vals);  
                vals[4] = shape4D[j*4 + 1, i];

                shape4D[j*4 + 2, i]  = myParserZ->Eval(vals);
                shape4D[j*4 + 3, i]  = myParserW->Eval(vals);
            }
        }                
   
        boite_englobante4D();                  
        project_4D_to_3D();               
    }
    else if(two_system == -1)     // Just one system...cooooooooooool
    {
        if(  implicitdef != 1 )
        {
            // in this case we have a simple equation system...
            if(there_is_condition == -1) // No condition to verify
                for(int j=0; j < nb_ligne   ; j++) {
                    jprime = (double)j/(double)(nb_ligne -1 ) ;
                    jprime = jprime * dif_u  + u_inf;

    
                    for (int i=0; i < nb_colone   ; i++)
                    {
                        iprime = (double)i/(double)(nb_colone-1 ) ;
                        iprime = iprime * dif_v + v_inf;
   
                        vals[0]=jprime; vals[1]=iprime;

						Tre2[j*3    , i]  = myParserX->Eval(vals);
                        vals[3] = Tre2[j*3    , i];

						Tre2[j*3 + 1, i]  = myParserY->Eval(vals);
                        vals[4] = Tre2[j*3 + 1, i];
                        Tre2[j*3 + 2, i]  = myParserZ->Eval(vals);
                    }	       
                } // End of if(there_is_condition == -1)

            // Here There is a condition to verify
            else {
                /// Calculate the grid
                for(int j=0; j < nb_ligne   ; j++) {
                    jprime = (double)j/(double)(nb_ligne -1 ) ;
                    jprime = jprime * dif_u  + u_inf;   
                    for (int i=0; i < nb_colone   ; i++) {
                        iprime = (double)i/(double)(nb_colone-1 ) ;
                        iprime = iprime * dif_v + v_inf;
   
                        vals[0]=jprime; vals[1]=iprime;

						Tre2[j*3    , i]  = myParserX->Eval(vals);
                        vals[3] = Tre2[j*3    , i];

						Tre2[j*3 + 1, i]  = myParserY->Eval(vals);
                        vals[4] = Tre2[j*3 + 1, i];

						Tre2[j*3 + 2, i]  = myParserZ->Eval(vals);
                        vals[5] = Tre2[j*3 + 2, i];

                        if( myParser_condition->Eval(vals) == 0) {
                            hidden_points[j, i] = 0;
                            there_is_hidden_points = 1; //
                        }
                    }		  
                }      
                /// We will try to resove HP

                    } // End  There is a condition to verify

        } // End  if(  implicitdef != 1 ) 
        // here we are going to work with implicit equation...	  
        else 
        {
            // Begin First case : n = 1, 3, 5.....  
            if(coefficent_fct_implicite_parity == 1)
            {
                for(int j=0; j < nb_ligne   ; j++)
                {
                    jprime = (double)j/(double)(nb_ligne -1 ) ;
                    jprime = jprime * dif_u  + u_inf;

    
                    for (int i=0; i < nb_colone   ; i++)
                    {
                        iprime = (double)i/(double)(nb_colone-1 ) ;
                        iprime = iprime * dif_v + v_inf;
   
                        vals[0]=jprime; vals[1]=iprime;

						Tre2[j*3    , i]  = jprime; // because  X = u
                        vals[3] = Tre2[j*3    , i];

						Tre2[j*3 + 1, i]  = iprime; // because  Y = v
                        vals[4] = Tre2[j*3 + 1, i];
                        //Tre2[j*3 + 2, i]  = pow(myParserZ.Eval(vals), 1/coefficent_fct_implicite);
   
                        valeur_de_fct_implicite = myParserZ->Eval(vals) ;
                        (valeur_de_fct_implicite  >= 0 ) ?
							Tre2[j*3 + 2, i]  = Math::Pow(valeur_de_fct_implicite, 1/coefficent_fct_implicite) : 
						Tre2[j*3 + 2, i]  = -Math::Pow(-valeur_de_fct_implicite, 1/coefficent_fct_implicite);
   
                    }
                }
            } // End of first case : n = 1, 3, 5.....  
            // Begin Second case : n = 2, 4, 6..... 
            else
            {
 
                // We begin with half of the grid...be careful...
                for(int j=0; j < nb_ligne ; j++) {
                    jprime = (double)j/(double)(nb_ligne -1 ) ;
                    jprime = jprime * dif_u  + u_inf;
    
                    for (int i=0; i < nb_colone   ; i++)
                    {
                        iprime = (double)i/(double)(nb_colone -1 ) ;
                        iprime = iprime * dif_v + v_inf;
   
                        vals[0]= vals[3] = jprime; vals[1] = vals[4] = iprime;

                        Tre2[j*3    , i]  =  jprime;
                        Tre2[j*3 + 1, i]  =  iprime;
                        valeur_de_fct_implicite = myParserZ->Eval(vals) ;
   
						if (valeur_de_fct_implicite  >= 0 )  Tre2[j*3 + 2, i] =  Math::Pow(valeur_de_fct_implicite, 1/coefficent_fct_implicite);

        
                        else  { 
                            hidden_points[j, i]  = 0;
                            there_is_hidden_points = 1; // we give the signal to take care of the fact that we have some HP
                        }        
                    }
                }


                //we tray to fix some points of the grid 

                for(int j=0; j < nb_ligne -1   ; j++)
                {
                    for (int i=0; i < nb_colone -1  ; i++) 	  
                    {   
                        if((hidden_points[j  , i  ] +  
                            hidden_points[j+1, i  ] +  
                            hidden_points[j+1, i+1] + 
                            hidden_points[j  , i+1]) >=3) 
                        { 
                            if(hidden_points[j, i] == 0) 
                                Tre2[j*3 + 2, i] = (Tre2[(j+1)*3 + 2, i] + Tre2[j*3 + 2, i+1])/2;
       		 
                            else if(hidden_points[j+1, i] == 0) 
                                Tre2[(j+1)*3 + 2, i] = (Tre2[(j+1)*3 + 2, i+1] + Tre2[j*3 + 2, i])/2;

                            else if(hidden_points[j+1, i+1] == 0) 
                                Tre2[(j+1)*3 + 2, i+1] = (Tre2[(j+1)*3 + 2, i] + Tre2[j*3 + 2, i+1])/2;

                            else if(hidden_points[j, i+1] == 0) 
                                Tre2[j*3 + 2, i+1] = (Tre2[j*3 + 2, i] + Tre2[(j+1)*3 + 2, i+1])/2;       
                        }               
                    }
                }	   
            }  // End of Second case : n = 2, 4, 6..... 	  	       
        } // end of implicit equation...	
    }
    //================ Begin treatement of two systemes ============   
    else
    {
        moitie_colone = nb_colone/2;  

        // We begin with half of the grid...be careful...
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u  + u_inf;
    
            for (int i=0; i < moitie_colone   ; i++)
            {
                iprime = (double)i/(double)(moitie_colone -1 ) ;
                iprime = iprime * dif_v + v_inf;
   
                vals[0]=jprime; vals[1]=iprime;

				Tre2[j*3    , i]  = myParserX->Eval(vals);
                vals[3] = Tre2[j*3    , i];

				Tre2[j*3 + 1, i]  = myParserY->Eval(vals);
                vals[4] = Tre2[j*3 + 1, i];

				Tre2[j*3 + 2, i]  = myParserZ->Eval(vals);
                vals[5] = Tre2[j*3 + 2, i];
                /*
                  if( myParser_condition.Eval(vals) == 0) {
                  hidden_points[j, i] = 0;
                  there_is_hidden_points = 1; //
                  }
                */
            }
        }

        // we finish the remaining of the grid...
        for(int j=0; j < nb_ligne   ; j++)
        {
            jprime = (double)j/(double)(nb_ligne -1 ) ;
            jprime = jprime * dif_u_2  + u_inf_2;
    
            for (int i=moitie_colone-1; i < nb_colone   ; i++)
            {
                iprime = (double)(i - moitie_colone+1)/(double)(nb_colone - moitie_colone ) ;
                iprime = iprime * dif_v_2 + v_inf_2;
   
                vals2[0]=jprime; vals2[1]=iprime;

				Tre2[j*3    , i]  = myParserX_2->Eval(vals2);
                vals2[3] = Tre2[j*3    , i];

				Tre2[j*3 + 1, i]  = myParserY_2->Eval(vals2);
                vals2[4] = Tre2[j*3 + 1, i];

				Tre2[j*3 + 2, i]  = myParserZ_2->Eval(vals2);
                vals2[5] = Tre2[j*3 + 2, i];
                /*
                  if( myParser_condition.Eval(vals2) == 0) {
                  hidden_points[j, i] = 0;
                  there_is_hidden_points = 1; //
                  }
                */
            }
        }
    }	       

    // In This code we attempt resolve some missing Points 
    // DB=Defined Border, NDB=Non defined Border, MP = Middle Point

    double DB_X, DB_Y, NDB_X, NDB_Y, MP_X, MP_Y;
    int    precision_step, i = 0, j = 0;

    if(there_is_condition == 1 && implicitdef != 1)
    {
    
        U_step =  dif_u/(double)(nb_ligne  -1);
        V_step =  dif_v/(double)(nb_colone -1);

        // In this case, we have just one missing point

        for(j=0; j < nb_ligne -1   ; j++)
        {
            for (i=0; i < nb_colone -1  ; i++) 	  
            {
                if((hidden_points[j  , i  ]  +  
                    hidden_points[j+1, i  ]  +  
                    hidden_points[j+1, i+1]  + 
                    hidden_points[j  , i+1]) >=3) 
                {  
                    ///============= First case : DL = Down-Left=====================	 
                    if(hidden_points[j, i] == 0)
                    {
                        // initialisation                       
                        DB_X  =  ((double)(j+1)) * U_step + u_inf;
                        DB_Y  =  ((double)(i+1)) * V_step + v_inf;
                        //DB_X  =  ((double)(j+1)) * U_step + u_inf;
                        //DB_Y  =  ((double)(i)) * V_step + v_inf;   
   
                        NDB_X =  ((double)(j)) * U_step + u_inf;
                        NDB_Y =  ((double)(i)) * V_step + v_inf; 
     
                        DL2[j*3    , i] = Tre2[(j+1)*3    , i];
                        DL2[j*3 + 1, i] = Tre2[(j+1)*3 + 1, i];
                        DL2[j*3 + 2, i] = Tre2[(j+1)*3 + 2, i];
   
                        precision_step = 0; // Approximation by 2^5
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 
                            // We compute this point and look if it fulfill the condition
                            // If yes => DB = MP; else NDB = MP;

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                DL2[j*3    , i] = vals[3];
                                DL2[j*3 + 1, i] = vals[4];
                                DL2[j*3 + 2, i] = vals[5];        
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   	  
                    } // End of if(hidden_points[j, i] == 0)    
                    ///============= Second case : HL = Height-Left ============                  
                    else if(hidden_points[j+1, i] == 0)
                    {
                        // initialisation                       
                        DB_X  =  ((double)(j))   * U_step + u_inf;
                        DB_Y  =  ((double)(i+1)) * V_step + v_inf;

                        //DB_X  =  ((double)(j+1))   * U_step + u_inf;
                        //DB_Y  =  ((double)(i+1)) * V_step + v_inf;
      
                        NDB_X =  ((double)(j+1)) * U_step + u_inf;
                        NDB_Y =  ((double)(i))   * V_step + v_inf; 
     
                        HL2[(j+1)*3    , i] = Tre2[(j+1)*3    , i+1];
                        HL2[(j+1)*3 + 1, i] = Tre2[(j+1)*3 + 1, i+1];
                        HL2[(j+1)*3 + 2, i] = Tre2[(j+1)*3 + 2, i+1];
   
                        precision_step = 0; // Approximation by 2^5
   
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 
                            // We compute this point and look if it fulfill the condition
                            // If yes => DB = MP; else NDB = MP;

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                HL2[(j+1)*3    , i] = vals[3];
                                HL2[(j+1)*3 + 1, i] = vals[4];
                                HL2[(j+1)*3 + 2, i] = vals[5];        
                            }
           
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   	  
                    } // End of if(hidden_points[j, i] == 0)
                    ///============= Third case : HR = Height-Right ============                  
                    else if(hidden_points[j+1, i+1] == 0)
                    {
                        // initialisation                       
                        DB_X  =  ((double)(j))   * U_step + u_inf;
                        DB_Y  =  ((double)(i))   * V_step + v_inf;
                        //DB_X  =  ((double)(j))   * U_step + u_inf;
                        //DB_Y  =  ((double)(i+1))   * V_step + v_inf;
   
                        NDB_X =  ((double)(j+1)) * U_step + u_inf;
                        NDB_Y =  ((double)(i+1)) * V_step + v_inf; 
     
                        HR2[(j+1)*3    , i+1] = Tre2[j*3    , i+1];
                        HR2[(j+1)*3 + 1, i+1] = Tre2[j*3 + 1, i+1];
                        HR2[(j+1)*3 + 2, i+1] = Tre2[j*3 + 2, i+1];
   
                        precision_step = 0; // Approximation by 2^5
   
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 
                            // We compute this point and look if it fulfill the condition
                            // If yes => DB = MP; else NDB = MP;

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                HR2[(j+1)*3    , i+1] = vals[3];
                                HR2[(j+1)*3 + 1, i+1] = vals[4];
                                HR2[(j+1)*3 + 2, i+1] = vals[5];        
                            }
           
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
                    } // End of if(hidden_points[j, i] == 0)
                    ///============= Fourth case : DR = Height-Right ============                  
                    else if(hidden_points[j, i+1] == 0)
                    {
                        // initialisation                       
                        DB_X  =  ((double)(j+1))   * U_step + u_inf;
                        DB_Y  =  ((double)(i))   * V_step + v_inf;
                        //DB_X  =  ((double)(j))   * U_step + u_inf;
                        //DB_Y  =  ((double)(i))   * V_step + v_inf;   
   
                        NDB_X =  ((double)(j)) * U_step + u_inf;
                        NDB_Y =  ((double)(i+1))   * V_step + v_inf; 
     
                        DR2[j*3    , i+1] = Tre2[j*3    , i];
                        DR2[j*3 + 1, i+1] = Tre2[j*3 + 1, i];
                        DR2[j*3 + 2, i+1] = Tre2[j*3 + 2, i];
   
                        precision_step = 0; // Approximation by 2^5
   
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 
                            // We compute this point and look if it fulfill the condition
                            // If yes => DB = MP; else NDB = MP;

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                DR2[j*3    , i+1] = vals[3];
                                DR2[j*3 + 1, i+1] = vals[4];
                                DR2[j*3 + 2, i+1] = vals[5];        
                            }
           
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   	  
                    } // End of if(hidden_points[j, i] == 0)   
                  
                }                // End of if((hidden_points[j, i] + ...)
            }
        }

        double DB2_X, DB2_Y, NDB2_X, NDB2_Y, MP2_X, MP2_Y;

        //==============================================================
        //========== In this case, we have two missing point ===========
        //==============================================================
        for(j=0; j < nb_ligne -1   ; j++)
        {
            for (i=0; i < nb_colone -1  ; i++)
            {
                if((hidden_points[j  , i  ]  +  
                    hidden_points[j+1, i  ]  +  
                    hidden_points[j+1, i+1]  + 
                    hidden_points[j  , i+1]) == 2) 
                {  
                    ///==================================	 
                    if(hidden_points[j, i] == 0 && hidden_points[j, i+1] == 0) {
                        // initialisation                       
                        DB_X  =  ((double)(j+1)) * U_step + u_inf;
                        DB_Y  =  ((double)(i)) * V_step + v_inf;
   
                        NDB_X =  ((double)(j)) * U_step + u_inf;
                        NDB_Y =  ((double)(i)) * V_step + v_inf; 
     
                        DL2[j*3    , i] = Tre2[(j+1)*3    , i];
                        DL2[j*3 + 1, i] = Tre2[(j+1)*3 + 1, i];
                        DL2[j*3 + 2, i] = Tre2[(j+1)*3 + 2, i];

  
                        DB2_X  =  ((double)(j+1))   * U_step + u_inf;
                        DB2_Y  =  ((double)(i+1))   * V_step + v_inf;
   
                        NDB2_X =  ((double)(j)) * U_step + u_inf;
                        NDB2_Y =  ((double)(i+1))   * V_step + v_inf; 
     
                        DR2[j*3    , i+1] = Tre2[(j+1)*3    , i+1];
                        DR2[j*3 + 1, i+1] = Tre2[(j+1)*3 + 1, i+1];
                        DR2[j*3 + 2, i+1] = Tre2[(j+1)*3 + 2, i+1];
   
                        // First point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                DL2[j*3    , i] = vals[3];
                                DL2[j*3 + 1, i] = vals[4];
                                DL2[j*3 + 2, i] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   
   
                        // Second point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP2_X = (DB2_X + NDB2_X)/2; 
                            MP2_Y = (DB2_Y + NDB2_Y)/2; 

                            vals[0] = MP2_X;
                            vals[1] = MP2_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB2_X = MP2_X;
                                NDB2_Y = MP2_Y;       
                            } 
                            else {
                                DB2_X = MP2_X;
                                DB2_Y = MP2_Y;
                                DR2[j*3    , i+1] = vals[3];
                                DR2[j*3 + 1, i+1] = vals[4];
                                DR2[j*3 + 2, i+1] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
       
   	  
                    } // End of if(hidden_points[j, i] == 0 && hidden_points[j, i+1] == 0)    

 
                    ///=========================                  
                  
                    else if(hidden_points[j, i] == 0 && hidden_points[j+1, i] == 0) {
                        // initialisation                       
                        DB_X  =  ((double)(j)) * U_step + u_inf;
                        DB_Y  =  ((double)(i+1)) * V_step + v_inf;
   
                        NDB_X =  ((double)(j)) * U_step + u_inf;
                        NDB_Y =  ((double)(i)) * V_step + v_inf; 
     
                        DL2[j*3    , i] = Tre2[j*3    , i+1];
                        DL2[j*3 + 1, i] = Tre2[j*3 + 1, i+1];
                        DL2[j*3 + 2, i] = Tre2[j*3 + 2, i+1];

  
                        DB2_X  =  ((double)(j+1))   * U_step + u_inf;
                        DB2_Y  =  ((double)(i+1))   * V_step + v_inf;
   
                        NDB2_X =  ((double)(j+1)) * U_step + u_inf;
                        NDB2_Y =  ((double)(i))   * V_step + v_inf; 
     
                        HL2[(j+1)*3    , i] = Tre2[(j+1)*3    , i+1];
                        HL2[(j+1)*3 + 1, i] = Tre2[(j+1)*3 + 1, i+1];
                        HL2[(j+1)*3 + 2, i] = Tre2[(j+1)*3 + 2, i+1];
   
                        // First point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                DL2[j*3    , i] = vals[3];
                                DL2[j*3 + 1, i] = vals[4];
                                DL2[j*3 + 2, i] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   
   
                        // Second point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP2_X = (DB2_X + NDB2_X)/2; 
                            MP2_Y = (DB2_Y + NDB2_Y)/2; 

                            vals[0] = MP2_X;
                            vals[1] = MP2_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB2_X = MP2_X;
                                NDB2_Y = MP2_Y;       
                            } 
                            else {
                                DB2_X = MP2_X;
                                DB2_Y = MP2_Y;
                                HL2[(j+1)*3    , i] = vals[3];
                                HL2[(j+1)*3 + 1, i] = vals[4];
                                HL2[(j+1)*3 + 2, i] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...

  
                    } // End of if(hidden_points[j, i] == 0 && hidden_points[j+1, i] == 0)


 
                    ///=========================                  
                  
                    else if(hidden_points[j+1, i] == 0 && hidden_points[j+1, i+1] == 0) {
                        // initialisation                       
                        DB_X  =  ((double)(j)) * U_step + u_inf;
                        DB_Y  =  ((double)(i+1)) * V_step + v_inf;
   
                        NDB_X =  ((double)(j+1)) * U_step + u_inf;
                        NDB_Y =  ((double)(i+1)) * V_step + v_inf; 
     
                        HR2[(j+1)*3    , i+1] = Tre2[j*3    , i+1];
                        HR2[(j+1)*3 + 1, i+1] = Tre2[j*3 + 1, i+1];
                        HR2[(j+1)*3 + 2, i+1] = Tre2[j*3 + 2, i+1];



                        DB2_X  =  ((double)(j))   * U_step + u_inf;
                        DB2_Y  =  ((double)(i)) * V_step + v_inf;
   
                        NDB2_X =  ((double)(j+1)) * U_step + u_inf;
                        NDB2_Y =  ((double)(i))   * V_step + v_inf; 
     
                        HL2[(j+1)*3    , i] = Tre2[j*3    , i];
                        HL2[(j+1)*3 + 1, i] = Tre2[j*3 + 1, i];
                        HL2[(j+1)*3 + 2, i] = Tre2[j*3 + 2, i];
   
                        // First point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                HR2[(j+1)*3    , i+1] = vals[3];
                                HR2[(j+1)*3 + 1, i+1] = vals[4];
                                HR2[(j+1)*3 + 2, i+1] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   
   
                        // Second point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP2_X = (DB2_X + NDB2_X)/2; 
                            MP2_Y = (DB2_Y + NDB2_Y)/2; 

                            vals[0] = MP2_X;
                            vals[1] = MP2_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB2_X = MP2_X;
                                NDB2_Y = MP2_Y;       
                            } 
                            else {
                                DB2_X = MP2_X;
                                DB2_Y = MP2_Y;
                                HL2[(j+1)*3    , i] = vals[3];
                                HL2[(j+1)*3 + 1, i] = vals[4];
                                HL2[(j+1)*3 + 2, i] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...



                    } // End of if(hidden_points[j+1, i] == 0 && hidden_points[j+1, i+1] == 0)



                    ///============= Fourth case : DR = Height-Right ============                  
                  
                    else if(hidden_points[j+1, i+1] == 0 && hidden_points[j, i+1] == 0) {
                        // initialisation                       
                        DB_X  =  ((double)(j+1)) * U_step + u_inf;
                        DB_Y  =  ((double)(i)) * V_step + v_inf;
   
                        NDB_X =  ((double)(j+1)) * U_step + u_inf;
                        NDB_Y =  ((double)(i+1)) * V_step + v_inf; 
     
                        HR2[(j+1)*3    , i+1] = Tre2[(j+1)*3    , i];
                        HR2[(j+1)*3 + 1, i+1] = Tre2[(j+1)*3 + 1, i];
                        HR2[(j+1)*3 + 2, i+1] = Tre2[(j+1)*3 + 2, i];



                        DB2_X  =  ((double)(j)) * U_step + u_inf;
                        DB2_Y  =  ((double)(i)) * V_step + v_inf;
   
                        NDB2_X =  ((double)(j)) * U_step + u_inf;
                        NDB2_Y =  ((double)(i+1))   * V_step + v_inf; 
     
                        DR2[j*3    , i+1] = Tre2[j*3    , i];
                        DR2[j*3 + 1, i+1] = Tre2[j*3 + 1, i];
                        DR2[j*3 + 2, i+1] = Tre2[j*3 + 2, i];
   
                        // First point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP_X = (DB_X + NDB_X)/2; 
                            MP_Y = (DB_Y + NDB_Y)/2; 

                            vals[0] = MP_X;
                            vals[1] = MP_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB_X = MP_X;
                                NDB_Y = MP_Y;       
                            } // End of if( myParser_condition.Eval(vals) == 0)
                            else {
                                DB_X = MP_X;
                                DB_Y = MP_Y;
                                HR2[(j+1)*3    , i+1] = vals[3];
                                HR2[(j+1)*3 + 1, i+1] = vals[4];
                                HR2[(j+1)*3 + 2, i+1] = vals[5];          
                            }
   
                            precision_step++;                                
                        }// End of while ( precision_step < 5)...
   
   
                        // Second point solution 
                        precision_step = 0; 
                        while ( precision_step < 5){
                            MP2_X = (DB2_X + NDB2_X)/2; 
                            MP2_Y = (DB2_Y + NDB2_Y)/2; 

                            vals[0] = MP2_X;
                            vals[1] = MP2_Y;
                            vals[3] = myParserX->Eval(vals);
                            vals[4] = myParserY->Eval(vals);
                            vals[5] = myParserZ->Eval(vals) ; 
    
                            if( myParser_condition->Eval(vals) == 0) {
                                NDB2_X = MP2_X;
                                NDB2_Y = MP2_Y;       
                            } 
                            else {
                                DB2_X = MP2_X;
                                DB2_Y = MP2_Y;
                                DR2[j*3    , i+1] = vals[3];
                                DR2[j*3 + 1, i+1] = vals[4];
                                DR2[j*3 + 2, i+1] = vals[5];          
                            }
   
                            precision_step++;                                
                        } // End of while ( precision_step < 5)...
                    } // End of if(hidden_points[j+1, i+1] == 0 && hidden_points[j, i+1] == 0)                   
                } // End of if((hidden_points[j, i] + ...)
            }
        }
    } // End of if(there_is_hidden_points == 1)

} // End of the fct calcul_objet3()

void  Model3D::boite_englobante3()
{		
    // calcul des minimas et des maximas des coordonnees: X,Y et Z

    MINX =999999999;//Tre2[0, 0];
    MINY =999999999;//Tre2[1, 0];
    MINZ =999999999;//Tre2[2, 0];

    MAXX =-999999999;//Tre2[0, 0];
    MAXY =-999999999;//Tre2[1, 0];
    MAXZ =-999999999;//Tre2[2, 0];

    for (int i=0; i < nb_ligne   ; i++)
        for (int j=0; j < nb_colone   ; j++) {
            if(hidden_points[i, j] == 1 || implicitdef != 1) {
                if(MINX > Tre2[i*3    , j] ) MINX = Tre2[i*3    , j] ;
     
                if(MINY > Tre2[i*3 + 1, j] ) MINY = Tre2[i*3 + 1, j] ;
     
                if(MINZ > Tre2[i*3 + 2, j] ) MINZ = Tre2[i*3 + 2, j] ;

                if(MAXX < Tre2[i*3    , j] ) MAXX = Tre2[i*3    , j] ;
     
                if(MAXY < Tre2[i*3 + 1, j] ) MAXY = Tre2[i*3 + 1, j] ;
     
                if(MAXZ < Tre2[i*3 + 2, j] ) MAXZ = Tre2[i*3 + 2, j] ;
            }
        }
    // On recherche la plus grande "difference" de distance entre les points
    // pour reduire la figure a un cube d'une longueur de 1.
    // On utilisera apres cette figure pour l'agrandir et bien la placer dans
    // la fenetre de vue:

    DIFX = MAXX - MINX ;
    DIFY = MAXY - MINY ;
    DIFZ = MAXZ - MINZ ;

    // save theses values for the equations generator :
    DIFX_tmp = DIFX; DIFY_tmp = DIFY; DIFZ_tmp = DIFZ;
    MINX_tmp = MINX; MINY_tmp = MINY; MINZ_tmp = MINZ;

    // Recherche du maximum :
    DIFMAXIMUM = DIFX;

    if (DIFY > DIFMAXIMUM) {DIFMAXIMUM = DIFY;};
    if (DIFZ > DIFMAXIMUM) {DIFMAXIMUM = DIFZ;};



    // On va inclure cet objet dans un cube de langueur maximum
    // egale a "hauteur_fenetre"

    double decalage_xo = -(MINX +MAXX)/2 ;
    double decalage_yo = -(MINY +MAXY)/2 ;
    double decalage_zo = -(MINZ +MAXZ)/2 ;

    for (int i=0; i < nb_ligne   ; i++)
        for (int j=0; j < nb_colone   ; j++) { 	
            Tre2[i*3    , j] = hauteur_fenetre * (Tre2[i*3    , j] + decalage_xo)/DIFMAXIMUM ;
            Tre2[i*3 + 1, j] = hauteur_fenetre * (Tre2[i*3 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
            Tre2[i*3 + 2, j] = hauteur_fenetre * (Tre2[i*3 + 2, j] + decalage_zo)/DIFMAXIMUM ; 	
    	}

    if(there_is_condition == 1 ) 
        for (int i=0; i < nb_ligne   ; i++)
            for (int j=0; j < nb_colone   ; j++)
                if(hidden_points[i, j] == 0) {
                    // HR  
                    HR2[i*3    , j] = hauteur_fenetre * (HR2[i*3    , j] + decalage_xo)/DIFMAXIMUM ;
                    HR2[i*3 + 1, j] = hauteur_fenetre * (HR2[i*3 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
                    HR2[i*3 + 2, j] = hauteur_fenetre * (HR2[i*3 + 2, j] + decalage_zo)/DIFMAXIMUM ;     
     
                    // DR  
                    DR2[i*3    , j] = hauteur_fenetre * (DR2[i*3    , j] + decalage_xo)/DIFMAXIMUM ;
                    DR2[i*3 + 1, j] = hauteur_fenetre * (DR2[i*3 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
                    DR2[i*3 + 2, j] = hauteur_fenetre * (DR2[i*3 + 2, j] + decalage_zo)/DIFMAXIMUM ;     
     
                    // HL  
                    HL2[i*3    , j] = hauteur_fenetre * (HL2[i*3    , j] + decalage_xo)/DIFMAXIMUM ;
                    HL2[i*3 + 1, j] = hauteur_fenetre * (HL2[i*3 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
                    HL2[i*3 + 2, j] = hauteur_fenetre * (HL2[i*3 + 2, j] + decalage_zo)/DIFMAXIMUM ;     
     
                    // DL  
                    DL2[i*3    , j] = hauteur_fenetre * (DL2[i*3    , j] + decalage_xo)/DIFMAXIMUM ;
                    DL2[i*3 + 1, j] = hauteur_fenetre * (DL2[i*3 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
                    DL2[i*3 + 2, j] = hauteur_fenetre * (DL2[i*3 + 2, j] + decalage_zo)/DIFMAXIMUM ;      
     
                }
    MINX = hauteur_fenetre * (MINX + decalage_xo)/DIFMAXIMUM;
    MINY = hauteur_fenetre * (MINY + decalage_yo)/DIFMAXIMUM;
    MINZ = hauteur_fenetre * (MINZ + decalage_zo)/DIFMAXIMUM;

    MAXX = hauteur_fenetre * (MAXX + decalage_xo)/DIFMAXIMUM;
    MAXY = hauteur_fenetre * (MAXY + decalage_yo)/DIFMAXIMUM;
    MAXZ = hauteur_fenetre * (MAXZ + decalage_zo)/DIFMAXIMUM;

    // Construction de la boite englobante...
  
    boiteenglobante2[0] = MAXX;
    boiteenglobante2[1] = MAXY;
    boiteenglobante2[2] = MAXZ;
  
    boiteenglobante2[3] = MINX;
    boiteenglobante2[4] = MAXY;
    boiteenglobante2[5] = MAXZ;
  
    boiteenglobante2[6] = MINX;
    boiteenglobante2[7] = MAXY;
    boiteenglobante2[8] = MINZ;
  
    boiteenglobante2[9]  = MAXX;
    boiteenglobante2[10] = MAXY;
    boiteenglobante2[11] = MINZ;
  
    boiteenglobante2[12] = MAXX;
    boiteenglobante2[13] = MINY;
    boiteenglobante2[14] = MAXZ;
  
    boiteenglobante2[15] = MINX;
    boiteenglobante2[16] = MINY;
    boiteenglobante2[17] = MAXZ;
  
    boiteenglobante2[18] = MINX;
    boiteenglobante2[19] = MINY;
    boiteenglobante2[20] = MINZ;
  
    boiteenglobante2[21]  = MAXX;
    boiteenglobante2[22]  = MINY;
    boiteenglobante2[23]  = MINZ;
}

void  Model3D::project_4D_to_3D()
{
    int i, j;
    double c4;
    for ( i=0; i < nb_ligne - coupure_ligne; ++i)
    {
        for ( j=0; j < nb_colone - coupure_col  ; ++j){
            c4 = 1/(shape4D[i*4 + 3, j] - 2);
            Tre2[i*3  , j] = c4*shape4D[i*4    , j];
            Tre2[i*3+1, j] = c4*shape4D[i*4 + 1, j];
            Tre2[i*3+2, j] = c4*shape4D[i*4 + 2, j];
        }
    }
}

void  Model3D::boite_englobante4D()
{		
    // calcul des minimas et des maximas des coordonnees: X,Y et Z

    MINX =999999999;
    MINY =999999999;
    MINZ =999999999;
    MINW =999999999;

    MAXX =-999999999;
    MAXY =-999999999;
    MAXZ =-999999999;
    MAXW =-999999999;


    for (int i=0; i < nb_ligne   ; i++)
        for (int j=0; j < nb_colone   ; j++) {

            if(MINX > shape4D[i*4    , j] ) MINX = shape4D[i*4    , j] ;    
            if(MINY > shape4D[i*4 + 1, j] ) MINY = shape4D[i*4 + 1, j] ;   
            if(MINZ > shape4D[i*4 + 2, j] ) MINZ = shape4D[i*4 + 2, j] ;    
            if(MINW > shape4D[i*4 + 3, j] ) MINW = shape4D[i*4 + 3, j] ;
     

            if(MAXX < shape4D[i*4    , j] ) MAXX = shape4D[i*4    , j] ;     
            if(MAXY < shape4D[i*4 + 1, j] ) MAXY = shape4D[i*4 + 1, j] ;     
            if(MAXZ < shape4D[i*4 + 2, j] ) MAXZ = shape4D[i*4 + 2, j] ;    
            if(MAXW < shape4D[i*4 + 3, j] ) MAXW = shape4D[i*4 + 3, j] ;
        }

    // On recherche la plus grande "difference" de distance entre les points
    // pour reduire la figure a un cube d'une longueur de 1.
    // On utilisera apres cette figure pour l'agrandir et bien la placer dans
    // la fenetre de vue:

    DIFX = MAXX - MINX ;
    DIFY = MAXY - MINY ;
    DIFZ = MAXZ - MINZ ;
    DIFW = MAXW - MINW ;
  
    // Recherche du maximum :
    DIFMAXIMUM = DIFX;

    if (DIFY > DIFMAXIMUM) {DIFMAXIMUM = DIFY;};
    if (DIFZ > DIFMAXIMUM) {DIFMAXIMUM = DIFZ;};
    if (DIFW > DIFMAXIMUM) {DIFMAXIMUM = DIFW;};



    // On va inclure cet objet dans un HperCube de langueur maximum
    // egale a "hauteur_fenetre"

    double decalage_xo = -(MINX +MAXX)/2 ;
    double decalage_yo = -(MINY +MAXY)/2 ;
    double decalage_zo = -(MINZ +MAXZ)/2 ;
    double decalage_wo = -(MINW +MAXW)/2 ;

    for (int i=0; i < nb_ligne   ; i++)
    {
        for (int j=0; j < nb_colone   ; j++)
        { 	
            shape4D[i*4    , j] = (shape4D[i*4    , j] + decalage_xo)/DIFMAXIMUM ;
            shape4D[i*4 + 1, j] = (shape4D[i*4 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
            shape4D[i*4 + 2, j] = (shape4D[i*4 + 2, j] + decalage_zo)/DIFMAXIMUM ;
            shape4D[i*4 + 3, j] = (shape4D[i*4 + 3, j] + decalage_wo)/DIFMAXIMUM ;    	        
    	}
	}	
}

void  Model3D::project_5D_to_4D()
{
    int i, j;
    double c4;
    for ( i=0; i < nb_ligne - coupure_ligne; ++i)
    {
        for ( j=0; j < nb_colone - coupure_col  ; ++j)
        {
            c4 = 1/(shape5D[i*5 + 4, j] - 2);
            shape4D[i*4  , j] = c4*shape5D[i*5    , j];
            shape4D[i*4+1, j] = c4*shape5D[i*5 + 1, j];
            shape4D[i*4+2, j] = c4*shape5D[i*5 + 2, j];
            shape4D[i*4+3, j] = c4*shape5D[i*5 + 3, j];   
        }
    }
}

void  Model3D::boite_englobante5D()
{		
    // calcul des minimas et des maximas des coordonnees: X,Y et Z

    MINX =999999999;
    MINY =999999999;
    MINZ =999999999;
    MINW =999999999;
    MINT =999999999;

    MAXX =-999999999;
    MAXY =-999999999;
    MAXZ =-999999999;
    MAXW =-999999999;
    MAXT =-999999999;


    for (int i=0; i < nb_ligne   ; i++)
    {
        for (int j=0; j < nb_colone   ; j++) {

            if(MINX > shape5D[i*5    , j] ) MINX = shape5D[i*5    , j] ;    
            if(MINY > shape5D[i*5 + 1, j] ) MINY = shape5D[i*5 + 1, j] ;   
            if(MINZ > shape5D[i*5 + 2, j] ) MINZ = shape5D[i*5 + 2, j] ;    
            if(MINW > shape5D[i*5 + 3, j] ) MINW = shape5D[i*5 + 3, j] ;
            if(MINT > shape5D[i*5 + 4, j] ) MINT = shape5D[i*5 + 4, j] ;
     

            if(MAXX < shape5D[i*5    , j] ) MAXX = shape5D[i*5    , j] ;     
            if(MAXY < shape5D[i*5 + 1, j] ) MAXY = shape5D[i*5 + 1, j] ;     
            if(MAXZ < shape5D[i*5 + 2, j] ) MAXZ = shape5D[i*5 + 2, j] ;    
            if(MAXW < shape5D[i*5 + 3, j] ) MAXW = shape5D[i*5 + 3, j] ;
            if(MAXT < shape5D[i*5 + 4, j] ) MAXT = shape5D[i*5 + 4, j] ;
        }
    }

    // On recherche la plus grande "difference" de distance entre les points
    // pour reduire la figure a un cube d'une longueur de 1.
    // On utilisera apres cette figure pour l'agrandir et bien la placer dans
    // la fenetre de vue:

    DIFX = MAXX - MINX ;
    DIFY = MAXY - MINY ;
    DIFZ = MAXZ - MINZ ;
    DIFW = MAXW - MINW ;
    DIFT = MAXT - MINT ;
  
    // Recherche du maximum :
    DIFMAXIMUM = DIFX;

    if (DIFY > DIFMAXIMUM) {DIFMAXIMUM = DIFY;};
    if (DIFZ > DIFMAXIMUM) {DIFMAXIMUM = DIFZ;};
    if (DIFW > DIFMAXIMUM) {DIFMAXIMUM = DIFW;};
    if (DIFT > DIFMAXIMUM) {DIFMAXIMUM = DIFT;};

    // On va inclure cet objet dans un HyperCube de langueur maximum
    // egale a "hauteur_fenetre"

    double decalage_xo = -(MINX +MAXX)/2 ;
    double decalage_yo = -(MINY +MAXY)/2 ;
    double decalage_zo = -(MINZ +MAXZ)/2 ;
    double decalage_wo = -(MINW +MAXW)/2 ;
    double decalage_to = -(MINT +MAXT)/2 ;

    for (int i=0; i < nb_ligne   ; i++)
    {
        for (int j=0; j < nb_colone   ; j++) { 	
            shape5D[i*5    , j] = (shape5D[i*5    , j] + decalage_xo)/DIFMAXIMUM ;
            shape5D[i*5 + 1, j] = (shape5D[i*5 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
            shape5D[i*5 + 2, j] = (shape5D[i*5 + 2, j] + decalage_zo)/DIFMAXIMUM ;
            shape5D[i*5 + 3, j] = (shape5D[i*5 + 3, j] + decalage_wo)/DIFMAXIMUM ; 
            shape5D[i*5 + 4, j] = (shape5D[i*5 + 4, j] + decalage_to)/DIFMAXIMUM ;   	
        
    	}
    }	
}

void  Model3D::project_6D_to_5D()
{
    for (int i=0; i < nb_ligne - coupure_ligne; ++i)
    {
        for (int j=0; j < nb_colone - coupure_col  ; ++j)
        {
            double c4 = 1/(shape6D[i*6 + 5, j] - 2);
            shape5D[i*5  , j] = c4*shape6D[i*6    , j];
            shape5D[i*5+1, j] = c4*shape6D[i*6 + 1, j];
            shape5D[i*5+2, j] = c4*shape6D[i*6 + 2, j];
            shape5D[i*5+3, j] = c4*shape6D[i*6 + 3, j];
            shape5D[i*5+4, j] = c4*shape6D[i*6 + 4, j];   
        }
    }
}
      
void  Model3D::boite_englobante6D()
{		
    // calcul des minimas et des maximas des coordonnees: X,Y et Z

    MINX =999999999;
    MINY =999999999;
    MINZ =999999999;
    MINW =999999999;
    MINT =999999999;
    MINS =999999999;

    MAXX =-999999999;
    MAXY =-999999999;
    MAXZ =-999999999;
    MAXW =-999999999;
    MAXT =-999999999;
    MAXS =-999999999;

    for (int i=0; i < nb_ligne   ; i++)
    {
        for (int j=0; j < nb_colone   ; j++)
        {
            if(MINX > shape6D[i*6    , j] ) MINX = shape6D[i*6    , j] ;    
            if(MINY > shape6D[i*6 + 1, j] ) MINY = shape6D[i*6 + 1, j] ;   
            if(MINZ > shape6D[i*6 + 2, j] ) MINZ = shape6D[i*6 + 2, j] ;    
            if(MINW > shape6D[i*6 + 3, j] ) MINW = shape6D[i*6 + 3, j] ;
            if(MINT > shape6D[i*6 + 4, j] ) MINT = shape6D[i*6 + 4, j] ;
            if(MINS > shape6D[i*6 + 5, j] ) MINS = shape6D[i*6 + 5, j] ;
     

            if(MAXX < shape6D[i*6    , j] ) MAXX = shape6D[i*6    , j] ;     
            if(MAXY < shape6D[i*6 + 1, j] ) MAXY = shape6D[i*6 + 1, j] ;     
            if(MAXZ < shape6D[i*6 + 2, j] ) MAXZ = shape6D[i*6 + 2, j] ;    
            if(MAXW < shape6D[i*6 + 3, j] ) MAXW = shape6D[i*6 + 3, j] ;
            if(MAXT < shape6D[i*6 + 4, j] ) MAXT = shape6D[i*6 + 4, j] ;
            if(MAXS < shape6D[i*6 + 5, j] ) MAXS = shape6D[i*6 + 5, j] ;
        }
    }

    // On recherche la plus grande "difference" de distance entre les points
    // pour reduire la figure a un cube d'une longueur de 1.
    // On utilisera apres cette figure pour l'agrandir et bien la placer dans
    // la fenetre de vue:

    DIFX = MAXX - MINX ;
    DIFY = MAXY - MINY ;
    DIFZ = MAXZ - MINZ ;
    DIFW = MAXW - MINW ;
    DIFT = MAXT - MINT ;
    DIFS = MAXS - MINS 
        ;
    // Recherche du maximum :
    DIFMAXIMUM = DIFX;

    if (DIFY > DIFMAXIMUM) {DIFMAXIMUM = DIFY;};
    if (DIFZ > DIFMAXIMUM) {DIFMAXIMUM = DIFZ;};
    if (DIFW > DIFMAXIMUM) {DIFMAXIMUM = DIFW;};
    if (DIFT > DIFMAXIMUM) {DIFMAXIMUM = DIFT;};
    if (DIFS > DIFMAXIMUM) {DIFMAXIMUM = DIFS;};


    // On va inclure cet objet dans un HyperCube de langueur maximum
    // egale a "hauteur_fenetre"

    double decalage_xo = -(MINX +MAXX)/2 ;
    double decalage_yo = -(MINY +MAXY)/2 ;
    double decalage_zo = -(MINZ +MAXZ)/2 ;
    double decalage_wo = -(MINW +MAXW)/2 ;
    double decalage_to = -(MINT +MAXT)/2 ;
    double decalage_so = -(MINS +MAXS)/2 ;

    for (int i=0; i < nb_ligne   ; i++)
    {
        for (int j=0; j < nb_colone   ; j++)
        { 	
            shape6D[i*6    , j] = (shape6D[i*6    , j] + decalage_xo)/DIFMAXIMUM ;
            shape6D[i*6 + 1, j] = (shape6D[i*6 + 1, j] + decalage_yo)/DIFMAXIMUM ;	
            shape6D[i*6 + 2, j] = (shape6D[i*6 + 2, j] + decalage_zo)/DIFMAXIMUM ;
            shape6D[i*6 + 3, j] = (shape6D[i*6 + 3, j] + decalage_wo)/DIFMAXIMUM ; 
            shape6D[i*6 + 4, j] = (shape6D[i*6 + 4, j] + decalage_to)/DIFMAXIMUM ;   	
            shape6D[i*6 + 5, j] = (shape6D[i*6 + 5, j] + decalage_so)/DIFMAXIMUM ;    
    	}
    }
}

void Model3D::drawBbox (Graphics^ g)
{
    if (box ==1)
    {
        int i;
        double c4; 
        for (i=0; i<4; i++)
        {
            c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

            tableaureferences[0]->setPoint(i, (int)(c4*boiteenglobante[i*3]  +demi_hauteur),
                                              (int)(c4*boiteenglobante[i*3+1]+demi_largeur));
        }

		g->DrawPolygon(gcnew Pen(Color::FromArgb(0, 0, 250)), tableaureferences[0]->Points());

        for (i=4; i<8; i++)
        {
            c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

            tableaureferences[0]->setPoint(i-4,
                                           (int)(c4*boiteenglobante[i*3] +demi_hauteur),
                                           (int)(c4*boiteenglobante[i*3 + 1]+demi_largeur));

        }

		g->DrawPolygon(gcnew Pen(Color::FromArgb(0, 0, 250)), tableaureferences[0]->Points());

        for (i=0; i<2; i++)
        {

            c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

            tableaureferences[0]->setPoint(i,
                                           (int)(c4*boiteenglobante[i*3]+demi_hauteur),
                                           (int)(c4*boiteenglobante[i*3+1]+demi_largeur));

        }

        i = 5; 
        c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

        tableaureferences[0]->setPoint(2,
                                       (int)(c4*boiteenglobante[i*3]+demi_hauteur),
                                       (int)(c4*boiteenglobante[i*3+1]+demi_largeur));

        i = 4;

        c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

        tableaureferences[0]->setPoint(3,
                                       (int)(c4*boiteenglobante[i*3]+demi_hauteur),
                                       (int)(c4*boiteenglobante[i*3+1]+demi_largeur));

		g->DrawPolygon(gcnew Pen(Color::FromArgb(250, 0, 0)), tableaureferences[0]->Points());

        //================================================

        FontFamily^ ff = gcnew FontFamily("Arial");
		System::Drawing::Font^ font = gcnew System::Drawing::Font(ff, 14, FontStyle::Regular, GraphicsUnit::Pixel);
		Brush^ br = gcnew SolidBrush(Color::FromArgb(250, 250, 0));

        g->DrawString(inf_u, font, br, (float)tableaureferences[0]->at(2).X, (float)tableaureferences[0]->at(2).Y+12);
        g->DrawString("U="+sup_u, font, br, (float)tableaureferences[0]->at(3).X, (float)tableaureferences[0]->at(3).Y+12);

		br = gcnew SolidBrush(Color::FromArgb(0, 250, 250));

        g->DrawString(inf_v, font, br, (float)tableaureferences[0]->at(3).X-12, (float)tableaureferences[0]->at(3).Y);
        g->DrawString("V="+sup_v, font, br, 
			(float)tableaureferences[0]->at(0).X-12, (float)tableaureferences[0]->at(0).Y);

        //================================================
        for (i=2; i<4; i++)
        {
            c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

            tableaureferences[0]->setPoint(i-2,
                                           (int)(c4*boiteenglobante[i*3    ]  +demi_hauteur),
                                           (int)(c4*boiteenglobante[i*3 + 1]  +demi_largeur));
        }

        i = 7; 

        c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

        tableaureferences[0]->setPoint(2,
                                       (int)(c4*boiteenglobante[i*3]+demi_hauteur),
                                       (int)(c4*boiteenglobante[i*3+1]+demi_largeur));

        i = 6;

        c4 = D/(boiteenglobante[i*3 +2]-Oprime[2]);

        tableaureferences[0]->setPoint(3,
                                       (int)(c4*boiteenglobante[i*3]+demi_hauteur),
                                       (int)(c4*boiteenglobante[i*3+1]+demi_largeur));

		g->DrawPolygon(gcnew Pen(Color::FromArgb(10, 200, 200)), tableaureferences[0]->Points());
    }
}

void Model3D::drawBbox(Device^ device)
{
	if (box == 1)
    {
		// draw all 6 side of the box
		device->RenderState->Lighting = false;

		// draw box
		device->SetStreamSource(0, boundingBoxVB, 0);
		device->Indices = boundingBoxIB;
		device->VertexFormat = CustomVertex::PositionColored::Format;
		device->DrawIndexedPrimitives(PrimitiveType::LineList, 0, 0, 16, 0, 12);
		device->Indices = nullptr;
	}
}

void Model3D::TrySelect(Point3F^ origin, Point3F^ dir)
{
	Vector3 rayOrigin = Vector3(origin->X, origin->Y, origin->Z);
	Vector3 rayDir = Vector3(dir->X, dir->Y, dir->Z);
	// try intersections with ALL triangles
	// do slow way for simplicity
	Plane testPlane;
	Vector3 triP1, triP2, triP3;
	IntersectInformation interInf;
	int nearestIdx = -1;
	float nearestDist = -1;
	int hIdx, vIdx;
	for (int i=0; i < nb_ligne - 1; i++)
	{
		for (int j=0; j < nb_colone - 1; j++)
		{
			// t1
			triP1 = Vector3((float)Tre2[i*3, j],
							(float)Tre2[i*3 + 1, j],
							(float)Tre2[i*3 + 2, j]);
			triP2 = Vector3((float)Tre2[(i+1)*3, j],
							(float)Tre2[(i+1)*3 + 1, j],
							(float)Tre2[(i+1)*3 + 2, j]);
			triP3 = Vector3((float)Tre2[(i+1)*3, j+1],
							(float)Tre2[(i+1)*3 + 1, j+1],
							(float)Tre2[(i+1)*3 + 2, j+1]);

			Geometry::IntersectTri(triP1, triP2, triP3, rayOrigin, rayDir, interInf);

			if (interInf.Dist != 0)
			{
				// check dist
				if (nearestDist == -1 || interInf.Dist < nearestDist)
				{
					// check not back facing
					// set as nearest intersection
					nearestDist = interInf.Dist;
					nearestIdx = i * j;
					hIdx = i;
					vIdx = j;
				}
			}
			
			// t2
			triP1 = Vector3((float)Tre2[i*3, j],
							(float)Tre2[i*3 + 1, j],
							(float)Tre2[i*3 + 2, j]);
			triP2 = Vector3((float)Tre2[(i+1)*3, j+1],
							(float)Tre2[(i+1)*3 + 1, j+1],
							(float)Tre2[(i+1)*3 + 2, j+1]);
			triP3 = Vector3((float)Tre2[i*3, j+1],
							(float)Tre2[i*3 + 1, j+1],
							(float)Tre2[i*3 + 2, j+1]);

			Geometry::IntersectTri(triP1, triP2, triP3, rayOrigin, rayDir, interInf);

			if (interInf.Dist != 0)
			{
				// check dist
				if (nearestDist == -1 || interInf.Dist < nearestDist)
				{
					// check not back facing
					// set as nearest intersection
					nearestDist = interInf.Dist;
					nearestIdx = i * j;
					hIdx = i;
					vIdx = j;
				}
			}
		}
	}

	if (nearestIdx != -1)
	{
		selectedTri = nearestIdx;
		PolyIsSelected = true;

		// build outline buffer
		if (selectedPolyOutlineVB != nullptr)
			delete selectedPolyOutlineVB;

		selectedPolyOutlineVB = gcnew VertexBuffer(CustomVertex::PositionOnly::typeid, 5, graphicsDevice,
												   Usage::None, CustomVertex::PositionOnly::Format,
												   Pool::Managed);
		array<CustomVertex::PositionOnly>^ verts = (array<CustomVertex::PositionOnly>^)selectedPolyOutlineVB->Lock(0, LockFlags::None);

		int i = hIdx;
		int j = vIdx;

		verts[0].Position = Vector3((float)Tre2[i*3, j],
									(float)Tre2[i*3 + 1, j],
									(float)Tre2[i*3 + 2, j]);
		selectedPolyPoints[0] = verts[0].Position;
		SelectedPolygon[0] = Point3F(verts[0].Position);

		verts[1].Position = Vector3((float)Tre2[(i+1)*3, j],
									(float)Tre2[(i+1)*3 + 1, j],
									(float)Tre2[(i+1)*3 + 2, j]);
		selectedPolyPoints[1] = verts[1].Position;
		SelectedPolygon[1] = Point3F(verts[1].Position);

		verts[2].Position = Vector3((float)Tre2[(i+1)*3, j+1],
									(float)Tre2[(i+1)*3 + 1, j+1],
									(float)Tre2[(i+1)*3 + 2, j+1]);
		selectedPolyPoints[2] = verts[2].Position;
		SelectedPolygon[2] = Point3F(verts[2].Position);

		verts[3].Position = Vector3((float)Tre2[i*3, j+1],
									(float)Tre2[i*3 + 1, j+1],
									(float)Tre2[i*3 + 2, j+1]);
		selectedPolyPoints[3] = verts[3].Position;
		SelectedPolygon[3] = Point3F(verts[3].Position);

		verts[4].Position = verts[0].Position;

		selectedPolyOutlineVB->Unlock();
	}
	else
		PolyIsSelected = false;
}

void Model3D::SetGraphicsDevice(Device^ device)
{
	graphicsDevice = device;
}

void Model3D::BuildDxBuffers()
{
	BuildDxBoundBox();
	BuildDxStructure();
	BuildDxGrids();
}

void Model3D::BuildDxBoundBox()
{
	// reset any used buffers
	if (boundingBoxVB != nullptr)
		delete boundingBoxVB;
	if (boundingBoxIB != nullptr)
		delete boundingBoxIB;

	boundingBoxVB = nullptr;
	boundingBoxIB = nullptr;

	Device^ device = graphicsDevice;

	boundingBoxVB = gcnew VertexBuffer(CustomVertex::PositionColored::typeid, 16, device, Usage::None,
									   CustomVertex::PositionColored::Format, Pool::Managed);
	array<CustomVertex::PositionColored>^ verts = (array<CustomVertex::PositionColored>^)boundingBoxVB->Lock(0, LockFlags::None);

	// top
	verts[0].Position = Vector3((float)MAXX,//boiteenglobante[0],
								(float)MAXY,//boiteenglobante2[1],
								(float)MAXZ);//boiteenglobante2[2]);
	verts[1].Position = Vector3((float)MINX,//boiteenglobante2[3],
								(float)MAXY,//boiteenglobante2[4],
								(float)MAXZ);//boiteenglobante2[5]);
	verts[5].Position = Vector3((float)MINX,//boiteenglobante2[15],
								(float)MINY,//boiteenglobante2[16],
								(float)MAXZ);//boiteenglobante2[17]);
	verts[4].Position = Vector3((float)MAXX,//boiteenglobante[12],
								(float)MINY,//boiteenglobante2[13],
								(float)MAXZ);//boiteenglobante2[14]);
	verts[0].Color = verts[1].Color = verts[5].Color = verts[4].Color = Color::FromArgb(250, 0, 0).ToArgb();

	// bottom
	verts[2].Position = Vector3((float)boiteenglobante2[6],
								(float)boiteenglobante2[7],
								(float)boiteenglobante2[8]);
	verts[3].Position = Vector3((float)boiteenglobante2[9],
								(float)boiteenglobante2[10],
								(float)boiteenglobante2[11]);
	verts[6].Position = Vector3((float)boiteenglobante2[18],
								(float)boiteenglobante2[19],
								(float)boiteenglobante2[20]);
	verts[7].Position = Vector3((float)boiteenglobante2[21],
								(float)boiteenglobante2[22],
								(float)boiteenglobante2[23]);
	verts[2].Color = verts[3].Color = verts[6].Color = verts[7].Color = Color::FromArgb(10, 200, 200).ToArgb();

	// corners
	verts[8].Position = verts[0].Position;
	verts[9].Position = verts[3].Position;
	verts[8].Color = verts[9].Color = Color::FromArgb(0, 0, 250).ToArgb();
	verts[10].Position = verts[1].Position;
	verts[11].Position = verts[2].Position;
	verts[10].Color = verts[11].Color = Color::FromArgb(0, 0, 250).ToArgb();
	verts[12].Position = verts[5].Position;
	verts[13].Position = verts[6].Position;
	verts[12].Color = verts[13].Color = Color::FromArgb(0, 0, 250).ToArgb();
	verts[14].Position = verts[4].Position;
	verts[15].Position = verts[7].Position;
	verts[14].Color = verts[15].Color = Color::FromArgb(0, 0, 250).ToArgb();

	boundingBoxVB->Unlock();

	boundingBoxIB = gcnew IndexBuffer(short::typeid, 24, device, Usage::None, Pool::Managed);
	array<short>^ indices = (array<short>^)boundingBoxIB->Lock(0, LockFlags::None);

	// top
	indices[0] = 0; indices[1] = 1;
	indices[2] = 1; indices[3] = 5;
	indices[4] = 5; indices[5] = 4;
	indices[6] = 4; indices[7] = 0;

	// bottom
	indices[8] = 2; indices[9] = 3;
	indices[10] = 3; indices[11] = 7;
	indices[12] = 7; indices[13] = 6;
	indices[14] = 6; indices[15] = 2;

	// corners
	indices[16] = 8; indices[17] = 9;
	indices[18] = 10; indices[19] = 11;
	indices[20] = 12; indices[21] = 13;
	indices[22] = 14; indices[23] = 15;

	boundingBoxIB->Unlock();

	// set top corner positions in object space
	bbPoint1 = verts[0].Position;
	bbPoint2 = verts[1].Position;
	bbPoint3 = verts[5].Position;
	bbPoint4 = verts[4].Position;
}

void Model3D::BuildDxGrids()
{
	// reset any used buffers
	if (gridLinesXVB != nullptr)
		delete gridLinesXVB;
	if (gridLinesYVB != nullptr)
		delete gridLinesYVB;
	if (gridLinesZVB != nullptr)
		delete gridLinesZVB;

	gridLinesXVB = nullptr;
	gridLinesYVB = nullptr;
	gridLinesZVB = nullptr;

	Device^ device = graphicsDevice;
	// divide box up into grid
	int spacing = gridSpacing;
	// work out how many lines fit in each plane
	int linesInXPos = (int)(MAXX / (float)spacing);
	int linesInXNeg = (int)(-MINX / (float)spacing);

	int linesInYPos = (int)(MAXY / (float)spacing);
	int linesInYNeg = (int)(-MINY / (float)spacing);

	int linesInZPos = (int)(MAXZ / (float)spacing);
	int linesInZNeg = (int)(-MINZ / (float)spacing);

	totalGridLinesXPos = linesInXPos;
	totalGridLinesXNeg = linesInXNeg;
	totalGridLinesYPos = linesInYPos;
	totalGridLinesYNeg = linesInYNeg;
	totalGridLinesZPos = linesInZPos;
	totalGridLinesZNeg = linesInZNeg;

	totalGridLinesX = linesInYPos + linesInYNeg + 1;
	totalGridLinesY = linesInXPos + linesInXNeg + 1;
	totalGridLinesZ = linesInXPos + linesInXNeg + 1;
	int totalVertsX = totalGridLinesX * 2;
	int totalVertsY = totalGridLinesY * 2;
	int totalVertsZ = totalGridLinesZ * 2;

	gridLinesXVB = gcnew VertexBuffer(CustomVertex::PositionOnly::typeid, totalVertsX, device,
									 Usage::None, CustomVertex::PositionOnly::Format,
									 Pool::Managed);
	array<CustomVertex::PositionOnly>^ gridVerts = (array<CustomVertex::PositionOnly>^)gridLinesXVB->Lock(0, LockFlags::None);

	// do x axis
	gridVerts[0].Position = Vector3((float)center[0] + (float)MAXX, (float)center[1], (float)center[2]);
	gridVerts[1].Position = Vector3((float)center[0] + (float)MINX, (float)center[1], (float)center[2]);

	float xPos = (float)center[0];
	int vIdx = 2;
	for (int i=0; i < linesInYPos; i++)
	{
		xPos += spacing;
		gridVerts[vIdx++].Position = Vector3((float)center[0] + (float)MAXX,
											 (float)center[1] + xPos, (float)center[2]);
		gridVerts[vIdx++].Position = Vector3((float)center[0] + (float)MINX,
											 (float)center[1] + xPos, (float)center[2]);
	}
	// do x neg
	xPos = (float)center[0];
	for (int i=0; i < linesInYNeg; i++)
	{
		xPos -= spacing;
		gridVerts[vIdx++].Position = Vector3((float)center[0] + (float)MAXX,
											 (float)center[1] + xPos, (float)center[2]);
		gridVerts[vIdx++].Position = Vector3((float)center[0] + (float)MINX,
											 (float)center[1] + xPos, (float)center[2]);
	}
	gridLinesXVB->Unlock();

	gridLinesYVB = gcnew VertexBuffer(CustomVertex::PositionOnly::typeid, totalVertsY, device,
									 Usage::None, CustomVertex::PositionOnly::Format,
									 Pool::Managed);
	gridVerts = (array<CustomVertex::PositionOnly>^)gridLinesYVB->Lock(0, LockFlags::None);

	// do y pos
	gridVerts[0].Position = Vector3((float)center[0], (float)center[1] + (float)MAXY, (float)center[2]);
	gridVerts[1].Position = Vector3((float)center[0], (float)center[1] + (float)MINY, (float)center[2]);

	float yPos = (float)center[1];
	vIdx = 2;
	for (int i=0; i < linesInXPos; i++)
	{
		yPos += spacing;
		gridVerts[vIdx++].Position = Vector3((float)center[0] + yPos,
											 (float)center[1] + (float)MAXY,
											 (float)center[2]);
		gridVerts[vIdx++].Position = Vector3((float)center[0] + yPos,
											 (float)center[1] + (float)MINY,
											 (float)center[2]);
	}
	// do y neg
	yPos = (float)center[1];
	for (int i=0; i < linesInXNeg; i++)
	{
		yPos -= spacing;
		gridVerts[vIdx++].Position = Vector3((float)center[0] + yPos,
											 (float)center[1] + (float)MAXY,
											 (float)center[2]);
		gridVerts[vIdx++].Position = Vector3((float)center[0] + yPos,
											 (float)center[1] + (float)MINY,
											 (float)center[2]);
	}
	gridLinesYVB->Unlock();

	gridLinesZVB = gcnew VertexBuffer(CustomVertex::PositionOnly::typeid, totalVertsZ, device,
									 Usage::None, CustomVertex::PositionOnly::Format,
									 Pool::Managed);
	gridVerts = (array<CustomVertex::PositionOnly>^)gridLinesZVB->Lock(0, LockFlags::None);

	// do z pos
	gridVerts[0].Position = Vector3((float)center[0], (float)center[1], (float)center[2] + (float)MAXZ);
	gridVerts[1].Position = Vector3((float)center[0], (float)center[1], (float)center[2] + (float)MINZ);

	float zPos = (float)center[2];
	vIdx = 2;
	for (int i=0; i < linesInXPos; i++)
	{
		zPos += spacing;
		gridVerts[vIdx++].Position = Vector3((float)center[0] + zPos,
											 (float)center[1],
											 (float)center[2] + (float)MAXZ);
		gridVerts[vIdx++].Position = Vector3((float)center[0] + zPos,
											 (float)center[1],
											 (float)center[2] + (float)MINZ);
	}
	// do z neg
	zPos = (float)center[2];
	for (int i=0; i < linesInXNeg; i++)
	{
		zPos -= spacing;
		gridVerts[vIdx++].Position = Vector3((float)center[0] + zPos,
											 (float)center[1],
											 (float)center[2] + (float)MAXZ);
		gridVerts[vIdx++].Position = Vector3((float)center[0] + zPos,
											 (float)center[1],
											 (float)center[2] + (float)MINZ);
	}
	gridLinesZVB->Unlock();
}

void Model3D::BuildDxStructure()
{
	// reset any used buffers
	if (structureFirstHalfVB != nullptr)
		delete structureFirstHalfVB;
	if (structureSecondHalfVB != nullptr)
		delete structureSecondHalfVB;
	if (structureOutlineFirstHalfVB != nullptr)
		delete structureOutlineFirstHalfVB;
	if (structureOutlineSecondHalfVB != nullptr)
		delete structureOutlineSecondHalfVB;

	structureFirstHalfVB = nullptr;
	structureSecondHalfVB = nullptr;
	structureOutlineFirstHalfVB = nullptr;
	structureOutlineSecondHalfVB = nullptr;

	Device^ device = graphicsDevice;

	numStructureTrisFH = numStructureTrisSH = (nb_ligne - 1) * (nb_colone - 1) * 2;

	// assume all polys have 4 points
	int vCount /*= numStructureVerts*/ = (nb_ligne - 1) * (nb_colone - 1) * 6;
	structureFirstHalfVB = gcnew VertexBuffer(CustomVertex::PositionNormal::typeid, vCount, device,
									 Usage::None, CustomVertex::PositionNormal::Format,
									 Pool::Managed);
	array<CustomVertex::PositionNormal>^ verts = (array<CustomVertex::PositionNormal>^)structureFirstHalfVB->Lock(0, LockFlags::None);

	numStructureOutlineLinesFH = (nb_ligne - 1) * (nb_colone - 1) * 4;
	int oVCount = (nb_ligne - 1) * (nb_colone - 1) * 8;
	structureOutlineFirstHalfVB = gcnew VertexBuffer(CustomVertex::PositionOnly::typeid, oVCount, device,
											Usage::None, CustomVertex::PositionOnly::Format, Pool::Managed);
	array<CustomVertex::PositionOnly>^ oVerts = (array<CustomVertex::PositionOnly>^)structureOutlineFirstHalfVB->Lock(0, LockFlags::None);

	int vIdx = 0;
	int ovIdx = 0;
	Vector3 e1, e2, faceNormal;
	for (int i=0; i < nb_ligne - 1; i++)
	{
		for (int j=0; j < nb_colone - 1; j++)
		{
			// t1
			verts[vIdx++].Position = Vector3((float)Tre2[i*3, j],
											 (float)Tre2[i*3 + 1, j],
											 (float)Tre2[i*3 + 2, j]);
			verts[vIdx++].Position = Vector3((float)Tre2[(i+1)*3, j],
											 (float)Tre2[(i+1)*3 + 1, j],
											 (float)Tre2[(i+1)*3 + 2, j]);
			verts[vIdx++].Position = Vector3((float)Tre2[(i+1)*3, j+1],
											 (float)Tre2[(i+1)*3 + 1, j+1],
											 (float)Tre2[(i+1)*3 + 2, j+1]);

			// bottom
			oVerts[ovIdx++].Position = verts[vIdx-3].Position;
			oVerts[ovIdx++].Position = verts[vIdx-2].Position;
			// right
			oVerts[ovIdx++].Position = verts[vIdx-2].Position;
			oVerts[ovIdx++].Position = verts[vIdx-1].Position;

			e1 = verts[vIdx-2].Position - verts[vIdx-3].Position;
			e2 = verts[vIdx-1].Position - verts[vIdx-3].Position;
			faceNormal = Vector3::Normalize(Vector3::Cross(e1, e2));
            verts[vIdx-1].Normal = faceNormal;
			verts[vIdx-2].Normal = faceNormal;
			verts[vIdx-3].Normal = faceNormal;
			
			// t2
			verts[vIdx++].Position = Vector3((float)Tre2[i*3, j],
											 (float)Tre2[i*3 + 1, j],
											 (float)Tre2[i*3 + 2, j]);
			verts[vIdx++].Position = Vector3((float)Tre2[(i+1)*3, j+1],
											 (float)Tre2[(i+1)*3 + 1, j+1],
											 (float)Tre2[(i+1)*3 + 2, j+1]);
			verts[vIdx++].Position = Vector3((float)Tre2[i*3, j+1],
											 (float)Tre2[i*3 + 1, j+1],
											 (float)Tre2[i*3 + 2, j+1]);

			// top
			oVerts[ovIdx++].Position = verts[vIdx-2].Position;
			oVerts[ovIdx++].Position = verts[vIdx-1].Position;
			// left
			oVerts[ovIdx++].Position = verts[vIdx-1].Position;
			oVerts[ovIdx++].Position = verts[vIdx-3].Position;

            verts[vIdx-1].Normal = faceNormal;
			verts[vIdx-2].Normal = faceNormal;
			verts[vIdx-3].Normal = faceNormal;
        }
    }
	structureFirstHalfVB->Unlock();
	structureOutlineFirstHalfVB->Unlock();

	structureSecondHalfVB = gcnew VertexBuffer(CustomVertex::PositionNormal::typeid, vCount, device,
									 Usage::None, CustomVertex::PositionNormal::Format,
									 Pool::Managed);
	verts = (array<CustomVertex::PositionNormal>^)structureSecondHalfVB->Lock(0, LockFlags::None);

	vIdx = 0;
	for (int i=0; i < nb_ligne - 1; i++)
	{
		for (int j=0; j < nb_colone - 1; j++)
		{
			// t1
			verts[vIdx++].Position = Vector3((float)Tre2[i*3, j],
											 (float)Tre2[i*3 + 1, j],
											 (float)Tre2[i*3 + 2, j]);
			verts[vIdx++].Position = Vector3((float)Tre2[(i+1)*3, j+1],
										     (float)Tre2[(i+1)*3 + 1, j+1],
											 (float)Tre2[(i+1)*3 + 2, j+1]);
			verts[vIdx++].Position = Vector3((float)Tre2[(i+1)*3, j],
											 (float)Tre2[(i+1)*3 + 1, j],
											 (float)Tre2[(i+1)*3 + 2, j]);

			e1 = verts[vIdx-2].Position - verts[vIdx-3].Position;
			e2 = verts[vIdx-1].Position - verts[vIdx-3].Position;
			faceNormal = Vector3::Normalize(Vector3::Cross(e1, e2));
            verts[vIdx-1].Normal = faceNormal;
			verts[vIdx-2].Normal = faceNormal;
			verts[vIdx-3].Normal = faceNormal;
			
			// t2
			verts[vIdx++].Position = Vector3((float)Tre2[i*3, j],
											 (float)Tre2[i*3 + 1, j],
											 (float)Tre2[i*3 + 2, j]);
			verts[vIdx++].Position = Vector3((float)Tre2[i*3, j+1],
											 (float)Tre2[i*3 + 1, j+1],
											 (float)Tre2[i*3 + 2, j+1]);
			verts[vIdx++].Position = Vector3((float)Tre2[(i+1)*3, j+1],
											 (float)Tre2[(i+1)*3 + 1, j+1],
											 (float)Tre2[(i+1)*3 + 2, j+1]);

            verts[vIdx-1].Normal = faceNormal;
			verts[vIdx-2].Normal = faceNormal;
			verts[vIdx-3].Normal = faceNormal;
        }
    }
	structureSecondHalfVB->Unlock();
}

void Model3D::UpdateDxMatrix()
{
	dxMatrix->M11 = (float)mat->xx;
	dxMatrix->M12 = (float)mat->yx;
	dxMatrix->M13 = (float)mat->zx;
	dxMatrix->M14 = (float)mat->wx;

	dxMatrix->M21 = (float)mat->xy;
	dxMatrix->M22 = (float)mat->yy;
	dxMatrix->M23 = (float)mat->zy;
	dxMatrix->M24 = (float)mat->wy;

	dxMatrix->M31 = (float)mat->xz;
	dxMatrix->M32 = (float)mat->yz;
	dxMatrix->M33 = (float)mat->zz;
	dxMatrix->M34 = (float)mat->wz;

	dxMatrix->M41 = (float)mat->xo;
	dxMatrix->M42 = (float)mat->yo;
	dxMatrix->M43 = (float)mat->zo;
	dxMatrix->M44 = (float)mat->wo;
}