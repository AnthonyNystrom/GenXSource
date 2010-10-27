//-----------------------------------------------------------------------------
// StereoPlayer.cpp : Implementation of the StereoPlayer class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"

// Anagryph implementation : COLOR_MASK or COLOR_POLYGON
// In the both style, you have to convert the source into gray scale...
// COLOR_MASK is simple way, but you can't change the colors 
//#define COLOR_MASK 1
// COLOR_MASK is implementd by colored polygon
#define COLOR_POLYGON 1

// When the source image updated, copying the buffer consumes more time, but more safe?
//#define BUFFER_COPY 1
#ifdef BUFFER_COPY
// With the BUFFER_COPY flag, using the Pixal Buffer Object extension would be faster?
//#define PBO 1
#endif //BUFFER_COPY

#ifdef PBO
// Using GLEW library for detecting the OpenGL extensions
#define GLEW_STATIC 1
#include "GL/glew.h"
#endif //PBO

// Using DevIL library for loading image files in various formats
#include <il/il.h>
#include <il/ilu.h>
#include <il/ilut.h>

#include <direct.h>

#include "DShowTextures.h"
#include "StereoPlayer.h"

// OpenGL constants
#ifndef GL_BGR
#define GL_BGR GL_BGR_EXT
#endif

#ifndef GL_BGRA
#define GL_BGRA GL_BGRA_EXT
#endif

// font display list indicies
#define STATS_FONT_DL_INDEX 1000
#define PLAYCTRL_FONT_DL_INDEX 1500

// GUI skin image file name
#define GUI_SKIN_FILENAME "gui.png"

// for easy to link...
#pragma comment(lib,"opengl32.lib")
#pragma comment(lib,"glu32.lib")
#pragma comment(lib,"devil.lib")
#pragma comment(lib,"ilu.lib")
#pragma comment(lib,"ilut.lib")
#if (_MSC_VER == 1200)
	#ifdef _DEBUG
		#pragma comment(lib,"strmbasd_vc6.lib")
	#else
		#pragma comment(lib,"strmbase_vc6.lib")
	#endif
#else
	#ifdef _DEBUG
		#pragma comment(lib,"strmbasd.lib")
	#else
		#pragma comment(lib,"strmbase.lib")
	#endif
#endif
#pragma comment(lib,"d3dx9.lib")
#pragma comment(lib,"d3d9.lib")
#pragma comment(lib,"winmm.lib")
#ifdef PBO
#pragma comment(lib,"glew32s.lib")
#endif //PBO

using namespace glsp;

// Constructor
StereoPlayer::StereoPlayer()
{
    // Initialize COM
    CoInitialize(NULL);

#ifdef PBO
    // Initialize GLEW library for using Pixel Buffer Extension
    GLenum err = glewInit();
    if (GLEW_OK != err)
    {
        char errText[1024];
        StringCchPrintf(errText, 1024, "Error: %s\n", glewGetErrorString(err));
        OutputDebugString(errText);
    }
#endif //PBO

    // Initialize DevIL library for loading images
    ilInit();
    iluInit();
    ilutInit();
    ilutRenderer(ILUT_OPENGL);

    // You have to set the home directory before loading the Play Control image
    StringCchCopy(m_homeDir, 256, "");

    // Rendering size
    m_wwidth = 640;
    m_wheight = 480;

    m_stereoEnabled = FALSE;

    for (unsigned int i=0; i<NUM_TEXTURES; i++)
    {
        // Initialize variables of the both sources
        m_graphManager[i] = NULL;

        StringCchCopy(m_textureFileName[i], _MAX_PATH, "none");
        m_textureID[i] = 0;
        m_sourceWidth[i] = 0;
        m_sourceHeight[i] = 0;
        m_textureWidth[i] = 16;
        m_textureHeight[i] = 16;
        m_pixelBuffer[i] = NULL;
        m_textureBuffer[i] = NULL;
        m_textureModfied[i] = FALSE;
        m_duration[i] = 0;
        m_withAudio[i] = TRUE;
        m_textureBufferObject[i] = 0;

        // Create Event objects
        char eventName[256];
        StringCchPrintf(eventName, 256, "ReadyToUseTexture_%i", i);
        m_readyToUseTextureEvent[i] = CreateEvent(NULL, TRUE, FALSE, eventName);
        StringCchPrintf(eventName, 256, "ReadyToUpdateTexture_%i", i);
        m_readyToUpdateTextureEvent[i] = CreateEvent(NULL, FALSE, FALSE, eventName);
    }

    // Player status
    m_stereoFormat = STEREO_FORMAT_SEPARATED;
    m_stereoType = STEREO_TYPE_LEFT;
    m_swap = FALSE;
	m_keepAspectRatio = TRUE;
    m_offset = 0.0f;
    m_panX = 0.0f;
    m_panY = 0.0f;
    m_zoom = 1.0f;
    m_forceSync = TRUE;
    m_speed = 1.0;
    m_loop = TRUE;
    m_volume = 0;
    m_playOnLoad = TRUE;

    // for FPS calculation
    m_fpsInterval = 0.5f;
    m_fpsLastTime = 0;
    m_frames = 0;
    m_fps = 60.0f;

    // for Statics rendering
    m_statisticsColor[0] = 1.0f;
    m_statisticsColor[1] = 1.0f;
    m_statisticsColor[2] = 1.0f;
    StringCchCopy(m_fontName, 256, "Courier");
    m_fontSize = 16;
    m_showStatistics = FALSE;

    // for Play Control
    m_playUIState = 0.0f;
    m_stopUIState = 0.0f;
    m_sliderUIState = 0.0f;
    m_sliderPos = -1000;
    m_playControl = PLAYCONTROL_AUTO;
    m_ctrlFadeLastTime = 0;
    m_ctrlFadeAlpha = 0.0f;
    m_ctrlFadeShowing = FALSE;
    m_uiTextureID = 0;

	m_transition = FALSE;
    m_transitionProgress = 0.0f;

    // Default base (clear) color
    m_baseColor[0] = 0.0f;
    m_baseColor[1] = 0.0f;
    m_baseColor[2] = 0.0f;

    // Default anagryph color
    setAnagryphColor(TEXTURE_LEFT,  1.0f, 0.0f, 0.0f);
    setAnagryphColor(TEXTURE_RIGHT, 0.0f, 0.0f, 1.0f);

    m_needsRefreshStencilBuffer = FALSE;

    m_initialized = FALSE;
}

// Destructor
StereoPlayer::~StereoPlayer()
{
    clearEvent();

    // Delete non-OpenGL resources
    for (unsigned int i=0; i<NUM_TEXTURES; i++)
    {
        // Delete Event objects
        if (m_readyToUseTextureEvent[i]) {
            CloseHandle(m_readyToUseTextureEvent[i]);
            m_readyToUseTextureEvent[i] = NULL;
        }
        if (m_readyToUpdateTextureEvent[i]) {
            CloseHandle(m_readyToUpdateTextureEvent[i]);
            m_readyToUpdateTextureEvent[i] = NULL;
        }

        if (m_graphManager[i]) {
            m_graphManager[i]->Stop();
            delete m_graphManager[i];
            m_graphManager[i] = NULL;
        }

        // Delete texture copy buffers
        if (m_textureBuffer[i])
            delete [] m_textureBuffer[i];
    }

    // Uninitialize COM
    CoUninitialize();
}

void StereoPlayer::initializeGL()
{
    if (m_initialized) return;

    m_initialized = TRUE;

    // Initialize OpenGL context and resources

    glClearColor(m_baseColor[0], m_baseColor[1], m_baseColor[2], 0.0f);
    glClearDepth(1.0f);
    glDepthFunc(GL_LEQUAL);
    glDisable(GL_DEPTH_TEST);  // We don't need Depth Buffer
    glEnable(GL_TEXTURE_2D);
    glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
    glEnable(GL_CULL_FACE);
    glDisable(GL_LIGHTING);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

    // Load the Play Control texture image from the home directory
    char curDir[_MAX_PATH];
    _getcwd(curDir, 256);
    _chdir(m_homeDir);
    ILuint imageID;
    ilGenImages(1, &imageID);
    ilBindImage(imageID);
    if (ilLoadImage(GUI_SKIN_FILENAME)) {
        if (glIsTexture(m_uiTextureID))
            glDeleteTextures(1, &m_uiTextureID);
        glGenTextures(1, &m_uiTextureID);
        glBindTexture(GL_TEXTURE_2D, m_uiTextureID);
        glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);        // for smooth fading
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);  // GUI image is exactly match the pixels
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);  // GUI image is exactly match the pixels
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, ilGetInteger(IL_IMAGE_WIDTH), ilGetInteger(IL_IMAGE_HEIGHT), 0, GL_RGBA, GL_UNSIGNED_BYTE, (void*)ilGetData());
        ilDeleteImages(1, &imageID);    // Now we don't need the DevIL image object
    }
    _chdir(curDir);

    // If we're "reconstructing" the OpenGL context, we have to set all the settings again
    loadLeftFile(m_textureFileName[TEXTURE_LEFT]);
    loadRightFile(m_textureFileName[TEXTURE_RIGHT]);
    setFormat(m_stereoFormat);
    setType(m_stereoType);
    setBaseColor(m_baseColor[0], m_baseColor[1], m_baseColor[2]);
    setRate(m_speed);
    setForceSync(m_forceSync);
    setLoop(m_loop);
    setVolume(m_volume);

    buildFont(m_fontName, m_fontSize, STATS_FONT_DL_INDEX);
    buildFont("System", 14, PLAYCTRL_FONT_DL_INDEX);
}

void StereoPlayer::terminateGL()
{
    clearEvent();

    // Delete OpenGL resources

    for (int i=0; i<NUM_TEXTURES; i++)
    {
        if (m_graphManager[i]) {
            delete m_graphManager[i];
            m_graphManager[i] = NULL;
        }

        if (m_textureBuffer[i]) {
            delete [] m_textureBuffer[i];
            m_textureBuffer[i] = NULL;
        }
    }

    if (glIsTexture(m_textureID[0]))
        glDeleteTextures(NUM_TEXTURES, m_textureID);

    if (glIsList(STATS_FONT_DL_INDEX))
        glDeleteLists(STATS_FONT_DL_INDEX, 255);
    if (glIsList(PLAYCTRL_FONT_DL_INDEX))
        glDeleteLists(PLAYCTRL_FONT_DL_INDEX, 255);

    if (glIsTexture(m_uiTextureID))
        glDeleteTextures(1, &m_uiTextureID);

    m_initialized = FALSE;
}

