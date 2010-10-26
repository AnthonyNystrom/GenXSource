#include "NuGenPSurfaceModel3D.h"

using namespace Microsoft::DirectX;
using namespace System::Drawing::Drawing2D;

void Model3D::tracer3(Graphics ^g, bool drawBox)
{
	int i, j, tmp, nb_poly, nb_valid_pts;
	double a4, cosinus, c4, z_tmp;
    int current_poly; 
    int itVectorData;

    // memory reservation
    polys_->Capacity = (nb_licol*nb_licol +1);
    moitie_colone = (int)(nb_colone/2);

	if (drawBox)
	    drawBbox(g);

    // Map projection...

    for ( i=0; i < nb_ligne - coupure_ligne; ++i)
    {
        for ( j=0; j < nb_colone - coupure_col  ; ++j)
        {
            c4 = D/(Tre[i*3+2, j]-Oprime[2]);
            Tre3[i*3  , j] = c4*Tre[i*3    , j]  +demi_hauteur;
            Tre3[i*3+1, j] = c4*Tre[i*3 + 1, j]  +demi_largeur;
            Tre3[i*3+2, j] = Tre[i*3 + 2, j];
        }
    }

    //=======================================================

    if(draw_poly_normals == 1)
    {
        for ( i=0; i < nb_ligne - coupure_ligne; ++i)
            for ( j=0; j < nb_colone - coupure_col  ; ++j) {
                Nor3[i*3  , j] = 20*Nor[i*3    , j] + Tre[i*3    , j];
                Nor3[i*3+1, j] = 20*Nor[i*3 + 1, j] + Tre[i*3 + 1, j];
                Nor3[i*3+2, j] = 20*Nor[i*3 + 2, j] + Tre[i*3 + 2, j];
            }                     
                     
        for ( i=0; i < nb_ligne - coupure_ligne; ++i)
            for ( j=0; j < nb_colone - coupure_col  ; ++j) {
                c4 = D/(Nor3[i*3+2, j]-Oprime[2]);
                Nor3[i*3  , j] = c4*Nor3[i*3    , j]  ;
                Nor3[i*3+1, j] = c4*Nor3[i*3 + 1, j]  ;
                //Nor3[i*3+2, j] = Nor3[i*3 + 2, j];
            }
    }

    //=======================================================

    // We verify if there is a condition...if yes, we compute
    // projections of arrays DR, DL, HR, HL
    if(there_is_condition == 1)
    {
        for ( i=0; i < nb_ligne - coupure_ligne; ++i)
        {
            for ( j=0; j < nb_colone - coupure_col  ; ++j)
            {
                if(hidden_points[i, j] == 0)
                {

                    //DR
                    c4 = D/(DR[i*3+2, j]-Oprime[2]);
                    DR3[i*3  , j] = c4*DR[i*3    , j]  +demi_hauteur;
                    DR3[i*3+1, j] = c4*DR[i*3 + 1, j]  +demi_largeur;
                    DR3[i*3+2, j] =    DR[i*3 + 2, j];

                    //DL
                    c4 = D/(DL[i*3+2, j]-Oprime[2]);
                    DL3[i*3  , j] = c4*DL[i*3    , j]  +demi_hauteur;
                    DL3[i*3+1, j] = c4*DL[i*3 + 1, j]  +demi_largeur;
                    DL3[i*3+2, j] =    DL[i*3 + 2, j];

                    //HR
                    c4 = D/(HR[i*3+2, j]-Oprime[2]);
                    HR3[i*3  , j] = c4*HR[i*3    , j]  +demi_hauteur;
                    HR3[i*3+1, j] = c4*HR[i*3 + 1, j]  +demi_largeur;
                    HR3[i*3+2, j] =    HR[i*3 + 2, j];

                    //HL
                    c4 = D/(HL[i*3+2, j]-Oprime[2]);
                    HL3[i*3  , j] = c4*HL[i*3    , j]  +demi_hauteur;
                    HL3[i*3+1, j] = c4*HL[i*3 + 1, j]  +demi_largeur;
                    HL3[i*3+2, j] =    HL[i*3 + 2, j];
                }
            }
        }
    } // End of if(there_is_condition == 1)...


    //=================================================================

    if(draw_poly_normals == 1)
    {
        for ( i=0; i < nb_ligne - 1 - coupure_ligne; ++i)
        {
            for ( j=0; j < nb_colone - 1 - coupure_col  ; ++j)
            {  
                current_poly = j*(nb_ligne-1 ) + i;
                Nor3[i*3  , j] = -30 * Nor[i*3    , j]  + Tre[i*3    , j];
                Nor3[i*3+1, j] = -30 * Nor[i*3 + 1, j]  + Tre[i*3 + 1, j];
                Nor3[i*3+2, j] = -30 * Nor[i*3 + 2, j]  + Tre[i*3 + 2, j];

                c4 = D/(Nor3[i*3+2, j]-Oprime[2]);
                Nor3[i*3  , j] = c4*Nor3[i*3    , j] +demi_hauteur;
                Nor3[i*3+1, j] = c4*Nor3[i*3 + 1, j] +demi_largeur;
                                             
                tableau[current_poly]->Norx = (int)Nor3[i*3   , j];
                tableau[current_poly]->Nory = (int)Nor3[i*3 +1, j];

                tableau[current_poly + 10000]->Norx = (int)Nor3[i*3   , j];
                tableau[current_poly + 10000]->Nory = (int)Nor3[i*3 +1, j];
            }
        }
    }

    //================================================

    moitie_colone = (int)(nb_colone/2);
  
    if(there_is_condition != 1 || implicitdef == 1)
    { 
        // construction of polygones : first case --> No hidden points
        if(there_is_hidden_points == -1)
        {
            for ( i=0; i < nb_ligne - 1 - coupure_ligne; ++i)
                for ( j=0; j < nb_colone - 1 - coupure_col  ; ++j)
                    if(two_separate_objects == -1 || j != (moitie_colone -2))	//two separate shapes	
                        if((z_tmp=Tre[i*3+2, j]+Tre[(i+1)*3+2, j]+Tre[i*3+2, j+1]+Tre[(i+1)*3+2, j+1])/4 < 460 ||
                           clipping == -1 )
                        {

                            // Cette partie calcule le vecteur normalise qui vient de l'observateur		

							a4= (double)Math::Sqrt(
								(Oprime[0]-Tre[i*3  , j] )*(Oprime[0]-Tre[i*3  , j] )
								+(Oprime[1]-Tre[i*3+1, j] )*(Oprime[1]-Tre[i*3+1, j] ) 
								+(Oprime[2]-Tre[i*3+2, j] )*(Oprime[2]-Tre[i*3+2, j] ));

                            // Just in case...must be changed
                            if( a4 > 0.00000001)
                            {    
                                Obser[0]=(Oprime[0]-Tre[i*3  , j] )/a4;
                                Obser[1]=(Oprime[1]-Tre[i*3+1, j] )/a4;
                                Obser[2]=(Oprime[2]-Tre[i*3+2, j] )/a4;   
                            }

                            cosinus=(((Obser[0]*Nor[i*3  , j])+(Obser[1]*Nor[i*3+1, j])+(Obser[2]*Nor[i*3+2, j]))/(1));
                            // Just in case...must be changed
                            if(cosinus >1 ) cosinus = 1;
                            if(cosinus < -1) cosinus = -1;
                            /*
                              tableau[current_poly].Norx = (int)Nor3[i*3  ][j];
                              tableau[current_poly].Nory = (int)Nor3[i*3+1][j];
                            */
                            //                      TRIAGE des POLYGONES
                            //========== triage des polygone selon leurs veleurs Z et creation d'une liste
                            //========== de polygones TRIES qu'on utilisera apres pour l'affichage final.


                            current_poly = j*(nb_ligne-1 ) + i;

                            tableaureferences[current_poly]->setPoint(0,
                                                                      (int)Tre3[i*3   , j],
                                                                      (int)Tre3[i*3 +1, j]);

                            tableaureferences[current_poly]->setPoint(1,
                                                                      (int)Tre3[(i+1)*3, j],
                                                                      (int)Tre3[(i+1)*3 +1, j]);

                            tableaureferences[current_poly]->setPoint(2,
                                                                      (int)Tre3[(i+1)*3, j+1],
                                                                      (int)Tre3[(i+1)*3 +1, j+1]);

                            tableaureferences[current_poly]->setPoint(3,
                                                                      (int)Tre3[i*3, j+1],
                                                                      (int)Tre3[i*3 +1, j+1]);

                            //=============  Triage des polygones selon la valeurs de leurs profondeurs Z====

 
                            itVectorData = 0;
                            tmp=0; 

                            nb_poly = polys_->Count;
                            if( nb_poly > 0)
                            { 
                                while( tmp < nb_poly) {   
	   
                                    if( z_tmp < polys_[tmp]->val_z )
                                    {

                                        tableau[current_poly]->points = tableaureferences[current_poly];
                                        tableau[current_poly]->val_z = z_tmp;
                                        tableau[current_poly]->val_cos = cosinus;	
   

                                        polys_->Insert(itVectorData, tableau[current_poly]);
                                        tmp = nb_poly ;
                                    }  
                                    else  
									{
                                        if( tmp==(nb_poly -1))
										{
											tableau[current_poly]->points = tableaureferences[current_poly];
                                            tableau[current_poly]->val_z = z_tmp;
                                            tableau[current_poly]->val_cos = cosinus;	


                                            polys_->Add(tableau[current_poly]);
                                            tmp = nb_poly ;
                                        }
                                        else    {     
                                            tmp++ ;
                                            itVectorData++;
                                        }

                                    }


                                }

                            }
                            else { 
                                tableau[current_poly]->points = tableaureferences[current_poly];
                                tableau[current_poly]->val_z = z_tmp;
                                tableau[current_poly]->val_cos = cosinus;	  
                                polys_->Add(tableau[current_poly]);
                            }
                        }
        } // End of if(there_is_hidden_points == -1)
        // here we have some hidden points to deal with...
        else
        {
            for ( i=0; i < nb_ligne - 1 - coupure_ligne; ++i)
                for ( j=0; j < nb_colone - 1 - coupure_col  ; ++j)
                    if( (nb_valid_pts = 
                         (hidden_points[i, j]     +  
                          hidden_points[i+1, j]   +  
                          hidden_points[i+1, j+1] + 
                          hidden_points[i, j+1])  )  >=3)  

                        if(two_separate_objects == -1 || j != (moitie_colone -2))	//two separate shapes	

                            if((z_tmp=Tre[i*3+2, j]+Tre[(i+1)*3+2, j]+Tre[i*3+2, j+1]+Tre[(i+1)*3+2, j+1])/4 < 460 ||
                               clipping == -1 )
                            {

                                // Cette partie calcule le vecteur normalise qui vient de l'observateur		

								a4= (double)Math::Sqrt(
                                                 (Oprime[0]-Tre[i*3  , j] )*(Oprime[0]-Tre[i*3  , j] )
                                                 +(Oprime[1]-Tre[i*3+1, j] )*(Oprime[1]-Tre[i*3+1, j] ) 
                                                 +(Oprime[2]-Tre[i*3+2, j] )*(Oprime[2]-Tre[i*3+2, j] ));

                                // Just in case...must be changed
                                if( a4 > 0.00000001) {    
                                    Obser[0]=(Oprime[0]-Tre[i*3  , j] )/a4;
                                    Obser[1]=(Oprime[1]-Tre[i*3+1, j] )/a4;
                                    Obser[2]=(Oprime[2]-Tre[i*3+2, j] )/a4;   
                                }

                                cosinus=(((Obser[0]*Nor[i*3  , j])+(Obser[1]*Nor[i*3+1, j])+(Obser[2]*Nor[i*3+2, j]))/(1));
                                // Just in case...must be changed
                                if(cosinus >1 ) cosinus = 1;
                                if(cosinus < -1) cosinus = -1;
                                /*
                                  tableau[current_poly].Norx = (int)Nor3[i*3  ][j];
                                  tableau[current_poly].Nory = (int)Nor3[i*3+1][j];
                                */
                                //                      TRIAGE des POLYGONES
                                //========== triage des polygone selon leurs veleurs Z et creation d'une liste
                                //========== de polygones TRIES qu'on utilisera apres pour l'affichage final.


                                current_poly = j*(nb_ligne-1 ) + i;

                                if(nb_valid_pts == 4){
                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)Tre3[i*3   , j],
                                                                              (int)Tre3[i*3 +1, j]);

                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)Tre3[(i+1)*3, j],
                                                                              (int)Tre3[(i+1)*3 +1, j]);

                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)Tre3[(i+1)*3, j+1],
                                                                              (int)Tre3[(i+1)*3 +1, j+1]);

                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)Tre3[i*3, j+1],
                                                                              (int)Tre3[i*3 +1, j+1]);

                                } else { 

                                    if(hidden_points[i, j] == 0) {

                                        tableaureferences[current_poly]->setPoint(0,
                                                                                  (int)((Tre3[i*3   , j+1]+Tre3[(i+1)*3   , j])/2),
                                                                                  (int)((Tre3[i*3 +1, j+1] + Tre3[(i+1)*3 +1, j])/2));

                                        tableaureferences[current_poly]->setPoint(1,
                                                                                  (int)Tre3[(i+1)*3, j],
                                                                                  (int)Tre3[(i+1)*3 +1, j]);

                                        tableaureferences[current_poly]->setPoint(2,
                                                                                  (int)Tre3[(i+1)*3, j+1],
                                                                                  (int)Tre3[(i+1)*3 +1, j+1]);

                                        tableaureferences[current_poly]->setPoint(3,
                                                                                  (int)Tre3[i*3, j+1],
                                                                                  (int)Tre3[i*3 +1, j+1]);

                                    }

                                    else if(hidden_points[i+1, j] == 0) {
                                        tableaureferences[current_poly]->setPoint(0,
                                                                                  (int)Tre3[i*3   , j],
                                                                                  (int)Tre3[i*3 +1, j]);
                                        tableaureferences[current_poly]->setPoint(1,
                                                                                  (int)(  (Tre3[i*3, j] + Tre3[(i+1)*3, j+1] )/2),
                                                                                  (int)(  (Tre3[i*3 +1, j] + Tre3[(i+1)*3 +1, j+1])/2));
                                        tableaureferences[current_poly]->setPoint(2,
                                                                                  (int)Tre3[(i+1)*3, j+1],
                                                                                  (int)Tre3[(i+1)*3 +1, j+1]);
                                        tableaureferences[current_poly]->setPoint(3,
                                                                                  (int)Tre3[i*3, j+1],
                                                                                  (int)Tre3[i*3 +1, j+1]);
                                    }

                                    else if(hidden_points[i+1, j+1] == 0) {

                                        tableaureferences[current_poly]->setPoint(0,
                                                                                  (int)Tre3[i*3   , j],
                                                                                  (int)Tre3[i*3 +1, j]);
                                        tableaureferences[current_poly]->setPoint(1,
                                                                                  (int)Tre3[(i+1)*3, j],
                                                                                  (int)Tre3[(i+1)*3 +1, j]);
                                        tableaureferences[current_poly]->setPoint(2,
                                                                                  (int)((Tre3[(i+1)*3, j] +Tre3[i*3, j+1] )/2),
                                                                                  (int)((Tre3[(i+1)*3 +1, j] + Tre3[i*3 +1, j+1])/2));
                                        tableaureferences[current_poly]->setPoint(3,
                                                                                  (int)Tre3[i*3, j+1],
                                                                                  (int)Tre3[i*3 +1, j+1]);

                                    }

                                    else {

                                        tableaureferences[current_poly]->setPoint(0,
                                                                                  (int)Tre3[i*3   , j],
                                                                                  (int)Tre3[i*3 +1, j]);
                                        tableaureferences[current_poly]->setPoint(1,
                                                                                  (int)Tre3[(i+1)*3, j],
                                                                                  (int)Tre3[(i+1)*3 +1, j]);
                                        tableaureferences[current_poly]->setPoint(2,
                                                                                  (int)Tre3[(i+1)*3, j+1],
                                                                                  (int)Tre3[(i+1)*3 +1, j+1]);
                                        tableaureferences[current_poly]->setPoint(3,
                                                                                  (int)((Tre3[(i+1)*3, j+1] + Tre3[i*3, j])/2),
                                                                                  (int)((Tre3[(i+1)*3 +1, j+1] +Tre3[i*3 +1, j] )/2));
                                    }

                                }


                                //=============  Triage des polygones selon la valeurs de leurs profondeurs Z====

                                tmp=0;
                                itVectorData = 0;



 
                                nb_poly = polys_->Count;
                                if( nb_poly > 0)
                                {  // z_tmp= Tre[i*3+2][j]+ Tre[(i+1)*3+2][j]+ Tre[i*3+2][j+1]+ Tre[(i+1)*3+2][j+1];
                                    while( tmp < nb_poly) {   
	   
                                        if( z_tmp < polys_[tmp]->val_z )
                                            //if( z_tmp < (*itVectorData)->valeur_z )
                                        {

                                            tableau[current_poly]->points = tableaureferences[current_poly];
                                            tableau[current_poly]->val_z = z_tmp;
                                            tableau[current_poly]->val_cos = cosinus;	
   

                                            polys_->Insert(itVectorData, tableau[current_poly]);
                                            tmp = nb_poly ;
                                        }
  
                                        else   {
                                            if( tmp==(nb_poly -1)) {

                                                tableau[current_poly]->points = tableaureferences[current_poly];
                                                tableau[current_poly]->val_z = z_tmp;
                                                tableau[current_poly]->val_cos = cosinus;	


                                                polys_->Add(tableau[current_poly]);
                                                tmp = nb_poly ;
                                            }
                                            else    {     
                                                tmp++ ;
                                                itVectorData++;
                                            }

                                        }


                                    }

                                }
                                else { 
                                    //
                                    tableau[current_poly]->points = tableaureferences[current_poly];
                                    tableau[current_poly]->val_z = z_tmp;
                                    tableau[current_poly]->val_cos = cosinus;	
  
                                    polys_->Add(tableau[current_poly]);
                                }
                            }
        }

    } // End of if(there_is_condition != 1)....
    // Fin de la construction du vecteur de polygones tri�.
    // cette partie doit etre changee pour fusionner avec ce qui vient en bas (
    // quant ca va etre test�et optimiser. 
    else
    {
        for ( i=0; i < nb_ligne - 1 - coupure_ligne; ++i)
            for ( j=0; j < nb_colone - 1 - coupure_col  ; ++j)
                if( (nb_valid_pts = 
                     (hidden_points[i  , j  ] +  
                      hidden_points[i+1, j  ] +  
                      hidden_points[i+1, j+1] + 
                      hidden_points[i  , j+1]  )) >=2)
                {
                    if(two_separate_objects == -1 || j != (moitie_colone -2))	//two separate shapes	

                        if((z_tmp=Tre[i*3+2, j]+Tre[(i+1)*3+2, j]+Tre[i*3+2, j+1]+Tre[(i+1)*3+2, j+1])/4 < 460 ||
                           clipping == -1 )
                        {

                            // Cette partie calcule le vecteur normalise qui vient de l'observateur		

							a4= (double)Math::Sqrt(
                                             (Oprime[0]-Tre[i*3  , j] )*(Oprime[0]-Tre[i*3  , j] )
                                             +(Oprime[1]-Tre[i*3+1, j] )*(Oprime[1]-Tre[i*3+1, j] ) 
                                             +(Oprime[2]-Tre[i*3+2, j] )*(Oprime[2]-Tre[i*3+2, j] ));

                            // Just in case...must be changed
                            if( a4 > 0.00000001) {    
                                Obser[0]=(Oprime[0]-Tre[i*3  , j] )/a4;
                                Obser[1]=(Oprime[1]-Tre[i*3+1, j] )/a4;
                                Obser[2]=(Oprime[2]-Tre[i*3+2, j] )/a4;   
                            }

                            cosinus=(((Obser[0]*Nor[i*3  , j])+(Obser[1]*Nor[i*3+1, j])+(Obser[2]*Nor[i*3+2, j]))/(1));
                            // Just in case...must be changed
                            if(cosinus >1 ) cosinus = 1;
                            if(cosinus < -1) cosinus = -1;

                            //                      TRIAGE des POLYGONES
                            //========== triage des polygone selon leurs veleurs Z et creation d'une liste
                            //========== de polygones TRIES qu'on utilisera apres pour l'affichage final.


                            current_poly = j*(nb_ligne-1 ) + i;
                            if(nb_valid_pts == 4){
                                tableaureferences[current_poly]->setPoint(0,
                                                                          (int)Tre3[i*3   , j],
                                                                          (int)Tre3[i*3 +1, j]);

                                tableaureferences[current_poly]->setPoint(1,
                                                                          (int)Tre3[(i+1)*3   , j],
                                                                          (int)Tre3[(i+1)*3 +1, j]);

                                tableaureferences[current_poly]->setPoint(2,
                                                                          (int)Tre3[(i+1)*3   , j+1],
                                                                          (int)Tre3[(i+1)*3 +1, j+1]);

                                tableaureferences[current_poly]->setPoint(3,
                                                                          (int)Tre3[i*3   , j+1],
                                                                          (int)Tre3[i*3 +1, j+1]);

                            } // End if(nb_valid_pts == 4)
 
                            else if(nb_valid_pts == 3 ){ 



                                if(  draw_hidden_poly_and_nonhidden == 1 ) {
                                    current_poly = j*(nb_ligne-1 ) + i ;

                                    tableaureferences[current_poly + 10000]->setPoint(0,
                                                                                      (int)Tre3[i*3   , j],
                                                                                      (int)Tre3[i*3 +1, j]);

                                    tableaureferences[current_poly + 10000]->setPoint(1,
                                                                                      (int)Tre3[(i+1)*3, j],
                                                                                      (int)Tre3[(i+1)*3 +1, j]);

                                    tableaureferences[current_poly + 10000]->setPoint(2,
                                                                                      (int)Tre3[(i+1)*3, j+1],
                                                                                      (int)Tre3[(i+1)*3 +1, j+1]);

                                    tableaureferences[current_poly + 10000]->setPoint(3,
                                                                                      (int)Tre3[i*3, j+1],
                                                                                      (int)Tre3[i*3 +1, j+1]);
                                         
                                }

                                if(hidden_points[i, j] == 0) {

                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)DL3[(i)*3   , j]  ,
                                                                              (int)DL3[(i)*3 +1, j]  );
                                    //(int)(DL3[(i+1)*3   , j+1]  ),
                                    //(int)(DL3[(i+1)*3 +1, j+1]  ));

                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)Tre3[(i+1)*3, j],
                                                                              (int)Tre3[(i+1)*3 +1, j]);

                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)Tre3[(i+1)*3, j+1],
                                                                              (int)Tre3[(i+1)*3 +1, j+1]);

                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)Tre3[i*3, j+1],
                                                                              (int)Tre3[i*3 +1, j+1]);

                                }

                                else if(hidden_points[i+1, j] == 0) {
                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)Tre3[i*3   , j],
                                                                              (int)Tre3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)HL3[(i+1)*3   , j],
                                                                              (int)HL3[(i+1)*3 +1, j]);
                                    //(int)(HL3[i*3   , j+1]),
                                    //(int)(HL3[i*3 +1, j+1]));
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)Tre3[(i+1)*3, j+1],
                                                                              (int)Tre3[(i+1)*3 +1, j+1]);
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)Tre3[i*3, j+1],
                                                                              (int)Tre3[i*3 +1, j+1]);
                                }

                                else if(hidden_points[i+1, j+1] == 0) {

                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)Tre3[i*3   , j],
                                                                              (int)Tre3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)Tre3[(i+1)*3   , j],
                                                                              (int)Tre3[(i+1)*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)HR3[(i+1)*3   , j+1],
                                                                              (int)HR3[(i+1)*3 +1, j+1]);
                                    //(int)(HR3[i*3   , j]),
                                    //(int)(HR3[i*3 +1, j]));
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)Tre3[i*3   , j+1],
                                                                              (int)Tre3[i*3 +1, j+1]);
                                }

                                else {

                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)Tre3[i*3   , j],
                                                                              (int)Tre3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)Tre3[(i+1)*3, j],
                                                                              (int)Tre3[(i+1)*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)Tre3[(i+1)*3, j+1],
                                                                              (int)Tre3[(i+1)*3 +1, j+1]);
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)DR3[i*3   , j+1],
                                                                              (int)DR3[i*3 +1, j+1]);
                                    //(int)(DR3[(i+1)*3   , j]),
                                    //(int)(DR3[(i+1)*3 +1, j]));
                                }

                            } // End if(nb_valid_pts == 3 )


                            else if(nb_valid_pts == 2 ){ 
                                current_poly = j*(nb_ligne-1 ) + i;

                                if(  draw_hidden_poly_and_nonhidden == 1 ) {


                                    tableaureferences[current_poly + 10000]->setPoint(0,
                                                                                      (int)Tre3[i*3   , j],
                                                                                      (int)Tre3[i*3 +1, j]);

                                    tableaureferences[current_poly + 10000]->setPoint(1,
                                                                                      (int)Tre3[(i+1)*3, j],
                                                                                      (int)Tre3[(i+1)*3 +1, j]);

                                    tableaureferences[current_poly + 10000]->setPoint(2,
                                                                                      (int)Tre3[(i+1)*3, j+1],
                                                                                      (int)Tre3[(i+1)*3 +1, j+1]);

                                    tableaureferences[current_poly + 10000]->setPoint(3,
                                                                                      (int)Tre3[i*3, j+1],
                                                                                      (int)Tre3[i*3 +1, j+1]);                                        
                                }

                                if(hidden_points[i, j+1] == 0 && hidden_points[i, j] == 0) {
                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)DL3[i*3   , j],
                                                                              (int)DL3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)Tre3[(i+1)*3, j],
                                                                              (int)Tre3[(i+1)*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)Tre3[(i+1)*3, j+1],
                                                                              (int)Tre3[(i+1)*3 +1, j+1]);
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)DR3[i*3   , j+1],
                                                                              (int)DR3[i*3 +1, j+1]);                              
                              
                                }
                                else if (hidden_points[i, j] == 0 && hidden_points[i+1, j] == 0){
                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)DL3[i*3   , j],
                                                                              (int)DL3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)HL3[(i+1)*3, j],
                                                                              (int)HL3[(i+1)*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)Tre3[(i+1)*3, j+1],
                                                                              (int)Tre3[(i+1)*3 +1, j+1]);
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)Tre3[i*3   , j+1],
                                                                              (int)Tre3[i*3 +1, j+1]);        
                                }
                                else if(hidden_points[i+1, j] == 0 && hidden_points[i+1, j+1] == 0){
                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)Tre3[i*3   , j],
                                                                              (int)Tre3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)HL3[(i+1)*3, j],
                                                                              (int)HL3[(i+1)*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)HR3[(i+1)*3, j+1],
                                                                              (int)HR3[(i+1)*3 +1, j+1]);
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)Tre3[i*3   , j+1],
                                                                              (int)Tre3[i*3 +1, j+1]);                    
                                }
                                else if(hidden_points[i+1, j+1] == 0 && hidden_points[i, j+1] == 0){
                                    tableaureferences[current_poly]->setPoint(0,
                                                                              (int)Tre3[i*3   , j],
                                                                              (int)Tre3[i*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(1,
                                                                              (int)Tre3[(i+1)*3, j],
                                                                              (int)Tre3[(i+1)*3 +1, j]);
                                    tableaureferences[current_poly]->setPoint(2,
                                                                              (int)HR3[(i+1)*3, j+1],
                                                                              (int)HR3[(i+1)*3 +1, j+1]);
                                    tableaureferences[current_poly]->setPoint(3,
                                                                              (int)DR3[i*3   , j+1],
                                                                              (int)DR3[i*3 +1, j+1]);          
                                }
                                // if we have to draw the shape along with the Hidden polygon 

                                else {
                                    /*  if(  draw_hidden_poly_and_nonhidden == 1 ) */ {
                                        current_poly = j*(nb_ligne-1 ) + i;

                                        tableaureferences[current_poly  ]->setPoint(0,
                                                                                    (int)Tre3[i*3   , j],
                                                                                    (int)Tre3[i*3 +1, j]);

                                        tableaureferences[current_poly ]->setPoint(1,
                                                                                   //(int)(Tre3[(i+1)*3, j]),
                                                                                   //(int)(Tre3[(i+1)*3 +1, j]));
                                                                                   (int)Tre3[i*3   , j],
                                                                                   (int)Tre3[i*3 +1, j]);
                                        tableaureferences[current_poly ]->setPoint(2,
                                                                                   //(int)(Tre3[(i+1)*3, j+1]),
                                                                                   //(int)(Tre3[(i+1)*3 +1, j+1]));
                                                                                   (int)Tre3[i*3   , j],
                                                                                   (int)Tre3[i*3 +1, j]);
                                        tableaureferences[current_poly ]->setPoint(3,
                                                                                   //(int)(Tre3[i*3, j+1]),
                                                                                   //(int)(Tre3[i*3 +1, j+1]));
                                                                                   (int)Tre3[i*3   , j],
                                                                                   (int)Tre3[i*3 +1, j]);                                         
                                    }
                                }

                                // END of : if we have to draw the shape along with the Hidden polygon

                            } // End if(nb_valid_pts == 2 )



                            //=============  Triage des polygones selon la valeurs de leurs profondeurs Z====

                            tmp=0;
                            itVectorData = 0;
 
                            nb_poly = polys_->Count;
                            if( nb_poly > 0)
                            {  // z_tmp= Tre[i*3+2][j]+ Tre[(i+1)*3+2][j]+ Tre[i*3+2][j+1]+ Tre[(i+1)*3+2][j+1];
                                while( tmp < nb_poly) {   
	   
                                    if( z_tmp < polys_[tmp]->val_z )
                                        //if( z_tmp < (*itVectorData)->valeur_z )
                                    {

                                        tableau[current_poly]->points = tableaureferences[current_poly];
                                        tableau[current_poly]->val_z = z_tmp;
                                        tableau[current_poly]->val_cos = cosinus;
  
                                        polys_->Insert(itVectorData, tableau[current_poly]);
    
                                        if(  draw_hidden_poly_and_nonhidden == 1 && (nb_valid_pts == 3 || nb_valid_pts == 2) )  {

                                            tableau[current_poly + 10000]->points = tableaureferences[current_poly + 10000];
                                            tableau[current_poly + 10000]->val_z = z_tmp;
                                            tableau[current_poly + 10000]->val_cos = cosinus;
                                            tableau[current_poly + 10000]->condition_validity = -1;  

                                            polys_->Insert(itVectorData, tableau[current_poly + 10000]);  
                                        }

                                        tmp = nb_poly ;
                                    }
  
                                    else   {
                                        if( tmp==(nb_poly -1)) {

                                            if(  draw_hidden_poly_and_nonhidden == 1 && (nb_valid_pts == 3 || nb_valid_pts == 2) ) {

                                                tableau[current_poly + 10000]->points = tableaureferences[current_poly + 10000];
                                                tableau[current_poly + 10000]->val_z = z_tmp;
                                                tableau[current_poly + 10000]->val_cos = cosinus;
                                                tableau[current_poly + 10000]->condition_validity = -1;      
     
                                                polys_->Add(tableau[current_poly + 10000]);
                                            }  


                                            tableau[current_poly]->points = tableaureferences[current_poly];
                                            tableau[current_poly]->val_z = z_tmp;
                                            tableau[current_poly]->val_cos = cosinus;
                                            polys_->Add(tableau[current_poly]);

                                            tmp = nb_poly ;
                                        }
                                        else   
										{     
                                            tmp++ ;
                                            itVectorData++;
                                        }
                                    }
                                }
                            }
                            else
							{ 
                                //
                                if(  draw_hidden_poly_and_nonhidden == 1 && (nb_valid_pts == 3 || nb_valid_pts == 2) ) {
                                    tableau[current_poly + 10000]->points = tableaureferences[current_poly + 10000];
                                    tableau[current_poly + 10000]->val_z = z_tmp;
                                    tableau[current_poly + 10000]->val_cos = cosinus;
                                    tableau[current_poly + 10000]->condition_validity = -1;   
                                    polys_->Add(tableau[current_poly + 10000]);
                                }

                                tableau[current_poly]->points = tableaureferences[current_poly];
                                tableau[current_poly]->val_z = z_tmp;
                                tableau[current_poly]->val_cos = cosinus;	  
                                polys_->Add(tableau[current_poly]);
                            }
                        }
                } // End if(HP[][] +... >=2 )

        // Here we are going to store the hidden polygone
        // only if it's needed by user : draw_hidden_poly_and_nonhidden == 1
                else 
                {
  
                    // ========================================================================== 
                    if(  draw_hidden_poly_and_nonhidden == 1 )
                        if(two_separate_objects == -1 || j != (moitie_colone -2))	//two separate shapes	

                            if((z_tmp=Tre[i*3+2, j]+Tre[(i+1)*3+2, j]+Tre[i*3+2, j+1]+Tre[(i+1)*3+2, j+1])/4 < 460 ||
                               clipping == -1 )
                            {

                                // Cette partie calcule le vecteur normalise qui vient de l'observateur		

								a4= (double)Math::Sqrt(
                                                 (Oprime[0]-Tre[i*3  , j] )*(Oprime[0]-Tre[i*3  , j] )
                                                 +(Oprime[1]-Tre[i*3+1, j] )*(Oprime[1]-Tre[i*3+1, j] ) 
                                                 +(Oprime[2]-Tre[i*3+2, j] )*(Oprime[2]-Tre[i*3+2, j] ));

                                // Just in case...must be changed
                                if( a4 > 0.00000001) {    
                                    Obser[0]=(Oprime[0]-Tre[i*3  , j] )/a4;
                                    Obser[1]=(Oprime[1]-Tre[i*3+1, j] )/a4;
                                    Obser[2]=(Oprime[2]-Tre[i*3+2, j] )/a4;   
                                }

                                cosinus=(((Obser[0]*Nor[i*3  , j])+(Obser[1]*Nor[i*3+1, j])+(Obser[2]*Nor[i*3+2, j]))/(1));
                                // Just in case...must be changed
                                if(cosinus >1 ) cosinus = 1;
                                if(cosinus < -1) cosinus = -1;


                                current_poly = j*(nb_ligne-1 ) + i + 10000;

                                tableaureferences[current_poly]->setPoint(0,
                                                                          (int)Tre3[i*3   , j],
                                                                          (int)Tre3[i*3 +1, j]);

                                tableaureferences[current_poly]->setPoint(1,
                                                                          (int)Tre3[(i+1)*3, j],
                                                                          (int)Tre3[(i+1)*3 +1, j]);

                                tableaureferences[current_poly]->setPoint(2,
                                                                          (int)Tre3[(i+1)*3, j+1],
                                                                          (int)Tre3[(i+1)*3 +1, j+1]);

                                tableaureferences[current_poly]->setPoint(3,
                                                                          (int)Tre3[i*3, j+1],
                                                                          (int)Tre3[i*3 +1, j+1]);




                                //=============  Triage des polygones selon la valeurs de leurs profondeurs Z====

 
                                itVectorData = 0;
                                tmp=0; 

                                nb_poly = polys_->Count;
                                if( nb_poly > 0)
                                { 
                                    while( tmp < nb_poly) {   
	   
                                        if( z_tmp < polys_[tmp]->val_z )
                                        {

                                            tableau[current_poly]->points = tableaureferences[current_poly];
                                            tableau[current_poly]->val_z = z_tmp;
                                            tableau[current_poly]->val_cos = cosinus;	
                                            tableau[current_poly]->condition_validity = -1;   

                                            polys_->Insert(itVectorData, tableau[current_poly]);
                                            tmp = nb_poly ;
                                        }  
                                        else  
										{
                                            if( tmp==(nb_poly -1)) {

                                                tableau[current_poly]->points = tableaureferences[current_poly];
                                                tableau[current_poly]->val_z = z_tmp;
                                                tableau[current_poly]->val_cos = cosinus;	
                                                tableau[current_poly]->condition_validity = -1;

                                                polys_->Add(tableau[current_poly]);
                                                tmp = nb_poly ;
                                            }
                                            else    {     
                                                tmp++ ;
                                                itVectorData++;
                                            }
                                        }
                                    }
                                }
                                else { 
                                    tableau[current_poly]->points = tableaureferences[current_poly];
                                    tableau[current_poly]->val_z = z_tmp;
                                    tableau[current_poly]->val_cos = cosinus;
                                    tableau[current_poly]->condition_validity = -1;	  
                                    polys_->Add(tableau[current_poly]);
                                }
                            }                  
                    // ==========================================================================

                } // End else
    } // End else...(there is a condition)

    //                      Fin du triage des polygones
    //=============================================================================


    //==============================================================================

    // Optimisation: Trying to eliminate some polygons with the Zbuffer algorithm

    //==============================================================================
    double cs;

    Polygon^ prop_poly;

    Q3PointArray^ p;

    itVectorData = 0;

    nb_poly = polys_->Count;

    if (zbuffer_active_ok == 1)  {


        for (j=0; j < nb_poly   ; j++) {

            prop_poly = polys_[j];    

            p = prop_poly->points;    

            prop_poly->zbuffer_validity = 1;

            zbuffer[0, j] = p->at(0).X;
            zbuffer[1, j] = p->at(0).Y;

            zbuffer[2, j] = p->at(1).X;
            zbuffer[3, j] = p->at(1).Y;

            zbuffer[4, j] = p->at(2).X;
            zbuffer[5, j] = p->at(2).Y;

            zbuffer[6, j] = p->at(3).X;
            zbuffer[7, j] = p->at(3).Y;

            // Polygon center

            zbuffer[8,j] = (zbuffer[0,j]+zbuffer[2,j]+zbuffer[4,j]+zbuffer[6,j])/4;

            zbuffer[9,j] = (zbuffer[1,j]+zbuffer[3,j]+zbuffer[5,j]+zbuffer[7,j])/4;

            // Recherche du rayon du cerle engloban le polygon

            r1 = Math::Sqrt( (double) ((zbuffer[8, j]-zbuffer[0, j])*(zbuffer[8, j]-zbuffer[0, j]) +

                                 (zbuffer[9, j]-zbuffer[1, j])*(zbuffer[9, j]-zbuffer[1, j])));

          

            r2 = Math::Sqrt( (double) ((zbuffer[8, j]-zbuffer[2, j])*(zbuffer[8, j]-zbuffer[2, j]) +

                                 (zbuffer[9, j]-zbuffer[3, j])*(zbuffer[9, j]-zbuffer[3, j])));

          

            r3 = Math::Sqrt( (double) ((zbuffer[8, j]-zbuffer[4, j])*(zbuffer[8, j]-zbuffer[4, j]) +

                                 (zbuffer[9, j]-zbuffer[5, j])*(zbuffer[9, j]-zbuffer[5, j])));

          

            r4 = Math::Sqrt( (double) ((zbuffer[8, j]-zbuffer[6, j])*(zbuffer[8, j]-zbuffer[6, j]) +

                                 (zbuffer[9, j]-zbuffer[7, j])*(zbuffer[9, j]-zbuffer[7, j])));

            maxr = r1;

            if(maxr<r2) maxr = r2; 

            if(maxr<r3) maxr = r3; 

            if(maxr<r4) maxr = r4;

            rayon[j] = maxr;  // rayon du cerle engloban le polygon

            if(rayon[j] < 4) zbuffer[10, j] =  1; //Polygon too small
            else zbuffer[10, j] =  -1 ;     

            itVectorData++;



        }

        for (j=0; j < nb_poly   ; j++)
        {
            X_averege1  = 6*(zbuffer[0, j]-zbuffer[4, j])/7 + zbuffer[4, j];
            Y_averege1  = 6*(zbuffer[1, j]-zbuffer[5, j])/7 + zbuffer[5, j];

            X_averege2 =  (zbuffer[0, j]-zbuffer[4, j])/7 + zbuffer[4, j];
            Y_averege2 =  (zbuffer[1, j]-zbuffer[5, j])/7 + zbuffer[5, j];

            X_averege3 =  6*(zbuffer[2, j]-zbuffer[6, j])/7 + zbuffer[6, j];
            Y_averege3 =  6*(zbuffer[3, j]-zbuffer[7, j])/7 + zbuffer[7, j];

            X_averege4 =  (zbuffer[2, j]-zbuffer[6, j])/7 + zbuffer[6, j];
            Y_averege4 =  (zbuffer[3, j]-zbuffer[7, j])/7 + zbuffer[7, j];

            X_averege = zbuffer[8, j];
            Y_averege = zbuffer[9, j];

            // we are going to test if one of the remaining poly has

            // the point(X_averege,Y_averege) in it's interior.
            point_interior  = -1;
            point_interior1 = -1;
            point_interior2 = -1;
            point_interior3 = -1;
            point_interior4 = -1;

            nb_intersection  = -1;
            nb_intersection1 = -1;
            nb_intersection2 = -1;
            nb_intersection3 = -1;
            nb_intersection4 = -1;

            for(i = nb_poly -1; i > j ; i--) {

                // Compute distance between the two polygons centers

                r1 = Math::Sqrt( (double) ((zbuffer[8, i]-zbuffer[8, j])*(zbuffer[8, i]-zbuffer[8, j]) + 

                                     (zbuffer[9, i]-zbuffer[9, j])*(zbuffer[9, i]-zbuffer[9, j])));

                // Perform calculation only if two cercls intersect

                //if( r1 < (zbuffer[10][i] + zbuffer[10][j]))  {

                if( r1 < (rayon[i] + rayon[j]))  {

                    if(point_interior  == -1) nb_intersection  = -1;

                    if(point_interior1 == -1) nb_intersection1 = -1;

                    if(point_interior2 == -1) nb_intersection2 = -1;

                    if(point_interior3 == -1) nb_intersection3 = -1;

                    if(point_interior4 == -1) nb_intersection4 = -1;

                    //=================== We work with the first point ==========================

                    if( (zbuffer_quality == 1 || zbuffer_quality == 5 ) && point_interior  == -1/*nb_intersection == -1*/ ){                              

                        if ((((zbuffer[1, i]<=Y_averege) && (Y_averege<zbuffer[7, i])) ||

                             ((zbuffer[7, i]<=Y_averege) && (Y_averege<zbuffer[1, i]))) &&

                            (X_averege <  (zbuffer[6, i] - zbuffer[0, i]) * (Y_averege - zbuffer[1, i])/(zbuffer[7, i] - zbuffer[1, i]) + zbuffer[0, i]))

                            nb_intersection*=-1;                                

                        if ((((zbuffer[3, i]<=Y_averege) && (Y_averege<zbuffer[1, i])) ||

                             ((zbuffer[1, i]<=Y_averege) && (Y_averege<zbuffer[3, i]))) &&

                            (X_averege <  (zbuffer[0, i] - zbuffer[2, i]) * (Y_averege - zbuffer[3, i])/(zbuffer[1, i] - zbuffer[3, i]) + zbuffer[2, i]))

                            nb_intersection*=-1; 

                        if ((((zbuffer[5, i]<=Y_averege) && (Y_averege<zbuffer[3, i])) ||

                             ((zbuffer[3, i]<=Y_averege) && (Y_averege<zbuffer[5, i]))) &&

                            (X_averege <  (zbuffer[2, i] - zbuffer[4, i]) * (Y_averege - zbuffer[5, i])/(zbuffer[3, i] - zbuffer[5, i])  + zbuffer[4, i]))

                            nb_intersection*=-1; 

                        if ((((zbuffer[7, i]<=Y_averege) && (Y_averege<zbuffer[5, i])) ||

                             ((zbuffer[5, i]<=Y_averege) && (Y_averege<zbuffer[7, i]))) &&

                            (X_averege <  (zbuffer[4, i] - zbuffer[6, i]) * (Y_averege - zbuffer[7, i])/(zbuffer[5, i] - zbuffer[7, i]) + zbuffer[6, i]))

                            nb_intersection*=-1; 

                        if(nb_intersection == 1 ) {
                            point_interior = 1;
                            /*
                              if(zbuffer[10][i] == 1) {
                              nb_intersection1 = 
                              nb_intersection2 = 
                              nb_intersection3 =
                              nb_intersection4 = 1;  // if Poly too small, we stop looking for another intersections              
                          
                              point_interior1 = 
                              point_interior2 = 
                              point_interior3 =
                              point_interior4 = 1;                          
                              }
                            */
                        } // end of if(nb_intersection == 1 )



                    } 
                    else point_interior  = 1;

                    //End of  if( nb_intersection == -1 )...

                    //=================== We work with the second point =========================

                    if(zbuffer_quality > 1  && point_interior1  == -1 /*nb_intersection1 == -1*/ ){                              

                        if ((((zbuffer[1, i]<=Y_averege1) && (Y_averege1<zbuffer[7, i])) ||

                             ((zbuffer[7, i]<=Y_averege1) && (Y_averege1<zbuffer[1, i]))) &&

                            (X_averege1 < (zbuffer[6, i] - zbuffer[0, i]) * (Y_averege1 - zbuffer[1, i])/(zbuffer[7, i] - zbuffer[1, i]) + zbuffer[0, i]))

                            nb_intersection1*=-1;                                

                        if ((((zbuffer[3, i]<=Y_averege1) && (Y_averege1<zbuffer[1, i])) ||

                             ((zbuffer[1, i]<=Y_averege1) && (Y_averege1<zbuffer[3, i]))) &&

                            (X_averege1 < (zbuffer[0, i] - zbuffer[2, i]) * (Y_averege1 - zbuffer[3, i])/(zbuffer[1, i] - zbuffer[3, i]) + zbuffer[2, i]))

                            nb_intersection1*=-1; 

                        if ((((zbuffer[5, i]<=Y_averege1) && (Y_averege1<zbuffer[3, i])) ||

                             ((zbuffer[3, i]<=Y_averege1) && (Y_averege1<zbuffer[5, i]))) &&

                            (X_averege1 < (zbuffer[2, i] - zbuffer[4, i]) * (Y_averege1 - zbuffer[5, i])/(zbuffer[3, i] - zbuffer[5, i])  + zbuffer[4, i]))

                            nb_intersection1*=-1; 

                        if ((((zbuffer[7, i]<=Y_averege1) && (Y_averege1<zbuffer[5, i])) ||

                             ((zbuffer[5, i]<=Y_averege1) && (Y_averege1<zbuffer[7, i]))) &&

                            (X_averege1 < (zbuffer[4, i] - zbuffer[6, i]) * (Y_averege1 - zbuffer[7, i])/(zbuffer[5, i] - zbuffer[7, i]) + zbuffer[6, i]))

                            nb_intersection1*=-1; 

                        if(nb_intersection1 == 1 ) {

                            point_interior1 = 1;
                            /*
                              if(zbuffer[10][i] == 1) {
                              nb_intersection  = 
                              nb_intersection2 = 
                              nb_intersection3 =
                              nb_intersection4 = 1;  // if Poly too small, we stop looking for another intersections              
                          
                              point_interior  = 
                              point_interior2 = 
                              point_interior3 =
                              point_interior4 = 1;
                              }
                            */
                        }

                    } 
                    else point_interior1 = 1; //End of  if( nb_intersection1 == -1 )...

                    //===================== We work with the third point ========================



                    if((zbuffer_quality > 1 ) &&  point_interior2  == -1 /*nb_intersection2 == -1*/ ){                              

                        if ((((zbuffer[1, i]<=Y_averege2) && (Y_averege2<zbuffer[7, i])) ||

                             ((zbuffer[7, i]<=Y_averege2) && (Y_averege2<zbuffer[1, i]))) &&

                            (X_averege2 < (zbuffer[6, i] - zbuffer[0, i]) * (Y_averege2 - zbuffer[1, i])/(zbuffer[7, i] - zbuffer[1, i]) + zbuffer[0, i]))

                            nb_intersection2*=-1;                                

                        if ((((zbuffer[3, i]<=Y_averege2) && (Y_averege2<zbuffer[1, i])) ||

                             ((zbuffer[1, i]<=Y_averege2) && (Y_averege2<zbuffer[3, i]))) &&

                            (X_averege2 < (zbuffer[0, i] - zbuffer[2, i]) * (Y_averege2 - zbuffer[3, i])/(zbuffer[1, i] - zbuffer[3, i]) + zbuffer[2, i]))

                            nb_intersection2*=-1; 

                        if ((((zbuffer[5, i]<=Y_averege2) && (Y_averege2<zbuffer[3, i])) ||

                             ((zbuffer[3, i]<=Y_averege2) && (Y_averege2<zbuffer[5, i]))) &&

                            (X_averege2 <  (zbuffer[2, i] - zbuffer[4, i]) * (Y_averege2 - zbuffer[5, i])/(zbuffer[3, i] - zbuffer[5, i])  + zbuffer[4, i]))

                            nb_intersection2*=-1; 

                        if ((((zbuffer[7, i]<=Y_averege2) && (Y_averege2<zbuffer[5, i])) ||

                             ((zbuffer[5, i]<=Y_averege2) && (Y_averege2<zbuffer[7, i]))) &&

                            (X_averege2 < (zbuffer[4, i] - zbuffer[6, i]) * (Y_averege2 - zbuffer[7, i])/(zbuffer[5, i] - zbuffer[7, i]) + zbuffer[6, i]))

                            nb_intersection2*=-1; 

                        if(nb_intersection2 == 1 ) {

                            point_interior2 = 1;
                            /*
                              if(zbuffer[10][i] == 1) {
                              nb_intersection = 
                              nb_intersection1 = 
                              nb_intersection3 =
                              nb_intersection4 = 1;  // if Poly too small, we stop looking for another intersections              
                          
                              point_interior = 
                              point_interior1 = 
                              point_interior3 =
                              point_interior4 = 1;
                              }
                            */
                        }

                    } 


                    else point_interior2 = 1 ; //End of  if( nb_intersection == -1 )...




                    //===================== We work with the fourth point ========================



                    if( (zbuffer_quality > 2 ) && point_interior3  == -1 /*nb_intersection3 == -1*/ ){                              

                        if ((((zbuffer[1, i]<=Y_averege3) && (Y_averege3<zbuffer[7, i])) ||

                             ((zbuffer[7, i]<=Y_averege3) && (Y_averege3<zbuffer[1, i]))) &&

                            (X_averege3 <  (zbuffer[6, i] - zbuffer[0, i]) * (Y_averege3 - zbuffer[1, i])/(zbuffer[7, i] - zbuffer[1, i]) + zbuffer[0, i]))

                            nb_intersection3*=-1;                                





                        if ((((zbuffer[3, i]<=Y_averege3) && (Y_averege3<zbuffer[1, i])) ||

                             ((zbuffer[1, i]<=Y_averege3) && (Y_averege3<zbuffer[3, i]))) &&

                            (X_averege3 < (zbuffer[0, i] - zbuffer[2, i]) * (Y_averege3 - zbuffer[3, i])/(zbuffer[1, i] - zbuffer[3, i]) + zbuffer[2, i]))

                            nb_intersection3*=-1; 



                        if ((((zbuffer[5, i]<=Y_averege3) && (Y_averege3<zbuffer[3, i])) ||

                             ((zbuffer[3, i]<=Y_averege3) && (Y_averege3<zbuffer[5, i]))) &&

                            (X_averege3 <  (zbuffer[2, i] - zbuffer[4, i]) * (Y_averege3 - zbuffer[5, i])/(zbuffer[3, i] - zbuffer[5, i])  + zbuffer[4, i]))

                            nb_intersection3*=-1; 





                        if ((((zbuffer[7, i]<=Y_averege3) && (Y_averege3<zbuffer[5, i])) ||

                             ((zbuffer[5, i]<=Y_averege3) && (Y_averege3<zbuffer[7, i]))) &&

                            (X_averege3 < (zbuffer[4, i] - zbuffer[6, i]) * (Y_averege3 - zbuffer[7, i])/(zbuffer[5, i] - zbuffer[7, i]) + zbuffer[6, i]))

                            nb_intersection3*=-1; 



                        if(nb_intersection3 == 1 ) {

                            point_interior3 = 1;
                            /*
                              if(zbuffer[10][i] == 1) {
                              nb_intersection  =
                              nb_intersection1 = 
                              nb_intersection2 =
                              nb_intersection4 = 1;  // if Poly too small, we stop looking for another intersections              

                              point_interior  =                          
                              point_interior1 = 
                              point_interior2 = 
                              point_interior4 = 1;
                              }
                            */
                        }

                    } //End of  if( nb_intersection == -1 )...



                    else  point_interior3 = 1;

                    //===================== We work with the fifth point ========================

                    if( zbuffer_quality > 3  && point_interior4  == -1  /*nb_intersection4 == -1*/ ){                              

                        if ((((zbuffer[1, i]<=Y_averege4) && (Y_averege4<zbuffer[7, i])) ||

                             ((zbuffer[7, i]<=Y_averege4) && (Y_averege4<zbuffer[1, i]))) &&

                            (X_averege4 <  ((zbuffer[6, i] - zbuffer[0, i]) * (Y_averege4 - zbuffer[1, i])+(zbuffer[7, i] - zbuffer[1, i])*zbuffer[0, i])/(zbuffer[7, i] - zbuffer[1, i])))

                            nb_intersection4*=-1;                                





                        if ((((zbuffer[3, i]<=Y_averege4) && (Y_averege4<zbuffer[1, i])) ||

                             ((zbuffer[1, i]<=Y_averege4) && (Y_averege4<zbuffer[3, i]))) &&

                            (X_averege4 < ((zbuffer[0, i] - zbuffer[2, i]) * (Y_averege4 - zbuffer[3, i])+ (zbuffer[1, i] - zbuffer[3, i])*zbuffer[2, i])/(zbuffer[1, i] - zbuffer[3, i])))

                            nb_intersection4*=-1; 



                        if ((((zbuffer[5, i]<=Y_averege4) && (Y_averege4<zbuffer[3, i])) ||

                             ((zbuffer[3, i]<=Y_averege4) && (Y_averege4<zbuffer[5, i]))) &&

                            (X_averege4 < ((zbuffer[2, i] - zbuffer[4, i]) * (Y_averege4 - zbuffer[5, i])+(zbuffer[3, i] - zbuffer[5, i])*zbuffer[4, i])/(zbuffer[3, i] - zbuffer[5, i])))

                            nb_intersection4*=-1; 





                        if ((((zbuffer[7, i]<=Y_averege4) && (Y_averege4<zbuffer[5, i])) ||

                             ((zbuffer[5, i]<=Y_averege4) && (Y_averege4<zbuffer[7, i]))) &&

                            (X_averege4 < ((zbuffer[4, i] - zbuffer[6, i])*(Y_averege4 - zbuffer[7, i])+(zbuffer[5, i] - zbuffer[7, i])*zbuffer[6, i])/(zbuffer[5, i] - zbuffer[7, i])))

                            nb_intersection4*=-1; 



                        if(nb_intersection4 == 1 ) {

                            point_interior4 = 1;
                            /*
                              if(zbuffer[10][i] == 1) { 
                              nb_intersection  =
                              nb_intersection1 = 
                              nb_intersection2 =
                              nb_intersection3 = 1;  // if Poly too small, we stop looking for another intersections              

                              point_interior  =                          
                              point_interior1 = 
                              point_interior2 = 
                              point_interior3 = 1;
                              }
                            */
                        }

                    } 
                    else  point_interior4 = 1;

                    //End of  if( nb_intersection == -1 )...


                    //===================== We test if ALL points are hidden =====================



                    if( point_interior  == 1 && 
                        point_interior1 == 1 && 
                        point_interior2 == 1 && 
                        point_interior3 == 1 && 
                        point_interior4 == 1   ){  

                        prop_poly = polys_[j];

                        i = j;

                        prop_poly->zbuffer_validity = 0;

                    } 

                } // End of if( r1 < (zbuffer[10][i] + zbuffer[10][j]))    

            } // End of for(i = nb_poly -1; i > j ; i--)

            itVectorData++;
        }
    }
    //==============================================================================

    // Optimisation's End 
    //==============================================================================


    //=========================================================================
    //                    AFFICHAGE DES POLYGONES
    int Norx2, Nory2;                     
    if(mesh ==1)
    {
        Pen ^defaultPen = gcnew Pen(Color::FromArgb(gridliner,gridlineg,gridlineb));
//        g.setPen(QColor(gridliner,gridlineg,gridlineb));

        Pen ^redcol = gcnew Pen(Color::Red);

        if(interior_surface == 1 && exterior_surface == 1)
            for (j=0; j < nb_poly   ; j++) {
                prop_poly = polys_[j];
                p         = prop_poly->points;
                cs        = prop_poly->val_cos; 
                if( zbuffer_active_ok != 1 || prop_poly->zbuffer_validity == 1) { 
                    if(cs >0) {
      
                        if(  draw_hidden_poly_and_nonhidden == 1 && prop_poly->condition_validity == -1) {

                            if (draw_cond_mesh == 1)
                            {
                                // g.setPen(redcol);
                                g->DrawLines(redcol, p->Points()); // .drawPolyline(*p);
                                g->DrawLine (redcol, p->at(0), p->at(3)); //.drawLine ( p->point(0), p->point(3)); 
//                                g.setPen(QColor(gridliner,gridlineg,gridlineb));
                            }
                            else
                            {
                                // g.setBrush(palette_cond_face[(int)(cs*256)]);
                                g->FillPolygon(palette_cond_face[(int)(cs*256)], p->Points()); //drawPolygon(*p);
                            }
                        }
                        else
                        {
                            //g.setBrush(palette_back_face[(int)(cs*256)]); 
                            g->FillPolygon(palette_back_face[(int)(cs*256)], p->Points());
                        }
                    } // End if(  draw_hidden_poly_and_nonhidden == 1 && ....
                    else
                    {
                        if(  draw_hidden_poly_and_nonhidden == 1 && prop_poly->condition_validity == -1){
 
                            if(draw_cond_mesh ==1)
                            {
                                g->DrawLines(redcol, p->Points());
                                g->DrawLine (redcol, p->at(0), p->at(3)); 

                                if (draw_poly_normals == 1)
                                {
                                    Norx2 = prop_poly->points->at(0).X;
                                    Nory2 = prop_poly->points->at(0).Y;

                                    g->DrawLine(redcol, prop_poly->Norx, prop_poly->Nory, Norx2, Nory2);
                                }
//                                g.setPen(QColor(gridliner,gridlineg,gridlineb));
                            }
                            else
                            {
                                g->FillPolygon(palette_cond_face[-(int)(cs*256)], p->Points());
                                if(draw_poly_normals == 1)
                                {
                                    Norx2 = prop_poly->points->at(0).X;
                                    Nory2 = prop_poly->points->at(0).Y;
                                    g->DrawLine(redcol, prop_poly->Norx, prop_poly->Nory, Norx2, Nory2);
                                }
                            }
                        }
                        else
                        {
                            g->FillPolygon(palette_front_face[-(int)(cs*256)], p->Points());

                            if(draw_poly_normals == 1)
                            {
                                Norx2 = prop_poly->points->at(0).X;
                                Nory2 = prop_poly->points->at(0).Y;

                                g->DrawLine(defaultPen, prop_poly->Norx, prop_poly->Nory, Norx2, Nory2);
                            }
                        }
                    }
                    itVectorData++;
                } 
            } //End of if( prop_poly->zbuffer_validity == 1)
        else {
            if(interior_surface == 1 ) 
                for (j=0; j < nb_poly   ; j++) {
                    prop_poly = polys_[j];
                    p         = prop_poly->points;
                    cs        = prop_poly->val_cos; 
                    if(cs >0) 
                    {
                        if(  draw_hidden_poly_and_nonhidden == 1 && prop_poly->condition_validity == -1)
                        {
                            g->FillPolygon(palette_cond_face[(int)(cs*256)], p->Points());
                        }
                        else
                        {
                            g->FillPolygon(palette_back_face[(int)(cs*256)], p->Points());
                        }
                    } 
                    itVectorData++;
                } 

            if(exterior_surface == 1 )
                for (j=0; j < nb_poly   ; j++) {
                    prop_poly = polys_[j];
                    p         = prop_poly->points;
                    cs        = prop_poly->val_cos; 
                    if(cs <= 0) {
           
                        if(  draw_hidden_poly_and_nonhidden == 1 && prop_poly->condition_validity == -1)
                        {
                            g->FillPolygon(palette_cond_face[-(int)(cs*256)], p->Points());
                        }
                        else
                        {
                            g->FillPolygon(palette_front_face[-(int)(cs*256)], p->Points());
                        }
                    }    
                    itVectorData++;
                }
        }

    } // End of if(mesh ==1) 
    else
    {
        Pen ^pen = gcnew Pen(Color::FromArgb (gridliner,gridlineg,gridlineb));

        if(interior_surface == 1 && exterior_surface == 1)
            for (j=0; j < nb_poly   ; j++) {
                prop_poly = polys_[j];
                p         = prop_poly->points;
                cs        = prop_poly->val_cos; 
                if( zbuffer_active_ok != 1 || prop_poly->zbuffer_validity == 1) { 
                    if(cs >0) {
                        //g.setBrush(palette_back_face[(int)(cs*256)]); 
                        g->DrawPolygon(pen, p->Points());
                    }
                    else {
                        //g.setBrush(palette_front_face[-(int)(cs*256)]);
                        g->DrawPolygon(pen, p->Points());
                    }    
                    itVectorData++;
                } 
            } //End of if( prop_poly->zbuffer_validity == 1)
        else
        {
            if(interior_surface == 1 ) 
                for (j=0; j < nb_poly   ; j++) {
                    prop_poly = polys_[j];
                    p         = prop_poly->points;
                    cs        = prop_poly->val_cos; 
                    if(cs >0) {
                        //g.setBrush(palette_back_face[(int)(cs*256)]); 
                        g->DrawPolygon(pen, p->Points());
                    } 
                    itVectorData++;
                } 

            if(exterior_surface == 1 )
                for (j=0; j < nb_poly   ; j++) {
                    prop_poly = polys_[j];
                    p         = prop_poly->points;
                    cs        = prop_poly->val_cos; 
                    if(cs <= 0) {
                        //g.setBrush(palette_front_face[-(int)(cs*256)]);
                        g->DrawPolygon(pen, p->Points());
                    }    
                    itVectorData++;
                }
        }
    }

    // hidden surface removal (HSR) 

    polys_->Clear();
