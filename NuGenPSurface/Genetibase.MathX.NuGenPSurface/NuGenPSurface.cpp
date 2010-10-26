#include "NuGenPSurface.h"
#include "Scene3D.h"

using namespace Genetibase::MathX;
using namespace System::Threading;
using namespace Genetibase::NuGenPSurface::AvalonBridge;

void NuGenPSurface::save_changes()
{   
    objet->expression_X_save = objet->expression_X;
    objet->expression_Y_save = objet->expression_Y;
    objet->expression_Z_save = objet->expression_Z;

    objet->MINX_save = objet->MINX;
    objet->DIFX_save = objet->DIFX;

    objet->MINY_save = objet->MINY;
    objet->DIFY_save = objet->DIFY;

    objet->MINZ_save = objet->MINZ;
    objet->DIFZ_save = objet->DIFZ;
}

void NuGenPSurface::nbtwistez_changed(int n)
{
    oldnb_twistez = n;
    objet->twistez((double)n/10, (double)oldcoeff_rayonz/10);    
    Update();
}

void NuGenPSurface::coeffrayonz_changed(int n)
{
    oldcoeff_rayonz = n;
    objet->twistez((double)oldnb_twistez/10, (double)n/10);    
    Update();
}

void NuGenPSurface::nbtwistey_changed(int n)
{
    oldnb_twistey = n;
    objet->twistey((double)n/10, (double)oldcoeff_rayony/10);    
    Update();
}

void NuGenPSurface::coeffrayony_changed(int n)
{
    oldcoeff_rayony = n;
    objet->twistey((double)oldnb_twistey/10, (double)n/10);    
    Update();
}

void NuGenPSurface::nbtwistex_changed(int n)
{
    oldnb_twistex = n;
    objet->twistex((double)n/10, (double)oldcoeff_rayonx/10);    
    Update();
}

void NuGenPSurface::coeffrayonx_changed(int n)
{
    oldcoeff_rayonx = n;
    objet->twistex((double)oldnb_twistex/10, (double)n/10);    
    Update();
}

void NuGenPSurface::scalex(int n)
{   
    objet->scalex(n);
    Update();
}

void NuGenPSurface::scaley(int n)
{
    objet->scaley(n);
    Update();
}

void NuGenPSurface::scalez(int n)
{
    objet->scalez(n);
    Update();
}

void NuGenPSurface::activescalex()
{
    scalexactivated *= -1;
}

void NuGenPSurface::activescaley()
{
    scaleyactivated *= -1;
}

void NuGenPSurface::activescalez()
{
    scalezactivated *= -1;
}

void NuGenPSurface::addcondt()
{
    add_condition*= -1;
    objet->draw_hidden_poly_and_nonhidden = add_condition;
    
    Update();
}

void NuGenPSurface::meshcondt()
{
    condition_mesh *= -1;
    objet->draw_cond_mesh = condition_mesh;
    
    Update();
}

void NuGenPSurface::help()
{
    objet->showhelp *= -1;
    
    Update();
}


void NuGenPSurface::start()
{
//    QThread::start();
}

void NuGenPSurface::wait()
{
// wait from QThread - we don't use it right now
}
/*
void NuGenPSurface::run()
{
    while( anim_ok == 1 || morph_ok == 1 || anim4_ok == 1 || anim5_ok == 1 )
    {
        msleep(latence);

        if(anim5_ok  == 1)
        {
            if(objet->sixdimshapes == 1)
            {
                objet->fct_bouton_Anim6 ();
            }
            else
            {
                objet->fct_bouton_Anim5 ();
            }
        }	 
        else if(anim4_ok  == 1)
        {
            objet->fct_bouton_Anim4 ();
        }
        else if(anim_ok  == 1)
        {
            if(morph_ok == 1)
            {
                objet->fct_bouton_AnimMorph();
            }
            else
            {
                objet->fct_bouton_Anim3();
            }
        }
        else
        {
            objet->fct_bouton_Morph3();
        }

        update();
    }
}
*/

void NuGenPSurface::morph()
{
    if(anim4_ok == 1)
    {
        MessageBox::Show("Desactivate 4D Rotation");
        return ;  
    }
    else
    {
        morph_ok *=-1;
        if(morph_ok == 1)  if( anim_ok != 1) start();
        else   if( anim_ok !=1)  wait(); 
    }
}

void NuGenPSurface::anim(){
    if(anim4_ok == 1) {
        MessageBox::Show("Desactivate 4D Rotation");
        return ; 
    }
    else {
        anim_ok *=-1;
        if(anim_ok == 1)  if( morph_ok != 1) start();
        else   if( morph_ok !=1)  wait(); 
    }
}


void NuGenPSurface::anim4xy(){

    if(anim_ok == 1 || morph_ok == 1) {
        MessageBox::Show("Desactivate 3D Rotation/Morph");
        return ; 
    }
    else {
        if(anim4_ok == -1) {
            anim4_ok = 1;
            objet->tetaxy_ok *= -1;
            start();
        }
        else {
            objet->tetaxy_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1) { anim4_ok = -1; wait();}
        }
    }

}


void NuGenPSurface::anim4xz(){

    if(anim_ok == 1 || morph_ok == 1) {
        MessageBox::Show("Desactivate 3D Rotation/Morph");
        return ; 
    }
    else {
        if(anim4_ok == -1) {
            anim4_ok = 1;
            objet->tetaxz_ok *= -1;
            start();
        }
        else {
            objet->tetaxz_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
   
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1) { anim4_ok = -1; wait();}
        }
    }
}

void NuGenPSurface::anim4yz(){

    if(anim_ok == 1 || morph_ok == 1) {
        MessageBox::Show("Desactivate 3D Rotation/Morph");
        return ; 
    }
    else {
        if(anim4_ok == -1) {
            anim4_ok = 1;
            objet->tetayz_ok *= -1;
            start();
        }
        else {
            objet->tetayz_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
   
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1) { anim4_ok = -1; wait();}
        }
    }
}

void NuGenPSurface::anim4xw(){

    if(anim_ok == 1 || morph_ok == 1) {
        MessageBox::Show("Desactivate 3D Rotation/Morph");
        return ; 
    }
    else {

        if(anim4_ok == -1) {
            anim4_ok = 1;
            objet->tetaxw_ok *= -1;
            start();
        }
        else {
            objet->tetaxw_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&
   
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1  
               ) { anim4_ok = -1; wait();}
        }
    }
}

void NuGenPSurface::anim4yw(){

    if(anim_ok == 1 || morph_ok == 1) {
        MessageBox::Show("Desactivate 3D Rotation/Morph");
        return ; 
    }
    else {
        if(anim4_ok == -1) {
            anim4_ok = 1;
            objet->tetayw_ok *= -1;
            start();
        }
        else {
            objet->tetayw_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&
   
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1  
               ) { anim4_ok = -1; wait();}
        }
    }
}

void NuGenPSurface::anim4zw(){

    if(anim_ok == 1 || morph_ok == 1) {
        MessageBox::Show("Desactivate 3D Rotation/Morph");
        return ; 
    }
    else {
        if(anim4_ok == -1) {
            anim4_ok = 1;
            objet->tetazw_ok *= -1;
            start();
        }
        else {
            objet->tetazw_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&
   
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1  
               ) { anim4_ok = -1; wait();}
        }
    }
}

//============= xy 5D rotations ===============//
void NuGenPSurface::anim5xy(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetaxy_ok *= -1;
            start();
        }
        else {
            objet->tetaxy_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }

}
//============= xz 5D rotations ===============//
void NuGenPSurface::anim5xz(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetaxz_ok *= -1;
            start();
        }
        else {
            objet->tetaxz_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }

}
//============= xw 5D rotations ===============//
void NuGenPSurface::anim5xw(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetaxw_ok *= -1;
            start();
        }
        else {
            objet->tetaxw_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }

}
//============= xt 5D rotations ===============//
void NuGenPSurface::anim5xt(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetaxt_ok *= -1;
            start();
        }
        else {
            objet->tetaxt_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }

}


//============= yz 5D rotations ===============//
void NuGenPSurface::anim5yz(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetayz_ok *= -1;
            start();
        }
        else {
            objet->tetayz_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}

//============= yw 5D rotations ===============//
void NuGenPSurface::anim5yw(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetayw_ok *= -1;
            start();
        }
        else {
            objet->tetayw_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}

//============= yt 5D rotations ===============//
void NuGenPSurface::anim5yt(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetayt_ok *= -1;
            start();
        }
        else {
            objet->tetayt_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}

//============= zw 5D rotations ===============//
void NuGenPSurface::anim5zw(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetazw_ok *= -1;
            start();
        }
        else {
            objet->tetazw_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}

//============= zt 5D rotations ===============//
void NuGenPSurface::anim5zt(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetazt_ok *= -1;
            start();
        }
        else {
            objet->tetazt_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}