void StereoPlayer::setStereoEnabled(bool enabled)
{
    // If enabled is TRUE, we can use Quad Buffer.
    m_stereoEnabled = enabled;
}

BOOL StereoPlayer::getStereoEnabled()
{
    return m_stereoEnabled;
}

// Calculate 2^n texture size
GLuint calcPowerOfTwo(GLuint source)
{
    if (source <= 64)
        return 64;
    else if (source <= 128)
        return 128;
    else if (source <= 256)
        return 256;
    else if (source <= 512)
        return 512;
    else if (source <= 1024)
        return 1024;
    else if (source <= 2048)
        return 2048;
    return 4096;
}

// Load an image file in the specified side
BOOL StereoPlayer::loadImageFile(unsigned int which, const char* filename)
{
    char localFilename[_MAX_PATH];
    char* lowCaseFilename;
    StringCchCopy(localFilename, _MAX_PATH, filename);
    lowCaseFilename = _strlwr(localFilename);

    // For some stereo format (.bms, .jps, etc.) support,
    // manually determine the filr format from its file extension.
    ILenum imageType = IL_TYPE_UNKNOWN;
    // In DevIL library (ver.1.6.7), some formats need to be flipped
    bool needsFlipped = TRUE;

    if (strstr(lowCaseFilename, ".bmp")) {
        imageType = IL_BMP;
        needsFlipped = FALSE;
    }
    else if (strstr(lowCaseFilename, ".bms")) {
        imageType = IL_BMP;
        m_stereoFormat = STEREO_FORMAT_HORIZONTAL;
        needsFlipped = FALSE;
    }
    else if (strstr(lowCaseFilename, ".gif")) {
        imageType = IL_GIF;
    }
    else if (strstr(lowCaseFilename, ".gis")) {
        imageType = IL_GIF;
        m_stereoFormat = STEREO_FORMAT_HORIZONTAL;
    }
    else if (strstr(lowCaseFilename, ".jpg") || strstr(lowCaseFilename, ".jpeg")) {
        imageType = IL_JPG;
    }
    else if (strstr(lowCaseFilename, ".jps")) {
        imageType = IL_JPG;
        m_stereoFormat = STEREO_FORMAT_HORIZONTAL;
    }
    else if (strstr(lowCaseFilename, ".pbm") || strstr(lowCaseFilename, ".pgm") || strstr(lowCaseFilename, ".pnm")) {
        imageType = IL_PNM;
    }
    else if (strstr(lowCaseFilename, ".png")) {
        imageType = IL_PNG;
    }
    else if (strstr(lowCaseFilename, ".pns")) {
        imageType = IL_PNG;
        m_stereoFormat = STEREO_FORMAT_HORIZONTAL;
    }
    else if (strstr(lowCaseFilename, ".psd")) {
        imageType = IL_PSD;
    }
    else if (strstr(lowCaseFilename, ".psp")) {
        imageType = IL_PSP;
    }
    else if (strstr(lowCaseFilename, ".bw") || strstr(lowCaseFilename, ".rgb") ||
             strstr(lowCaseFilename, ".rgba") || strstr(lowCaseFilename, ".sgi")) {
        imageType = IL_SGI;
    }
    else if (strstr(lowCaseFilename, ".tga")) {
        imageType = IL_TGA;
    }
    else if (strstr(lowCaseFilename, ".tif") || strstr(lowCaseFilename, ".tiff")) {
        imageType = IL_TIF;
    }
    else if (strstr(lowCaseFilename, ".raw")) {
        imageType = IL_RAW;
    }

    if (imageType == IL_TYPE_UNKNOWN)
        return FALSE;

    // Load the image file into the texture buffer
    ILuint imageID;
    ilGenImages(1, &imageID);
    ilBindImage(imageID);
    ILboolean result =  ilLoad(imageType, (char*)filename);
    if (!result) {
        ilDeleteImages(1, &imageID);
        return FALSE;
    }
    if (needsFlipped)
        iluFlipImage();
    // We use the BGR internal texture format
    ilConvertImage(IL_BGR, IL_UNSIGNED_BYTE);

    m_sourceWidth[which] = ilGetInteger(IL_IMAGE_WIDTH);
    m_sourceHeight[which] = ilGetInteger(IL_IMAGE_HEIGHT);
    m_textureWidth[which] = calcPowerOfTwo(m_sourceWidth[which]);
    m_textureHeight[which] = calcPowerOfTwo(m_sourceHeight[which]);
    m_duration[which] = 0;

    // Allocate enough texture memory and generate a texture object
    initTexture(which);
    // Copy the image data into a part of the texture memory
    glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_sourceWidth[which], m_sourceHeight[which],
                    GL_BGR, GL_UNSIGNED_BYTE, (void*)ilGetData());
    // Now we don't need the DevIL image object
    ilDeleteImages(1, &imageID);

    m_transitionProgress = 1.0f;

    return TRUE;
}

// Load a movie or image file in the specified side
bool StereoPlayer::loadFile(unsigned int which, const char* filename, BOOL withAudio)
{
    if (which >= NUM_TEXTURES) return FALSE;

    // Copy the file name and audio enable flag.
    // If you give NULL or "none" as the file name, clear the specified side and rename it as "none".
    if (!filename || strcmp(filename, "none")==0) {
        StringCchCopy(m_textureFileName[which], _MAX_PATH, "none");
    } else {
        StringCchCopy(m_textureFileName[which], _MAX_PATH, filename);
	}
    m_withAudio[which] = withAudio;

    // Load the file after OpegGL context will be created
    if (!m_initialized) return FALSE;

    // Change the working directory to the pre-defined directory
    char curDir[256];
    _getcwd(curDir, 256);
    if (m_homeDir) _chdir(m_homeDir);

    // Clear the old source
    if (m_graphManager[which]) {
        clearEvent();
        m_graphManager[which]->Stop();
        delete m_graphManager[which];
        m_graphManager[which] = NULL;
    }
    if (glIsTexture(m_textureID[which]))
        glDeleteTextures(1, &m_textureID[which]);

    // If you want to clear the old source, that's all.
    if (strcmp(filename, "none")==0) {
        if (m_homeDir) _chdir(curDir);
        return TRUE;
    }

    // Check if the file is a kind of image file...
    if (loadImageFile(which, filename)) {
        if (m_homeDir) _chdir(curDir);
        return TRUE;
    }

    // Now set up for a new movie source
    m_graphManager[which] = new CTextureRendererGraphManager;
    CComBSTR cbstrFilename(filename);
    BSTR bstrFilename = cbstrFilename;
    HRESULT result = m_graphManager[which]->LoadFile(bstrFilename, withAudio);
    if (result == E_FAIL) {
        // Failed to load... You have to show error messages by your self
        delete m_graphManager[which];
        m_graphManager[which] = NULL;
        StringCchCopy(m_textureFileName[which], _MAX_PATH, "none");
        // Restore the working directory
        if (m_homeDir) _chdir(curDir);
        return FALSE;
    }
    // Register the texture buffer copy callback
    switch (which) {
        case TEXTURE_LEFT: m_graphManager[which]->RegisterCallback(rendererCallback, this, TEXTURE_LEFT); break;
        case TEXTURE_RIGHT: m_graphManager[which]->RegisterCallback(rendererCallback, this, TEXTURE_RIGHT); break;
    }
    // Set up the source information
    LONG width, height;
    m_graphManager[which]->GetWidth(&width);
    m_graphManager[which]->GetHeight(&height);
    m_sourceWidth[which] = width;   // Original width of the source
    m_sourceHeight[which] = height; // Original height of the source
    m_textureWidth[which] = calcPowerOfTwo(m_sourceWidth[which]);   // Width of the minimum texture
    m_textureHeight[which] = calcPowerOfTwo(m_sourceHeight[which]); // Height of the minimum texture
    m_graphManager[which]->GetDuration(&m_duration[which]); // Get the source duration in sec.
    initTexture(which); // Allocate enough texture memory and generate a texture object

    // Set the given parameters to th graph manager
    m_graphManager[which]->SetRate(m_speed);
    m_graphManager[which]->SetLoop(m_loop);
    m_graphManager[which]->SetVolume(m_volume);

    // Restore the working directory
    if (m_homeDir) _chdir(curDir);

    // Play after loading
    if (m_playOnLoad) {
        // If you load both the left and right movies at the same time,
        // ForceSync and this reposition will synchronize the movies
        setPosition(0.0);

        play();
    }

    m_transitionProgress = 1.0f;

    return TRUE;
}

GLuint StereoPlayer::getWidth(unsigned int which)
{
    if (which >= NUM_TEXTURES) return 0;
    return m_sourceWidth[which];
}

GLuint StereoPlayer::getHeight(unsigned int which)
{
    if (which >= NUM_TEXTURES) return 0;
    return m_sourceHeight[which];
}

double StereoPlayer::getDuration(unsigned int which)
{
    if (which >= NUM_TEXTURES) return 0;
    if (m_graphManager[which]) {
        double duration;
        m_graphManager[which]->GetDuration(&duration);
        return duration;
    }
    return 0.0;
}

GLuint StereoPlayer::getPlayerWidth()
{
    GLuint width = getWidth();
    if (m_stereoFormat == STEREO_FORMAT_HORIZONTAL)
        width = width / 2;
    if (m_stereoType == STEREO_TYPE_HORIZONTAL)
        width = width * 2;
    return width;
}

GLuint StereoPlayer::getPlayerHeight()
{
    GLuint height = getHeight();
    if (m_stereoFormat == STEREO_FORMAT_VERTICAL)
        height = height / 2;
    if (m_stereoType == STEREO_TYPE_VERTICAL)
        height = height * 2;
    return height;
}

void StereoPlayer::setFormat(unsigned int format)
{
    m_stereoFormat = format;
}

