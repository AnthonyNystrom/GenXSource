/*
** Include file for NewGUI
*/

// Enable new Toolbar support
#define USE_NEW_DOCK_BAR
// Enable new menubar support
#define USE_NEW_MENU_BAR

#include "WinAddon.h"
#include "EGDialogBar.h"
#include "EGStatusBar.h"
#include "NewTheme.h"
#include "EGToolbar.h"
#include "EGMenu.h"
#include "EGMenuApp.h"
#include "EGMenuBar.h"
#include "EGMDIClient.h"
#include "PaintManager.h"
//#include "EGHeaderControl.h"
//#include "EGSplitterWnd.h"
#include "Controls\EGTreeCtrl.h"
#include "Controls\EGTabCtrl.h"
#include "Controls\EGPageCtrl.h"
#include "Wizards\EGParamsDlg.h"
#include "Wizards\EGAppParamsPage.h"
#include "Wizards\EGProperty.h"
//#include "EGHyperLink.h"
//#include "EGPaneBar.h"
//#include "EGTaskBar.h"
//#include "EGListCtrl.h"
#include "Controls\EGButtonsBar.h"
#include "Controls\EGListCtrl.h"

#include "Toolbars\EGCtrlbar.h"
#include "Toolbars\EGTaskbar.h"

#include "PropertyGrid\EGPropertyGrid.h"

/* #pragma comment(lib, _EXGUI_LIB_FILE_ ) */

/* #pragma message(" ") */
/* #pragma message("*******************************************************") */
/* #pragma message("** Expert GUI Library. (c) 2005 Alexander E. Sorokin **") */
/* #pragma message("** Static linking...                                 **") */
/* #pragma message("** Features:                                         **") */
/* #pragma message("** - Custom themes support                           **") */
/* #pragma message("** - Themed header control                           **") */
/* #pragma message("** - Params dialog support                           **") */
/* #pragma message("** - Ehanced property page concept                   **") */
/* #pragma message("** - Font cracker                                    **") */
/* #pragma message("** - MDI tab manager                                 **") */
/* #pragma message("** - Controls Paint Manager                          **") */
/* #pragma message("** Controls                                          **") */
/* #pragma message("** - HyperLink                                       **") */
/* #pragma message("** - Custom draw tab                                 **") */
/* #pragma message("** - Outlook 2003 bar                                **") */
/* #pragma message("** - Pane bar                                        **") */
/* #pragma message("** - Task bar                                        **") */
/* #pragma message("** - ListCtrl                                        **") */
/* #pragma message("*******************************************************") */
/* #pragma message(" ") */