//============= zt 5D rotations ===============//
void NuGenPSurface::anim5wt(){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {
        if(anim5_ok == -1) {
            anim5_ok = 1;
            objet->tetawt_ok *= -1;
            start();
        }
        else {
            objet->tetawt_ok *= -1;
            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}

//============= zt 5D rotations ===============//
void NuGenPSurface::animND(int n){

    if(anim_ok == 1 || morph_ok == 1 || anim4_ok == 1) {
        MessageBox::Show("Desactivate 3D/4D  Rotation/Morph");
        return ; 
    }
    else {

        switch(n)  {
        case 1  : objet->tetaxy_ok *= -1;break;
        case 2  : objet->tetaxy_ok *= -1;break;
        case 3  : objet->tetaxz_ok *= -1;break;
        case 4  : objet->tetaxw_ok *= -1;break;
        case 5  : objet->tetayw_ok *= -1;break;
        case 6  : objet->tetazw_ok *= -1;break;
        case 7  : objet->tetaxt_ok *= -1;break;
        case 8  : objet->tetayt_ok *= -1;break;
        case 9  : objet->tetazt_ok *= -1;break;
        case 10 : objet->tetawt_ok *= -1;break;
        case 11 : objet->tetaxs_ok *= -1;break;
        case 12 : objet->tetays_ok *= -1;break;
        case 13 : objet->tetazs_ok *= -1;break;
        case 14 : objet->tetaws_ok *= -1;break;
        case 15 : objet->tetats_ok *= -1;break;
        }
        if(anim5_ok == -1) {
            anim5_ok = 1;
            start();
        }
        else {

            if(objet->tetaxy_ok == -1 && 
               objet->tetaxz_ok == -1 && 
               objet->tetayz_ok == -1 &&  
               objet->tetaxw_ok == -1 && 
               objet->tetayw_ok == -1 && 
               objet->tetazw_ok == -1 &&
               objet->tetaxt_ok == -1 &&
               objet->tetayt_ok == -1 &&
               objet->tetazt_ok == -1 &&
               objet->tetawt_ok == -1 && 
               objet->tetaxs_ok == -1 &&
               objet->tetays_ok == -1 &&
               objet->tetazs_ok == -1 &&
               objet->tetaws_ok == -1 &&
               objet->tetats_ok == -1 ) { anim5_ok = -1; wait();}
        }
    }
}

void NuGenPSurface::anim6xs(){
    animND(11);
}
void NuGenPSurface::anim6ys(){
    animND(12);
}
void NuGenPSurface::anim6zs(){
    animND(13);
}
void NuGenPSurface::anim6ws(){
    animND(14);
}
void NuGenPSurface::anim6ts(){
    animND(15);
}

void NuGenPSurface::anglexy(int cl)
{   
    objet->tetaxy = cl;
}

void NuGenPSurface::anglexz(int cl)
{   
    objet->tetaxz = cl;
}

void NuGenPSurface::angleyz(int cl)
{   
    objet->tetayz = cl;
}

void NuGenPSurface::anglexw(int cl)
{   
    objet->tetaxw = cl;
}

void NuGenPSurface::angleyw(int cl)
{   
    objet->tetayw = cl;
}

void NuGenPSurface::anglezw(int cl)
{   
    objet->tetazw = cl;
}

void NuGenPSurface::anglext(int cl)
{   
    objet->tetaxt = cl;
}

void NuGenPSurface::angleyt(int cl)
{   
    objet->tetayt = cl;
}

void NuGenPSurface::anglezt(int cl)
{   
    objet->tetazt = cl;
}

void NuGenPSurface::anglewt(int cl)
{   
    objet->tetawt = cl;
}

void NuGenPSurface::anglexs(int cl)
{   
    objet->tetaxs = cl;
}

void NuGenPSurface::angleys(int cl)
{   
    objet->tetays = cl;
}

void NuGenPSurface::anglezs(int cl)
{   
    objet->tetazs = cl;
}

void NuGenPSurface::anglews(int cl)
{   
    objet->tetaws = cl;
}

void NuGenPSurface::anglets(int cl)
{   
    objet->tetats = cl;
}

void NuGenPSurface::cutline(int cl)
{   
    objet->coupure_ligne = cl;
    Update();
}

void NuGenPSurface::step_morph(int cl)
{   
    objet->step = (double)1/(double)cl;
    
    Update();
}

void NuGenPSurface::latence_change(int cl)
{   
    latence = cl;
    objet->latence = cl;
       
    Update();
}

void NuGenPSurface::zbuffer_quality_change(int cl)
{   
    objet->zbuffer_quality = cl;
       
    Update();
}

void NuGenPSurface::zbuffer_activate()
{   
    objet->zbuffer_active_ok  *= -1;
       
    Update();
}


void NuGenPSurface::linecolumn(int cl)
{   
    objet->nb_licol = cl;
    objet->nb_colone = cl;    
    objet->nb_ligne = cl;
    objet->fct_bouton_Morph3();
       
    Update();
}


void NuGenPSurface::cutcolumn(int cl)
{   objet->coupure_col = cl;

    Update();
}


void NuGenPSurface::red(int cl)
{   switch(colortype) {

    case 0:  objet->frontsurfr = cl;objet->initialiser_palette();    
        
        Update();break;
    case 1:  objet->backsurfr = cl;objet->initialiser_palette();    
        
        Update();break;
    case 2:  objet->gridliner = cl;objet->initialiser_palette();
        
        Update();break;
    case 3:  objet->backgroundr = cl;objet->initialiser_palette();
        
        Update();break;
    };
    
}


void NuGenPSurface::green(int cl)
{
    switch(colortype) {

    case 0:  objet->frontsurfg = cl;objet->initialiser_palette();
        
        Update();break;
    case 1:  objet->backsurfg= cl;objet->initialiser_palette();
        
        Update();break;
    case 2:  objet->gridlineg = cl; objet->initialiser_palette();
        
        Update();break;
    case 3:  objet->backgroundg = cl; objet->initialiser_palette();
        
        Update();break;
    };
    
}

void NuGenPSurface::blue(int cl)
{   switch(colortype) {

    case 0:  objet->frontsurfb = cl;objet->initialiser_palette();
        
        Update();break;
    case 1:  objet->backsurfb= cl; objet->initialiser_palette();
        
        Update();break;
    case 2:  objet->gridlineb = cl;objet->initialiser_palette();
        
        Update();break;
    case 3:  objet->backgroundb = cl;objet->initialiser_palette();
        
        Update();break;
    };
    
}

void NuGenPSurface::transparence()
{   

    switch(colortype) {
    case 0:  objet->fronttrans *= -1;objet->initialiser_palette();
        
        Update();break;
    case 1:  objet->backtrans  *= -1;objet->initialiser_palette();
        
        Update();break;

    };

}

void NuGenPSurface::valueChanged()
{
    objet->fct_calcul3();
    Update();
}

void NuGenPSurface::newFile()
{
    objet->mesh *= -1;
    
    Update();
}

void NuGenPSurface::boxok()
{
    objet->box *= -1;
    
    Update();
}

void NuGenPSurface::interior(){

    objet->interior_surface *= -1;
    
    Update();
}

void NuGenPSurface::exterior(){

    objet->exterior_surface *= -1;
    
    Update();
}

void NuGenPSurface::infosok()
{
    objet->infos *= -1;
    
    Update();
}

void NuGenPSurface::clipok()
{
    objet->clipping *= -1;
    
    Update();
}

void NuGenPSurface::setFunction(int fct)
{
    if(fct == 0 )
    {
        objet->two_system = 1;

        objet->expression_X = "(3*(1+sin(v)) + 2*(1-cos(v)/2)*cos(u))*cos(v)"; 
        objet->expression_Y = "(4+2*(1-cos(v)/2)*cos(u))*sin(v)"; 
        objet->expression_Z = "-2*(1-cos(v)/2) * sin(u)"; 
        objet->inf_u = "0"; 
        objet->sup_u = "2*pi"; 
        objet->inf_v = "0"; 
        objet->sup_v = "pi";

        objet->expression_X_2 = "3*(1+sin(v))*cos(v) - 2*(1-cos(v)/2)*cos(u)"; 
        objet->expression_Y_2 = "4*sin(v)"; 
        objet->expression_Z_2 = "-2*(1-cos(v)/2)* sin(u)"; 
        objet->inf_u_2 = "0"; 
        objet->sup_u_2 = "2*pi"; 
        objet->inf_v_2 = "pi"; 
        objet->sup_v_2 = "2*pi";
    }

    objet->fct_calcul3();

    Update();
}

void NuGenPSurface::videorecord(){    
    //if(video_ok == 1) {
    //    counter = 0;
    //    f  = new QFile("movie.png" );   
    //    f->open(QIODevice::WriteOnly/* | IO_Append*/);

    //}
    //else {
    //    video_ok = -1; // to stop video recording.
    //    f->close();    
    //} 
    
}

void NuGenPSurface::screenshot(){
// FIXME: we can do this with Image too
//    if(jpg_ok == 1) pixmap->save("screenshot.jpg", "JPEG", quality_image);
//    if(png_ok == 1) pixmap->save("screenshot.png", "PNG" , quality_image);
//    if(bmp_ok == 1) pixmap->save("screenshot.bmp", "BMP" , quality_image);
}

void NuGenPSurface::png(){
    png_ok*=-1;
}

void NuGenPSurface::jpg(){
    jpg_ok*=-1;
}

void NuGenPSurface::bmp(){
    bmp_ok*=-1;
}

void NuGenPSurface::quality(int c){
    quality_image = c;
}

void NuGenPSurface::png2(){
    png2_ok*=-1;
}

void NuGenPSurface::jpg2(){
    jpg2_ok*=-1;
}

void NuGenPSurface::bmp2(){
    bmp2_ok*=-1;
}

void NuGenPSurface::quality2(int c){
    quality_image2 = c;
}

void NuGenPSurface::activate_frame(){
    frames_ok *= -1;
}

void NuGenPSurface::frame_name_short(){
    short_names *= -1;
}

void NuGenPSurface::frame_name_big(){
    big_names *= -1;
}

void NuGenPSurface::dumpMovie ()
{
    //if((video_ok == 1 || frames_ok == 1 ) && counter < 100)
    //{
    //    // this part is for the set of Frames

    //    if(frames_ok == 1)
    //    {
    //        if(short_names == 1)
    //        {
    //            if(jpg2_ok == 1 )pixmap->save(directory+"/Frame"+QString::number(counter)+".jpg", "JPEG", quality_image2);

    //            if(png2_ok == 1 )pixmap->save(directory+"/Frame"+QString::number(counter)+".png", "PNG", quality_image2);

    //            if(bmp2_ok == 1 )pixmap->save(directory+"/Frame"+QString::number(counter)+".bmp", "BMP", quality_image2);
    //        }

    //        if(big_names == 1)
    //        {
    //            if(counter<10)
    //            {
    //                if(jpg2_ok == 1) pixmap->save(directory+"/Frame0"+QString::number(counter)+".jpg", "JPEG", quality_image2);

    //                if(png2_ok == 1) pixmap->save(directory+"/Frame0"+QString::number(counter)+".png", "PNG", quality_image2);

    //                if(bmp2_ok == 1) pixmap->save(directory+"/Frame0"+QString::number(counter)+".bmp", "BMP", quality_image2);
    //            }
    //            else
    //            {
    //                if(jpg2_ok == 1) pixmap->save(directory+"/Frame"+QString::number(counter)+".jpg", "JPEG", quality_image2);

    //                if(png2_ok == 1) pixmap->save(directory+"/Frame"+QString::number(counter)+".png", "PNG", quality_image2);

    //                if(bmp2_ok == 1) pixmap->save(directory+"/Frame"+QString::number(counter)+".bmp", "BMP", quality_image2);
    //            }
    //        }
    //    }

    //    //this part is for PNG movies

    //    if(video_ok == 1)
    //    {
    //       // pngfile->packImage(pixmap->convertToImage());
    //    }

    //    // this is just for security to prevent hardrive saturation
    //    counter++; 
    //}
}

void NuGenPSurface::OnResize(EventArgs ^e)
{
	if (Width > 0 && Height > 0)
	{
		if (pixmap != nullptr)
			delete pixmap;
		pixmap = gcnew Bitmap(this->Width, this->Height);
	}

	//if (objet != nullptr)
	//{
	//	//objet->largeur_fenetre = this->Height;
	//	//objet->hauteur_fenetre = this->Width;
	//	//objet->Init();
	//}

	/// <newCode>
	CreateMatrices();
	
	if (BackgroundImage != nullptr && bgTex != nullptr)
		ReBuildBgBuffers();
	/// </newCode>
	Invalidate();
}

/// <modified>
void NuGenPSurface::OnPaint(PaintEventArgs ^e)
{
	// try a paint operation if one is not currently active
	if (System::Threading::Monitor::TryEnter(this))
	{
		if (MyRenderMode == RenderingMode::Software)
		{
			Graphics ^g;
			if (pixmap != nullptr)
			{
				g = Graphics::FromImage(pixmap);

				if (MyAntialiasing)
					g->SmoothingMode = Drawing::Drawing2D::SmoothingMode::HighQuality;
				else
					g->SmoothingMode = Drawing::Drawing2D::SmoothingMode::Default;
				
				g->Clip = gcnew System::Drawing::Region(Rectangle(0, 0, this->Width, this->Height));
				//g->FillRectangle(Brushes::Black, Rectangle(0, 0, width_, height_));
				
				SolidBrush^ myBrush;
				if (this->BackgroundImage == nullptr)
					myBrush = gcnew SolidBrush(BackgroundColor);
				else
					myBrush = gcnew SolidBrush(Color::Transparent);
				//g->FillRectangle(myBrush, Rectangle(0, 0, width_, height_));
				g->Clear(myBrush->Color);
				g->FillRectangle(myBrush, Rectangle(0, 0, this->Width, this->Height));
					
				// since it was centered with a window width of 650, translate by the current width - 650 divided by 2, same thing for the height
				g->TranslateTransform((float)(int)((this->Width - 650)/2), (float)(int)((this->Height - 650)/2));
				
				try 
				{
					objet->tracer3(g, MyDrawBorder);
				}
				catch (Exception^)
				{
					g->Clear(myBrush->Color);
					g->FillRectangle(myBrush, Rectangle(0, 0, this->Width, this->Height));
					if (myBrush->Color == Color::Red)
					{
						myBrush->Color == Color::White;
					}
					else
					{
						myBrush->Color = Color::Red;
					}
					g->DrawString("Error Drawing Image", this->Font, myBrush, (float)this->Width/2, (float)this->Height/2);
				}
				//g = this->CreateGraphics();
				//g->DrawImage(pixmap, 0, 0, pixmap->Width, pixmap->Height);
				e->Graphics->DrawImage(pixmap, 0, 0);

			   // dumpMovie (); 
			}
		}
		else if (MyRenderMode == RenderingMode::DirectX)
		{
			DrawDxFrame(e);
			// do software pass (for text etc)
			objet->SoftwarePass(e->Graphics, MyDrawBorder, matStack);
		}
		Monitor::Exit(this);
	}
}
/// </modified>

void NuGenPSurface::OnMouseDown( MouseEventArgs ^e)
{
    if ( e->Button == ::MouseButtons::Left ) btgauche = 1;
    else btgauche = 0;
    
    if ( e->Button == ::MouseButtons::Right ) btdroit = 1;
    else btdroit = 0;
    
    if ( e->Button == ::MouseButtons::Middle ) btmilieu = 1;
    else btmilieu = 0;

    objet->ancienx = e->Y;
    objet->ancieny = e->X;
}

void NuGenPSurface::OnMouseUp( MouseEventArgs ^e)
{
    if (e->Button == ::MouseButtons::Left)
        btgauche = 0;

    if (e->Button == ::MouseButtons::Right)
        btdroit = 0;

    if (e->Button == ::MouseButtons::Middle)
        btmilieu = 0;
}

void NuGenPSurface::OnMouseMove( MouseEventArgs ^e )
{
	/// <newCode>
	if ((int)e->Button > 0)
	{
		objet->anglex = (objet->ancienx - (double)e->Y);
		objet->angley = (objet->ancieny - (double)e->X);

		objet->ancienx = (double)e->Y;
		objet->ancieny = (double)e->X;

		if(btgauche==1)
			objet->fct_bouton_gauche3();

		if(btdroit==1)
		{  
			if(objet->anglex > 0 )
				objet->coefficient = 1.1f;
			else
				objet->coefficient = 0.90f;

			objet->fct_bouton_droit3();
		}

		if(btmilieu == 1)
			objet->fct_bouton_milieu3();
	    
		Refresh();
	//    Update();
	}
	/// </newCode>
}

void NuGenPSurface::UpdateObjectColors()
{
	objet->frontsurfr = MyFrontSurfaceColor.R;
	objet->frontsurfg = MyFrontSurfaceColor.G;
	objet->frontsurfb = MyFrontSurfaceColor.B;

	objet->backsurfr = MyBackSurfaceColor.R;
	objet->backsurfg = MyBackSurfaceColor.G;
	objet->backsurfb = MyBackSurfaceColor.B;

	objet->gridliner = MyGridLineColor.R;
	objet->gridlineg = MyGridLineColor.G;
	objet->gridlineb = MyGridLineColor.B;

	objet->initialiser_palette();
}

void NuGenPSurface::UpdateQuality()
{
	if (MyQuality == QualityEnum::Lowest)
	{
		objet->nb_licol = 25;
	}
	else if (MyQuality == QualityEnum::Low)
	{
		objet->nb_licol = 30;
	}
	else if (MyQuality == QualityEnum::Medium)
	{
		objet->nb_licol = 35;
	}	
		else if (MyQuality == QualityEnum::High)
	{
		objet->nb_licol = 40;
	}	
		else if (MyQuality == QualityEnum::Highest)
	{
		objet->nb_licol = 50;
	}

	objet->zbuffer_quality = MyOptimizeLevel;
	objet->Init();

	/// <newCode>
	delete shapePointCache;
	shapePointCache = objet->CreateStructurePointsList();
	objet->PolyIsSelected = false;
	/// </newCode>
}

void NuGenPSurface::OnKeyPress ( KeyPressEventArgs ^e )
{
    objet->keyboard = "" + e->KeyChar;
    Update();
}

void NuGenPSurface::OpenSaveImageDialog()
{
	  SaveFileDialog^ saveFileDialog1 = gcnew SaveFileDialog;

      saveFileDialog1->Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif";
      saveFileDialog1->FilterIndex = 1;
	  saveFileDialog1->AddExtension = true;
      saveFileDialog1->RestoreDirectory = true;

      if ( saveFileDialog1->ShowDialog() == ::DialogResult::OK )
      {
			// call SaveImageToFile
		  ImageFormat^ format;
		  if (saveFileDialog1->FilterIndex == 1)
			  format = ImageFormat::Bmp;
		  else if (saveFileDialog1->FilterIndex == 2)
			  format = ImageFormat::Jpeg;
		  else if (saveFileDialog1->FilterIndex == 3)
			  format = ImageFormat::Gif;
		  else if (saveFileDialog1->FilterIndex == 4)
			  format = ImageFormat::Tiff;

		  SaveImageToFile(saveFileDialog1->FileName, format);
      }

}

void NuGenPSurface::SaveImageToFile(String ^filepath, ImageFormat ^format)
{
	if (MyRenderMode == RenderingMode::Software)
		pixmap->Save(filepath, format);
	else if (MyRenderMode == RenderingMode::DirectX)
	{
		Surface^ backbuffer = graphicsDevice->GetBackBuffer(0, 0, BackBufferType::Mono);
		SurfaceLoader::Save(filepath, ImageFileFormat::Jpg, backbuffer);
		delete backbuffer;
	}
}

void NuGenPSurface::UpdateShape()
{
/*
reDraw = false;
objet->fivedimshapes = 1;
Expression_X = ("v *cos(u) -0.5*v^2*cos(2*u)");
Expression_Y = ("-v*sin(u) -0.5*v^2*sin(2*u)");
  Expression_Z = ("4* v^1.5 *cos(3* u / 2) / 3") ;
Expression_W = ("cos(u)") ;
Expression_T = ("cos(u)") ;

  uMin = ("0") ; 
  uMax = ("4*pi") ; 
  vMin = ("0") ; 
  vMax = ("1") ; 

  reDraw = true;
	valueChanged();
	Refresh();
	return;
*/
	reDraw = false;
	if (myShape == Shapes::Prism)
    {
        objet->two_system = -1;
        Expression_X = ("cos(u)*cos(v)*(abs(cos(3*v/4))^500 + abs(sin(3*v/4))^500)^(-1/260)*(abs(cos(4*u/4))^200 + abs(sin(4*u/4))^200)^(-1/200)");
        Expression_Y = ("cos(u)*sin(v)*(abs(cos(3*v/4))^500 + abs(sin(3*v/4))^500)^(-1/260)*(abs(cos(4*u/4))^200 + abs(sin(4*u/4))^200)^(-1/200)");
        Expression_Z = ("sin(u)*(abs(cos(4*u/4))^200 + abs(sin(4*u/4))^200)^(-1/200)");
        uMin = ("-pi/2");
        uMax = ("pi/2");
        vMin = ("0");
        vMax = ("2*pi");
    }
    else if (myShape == Shapes::Cube)
    {
        objet->two_system = -1;
        Expression_X = ("cos(u)*cos(v)*(abs(cos(4*u/4))^100 + abs(sin(4*u/4))^100)^(-1/100)*(abs(cos(4*v/4))^100 + abs(sin(4*v/4))^100)^(-1/100)");
        Expression_Y = ("cos(u)*sin(v)*(abs(cos(4*u/4))^100 + abs(sin(4*u/4))^100)^(-1/100)*(abs(cos(4*v/4))^100 + abs(sin(4*v/4))^100)^(-1/100)");
        Expression_Z = ("sin(u)*(abs(cos(4*u/4))^100 + abs(sin(4*u/4))^100)^(-1/100)");
        uMin = ("-pi/2");
        uMax = ("pi/2");
        vMin = ("0");
        vMax = ("2*pi");
    }
	else if (myShape == Shapes::Hexagon)
	{
		objet->two_system = -1;
		Expression_X = ("cos(u)*cos(v)*(abs(cos(4*u/4))^300 + abs(sin(4*u/4))^300)^(-1/300)*(abs(cos(6*v/4))^400 + abs(sin(6*v/4))^400)^(-1/1000)");
		Expression_Y = ("cos(u)*sin(v)*(abs(cos(4*u/4))^300 + abs(sin(4*u/4))^300)^(-1/300)*(abs(cos(6*v/4))^400 + abs(sin(6*v/4))^400)^(-1/1000)");
		Expression_Z = ("sin(u)*(abs(cos(4*u/4))^300 + abs(sin(4*u/4))^300)^(-1/300)");
		uMin = ("-pi/2");
		uMax = ("pi/2");
		vMin = ("0");
		vMax = ("2*pi");
	}
    else if (myShape == Shapes::Cone)
    {
        objet->two_system = -1;
        Expression_X = ("cos(u)*cos(v)*(abs(cos(4*u/4))^1 + abs(sin(4*u/4))^1)^(-1/1)*(abs(cos(4*v/4))^1 + abs(sin(4*v/4))^1)^(-1/100)");
        Expression_Y = ("cos(u)*sin(v)*(abs(cos(4*u/4))^1 + abs(sin(4*u/4))^1)^(-1/1)*(abs(cos(4*v/4))^1 + abs(sin(4*v/4))^1)^(-1/100)");
        Expression_Z = ("sin(u)*(abs(cos(4*u/4))^1 + abs(sin(4*u/4))^1)^(-1/1)");
        uMin = ("-pi/2");
        uMax = ("pi/2");
        vMin = ("0");
        vMax = ("2*pi");
    }
    else if (myShape == Shapes::Diamond)
    {
        objet->two_system = -1;
        Expression_X = ("cos(u)*cos(v)*(abs(cos(4*u/4))^1 + abs(sin(4*u/4))^1)^(-1/1)*(abs(cos(4*v/4))^1 + abs(sin(4*v/4))^1)^(-1/1)");
        Expression_Y = ("cos(u)*sin(v)*(abs(cos(4*u/4))^1 + abs(sin(4*u/4))^1)^(-1/1)*(abs(cos(4*v/4))^1 + abs(sin(4*v/4))^1)^(-1/1)");
        Expression_Z = ("sin(u)*(abs(cos(4*u/4))^1 + abs(sin(4*u/4))^1)^(-1/1)");
        uMin = ("-pi/2");
        uMax = ("pi/2");
        vMin = ("0");
        vMax = ("2*pi");
    }
        else if (myShape == Shapes::Shape_10)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*(abs(cos(3*u/4))^1 + abs(sin(3*u/4))^1)^(-1/1)*(abs(cos(6*v/4))^1 + abs(sin(6*v/4))^1)^(-1/1)");
            Expression_Y = ("cos(u)*sin(v)*(abs(cos(3*u/4))^1 + abs(sin(3*u/4))^1)^(-1/1)*(abs(cos(6*v/4))^1 + abs(sin(6*v/4))^1)^(-1/1)");
            Expression_Z = ("sin(u)*(abs(cos(3*u/4))^1 + abs(sin(3*u/4))^1)^(-1/1)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Star_7)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*(abs(cos(7*v/4))^1.7 + abs(sin(7*v/4))^1.7)^(-1/0.2)*(abs(cos(7*u/4))^1.7 + abs(sin(7*u/4))^1.7)^(-1/0.2)");
            Expression_Y = ("cos(u)*sin(v)*(abs(cos(7*v/4))^1.7 + abs(sin(7*v/4))^1.7)^(-1/0.2)*(abs(cos(7*u/4))^1.7 + abs(sin(7*u/4))^1.7)^(-1/0.2)");
            Expression_Z = ("sin(u)*(abs(cos(7*u/4))^1.7 + abs(sin(7*u/4))^1.7)^(-1/0.2)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Shape_8)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*(abs(cos(3*u/4))^100 + abs(sin(3*u/4))^100)^(-1/100)*(abs(cos(2*v/4))^0.3 + abs(sin(2*v/4))^0.2)^(-1/0.7)");
            Expression_Y = ("cos(u)*sin(v)*(abs(cos(3*u/4))^100 + abs(sin(3*u/4))^100)^(-1/100)*(abs(cos(2*v/4))^0.3 + abs(sin(2*v/4))^0.2)^(-1/0.7)");
            Expression_Z = ("sin(u)*(abs(cos(3*u/4))^100 + abs(sin(3*u/4))^100)^(-1/100)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Shape_9)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*(abs(cos(2*u/4))^10 + abs(sin(2*u/4))^10)^(-1/10)*(abs(cos(8*v/4))^100 + abs(sin(8*v/4))^30)^(-1/60)");
            Expression_Y = ("cos(u)*sin(v)*(abs(cos(2*u/4))^10 + abs(sin(2*u/4))^10)^(-1/10)*(abs(cos(8*v/4))^100 + abs(sin(8*v/4))^30)^(-1/60)");
            Expression_Z = ("sin(u)*(abs(cos(2*u/4))^10 + abs(sin(2*u/4))^10)^(-1/10)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Star)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*(abs(cos(1*u/4))^0.5 + abs(sin(1*u/4))^0.5)^(-1/0.3)*(abs(cos(5*v/4))^1.7 + abs(sin(5*v/4))^1.7)^(-1/0.1)");
            Expression_Y = ("cos(u)*sin(v)*(abs(cos(1*u/4))^0.5 + abs(sin(1*u/4))^0.5)^(-1/0.3)*(abs(cos(5*v/4))^1.7 + abs(sin(5*v/4))^1.7)^(-1/0.1)");
            Expression_Z = ("sin(u)*(abs(cos(1*u/4))^0.5 + abs(sin(1*u/4))^0.5)^(-1/0.3)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Implicit_Lemniscape)
        {
            objet->two_system = -1;
            Expression_X = ("cos(v)*sqrt(abs(sin(2*u)))*cos(u)");
            Expression_Y = ("cos(v)*sqrt(abs(sin(2*u)))*sin(u)");
            Expression_Z = ("(cos(v)*sqrt(abs(sin(2*u)))*cos(u))^2 - (cos(v)*sqrt(abs(sin(2*u)))*sin(u))^2 + 2*(cos(v)*sqrt(abs(sin(2*u)))*cos(u))*(cos(v)*sqrt(abs(sin(2*u)))*sin(u))*(tan(v)^2)");
            uMin = ("0");
            uMax = ("pi");
            vMin = ("0");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Twisted_heart)
        {
            objet->two_system = -1;
            Expression_X = ("( abs(v) - abs(u) - abs(tanh((1/sqrt(2))*u)/(1/sqrt(2))) + abs(tanh((1/sqrt(2))*v)/(1/sqrt(2))) )*sin(v)");
            Expression_Y = ("( abs(v) - abs(u) - abs(tanh((1/sqrt(2))*u)/(1/sqrt(2))) - abs(tanh((1/sqrt(2))*v)/(1/sqrt(2))) )*cos(v)");
            Expression_Z = ("(1/sqrt(2))*( u^2 + v^2 ) / ( cosh((1/sqrt(2))*u)*cosh((1/sqrt(2))*v) )");
            uMin = ("0");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Folium)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u) *(2*v/pi - tanh(v))");
            Expression_Y = ("cos(u + 2*pi / 3) / cosh(v)");
            Expression_Z = ("cos(u - 2*pi / 3) / cosh(v)");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Heart)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*(4*sqrt(1-v^2)*sin(abs(u))^abs(u))");
            Expression_Y = ("sin(u) *(4*sqrt(1-v^2)*sin(abs(u))^abs(u))");
            Expression_Z = ("v");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-1");
            vMax = ("1");
        }
        else if (myShape == Shapes::Bow_Tie)
        {
            objet->two_system = -1;
            Expression_X = ("sin(u) / (sqrt(2) + sin(v))");
            Expression_Y = ("sin(u) / (sqrt(2) + sin(v))");
            Expression_Z = ("cos(u) / (1 + sqrt(2))");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Triaxial_Hexatorus)
        {
            objet->two_system = -1;
            Expression_X = ("sin(u) / (sqrt(2) + cos(v))");
            Expression_Y = ("sin(u+2*pi/3) / (sqrt(2) + cos(v+2*pi/3))");
            Expression_Z = ("cos(u-2*pi/3) / (sqrt(2) + cos(v-2*pi/3))");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Ghost_Plane)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*sinh(v) / (cosh(v) - cos(u))");
            Expression_Y = ("cos(u)*sin(u) / (cosh(v) - cos(u))");
            Expression_Z = ("sin(u)");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Bent_Horns)
        {
            objet->two_system = -1;
            Expression_X = ("(2 + cos(u))*(v/3 - sin(v))");
            Expression_Y = ("(2 + cos(u - 2*pi / 3)) *(cos(v) - 1) ");
            Expression_Z = ("(2 + cos(u + 2*pi / 3))*(cos(v) - 1) ");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-2*pi");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Richmond)
        {
            objet->two_system = -1;
            Expression_X = ("(-3* u - u*5 + 2*u^3*v^2 + 3*u*v^4) / (6*(u*u + v*v))");
            Expression_Y = ("(-3*v - 3*u^4*v - 2*u^2*v^3 + v^5) / (6*(u*u + v*v))");
            Expression_Z = ("u");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Kidney)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u) *(3  *cos(v) - cos(3  *v))");
            Expression_Y = ("sin(u)  *(3  *cos(v) - cos(3 * v))");
            Expression_Z = ("3  *sin(v) - sin(3 * v)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Kinky_Torus)
        {
            objet->two_system = -1;
            Expression_X = ("1/cosh(u) - cos(v)");
            Expression_Y = ("sin(v)");
            Expression_Z = ("u / pi - tanh(v)");
            uMin = ("-2*pi");
            uMax = ("2*pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Snail)
        {
            objet->two_system = -1;
            Expression_X = ("u*cos(v)*sin(u)");
            Expression_Y = ("u*cos(u)*cos(v)");
            Expression_Z = ("-u*sin(v)");
            uMin = ("0");
            uMax = ("2");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Limpet_Torus)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u) / (sqrt(2) + sin(v))");
            Expression_Y = ("sin(u) / (sqrt(2) + sin(v))");
            Expression_Z = ("1 / (sqrt(2) + cos(v))");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Twisted_Triaxial)
        {
            objet->two_system = -1;
            Expression_X = ("(1-sqrt(u*u + v*v) / sqrt(2*pi*pi))*cos(u)*cos(v)+sqrt(u*u + v*v) / sqrt(2*pi*pi)*sin(u)*sin(v)");
            Expression_Y = ("(1-sqrt(u*u + v*v) / sqrt(2*pi*pi))*cos(u+2*pi/3)*cos(v+2*pi/3)+sqrt(u*u + v*v) / sqrt(2*pi*pi)*sin(u+2*pi/3)*sin(v+2*pi/3)");
            Expression_Z = (" (1-sqrt(u*u + v*v) / sqrt(2*pi*pi))*cos(u+4*pi/3)*cos(v+4*pi/3)+sqrt(u*u + v*v) / sqrt(2*pi*pi)*sin(u+4*pi/3)*sin(v+4*pi/3)");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Apple)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u) *(4 + 3.8* cos(v))");
            Expression_Y = ("sin(u) *(4 + 3.8* cos(v))");
            Expression_Z = ("(cos(v) + sin(v) - 1)* (1 + sin(v)) *log(1 - pi *v / 10) + 7.5 *sin(v)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Boy)
        {
            objet->two_system = -1;
            Expression_X = ("2/3* (cos(u)* cos(2*v) + sqrt(2)* sin(u)* cos(v))* cos(u) / (sqrt(2) - sin(2*u)* sin(3*v))");
            Expression_Y = ("2/3* (cos(u)* sin(2*v) - sqrt(2)* sin(u)* sin(v))* cos(u) / (sqrt(2) - sin(2*u)* sin(3*v))");
            Expression_Z = ("sqrt(2)* cos(u)* cos(u) / (sqrt(2) - sin(2*u)* sin(3*v))");
            uMin = ("0");
            uMax = ("pi");
            vMin = ("0");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Maeders_Owl)
        {
            objet->two_system = -1;
            Expression_X = ("v *cos(u) - 0.5* v^2 * cos(2* u)");
            Expression_Y = ("-v *sin(u) - 0.5* v^2 * sin(2* u)");
            Expression_Z = ("4 *v^1.5 * cos(3 *u / 2) / 3");
            uMin = ("0");
            uMax = ("4*pi");
            vMin = ("0");
            vMax = ("1");
        }
        else if (myShape == Shapes::Cone_2)
        {
            objet->two_system = -1;
            Expression_X = ("u*cos(v)");
            Expression_Y = ("u*sin(v)");
            Expression_Z = ("u");
            uMin = ("-1");
            uMax = ("1");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Eight)
        {
            objet->two_system = -1;
            Expression_X = ("(2 + 0.2*sin(2*pi*u))*sin(pi*v)");
            Expression_Y = ("0.2*cos(2*pi*u) *3*cos(2*pi*v)");
            Expression_Z = ("(2 + 0.2*sin(2*pi*u))*cos(pi*v)");
            uMin = ("0");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("3*pi/4");
        }
        else if (myShape == Shapes::Drop)
        {
            objet->two_system = -1;
            Expression_X = ("u*cos(v)");
            Expression_Y = ("u*sin(v)");
            Expression_Z = ("exp(-u*u)*(sin(2*pi*u) - u*cos(3*v))");
            uMin = ("0");
            uMax = ("2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Plan)
        {
            objet->two_system = -1;
            Expression_X = ("u");
            Expression_Y = ("0");
            Expression_Z = ("v");
            uMin = ("-1");
            uMax = ("1");
            vMin = ("-1");
            vMax = ("1");
        }
        else if (myShape == Shapes::Ellipsoide)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)");
            Expression_Y = ("sin(u)*cos(v)");
            Expression_Z = ("sin(v)");
            uMin = ("0");
            uMax = ("6.2831");
            vMin = ("-1.57");
            vMax = ("1.5708");
        }
        else if (myShape == Shapes::EightSurface)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*sin(2*v)");
            Expression_Y = ("sin(u)*sin(2*v)");
            Expression_Z = ("sin(v)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Dini)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*sin(v)");
            Expression_Y = ("sin(u)*sin(v)");
            Expression_Z = ("(cos(v)+log(tan(v/2))) + 0.2*u");
            uMin = ("0");
            uMax = ("12.4");
            vMin = ("0.1");
            vMax = ("2");
        }
        else if (myShape == Shapes::Flower)
        {
            objet->two_system = -1;
            Expression_X = ("v *cos(u) -0.5*v^2*cos(2*u)");
            Expression_Y = ("-v*sin(u) -0.5*v^2*sin(2*u)");
            Expression_Z = ("4* v^1.5 *cos(3* u / 2) / 3");
            uMin = ("0");
            uMax = ("4*pi");
            vMin = ("0");
            vMax = ("1");
        }
        else if (myShape == Shapes::Cosinus)
        {
            objet->two_system = -1;
            Expression_X = ("u");
            Expression_Y = ("v");
            Expression_Z = ("sin(pi*((u)^2+(v)^2))/2");
            uMin = ("-1");
            uMax = ("1");
            vMin = ("-1");
            vMax = ("1");
        }
        else if (myShape == Shapes::Shell)
        {
            objet->two_system = -1;
            Expression_X = ("1.2^v*(sin(u)^2 *sin(v))");
            Expression_Y = ("1.2^v*(sin(u)^2 *cos(v))");
            Expression_Z = ("1.2^v*(sin(u)*cos(u))");
            uMin = ("0");
            uMax = ("pi");
            vMin = ("-pi/4");
            vMax = ("5*pi/2");
        }
        else if (myShape == Shapes::Sphere)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)");
            Expression_Y = ("cos(u)*sin(v)");
            Expression_Z = ("sin(u)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Steiner)
        {
            objet->two_system = -1;
            Expression_X = ("(sin(2 * u) * cos(v) * cos(v))");
            Expression_Y = ("(sin(u) * sin(2 * v))");
            Expression_Z = ("(cos(u) * sin(2 * v))");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Cross_cap)
        {
            objet->two_system = -1;
            Expression_X = ("(sin(u) * sin(2 * v) / 2)");
            Expression_Y = ("(sin(2 * u) * cos(v) * cos(v))");
            Expression_Z = ("(cos(2 * u) * cos(v) * cos(v))");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Boys)
        {
            objet->two_system = -1;
            Expression_X = ("(2/3)*(cos(u)*cos(2*v)+sqrt(2)*sin(u)*cos(v))*cos(u) /(sqrt(2) - sin(2*u)*sin(3*v))");
            Expression_Y = ("(2/3)*(cos(u)*sin(2*v)-sqrt(2)*sin(u)*sin(v))*cos(u) /(sqrt(2)-sin(2*u)*sin(3*v))");
            Expression_Z = ("sqrt(2)*cos(u)^2 / (sqrt(2) - sin(2*u)*sin(2*v))");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Torus)
        {
            objet->two_system = -1;
            Expression_X = ("(1+ 0.5*cos(u))*cos(v)");
            Expression_Y = ("(1+ 0.5*cos(u))*sin(v)");
            Expression_Z = ("0.5*sin(u)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Klein)
        {
            objet->two_system = -1;
            Expression_X = ("(3*(1+sin(v)) + 2*(1-cos(v)/2)*cos(u))*cos(v)");
            Expression_Y = ("(4+2*(1-cos(v)/2)*cos(u))*sin(v)");
            Expression_Z = ("-2*(1-cos(v)/2) * sin(u)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Moebius)
        {
            objet->two_system = -1;
            Expression_X = ("cos(v)+u*cos(v/2)*cos(v)");
            Expression_Y = ("sin(v)+u*cos(v/2)*sin(v)");
            Expression_Z = ("u*sin(v/2)");
            uMin = ("-0.4");
            uMax = ("0.4");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Riemann)
        {
            objet->two_system = -1;
            Expression_X = ("u*v");
            Expression_Y = ("v^2 - u^2");
            Expression_Z = ("30*u");
            uMin = ("-6");
            uMax = ("6");
            vMin = ("-25");
            vMax = ("25");
        }
        else if (myShape == Shapes::Klein_2)
        {
            objet->two_system = -1;
            Expression_X = ("(2 + cos(v/2)* sin(u) - sin(v/2)* sin(2 *u))* cos(v)");
            Expression_Y = ("(2 + cos(v/2)* sin(u) - sin(v/2)* sin(2 *u))* sin(v)");
            Expression_Z = ("sin(v/2)* sin(u) + cos(v/2) *sin(2* u)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Henneberg)
        {
            objet->two_system = -1;
            Expression_X = ("2*sinh(u)*cos(v) -(2/3)*sinh(3*u)*cos(3*v)");
            Expression_Y = ("2*sinh(u)*sin(v) +(2/3)*sinh(3*u)*sin(3*v)");
            Expression_Z = ("2*cosh(2*u)*cos(2*v)");
            uMin = ("-1");
            uMax = ("1");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Enneper)
        {
            objet->two_system = -1;
            Expression_X = ("u -u^3/3  + u*v^2");
            Expression_Y = ("v -v^3/3  + v*u^2");
            Expression_Z = ("u^2 - v^2");
            uMin = ("-2");
            uMax = ("2");
            vMin = ("-2");
            vMax = ("2");
        }
        else if (myShape == Shapes::Helix)
        {
            objet->two_system = -1;
            Expression_X = ("(1-0.1*cos(v))*cos(u)");
            Expression_Y = ("(1-0.1*cos(v))*sin(u)");
            Expression_Z = ("0.1*(sin(v) + u/1.7 -10)");
            uMin = ("0");
            uMax = ("4*pi");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Hexaedron)
        {
            objet->two_system = -1;
            Expression_X = ("cos(v)^3*cos(u)^3");
            Expression_Y = ("sin(v)^3*cos(u)^3");
            Expression_Z = ("sin(u)^3");
            uMin = ("-1.3");
            uMax = ("1.3");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_1)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)");
            Expression_Y = ("cos(u)*sin(v)");
            Expression_Z = ("sin(u)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_2)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)");
            Expression_Y = ("cos(u)*sin(v)");
            Expression_Z = ("sin(u)*sin(v)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_3)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)");
            Expression_Y = ("cos(u)*sin(v)");
            Expression_Z = ("sin(u)*sin(v)*cos(v)");
            uMin = ("-pi/2");
            uMax = ("pi/2");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_4)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)");
            Expression_Y = ("cos(u)*sin(v)*sin(v)");
            Expression_Z = ("sin(u)*sin(v)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_5)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*sin(v)");
            Expression_Y = ("cos(u)*sin(v)*sin(v)");
            Expression_Z = ("sin(u)*sin(v)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_6)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*sin(u)");
            Expression_Y = ("sin(u)");
            Expression_Z = ("sin(u)*sin(v)*sin(u)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_7)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*sin(u)");
            Expression_Y = ("sin(u)*sin(v)*cos(u)");
            Expression_Z = ("sin(u)*sin(v)*sin(u)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_8)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*sin(u)");
            Expression_Y = ("cos(u)*sin(v)*cos(u)");
            Expression_Z = ("sin(u)*sin(v)*sin(u)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Sphere_9)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*sin(u)");
            Expression_Y = ("cos(u)*sin(v)*cos(u)*sin(v)");
            Expression_Z = ("sin(u)*sin(v)*sin(u)*sin(v)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Sphere_10)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)*sin(u)*sin(v)*sin(v)*sin(v)");
            Expression_Y = ("cos(u)*sin(v)*cos(u)*sin(v)");
            Expression_Z = ("sin(u)*sin(v)*sin(u)*sin(v)*cos(u)");
            uMin = ("-pi/2");
            uMax = ("0");
            vMin = ("0");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Catalan)
        {
            objet->two_system = -1;
            Expression_X = ("u-sin(u)*cosh(v)");
            Expression_Y = ("1-cos(u)*cosh(v)");
            Expression_Z = ("4*sin(1/2*u)*sinh(v/2)");
            uMin = ("-pi");
            uMax = ("3*pi");
            vMin = ("-2");
            vMax = ("2");
        }
        else if (myShape == Shapes::Toupie)
        {
            objet->two_system = -1;
            Expression_X = ("(abs(u)-1)^2 * cos(v)");
            Expression_Y = ("(abs(u)-1)^2 * sin(v)");
            Expression_Z = ("u");
            uMin = ("-1");
            uMax = ("1");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Bonbon)
        {
            objet->two_system = -1;
            Expression_X = ("u");
            Expression_Y = ("cos (u)*cos (v)");
            Expression_Z = ("cos (u)*sin (v)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Curve)
        {
            objet->two_system = -1;
            Expression_X = ("cos(2*u)");
            Expression_Y = ("sin(3*u)");
            Expression_Z = ("cos(u)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("0");
        }
        else if (myShape == Shapes::Trumpet)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*sin(v)");
            Expression_Y = ("sin(u)*sin(v)");
            Expression_Z = ("(cos(v)+log(tan(1/2*v)))");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0.03");
            vMax = ("2");
        }
        else if (myShape == Shapes::Helice_Curve)
        {
            objet->two_system = -1;
            Expression_X = ("sin(u)");
            Expression_Y = ("cos(u)");
            Expression_Z = ("(u^2)/100");
            uMin = ("0");
            uMax = ("6*pi");
            vMin = ("0");
            vMax = ("0");
        }
        else if (myShape == Shapes::Cresent)
        {
            objet->two_system = -1;
            Expression_X = ("(2 + sin(2* pi* u) *sin(2 *pi* v)) *sin(3* pi *v)");
            Expression_Y = ("(2 + sin(2* pi *u) *sin(2 *pi *v)) *cos(3* pi *v)");
            Expression_Z = ("cos(2* pi *u) *sin(2* pi* v) + 4 *v - 2");
            uMin = ("0");
            uMax = ("1");
            vMin = ("0");
            vMax = ("1");
        }
        else if (myShape == Shapes::Shoe)
        {
            objet->two_system = -1;
            Expression_X = ("u");
            Expression_Y = ("v");
            Expression_Z = ("1/3*u^3  - 1/2*v^2");
            uMin = ("-2");
            uMax = ("2");
            vMin = ("-2");
            vMax = ("2");
        }
        else if (myShape == Shapes::Snake)
        {
            objet->two_system = -1;
            Expression_X = ("1.2*(1 -v/(2*pi))*cos(3*v)*(1 + cos(u)) + 3*cos(3*v)");
            Expression_Y = ("1.2*(1 -v/(2*pi))*sin(3*v)*(1 + cos(u)) + 3*sin(3*v)");
            Expression_Z = ("9*v/(2*pi) + 1.2*(1 - v/(2*pi))*sin(u)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Roman)
        {
            objet->two_system = -1;
            Expression_X = ("1/2*sin(2*u)*sin(v)^2");
            Expression_Y = ("1/2*sin(u)*cos(2*v)");
            Expression_Z = ("1/2*cos(u)*sin(2*v)");
            uMin = ("0");
            uMax = ("pi");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
        else if (myShape == Shapes::Hyperhelicoidal)
        {
            objet->two_system = -1;
            Expression_X = ("(sinh(v)*cos(3*u))/(1+cosh(u)*cosh(v))");
            Expression_Y = ("(sinh(v)*sin(3*u))/(1+cosh(u)*cosh(v))");
            Expression_Z = ("(cosh(v)*sinh(u))/(1+cosh(u)*cosh(v))");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Horn)
        {
            objet->two_system = -1;
            Expression_X = ("(2 + u*cos(v))*sin(2*pi*u)");
            Expression_Y = ("(2 + u*cos(v))*cos(2*pi*u) + 2*u");
            Expression_Z = ("u *sin(v)");
            uMin = ("0");
            uMax = ("1");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Helicoidal)
        {
            objet->two_system = -1;
            Expression_X = ("sinh(v)*sin(u)");
            Expression_Y = ("-sinh(v)*cos(u)");
            Expression_Z = ("3*u");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Catenoid)
        {
            objet->two_system = -1;
            Expression_X = ("2*cosh(v/2)*cos(u)");
            Expression_Y = ("2*cosh(v/2)*sin(u)");
            Expression_Z = ("v");
            uMin = ("-pi");
            uMax = ("pi");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Kuen)
        {
            objet->two_system = -1;
            Expression_X = ("(2*(cos(u) + u*sin(u))*sin(v))/(1+ u^2*sin(v)^2)");
            Expression_Y = ("(2*(sin(u) - u*cos(u))*sin(v))/(1+ u^2*sin(v)^2)");
            Expression_Z = ("log(tan(1/2 *v)) + (2*cos(v))/(1+ u^2*sin(v)^2)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0.01");
            vMax = ("pi-0.01");
        }
        else if (myShape == Shapes::Hellipticparaboloid)
        {
            objet->two_system = -1;
            Expression_X = ("v*2*cos(u)");
            Expression_Y = ("v*sin(u)");
            Expression_Z = ("v^2");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("0");
            vMax = ("2");
        }
        else if (myShape == Shapes::Enneper_2)
        {
            objet->two_system = -1;
            Expression_X = ("u*cos(v)-u^3/3*cos(3*v)");
            Expression_Y = ("-u*sin(v)-u^(3)/3*sin(3*v)");
            Expression_Z = ("u^2*cos(2*v)");
            uMin = ("0");
            uMax = ("1.2");
            vMin = ("-pi");
            vMax = ("pi");
        }
        else if (myShape == Shapes::Stereosphere)
        {
            objet->two_system = -1;
            Expression_X = ("2.*u/(u*u+v*v+1.)");
            Expression_Y = ("2.*v/(u*u+v*v+1.)");
            Expression_Z = ("(u*u+v*v-1.)/(u*u+v*v+1.)");
            uMin = ("-2");
            uMax = ("2");
            vMin = ("-2");
            vMax = ("2");
        }
		else if (myShape == Shapes::Cliffordtorus)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u+v)/(sqrt(2.)+cos(v-u))");
            Expression_Y = ("sin(u+v)/(sqrt(2.)+cos(v-u))");
            Expression_Z = ("sin(v-u)/(sqrt(2.)+cos(v-u))");
            uMin = ("0");
            uMax = ("pi");
            vMin = ("0");
            vMax = ("2*pi");
        }
        else if (myShape == Shapes::Fresnel_1)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)/(-2.*sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))*cos((acos(-(-0.941/6.+0.374*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4)-1.309/6.*((cos(u)^6+sin(u)^6)*cos(v)^6+sin(v)^6)-1.221*cos(u)^2*cos(v)^4*sin(u)^2*sin(v)^2)/sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))^3)+pi)/3.)+0.8)");
            Expression_Y = ("sin(u)*cos(v)/(-2.*sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))*cos((acos(-(-0.941/6.+0.374*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4)-1.309/6.*((cos(u)^6+sin(u)^6)*cos(v)^6+sin(v)^6)-1.221*cos(u)^2*cos(v)^4*sin(u)^2*sin(v)^2)/sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))^3)+pi)/3.)+0.8)");
            Expression_Z = ("sin(v)/(-2.*sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))*cos((acos(-(-0.941/6.+0.374*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4)-1.309/6.*((cos(u)^6+sin(u)^6)*cos(v)^6+sin(v)^6)-1.221*cos(u)^2*cos(v)^4*sin(u)^2*sin(v)^2)/sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))^3)+pi)/3.)+0.8)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("-pi/2");
            vMax = ("pi/2");

        }
        else if (myShape == Shapes::Fresnel_2)
        {
            objet->two_system = -1;
            Expression_X = ("cos(u)*cos(v)/(-2.*sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))*cos((acos(-(-0.941/6.+0.374*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4)-1.309/6.*((cos(u)^6+sin(u)^6)*cos(v)^6+sin(v)^6)-1.221*cos(u)^2*cos(v)^4*sin(u)^2*sin(v)^2)/sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))^3)-pi)/3.)+0.8)");
            Expression_Y = ("sin(u)*cos(v)/(-2.*sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))*cos((acos(-(-0.941/6.+0.374*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4)-1.309/6.*((cos(u)^6+sin(u)^6)*cos(v)^6+sin(v)^6)-1.221*cos(u)^2*cos(v)^4*sin(u)^2*sin(v)^2)/sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))^3)-pi)/3.)+0.8)");
            Expression_Z = ("sin(v)/(-2.*sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))*cos((acos(-(-0.941/6.+0.374*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4)-1.309/6.*((cos(u)^6+sin(u)^6)*cos(v)^6+sin(v)^6)-1.221*cos(u)^2*cos(v)^4*sin(u)^2*sin(v)^2)/sqrt(0.965/3.-0.935/3.*((cos(u)^4+sin(u)^4)*cos(v)^4+sin(v)^4))^3)-pi)/3.)+0.8)");
            uMin = ("0");
            uMax = ("2*pi");
            vMin = ("-pi/2");
            vMax = ("pi/2");
        }
		
		reDraw = true;
		valueChanged();
		/// <newCode>
		delete shapePointCache;
		shapePointCache = objet->CreateStructurePointsList();
		objet->PolyIsSelected = false;
		/// </newCode>
		Refresh();
		//Update();
}