void StereoPlayer::setType(unsigned int type)
{
    m_stereoType = type;

    // Some stereo type requires to set up the stencil buffer
    switch (type)
    {
        case STEREO_TYPE_HORIZONTAL_INTERLEAVED:
        case STEREO_TYPE_VERTICAL_INTERLEAVED:
        case STEREO_TYPE_SHARP3D:
            m_needsRefreshStencilBuffer = TRUE;
            break;
    }
}

void StereoPlayer::rendererCallback(void* obj, BYTE* pPixelBuffer, DWORD which)
{
    // This callback function is called at every movie frame updates.
    // This function is executed in the graph manager thread.
    if (!obj) return;
    StereoPlayer* player = (StereoPlayer*)obj;

    // Give the updated texture buffer's pointer to the main OpenGL rendering thread.
    player->m_pixelBuffer[which] = pPixelBuffer;
    player->m_textureModfied[which] = TRUE;

    // Wait until the main OpenGL rendering thread uses the updated image buffer...
    SetEvent(player->m_readyToUseTextureEvent[which]);
    WaitForSingleObject(player->m_readyToUpdateTextureEvent[which], 1000/30);//INFINITE);   
    ResetEvent(player->m_readyToUseTextureEvent[which]);
}

void StereoPlayer::setupStencilBuffer()
{
    // Setup Stencil Buffer for the interlace masking

    glViewport(0, 0, m_wwidth, m_wheight);

    glStencilFunc(GL_ALWAYS, 1, 1);
    glStencilOp(GL_REPLACE, GL_REPLACE, GL_REPLACE);
    glClearStencil(0);
    glClear(GL_STENCIL_BUFFER_BIT);
    glDisable(GL_DEPTH_TEST);
    glDrawBuffer(GL_NONE);
    glEnable(GL_STENCIL_TEST);

    glPushAttrib(GL_ENABLE_BIT);

    const GLubyte HORIZONTAL_STIPPLE[] =
    {
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
        0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,
    };
    const GLubyte VERTICAL_STIPPLE[] =
    {
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
        0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,0xAA,
    };
    switch (m_stereoType) {
        case STEREO_TYPE_HORIZONTAL_INTERLEAVED:
            glPolygonStipple(HORIZONTAL_STIPPLE);
            break;
        case STEREO_TYPE_VERTICAL_INTERLEAVED:
        case STEREO_TYPE_SHARP3D:
            glPolygonStipple(VERTICAL_STIPPLE);
            break;
    }
    glEnable(GL_POLYGON_STIPPLE);
    glMatrixMode(GL_PROJECTION);
    glPushMatrix();
    glLoadIdentity();
    glOrtho(0.0,1.0,1.0,0.0,0.0,1.0);
    glMatrixMode(GL_MODELVIEW);
    glPushMatrix();
    glLoadIdentity();
    glBegin(GL_QUADS);
    glVertex2i(0, 0);
    glVertex2i(0, 1);
    glVertex2i(1, 1);
    glVertex2i(1, 0);
    glEnd();
    glPopMatrix();
    glDisable(GL_POLYGON_STIPPLE);
    glMatrixMode(GL_PROJECTION);
    glPopMatrix();
    glMatrixMode(GL_MODELVIEW);

    glStencilOp(GL_KEEP,GL_KEEP,GL_KEEP);
    glDisable(GL_STENCIL_TEST);
    glDrawBuffer(GL_BACK);

    glPopAttrib();

    glFlush();

    m_needsRefreshStencilBuffer = FALSE;
}

void StereoPlayer::initTexture(unsigned int which)
{
    // Allocate texture memory
    GLubyte* buffer = NULL;
    long dataSize = 4 * sizeof(GLubyte) * m_textureWidth[which] * m_textureHeight[which];
    buffer = (GLubyte*)malloc(dataSize);

    // Clear with the base color
    GLubyte r = GLubyte(255 * m_baseColor[0]);
    GLubyte g = GLubyte(255 * m_baseColor[1]);
    GLubyte b = GLubyte(255 * m_baseColor[2]);
    GLubyte* bufferPtr = buffer;
    for (unsigned int i=0; i<m_textureWidth[which] * m_textureHeight[which]; i++) {
        *bufferPtr = b; bufferPtr++;
        *bufferPtr = g; bufferPtr++;
        *bufferPtr = r; bufferPtr++;
    }

    // Generate Texture Object Index
    if (glIsTexture(m_textureID[which]))
        glDeleteTextures(1, &m_textureID[which]);
    glGenTextures(1, &m_textureID[which]);
    // Set up te thexture parameters
    glBindTexture(GL_TEXTURE_2D, m_textureID[which]);
    glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexImage2D(GL_TEXTURE_2D, 0, 3, m_textureWidth[which], m_textureHeight[which], 0, GL_BGR, GL_UNSIGNED_BYTE, buffer);

    // Now we don't need the image buffer
    free(buffer);

    if (m_graphManager[which]) {
#ifdef BUFFER_COPY
#ifdef PBO
        if (GLEW_EXT_pixel_buffer_object)
        {
            // Generate a Pixel Buffer Object
            glGenBuffers(1, &m_textureBufferObject[which]);
            glBindBuffer(GL_PIXEL_UNPACK_BUFFER_EXT, m_textureBufferObject[which]);  
            glBufferData(GL_PIXEL_UNPACK_BUFFER_EXT, m_sourceWidth[which] * m_sourceHeight[which] * 3, NULL, GL_STREAM_DRAW);  
            glBindBuffer(GL_PIXEL_UNPACK_BUFFER_EXT, 0);
        }
        else 
#endif //PBO
        {
            // Allocate memory for copying the texture image data
            if (m_textureBuffer[which]) delete[] m_textureBuffer[which];
            m_textureBuffer[which] = new BYTE[m_sourceWidth[which] * m_sourceHeight[which] * 3];
        }
#endif //BUFFER_COPY
    }
}

void StereoPlayer::setStatisticsFont(HFONT hFont)
{
    // Create a Font Display List from font hanlde for rendering the statistics
    if (hFont)
    {
        LOGFONT lf;
        GetObject(hFont, sizeof(LOGFONT), &lf);
        StringCchCopy(m_fontName, 256, lf.lfFaceName);
        m_fontSize = -lf.lfHeight;

        if (glIsList(STATS_FONT_DL_INDEX))
            glDeleteLists(STATS_FONT_DL_INDEX,255);

        HDC hDC = GetDC(NULL);
        HFONT hOldFont;
        hOldFont = (HFONT)SelectObject (hDC, hFont);
        wglUseFontBitmaps(hDC, 0, 255, STATS_FONT_DL_INDEX);
        SelectObject(hDC, hOldFont);
        ReleaseDC(NULL, hDC);
    }
}

void StereoPlayer::setStatisticsFont(const char* fontName, int fontSize)
{
    // Create a Font Display List from font name and size for rendering the statistics
    StringCchCopy(m_fontName, 256, fontName);
    m_fontSize = fontSize;

    if (glIsList(STATS_FONT_DL_INDEX))
        glDeleteLists(STATS_FONT_DL_INDEX,255);

    buildFont(fontName, fontSize, STATS_FONT_DL_INDEX);
}

const char* StereoPlayer::getStatisticsFontName()
{
    return m_fontName;
}

int StereoPlayer::getStatisticsFontSize()
{
    return m_fontSize;
}

void StereoPlayer::buildFont(const char* fontName, int fontSize, GLuint baseDLIndex)
{
    // Create a Font Display List from font name and size
    HDC hDC = GetDC(NULL);

    HFONT hFont, hOldFont;
    hFont = CreateFont(fontSize, 0, 0, 0,
        FW_REGULAR, FALSE, FALSE, FALSE, ANSI_CHARSET,
        OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS, PROOF_QUALITY,
        FIXED_PITCH | FF_MODERN, fontName);

    hOldFont = (HFONT)SelectObject (hDC, hFont);
    wglUseFontBitmaps(hDC, 0, 255, baseDLIndex);
    SelectObject(hDC, hOldFont);
    DeleteObject(hFont);

    ReleaseDC(NULL, hDC);
}

void StereoPlayer::reshape(int w, int h)
{
    // Keep the rendering size parameters
    m_wwidth = w;
    m_wheight = h;

    // Stencil Buffer need to be updated
    switch (m_stereoType) {
        case STEREO_TYPE_HORIZONTAL_INTERLEAVED:
        case STEREO_TYPE_VERTICAL_INTERLEAVED:
        case STEREO_TYPE_SHARP3D:
            m_needsRefreshStencilBuffer = TRUE;
            break;
    }
}

#define BUFFER_OFFSET(i) ((char *)NULL + (i))

