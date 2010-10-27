//// =================================================================
/*!	\file GUIConsts.h
	
	Revision History:
	
	\par [8.8.2007]	16:22 by Sergey Zdanevich
	File created.
*/// ==================================================================

#ifndef __GUI_CONSTS_H__
#define __GUI_CONSTS_H__

//! GUI version
#define GUI_VERSION 0.69

/*! \mainpage
GUI
First of all look on the following chapters:
\li \ref highLevelPage General GUI concepts
\li \ref eventsPage GUI Event
*/

/*! \page highLevelPage General GUI concepts
GUI consists of 5 parts:
\li GUIController
\li GUISystem
\li GUIControl
\li GUISkin
\li GUILayout

High-level scheme of interaction:

\image html highlevel.jpg "High Level Architecture"

Now each component will be described in details.

\section sectionGUIController GUIController

All the events from an OS or a wrapper (Mobile FrameWork is a wrapper) are described by
GUIController, and transferred to GUISystem or to the specific focused control.
GUIController also places a focus depending on the user actions.

GUIController is a basic class from which we have to inherit in order to implement our individual controller for some needs.
For instance mouse controller or touchPad.

Controller can be added to GUISystem by AddController() function.

There is one controller implemented in GUI - GUIControllerPhoneKeyboard.
This controller implements the GUI control from the phone keyboard.

\section sectionGUISystem GUISystem

GUISystem is a link between all the GUI components.
There should be only one copy of GUISystem class.

With the help of GUISystem one can add a controller to the system, set a skin and a font by default,
create/destroy a window, send an event to some control or send a global event to all controls,
set a focus to some control.

GUISystem has 2 more important functions: Update() and Render(), they must be called at each frame.

Update() - calls Update() with all the system controllers and sends global event - eGlobalEvent::EGE_ON_UPDATE
Render() – defines the area needed for drawing, calls Draw() with all the controls, 

Calls for y focus drawing of the current skin, 

And calls Render() with all the controllers

\section sectionGUIControl   GUIControl

GUIControl is a basic GUI control element.
All the other control elements must be inherited from this class.

GUIControl represents a rectangle area which can receive events from the controller if it is in GUISystem focus.

Each control has states:
Focused or not, static or not, available or not, seen/unseen, pressed or not, selected or not.

Each control can have an event handler, and one can send an event to any control.

One can add an unlimited amount of child objects inherited from GUIControl to any control, which will be drawn in the area of its parent.

GUISkin can be installed to any control for its individual drawing.
GUILayout can be installed to any control for individual placement of its child objects.


\section sectionGUISkin GUISkin

GUISkin and its derivative classes can be installed to any GUIControl and GUISystem by default.

Skin deals with control drawing depending on its class types, drawing types and state.
Class type defines the type of data that can be possessed by a control and the way the data can be obtained.
Drawing type defines the style in which the control will be drawn.
For instance there can be a background text control which can be drawn in a common button style.

The skin also draws the control focus.


\section sectionGUILayout GUILayout
In order not to state coordinates for all the controls there is a GUILayout.
Layout places child objects of the control to which it is installed according to the law described in the Layout itself.

GUILayout is a basic class from which one should inherit in order to implement its specific placement scheme.

There are 2 Layouts of that kind implemented in GUI:

\li GUILayoutBox – places the kids in a horizontal or vertical line.
\li GUILayoutGrid – places the kids into a rectangle table.

Example of using GUI library with FrameWork

\code
TestApp::TestApp(Application * app)
{
	pApp = app;

	// Getting graphics system
	graphics = app->GetGraphicsSystem();

	// Creating Resource System which will be deleted then
	resourceSystem = app->CreateResourceSystem();

	// Opening the resource file, which should be closed then
	resourceSystem->Open("res.bar");

	// Creating the graphical font
	font = resourceSystem->CreateFont(IDB_FONT_MENU, IDB_CHARTABLE);

	// Creating main class of GUISystem
	// This class is a link between all the GUI components
	guiSystem = new GUISystem();

	// Creating a controller. In this case a phone keyboard controller is created
	controller = new GUIControllerPhoneKeyboard(guiSystem);

	// Creating skin. In this case a software skin is created which doesn’t use sprites while drawing.
	softSkin = new GUISkinSoftware();

	// Sets font to the skin
	softSkin->SetDefaultFont(font);

	// Setting default skin. This skin will be used for drawing controls for which the individual skin is not set.
	guiSystem->SetDefaultSkin(softSkin);

	// Adding a controller to the system. There can be several controllers and they will work simultaneously,
	// for instance, keyboard and mouse. Without a controller the system won’t receive notifications about user actions.
	guiSystem->AddController(controller);
}

void TestApp::Update()
{
	guiSystem->Update();
}

void TestApp::Render()
{
	guiSystem->Render();
}
\endcode
*/

//! GUI constants
enum eGUIConsts
{
	EGC_HANDLER_RESERVE = 5,
	EGC_PHONE_KEYBOARD_CONTROLLER_TEMP_LIST_RESERVE = 20,
	EGC_NUMERIC_TEXT_RESERVE = 32,
	EGC_MAX_CHAR_IN_DECIMAL_NUMBER = 10
};

#endif // __GUI_CONSTS_H__