/// <newCode>

void NuGenPSurface::CreateGraphicsDevice()
{
	try
	{
		if (graphicsDevice != nullptr)
			delete graphicsDevice;

		presentParams->Windowed = true;
		presentParams->AutoDepthStencilFormat = DepthFormat::D16;
		presentParams->SwapEffect = SwapEffect::Discard;
		presentParams->EnableAutoDepthStencil = true;
		//if (generalHwDSettings.MultiSample && MyAntialiasing)
		presentParams->MultiSample = MultiSampleType::None;//FourSamples;

		DeviceType devType = DeviceType::Software;
		if (directxHwDSettings.HardwareDevice)
			devType = DeviceType::Hardware;
		CreateFlags vertexProcessing = CreateFlags::SoftwareVertexProcessing;
		if (directxHwDSettings.HwTnL)
		{
			vertexProcessing = CreateFlags::HardwareVertexProcessing;
			//if (directxHwDSettings.PureDevice)
			//	vertexProcessing = vertexProcessing | CreateFlags::PureDevice;
		}

		graphicsDevice = gcnew Device(directxHwDSettings.AdapterInf->Adapter, devType, this,
									  vertexProcessing, presentParams);

		graphicsDevice->DeviceResizing += gcnew System::ComponentModel::CancelEventHandler(this, &NuGenPSurface::CancelResize);
	}
	catch (Exception^ e) { MessageBox::Show("Failed to create DirectX device\n\n" + e->Message); }
}