// BIG function for rendeing the image planes, the statistics information and the play control
void StereoPlayer::render()
{
    unsigned int which;

    // Tell the graph manager thread that we're waiting for image buffer updates
    if (!m_initialized) {
        for (which=0; which<NUM_TEXTURES; which++)
            SetEvent(m_readyToUpdateTextureEvent[which]);
        return;
    }

    // Wail until the graph manager thread updates the image buffer
    DWORD waitResult = WaitForMultipleObjects(NUM_TEXTURES, m_readyToUseTextureEvent, FALSE, 1000/60);

    if (waitResult >= WAIT_OBJECT_0 && waitResult < WAIT_OBJECT_0 + NUM_TEXTURES)
    {
        // OK, the image buffer is updated.

        for (which=0; which<NUM_TEXTURES; which++)
        {
            if (m_graphManager[which] && m_textureModfied[which]) {
#ifdef BUFFER_COPY
#ifdef PBO
                if (GLEW_EXT_pixel_buffer_object)
                {
                    if (m_pixelBuffer[which])
                    {
                        // Copy the image buffer data into the Pixel Buffer Object
                        glBindBuffer(GL_PIXEL_UNPACK_BUFFER_EXT, m_textureBufferObject[which]);
                        void *pboMemory = glMapBuffer(GL_PIXEL_UNPACK_BUFFER_EXT, GL_WRITE_ONLY);  
                        memcpy(pboMemory, m_pixelBuffer[which], m_sourceWidth[which] * m_sourceHeight[which] * 3);
                        // Now the graph manager thread can go ahead...
                        SetEvent(m_readyToUpdateTextureEvent[which]);
                        if (!glUnmapBuffer(GL_PIXEL_UNPACK_BUFFER_EXT)) {
                            OutputDebugString("glUnmapBuffer() error\n");
                        }

                        // Update the texture
                        glBindTexture(GL_TEXTURE_2D, m_textureID[which]);
                        glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_sourceWidth[which], m_sourceHeight[which],
                                        GL_BGR, GL_UNSIGNED_BYTE, BUFFER_OFFSET(0));

                        glBindBuffer(GL_PIXEL_UNPACK_BUFFER_EXT, 0);
                    }
                    else
                        SetEvent(m_readyToUpdateTextureEvent[which]);
                }
                else
#endif //PBO
                {
                    if (m_pixelBuffer[which]) {
                        // Normally copy the image buffer for thread safety?
                        memcpy(m_textureBuffer[which], m_pixelBuffer[which], m_sourceWidth[which] * m_sourceHeight[which] * 3);
                        // Now the graph manager thread can go ahead...
                        SetEvent(m_readyToUpdateTextureEvent[which]);
                        // Update the texture
                        glBindTexture(GL_TEXTURE_2D, m_textureID[which]);
                        glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_sourceWidth[which], m_sourceHeight[which],
                                        GL_BGR, GL_UNSIGNED_BYTE, (void*)m_textureBuffer[which]);
                    }
                    else
                        SetEvent(m_readyToUpdateTextureEvent[which]);
                }
#else //BUFFER_COPY
                if (m_pixelBuffer[which]) {
                    // We don't copy any image buffer. It's of course fast, but is it danger or not?
                    // Update the texture
                    glBindTexture(GL_TEXTURE_2D, m_textureID[which]);
                    glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_sourceWidth[which], m_sourceHeight[which],
                                    GL_BGR, GL_UNSIGNED_BYTE, (void*)m_pixelBuffer[which]);
                }
                SetEvent(m_readyToUpdateTextureEvent[which]);
#endif //BUFFER_COPY
                m_textureModfied[which] = FALSE;
            }
        }

    }
/*  else
    {
        // Waiting the event is timed out.
        for (which=0; which<NUM_TEXTURES; which++)
        {
            SetEvent(m_readyToUpdateTextureEvent[which]);
        }
    }
*/

    // Smart aspect ratio calculation...

    // Aspect ratio of the source
    float sourceAspect[NUM_TEXTURES] = { 1.0f, 1.0f };
    if (m_sourceWidth[TEXTURE_LEFT] != 0) {
        sourceAspect[TEXTURE_LEFT] = (float)m_sourceWidth[TEXTURE_LEFT] / m_sourceHeight[TEXTURE_LEFT];
        sourceAspect[TEXTURE_RIGHT] = sourceAspect[TEXTURE_LEFT];
	}
    if (m_sourceWidth[TEXTURE_RIGHT] != 0) {
        sourceAspect[TEXTURE_RIGHT] = (float)m_sourceWidth[TEXTURE_RIGHT] / m_sourceHeight[TEXTURE_RIGHT];
	}

    // Aspect ratio of the rendering area
    float renderAspect = 1.0f;
    if (m_wheight != 0)
        renderAspect = ((float)m_wwidth / m_wheight);
    // In the horizontal split or the vertical split stereo type, the viewport is half of the rendering area
    if (m_stereoType == STEREO_TYPE_HORIZONTAL)
        renderAspect *= 0.5f;
    else if (m_stereoType == STEREO_TYPE_VERTICAL)
        renderAspect *= 2.0f;
    switch (m_stereoFormat) {
    case STEREO_FORMAT_SEPARATED:
    case STEREO_FORMAT_HORIZONTAL_COMP:
    case STEREO_FORMAT_VERTICAL_COMP:
        break;
    case STEREO_FORMAT_HORIZONTAL:
        renderAspect *= 2.0f;
        break;
    case STEREO_FORMAT_VERTICAL:
        renderAspect *= 0.5f;
        break;
    }
    // Decide between width and height of the source to fit ro
    bool fitWidth = TRUE;
    if (m_sourceHeight[TEXTURE_LEFT] > 0 && renderAspect >= (float)m_sourceWidth[TEXTURE_LEFT] / m_sourceHeight[TEXTURE_LEFT])
        fitWidth = FALSE;

	if (!m_keepAspectRatio) {
		sourceAspect[TEXTURE_LEFT] = renderAspect;
		sourceAspect[TEXTURE_RIGHT] = renderAspect;
	}

    // Set up the vertices
    float leftRect[4][NUM_TEXTURES], rightRect[4][NUM_TEXTURES];    // Vertices of the image planes
    if (fitWidth) {
        leftRect[0][0] = m_offset-1.0f; leftRect[0][1] = -1.0f * (renderAspect / sourceAspect[TEXTURE_LEFT]);
        leftRect[1][0] = m_offset+1.0f; leftRect[1][1] = -1.0f * (renderAspect / sourceAspect[TEXTURE_LEFT]);
        leftRect[2][0] = m_offset+1.0f; leftRect[2][1] =  1.0f * (renderAspect / sourceAspect[TEXTURE_LEFT]);
        leftRect[3][0] = m_offset-1.0f; leftRect[3][1] =  1.0f * (renderAspect / sourceAspect[TEXTURE_LEFT]);
        rightRect[0][0] = -m_offset-1.0f; rightRect[0][1] = -1.0f * (renderAspect / sourceAspect[TEXTURE_RIGHT]);
        rightRect[1][0] = -m_offset+1.0f; rightRect[1][1] = -1.0f * (renderAspect / sourceAspect[TEXTURE_RIGHT]);
        rightRect[2][0] = -m_offset+1.0f; rightRect[2][1] =  1.0f * (renderAspect / sourceAspect[TEXTURE_RIGHT]);
        rightRect[3][0] = -m_offset-1.0f; rightRect[3][1] =  1.0f * (renderAspect / sourceAspect[TEXTURE_RIGHT]);
    } else {
        leftRect[0][0] = m_offset-1.0f / (renderAspect / sourceAspect[TEXTURE_LEFT]); leftRect[0][1] = -1.0f;
        leftRect[1][0] = m_offset+1.0f / (renderAspect / sourceAspect[TEXTURE_LEFT]); leftRect[1][1] = -1.0f;
        leftRect[2][0] = m_offset+1.0f / (renderAspect / sourceAspect[TEXTURE_LEFT]); leftRect[2][1] =  1.0f;
        leftRect[3][0] = m_offset-1.0f / (renderAspect / sourceAspect[TEXTURE_LEFT]); leftRect[3][1] =  1.0f;
        rightRect[0][0] = -m_offset-1.0f / (renderAspect / sourceAspect[TEXTURE_RIGHT]); rightRect[0][1] = -1.0f;
        rightRect[1][0] = -m_offset+1.0f / (renderAspect / sourceAspect[TEXTURE_RIGHT]); rightRect[1][1] = -1.0f;
        rightRect[2][0] = -m_offset+1.0f / (renderAspect / sourceAspect[TEXTURE_RIGHT]); rightRect[2][1] =  1.0f;
        rightRect[3][0] = -m_offset-1.0f / (renderAspect / sourceAspect[TEXTURE_RIGHT]); rightRect[3][1] =  1.0f;
    }

    // Set up the texture coordinates
    float u_rt[NUM_TEXTURES];   // Width in the texture coordinate
    float v_rt[NUM_TEXTURES];   // Height in the texture coordinate
    for (which=0; which<NUM_TEXTURES; which++) {
        u_rt[which] = (float)m_sourceWidth[which] / m_textureWidth[which];
        v_rt[which] = (float)m_sourceHeight[which] / m_textureHeight[which];
    }
    if (u_rt[0] == 0.0f || v_rt[0] == 0.0f) {
        u_rt[0] = u_rt[1];
        v_rt[0] = v_rt[1];
    }
    if (u_rt[1] == 0.0f || v_rt[1] == 0.0f) {
        u_rt[1] = u_rt[0];
        v_rt[1] = v_rt[0];
    }

    // Resolve the swapping asignment for the texture object ID
    GLuint swappedTextureID[NUM_TEXTURES] = { m_textureID[TEXTURE_LEFT], m_textureID[TEXTURE_RIGHT] };
    if (m_stereoFormat == STEREO_FORMAT_SEPARATED && m_swap) {
        swappedTextureID[TEXTURE_LEFT] = m_textureID[TEXTURE_RIGHT];
        swappedTextureID[TEXTURE_RIGHT] = m_textureID[TEXTURE_LEFT];
    }

    // If the rendering area is resized or the stereo type is just changed to some type,
    // we have to set up the Stencil Buffer for masking.
    if (m_needsRefreshStencilBuffer)
        setupStencilBuffer();

    // Prepare color and blend mode
    glColorMask(GL_TRUE, GL_TRUE, GL_TRUE, GL_TRUE);
#ifdef COLOR_POLYGON
    if (m_stereoType == STEREO_TYPE_ANAGRYPH) {
        // Anagryph needs additive blend rendering
        glEnable(GL_BLEND);
        glBlendFunc(GL_ONE, GL_ONE);
    }
