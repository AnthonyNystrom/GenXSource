//-----------------------------------------------------------------------------
// glutStereoPlayer.cpp : GLStereoPlayer with the glut library
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include <stdlib.h>
#include <stdio.h>
#include <windows.h>
#include <gl/glut.h>

#include "StereoPlayer.h"

glsp::StereoPlayer* g_stereoPlayer = NULL;    // StereoPlayer class instance

bool g_quadbuffer = FALSE;              // QuadBuffer availability
bool g_fullscreen = FALSE;				// Full screen mode
char leftFileName[_MAX_PATH] = "none";  // Left source file name
char rightFileName[_MAX_PATH] = "none"; // Right source file name

// Create a StereoPlayer instance
void init()
{
    g_stereoPlayer = new glsp::StereoPlayer;
    g_stereoPlayer->initializeGL();
}

// Destroy the StereoPlayer instance
void term()
{
    if (g_stereoPlayer) {
        g_stereoPlayer->terminateGL();
        delete g_stereoPlayer;
    }
}

// Render the scene
void render()
{
    g_stereoPlayer->render();

    glutSwapBuffers();
}

// Resize the viewport area
void reshape(int w, int h)
{
    g_stereoPlayer->reshape(w, h);
}

// Mouse click event
void mouseFunc(int button, int state, int x, int y)
{
    if (state == GLUT_UP)
        g_stereoPlayer->mouseUp(x, y);
}

// Mouse motion event with buttons down (dragging)
void mouseMotionFunc(int x, int y)
{
    g_stereoPlayer->mouseMove(x, y, TRUE);
}

// Mouse motion event without buttons down (hovering)
void mousePassiveMotionFunc(int x, int y)
{
    g_stereoPlayer->mouseMove(x, y, FALSE);
}

// Handle keyboard short-cuts
void keyboard(unsigned char key, int, int)
{
    switch (key) {
        case 'c':   // Toggle Play/Pause
            g_stereoPlayer->toggle();
            break;
        break;
        case 'x':   // Stop
            g_stereoPlayer->stop();
            break;
        break;
        case 'z':   // Rewind
            g_stereoPlayer->setPosition(0);
            break;
        break;
        case 'f':   // Change the Source Format
            g_stereoPlayer->toggleFormat();
            break;
        break;
        case 't':   // Change the Stereo Rendering Type
            g_stereoPlayer->toggleType();
            break;
        break;
        case 'w':   // Swap Left/Right side
            g_stereoPlayer->toggleSwap();
            break;
        break;
        case 'l':   // Toggle the Loop mode
            g_stereoPlayer->toggleLoop();
            break;
        break;
        case 's':   // Show/Hide the Statistics Information
            g_stereoPlayer->toggleStatistics();
            break;
        break;
        case 'p':   // Hide/Auto/Show the Play Control
            g_stereoPlayer->togglePlayControl();
            break;
        break;
        case 27:    // [ESC] key to exit
            exit(0);
            break;
    }
}

// Handle special keyboard short-cuts
void special(int key, int, int)
{
    switch (key) {
        case GLUT_KEY_F1:   // Toggle the Full Screen mode
            g_fullscreen = !g_fullscreen;
            if (g_fullscreen) {
                glutFullScreen();
            } else {
                // Fit to the source size
                glutReshapeWindow(g_stereoPlayer->getPlayerWidth(), g_stereoPlayer->getPlayerHeight());
                glutPositionWindow(100, 100);
            }
            break;
    }
}

// Main routine
int main(int argc, char** argv)
{
    // Register the exit callback
    atexit(term);

    // Parse the command line arguments
    for (int i=1; i<argc; i++) {
        if (strcmp(argv[i], "-f") == 0)
            g_fullscreen = TRUE;
        else if (strcmp(argv[i], "-q") == 0)
            g_quadbuffer = TRUE;
        else {
            if (strcmp(leftFileName, "none")==0)
                StringCchCopy(leftFileName, _MAX_PATH, argv[i]);
            else if (strcmp(rightFileName, "none")==0)
                StringCchCopy(rightFileName, _MAX_PATH, argv[i]);
        }
    }

    // At least a left file name is needed
    if (strcmp(leftFileName, "none")==0) {
        fprintf(stderr, "\n"
                        "USAGE: glutStereoPlayer.exe [-q] [-f] <left source> [<right source>]\n\n"
                        "options: -q : Enable QuadBuffer stereo.\n"
                        "options: -f : Launch in the full screen mode.\n"
                        "example: glutStereoPlayer.exe -f left.avi right.avi\n"
                        "         glutStereoPlayer.exe left.bmp\n");
        return 0;
    }

    // Initialize glut
    glutInit(&argc, argv);
    if (!g_quadbuffer)
        glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_STENCIL);
    else
        glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_STENCIL | GLUT_STEREO);

    glutInitWindowSize(640, 480);
    glutInitWindowPosition(100, 100);
    glutCreateWindow("glutStereoPlayer");

    init();

    if (!g_stereoPlayer->loadLeftFile(leftFileName)) {
        fprintf(stderr, "ERROR: Can't open the left source file %s.\n", leftFileName);
        return 0;
    }
    if (!g_stereoPlayer->loadRightFile(rightFileName)) {
        fprintf(stderr, "ERROR: Can't open the right source file %s.\n", rightFileName);
    }
    if (!g_fullscreen)
        glutReshapeWindow(g_stereoPlayer->getPlayerWidth(), g_stereoPlayer->getPlayerHeight());
    else
        glutFullScreen();

    // Display the usages
    fprintf(stdout, "\n");
    fprintf(stdout, "Left source:  %s\n", leftFileName);
    fprintf(stdout, "Right source: %s\n", rightFileName);
    fprintf(stdout, "\n");
    fprintf(stdout, "USAGE: [Esc] Exit the program.\n");
    fprintf(stdout, "       [c]   Toggle Play/Pause.\n");
    fprintf(stdout, "       [x]   Stop.\n");
    fprintf(stdout, "       [c]   Rewind.\n");
    fprintf(stdout, "       [f]   Change the Source Format.\n");
    fprintf(stdout, "       [t]   Change the Stereo Rendering Type.\n");
    fprintf(stdout, "       [w]   Swap Left/Right side.\n");
    fprintf(stdout, "       [l]   Toggle the Loop mode.\n");
    fprintf(stdout, "       [s]   Show/Hide the Statistics Information.\n");
    fprintf(stdout, "       [p]   Hide/Auto/Show the Play Control.\n");
    fprintf(stdout, "       [F1]  Toggle the Full Screen mode.\n");

    // Register callbacks
    glutDisplayFunc(render);
    glutReshapeFunc(reshape);
    glutMouseFunc(mouseFunc);
    glutMotionFunc(mouseMotionFunc);
    glutPassiveMotionFunc(mousePassiveMotionFunc);
    glutKeyboardFunc(keyboard);
    glutSpecialFunc(special);
    glutIdleFunc(render);

    // Main loop
    glutMainLoop();

    return 0;
}