void NuGenPSurface::CancelResize(System::Object^ sender, System::ComponentModel::CancelEventArgs^ e)
{
	if (this->Width == 0 || this->Height == 0)
		e->Cancel = true;
}

void NuGenPSurface::CreateMatrices()
{
	// need better values to recreate view
	matrixProjDx = Matrix::PerspectiveFovLH(45.0f, (float)this->Width / (float)this->Height,
											10.0f, 5000.0f);

	matrixViewDx = Matrix::LookAtLH(Vector3(0, 0, 1075), Vector3(0,0,0), Vector3(0,1,0));
}

void NuGenPSurface::DrawDxFrame(PaintEventArgs ^e)
{
	try
	{
		if (graphicsDevice != nullptr && graphicsDevice->CheckCooperativeLevel())
		{
			graphicsDevice->RenderState->Lighting = true;
			graphicsDevice->Lights[0]->Direction = Vector3(0, 0, -1);
			graphicsDevice->Lights[0]->Type = LightType::Directional;
			graphicsDevice->Lights[0]->DiffuseColor = ColorValue::FromColor(Color::LightGray);
			graphicsDevice->Lights[0]->AmbientColor = ColorValue::FromColor(Color::Gray);
			graphicsDevice->Lights[0]->Enabled = true;
			graphicsDevice->Lights[0]->Update();

			graphicsDevice->Clear(ClearFlags::Target | ClearFlags::ZBuffer, BackgroundColor, 1.0, 0);
			graphicsDevice->BeginScene();

			if (bgTex != nullptr)
			{
				// draw background image
				graphicsDevice->RenderState->Lighting = false;
				graphicsDevice->RenderState->ZBufferWriteEnable = false;
				graphicsDevice->SetTexture(0, bgTex);
				graphicsDevice->Transform->Projection = Microsoft::DirectX::Matrix::OrthoLH((float)this->Width, (float)this->Height, 0.0f, 1.0f);
				graphicsDevice->Transform->View = Microsoft::DirectX::Matrix::Identity;
				graphicsDevice->Transform->World = Microsoft::DirectX::Matrix::Translation(-((float)this->Width)/2.0f, -((float)this->Height)/2.0f, 0);

				graphicsDevice->VertexFormat = CustomVertex::PositionTextured::Format;
				graphicsDevice->SetStreamSource(0, bgVBuffer, 0);
				graphicsDevice->DrawPrimitives(PrimitiveType::TriangleList, 0, 2);

				graphicsDevice->RenderState->ZBufferEnable = true;
				graphicsDevice->SetTexture(0, nullptr);
				graphicsDevice->RenderState->Lighting = true;
				graphicsDevice->RenderState->ZBufferWriteEnable = true;
			}

			// reset view
			matStack[2] = matrixProjDx;
			graphicsDevice->Transform->Projection = matrixProjDx;
			matStack[1] = matrixViewDx;
			graphicsDevice->Transform->View = matrixViewDx;
			matStack[0] = matrixWorldDx;
			graphicsDevice->Transform->World = matrixWorldDx;

			objet->tracer3(graphicsDevice, MyDrawBorder, matStack);

			graphicsDevice->EndScene();
			graphicsDevice->Present();
		}
	}
	catch (Exception^) { }
}