#endif

    // Enable Texture and Stencil Test bits
    glEnable(GL_TEXTURE_2D);
    if (m_stereoType == STEREO_TYPE_HORIZONTAL_INTERLEAVED ||
        m_stereoType == STEREO_TYPE_VERTICAL_INTERLEAVED ||
        m_stereoType == STEREO_TYPE_SHARP3D)
        glEnable(GL_STENCIL_TEST);
    else
        glDisable(GL_STENCIL_TEST);

    if (m_transition && m_transitionProgress > 0.0f)
        glEnable(GL_BLEND);

    // Clear the color buffers with the base color
    glViewport(0, 0, m_wwidth, m_wheight);
    if (!m_transition || m_transitionProgress == 0.0f)
	    glClearColor(m_baseColor[0], m_baseColor[1], m_baseColor[2], 0.0f);
    if (m_stereoType == STEREO_TYPE_QUADBUFFER) {
        glDrawBuffer(GL_BACK_LEFT);
        if (!m_transition || m_transitionProgress == 0.0f)
	        glClear(GL_COLOR_BUFFER_BIT);
        glDrawBuffer(GL_BACK_RIGHT);
    }
    else
        glDrawBuffer(GL_BACK);
    if (!m_transition || m_transitionProgress == 0.0f)
	    glClear(GL_COLOR_BUFFER_BIT);

    // Set up the view matrix for rendering the image planes (orthogonal and identity model view)
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
    glMatrixMode(GL_PROJECTION);
    glPushMatrix();
    glLoadIdentity();
    glMatrixMode(GL_MODELVIEW);

    bool anagryphBufferCleared = FALSE;
    if (m_stereoType == STEREO_TYPE_ANAGRYPH) {
        // Because the anagryph rendering uses the additive blend rendering,
        // the first rendering path must fill the image plane (rendering area) in black.
        glDisable(GL_BLEND);
        glDisable(GL_TEXTURE_2D);
		if (m_transition)
	        glColor4f(0.0f, 0.0f, 0.0f, 1.0f-m_transitionProgress);
		else
    	    glColor4f(0.0f, 0.0f, 0.0f, 1.0f);
    }
    else {
		if (m_transition)
	        glColor4f(1.0f, 1.0f, 1.0f, 1.0f-m_transitionProgress);
		else
	        glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
	}

    // render the left side
    if (m_stereoType != STEREO_TYPE_RIGHT)
    {
        // Bind the texture object
        if (glIsTexture(swappedTextureID[TEXTURE_LEFT]))
            glBindTexture(GL_TEXTURE_2D, swappedTextureID[TEXTURE_LEFT]);
        else
            goto END_RENDER_LEFT;

        // Prepare the viewpoint and draw buffer depends on the stereo rendering type
        if (m_stereoType == STEREO_TYPE_HORIZONTAL)
            glViewport(0, 0, m_wwidth/2, m_wheight);
        else if (m_stereoType == STEREO_TYPE_VERTICAL)
            glViewport(0, m_wheight/2, m_wwidth, m_wheight/2);
        else if (m_stereoType == STEREO_TYPE_QUADBUFFER)
            glDrawBuffer(GL_BACK_LEFT);

        // View area settings
        glLoadIdentity();
        glScalef(m_zoom, m_zoom, m_zoom);
        glTranslatef(-m_panX, m_panY, 0.0f);

        // Multi-path rendering for the Sharp3D type and the first clear of the anagryph type
        for (unsigned int count=0; count<2; count++)
        {
            if (m_stereoType == STEREO_TYPE_ANAGRYPH && anagryphBufferCleared) {
                // The image plane area is cleared in black, so prepare for rendering in anagryph color
                glEnable(GL_BLEND);
                glEnable(GL_TEXTURE_2D);
#ifdef COLOR_MASK
                glColorMask(GL_TRUE, GL_FALSE, GL_FALSE, GL_FALSE);
				if (m_transition)
	                glColor4f(1.0f, 1.0f, 1.0f, 1.0f-m_transitionProgress);
				else
	                glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
#endif
#ifdef COLOR_POLYGON
				if (m_transition)
	                glColor4f(m_anagryphColor[TEXTURE_LEFT][0], 
							  m_anagryphColor[TEXTURE_LEFT][1], 
							  m_anagryphColor[TEXTURE_LEFT][2], 
							  1.0f-m_transitionProgress);
				else
	                glColor3fv(m_anagryphColor[TEXTURE_LEFT]);
#endif
            }
            else if (m_stereoType == STEREO_TYPE_HORIZONTAL_INTERLEAVED ||
                 m_stereoType == STEREO_TYPE_VERTICAL_INTERLEAVED)
                glStencilFunc(GL_EQUAL,1,1);
            else if (m_stereoType == STEREO_TYPE_SHARP3D) {
                if (count==0) {
                    glStencilFunc(GL_EQUAL, 1, 1);
                    glColorMask(GL_TRUE, GL_FALSE, GL_TRUE, GL_TRUE);
                } else {
                    glStencilFunc(GL_NOTEQUAL, 1, 1);
                    glColorMask(GL_FALSE, GL_TRUE, GL_FALSE, GL_FALSE);
                }
            }

            // Render an image plane
            glBegin(GL_QUADS);
            switch (m_stereoFormat) {
                case STEREO_FORMAT_SEPARATED:
                    glTexCoord2f(0.0f,       0.0f); glVertex3f(leftRect[0][0], leftRect[0][1],  0.0f);
                    glTexCoord2f(u_rt[0],    0.0f); glVertex3f(leftRect[1][0], leftRect[1][1],  0.0f);
                    glTexCoord2f(u_rt[0], v_rt[0]); glVertex3f(leftRect[2][0], leftRect[2][1],  0.0f);
                    glTexCoord2f(0.0f,    v_rt[0]); glVertex3f(leftRect[3][0], leftRect[3][1],  0.0f);
                    break;
                case STEREO_FORMAT_HORIZONTAL:
                case STEREO_FORMAT_HORIZONTAL_COMP:
                    if (!m_swap) {
                        glTexCoord2f(0.0f,            0.0f); glVertex3f(leftRect[0][0], leftRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0]/2.0f,    0.0f); glVertex3f(leftRect[1][0], leftRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0]/2.0f, v_rt[0]); glVertex3f(leftRect[2][0], leftRect[2][1],  0.0f);
                        glTexCoord2f(0.0f,         v_rt[0]); glVertex3f(leftRect[3][0], leftRect[3][1],  0.0f);
                    } else {
                        glTexCoord2f(u_rt[0]/2.0f,    0.0f); glVertex3f(leftRect[0][0], leftRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0],         0.0f); glVertex3f(leftRect[1][0], leftRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0],      v_rt[0]); glVertex3f(leftRect[2][0], leftRect[2][1],  0.0f);
                        glTexCoord2f(u_rt[0]/2.0f, v_rt[0]); glVertex3f(leftRect[3][0], leftRect[3][1],  0.0f);
                    }
                    break;
                case STEREO_FORMAT_VERTICAL:
                case STEREO_FORMAT_VERTICAL_COMP:
                    if (!m_swap) {
                        glTexCoord2f(0.0f,            0.0f); glVertex3f(leftRect[0][0], leftRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0],         0.0f); glVertex3f(leftRect[1][0], leftRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0], v_rt[0]/2.0f); glVertex3f(leftRect[2][0], leftRect[2][1],  0.0f);
                        glTexCoord2f(0.0f,    v_rt[0]/2.0f); glVertex3f(leftRect[3][0], leftRect[3][1],  0.0f);
                    } else {
                        glTexCoord2f(0.0f,    v_rt[0]/2.0f); glVertex3f(leftRect[0][0], leftRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0], v_rt[0]/2.0f); glVertex3f(leftRect[1][0], leftRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0],      v_rt[0]); glVertex3f(leftRect[2][0], leftRect[2][1],  0.0f);
                        glTexCoord2f(0.0f,         v_rt[0]); glVertex3f(leftRect[3][0], leftRect[3][1],  0.0f);
                    }
                    break;
            }
            glEnd();

            if (m_stereoType == STEREO_TYPE_ANAGRYPH && !anagryphBufferCleared) {
                anagryphBufferCleared = TRUE;
                continue;
            }
            if (m_stereoType != STEREO_TYPE_SHARP3D) break;
        }
    }
