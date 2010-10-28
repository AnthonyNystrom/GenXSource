
#if !defined(BACKGROUND_H)
#define BACKGROUND_H

struct SCOLOR4F
{
	float r, g, b, a;
};

enum MIRROR_TYPE	{ MIRROR00, MIRROR10, MIRROR01, MIRROR11 	    };


struct SBACKGROUND_PARAMS
{
	int row_num;
	int col_num;
	SCOLOR4F corner [4];
	MIRROR_TYPE mirror_type;
	CString texture_file;
};

// background construction and visualizing (2D and 3D)
class CBackground
{
private:
    
    float m_depth;
    
	int m_row_num;
    
	int m_col_num;
    
    SCOLOR4F m_corners[4];
    
    MIRROR_TYPE m_mirror_type;
    
	CString m_texture_file;
    
    GLuint m_textureID;
   
    bool CreateTexture();
    
    unsigned char *LoadFromBmp(char *filename, int* bmpWidth, int* bmpHeight);
public:
    
    CBackground();
    
	~CBackground();
   
    void SetParams(SBACKGROUND_PARAMS *params);
    
	void GetParams(SBACKGROUND_PARAMS *params);
    
    void Display();
    
    void SetDepth(float depth);
};

#endif //BACKGROUND_H