void NuGenPSurface::OnPaintBackground(PaintEventArgs ^e)
{
	// don't paint background
	if (MyRenderMode == RenderingMode::Software)
		UserControl::OnPaintBackground(e);
}

array<Point3F>^ NuGenPSurface::CreateCurrentPointsList()
{
	return objet->CreateStructurePointsList();
}

array<array<Point3F>^>^ NuGenPSurface::CreatePointsListByLevel()
{
	return objet->CreatePointsListByLevel();
}

void NuGenPSurface::DetectAdapter()
{
	try
	{
		// choose adapter
		directxHwDSettings.AdapterInf = Manager::Adapters->Default;

		int adapter = directxHwDSettings.AdapterInf->Adapter;
		
		// detect capabilities
		directxHwDSettings.HardwareDevice = Manager::CheckDeviceType(adapter, DeviceType::Hardware, Format::X8R8G8B8,
																	 Format::X8R8G8B8, true);

		generalHwDSettings.MultiSample = Manager::CheckDeviceMultiSampleType(adapter, DeviceType::Hardware, Format::X8R8G8B8,
																			 true, MultiSampleType::FourSamples);

		Caps caps = Manager::GetDeviceCaps(adapter, DeviceType::Hardware);
		directxHwDSettings.HwTnL = caps.DeviceCaps.SupportsHardwareTransformAndLight;
		directxHwDSettings.PureDevice = caps.DeviceCaps.SupportsPureDevice;

		/*String^ debugDeviceStr = String::Format("Debug Device Info:\nDevice:'{0}'\nHardware:{1}\nMultiSample:{2}\nTnL:{3}\nPure:{4}",
											    directxHwDSettings.AdapterInf->Information.Description,
												directxHwDSettings.HardwareDevice,
												generalHwDSettings.MultiSample,
												directxHwDSettings.HwTnL,
												directxHwDSettings.PureDevice);
		MessageBox::Show(debugDeviceStr);*/
	}
	catch (Exception^)
	{
		MessageBox::Show("Problem detecting adapter");
	}
}