END_RENDER_LEFT:

    // RIGHT EYE
    if (m_stereoType != STEREO_TYPE_LEFT)
    {
        // Bind the texture object
        if (m_stereoFormat == STEREO_FORMAT_SEPARATED) {
            glBindTexture(GL_TEXTURE_2D, swappedTextureID[TEXTURE_RIGHT]);
            if (!glIsTexture(swappedTextureID[TEXTURE_RIGHT]) ) goto END_RENDER_RIGHT;
        }
        else {
            if (glIsTexture(swappedTextureID[TEXTURE_LEFT]))
                glBindTexture(GL_TEXTURE_2D, swappedTextureID[TEXTURE_LEFT]);
            else
                goto END_RENDER_RIGHT;
        }

        // Prepare the viewpoint and draw buffer depends on the stereo rendering type
        if (m_stereoType == STEREO_TYPE_HORIZONTAL)
            glViewport(m_wwidth/2, 0, m_wwidth/2, m_wheight);
        else if (m_stereoType == STEREO_TYPE_VERTICAL)
            glViewport(0, 0, m_wwidth, m_wheight/2);
        else if (m_stereoType == STEREO_TYPE_QUADBUFFER)
            glDrawBuffer(GL_BACK_RIGHT);

        // View area settings
        glLoadIdentity();
        glScalef(m_zoom, m_zoom, m_zoom);
        glTranslatef(-m_panX, m_panY, 0.0f);

        // Multi-path rendering for the Sharp3D type and the first clear of the anagryph type
        for (unsigned int count=0; count<2; count++)
        {
            if (m_stereoType == STEREO_TYPE_ANAGRYPH && anagryphBufferCleared) {
                // The image plane area is cleared in black, so prepare for rendering in anagryph color
                glEnable(GL_BLEND);
                glEnable(GL_TEXTURE_2D);
#ifdef COLOR_MASK
                glColorMask(GL_FALSE, GL_FALSE, GL_TRUE, GL_FALSE);
				if (m_transition)
	                glColor4f(1.0f, 1.0f, 1.0f, 1.0f-m_transitionProgress);
				else
    	            glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
#endif
#ifdef COLOR_POLYGON
				if (m_transition)
	                glColor4f(m_anagryphColor[TEXTURE_RIGHT][0],
							  m_anagryphColor[TEXTURE_RIGHT][1],
							  m_anagryphColor[TEXTURE_RIGHT][2],
							  1.0f-m_transitionProgress);
				else
	                glColor3fv(m_anagryphColor[TEXTURE_RIGHT]);
#endif
            }
            else if (m_stereoType == STEREO_TYPE_HORIZONTAL_INTERLEAVED ||
                 m_stereoType == STEREO_TYPE_VERTICAL_INTERLEAVED)
                glStencilFunc(GL_NOTEQUAL,1,1);
            else if (m_stereoType == STEREO_TYPE_SHARP3D) {
                if (count==0) {
                    glStencilFunc(GL_NOTEQUAL, 1, 1);
                    glColorMask(GL_TRUE, GL_FALSE, GL_TRUE, GL_TRUE);
                } else {
                    glStencilFunc(GL_EQUAL, 1, 1);
                    glColorMask(GL_FALSE, GL_TRUE, GL_FALSE, GL_FALSE);
                }
            }

            // Render an image plane
            glBegin(GL_QUADS);
            switch (m_stereoFormat) {
                case STEREO_FORMAT_SEPARATED:
                    glTexCoord2f(0.0f,       0.0f); glVertex3f(rightRect[0][0], rightRect[0][1],  0.0f);
                    glTexCoord2f(u_rt[1],    0.0f); glVertex3f(rightRect[1][0], rightRect[1][1],  0.0f);
                    glTexCoord2f(u_rt[1], v_rt[1]); glVertex3f(rightRect[2][0], rightRect[2][1],  0.0f);
                    glTexCoord2f(0.0f,    v_rt[1]); glVertex3f(rightRect[3][0], rightRect[3][1],  0.0f);
                    break;
                case STEREO_FORMAT_HORIZONTAL:
                case STEREO_FORMAT_HORIZONTAL_COMP:
                    if (!m_swap) {
                        glTexCoord2f(u_rt[0]/2.0f,    0.0f); glVertex3f(rightRect[0][0], rightRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0],         0.0f); glVertex3f(rightRect[1][0], rightRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0],      v_rt[0]); glVertex3f(rightRect[2][0], rightRect[2][1],  0.0f);
                        glTexCoord2f(u_rt[0]/2.0f, v_rt[0]); glVertex3f(rightRect[3][0], rightRect[3][1],  0.0f);
                    }
                    else {
                        glTexCoord2f(0.0f,            0.0f); glVertex3f(rightRect[0][0], rightRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0]/2.0f,    0.0f); glVertex3f(rightRect[1][0], rightRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0]/2.0f, v_rt[0]); glVertex3f(rightRect[2][0], rightRect[2][1],  0.0f);
                        glTexCoord2f(0.0f,         v_rt[0]); glVertex3f(rightRect[3][0], rightRect[3][1],  0.0f);
                    }
                    break;
                case STEREO_FORMAT_VERTICAL:
                case STEREO_FORMAT_VERTICAL_COMP:
                    if (!m_swap) {
                        glTexCoord2f(0.0f,    v_rt[0]/2.0f); glVertex3f(rightRect[0][0], rightRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0], v_rt[0]/2.0f); glVertex3f(rightRect[1][0], rightRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0],      v_rt[0]); glVertex3f(rightRect[2][0], rightRect[2][1],  0.0f);
                        glTexCoord2f(0.0f,         v_rt[0]); glVertex3f(rightRect[3][0], rightRect[3][1],  0.0f);
                    } else {
                        glTexCoord2f(0.0f,            0.0f); glVertex3f(rightRect[0][0], rightRect[0][1],  0.0f);
                        glTexCoord2f(u_rt[0],         0.0f); glVertex3f(rightRect[1][0], rightRect[1][1],  0.0f);
                        glTexCoord2f(u_rt[0], v_rt[0]/2.0f); glVertex3f(rightRect[2][0], rightRect[2][1],  0.0f);
                        glTexCoord2f(0.0f,    v_rt[0]/2.0f); glVertex3f(rightRect[3][0], rightRect[3][1],  0.0f);
                    }
                    break;
            }
            glEnd();

            if (m_stereoType == STEREO_TYPE_ANAGRYPH && !anagryphBufferCleared) {
                anagryphBufferCleared = TRUE;
                continue;
            }
            if (m_stereoType != STEREO_TYPE_SHARP3D) break;
        }
    }
