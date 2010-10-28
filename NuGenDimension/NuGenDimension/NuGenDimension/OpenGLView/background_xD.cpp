#include "stdafx.h"
#include "background_xD.h"
#include "math.h"
#include <io.h>
#include <fcntl.h>
#include <gl\glu.h>
#include <share.h>

//-----------------------------------------------------------//
//                         public                            //
//-----------------------------------------------------------//

//-------------------------------------------------------------
//constructor
//public
#define FAR_CLIP 2000.0

CBackground::CBackground()
{
    m_depth = FAR_CLIP;
    m_row_num = 1;
    m_col_num = 1;

	m_corners[0].r = 0.2f;
	m_corners[0].g = 0.2f;
	m_corners[0].b = 0.6f;
	m_corners[0].a = 1.0f; 

	m_corners[1].r = 0.2f;
	m_corners[1].g = 0.2f;
	m_corners[1].b = 0.6f;
	m_corners[1].a = 1.0f; 

    m_corners[2].r = 0.9f;
    m_corners[2].g = 0.9f;
    m_corners[2].b = 1.0f;
    m_corners[2].a = 1.0f; 

    m_corners[3].r = 0.9f;
    m_corners[3].g = 0.9f;
    m_corners[3].b = 1.0f;
    m_corners[3].a = 1.0f; 
    
    m_mirror_type = MIRROR11;
    m_textureID = 0;
    m_texture_file.Empty();
}
//-------------------------------------------------------------
//destructor
//public
CBackground::~CBackground()
{
    if(m_textureID)
    {
        glDeleteTextures(1, &m_textureID);
        m_textureID = 0;
    }  
}
//-------------------------------------------------------------
//set background params	
//public
void CBackground::SetParams(SBACKGROUND_PARAMS *params)
{
    if(!params)        return;
    if(m_textureID)
    {
        glDeleteTextures(1, &m_textureID);
        m_textureID = 0;
    }  
    m_row_num = params->row_num;
    m_col_num = params->col_num;
    m_mirror_type = params->mirror_type;
    m_texture_file = params->texture_file;
    for( int i=0; i<4; i++)
    {
        m_corners[i].r = params->corner[i].r;
        m_corners[i].g = params->corner[i].g;
        m_corners[i].b = params->corner[i].b;
        m_corners[i].a = params->corner[i].a;
    }
}
//-------------------------------------------------------------
//get background params	
//public
void CBackground::GetParams(SBACKGROUND_PARAMS *params)
{
    if(!params)        return;
    params->row_num = m_row_num;
    params->col_num = m_col_num;
    params->mirror_type = m_mirror_type;
    params->texture_file = m_texture_file;
    for( int i=0; i<4; i++)
    {
        params->corner[i].r = m_corners[i].r;
        params->corner[i].g = m_corners[i].g;
        params->corner[i].b = m_corners[i].b;
        params->corner[i].a = m_corners[i].a;
    }
}
//-------------------------------------------------------------
//set z coordinate of the background
//this function is called in CScene::SetBackgroung(CBackground *background)
//public
void CBackground::SetDepth(float depth)
{
    m_depth = depth;
}
//-------------------------------------------------------------
//showing background
//called in CScene::Display()
//public
void CBackground::Display()
{
    int i, j;

    if(m_textureID==0 && !m_texture_file.IsEmpty())
    {
        CreateTexture();
        if(m_textureID==0)
        {    
            ASSERT(0);
            return;
        }
    }

    glMatrixMode(GL_MODELVIEW);
	glPushMatrix();
	glLoadIdentity();
		
	glMatrixMode(GL_PROJECTION);
	glPushMatrix();
	glLoadIdentity();
	glOrtho(0, 1, 0, 1, 0, -(m_depth+1));
			
    glPushAttrib(GL_ENABLE_BIT| GL_TEXTURE_BIT | GL_POLYGON_BIT);
    glDisable(GL_CULL_FACE);
    glDisable(GL_BLEND);  
    glDisable(GL_LIGHTING);
    //if we have texture (*.bmp file) then show texture
	if(m_textureID)
    {
        bool vert_mirror_status=false;
	    bool hor_mirror_status=true;

        glBindTexture(GL_TEXTURE_2D, m_textureID);
	    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	    glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
			
	    glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);
        for(i=0; i<m_col_num; i++)
		{
			for(j=0; j<m_row_num; j++)
			{
				glBegin(GL_QUADS);
					if(!vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(0.f,0.f);
					if(vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(1.f,0.f);
					if(!vert_mirror_status && hor_mirror_status)
						glTexCoord2f(0.f,1.f);
					if(vert_mirror_status && hor_mirror_status)
						glTexCoord2f(1.f,1.f);
                    glVertex3f((float)i/(float)m_col_num, (float)(j+1)/(float)m_row_num, m_depth);
                    
					if(!vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(1.f,0.f);
					if(vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(0.f,0.f);
					if(!vert_mirror_status && hor_mirror_status)
						glTexCoord2f(1.f,1.f);
					if(vert_mirror_status && hor_mirror_status)
						glTexCoord2f(0.f,1.f);
    				glVertex3f((float)(i+1)/(float)m_col_num, (float)(j+1)/(float)m_row_num, m_depth);
                    

					if(!vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(1.f,1.f);
					if(vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(0.f,1.f);
					if(!vert_mirror_status && hor_mirror_status)
						glTexCoord2f(1.f,0.f);
					if(vert_mirror_status && hor_mirror_status)
						glTexCoord2f(0.f,0.f);
    				glVertex3f((float)(i+1)/(float)m_col_num, (float)j/(float)m_row_num, m_depth);
	
					if(!vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(0.f,1.f);
					if(vert_mirror_status && !hor_mirror_status)
						glTexCoord2f(1.f,1.f);
					if(!vert_mirror_status && hor_mirror_status)
						glTexCoord2f(0.f,0.f);
					if(vert_mirror_status && hor_mirror_status)
						glTexCoord2f(1.f,0.f);
					glVertex3f((float)i/(float)m_col_num, (float)j/(float)m_row_num,m_depth);


				glEnd();
			
			    if(m_mirror_type == MIRROR01 || m_mirror_type == MIRROR11)
				    hor_mirror_status=!hor_mirror_status;
		    }
            if(m_mirror_type == MIRROR10 || m_mirror_type == MIRROR11)
			    vert_mirror_status=!vert_mirror_status;

            hor_mirror_status=true;
		}
	}
	else
	{
        bool vert_mirror_status=false;
	    bool hor_mirror_status=false;

        glEnable(GL_BLEND);
		glDisable(GL_TEXTURE_2D);

		for(i=0; i<m_col_num; i++)
		{
			for(j=0; j<m_row_num; j++)
			{
				glBegin(GL_QUADS);
			    	if(!vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[0].r, m_corners[0].g, m_corners[0].b,1.0f);
			    	if(vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[1].r, m_corners[1].g, m_corners[1].b,1.0f);
			    	if(vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[2].r, m_corners[2].g, m_corners[2].b,1.0f);
                    if(!vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[3].r, m_corners[3].g, m_corners[3].b,1.0f);
                    glVertex3f((float)i/(float)m_col_num, (float)(j+1)/(float)m_row_num,m_depth);
                    //glVertex2f((float)i/(float)m_col_num, (float)(j+1)/(float)m_row_num);
                
			    	if(vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[0].r, m_corners[0].g, m_corners[0].b,1.0f);			
                    if(!vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[1].r, m_corners[1].g, m_corners[1].b,1.0f);			
			    	if(!vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[2].r, m_corners[2].g, m_corners[2].b,1.0f);			
			    	if(vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[3].r, m_corners[3].g, m_corners[3].b,1.0f);		
			    	glVertex3f((float)(i+1)/(float)m_col_num, (float)(j+1)/(float)m_row_num,m_depth);
                    //glVertex2f((float)(i+1)/(float)m_col_num, (float)(j+1)/(float)m_row_num);
                    
                
			    	if(vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[0].r, m_corners[0].g, m_corners[0].b,1.0f);			
			    	if(!vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[1].r, m_corners[1].g, m_corners[1].b,1.0f);			
                    if(!vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[2].r, m_corners[2].g, m_corners[2].b,1.0f);			
			    	if(vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[3].r, m_corners[3].g, m_corners[3].b,1.0f);			
                    glVertex3f((float)(i+1)/(float)m_col_num, (float)j/(float)m_row_num,m_depth);
                    //glVertex2f((float)(i+1)/(float)m_col_num, (float)j/(float)m_row_num);
                
			    	if(!vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[0].r, m_corners[0].g, m_corners[0].b,1.0f);			
			    	if(vert_mirror_status && hor_mirror_status)
			    		glColor4f(m_corners[1].r, m_corners[1].g, m_corners[1].b,1.0f);				
			    	if(vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[2].r, m_corners[2].g, m_corners[2].b,1.0f);			
                    if(!vert_mirror_status && !hor_mirror_status)
			    		glColor4f(m_corners[3].r, m_corners[3].g, m_corners[3].b,1.0f);	
                    glVertex3f((float)i/(float)m_col_num, (float)j/(float)m_row_num,m_depth);
                    //glVertex2f((float)i/(float)m_col_num, (float)j/(float)m_row_num);
    		    	
			    glEnd();
			
			    if(m_mirror_type == MIRROR01 || m_mirror_type == MIRROR11)
				    hor_mirror_status=!hor_mirror_status;
		    }
            if(m_mirror_type == MIRROR10 || m_mirror_type == MIRROR11)
			    vert_mirror_status=!vert_mirror_status;

            hor_mirror_status=false;
	    }
    }

	glPopAttrib();
	glPopMatrix();
		
	glMatrixMode(GL_MODELVIEW);
	glPopMatrix();
}

//-----------------------------------------------------------//
//                         private                           //
//-----------------------------------------------------------//

//creating background texture from *.bmp file
//private
bool CBackground::CreateTexture()
{
    if(m_texture_file.IsEmpty())
    {
        if(m_textureID)
        {
            glDeleteTextures(1, &m_textureID);
            m_textureID = 0;
        }
        return false;
    }

    if(m_textureID)
        if(m_textureID)
		    glDeleteTextures(1, &m_textureID);
  
    //glGenTextures(1, &m_textureID);
        m_textureID = 10067;

    ASSERT(m_textureID>0);
			
    int textuere_width;
    int texture_height;
    int tmp_width;
    int tmp_height;
    BYTE *pTexture = NULL;
    BYTE *pTmpData = NULL;
    char path[MAX_PATH];

    //sprintf(path, "%s", (LPCTSTR) m_texture_file);#OBSOLETE
	sprintf_s(path, "%s", (LPCTSTR) m_texture_file,m_texture_file.GetLength());
    
	pTmpData = LoadFromBmp(path, &tmp_width, &tmp_height);
				
	if(pTmpData)
	{
    	textuere_width=32;
		textuere_width = (int)pow((float)2.0f,(float)
			((log((float)tmp_width)/log(2.0f))+
				1.0-
				(log((float)m_col_num)/log(2.0f))));
		if(textuere_width<32)
		    textuere_width=32;
		if(textuere_width>1024)
			textuere_width=1024;

		texture_height=32;
		texture_height = (int)pow((float)2.0f,
			(float)((log((float)tmp_height)/log(2.0f))+1.0-(log((float)m_row_num)/log(2.0f))));
		if(texture_height<32)
		    texture_height=32;
		if(texture_height>1024)
			texture_height=1024;

		pTexture = new BYTE[texture_height*textuere_width*3];
		if(pTexture)
		{
		    gluScaleImage(GL_RGB, tmp_width, tmp_height, GL_UNSIGNED_BYTE, pTmpData, textuere_width, texture_height, GL_UNSIGNED_BYTE, pTexture);
			
            glBindTexture(GL_TEXTURE_2D, m_textureID);
            glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, textuere_width, texture_height, 0, GL_RGB, GL_UNSIGNED_BYTE, (GLubyte*) pTexture);
			delete[] pTmpData;
            delete[] pTexture;
		}
        else
        {
            return false;
        }
    }
    else
    {
        return false;
    }
    return true;
}
//-------------------------------------------------------------
//this function loads *.bmp file and returns width and height 
//pointer to the picture storage (rgbrgbrgb...), 
//size of this storage is bmpWidth*bmpHeight*3*sizeof(unsigned char)
//this memory shoud be free after using; 
//this function is used in CBackground::CreateTexture()
//private
unsigned char *CBackground::LoadFromBmp(char *filename, int* bmpWidth, int* bmpHeight)
{
    LPRGBQUAD   ptrToPalette;  
    char*       ptrToPixels;   
    long        bitmapRow, bitmapColumn;  
	LPBITMAPINFOHEADER PtrToBmpHdr;
	LPBITMAPFILEHEADER PtrToFileHdr;
	unsigned char *Texels;
	unsigned char *texture_pixels;

	int    size_texture;
	unsigned char *ptex;

	int    length;
	char  *texture_file;
	UINT  nBytesRead;
	int    fh;
	char errorstr[_MAX_PATH+25]="Can't open bmp file ";

   /* Open file for input: */
	//if( (fh = _open(filename, _O_RDONLY )) == -1 )  #OBSOLETE
	if((_sopen_s(&fh,filename,_O_RDONLY,_SH_DENYRW,0))!=0)
	{
		//::MessageBox(NULL, strcat(errorstr,filename),"Error",MB_OK);#OBSOLETE
		strcat_s(errorstr,sizeof(errorstr),filename);
		::MessageBox(NULL, errorstr,"Error",MB_OK);
        return NULL;
	}
	length = _filelength(fh);

	texture_file = (char*)malloc(length);

	if(texture_file == NULL)
		::MessageBox(NULL, "Can not load picture. \nPicture is too large.","Error",MB_OK);

	if( ( nBytesRead = _read( fh, texture_file, length ) ) <= 0 ) 
	{
		_close( fh );
		::MessageBox(NULL, "Read texture file","Error",MB_OK);
		exit(-3);
	}
	_close( fh );

	PtrToFileHdr = (LPBITMAPFILEHEADER)texture_file;
	PtrToBmpHdr = (LPBITMAPINFOHEADER)(texture_file+sizeof(BITMAPFILEHEADER));
  
	if (!PtrToBmpHdr) 
	{
		DWORD err = GetLastError();
		goto ErrorExit;
	}

    Texels = (unsigned char *)malloc(PtrToBmpHdr->biWidth * 3);

    ptrToPalette = (LPRGBQUAD) ((LPSTR) PtrToBmpHdr + PtrToBmpHdr->biSize);
    ptrToPixels  = (char*) PtrToBmpHdr + (PtrToBmpHdr->biSize + 
                           PtrToBmpHdr->biClrUsed * sizeof(RGBQUAD));

	if ( PtrToBmpHdr->biBitCount == 4) 
	{
            ptrToPixels += ((PtrToBmpHdr->biHeight - 1) * (long )((PtrToBmpHdr->biWidth/2+3) & ~3));
	} 
	else 
	{
		if ( PtrToBmpHdr->biBitCount == 8) 
		{
            ptrToPixels += ((PtrToBmpHdr->biHeight - 1) * (long )((PtrToBmpHdr->biWidth+3) & ~3));
		} 
		else 
		{
			if ( PtrToBmpHdr->biBitCount == 24) 
			{
				ptrToPixels += ((PtrToBmpHdr->biHeight - 1) * (long )((PtrToBmpHdr->biWidth*3+3) & ~3));
			}
		}
	}
	size_texture = PtrToBmpHdr->biWidth*PtrToBmpHdr->biHeight;
	ptex = (unsigned char*)malloc(3*size_texture);
	if (ptex == NULL) 
	{
		goto ErrorExit;
	}

	*bmpWidth = PtrToBmpHdr->biWidth;
	*bmpHeight = PtrToBmpHdr->biHeight;
  /*if ( ((*bmpWidth-1) & *bmpWidth)||((*bmpHeight-1) & *bmpHeight) )
	  ::MessageBox(NULL, "Texture dimension has to be power of 2","Error",MB_OK);
  */  
	texture_pixels = ptex;

    for (bitmapRow = 0;  bitmapRow < PtrToBmpHdr->biHeight; bitmapRow++) 
	{
		LPSTR   rowPtr;
		rowPtr = (LPSTR)ptrToPixels;

        for (bitmapColumn = 0; bitmapColumn < PtrToBmpHdr->biWidth; ) 
		{
            unsigned char p;
			if ( PtrToBmpHdr->biBitCount == 4) 
			{
				p = *rowPtr;
				rowPtr++;
				Texels[bitmapColumn*3  ] = ptrToPalette[p>>4].rgbRed;
				Texels[bitmapColumn*3+1] = ptrToPalette[p>>4].rgbGreen;
				Texels[bitmapColumn*3+2] = ptrToPalette[p>>4].rgbBlue;
				bitmapColumn++;
				Texels[bitmapColumn*3  ] = ptrToPalette[p&0xf].rgbRed;
				Texels[bitmapColumn*3+1] = ptrToPalette[p&0xf].rgbGreen;
				Texels[bitmapColumn*3+2] = ptrToPalette[p&0xf].rgbBlue;
				bitmapColumn++;
			} 
			else 
				if ( PtrToBmpHdr->biBitCount == 8) 
				{
					p = *rowPtr;
					rowPtr++;
					Texels[bitmapColumn*3  ] = ptrToPalette[p].rgbRed;
					Texels[bitmapColumn*3+1] = ptrToPalette[p].rgbGreen;
					Texels[bitmapColumn*3+2] = ptrToPalette[p].rgbBlue;
					bitmapColumn++;
				} 
				else 
					if ( PtrToBmpHdr->biBitCount == 24) 
					{
						Texels[bitmapColumn*3+2] = *rowPtr++;
						Texels[bitmapColumn*3+1] = *rowPtr++;
						Texels[bitmapColumn*3  ] = *rowPtr++;
						bitmapColumn++;
					}
		}
        
		if (PtrToBmpHdr->biBitCount==4) 
		{
			ptrToPixels -= ((PtrToBmpHdr->biWidth >> 1)+3) & ~3;
		} 
		else 
		{
			if(PtrToBmpHdr->biBitCount==8) 
			{
				ptrToPixels -= (PtrToBmpHdr->biWidth+3) & ~3;
			} 
			else 
			{
				if(PtrToBmpHdr->biBitCount==24) 
				{
					ptrToPixels -= ((PtrToBmpHdr->biWidth*3+3) & ~3);
				}
			}
		}
		memcpy( ptex, &Texels[0], 3 * PtrToBmpHdr->biWidth);
		ptex += 3 * PtrToBmpHdr->biWidth;
    } 

	free(texture_file); 
	free(Texels);

	return texture_pixels;
    
	ErrorExit:
	free(texture_file);
	return NULL;
}