void NuGenPSurface::OnMouseDoubleClick(MouseEventArgs^ e)
{
	if (selectPolygons)
	{
		// Try selection
		TrySelectFrom2DScreen(e->X, e->Y);
		Refresh();
	}
}

void NuGenPSurface::TrySelectFrom2DScreen(int x, int y)
{
	Vector3 pickRay, pickRayOrigin, pickRayDir;

	Microsoft::DirectX::Matrix projection = matStack[2];
	Microsoft::DirectX::Matrix view = matStack[1];
	Microsoft::DirectX::Matrix world = matStack[0];

	// Compute the vector of the pick ray in screen space
	pickRay.X = (((2.0f * x) / this->Width) - 1) / projection.M11;
	pickRay.Y = -(((2.0f * y) / this->Height) - 1) /  projection.M22;
	pickRay.Z = 1.0f;

	Microsoft::DirectX::Matrix matInverseView = Matrix::Invert(view);

	// Transform the screen space pick ray into 3D space
	pickRayDir.X  = pickRay.X * matInverseView.M11 + pickRay.Y * matInverseView.M21 + pickRay.Z * matInverseView.M31;
	pickRayDir.Y  = pickRay.X * matInverseView.M12 + pickRay.Y * matInverseView.M22 + pickRay.Z * matInverseView.M32;
	pickRayDir.Z  = pickRay.X * matInverseView.M13 + pickRay.Y * matInverseView.M23 + pickRay.Z * matInverseView.M33;
	pickRayDir.Normalize();

	pickRayOrigin.X = matInverseView.M41;
	pickRayOrigin.Y = matInverseView.M42;
	pickRayOrigin.Z = matInverseView.M43;

	pickRayOrigin.Add(Vector3::Multiply(pickRayDir, 1.0f));	//	near plane

	// transform trough world space
	Microsoft::DirectX::Matrix inverseWorld = Matrix::Invert(world); // scene-world == object-world

	Vector3 localOrigin = Vector3::TransformCoordinate(pickRayOrigin, inverseWorld);
	Vector3 localDir = Vector3::TransformNormal(pickRayDir, inverseWorld);

	objet->TrySelect(gcnew Point3F(localOrigin.X, localOrigin.Y, localOrigin.Z),
					 gcnew Point3F(localDir.X, localDir.Y, localDir.Z));
}