/*
    if(infos ==1)
    {
        if((i= nb_colone - 1 - coupure_col) <0) i=0;
        if((j= nb_ligne - 1 - coupure_ligne) <0) j=0;

        g.setPen(QColor(250,0,0));
        g.drawText(5,15,"Grid = "+QString::number(i)+" x "+QString::number(j)+" = "+QString::number(i*j));

        g.setPen(QColor(0,250,0));
        g.drawText(5, 15, "Grid = ");

        g.setPen(QColor(250,0,0));
        g.drawText(5,30,"Polygn = "+QString::number(nb_poly));

        g.setPen(QColor(0,250,0));
        g.drawText(5, 30, "Polygn = ");

        g.setPen(QColor(250,0,0));
        g.drawText(5,45,"t_Step = "+QString::number((int)(1/step)+1));

        g.setPen(QColor(0,250,0));
        g.drawText(5, 45, "t_Step = ");

        g.setPen(QColor(250,0,0));
        g.drawText(5,60,"Latency = "+QString::number(latence)+"ms");

        g.setPen(QColor(0,250,0));
        g.drawText(5, 60, "Latency = ");

        if(zbuffer_active_ok == 1 )
        {	   
            g.setPen(QColor(250,0,0));
            g.drawText(5,75,"Zbuffer = "+QString::number(zbuffer_quality));

            g.setPen(QColor(0,250,0));
            g.drawText(5, 75, "Zbuffer = ");
        }

        if(independantwindow == 1)
        {
            g.setPen(QColor(250,0,0));
            g.drawText(5,75,"  h=Help  ");
        }	   

        if(showhelp == 1)
        {
            g.setPen(QColor(250,0,0));
            g.drawText(5,75,"| h=Help | a=Anim | p=Morfh | m=Mesh | b=Box | i=Infos | c=Clip |");	
            g.setPen(QColor(250,0,0));
            g.drawText(5,95,"| key_left=line-- | key_right=line++ | key_up=column++ | key_down=column-- |");
            g.setPen(QColor(250,0,0));
            g.drawText(5,110,"| key_Prior=Grid++ | key_Next=Grid-- |");   
        }	   	   
    }        
*/
}


