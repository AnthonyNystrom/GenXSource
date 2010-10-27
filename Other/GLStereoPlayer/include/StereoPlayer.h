//-----------------------------------------------------------------------------
// StereoPlayer.cpp : Interface of the StereoPlayer class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#ifndef _STEREOPLAYER_H_
#define _STEREOPLAYER_H_

#include <stdlib.h>
#include <stdio.h>
#include <strsafe.h>

// OpenGL headers
#include <GL/gl.h>
#include <GL/glu.h>

namespace glsp
{

// Stereo source formats
const unsigned int STEREO_FORMAT_SEPARATED       = 0;
const unsigned int STEREO_FORMAT_HORIZONTAL      = 1;
const unsigned int STEREO_FORMAT_HORIZONTAL_COMP = 2;
const unsigned int STEREO_FORMAT_VERTICAL        = 3;
const unsigned int STEREO_FORMAT_VERTICAL_COMP   = 4;

// Stereo rendering types
const unsigned int STEREO_TYPE_LEFT       = 0;
const unsigned int STEREO_TYPE_RIGHT      = 1;
const unsigned int STEREO_TYPE_ANAGRYPH   = 2;
const unsigned int STEREO_TYPE_HORIZONTAL = 3;
const unsigned int STEREO_TYPE_VERTICAL = 4;
const unsigned int STEREO_TYPE_HORIZONTAL_INTERLEAVED = 5;
const unsigned int STEREO_TYPE_VERTICAL_INTERLEAVED = 6;
const unsigned int STEREO_TYPE_SHARP3D = 7;
const unsigned int STEREO_TYPE_QUADBUFFER = 8;

// Play Control styles
const unsigned int PLAYCONTROL_HIDE = 0;
const unsigned int PLAYCONTROL_AUTO = 1;
const unsigned int PLAYCONTROL_SHOW = 2;

// Don't change!!
#define NUM_TEXTURES  2
#define TEXTURE_LEFT  0
#define TEXTURE_RIGHT 1

// Stereo Movie/Image Player class
class StereoPlayer
{
public:

    StereoPlayer();             // Constructor
    virtual ~StereoPlayer();    // Destructor

	float getVersion() { return 0.600f; }

    void initializeGL();    // Initialize/reconstruct OpenGL objects and resources.
    void terminateGL();     // Destroy OpenGL objects.

    void setStereoEnabled(bool enabled);    // Set the Quad Buffer availability.
    BOOL getStereoEnabled();                // Get the Quad Buffer availability.
    void setupStencilBuffer();              // Setup Stencil Buffer for the interlace masking.

    // File Loading
    void setHomeDir(const char* homeDir) { StringCchCopy(m_homeDir, _MAX_PATH, homeDir); }    // Set the home directory for loading the gui image and source files.
    const char* getHomeDir() { return m_homeDir; }                          // Get the home directory.
    bool loadFile(unsigned int which, const char* filename, BOOL withAudio=TRUE);   // Load a single image or movie source file.
    bool loadLeftFile(const char* filename, BOOL withAudio=TRUE) { return loadFile(TEXTURE_LEFT, filename, withAudio); }    // Load a left side source file.
    bool loadRightFile(const char* filename, BOOL withAudio=TRUE) { return loadFile(TEXTURE_RIGHT, filename, withAudio); }  // Load a right side source file.
    const char* getFileName(unsigned int which=TEXTURE_LEFT) { if (which < NUM_TEXTURES) return m_textureFileName[which]; return NULL; }    // Get the file name of the source.
    const char* getLeftFileName() { return m_textureFileName[TEXTURE_LEFT]; }   // Get the file name of the left side source.
    const char* getRightFileName() { return m_textureFileName[TEXTURE_RIGHT]; } // Get the file name of the right side source.

    GLuint getWidth(unsigned int which=TEXTURE_LEFT);       // Get the width of the source.
    GLuint getHeight(unsigned int which=TEXTURE_LEFT);      // Get the height of the source.
    double getDuration(unsigned int which=TEXTURE_LEFT);    // Get the duration of the source in sec.

    GLuint getPlayerWidth();                                // Get the width of the player.
    GLuint getPlayerHeight();                               // Get the height of the player.

    // Stereo Settings
    void setFormat(unsigned int format);                    // Set the stereo source format.
    unsigned int getFormat() { return m_stereoFormat; }     // Get the stereo source format.
    void toggleFormat();                                    // Change the stereo source format in cycle.
    void setType(unsigned int type);                        // Set the stereo rendering type.
    unsigned int getType() { return m_stereoType; }         // Get the stereo rendering type.
    void toggleType();                                      // Change the stereo rendering type in cycle.