END_RENDER_RIGHT:

    glPopMatrix();

    // Check the movie source if it reaches its end (and loop)
    for (which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which])
            m_graphManager[which]->CheckMovieStatus();
    }

    // Next, prepare for rendering the play control and the statistics information

    unsigned long currentTime = ::timeGetTime();
	float intervalInSec = (currentTime - m_ctrlFadeLastTime) / 1000.0f;

    // Calculate alpha value for fading the play control
    if (m_ctrlFadeLastTime > 0)
    {
        if (m_ctrlFadeShowing) {
            m_ctrlFadeAlpha += intervalInSec * 5.0f;
            if (m_ctrlFadeAlpha > 1.0f) m_ctrlFadeAlpha = 1.0f;
        } else {
            m_ctrlFadeAlpha -= intervalInSec * 1.0f;
            if (m_ctrlFadeAlpha < 0.0f) m_ctrlFadeAlpha = 0.0f;
        }
    }
    m_ctrlFadeLastTime = currentTime;

    // Calculate Frame Per Second (FPS)
    m_frames++;
    if (currentTime - m_fpsLastTime > m_fpsInterval * 1000.0f)
    {
        m_fps = (float)m_frames / (currentTime - m_fpsLastTime) * 1000.0f;
        m_fpsLastTime = currentTime;
        m_frames = 0;
    }

    // Render the play control and the statistics information
    if (m_showStatistics || m_playControl != PLAYCONTROL_HIDE)
    {
        // Setup an orthogonal pixel-to-pixel coordinate projection matrix
        glMatrixMode(GL_PROJECTION);
        glPushMatrix();
        glLoadIdentity();
        glOrtho(0.0, m_wwidth, m_wheight, 0.0, 0.0, 1.0);
        glMatrixMode(GL_MODELVIEW);
        glPushMatrix();
        glLoadIdentity();
        glViewport(0, 0, m_wwidth, m_wheight);

        // Bind the texture object of the play control
        glBindTexture(GL_TEXTURE_2D, m_uiTextureID);

        unsigned int sourceIndex[2] = { 0, 0 };
        if (m_stereoFormat == STEREO_FORMAT_SEPARATED) sourceIndex[1] = 1;
        switch (m_stereoType)
        {
        case STEREO_TYPE_LEFT:
            drawStatistics(0, 0, sourceIndex[0]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            break;
        case STEREO_TYPE_RIGHT:
            drawStatistics(0, 0, sourceIndex[1]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            break;
        case STEREO_TYPE_ANAGRYPH:
#ifdef COLOR_MASK
            glColorMask(GL_TRUE, GL_FALSE, GL_FALSE, GL_FALSE);
#endif
#ifdef COLOR_POLYGON
			if (m_transition)
	            glColor4f(m_anagryphColor[TEXTURE_LEFT][0],
						  m_anagryphColor[TEXTURE_LEFT][1],
						  m_anagryphColor[TEXTURE_LEFT][2],
						  1.0f-m_transitionProgress);
			else
	            glColor3fv(m_anagryphColor[TEXTURE_LEFT]);
#endif
            drawStatistics(0, 0, sourceIndex[0]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
#ifdef COLOR_MASK
            glColorMask(GL_FALSE, GL_FALSE, GL_TRUE, GL_FALSE);
#endif
#ifdef COLOR_POLYGON
			if (m_transition)
	            glColor4f(m_anagryphColor[TEXTURE_RIGHT][0],
						  m_anagryphColor[TEXTURE_RIGHT][1],
						  m_anagryphColor[TEXTURE_RIGHT][2],
						  1.0f-m_transitionProgress);
			else
	            glColor3fv(m_anagryphColor[TEXTURE_RIGHT]);
#endif
            drawStatistics(0, 0, sourceIndex[1]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            break;
        case STEREO_TYPE_HORIZONTAL:
            drawStatistics(0, 0, sourceIndex[0]);
            drawPlayControl(0, 0, m_wwidth/2, m_wheight);
            drawStatistics(m_wwidth/2, 0, sourceIndex[1]);
            drawPlayControl(m_wwidth/2, 0, m_wwidth/2, m_wheight);
            break;
        case STEREO_TYPE_VERTICAL:
            drawStatistics(0, 0, sourceIndex[0]);
            drawPlayControl(0, 0, m_wwidth, m_wheight/2);
            drawStatistics(0, m_wheight/2, sourceIndex[1]);
            drawPlayControl(0, m_wheight/2, m_wwidth, m_wheight/2);
            break;
        case STEREO_TYPE_HORIZONTAL_INTERLEAVED:
        case STEREO_TYPE_VERTICAL_INTERLEAVED:
        case STEREO_TYPE_SHARP3D:
            glStencilFunc(GL_EQUAL,1,1);
            drawStatistics(0, 0, sourceIndex[0]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            glStencilFunc(GL_NOTEQUAL,1,1);
            drawStatistics(0, 0, sourceIndex[1]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            break;
        case STEREO_TYPE_QUADBUFFER:
            glDrawBuffer(GL_BACK_LEFT);
            drawStatistics(0, 0, sourceIndex[0]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            glDrawBuffer(GL_BACK_RIGHT);
            drawStatistics(0, 0, sourceIndex[1]);
            drawPlayControl(0, 0, m_wwidth, m_wheight);
            break;
       }

        // Restore the the last projection matrix
        glPopMatrix();
        glMatrixMode(GL_PROJECTION);
        glPopMatrix();
        glMatrixMode(GL_MODELVIEW);
    }

#ifdef COLOR_POLYGON
    if (m_stereoType == STEREO_TYPE_ANAGRYPH)
        glDisable(GL_BLEND);
#endif

    if (m_stereoType == STEREO_TYPE_HORIZONTAL_INTERLEAVED ||
        m_stereoType == STEREO_TYPE_VERTICAL_INTERLEAVED ||
        m_stereoType == STEREO_TYPE_SHARP3D)
        glDisable(GL_STENCIL_TEST);

    if (m_transitionProgress > 0.0f)
        m_transitionProgress -= intervalInSec*0.25f;
    else
        m_transitionProgress = 0.0f;
}

void StereoPlayer::toggleFormat()
{
    // Change the stereo format in cycles
    switch (m_stereoFormat) {
        case STEREO_FORMAT_SEPARATED: setFormat(STEREO_FORMAT_HORIZONTAL); break;
        case STEREO_FORMAT_HORIZONTAL: setFormat(STEREO_FORMAT_HORIZONTAL_COMP); break;
        case STEREO_FORMAT_HORIZONTAL_COMP: setFormat(STEREO_FORMAT_VERTICAL); break;
        case STEREO_FORMAT_VERTICAL: setFormat(STEREO_FORMAT_VERTICAL_COMP); break;
        case STEREO_FORMAT_VERTICAL_COMP: setFormat(STEREO_FORMAT_SEPARATED); break;
    }
}

void StereoPlayer::toggleType()
{
    // Change the stereo type in cycles
    switch (m_stereoType) {
        case STEREO_TYPE_LEFT: setType(STEREO_TYPE_RIGHT); break;
        case STEREO_TYPE_RIGHT: setType(STEREO_TYPE_ANAGRYPH); break;
        case STEREO_TYPE_ANAGRYPH: setType(STEREO_TYPE_HORIZONTAL); break;
        case STEREO_TYPE_HORIZONTAL: setType(STEREO_TYPE_VERTICAL); break;
        case STEREO_TYPE_VERTICAL: setType(STEREO_TYPE_HORIZONTAL_INTERLEAVED); break;
        case STEREO_TYPE_HORIZONTAL_INTERLEAVED: setType(STEREO_TYPE_VERTICAL_INTERLEAVED); break;
        case STEREO_TYPE_VERTICAL_INTERLEAVED: setType(STEREO_TYPE_SHARP3D); break;
        case STEREO_TYPE_SHARP3D:  // Can we use Quad Buffer?
            if (m_stereoEnabled) setType(STEREO_TYPE_QUADBUFFER); else setType(STEREO_TYPE_LEFT); break;
        case STEREO_TYPE_QUADBUFFER: setType(STEREO_TYPE_LEFT); break;
    }
}

void StereoPlayer::setSwap(BOOL swap)
{
    // Swap Left/Right side asignment
    m_swap = swap;
}

BOOL StereoPlayer::getSwap()
{
    return m_swap;
}

void StereoPlayer::setOffset(float offset)
{
    // Set the stereo offset between the sides
    m_offset = offset;
}

float StereoPlayer::getOffset()
{
    return m_offset;
}

void StereoPlayer::setStatisticsColor(float r, float g, float b)
{
    m_statisticsColor[0] = r;
    m_statisticsColor[1] = g;
    m_statisticsColor[2] = b;
}

void StereoPlayer::getStatisticsColor(float* r, float* g, float* b)
{
    *r = m_statisticsColor[0];
    *g = m_statisticsColor[1];
    *b = m_statisticsColor[2];
}

void StereoPlayer::setBaseColor(float r, float g, float b)
{
    m_baseColor[0] = r;
    m_baseColor[1] = g;
    m_baseColor[2] = b;
}

void StereoPlayer::getBaseColor(float* r, float* g, float* b)
{
    *r = m_baseColor[0];
    *g = m_baseColor[1];
    *b = m_baseColor[2];
}

void StereoPlayer::setAnagryphColor(unsigned int which, float r, float g, float b)
{
    if (which >= NUM_TEXTURES) return;

    m_anagryphColor[which][0] = r;
    m_anagryphColor[which][1] = g;
    m_anagryphColor[which][2] = b;
}

void StereoPlayer::getAnagryphColor(unsigned int which, float* r, float* g, float* b)
{
    if (which >= NUM_TEXTURES) return;

    *r = m_anagryphColor[which][0];
    *g = m_anagryphColor[which][1];
    *b = m_anagryphColor[which][2];
}

void StereoPlayer::setStatistics(BOOL statistics)
{
    m_showStatistics = statistics;
    m_fps = 0.0f;
}

BOOL StereoPlayer::getStatistics()
{
    return m_showStatistics;
}

void StereoPlayer::setPlayControl(unsigned int playControl)
{
    m_playControl = playControl;
}

unsigned int StereoPlayer::getPlayControl()
{
    return m_playControl;
}

void StereoPlayer::drawText(int offx, int offy, int line, const char* text, ...)
{
    // Draw a text mesage at the specified position and after some blank lines
    va_list ap;
    va_start(ap, text);
    char buffer[1024];
    StringCchVPrintf(buffer, 1024, text, ap);
    glRasterPos2i(offx + 4, offy + m_fontSize * line);
    glCallLists (strlen(buffer), GL_UNSIGNED_BYTE, buffer);
    va_end(ap);
}

void StereoPlayer::drawStatistics(int offx, int offy, int which)
{
    if (!m_showStatistics) return;

    // Render the statistics information

    glPushAttrib(GL_CURRENT_BIT);

    glDisable(GL_TEXTURE_2D);

#ifdef COLOR_POLYGON
    if (m_stereoType != STEREO_TYPE_ANAGRYPH)
        glColor4f(m_statisticsColor[0], m_statisticsColor[1], m_statisticsColor[2], 1.0);
    else
        glBlendFunc(GL_ONE, GL_ONE);
#else
    glColor4f(m_statisticsColor[0], m_statisticsColor[1], m_statisticsColor[2], 1.0);
#endif

    glListBase (STATS_FONT_DL_INDEX);

    int line = 0;
    drawText(offx, offy, ++line, "Filename: %s", m_textureFileName[which]);
    drawText(offx, offy, ++line, "FPS:      %.2f", m_fps);
    drawText(offx, offy, ++line, "Size:     %i x %i", m_sourceWidth[which], m_sourceHeight[which]);
    drawText(offx, offy, ++line, "Viewing:  x %.2f @ (%.2f, %.2f)", m_zoom, m_panX, m_panY);
//  drawText(offx, offy, ++line, "Dropped Frame:  %i", getNumDropFrames(which));    // not work yet...
    double pos = 0.0;
    if (m_graphManager[which])
        m_graphManager[which]->GetPosition(&pos);
    drawText(offx, offy, ++line, "Position: %f", pos);
    drawText(offx, offy, ++line, "Duration: %f", m_duration[which]);
    drawText(offx, offy, ++line, "Speed:    x %.2f", getRate());
    drawText(offx, offy, ++line, "State:    %s", isPlaying()?"Playing":"Stop");
    drawText(offx, offy, ++line, "Loop:     %s", getLoop()?"Yes":"No");
    drawText(offx, offy, ++line, "Sync:     %s", getForceSync()?"Force":"Don't care");
    drawText(offx, offy, ++line, "Audio:    %s", m_withAudio[which]?"Load":"Ignore");
    drawText(offx, offy, ++line, "Volume:   %i", getVolume());
    switch (m_stereoFormat) {
        case STEREO_FORMAT_SEPARATED:       drawText(offx, offy, ++line, "Format:   Separate movies"); break;
        case STEREO_FORMAT_HORIZONTAL:      drawText(offx, offy, ++line, "Format:   Horizontally combined"); break;
        case STEREO_FORMAT_HORIZONTAL_COMP: drawText(offx, offy, ++line, "Format:   Horizontally combined and compressed"); break;
        case STEREO_FORMAT_VERTICAL:        drawText(offx, offy, ++line, "Format:   Vertically combined"); break;
        case STEREO_FORMAT_VERTICAL_COMP:   drawText(offx, offy, ++line, "Format:   Vertically combined and compressed"); break;
    }
    switch (m_stereoType) {
        case STEREO_TYPE_LEFT:       drawText(offx, offy, ++line, "Stereo:   Left only"); break;
        case STEREO_TYPE_RIGHT:      drawText(offx, offy, ++line, "Stereo:   Right only"); break;
        case STEREO_TYPE_ANAGRYPH:   drawText(offx, offy, ++line, "Stereo:   Anagryph"); break;
        case STEREO_TYPE_HORIZONTAL: drawText(offx, offy, ++line, "Stereo:   Horizontal split"); break;
        case STEREO_TYPE_VERTICAL: drawText(offx, offy, ++line, "Stereo:   Vertical split"); break;
        case STEREO_TYPE_HORIZONTAL_INTERLEAVED: drawText(offx, offy, ++line, "Stereo:   Horizontal interleaved"); break;
        case STEREO_TYPE_VERTICAL_INTERLEAVED: drawText(offx, offy, ++line, "Stereo:   Vertical interleaved"); break;
        case STEREO_TYPE_SHARP3D: drawText(offx, offy, ++line, "Stereo:   Sharp3D"); break;
        case STEREO_TYPE_QUADBUFFER: drawText(offx, offy, ++line, "Stereo:   Quad-buffer"); break;
    }
    drawText(offx, offy, ++line, "Swapped:  %s", m_swap?"Yes":"No");
    drawText(offx, offy, ++line, "Offset:   %.2f", m_offset);

    glEnable(GL_TEXTURE_2D);
    glPopAttrib();
}

#define BUTTON_SIZE 32

void StereoPlayer::drawPlayControl(int offx, int offy, int wwidth, int wheight)
{
    if (m_playControl == PLAYCONTROL_HIDE) return;

    if (!m_graphManager[TEXTURE_LEFT] && !m_graphManager[TEXTURE_RIGHT]) return;

    // Render the Play Control with textured polygons

    glPushMatrix();
    glPushAttrib(GL_CURRENT_BIT);

    glTranslatef((float)offx, (float)offy, 0);

    if (m_playControl == PLAYCONTROL_SHOW)
        glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
    else
        glColor4f(1.0f, 1.0f, 1.0f, m_ctrlFadeAlpha);

#ifdef COLOR_POLYGON
    if (m_stereoType != STEREO_TYPE_ANAGRYPH)
#endif
        glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

    int offset = 0;

    glBegin(GL_QUADS);

    float playOffset = 0.0f;

    // Play/Pause button
    if (isPlaying())
        playOffset += 0.125f;
    glTexCoord2f(playOffset, m_playUIState);
    glVertex2i(offset, wheight - BUTTON_SIZE);
    glTexCoord2f(playOffset, m_playUIState + 0.25f);
    glVertex2i(offset, wheight);
    glTexCoord2f(playOffset + 0.125f, m_playUIState + 0.25f);
    glVertex2i(offset + BUTTON_SIZE, wheight);
    glTexCoord2f(playOffset + 0.125f, m_playUIState);
    glVertex2i(offset + BUTTON_SIZE, wheight - BUTTON_SIZE);
    
    offset += BUTTON_SIZE;

    // Stop button
    glTexCoord2f(0.25f, m_stopUIState);
    glVertex2i(offset + 0, wheight - BUTTON_SIZE);
    glTexCoord2f(0.25f, m_stopUIState + 0.25f);
    glVertex2i(offset + 0, wheight);
    glTexCoord2f(0.375f, m_stopUIState + 0.25f);
    glVertex2i(offset + BUTTON_SIZE, wheight);
    glTexCoord2f(0.375f, m_stopUIState);
    glVertex2i(offset + BUTTON_SIZE, wheight - BUTTON_SIZE);

    offset += BUTTON_SIZE;

    // Left side of Position slider
    glTexCoord2f(0.5f, 0.0f);
    glVertex2i(offset + 0, wheight - BUTTON_SIZE);
    glTexCoord2f(0.5f, 0.25f);
    glVertex2i(offset + 0, wheight);
    glTexCoord2f(0.625f, 0.25f);
    glVertex2i(offset + BUTTON_SIZE, wheight);
    glTexCoord2f(0.625f, 0.0f);
    glVertex2i(offset + BUTTON_SIZE, wheight - BUTTON_SIZE);

    offset += BUTTON_SIZE;

    // Center of Position slider
    glTexCoord2f(0.625f, 0.0f);
    glVertex2i(offset + 0, wheight - BUTTON_SIZE);
    glTexCoord2f(0.625f, 0.25f);
    glVertex2i(offset + 0, wheight);
    glTexCoord2f(0.75f, 0.25f);
    glVertex2i(wwidth - BUTTON_SIZE * 4, wheight);
    glTexCoord2f(0.75f, 0.0f);
    glVertex2i(wwidth - BUTTON_SIZE * 4, wheight - BUTTON_SIZE);

    offset = wwidth - BUTTON_SIZE * 4;

    // Right side of Position slider
    glTexCoord2f(0.75f, 0.0f);
    glVertex2i(offset + 0, wheight - BUTTON_SIZE);
    glTexCoord2f(0.75f, 0.25f);
    glVertex2i(offset + 0, wheight);
    glTexCoord2f(0.875f, 0.25f);
    glVertex2i(offset + BUTTON_SIZE, wheight);
    glTexCoord2f(0.875f, 0.0f);
    glVertex2i(offset + BUTTON_SIZE, wheight - BUTTON_SIZE);

    offset += BUTTON_SIZE;

    // Position/duration text area
    glTexCoord2f(0.5f, 0.25f);
    glVertex2i(offset + 0, wheight - BUTTON_SIZE);
    glTexCoord2f(0.5f, 0.5f);
    glVertex2i(offset + 0, wheight);
    glTexCoord2f(0.875f, 0.5f);
    glVertex2i(wwidth, wheight);
    glTexCoord2f(0.875f, 0.25f);
    glVertex2i(wwidth, wheight - BUTTON_SIZE);

    // Position slider knob
    m_sliderPos = -1000;
    if (getDuration() > 0.0f)
    {
        m_sliderPos = BUTTON_SIZE * 3 - 26 + (int)((wwidth - BUTTON_SIZE * 7 + 52) * getPosition() / getDuration()) - 16;
        glTexCoord2f(0.375f, m_sliderUIState);
        glVertex2i(m_sliderPos, wheight - BUTTON_SIZE);
        glTexCoord2f(0.375f, m_sliderUIState + 0.25f);
        glVertex2i(m_sliderPos, wheight);
        glTexCoord2f(0.5f, m_sliderUIState + 0.25f);
        glVertex2i(m_sliderPos + BUTTON_SIZE, wheight);
        glTexCoord2f(0.5f, m_sliderUIState);
        glVertex2i(m_sliderPos + BUTTON_SIZE, wheight - BUTTON_SIZE);
    }

    glEnd();

    // Position/duration text
    glDisable(GL_TEXTURE_2D);
    if (m_playControl == PLAYCONTROL_SHOW)
        glColor4f(0.0f, 0.0f, 0.0f, 1.0f);
    else
        glColor4f(0.0f, 0.0f, 0.0f, m_ctrlFadeAlpha);

    char timeText[256];
    double time = getPosition();
    double duration = getDuration();
    StringCchPrintf(timeText, 256, "%02i:%02i/%02i:%02i",
        (int)time/60, (int)time - (int)time/60*60,
        (int)duration/60, (int)duration - (int)duration/60*60);
    glListBase (PLAYCTRL_FONT_DL_INDEX);
    drawText(wwidth - BUTTON_SIZE * 3 + 6, wheight - 11, 0, timeText);

#ifdef COLOR_POLYGON
    if (m_stereoType != STEREO_TYPE_ANAGRYPH)
#endif
    glDisable(GL_BLEND);

    glEnable(GL_TEXTURE_2D);

    glPopMatrix();
    glPopAttrib();
}

void StereoPlayer::mouseUp(int x, int y)
{
    // Mouse click event handling

    int wwidth = m_wwidth;
    int wheight= m_wheight;
    switch (m_stereoType)
    {
        case STEREO_TYPE_HORIZONTAL:
            if (x > m_wwidth/2)
                x -= m_wwidth/2;
            wwidth /= 2;
            break;
        case STEREO_TYPE_VERTICAL:
            if (y > m_wheight/2)
                y -= m_wheight/2;
            wheight /= 2;
            break;
        default:
            break;
    }

    if (y >= wheight - BUTTON_SIZE && y <= wheight)
    {
        if (x >= 0 && x < BUTTON_SIZE) {
            // Play/Pause button
            if (isPlaying())
                pause();
            else
                play();
        }
        else if (x >= BUTTON_SIZE && x < BUTTON_SIZE * 2) {
            // Stop button
            stop();
        }
    }
}

void StereoPlayer::mouseMove(int x, int y, BOOL down)
{
    // Mouse move event handling

    // Rescale the mouse position depends on the current stereo type
    int wwidth = m_wwidth;
    int wheight= m_wheight;
    switch (m_stereoType)
    {
        case STEREO_TYPE_HORIZONTAL:
            if (x > m_wwidth/2)
                x -= m_wwidth/2;
            wwidth /= 2;
            break;
        case STEREO_TYPE_VERTICAL:
            if (y > m_wheight/2)
                y -= m_wheight/2;
            wheight /= 2;
            break;
        default:
            break;
    }

    // 'Mouse Over' detections
    m_playUIState = 0.0f;
    m_stopUIState = 0.0f;
    m_sliderUIState = 0.0f;
    if (y >= wheight - BUTTON_SIZE && y <= wheight)
    {
        if (x >= 0 && x < BUTTON_SIZE) {
            if (!down)
                m_playUIState = 0.25f;
            else
                m_playUIState = 0.5f;
        }
        else if (x >= BUTTON_SIZE && x < BUTTON_SIZE * 2) {
            if (!down)
                m_stopUIState = 0.25f;
            else
                m_stopUIState = 0.5f;
        }
        else if (x >= m_sliderPos && x < m_sliderPos + BUTTON_SIZE) {
            m_sliderUIState = 0.25f;
        }

        // Dragging the Position slider knob
        if (x >= BUTTON_SIZE * 3 - 26 - 16 && x < wwidth - BUTTON_SIZE * 4 + 26 + (32/* margine for stop the last frame*/) && down && getDuration() > 0.0f) {
            float pos = (float)(x - (BUTTON_SIZE * 3 - 26)) / (wwidth - BUTTON_SIZE * 7 + 52);
            if (pos < 0.0f) pos = 0.0f;
            if (pos > 1.0f) pos = 1.0f;
            setPosition(getDuration() * pos);
        }

        m_ctrlFadeShowing = TRUE;
    }
    else
        m_ctrlFadeShowing = FALSE;
}

void StereoPlayer::clearEvent()
{
    // Release the waiting loop in the graph manager thread
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) SetEvent(m_readyToUpdateTextureEvent[which]);
    }
}

void StereoPlayer::play()
{
    clearEvent();

    if (m_forceSync)
        setPosition(getPosition());

    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->Run();
    }
}

void StereoPlayer::pause()
{
    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->Pause();
    }

    if (m_forceSync)
        setPosition(getPosition());
}

void StereoPlayer::stop(BOOL forcedStop)
{
    clearEvent();
    setPosition(0.0);
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->Stop(FALSE);
    }

    if (m_forceSync)
        setPosition(getPosition());
}

BOOL StereoPlayer::isPlaying(unsigned int which)
{
    if (which >= NUM_TEXTURES) return FALSE;

    clearEvent();
    if (m_graphManager[which]) {
        BOOL playing;
        m_graphManager[which]->IsPlaying(&playing);
        return playing;
    }
    return FALSE;
}

int StereoPlayer::getNumDropFrames(unsigned int which)
{
    if (which >= NUM_TEXTURES) return 0;

    clearEvent();
    if (m_graphManager[which]) {
        int framesDropped;
        m_graphManager[which]->GetFramesDropped(&framesDropped);
        return framesDropped;
    }
    return 0;
}

void StereoPlayer::setLoop(BOOL loop)
{
    // Keep the value in a local variable for reconstruction of the graph manager
    m_loop = loop;

    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->SetLoop(loop);
    }
}