/// <newCode>
void Model3D::tracer3(Device ^device, bool drawBox, array<Microsoft::DirectX::Matrix>^ matStack)
{
	// rotate world
	matStack[0] = *dxMatrix;
	device->Transform->World = *dxMatrix;

    // memory reservation
    polys_->Capacity = (nb_licol*nb_licol +1);
    moitie_colone = (int)(nb_colone/2);

	if (drawBox)
	    drawBbox(device);

	if (gridMode != GridMode::None)
		DrawGrid(device, matStack);

	// draw all polygons
	device->RenderState->Lighting = true;
	device->VertexFormat = CustomVertex::PositionNormal::Format;
	Color oAmb = device->RenderState->Ambient;
	Material orig = device->Material;

	// draw polygons
	Material mt;
	mt.AmbientColor = ColorValue::FromColor(Color::DimGray);
	mt.DiffuseColor = ColorValue::FromColor(structSecondHalfMat->Diffuse);//Color::RosyBrown
	device->RenderState->DiffuseMaterialSource = ColorSource::Material;
	device->Material = mt;

	//device->RenderState->FillMode = DxFillMode;

	if (structureFirstHalfVB != nullptr)
	{
		// first half
		device->SetStreamSource(0, structureFirstHalfVB, 0);
		device->DrawPrimitives(PrimitiveType::TriangleList, 0, numStructureTrisFH);
	}

	if (structureSecondHalfVB != nullptr)
	{
		// second half
		mt.DiffuseColor = ColorValue::FromColor(structFirstHalfMat->Diffuse);
		device->Material = mt;
		device->SetStreamSource(0, structureSecondHalfVB, 0);
		device->DrawPrimitives(PrimitiveType::TriangleList, 0, numStructureTrisSH);
	}

	//device->RenderState->FillMode = Microsoft::DirectX::Direct3D::FillMode::Solid;

	// highlight selected tri
	/*if (selectedTri != -1)
	{
		device->RenderState->CullMode = Cull::None;
		device->RenderState->ZBufferEnable = false;
		mt.DiffuseColor = ColorValue::FromColor(Color::Red);
		device->Material = mt;
		int vIdx = selectedTri * 3;
		device->DrawPrimitives(PrimitiveType::TriangleList, vIdx, 10);
		device->RenderState->ZBufferEnable = true;
		device->RenderState->CullMode = Cull::CounterClockwise;
	}*/

	// draw outlines
	//device->RenderState->Lighting = true;
	//mt.AmbientColor = ColorValue::FromColor(*outlineClrBlend);
	//mt.DiffuseColor = ColorValue::FromColor(*outlineClrBlend);
	//device->Material = mt;
	////device->RenderState->AmbientColor = Color::Red.ToArgb();//(*outlineClrBlendFirstHalf).ToArgb();
	//device->VertexFormat = CustomVertex::PositionOnly::Format;
	////// first half
	//device->SetStreamSource(0, structureOutlineFirstHalfVB, 0);
	//device->DrawPrimitives(PrimitiveType::LineList, 0, numStructureOutlineLinesFH);
	//// second half
	////device->RenderState->Ambient = *outlineClrBlendSecondHalf;
	//device->SetStreamSource(0, structureOutlineSecondHalfVB, 0);
	//device->DrawPrimitives(PrimitiveType::LineList, 0, numStructureOutlineLinesSH);

	device->Material = orig;
	device->RenderState->Ambient = oAmb;
	//device->DrawIndexedPrimitives(PrimitiveType::TriangleList, 0, 0, numStructureVerts, 0, numStructureTris);

	if (PolyIsSelected)
	{
		// draw selected polygon outline
		device->RenderState->ZBufferEnable = false;
		device->RenderState->Lighting = true;
		Material mt;
		Material oMt = device->Material;
		mt.AmbientColor = ColorValue::FromColor(selectedPolyOutlineClr);
		mt.DiffuseColor = ColorValue::FromColor(selectedPolyOutlineClr);
		device->RenderState->DiffuseMaterialSource = ColorSource::Material;
		int oAmb = device->RenderState->AmbientColor;
		device->RenderState->AmbientColor = selectedPolyOutlineClr.ToArgb();
		device->Material = mt;

		device->VertexFormat = CustomVertex::PositionOnly::Format;
		device->SetStreamSource(0, selectedPolyOutlineVB, 0);
		device->DrawPrimitives(PrimitiveType::LineStrip, 0, 4);

		device->Material = oMt;
		device->RenderState->ZBufferEnable = true;
		device->RenderState->AmbientColor = oAmb;
	}
}