void NuGenPSurface::OnBackgroundImageChanged(EventArgs^ e)
{
	if (bgTex != nullptr)
		delete bgTex;
	if (BackgroundImage != nullptr)
	{
		System::IO::MemoryStream^ ms = gcnew System::IO::MemoryStream();
		BackgroundImage->Save(ms, System::Drawing::Imaging::ImageFormat::Bmp);
		ms->Seek(0, System::IO::SeekOrigin::Begin);
		bgTex = TextureLoader::FromStream(graphicsDevice, ms);
		delete ms;
		bgImgSize.Width = (float)BackgroundImage->Size.Width;
		bgImgSize.Height = (float)BackgroundImage->Size.Height;

		ReBuildBgBuffers();
	}
}

void NuGenPSurface::ReBuildBgBuffers()
{
	if (bgVBuffer != nullptr)
	{
		array<CustomVertex::PositionTextured>^ verts = (array<CustomVertex::PositionTextured>^)bgVBuffer->Lock(0, LockFlags::None);

		if (this->BackgroundImageLayout == ImageLayout::Stretch)
		{
			verts[0].Position = Vector3(0.0f, 0.0f, 0.5f);
			verts[2].Position = Vector3((float)this->Width, 0.0f, 0.5f);
			verts[1].Position = Vector3((float)this->Width, (float)this->Height, 0.5f);

			verts[3].Position = verts[1].Position;
			verts[5].Position = Vector3(0.0f, (float)this->Height, 0.5f);
			verts[4].Position = verts[0].Position;

			verts[0].Tu = 0; verts[0].Tv = 0;
			verts[2].Tu = 1.0f; verts[2].Tv = 0;
			verts[1].Tu = 1.0f; verts[1].Tv = -1.0f;

			verts[3].Tu = 1.0f; verts[3].Tv = -1.0f;
			verts[5].Tu = 0; verts[5].Tv = -1.0f;
			verts[4].Tu = 0; verts[4].Tv = 0;
		}
		else if (this->BackgroundImageLayout == ImageLayout::None)
		{
			verts[0].Position = Vector3(0.0f, (float)this->Height - bgImgSize.Height, 0.5f);
			verts[2].Position = Vector3(bgImgSize.Width, (float)this->Height - bgImgSize.Height, 0.5f);
			verts[1].Position = Vector3(bgImgSize.Width, (float)this->Height, 0.5f);

			verts[3].Position = verts[1].Position;
			verts[5].Position = Vector3(0.0f, (float)this->Height, 0.5f);
			verts[4].Position = verts[0].Position;

			verts[0].Tu = 0; verts[0].Tv = 0;
			verts[2].Tu = 1.0f; verts[2].Tv = 0;
			verts[1].Tu = 1.0f; verts[1].Tv = -1.0f;

			verts[3].Tu = 1.0f; verts[3].Tv = -1.0f;
			verts[5].Tu = 0; verts[5].Tv = -1.0f;
			verts[4].Tu = 0; verts[4].Tv = 0;
		}
		else if (this->BackgroundImageLayout == ImageLayout::Center)
		{
			float midX = (float)this->Width / 2.0f;
			float midY = (float)this->Height / 2.0f;

			float halfImgWidth = bgImgSize.Width / 2.0f;
			float halfImgHeight = bgImgSize.Height / 2.0f;

			verts[0].Position = Vector3(midX - halfImgWidth, midY - halfImgHeight, 0.5f);
			verts[2].Position = Vector3(midX + halfImgWidth, midY - halfImgHeight, 0.5f);
			verts[1].Position = Vector3(midX + halfImgWidth, midY + halfImgHeight, 0.5f);

			verts[3].Position = verts[1].Position;
			verts[5].Position = Vector3(midX - halfImgWidth, midY + halfImgHeight, 0.5f);
			verts[4].Position = verts[0].Position;

			verts[0].Tu = 0; verts[0].Tv = 0;
			verts[2].Tu = 1.0f; verts[2].Tv = 0;
			verts[1].Tu = 1.0f; verts[1].Tv = -1.0f;

			verts[3].Tu = 1.0f; verts[3].Tv = -1.0f;
			verts[5].Tu = 0; verts[5].Tv = -1.0f;
			verts[4].Tu = 0; verts[4].Tv = 0;
		}
		else if (this->BackgroundImageLayout == ImageLayout::Zoom)
		{
			float oAspect = bgImgSize.Width / bgImgSize.Height;

			float xScale = (float)this->Width / bgImgSize.Width;
			float yScale = (float)this->Height / bgImgSize.Height;

			float ySize = bgImgSize.Height * xScale;
			float xSize = bgImgSize.Width * yScale;

			if (ySize < (float)this->Height)
			{
				float midY = (float)this->Height / 2.0f;
				float halfHeight = ySize / 2.0f;

				// use x to scale
				verts[0].Position = Vector3(0.0f, midY - halfHeight, 0.5f);
				verts[2].Position = Vector3((float)this->Width, midY - halfHeight, 0.5f);
				verts[1].Position = Vector3((float)this->Width, midY + halfHeight, 0.5f);

				verts[3].Position = verts[1].Position;
				verts[5].Position = Vector3(0.0f, midY + halfHeight, 0.5f);
				verts[4].Position = verts[0].Position;

				
			}
			else
			{
				float midX = (float)this->Width / 2.0f;
				float halfWidth = xSize / 2.0f;

				// use y to scale
				verts[0].Position = Vector3(midX - halfWidth, 0.0f, 0.5f);
				verts[2].Position = Vector3(midX + halfWidth, 0.0f, 0.5f);
				verts[1].Position = Vector3(midX + halfWidth, (float)this->Height, 0.5f);

				verts[3].Position = verts[1].Position;
				verts[5].Position = Vector3(midX - halfWidth, (float)this->Height, 0.5f);
				verts[4].Position = verts[0].Position;
			}
			verts[0].Tu = 0; verts[0].Tv = 0;
			verts[2].Tu = 1.0f; verts[2].Tv = 0;
			verts[1].Tu = 1.0f; verts[1].Tv = -1.0f;

			verts[3].Tu = 1.0f; verts[3].Tv = -1.0f;
			verts[5].Tu = 0; verts[5].Tv = -1.0f;
			verts[4].Tu = 0; verts[4].Tv = 0;
		}
		else if (this->BackgroundImageLayout == ImageLayout::Tile)
		{
			float scaleX = (float)this->Width / bgImgSize.Width;
			float scaleY = (float)this->Height / bgImgSize.Height;

			verts[0].Position = Vector3(0.0f, 0.0f, 0.5f);
			verts[2].Position = Vector3((float)this->Width, 0.0f, 0.5f);
			verts[1].Position = Vector3((float)this->Width, (float)this->Height, 0.5f);

			verts[3].Position = verts[1].Position;
			verts[5].Position = Vector3(0.0f, (float)this->Height, 0.5f);
			verts[4].Position = verts[0].Position;

			verts[0].Tu = 0; verts[0].Tv = scaleY;
			verts[2].Tu = scaleX; verts[2].Tv = scaleY;
			verts[1].Tu = scaleX; verts[1].Tv = 0;

			verts[3].Tu = scaleX; verts[3].Tv = 0;
			verts[5].Tu = 0; verts[5].Tv = 0;
			verts[4].Tu = 0; verts[4].Tv = scaleY;
		}

		bgVBuffer->Unlock();
	}
}