    void setSwap(BOOL swap);                    // Swap Left/Right side asignment.
    BOOL getSwap();                             // Get Left/Right swapping state.
    void toggleSwap() { setSwap(!getSwap()); }  // Toggle Left/Right side asignment.
	void setKeepAspectRatio(BOOL keepAspect) { m_keepAspectRatio = keepAspect; }  // Set keeping aspect ratio or not.
	BOOL getKeepAspectRatio() { return m_keepAspectRatio; }					      // Get keeping aspect ratio or not.
    void toggleKeepAspectRatio() { setKeepAspectRatio(!getKeepAspectRatio()); }  // Toggle keeping aspect ratio.
    void setOffset(float offset);               // Set the stereo offset.
    float getOffset();                          // Get the stereo offset.

    void setAnagryphColor(unsigned int which, float r, float g, float b);       // Set the anagryph color.
    void getAnagryphColor(unsigned int which, float* r, float* g, float* b);    // Get the anagryph color.

    // Rendering
    void reshape(int width, int height);        // Resize the rendering area.
    void render();                              // Render the image planes, statistics and play control.
    void mouseMove(int x, int y, BOOL down);    // Handle mouse move event.
    void mouseUp(int x, int y);                 // Handle mouse click event.

    void setBaseColor(float r, float g, float b);       // Set base (clear) color.
    void getBaseColor(float* r, float* g, float* b);    // Get base (clear) color.

    void setStatistics(BOOL statistics);                            // Show or hide the statistics information.
    BOOL getStatistics();                                           // Get showing the statistics information or not.
    void toggleStatistics() { setStatistics(!getStatistics()); }    // Toggle show and hide the statistics information.

    void setStatisticsFont(HFONT font); // Create a Font Display List from font hanlde for rendering the statistics.
    void setStatisticsFont(const char* fontName, int fontSize); // Create a Font Display List from font name and size for rendering the statistics.
    const char* getStatisticsFontName();                        // Get font name of the the statistics information.
    int getStatisticsFontSize();                                // Get font size of the the statistics information.
    void setStatisticsColor(float r, float g, float b);         // Get color of the the statistics information.
    void getStatisticsColor(float* r, float* g, float* b);      // Set color of the the statistics information.

    void setPlayControl(unsigned int playControl);                          // Set the play control mode.
    unsigned int getPlayControl();                                          // Get the play control mode.
    void togglePlayControl() { setPlayControl((getPlayControl()+1)%3); }    // Toggle the play control mode in cycles.

    void setPanX(float x) { m_panX = x; }       // Set horizontal view area shift.
    float getPanX() { return m_panX; }          // Get horizontal view area shift.
    void setPanY(float y) { m_panY = y; }       // Set vertical view area shift.
    float getPanY() { return m_panY; }          // Get vertical view area shift.
    void setZoom(float zoom) { m_zoom = zoom; } // Set view area zooming.
    float getZoom() { return m_zoom; }          // Get view area zooming.

    // Play Control
    void play();                                                // Play the movie.
    void pause();                                               // Pause and keep the current image.
    void toggle() { if (isPlaying()) pause(); else play(); }    // Toggle Play and Pause.
    void stop(BOOL forcedStop = FALSE);                         // Stop and rewind.
    BOOL isPlaying(unsigned int which=TEXTURE_LEFT);            // Returns TRUE if currently playing.
    int  getNumDropFrames(unsigned int which=TEXTURE_LEFT);     // Get the number of drop frames (not work yet...).
    void setLoop(BOOL loop);                                    // Set the loop mode.
    BOOL getLoop();                                             // Get the loop mode.
    void toggleLoop() { setLoop(!getLoop()); }                  // Toggle the loop mode.

    void setPosition(double pos);                               // Set playing position in sec.
    double getPosition();                                       // Get playing position in sec.
    void setForceSync(BOOL sync);                               // Force synchronize when play, pause, stop and reposition.
    BOOL getForceSync();                                        // Get Force synchronization mode.
    void toggleForceSync() { setForceSync(!getForceSync()); }   // Toggle Force synchronization mode.
    void setRate(double rate);                                  // Set the playback speed (1.0 is original).
    double getRate();                                           // Get the playback speed (1.0 is original).

    void setVolume(LONG volume);    // Set the sound volume (0L is max, -10000L is min)
    LONG getVolume();               // Get the sound volume (0L is max, -10000L is min)
    void volumeSilence();           // Mute the sound volume.
    void volumeFull();              // Maximize the sound volume.

    void setPlayOnLoad(BOOL playOnLoad) { m_playOnLoad = playOnLoad; }  // Set "Play after loading" or not.
    BOOL getPlayOnLoad() { return m_playOnLoad; }                       // Get "Play after loading" or not.

	void setTransition(BOOL transition) { m_transition = transition; }  // Set fade transiotn or not when loading a new file
	BOOL getTransition() { return m_transition; }						// Get fade transiotn or not when loading a new file

protected:

    void buildFont(const char* fonrName, int fontSize, GLuint baseIndex);   // Create a Font Display List from font name and size.
    void initTexture(unsigned int which);                               // Allocate enough texture memory and generate a texture object.
    BOOL loadImageFile(unsigned int which, const char* filename);       // Load an image file in the specified side.
    void clearEvent();                                                  // Release the waiting loop in the graph manager thread.

    void drawText(int offx, int offy, int line, const char* text, ...); // Draw a text mesage.
    void drawStatistics(int offx, int offy, int which);                 // Render the statistics information.
    void drawPlayControl(int offx, int offy, int wwidth, int wheight);  // Render the Play Control with textured polygons.

    // Texture buffer transfer callback function
    static void rendererCallback(void* obj, BYTE* pPixelBuffer, DWORD which);

protected:

    char m_homeDir[_MAX_PATH];  // Home directory
    int m_wwidth;			    // Width of the player render area
    int m_wheight;              // Height of the player render area
    BOOL  m_stereoEnabled;      // Quad Buffer availability

    class CTextureRendererGraphManager* m_graphManager[NUM_TEXTURES];   // Graph Managers

    HANDLE m_readyToUseTextureEvent[NUM_TEXTURES];
    HANDLE m_readyToUpdateTextureEvent[NUM_TEXTURES];

    char m_textureFileName[NUM_TEXTURES][_MAX_PATH];  // File names of the sources
    GLuint m_textureID[NUM_TEXTURES];           // Texture ID of the sources
    GLuint m_sourceWidth[NUM_TEXTURES];         // Original width of the sources
    GLuint m_sourceHeight[NUM_TEXTURES];        // Original height of the sources
    GLuint m_textureWidth[NUM_TEXTURES];        // 2^n texture width of the sources
    GLuint m_textureHeight[NUM_TEXTURES];       // 2^n texture height of the sources
    BOOL   m_withAudio[NUM_TEXTURES];           // Play sound with the source or not
    double m_duration[NUM_TEXTURES];            // Duration of the sources in sec.
    BYTE* m_pixelBuffer[NUM_TEXTURES];          // Pointer of the updated image buffer
    BYTE* m_textureBuffer[NUM_TEXTURES];        // Image buffers for copying the updated images to
    BOOL  m_textureModfied[NUM_TEXTURES];       // Image data is updated or not
    unsigned int m_textureBufferObject[NUM_TEXTURES];   // Pixel Buffer Objects for copying the updated images to

    unsigned int m_stereoFormat;                // Stereo source format
    unsigned int m_stereoType;                  // Stereo rendering type
    float m_anagryphColor[NUM_TEXTURES][3];     // Anagrypf colors
    BOOL  m_swap;                               // Swap Left/Right side asignment or not
	BOOL m_keepAspectRatio;						// Keep aspect ratio or not
    float m_offset;                             // Stereo offset value
    float m_panX;                               // Horizontal shift of the view area
    float m_panY;                               // Vertical shift of the view area
    float m_zoom;                               // Zoom scale of the view area

    float m_baseColor[3];           // Base (clear) color

    double m_speed;                 // Playback speed (1.0 is original)
    BOOL  m_loop;                   // Loop the playback or not
    long  m_volume;                 // Sound volume (0L is max, -10000L is min)
    BOOL  m_forceSync;              // Force synchronization mode
    BOOL m_playOnLoad;				// Play after loading or not

    // Statistics rendering
    BOOL  m_showStatistics;         // Show the statistics information or not
    char  m_fontName[256];          // Font name of the statistics information
    int   m_fontSize;               // Font size of the statistics information
    float m_statisticsColor[3];     // Color of the statistics information

    // For calculate Frame Per Second (FPS)
    float m_fpsInterval;            // FPS update interval in sec.
    unsigned long m_fpsLastTime;    // Last time when the FPS is updated
    unsigned long m_frames;         // Frame counter
    float m_fps;                    // Current FPS

    // Play Control rendering
    int m_playControl;              // Play Control showing mode
    GLuint m_uiTextureID;           // Texture ID for the play control interface
    float m_playUIState;            // State of the play button (in v-texture coordinate)
    float m_stopUIState;            // State of the stop button (in v-texture coordinate)
    float m_sliderUIState;          // State of the playback position slider knob (in v-texture coordinate)
    int m_sliderPos;                // Position of the playback position slider knob
    BOOL m_ctrlFadeShowing;         // Play Control is appearing or disappearing
    unsigned long m_ctrlFadeLastTime;   // For calculating the play control fading animation interval
    float m_ctrlFadeAlpha;              // Alpha value of the play control

	BOOL  m_transition;                 // Use transition whn loading a new file
    float m_transitionProgress;         // Transition progress counter (begin from 1.0, end to 0.0)

    BOOL m_needsRefreshStencilBuffer;   // Need to refresh the stencil buffer at the next rendering time
    BOOL m_initialized;                 // OpenGL context is initialized or not
};

}; // glsp

#endif //_STEREOPLAYER_H_