array<Point3F>^ Model3D::CreateStructurePointsList()
{
	int numPoints = nb_ligne * nb_colone;
	array<Point3F>^ points = gcnew array<Point3F>(numPoints);

	int pIdx = 0;
	for (int i=0; i < nb_ligne; i++)
	{
		for (int j=0; j < nb_colone; j++)
		{
			int tre2Idx = i*3;
			points[pIdx++] = Point3F((float)Tre2[tre2Idx, j],
								     (float)Tre2[tre2Idx + 1, j],
								     (float)Tre2[tre2Idx + 2, j]);
		}
	}

	return points;
}

array<array<Point3F>^>^ Model3D::CreatePointsListByLevel()
{
	array<array<Point3F>^>^ levels = gcnew array<array<Point3F>^>(nb_colone);
	for (int level=0; level < nb_colone; level++)
	{
		array<Point3F>^ points = levels[level] = gcnew array<Point3F>(nb_ligne);
		for (int point=0; point < nb_ligne; point++)
		{
			int tre2Idx = point * 3;
			points[point] = Point3F((float)Tre2[tre2Idx, level],
									(float)Tre2[tre2Idx + 1, level],
									(float)Tre2[tre2Idx + 2, level]);
		}
	}
	return levels;
}

void Model3D::SoftwarePass(Graphics^ g, bool drawBox, array<Microsoft::DirectX::Matrix>^ matStack)
{
	Device^ device = graphicsDevice;

	// calculate screen coords of top corners of box (for text rendering)
	Viewport^ viewport = device->Viewport;
	Vector3 screenPos;
	FontFamily^ ff = gcnew FontFamily("Arial");
	System::Drawing::Font^ font = gcnew System::Drawing::Font(ff, 14, FontStyle::Regular, GraphicsUnit::Pixel);
	Brush^ br = nullptr;
	if (drawBox)
	{
		screenPos = Vector3::Project(bbPoint1, viewport, matStack[2], matStack[1], matStack[0]);
		Vector2 point1 = Vector2(screenPos.X, screenPos.Y);

		screenPos = Vector3::Project(bbPoint3, viewport, matStack[2], matStack[1], matStack[0]);
		Vector2 point3 = Vector2(screenPos.X, screenPos.Y);

		screenPos = Vector3::Project(bbPoint4, viewport, matStack[2], matStack[1], matStack[0]);
		Vector2 point4 = Vector2(screenPos.X, screenPos.Y);

		// draw text for bounding box
		br = gcnew SolidBrush(Color::FromArgb(250, 250, 0));

		g->DrawString(inf_u, font, br, point3.X, point3.Y + 12);
		g->DrawString("U="+sup_u, font, br, point4.X, point4.Y + 12);

		br = gcnew SolidBrush(Color::FromArgb(0, 250, 250));

		g->DrawString(inf_v, font, br, point4.X - 12, point4.Y);
		g->DrawString("V="+sup_v, font, br, point1.X - 12, point1.Y);
	}

	// draw text for each point in selected polygon
	if (PolyIsSelected && drawSelectedPolyText)
	{
		font = gcnew System::Drawing::Font(ff, 11, FontStyle::Regular, GraphicsUnit::Pixel);
		br = gcnew SolidBrush(selectedPolyTextClr);
		Brush^ bgBr = nullptr;
		String^ text = nullptr;
		System::Globalization::CultureInfo^ c = gcnew System::Globalization::CultureInfo("en-US");
		if (decimalPoints != -1)
		{
			c->NumberFormat->NumberDecimalDigits = decimalPoints;
			c->NumberFormat->NumberDecimalSeparator = ".";
			c->NumberFormat->NumberGroupSeparator = "";
		}
		Color clr1 = Color::FromArgb(162, Color::LightSteelBlue.R, Color::LightSteelBlue.G, Color::LightSteelBlue.B);
		Color clr2 = Color::FromArgb(162, Color::White.R, Color::White.G, Color::White.B);

		// pre-calc relative point positions to average
		Vector3 avrPos3;
		for (int i=0; i < 4; i++)
		{
			avrPos3.X += selectedPolyPoints[i].X;
			avrPos3.Y += selectedPolyPoints[i].Y;
			avrPos3.Z += selectedPolyPoints[i].Z;
		}
		avrPos3.X /= 4;
		avrPos3.Y /= 4;
		avrPos3.Z /= 4;
		avrPos3 = Vector3::Project(avrPos3, viewport, matStack[2], matStack[1], matStack[0]);

		for (int i=0; i < 4; i++)
		{
			screenPos = Vector3::Project(selectedPolyPoints[i], viewport, matStack[2], matStack[1], matStack[0]);

			String^ xStr = nullptr;
			String^ yStr = nullptr;
			String^ zStr = nullptr;
			if (decimalPoints != -1)
			{
				xStr = selectedPolyPoints[i].X.ToString("N", c);
				yStr = selectedPolyPoints[i].Y.ToString("N", c);
				zStr = selectedPolyPoints[i].Z.ToString("N", c);
			}
			else
			{
				xStr = selectedPolyPoints[i].X.ToString();
				yStr = selectedPolyPoints[i].Y.ToString();
				zStr = selectedPolyPoints[i].Z.ToString();
			}

			String^ text = String::Format("[{0}, {1}, {2}]", xStr,
														     yStr,
														     zStr);
			SizeF textSz = g->MeasureString(text, font);

			// calc poitioning of text
			Point textPos;
			if (screenPos.X > avrPos3.X)
				textPos.X = (int)screenPos.X + 60;
			else
				textPos.X = (int)screenPos.X - 60;
			if (screenPos.Y > avrPos3.Y)
				textPos.Y = (int)screenPos.Y + 60;
			else
				textPos.Y = (int)screenPos.Y - 60;

			bgBr = gcnew LinearGradientBrush(PointF(0, textPos.Y-(textSz.Height/2)),
											 PointF(0, textPos.Y+(textSz.Height/2)),
											 clr1, clr2);
			
			g->DrawLine(Pens::LightSteelBlue, textPos.X, textPos.Y, (int)screenPos.X, (int)screenPos.Y);
			g->FillRectangle(bgBr, textPos.X-(textSz.Width/2), textPos.Y-(textSz.Height/2),
							 textSz.Width, textSz.Height);
			g->DrawString(text, font, br, textPos.X-(textSz.Width/2), textPos.Y-(textSz.Height/2));
		}
	}

	g->Flush();
}

