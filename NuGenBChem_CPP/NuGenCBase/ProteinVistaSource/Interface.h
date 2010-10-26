#pragma once
#ifdef _DEBUG
#define new DEBUG_NEW
//#undef THIS_FILE
//static char THIS_FILE[] = __FILE__;
#endif
 
class CProteinVistaApp;
class CProteinVistaView;
class CProteinVistaRenderer;
////////////////////////////////////////////////////

CProteinVistaApp* GetMainApp();
CProteinVistaView* GetMainActiveView();
CProteinVistaRenderer * GetProteinVistaRenderer();
 
HWND GetMainHwnd();
void RemoveMainApp();
void MessagePump();
void PumpMessage();
 