void NuGenPSurface::OnBackgroundImageLayoutChanged(EventArgs^ e)
{
	ReBuildBgBuffers();

	/*if (MyRenderMode != RenderingMode::DirectX)
	{*/
		Refresh();
	//}
}

void NuGenPSurface::XamlExport()
{
	// ask for save location
	SaveFileDialog^ saveFileDialog1 = gcnew SaveFileDialog;
	saveFileDialog1->Filter = "XAML (*.xaml)|*.xaml";
	saveFileDialog1->AddExtension = true;
	saveFileDialog1->RestoreDirectory = true;

	if (saveFileDialog1->ShowDialog() == DialogResult::OK)
	{
		Scene3D^ scene = nullptr;
		if (contentType == ContentType::PSurface)
		{
			// convert to XAML bridge - simply rip from DX VB
			array<CustomVertex::PositionNormal>^ verts = (array<CustomVertex::PositionNormal>^)objet->structureFirstHalfVB->Lock(0, LockFlags::None);
			array<int>^ triIndices = gcnew array<int>(verts->Length);
			// index first half of shape
			verts = IndexRawTriangles(verts, triIndices);
			objet->structureFirstHalfVB->Unlock();
			
			// create scene
			scene = gcnew Scene3D();
			scene->Models = gcnew array<Genetibase::NuGenPSurface::AvalonBridge::Model3D^>(2);

			Genetibase::NuGenPSurface::AvalonBridge::Model3D^ model = scene->Models[0] = gcnew Genetibase::NuGenPSurface::AvalonBridge::Model3D();
			Geometry3D^ geometry = model->Geometry = gcnew Geometry3D();
			geometry->Vertices = gcnew array<Vector3D^>(verts->Length);
			geometry->Normals = gcnew array<Vector3D^>(verts->Length);

			// copy data
			for (int vert = 0; vert < verts->Length; vert++)
			{
				geometry->Vertices[vert] = gcnew Vector3D(verts[vert].X, verts[vert].Y, verts[vert].Z);
				geometry->Normals[vert] = gcnew Vector3D(verts[vert].Nx, verts[vert].Ny, verts[vert].Nz);
			}
			geometry->PrimIndices = triIndices;

			// now second half
			verts = (array<CustomVertex::PositionNormal>^)objet->structureSecondHalfVB->Lock(0, LockFlags::None);
			triIndices = gcnew array<int>(verts->Length);
			// index first half of shape
			verts = IndexRawTriangles(verts, triIndices);
			objet->structureSecondHalfVB->Unlock();

			model = scene->Models[1] = gcnew Genetibase::NuGenPSurface::AvalonBridge::Model3D();
			geometry = model->Geometry = gcnew Geometry3D();
			geometry->Vertices = gcnew array<Vector3D^>(verts->Length);
			geometry->Normals = gcnew array<Vector3D^>(verts->Length);

			// copy data
			for (int vert = 0; vert < verts->Length; vert++)
			{
				geometry->Vertices[vert] = gcnew Vector3D(verts[vert].X, verts[vert].Y, verts[vert].Z);
				geometry->Normals[vert] = gcnew Vector3D(verts[vert].Nx, verts[vert].Ny, verts[vert].Nz);
			}
			geometry->PrimIndices = triIndices;
		}

		if (scene != nullptr)
		{
			// export to XAML via WPF/avalon classes
			System::Windows::Media::Media3D::Model3DGroup^ avObj = scene->ToAvalonObj();
			System::IO::FileStream^ fs = System::IO::File::Open(saveFileDialog1->FileName, System::IO::FileMode::Create);
			System::Windows::Markup::XamlWriter::Save(avObj, fs);
			fs->Close();
		}
	}
}

array<CustomVertex::PositionNormal>^ NuGenPSurface::IndexRawTriangles(array<CustomVertex::PositionNormal>^ triangles,
																	  array<int>^ oIndices)
{
    // index each unique vertex
    List<CustomVertex::PositionNormal>^ vertices = gcnew List<CustomVertex::PositionNormal>();
    for (int tri = 0; tri < triangles->Length; tri++)
    {
        // look for existing vertex
        bool indexedVert = false;
        for (int vIdx = 0; vIdx < vertices->Count; vIdx++)
        {
			if (vertices[vIdx].Position == triangles[tri].Position &&
				vertices[vIdx].Normal == triangles[tri].Normal)
            {
                oIndices[tri] = vIdx;
                indexedVert = true;
                break;
            }
        }
        if (!indexedVert)
        {
            // index new vertex
			oIndices[tri] = vertices->Count;
            vertices->Add(triangles[tri]);
        }
    }

    array<CustomVertex::PositionNormal>^ oVertices = vertices->ToArray();
    vertices->Clear();

	return oVertices;
}

/// </newCode>