void Model3D::DrawGrid(Device^ device, array<Microsoft::DirectX::Matrix>^ matStack)
{
	// draw 3d grid
	Microsoft::DirectX::Matrix oWorld = Microsoft::DirectX::Matrix::Multiply(matStack[0], Microsoft::DirectX::Matrix::Identity);

	device->VertexFormat = CustomVertex::PositionOnly::Format;

	// set grid clr
	Material mt;
	Material oMt = device->Material;
	int oAmb = device->RenderState->AmbientColor;
	device->RenderState->DiffuseMaterialSource = ColorSource::Material;
	device->RenderState->Lighting = true;

	mt.AmbientColor = ColorValue::FromColor(Color::FromArgb(gridliner, gridlineg, gridlineb));
	mt.DiffuseColor = ColorValue::FromColor(Color::Black);
	device->Material = mt;
	device->RenderState->AmbientColor = Color::FromArgb(gridliner, gridlineg, gridlineb).ToArgb();

	if (gridMode == GridMode::ThreeDimensions)
	{
		// x
		device->SetStreamSource(0, gridLinesXVB, 0);
		for (int y=0; y < totalGridLinesZPos + 1; y++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, 0, y * (float)gridSpacing) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesX);
		}
		for (int y=1; y < totalGridLinesZNeg + 1; y++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, 0, y * -(float)gridSpacing) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesX);
		}

		// y
		device->SetStreamSource(0, gridLinesYVB, 0);
		for (int z=0; z < totalGridLinesZPos + 1; z++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, 0, z * (float)gridSpacing) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesY);
		}
		for (int z=1; z < totalGridLinesZNeg + 1; z++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, 0, z * -(float)gridSpacing) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesY);
		}

		// z
		device->SetStreamSource(0, gridLinesZVB, 0);
		for (int y=0; y < totalGridLinesYPos + 1; y++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, y * (float)gridSpacing, 0) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesZ);
		}
		for (int y=1; y < totalGridLinesYNeg + 1; y++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, y * -(float)gridSpacing, 0) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesZ);
		}
	}
	else if (gridMode == GridMode::TwoDimensions)
	{
		// x
		device->SetStreamSource(0, gridLinesXVB, 0);
		device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesX);

		// y
		device->SetStreamSource(0, gridLinesYVB, 0);
		device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesY);
	}
	else if (gridMode == GridMode::ThreeDimensionsTwoPlanes)
	{
		// x
		device->SetStreamSource(0, gridLinesXVB, 0);
		device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesX);

		for (int y=1; y < totalGridLinesZPos + 1; y++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, 0, y * (float)gridSpacing) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, 1);
		}
		for (int y=1; y < totalGridLinesZNeg + 1; y++)
		{
			device->Transform->World = Microsoft::DirectX::Matrix::Translation(0, 0, y * -(float)gridSpacing) * oWorld;
			device->DrawPrimitives(PrimitiveType::LineList, 0, 1);
		}

		device->Transform->World = Microsoft::DirectX::Matrix::Identity * oWorld;

		// y
		device->SetStreamSource(0, gridLinesYVB, 0);
		device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesY);

		// z
		device->SetStreamSource(0, gridLinesZVB, 0);
		device->DrawPrimitives(PrimitiveType::LineList, 0, totalGridLinesZ);
	}

	device->Transform->World = oWorld;
	device->Material = oMt;
	device->RenderState->AmbientColor = oAmb;
}

/// </newCode>