BOOL StereoPlayer::getLoop()
{
    return m_loop;
}

void StereoPlayer::setPosition(double pos)
{
    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->SetPosition(pos);
    }
}

double StereoPlayer::getPosition()
{
    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) {
            double pos;
            if (m_graphManager[which]->GetPosition(&pos) == S_OK)
                return pos;
        }
    }
    return 0.0;
}

void StereoPlayer::setForceSync(BOOL sync)
{
    m_forceSync = sync;
}

BOOL StereoPlayer::getForceSync()
{
    return m_forceSync;
}

void StereoPlayer::setRate(double rate)
{
    // Keep the value in a local variable for reconstruction of the graph manager
    m_speed = rate;

    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->SetRate(rate);
    }
}

double StereoPlayer::getRate()
{
    return m_speed;
}

void StereoPlayer::setVolume(LONG volume)
{
    // Keep the value in a local variable for reconstruction of the graph manager
    m_volume = volume;

    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->SetVolume(volume);
    }
}

LONG StereoPlayer::getVolume()
{
    return m_volume;
}

void StereoPlayer::volumeSilence()
{
    m_volume = -10000L;

    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->VolumeSilence();
    }
}

void StereoPlayer::volumeFull()
{
    m_volume = 0L;

    clearEvent();
    for (unsigned int which=0; which<NUM_TEXTURES; which++) {
        if (m_graphManager[which]) m_graphManager[which]->VolumeFull();
    }
}
