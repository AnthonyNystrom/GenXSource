# 1 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss"
/*
============================================================================
 Name        : Aaf.rss
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application resource file
============================================================================
*/

//  RESOURCE IDENTIFIER
NAME HEWB    // 4 letter ID


//  INCLUDES
# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.rh" 1
# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.hrh" 1



# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\uikon.hrh" 1
// UIKON.HRH
//
// Copyright (c) Symbian Software Ltd 1997 - 2005. All rights reserved.
//




/** UID for dynamically-loaded Uikon library.
@publishedAll 
@released 
*/



/** Application resource file standard offsets.
@publishedAll 
@released 
*/
enum TEikAppResourceFileStandardOffset
	{
	/** Offset of RSS_SIGNATURE resource.
	*/
	ESignatureResourceOffset=1,
	/** Offset of NAME resource.
	*/
	EDefaultNameResourceOffset,
	/** Offset of EIK_APP_INFO resource.
	*/
	EAppInfoResourceOffset
	};


/** Default RSS_SIGNATURE signature field value.
@publishedAll 
@released 
*/



/** ID of "Cancel" command button.
@publishedAll 
@released 
*/


// standard commands

/** "Canceled" command ID
@publishedAll 
@released 
*/


/** "Exit" command ID.
@publishedAll 
@released 
*/



// EIKSPANE.HRH
//
// Copyright (c) Symbian Software Ltd 1997 - 2004. All rights reserved.
//

// Status pane server, pane declaration flags

/** STATUS_PANE flags resource flag: the pane is owned by the UI server.
@publishedAll 
@released 
*/


/** STATUS_PANE flags resource flag: the pane is owned by the client application.
@publishedAll 
@released 
*/



// Status pane server, top level layout flags

/** STATUS_PANE_LAYOUT position resource flag: lay out panes from the top of the status pane area.
@publishedAll 
@released 
*/


/** STATUS_PANE flags resource flag: lay out panes from the bottom of the status pane area.
@publishedAll 
@released 
*/


/** STATUS_PANE flags resource flag: lay out panes from the left of the status pane area.
@publishedAll 
@released 
*/


/** STATUS_PANE flags resource flag: lay out panes from the right of the status pane area.
@publishedAll 
@released 
*/



/** STATUS_PANE_LAYOUT position resource flag: lay out panes horizontally.
@publishedAll 
@released 
*/


/** STATUS_PANE_LAYOUT position resource flag: lay out panes vertically.
@publishedAll 
@released 
*/



// Status pane server, pane layout flags
/** STATUS_PANE flags resource flag: status pane is stretched to fill the available space.
@publishedAll 
@released 
*/



// Status pane app constants
/** STATUS_PANE_APP_MODEL layout default value: use default layout.
@publishedAll 
@released 
*/


// error resolver
/** SINGLE_ERROR flags resource flag: this error should be dealt with at the system level and indicates and unrecoverable error (which will result in a panic) if it reaches the UI.
@publishedAll 
@released 
*/


/** SINGLE_ERROR flags resource flag: this error should not be displayed.
@publishedAll 
@released 
*/


/** SINGLE_ERROR flags resource flag: this error is used for filling in empty slots in error ranges.
@publishedAll 
@released 
*/
// used for filling in empty slots in error ranges


// EIKDEF.HRH
//
// Copyright (c) Symbian Software Ltd 1997 - 2004. All rights reserved.
//








/** Shell application bitmap file.
@publishedAll 
@released 
*/


/** CP1252 ellipsis character value.
@publishedAll 
@released 
*/
// cp1252=133

/** CP1252 left single quote character value.
@publishedAll 
@released 
*/
// cp1252=145

/** CP1252 right single quote character value.
@publishedAll 
@released 
*/
// cp1252=146

/** CP1252 left double quote character value.
@publishedAll 
@released 
*/
// cp1252=147

/** CP1252 right double quote character value.
@publishedAll 
@released 
*/
// cp1252=148

/** CP1252 copyright character value.
@publishedAll 
@released 
*/


/** CP1252 soft hyphen character value.
@publishedAll 
@released 
*/


/** CP1252 hyphen character value.
@publishedAll 
@released 
*/


/** UID of Uikon plugin interface notifiers.
@internalComponent
*/



# 4 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.hrh" 2


# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\lafpublc.hrh" 1
/*
* ==============================================================================
*  Name        : lafpublc.hrh
*  Part of     : 
*  Description : 
*  Version     : 
*
*  Copyright © 2002-2005 Nokia. All rights reserved.
*  This material, including documentation and any related 
*  computer programs, is protected by copyright controlled by 
*  Nokia. All rights are reserved. Copying, including 
*  reproducing, storing, adapting or translating, any 
*  or all of this material requires the prior written consent of 
*  Nokia. This material also contains confidential 
*  information which may not be disclosed to others without the 
*  prior written consent of Nokia.
* ==============================================================================
*/




enum TLafReferenceButtonGroupControls
	{
	ELafBgCba=100
	};


# 6 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.hrh" 2


# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikcolor.hrh" 1
// EIKCOLOR.HRH
//
// Copyright (c) Symbian Software Ltd 1997 - 2004. All rights reserved.
//

/** RGB colour constant: black.
@publishedAll 
@released 
*/


/** RGB colour constant: dark gray.
@publishedAll 
@released 
*/


/** RGB colour constant: dark red.
@publishedAll 
@released 
*/


/** RGB colour constant: dark green.
@publishedAll 
@released 
*/


/** RGB colour constant: dark yellow.
@publishedAll 
@released 
*/


/** RGB colour constant: dark blue.
@publishedAll 
@released 
*/


/** RGB colour constant: dark magenta.
@publishedAll 
@released 
*/


/** RGB colour constant: dark cyan.
@publishedAll 
@released 
*/


/** RGB colour constant: red.
@publishedAll 
@released 
*/


/** RGB colour constant: green.
@publishedAll 
@released 
*/


/** RGB colour constant: yellow.
@publishedAll 
@released 
*/


/** RGB colour constant: blue.
@publishedAll 
@released 
*/


/** RGB colour constant: magenta.
@publishedAll 
@released 
*/


/** RGB colour constant: cyan.
@publishedAll 
@released 
*/


/** RGB colour constant: gray.
@publishedAll 
@released 
*/


/** RGB colour constant: dithered light gray.
@publishedAll 
@released 
*/


/** RGB colour constant: 1-in-4 dithered gray.
@publishedAll 
@released 
*/


/** RGB colour constant: white.
@publishedAll 
@released 
*/



/** RGB colour constant: Symbian green.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian light green.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian blue.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian dark blue.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian orange.
@publishedAll 
@released 
*/



/** Logical colour value for use in resources: The colour of a window's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in a window. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the background contents of a control. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text inside a control. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The background colour of a control's border. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in a control's border. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a control's background when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a control when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a control's background when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a control when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a control's background when dimmed and highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a control when dimmed and highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a dialog's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text inside a dialog. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: A dialog title's background colour. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: A dialog title's background colour when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The text colour in a dialog's title. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The text colour in a dialog's title when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menubar's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menubar. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menubar title's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menubar title. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menupane's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menu pane background when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menu pane background when dimmed and highlighted. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane when dimmed and highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background, when the button is in the unset state. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background when the button is in the set state. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background when the button is in the set state and is pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background when the button is in the unset state and is pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button text. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button text when the button is pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button text when the button is dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the message window foreground. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the message window background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar border. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar shaft background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar shaft background when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar shaft background when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the background for scroll bars with no thumb and no shaft. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of scroll bar buttons. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of scroll bar buttons when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of scroll bar buttons when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's background when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's background when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's edge. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a toolbar's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a toolbar. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a status pane's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a status pane. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a label. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of emphasised text in a label. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a label when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the label background when highlighted and dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the label background when highlighted. 
@publishedAll 
@released 
*/

# 8 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.hrh" 2



// EIKBTGPC.HRH





// These are for Enhanced Cba





// EIKBUTB.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//




// EIKCHLST.HRH

// EIKEDWIN.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

// Introduced in avkon temporarily and deprecated:


// CEikEdwin Flags ('flags' in EDWIN resource)





























// AVKON EDWIN CONSTANTS

// Number input mode keymappings for * and # keys ('numeric_keymap' in EDWIN resource)
// If only the number mode is permitted in an editor, special characters can be
// entered by pressing # key or multitapping '*' key.
// If also text input mode is available in the editor, special characters entered
// with '*' key are shown inside special character table pop-up.
// - Default keymapping for editors that allows both number and 
// text input modes: EAknEditorAlphanumericNumberModeKeymap.
// - Default keymapping for editors that allows only number input 
// mode: EAknEditorStandardNumberModeKeymap.
//
// EAknEditorAlphanumericNumberModeKeymap value is not valid for 
// number input mode only editors. Use avkon_flags 
// EAknEditorFlagUseSCTNumericCharmap flag instead.
enum TAknEditorNumericKeymap
    {
                                            // Available characters:
    EAknEditorStandardNumberModeKeymap,     // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, *, +, p, w, #
    EAknEditorPlainNumberModeKeymap,        // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0
    EAknEditorCalculatorNumberModeKeymap,   // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, +, –, *, /, <decimal separator>
    EAknEditorConverterNumberModeKeymap,    // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, +, –, E, <decimal separator>
    EAknEditorToFieldNumberModeKeymap,      // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, +, ;
    EAknEditorFixedDiallingNumberModeKeymap,// 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, *, +, p, w, ?, #
    EAknEditorSATNumberModeKeymap,          // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, *, +, #
    EAknEditorSATHiddenNumberModeKeymap,    // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, *, #
    EAknEditorAlphanumericNumberModeKeymap, // 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, #, <all alpha mode special characters>
    EAknEditorReservedInternalUseKeymap = 100   // do not use - for internal use only 
    };

// Case mode flags for input languages that supports case.
// 'default_case' and 'allowed_case_modes' in EDWIN resource.





// Input mode flags for editors.
// 'default_input_mode' and 'allowed_input_modes' in EDWIN resource.

// All text input modes that are available in current language.



// Japanese input modes - only effective in Japanese variant.
// half-width Katakana
// full-width latin alphabet
// full-width numeric (0-9)
// full-width Katakana
// Hiragana/Kanji
// only Hiragana
// half-width Latin alphabet



// Following two are deprecated


//

// Avkon editor flags ('avkon_flags' in EDWIN resource).
// -----------------------------------------------------

// case changes are not allowed
// predictive text entry is not allowed
// editor indicators are not shown
// the cursor cannot be moved
// edit menu cannot be opened from edit key

// The character next to the cursor (if exists) is replaced by a new entered character.
// This flag has effect only in latin multitap input.

// The number input mode uses same special character table character set
// as alpha input mode if the flag is set. The flag overrides
// number mode keymapping if they are in conflict. This flag can be used
// also with number input mode only editors.

// Input language is changed to English locally in the editor.

// Chinese find mode input.

// Line feed character is added with scroll down key event 
// if the cursor is at the end of the buffer. The functionality is 
// available only in certain variants.



// NOTICE! Flag value 0x80000000 is reserved for internal use by FEP.

// EIKLABEL.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//











enum EEikLabelStandardFont
    {
    EEikLabelFontNormal,
    EEikLabelFontLegend,
    EEikLabelFontAnnotation
    };
    
// EIKLBBUT.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//





// EIKLBX.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//









// EIKMENU.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//










// more flags to determine the type of scroll bar to be used
// deprecated in Uikon 1.2



// remaining flags only used internally





//The following value is reserved       0x020000
//(Added by GT)

// EIKTBAR.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

// Toolbar flags






// Toolbar control flags




// standard toolbat sizes













// EIKCMBUT.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//





















//to be removed ...  (Comment added by GT)





// EIKLBBUT.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//




// EIKDEF.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//













// Built-in app UIDs





































// EIKCMDS.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//






































































// EIKCTRLS.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

enum TEikStockControls
    {
    EEikCtCommandButton,
    EEikCtLabeledButton,
    EEikCtEdwin,
    EEikCtRichTextEditor,
    EEikCtSecretEd,
    EEikCtCheckBox,
    EEikCtChoiceList,
    EEikCtOptionButton,
    EEikCtHorOptionButList,
    EEikCtFileNameSel,
    EEikCtFileNameEd,
    EEikCtFolderNameSel,
    EEikCtFolderNameEd,
    EEikCtDriveNameSel,
    EEikCtListBox,
    EEikCtCharMap,
    EEikCtCharEd,
    EEikCtImage,
    EEikCtImageArray,
    EEikCtLabel,
    EEikCtComboBox,
    EEikCtProgInfo,
    EEikCtFlPtEd,
    EEikCtFxPtEd,
    EEikCtTwipsEd,
    EEikCtNumberEditor,
    EEikCtRangeEditor,
    EEikCtTimeEditor,
    EEikCtDateEditor,
    EEikCtTimeAndDateEditor,
    EEikCtDurationEditor,
    EEikCtTimeOffsetEditor,
    EEikCtLatitudeEditor_Not_Supported,
    EEikCtLongitudeEditor_Not_Supported,
    EEikCtBorderPreview,
    EEikCtFontPreview,
    EEikCtGraySelector,
    EEikCtWorldSelector,
    EEikCtGlobalTextEditor,
    EEikCtSoundSelector,
    EEikCtClock,
    EEikCtPrintPreview,
    EEikCtMenuButton,
    EEikCtDirContentsListBox,
    EAknCtIpFieldEditor,
    EEikCtCalendar,
    EEikCtSpacer,
    EEikCtColListBox,
    EEikCtRangeRefEditor,
    EEikCtTextButton,
    EEikCtBitmapButton,
    EEikCtDialogToolBar,
    EEikCtFileBrowser,
    EEikCtFileNameLabel,
    EEikCtUnifiedFileSel,
    EEikCtTelephoneNumberEditor,
    EEikCtTwoPictureCommandButton,
    EEikCtInverterCommandButton,
    EEikCtScrollBar,
    EEikCtArrowHeadScrollBar
    };

// not available before Release 6.1
enum TEikStockButtonGroupControls
    {
    EEikBgToolBar,
    EEikBgButtonPanel,
    EEikBgCba
    };

// EIKPRIV.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//












// > EEikBidDeferValidateFocusControl


































































//preview dialog







// setup menubutton






//printer setup






//serial port setup











//printrange setup







//print progress
                                     



//previewrange setup




//page setup














































































































// EIKDIALG.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
 
















// eikdialg.cpp seems to define 0x10000, 0x20000, 0x40000

// eikdialg.cpp seems to define 0x100000






// EEikDlgItemSeparatorAfter has been deprecated.































// AKNLAF
// (Added by GT)

// default





// EIKSPANE.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

// Status pane server, pane declaration flags




// Status pane server, top level layout flags









// Status pane server, pane layout flags



// Status pane app constants



// Redefine the base-defined keys to Series 60 specific names.
// These key events are reserved for Series 60 Platform.









// Voice key
















// Following key events are reserved for internal use by Series 60 Platform:
// EKeyF19, EKeyF20, EKeyF21, EKeyF22, EKeyF23, EKeyF24,
// EKeyApplication9, EKeyApplicationA, EKeyApplicationB, EKeyApplicationC
// EKeyApplicationD, EKeyApplicationE, EKeyApplicationF
// EKeyDeviceF for resolution change and Qwerty input mode in emulator.

// Scancodes (TStdScanCode) 0xE0 - 0xF7 and key codes (TKeyCode) 0xF880 - 0xF897 
// are reserved for product specific use. The meaning of the event can vary 
// in different products.

// Series 60 platform reserves all EKeyDeviceX and EKeyApplicationX events for
// future use.




// EIKCFDLG.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
 












// EIKDIAL.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//





enum
	{
	EDialDlgButFreeDial=1,
	EDialDlgButDial,
	EDialDlgButDialOut,
	EFreeDialDlgButClear,
	EFreeDialDlgButRedial,
	EDialDlgEdwin1,
	EDialDlgEdwin2,
	EDialDlgEdwin3,
	EDialDlgEdwin4,
	EDialDlgEdwin5,
	EFreeDialDlgEdwin
	};

// EIKFBROW.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#define EEikFileBrowseHideRom				0x0001
//#define EEikFileBrowseShowSystem			0x0020
//#define EEikFileBrowseShowHidden			0x0040
//#define EEikFileBrowseAllowFolderCreation	0x0200

// EIKTIME.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//




/*
#define KEikTimeCityCountryNameWidth	15

#define EEikTimeCidSetCityCity			0x1901
#define EEikTimeCidSetCityCountry		0x1902
#define EEikTimeCidSetTimeTime			0x1911
#define EEikTimeCidSetTimeDate			0x1912
#define EEikTimeCidDstHome				0x1971
#define EEikTimeCidDstEuropean			0x1972
#define EEikTimeCidDstNorthern			0x1973
#define EEikTimeCidDstSouthern			0x1974
#define EEikTimeCidTimeFormat			0x1802
#define EEikTimeCidTimeSeparator		0x1803
#define EEikTimeCidDateFormat			0x1813
#define EEikTimeCidDateSeparator		0x1814
#define EEikTimeCidWorkayMonday			0x1770
#define EEikTimeCidWorkayTuesday		0x1771
#define EEikTimeCidWorkayWednesday		0x1772
#define EEikTimeCidWorkayThursday		0x1773
#define EEikTimeCidWorkayFriday			0x1774
#define EEikTimeCidWorkaySaturday		0x1775
#define EEikTimeCidWorkaySunday			0x1776
#define EEikTimeCidDialogDescription	0x1961
*/

// EIKCHLST.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
/*
#define EEikChlistArrayOwnedExternally	0x01
#define EEikChlistIncrementalMatching	0x02
#define EEikChlistNoPopout				0x04
*/
// EIKCAL.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
/*
#define EEikCalendarFlagStartMonthIsAlwaysJanuary	0x01
#define EEikCalendarFlagNavigateChronologically		0x02
*/
// EIKCLOCK.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
/* by H.Domon,K3 19 Apr 2001
enum
	{
	EEikLocaleConformantClockTypeChangesFormatAccordingToLocale,
	EEikLocaleConformantClockTypeDigital,
	EEikLocaleConformantClockTypeAnalog,
	EEikUsesDefaultSystemColors = 0x04
	};

enum
	{
	EEikResourceConstructedClockDigitalDisplayHorizontalTextAlignmentLeft,
	EEikResourceConstructedClockDigitalDisplayHorizontalTextAlignmentCenter,
	EEikResourceConstructedClockDigitalDisplayHorizontalTextAlignmentRight
	};

enum
	{
	EEikResourceConstructedClockDigitalDisplayVerticalTextAlignmentTop,
	EEikResourceConstructedClockDigitalDisplayVerticalTextAlignmentCenterInclDescent,
	EEikResourceConstructedClockDigitalDisplayVerticalTextAlignmentCenterExclDescent,
	EEikResourceConstructedClockDigitalDisplayVerticalTextAlignmentBottomInclDescent,
	EEikResourceConstructedClockDigitalDisplayVerticalTextAlignmentBottomExclDescent
	};

enum
	{
	EEikResourceConstructedClockAnalogDisplayHandOneRevPer12Hours,
	EEikResourceConstructedClockAnalogDisplayHandOneRevPerHour,
	EEikResourceConstructedClockAnalogDisplayHandOneRevPerMinute
	};

enum
	{
	EEikResourceConstructedClockAnalogDisplayHandFeatureLine,
	EEikResourceConstructedClockAnalogDisplayHandFeaturePolyLine,
	EEikResourceConstructedClockAnalogDisplayHandFeatureCircle
	};
by H.Domon,K3 19 Apr 2001
*/
// EIKGYSEL.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
/*
#define EEikGraySelIncrementalMatching		0x0001
#define EEikGraySelDisplay4Gray				0x0002
#define EEikGraySelDisplay16Gray			0x0004
#define EEikGraySelDisplayNoText			0x0010
#define EEikGraySelDisplay16Color			0x0020
#define EEikGraySelColorIfAvailable			0x1000
*/
// EIKMFNE.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//




//


// default is that the direction is a suffix
//
// The values of the following must be reflected in CTimeEditor in eikmfne.cpp
//







// EIKPROGI.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


enum TEikProgressTextType
	{
	EEikProgressTextNone,
	EEikProgressTextPercentage,
	EEikProgressTextFraction
	};

// EIKRTED.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//






// EIKSPACE.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//









/*
// EIKWSEL.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

#define EEikWorldSelNoBorder				0x0001
#define EEikWorldSelSecondSelector			0x0008
#define EEikWorldSelCountrySelector			0x1000
*/

// EIKCFDLG.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
/* 
#define EEikCidFileNameSel			10
#define EEikCidFileNameEd			20 
#define EEikCidFolderNameSel		30
#define EEikCidFolderNameEd			40
#define EEikCidDriveNameSel			50

#define EEikCidUseNewFileChbx		100

#define EEikBidCreateFile			150

#define EEikBidBrowse				200
*/
// EIKFBROW.HRH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//
/*
#define EEikFileBrowseHideRom				0x0001
#define EEikFileBrowseShowSystem			0x0020
#define EEikFileBrowseShowHidden			0x0040
#define EEikFileBrowseAllowFolderCreation	0x0200
*/


enum
	{
	EEikCidImageType=1
	};

// Used to inform current foreground window group of popup windows that require it
// to fade.
enum
	{
	EEikPopupOnTop = 1100,
	EEikPopupGone
	};

// Used to tell SysAp that keylock is enabled or disabled
enum
	{
	EEikKeyLockEnabled = 1110,
	EEikKeyLockDisabled = 1111,
    EEikLeyLockDisabled = 1111,  // TODO: This should be removed!!!
	EEikKeyLockPowerKeyPressed
	};

//
// EIKHFDLG
//
// Constants used in resource for heap fail dialog
//

enum
	{
	EEikHeapFailSettingListId = 999
	};


// COmmand operationtypes for enhanced cba
enum TEikCommandOperationTypes
	{
	EEikCommandTypeAnyCommand,
	EEikCommandTypeMenu,
	EEikCommandTypeOpenItem,
	EEikCommandTypeDeleteItem,
	EEikCommandTypeOperatesItem,
	EEikCommandTypeCreateItem,
	EEikCommandTypePositiveAnswer,
	EEikCommandTypeNegativeAnswer,
	EEikCommandTypeCancel,
	EEikCommandTypeExitOrBack,
	EEikCommandTypeExitApplication
	};

/**
* Series 60 Button flags
*
* @since Series 60 3.1
*/
// for Text&Icon button, by default text is on the right side

// MinimumSize() will return the width to fit the text

// no frame is drawn for the button, useful for icon buttons 

// Reports events if the button is pressed for a long time.

// by default event is reported on key up

// When button has frame frame won't be changed on press event

// Button requests exit on button up event


// avkon button state flags
// state has latchable frame

	
// ======== avkon toolbar flags ===============

// position of the toolbar can be changed with SetPosition() before
// toolbar is shown, by default it is fixed


// Middle item will be focused, by default focus is on the first item




# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.rh" 2


# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\uikon.rh" 1
// UIKON.RH
//
// Copyright (c) Symbian Software Ltd 1997 - 2005. All rights reserved.
//





# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\BADEF.RH" 1
// BADEF.RH
//
// Copyright (c) 1997-2000 Symbian Ltd.  All rights reserved.
//




STRUCT BA_RSS_SIGNATURE
    {
    LONG signature=0; // this value must be changed if the resource file is loaded by an environment
    SRLINK self;
    }

STRUCT TBUF
    {
    BUF buf; // non-zero terminated text string
    }

STRUCT LBUF
    {
    LTEXT txt; // leading-byte counted text string
    }

STRUCT ARRAY
    {
    STRUCT items[];
    }

STRUCT UID_NAME_PAIR
	{
	LONG uid;
	STRUCT name; // an LBUF
	}



# 10 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\uikon.rh" 2








# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\BAERRRSVR.RH" 1
// BAERRRSVR.RH
//
// Copyright (c) 2003 Symbian Ltd.  All rights reserved.
//





// Error Resolver

STRUCT ERROR_SET
	{
	STRUCT errorarrays[];
	}

STRUCT ERROR_ARRAY
	{
	LONG startError;
	LLINK errors;
	}

STRUCT SINGLE_ERROR
	{
	BYTE flags=0;
	LLINK text=0;
	}


# 18 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\uikon.rh" 2




STRUCT RESOURCE_LINK
	{
	LLINK id;
	}

STRUCT HOTKEY
    {
    LONG command;
    LONG key;
    }

STRUCT HOTKEYS
    {
    STRUCT plain[];
    STRUCT control[];
    STRUCT shift_control[];
    }

// Removed by Bill
//#include <eikfont.rh> // for STRUCT FONT

STRUCT NUMBER_INT8
	{
	BYTE value=0;
	}

STRUCT NUMBER_INT16
	{
	WORD value=0;
	}

STRUCT NUMBER_INT32
	{
	LONG value=0;
	}

// EIKCOLOR.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT COLOR
	{
	BYTE red;
	BYTE green;
	BYTE blue;
	}

STRUCT CTRL_COLOR
	{
	WORD logical_color;
	STRUCT color; // COLOR
	}

STRUCT COLORLIST
	{
	LLINK array_id=0;
	}

STRUCT EIKCOLORLIST
	{
	LLINK array_id=0;
	LLINK ext_id;
	}

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikcolor.hrh" 1
// EIKCOLOR.HRH
//
// Copyright (c) Symbian Software Ltd 1997 - 2004. All rights reserved.
//

/** RGB colour constant: black.
@publishedAll 
@released 
*/


/** RGB colour constant: dark gray.
@publishedAll 
@released 
*/


/** RGB colour constant: dark red.
@publishedAll 
@released 
*/


/** RGB colour constant: dark green.
@publishedAll 
@released 
*/


/** RGB colour constant: dark yellow.
@publishedAll 
@released 
*/


/** RGB colour constant: dark blue.
@publishedAll 
@released 
*/


/** RGB colour constant: dark magenta.
@publishedAll 
@released 
*/


/** RGB colour constant: dark cyan.
@publishedAll 
@released 
*/


/** RGB colour constant: red.
@publishedAll 
@released 
*/


/** RGB colour constant: green.
@publishedAll 
@released 
*/


/** RGB colour constant: yellow.
@publishedAll 
@released 
*/


/** RGB colour constant: blue.
@publishedAll 
@released 
*/


/** RGB colour constant: magenta.
@publishedAll 
@released 
*/


/** RGB colour constant: cyan.
@publishedAll 
@released 
*/


/** RGB colour constant: gray.
@publishedAll 
@released 
*/


/** RGB colour constant: dithered light gray.
@publishedAll 
@released 
*/


/** RGB colour constant: 1-in-4 dithered gray.
@publishedAll 
@released 
*/


/** RGB colour constant: white.
@publishedAll 
@released 
*/



/** RGB colour constant: Symbian green.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian light green.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian blue.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian dark blue.
@publishedAll 
@released 
*/


/** RGB colour constant: Symbian orange.
@publishedAll 
@released 
*/



/** Logical colour value for use in resources: The colour of a window's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in a window. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the background contents of a control. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text inside a control. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The background colour of a control's border. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in a control's border. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a control's background when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a control when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a control's background when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a control when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a control's background when dimmed and highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a control when dimmed and highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a dialog's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text inside a dialog. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: A dialog title's background colour. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: A dialog title's background colour when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The text colour in a dialog's title. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The text colour in a dialog's title when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menubar's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menubar. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menubar title's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menubar title. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menupane's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menu pane background when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane when highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the menu pane background when dimmed and highlighted. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of text in the menu pane when dimmed and highlighted for selection. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background, when the button is in the unset state. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background when the button is in the set state. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background when the button is in the set state and is pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button background when the button is in the unset state and is pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button text. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button text when the button is pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the button text when the button is dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the message window foreground. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the message window background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar border. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar shaft background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar shaft background when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the scroll bar shaft background when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the background for scroll bars with no thumb and no shaft. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of scroll bar buttons. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of scroll bar buttons when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of scroll bar buttons when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's background when pressed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's background when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a scroll bar thumb's edge. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a toolbar's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a toolbar. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of a status pane's background. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a status pane. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a label. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of emphasised text in a label. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the text in a label when dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the label background when highlighted and dimmed. 
@publishedAll 
@released 
*/


/** Logical colour value for use in resources: The colour of the label background when highlighted. 
@publishedAll 
@released 
*/

# 87 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\uikon.rh" 2


// EIKFONT.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#include <eikfont.hrh>

STRUCT FONT
	{
	LONG fontid; // a value from fontids.h
	WORD flags=0; // a combination of EGulFontFlagXxxxs
	}

STRUCT NAMED_FONT
	{
	LTEXT name;
	WORD height;	// in twips
	WORD flags=0; // a combination of EGulFontFlagXxxxs
	}

// EIKPRIV.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT INT16
	{
	WORD value;
	}

// EIKPRTPV.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

// EIKDEF.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT RSS_SIGNATURE
    {
    LONG signature= 4 ;
    SRLINK self;
    }

STRUCT TBUF1	{ BUF<1> buf; }
STRUCT TBUF2	{ BUF<2> buf; }
STRUCT TBUF4	{ BUF<4> buf; }
STRUCT TBUF8	{ BUF<8> buf; }
STRUCT TBUF9	{ BUF<9> buf; }
STRUCT TBUF16	{ BUF<16> buf; }
STRUCT TBUF32	{ BUF<32> buf; }
STRUCT TBUF40	{ BUF<40> buf; }
STRUCT TBUF64	{ BUF<64> buf; }
STRUCT TBUF80	{ BUF<80> buf; }
STRUCT TBUF128	{ BUF<128> buf; }
STRUCT TBUF256	{ BUF<256> buf; }

STRUCT EIK_APP_INFO
    {
    LLINK hotkeys=0;
    LLINK menubar=0;
    LLINK toolbar=0;
    LLINK toolband=0;
    LLINK cba=0;	
    LLINK status_pane=0;
    LLINK custom_app_info_extension=0;	
    }

STRUCT PAGESIZE
	{
	LONG width=0;
	LONG height=0;
	}

STRUCT VIEW_ID
	{
	LONG appuid;
	LONG viewuid;
	}

// EIKSPANE.RH
//
// Copyright (c) 1999 - 2003 Symbian Ltd.  All rights reserved.
//


STRUCT STATUS_PANE_SYSTEM_MODEL				// System wide status pane declaration
	{
	STRUCT panes[];							// entire set of valid SPANE_PANEs
	LLINK default_layout;					// default STATUS_PANE_LAYOUT
	LLINK layouts[];						// entire set of STATUS_PANE_LAYOUTs
	}
 
STRUCT SPANE_PANE
	{
	LONG id;
	WORD flags= 0 ;
	WORD type;
	LLINK resource=0;
	}

STRUCT STATUS_PANE_LAYOUT
	{
	WORD position;
	STRUCT pane_tree;						// SPANE_LAYOUT_TREE_NODE root of the layout tree
	}

STRUCT SPANE_LAYOUT_TREE_NODE
	{
	LONG id=0;
	WORD flags;
	WORD size;
	STRUCT sub_panes[];						// SPANE_LAYOUT_TREE_NODE sub panes
	}

STRUCT STATUS_PANE_APP_MODEL				// Application specific status pane
	{
	LLINK layout= 0 ;		// STATUS_PANE_LAYOUT to use for this app
	STRUCT panes[];							// SPANE_PANE overrides to use for this app
	}

// Notifiers

STRUCT NOTIFIER_LIST
	{
	LONG dll_uid;
	
	LEN WORD STRUCT notifier_list[]; // notifier data
	}

STRUCT NOTIFIER_DATA
	{
	LONG notifier_uid;
	}


# 3 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.rh" 2











STRUCT BTG_RESOURCE_COLLECTION
    {
    STRUCT btg_sets[];
    STRUCT btg_buttons[];
    }

// EIKEDWIN.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


# 36 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.rh"

STRUCT EDWIN
    {
    LONG flags=0;
    WORD width=0;
    WORD lines=1;
    WORD maxlength=0;
	WORD default_case= 0x4 ;	WORD allowed_case_modes= 0x1  | 0x2  | 0x4  ;	WORD numeric_keymap=EAknEditorStandardNumberModeKeymap;	WORD allowed_input_modes= 0x1  | 0x2  | 0x4  | 0x8  | 0x10  | 0x20  | 0x40  | 0x80  | 0x200  ;	WORD default_input_mode= 0x1 ;	LLINK special_character_table=-1;	WORD avkon_flags= 0x000 ;	WORD max_view_height_in_lines=0;	WORD base_line_delta=0;	WORD spare=0; 
    }

// EIKIMAGE.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


STRUCT IMAGE
	{
	WORD horiz_align= 0x01 ;
	WORD vert_align= 0x00 ;
	LTEXT bmpfile="";
    WORD bmpid;
    WORD bmpmask=0xffff;
	LLINK extension=0;
	}

// EIKLABEL.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT LABEL
    {
	WORD flags = 0;
	WORD horiz_align= 0x01 ;
	WORD vert_align= 0x00 ;
	WORD standard_font=EEikLabelFontNormal;
	WORD reserve_length=0;
    LTEXT txt;
    }

// EIKLBX.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT LISTBOX
	{
	BYTE version=0;
	LONG flags = 0;
	WORD height = 5;		// in items
	WORD width = 10;		// in chars
    LLINK array_id = 0;
	}


// EIKMENU.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT MENU_PANE
    {
    STRUCT items[]; // MENU_ITEMs
	LLINK extension=0;
    }

STRUCT MENU_ITEM
    {
    LONG command=0;
    LLINK cascade=0;
    LONG flags=0;
    LTEXT txt;
	LTEXT extratxt="";
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
	LLINK extension=0;
    }

STRUCT MENU_BAR
    {
    STRUCT titles[]; // MENU_BAR_ITEMs
	LLINK extension=0;
    }

STRUCT MENU_TITLE
    {
    LLINK menu_pane;
    LTEXT txt;
    LONG flags=0;
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
	LLINK extension=0;
    }


// EIKMNBUT.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#include <eikcmbut.hrh>

STRUCT MNBUT
    {
	LLINK popout=0;
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0; // ETextRightPictureLeft
	LTEXT helptxt="";
	LLINK extension=0;
    LTEXT txt="";
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
    }

// EIKTBAR.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


STRUCT TOOLBAR
    {
	WORD length=0;
	WORD breadth= 70 ;
	BYTE lines=1;
    LONG flags=0;
    STRUCT controls[];
	LLINK extension=0;
    }

STRUCT TOOLBAND
    {
	WORD length=0;
	WORD breadth= 25 ;
	BYTE lines=1;
    LONG flags= 0x01 ;
    STRUCT controls[];
	LLINK extension=0;
    }

STRUCT TBAR_CTRL
	{
	WORD type;
	WORD id=0;
	WORD flags=0;
	WORD length=0;
	STRUCT control;
	}

STRUCT TBAR_BUTTON
	{
	WORD type=0; // EEikCtCommandButton;
	WORD id=0;
	WORD flags=0;
	WORD length=0;
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0; // ETextRightPictureLeft|EEikCmdButShareExcess
	LTEXT helptxt="";
	LLINK extension=0;
    LTEXT txt="";
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
	}

STRUCT TBAR_PICBUT
	{			
	WORD type=EEikCtTwoPictureCommandButton;
	WORD id=0;
	WORD flags=0;
	WORD length=0;
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0; // ETextRightPictureLeft|EEikCmdButShareExcess
	LTEXT helptxt="";
	LLINK extension=0;
    LTEXT txt="";
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
    WORD bmpsecondid=0xffff;
    WORD bmpsecondmask=0xffff;
	}

// EIKCMBUT.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


STRUCT CMBUT
    {
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0; // ETextRightPictureLeft|EEikCmdButShareExcess
	LTEXT helptxt="";
	LLINK extension=0;
	LTEXT txt="";
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
    }

STRUCT PICMBUT
    {
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0;	// ETextRightPictureLeft|EEikCmdButShareExcess
	LTEXT helptxt="";
	LLINK extension=0;
	LTEXT txt="";
	LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
    WORD bmpsecondid=0xffff;
    WORD bmpsecondmask=0xffff;
    }

STRUCT TXTBUT
    {
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0; // EFirstRightSecondLeft|EEikCmdButShareExcess
	LTEXT helptxt="";
	LLINK extension=0;
    LTEXT txt1="";
    LTEXT txt2="";
    }

STRUCT BMPBUT
    {
	BYTE version=0;
	WORD behavior=0;
	WORD layout=0; // ETextRightPictureLeft|EEikCmdButShareExcess
	LTEXT helptxt="";
	LLINK extension=0;
	LTEXT bmp1file="";
    WORD bmp1id=0xffff;
    WORD bmp1mask;
	LTEXT bmp2file="";
    WORD bmp2id=0xffff;
    WORD bmp2mask;
    }


// EIKLBBUT.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


STRUCT LBBUT
    {
	WORD buttontype=0; // EEikCtCommandButton
	STRUCT button;
	LONG hotkey;
	BYTE flags=1; // EEikLabeledButtonShowHotKey
    }

STRUCT CONTROLS
	{
	LEN BYTE STRUCT controls[];
	}

// EIKDIALG.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

// only buttons defined in Uikon

STRUCT DLG_BUTTONS
    {
	LONG flags= 0x01 ;
    STRUCT buttons[];
    }

STRUCT DLG_BUTTON
    {
	WORD id=0;
	WORD buttontype=0; // EEikCtCommandButton
	STRUCT button;
	LONG hotkey;
	BYTE flags=1;
	BYTE line=1;
    }


// EIKCBA.RH
//
// Copyright (c) 1998 Symbian Ltd.  All rights reserved.
//

STRUCT CBA
	{
	LONG flags= 0x01 ;
	WORD breadth=80; // !!! make this accessible from LAF
	LLINK related_buttons=0;
   	STRUCT buttons[];
	}

STRUCT CBA_BUTTON
	{
	BYTE version=0;
	WORD id=0;
   	LTEXT txt="";
	LTEXT bmpfile="";
  	WORD bmpid=0xffff;		// -1
    WORD bmpmask=0xffff;	// -1
	}


// EIKDIALG.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

// buttons are defined in Uikon

STRUCT DIALOG
    {
    LONG flags=0;
    LTEXT title="";
	LLINK pages=0;
    LLINK buttons=0;
    STRUCT items[];
	LLINK form=0;
    }

STRUCT DLG_LINE
    {
    WORD type;
    LTEXT prompt;
    WORD id=0;
	LONG itemflags=0;
    STRUCT control;
	LTEXT trailer="";
	LTEXT bmpfile = "" ;   // NEW AVKON bitmap filename 
	WORD bmpid = 0xffff ; // NEW AVKON icon image ID
	WORD bmpmask ;		// NEW AVKON icon mask ID
	LTEXT tooltip = "" ;	// NEW AVKON hint text for form line
    }

STRUCT DLG_LINE_INDIRECT
    {
    WORD type= -1 ;
	LLINK line;
    }

STRUCT PAGE
	{
	WORD id=0;
	LTEXT text;
	LTEXT bmpfile = "" ;   // NEW AVKON bitmap filename 
	WORD bmpid = 0xffff ; // NEW AVKON icon image ID
	WORD bmpmask ;		// NEW AVKON icon mask ID
	LLINK lines=0;
	LLINK form=0 ;	
	WORD flags=0 ;	
	}

STRUCT DIALOG_INDIRECT
    {
    LONG flags=0;
    LTEXT title;
	LLINK pages=0;
    LLINK buttons=0;
    WORD count= -1 ;
	LLINK main=0;
    }

STRUCT FORM
	{
	WORD flags=0 ;
	STRUCT items[] ;
	}

// EIKDTLBX.RH
//
// Copyright (c) 1998 Symbian Ltd.  All rights reserved.
//

STRUCT DTLBOX
	{
	WORD flags=0;
	WORD modelflags=0;
	WORD height=5;	// in items
	WORD width=0;	// in pixels
	}

// EIKIMGAR.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT IMAGE_ARRAY
	{
	WORD width=0;
	LLINK array_id=0;
	}

// EIKCAL.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//
// calendar STRUCT
//

STRUCT CALENDAR
	{
	WORD flags=0; // a combination of EEikCalendarFlagXxxxxs
	BYTE nummonths;
	BYTE nummonthsperrowofmonths;
	WORD verticalspaceaboveyear;
	WORD verticalspacebetweenyearandtoprowofmonths;
	WORD verticalspacebetweeneachrowofmonths;
	WORD verticalspacebeneathbottomrowofmonths;
	WORD horizontalspaceoneachsideofmonthmatrix;
	WORD horizontalspacebetweeneachcolumnofmonths;
	WORD verticalspacebetweenmonthnameanddaynameinitials;
	WORD verticalspacebetweendaynameinitialsandtoprowofdays;
	WORD verticalspacebetweeneachrowofdays;
	WORD horizontalspacebetweeneachcolumnofdays;
	STRUCT yearfont; // FONT
	STRUCT monthfont;
	STRUCT weekdayinitialfont;
	STRUCT numeralfont;
	LTEXT textseparatingyearrange;
	}


// EIKCLOCK.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//
// primitive STRUCTs
//

STRUCT POINT
	{
	WORD x;
	WORD y;
	}
/* by H.Domon,K3 19 Apr 2001
STRUCT CLOCK_SHADOW
	{
	BYTE donotchange=1;
	STRUCT color; // COLOR
	BYTE offsetX;
	BYTE offsetY;
	}

STRUCT NO_CLOCK_SHADOW
	{
	BYTE donotchange=0;
	}
by H.Domon,K3 19 Apr 2001
*/
STRUCT CLOCK_PEN_SOLID
	{
	BYTE donotchange=1;
	BYTE color; // COLOR	// By H.Domon,K3. 23 Aug 2001
	BYTE size;
	}
/* by H.Domon,K3 19 Apr 2001
STRUCT CLOCK_PEN_NULL
	{
	BYTE donotchange=0;
	}

STRUCT CLOCK_BRUSH_SOLID
	{
	BYTE donotchange=1;
	STRUCT color; // COLOR
	}

STRUCT CLOCK_BRUSH_NULL
	{
	BYTE donotchange=0;
	}

//
// general clock STRUCT
//

STRUCT CLOCK
	{
	BYTE type=EEikLocaleConformantClockTypeChangesFormatAccordingToLocale; // one of the EEikLocaleConformantClockTypeXxxxxs
	BYTE typeneverchanges=0;
	LLINK digitalresourceid; // DIGITAL_CLOCK
	LLINK analogresourceid; // ANALOG_CLOCK_WITH_MASKED_FACE or ANALOG_CLOCK_WITH_UNMASKED_FACE
	}

//
// digital clock STRUCTs
//

STRUCT DIGITAL_CLOCK_TEXT_SECTION_TEXT_BLOCK
	{
	LTEXT text;
	}

STRUCT DIGITAL_CLOCK_TEXT_SECTION
	{
	STRUCT font; // FONT
	STRUCT textcolor; // COLOR
	BYTE horizontalalignment; // one of the EEikResourceConstructedClockDigitalDisplayHorizontalTextAlignmentXxxxxs
	BYTE verticalalignment; // one of the EEikResourceConstructedClockDigitalDisplayVerticalTextAlignmentXxxxxs
	WORD horizontalmargin=0;
	WORD verticalmargin=0;
	LEN BYTE STRUCT textblocks[]; // DIGITAL_CLOCK_TEXT_SECTION_TEXT_BLOCKs - will each be interspersed with a flashing-block delimiter
	}

STRUCT DIGITAL_CLOCK
	{
	BYTE withsingleblackborder=1;
	WORD offsetfromuniversaltime=0;
	STRUCT shadow; // CLOCK_SHADOW or NO_CLOCK_SHADOW
	STRUCT backgroundcolor; // COLOR
	LEN BYTE STRUCT textsections[]; // DIGITAL_CLOCK_TEXT_SECTIONs
	}

//
// analog clock STRUCTs
//
by H.Domon,K3 19 Apr 2001
*/
STRUCT ANALOG_CLOCK_HAND_FEATURE_LINE
	{
	STRUCT pen; // CLOCK_PEN_SOLID or CLOCK_PEN_NULL
	STRUCT startpoint; // POINT
	STRUCT endpoint; // POINT
	}
/* by H.Domon,K3 19 Apr 2001
STRUCT ANALOG_CLOCK_HAND_FEATURE_POLY_LINE
	{
	STRUCT pen; // CLOCK_PEN_SOLID or CLOCK_PEN_NULL
	STRUCT brush; // CLOCK_BRUSH_SOLID or CLOCK_BRUSH_NULL
	BYTE closed;
	LEN BYTE STRUCT points[]; // POINTs
	}

STRUCT ANALOG_CLOCK_HAND_FEATURE_CIRCLE
	{
	STRUCT pen; // CLOCK_PEN_SOLID or CLOCK_PEN_NULL
	STRUCT brush; // CLOCK_BRUSH_SOLID or CLOCK_BRUSH_NULL
	STRUCT circlecenter; // POINT
	WORD radius;
	}

STRUCT ANALOG_CLOCK_HAND_FEATURE
	{
	BYTE type; // one of the EEikResourceConstructedClockAnalogDisplayHandFeatureXxxxxs
	STRUCT feature; // ANALOG_CLOCK_HAND_FEATURE_LINE, ANALOG_CLOCK_HAND_FEATURE_POLY_LINE, or ANALOG_CLOCK_HAND_FEATURE_CIRCLE depending on "type"
	}

STRUCT ANALOG_CLOCK_HAND
	{
	BYTE type; // one of the EEikResourceConstructedClockAnalogDisplayHandOneRevPerXxxxxs
	LEN BYTE STRUCT features[]; // ANALOG_CLOCK_HAND_FEATUREs
	}

STRUCT ANALOG_CLOCK_WITH_MASKED_FACE
	{
	BYTE withsingleblackborder=0;
	WORD offsetfromuniversaltime=0;
	STRUCT shadow; // CLOCK_SHADOW or NO_CLOCK_SHADOW
	LTEXT filecontainingface=""; // defaults to the system multi-bitmap file
	LONG faceid; // a value defined in a *.MBG file
	BYTE donotchange=1;
	LTEXT filecontainingfacemask=""; // defaults to the system multi-bitmap file
	LONG facemaskid; // a value defined in a *.MBG file
	LEN BYTE STRUCT hands[]; // ANALOG_CLOCK_HANDs
	}

STRUCT ANALOG_CLOCK_WITH_UNMASKED_FACE
	{
	BYTE withsingleblackborder=0;
	WORD offsetfromuniversaltime=0;
	STRUCT shadow; // CLOCK_SHADOW or NO_CLOCK_SHADOW
	LTEXT filecontainingface=""; // defaults to the system multi-bitmap file
	LONG faceid; // a value defined in a *.MBG file
	BYTE donotchange=0;
	LEN BYTE STRUCT hands[]; // ANALOG_CLOCK_HANDs
	}
by H.Domon,K3 19 Apr 2001
*/
// EIKCMBOX.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT COMBOBOX
	{
    WORD width; // in characters
	WORD maxlength;
	WORD maxarraysize;
	}

// EIKFPNE.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT FLPTED
	{
	WORD maxlength=18;
	DOUBLE min=-9.9e99;
	DOUBLE max=9.9e99;
	DOUBLE default=0; // if !(min<=default<=max), default = min.
	}

STRUCT FIXPTED
	{
	WORD decimalplaces=2;// arbitrary, = number of pence in the pound
	LONG min;
	LONG max;
	LONG default=0; // if !(min<=default<=max), default = min.
	}

STRUCT TWIPSED
	{
	LONG min=0;// but can be set negative
	LONG max;
	}

// EIKGYSEL.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT GRAYSEL
	{
    WORD flags=0;
	}

//#include <eikgysel.hrh>

// EIKHOPBT.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT HOROPBUT
	{
	WORD buttonSpacing=12;
	WORD textOnRight=0;
	LLINK array_id=0;
	}

STRUCT OPBUT
	{
    WORD id=0;
    LTEXT text;
	}

// EIKMFNE.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#include <eikmfne.hrh>

//
// primitive STRUCTs
//

STRUCT TIME
	{
	BYTE second=0;
	BYTE minute=0;
	BYTE hour=0;
	}

STRUCT DATE
	{
	BYTE day=0; // from 0 to 27/28/29/30 inclusive
	BYTE month=0; // from 0 to 11 inclusive
	WORD year;
	}

STRUCT TIME_AND_DATE
	{
	BYTE second=0;
	BYTE minute=0;
	BYTE hour=0;
	BYTE day=0; // from 0 to 27/28/29/30 inclusive
	BYTE month=0; // from 0 to 11 inclusive
	WORD year;
	}

STRUCT DURATION
	{
	LONG seconds=0; // must be greater than or equal to zero
	}

STRUCT TIME_OFFSET
	{
	LONG seconds=0;
	}

STRUCT LATITUDE_AND_LONGITUDE
	{
	LTEXT degreesign;
	LTEXT minutesign;
	LTEXT secondsign;
	LTEXT north;
	WORD keytomatchfornorth;
	LTEXT south;
	WORD keytomatchforsouth;
	LTEXT east;
	WORD keytomatchforeast;
	LTEXT west;
	WORD keytomatchforwest;
	}

STRUCT MFNE_SYMBOLIC_ITEM
	{
	LONG id;
	WORD keytomatch;
	LTEXT text;
	}

STRUCT CELL
	{
	WORD row=0;
	WORD col=0;
	}

//
// STRUCTs for the different types of field
//

STRUCT MFNE_SEPARATOR
	{
	LTEXT text;
	}

STRUCT MFNE_NUMBER
	{
	LONG min;
	LONG max;
	BYTE flags=0;
	}

STRUCT MFNE_SYMBOL
	{
	LEN BYTE STRUCT symbolicitems[]; // MFNE_SYMBOLIC_ITEMs
	}

//
// STRUCTs for the different types of mfne
//

STRUCT NUMBER_EDITOR
	{
	LONG min;
	LONG max;
	}

STRUCT RANGE_EDITOR
	{
	LONG min;
	LONG max;
	LTEXT separatortext="-";
	}

STRUCT CELL_RANGE_EDITOR
	{
	STRUCT minCell; // CELL
	STRUCT maxCell; // CELL
	LTEXT separatortext=":";
	}

STRUCT TIME_EDITOR
	{
	STRUCT minTime; // TIME
	STRUCT maxTime; // TIME
	BYTE flags=0; // permitted flags: EEikTimeWithout[Seconds][Hours]Field
	}

STRUCT DATE_EDITOR
	{
	STRUCT minDate; // DATE
	STRUCT maxDate; // DATE
	BYTE flags=0; // permitted flags: EEikDateWithoutPopoutCalendar
	}

STRUCT TIME_AND_DATE_EDITOR
	{
	STRUCT minTimeAndDate; // TIME_AND_DATE
	STRUCT maxTimeAndDate; // TIME_AND_DATE
	BYTE flags=0; // permitted flags: EEikTimeWithout[Seconds][Hours]Field|EEikDateWithoutPopoutCalendar
	LTEXT interveningtext=" ";
	}

STRUCT DURATION_EDITOR
	{
	STRUCT minDuration; // DURATION
	STRUCT maxDuration; // DURATION
	BYTE flags=0; // permitted flags: EEikTimeWithout[Seconds][Hours]Field
	}

STRUCT TIME_OFFSET_EDITOR
	{
	STRUCT minTimeOffset; // TIME_OFFSET
	STRUCT maxTimeOffset; // TIME_OFFSET
	BYTE flags=0; // permitted flags: EEikTimeWithout[Seconds][Hours]Field
	}

STRUCT LATITUDE_EDITOR
	{
	LONG flags=0;
	LLINK latlongresourceid; // LATITUDE_AND_LONGITUDE
	}

STRUCT LONGITUDE_EDITOR
	{
	LONG flags=0;
	LLINK latlongresourceid; // LATITUDE_AND_LONGITUDE
	}

// EIKPROGI.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT PROGRESSINFO
 	{
    WORD text_type=0;
	WORD splits_in_block=0;
	LONG finalval;
	LONG width=120;
	LONG height=10;
	}

//#include <eikprogi.hrh>

// EIKRTED.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#include <eikrted.hrh>
//#include <eikgted.hrh>
# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\gulftflg.hrh" 1
// GULFTFLG.HRH
//
// Copyright (c) Symbian Software Ltd 1997 - 2004. All rights reserved.
//

/** Maximum length for text selection.
@publishedAll 
@released 
*/


/** All font controls.
@publishedAll 
@released 
*/


/** Font control name.
@publishedAll 
@released 
*/


/** Font control size.
@publishedAll 
@released 
*/


/** Bold font control.
@publishedAll 
@released 
*/


/** Italic font control.
@publishedAll 
@released 
*/


/** Underline font control.
@publishedAll 
@released 
*/


/** Strikethrough font control.
@publishedAll 
@released 
*/


/** Font text color control.
@publishedAll 
@released 
*/


/** Print position font control.
@publishedAll 
@released 
*/


/** All fonts.
@publishedAll 
@released 
*/


/** Monospace fonts only.
@publishedAll 
@released 
*/


/** No monospace fonts.
@publishedAll 
@released 
*/


/** Symbol fonts only.
@publishedAll 
@released 
*/


/** No symbol fonts.
@publishedAll 
@released 
*/


/** No monospace or symbol fonts.
@publishedAll 
@released 
*/

# 891 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.rh" 2


STRUCT RTXTED
    {
	WORD width=0;
	WORD height=0;
	WORD numlines=1;
	WORD textlimit=0;
	LONG flags=0;
	WORD fontcontrolflags=1;
	WORD fontnameflags= 0x10 ;
	WORD default_case= 0x4 ;	WORD allowed_case_modes= 0x1  | 0x2  | 0x4  ;	WORD numeric_keymap=EAknEditorStandardNumberModeKeymap;	WORD allowed_input_modes= 0x1  | 0x2  | 0x4  | 0x8  | 0x10  | 0x20  | 0x40  | 0x80  | 0x200  ;	WORD default_input_mode= 0x1 ;	LLINK special_character_table=-1;	WORD avkon_flags= 0x000 ;	WORD max_view_height_in_lines=0;	WORD base_line_delta=0;	WORD spare=0; 
    }

// EIKSECED.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT SECRETED
	{
 	WORD num_letters=8;
	}

// EIKSPACE.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#include <eikspace.hrh>

STRUCT SPACER
    {
	WORD width=0;
	WORD height=0;
	LONG color=0; // clear
    }

// EIKWSEL.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT WORLD_SELECTOR
	{
	WORD flags=0;
	WORD chars_wide=0;
	LTEXT countrysearch="";
	LTEXT worldsearch="";
	}

//#include <eikwsel.hrh>
// EIKDLGTB.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

//#include <eikctrls.hrh>

STRUCT DLG_TOOLBAR
    {
	WORD width=0;
    STRUCT controls[];
    }

STRUCT DLG_TBAR_BUTTON
	{
	WORD type=1; // EEikCtLabeledButton
	WORD id=0;
	WORD flags=0;
	WORD length=0;
	WORD buttontype=0; // EEikCtCommandButton
	STRUCT button;
	LONG hotkey;
	BYTE hotkeyflags=1; // EEikLabeledButtonShowHotKey
	}

STRUCT DLG_TBAR_SPACER
	{
	WORD type=EEikCtSpacer;
	WORD id=0;
	WORD flags=0;
	WORD length=0;
	WORD width=0;
	WORD height=0;
	LONG color=0; // clear
	}

STRUCT DLG_TBAR_CTRL
	{
	WORD type;
	WORD id=0;
	WORD flags=0;
	WORD length=0;
	STRUCT control;
	}
// EIKPRTPV.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT PRTPREV
    {
    WORD width;
    WORD height;
    }

// EIKFSEL.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT FILENAMESELECTOR
	{
	WORD maxdisplaychar=15;
	}

STRUCT FILENAMEEDITOR
	{
    WORD width = 15;
	}

STRUCT FOLDERNAMESELECTOR
	{
	WORD maxdisplaychar=15;
	}

STRUCT FOLDERNAMEEDITOR
	{
    WORD width = 15;
	}

STRUCT DRIVENAMESELECTOR
	{
	}

// EIKGTED.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//

STRUCT GTXTED
    {
	WORD width=0;
	WORD height=0;
	WORD numlines=1;
	WORD textlimit=0;
	LONG flags=0;
	WORD fontcontrolflags=1;
	WORD fontnameflags= 0x10 ;
	WORD default_case= 0x4 ;	WORD allowed_case_modes= 0x1  | 0x2  | 0x4  ;	WORD numeric_keymap=EAknEditorStandardNumberModeKeymap;	WORD allowed_input_modes= 0x1  | 0x2  | 0x4  | 0x8  | 0x10  | 0x20  | 0x40  | 0x80  | 0x200  ;	WORD default_input_mode= 0x1 ;	LLINK special_character_table=-1;	WORD avkon_flags= 0x000 ;	WORD max_view_height_in_lines=0;	WORD base_line_delta=0;	WORD spare=0; 
    }

// EIKUFSEL.RH
//
// Copyright (c) 1997-1999 Symbian Ltd.  All rights reserved.
//


STRUCT UNIFIEDNAMESELECTOR
	{
	WORD maxdisplaychar=15;
	}

//
// Resource structure for CAknIpFieldEditor
// 

STRUCT IP_FIELD_EDITOR
	{
	STRUCT min_field_values;
	STRUCT max_field_values;
	BYTE flags;		
	}

STRUCT IP_FIELD
	{
	BYTE first_field;
	BYTE second_field;
	BYTE third_field;
	BYTE fourth_field;
	}

// For Enhanced CBA

STRUCT CBA_BUTTON_LINK
	{
	BYTE version= 0x10 ;
	LLINK button=0;
	}


STRUCT ENHANCED_CBA_BUTTON
	{
	BYTE version= 0x20 ;
	BYTE operationType=EEikCommandTypeAnyCommand;
	WORD id=0;
   	LTEXT txt="";
	LTEXT bmpfile="";
  	WORD bmpid=0xffff;		// -1
	WORD bmpmask=0xffff;	// -1
	}

STRUCT EIK_APP_INFO_EXT
	{
	LLINK touch_pane=0;
	LLINK popup_toolbar=0;
	LONG supported_resolutions=0;
	LLINK spare=0; // Reserved for another eik_app_info extension
	}

// for defining priority order for each button (enhanced cba)
STRUCT CBA_PRIORITIES
	{
	STRUCT buttons[]; // array of buttons in number order ( 1. SK1, 2. SK2 etc )
	}

STRUCT BUTTON_PRIORITY
	{
	BYTE priority_order[]; // e.g. EEikCommandTypeOpenItem, EEikCommandTypeAnyCommand 
	}

/**
* Series 60 Button
*
* @since Series 60 3.1
*/
STRUCT AVKON_BUTTON_STATE
    {
    WORD flags=0;
    LTEXT txt="";
    LTEXT helptxt="";
    LTEXT bmpfile="";
    WORD bmpid=0xffff;
    WORD bmpmask=0xffff;
    WORD dim_bmpid=0xffff;
    WORD dim_bmpmask=0xffff;
    WORD press_bmpid=0xffff;
    WORD press_bmpmask=0xffff;
    WORD hover_bmpid=0xffff;
    WORD hover_bmpmask=0xffff;
    LLINK extension=0;
    }

STRUCT AVKON_BUTTON
    {
    WORD flags=0;
    WORD state=0;
    STRUCT states[];
    LLINK extension=0;
    }

STRUCT AVKON_TOOLBAR
    {
    BYTE lines=1;
    LONG flags=0;
    STRUCT items[];
    LLINK extension=0;
    }

# 16 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\avkon.rsg" 1































































































































































































































































































































































































































# 17 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\avkon.rh" 1
/*
* ============================================================================
*  Name     : Avkon.rh
*  Part of  : Avkon
*
*  Description:
*     Resource structure definitions for Avkon 
*  Version:
*
*  Copyright © 2002 Nokia Corporation.
*  This material, including documentation and any related 
*  computer programs, is protected by copyright controlled by 
*  Nokia Corporation. All rights are reserved. Copying, 
*  including reproducing, storing,  adapting or translating, any 
*  or all of this material requires the prior written consent of 
*  Nokia Corporation. This material also contains confidential 
*  information which may not be disclosed to others without the 
*  prior written consent of Nokia Corporation.
* ============================================================================
*/

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\avkon.hrh" 1
/*
* ============================================================================
*  Name     : Avkon.hrh
*  Part of  : Avkon
*
*  Description:
*     Constants for Avkon that may need to be used in resource files
*  Version:
*
*  Copyright © 2002 Nokia Corporation.
*  This material, including documentation and any related
*  computer programs, is protected by copyright controlled by
*  Nokia Corporation. All rights are reserved. Copying,
*  including reproducing, storing,  adapting or translating, any
*  or all of this material requires the prior written consent of
*  Nokia Corporation. This material also contains confidential
*  information which may not be disclosed to others without the
*  prior written consent of Nokia Corporation.
* ============================================================================
*/











# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\aknfontcategory.hrh" 1
/*
* ============================================================================
*  Name     : AknFontCategory.hrh
*  Part of  : FontUtils
*  Description : Avkon font categories correspond to the font "Category types" in the 
*  Series60 layout tool
*
*  Copyright © 2002-2004 Nokia. All rights reserved.
*  This material, including documentation and any related 
*  computer programs, is protected by copyright controlled by 
*  Nokia. All rights are reserved. Copying, including 
*  reproducing, storing, adapting or translating, any 
*  or all of this material requires the prior written consent of 
*  Nokia. This material also contains confidential 
*  information which may not be disclosed to others without the 
*  prior written consent of Nokia.
* ==============================================================================
*/





/**
* Definition of Series 60 logical font categories. 
* Note: These are not logical font ids
* 
* @since Series 60 2.8
*/







enum TAknFontCategory
	{
	EAknFontCategoryUndefined =		0 ,
	EAknFontCategoryPrimary =		1 ,
	EAknFontCategorySecondary =		2 ,
	EAknFontCategoryTitle =			3 ,
	EAknFontCategoryPrimarySmall =	4 ,
	EAknFontCategoryDigital =		5 
	};



// End of File
# 32 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\avkon.hrh" 2

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\aknfontidoffsets.hrh" 1
/*
* ============================================================================
*  Name     : AknFontIdOffsets.h
*  Part of  : FontUtils
*
*  Description: Constants and offsets for Scalable and Encoded Font IDs
*
*  Copyright © 2002-2004 Nokia. All rights reserved.
*  This material, including documentation and any related 
*  computer programs, is protected by copyright controlled by 
*  Nokia. All rights are reserved. Copying, including 
*  reproducing, storing, adapting or translating, any 
*  or all of this material requires the prior written consent of 
*  Nokia. This material also contains confidential 
*  information which may not be disclosed to others without the 
*  prior written consent of Nokia.
* ==============================================================================
*/




/**
* Constants and Offsets for Scalable and Encoded Font IDs
*/

// Constant to form the base value for the definition of Logical fonts

// This constant is retained for binary compatibility




enum {EScalableFontIdOffset = (0x1000)  };

// "Lowest" is defined in an unsigned context



// Define lowest end of range of IDs to be generated by toolchain
// Start at Scalable ID offset, but leave space for all the "lay scaled" IDs



// End of File
# 33 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\avkon.hrh" 2


// This file is very important for applications!
//
// Here we have many names you can find from Series 60 specifications
// This file provides access for applications to the following services:
//   * Selection Service
//   * Find
//

// moved from uikon.hrh


// internal, do not use (maybe you might need to use LoopScrolling though, but use it with some other flags...)






// Note there's also EAknListBoxViewerFlags, which is better for apps.





// These are combined to the viewerflag to make it fit to WORD in resources.



// The next ones are used internally by SingleGraphicStyle and DoubleGraphicStyle





// Implementation detail, do not use - for dialog flags


//
//
//  SELECTION SERVICE IMPLEMENTATION STARTS HERE (Use these in applications!)
//
// LISTBOX FLAGS (use these in applications)
//








// if pen input is supported, then these two flags can be used 
// to get Multiselection and Markable lists to work as s60 style by using stylus










//
// Viewers (see Selection service)
//


//
// DIALOG FLAGS (applications should use these!)
//








// Flags for note dialogs:









// Flags for query dialogs:





//
// CBA LABELS (applications should use these)
//










//
// Menu panes (context specific menu panes)
//






//
// There are also ready-made HandleCommandL() implementations for selection service in aknutils.h
//
// AknSelectionService::HandleSelectionListProcessCommandL(aCommand, iListBox);
// AknSelectionService::HandleMultiSelectionListProcessCommandL(aCommand, iListBox);
// AknSelectionService::HandleMarkableSelectionListProcessCommandL(aCommand, iListBox);
//

//
// Application icon dimensions (for proper layout in Application Shell and Fast Application Swap)
//

enum
    {
    EAknApplicationIconWidth = 42,
    EAknApplicationIconHeight = 29
    };

//
//
//  SELECTION SERVICE IMPLEMENTATION ENDS HERE
//
//


//
// FIND IMPLEMENTATION STARTS HERE, use AknFind with these.
//

// Options menu. Use these instead of R_AVKON_MENUPANE_[SELECTION|MARKABLE]_LIST when find popup window is needed





//
// FIND IMPLEMENTATION ENDS HERE
//




// for ProcessCommandL()
enum
    {
    EAknDummyForProcessCommands=4000,
    EAknMarkAll, // selection service impl
    EAknUnmarkAll, // selection service impl
    EAknCmdHelp,
    EAknCmdAddToPinboard,
    EAknCmdFindPopupActivated,
    EAknCmdEditModeT9,
    EAknCmdEditModeMultitap,
    EAknCmdEditModeNumber,
    EAknCmdEditInsertNumber,
    EAknCmdEditInsertSymbol,
    EAknCmdInputLanguage,
    EAknCmdPredictiveTextCascade,
    EAknCmdT9PredictiveMatches,
    EAknCmdT9PredictiveInsertWord,
    EAknCmdT9PredictiveEditWord,
    EAknCmdT9PredictiveT9Off,
    EAknCmdMultitapPredictiveT9On,
    EAknCmdExit,
    EAknCmdEditTextMenu,
    EAknCmdEditListMenu, // selection service impl
    EAknCmdMark, // selection service impl
    EAknCmdUnmark, // selection service impl
    EAknCmdOpen,    // selection service impl

    // command ids for APAC build
    EChinFepCmdInstructions,
    EChinFepCmdModePinyin,
    EChinFepCmdModeZhuyin,
    EChinFepCmdModeStroke,
    EChinFepCmdModeLatinUpper,
    EChinFepCmdModeLatinLower,
    EChinFepCmdModeNumber,
    EChinFepCmdModeZhuyinFind,
    EChinFepCmdModeStrokeFind,

    EAknCmdEditModeArabicIndicNumber,
    EAknCmdEditModeAlpha,
    EAknCmdEditModeLatinText,

    // command ids for Japanese build
    EJapanFepCmdModeHiragana,
    EJapanFepCmdModeKatakana,
    EJapanFepCmdModePictographs,
    EJapanFepCmdModeFullWidth,
    EJapanFepCmdModeHalfWidth,
    EJapanFepCmdModePredictiveOn,
    EJapanFepCmdModePredictiveOff,
    EJapanFepCmdModeUserDic,
    EJapanFepCmdModeKutenCodeInput,
    EAknCmdEditInsertPictograph,
    EJapanFepCmdPredictiveTextCascade,
    EJapanFepCmdMultitapPredictiveT9On,
    EJapanFepCmdEditModeAlpha,
    EJapanFepCmdEditModeNumber,

    // Command ids for CangJie IME and Chinese Option Menu
    EAknCmdInputMethod,
    EChinFepCmdModeCangJie,
    EChinFepCmdModeCangJieOption,
    
    // Edit menu specific command id
    EAknCmdEditMenuSctSelected,
    
    EAknCmdEnd
};

/**
* Note that although these are the command ids resulting from the CBA key press, codes returned from
* CAknDialog::ExecuteLD are mapped in the routine CEikDialog::MappedCommandId.  This routine should be consulted
* before knowing what the dialog will return.
*/
enum        // CBA constants
    {
    EAknSoftkeyOk = -2 ,
    EAknSoftkeyCancel = -1 ,
    EAknSoftkeySelect = -6 ,
    EAknSoftkeyOptions= 3000,
    EAknSoftkeyBack,
    EAknSoftkeyMark,
    EAknSoftkeyUnmark,
    EAknSoftkeyInsert,
    EAknSoftkeyYes,
    EAknSoftkeyNo,
    EAknSoftkeyDone,
    EAknSoftkeyClose,
    EAknSoftkeyExit,
    EAknSoftkeyClear,
    EAknSoftkeyUnlock,
    EAknSoftkeySave,
    EAknSoftkeyShow,
    EAknSoftkeyListen,
    EAknSoftkeyRead,
    EAknSoftkeySearch,
    EAknSoftkeyEmpty,
    EAknSoftkeyLock,
    EAknSoftkeyCall,
    EAknSoftkeyAgain,
    EAknSoftkeyQuit,
    EAknSoftkeyDetails,
    EAknSoftkeyNext,
    EAknSoftkeyOther,
    EAknSoftkeyEdit,
    EAknSoftkeyOpen,
    EAknSoftkeySend,
    EAknSoftkeyView
    };

enum    {
    EAknSoftkeyDummyUsedWithShortcuts = 3200
    };

enum
    {
    EAknCBAFlagRespondWhenInvisible = 0x02   // softkeys still respond to key events even when control is invisible
    };


// Predefined softkey ranges for supporting limited automatic control of such things as
// return from dialog-like contexts.
// All dialog-like contexts should  return 0 for values in the first range.
// CEikDialog does not use the other implied ranges at all.
// Other contexts may use the "accept" range for internal behaviour






// Define a Cba version number of differentiate from GT CBA
// Current GT version is 0, but it's worth avoiding 1,2 etc in
// case of future incompatibilities.


//
//
// Avkon control id's start from 5700
// Phonebook control id's start from 6000

//
// Avkon Control Type codes
//



enum
    {
    EAknCtTest = 5699 ,
    EAknControlIdBase = 5700,

    EAknCtFormattedListBox, // generic formatted listbox

    // "General list pane descendants"
    EAknCtSingleListBox,
    EAknCtSingleNumberListBox,
    EAknCtSingleHeadingListBox,
    EAknCtSingleGraphicListBox,
    EAknCtSingleGraphicHeadingListBox,
    EAknCtSingleNumberHeadingListBox,
    EAknCtSingleLargeListBox,
    EAknCtDoubleListBox,
    EAknCtDoubleNumberListBox,   // this is also big number implementation
    EAknCtDoubleTimeListBox,
    EAknCtDoubleLargeListBox,  // this is also big large graphic implementation
    EAknCtDoubleGraphicListBox,
    EAknCtSettingListBox,     // this is also big setting pane implementation
    EAknCtSettingNumberListBox,  // this is also big setting number pane implementation

    // "Pop up window list pane descendants"
    EAknCtSinglePopupMenuListBox,
    EAknCtSingleGraphicPopupMenuListBox,
    EAknCtSingleGraphicHeadingPopupMenuListBox,
    EAknCtMenuDoublePopupMenuListBox,
    EAknCtSinglePopupSubmenuListBox,
    EAknCtDoubleLargeGraphicPopupMenuListBox,
    EAknCtSingleHeadingPopupMenuListBox,

    // "List pane elements and descendants(settings, edited)"
    EAknSetListBox,

    // "List pane elements (form, popup)"
    EAknFormGraphicListBox,

    // These are listboxes too.
    EAknCtAppGrid,
    EAknCtPinbGrid,      // This is provided as example only.
    EAknCtQdialGrid,     // this is provided as example only.
    EAknCtCaleMonthGrid, // this is provided as example only.

    // Avkon Status pane controls
    EAknCtTitlePane,
    EAknCtContextPane,
    EAknCtNaviPane,
    EAknCtSignalPane,
    EAknCtBatteryPane,
    EAknCtIndicatorPane,
    EAknCtIdleContextPane,
//
//  For AVKON notes and queries:
//
    EAknCtNote,
    EAknCtQuery,
    EAknCtMultilineQuery,
//
// For AVKON popup field:
//
    EAknCtPopupField,
    EAknCtPopupFieldText,
// For AVKON Form
    EAknCtFormLabelEditor,

// For LAF reader
    EAknCtLAF,
    EAknCtPopupHeadingPane,
    EAknCtSearchField,

//  For Setting Page
    EAknCtSettingPage,

// For AknVolumeControl and AknSlider
    EAknCtVolumeControl,
    EAknCtSlider,
    EAknCtListQueryControl,
    EAknCtSpecialCharacterMap,

// For AknPhoneNumberEditor
    EAknPhoneNumberEditor,

// Numeric secret editor
    EAknCtNumericSecretEditor,

// Popup Setting List Page
    EAknCtPopupSettingList,
// Message query
    EAknCtMessageQuery,

// Specialized numeric edwins
    EAknCtIntegerEdwin,
    EAknCtFloatingPointEdwin,
    EAknCtSelectionListFixedFind,
    EAknCtSelectionListPopupFind,
    EAknCtEmptyList,
    EAknCtDouble2GraphicListBox,
    EAknCtDouble2LargeListBox,
    EAknCtDouble2PopupMenuListBox,

// NOTE: DO NOT USE THESE LISTS (break ui-style, only for java)!!!
    EAknCtSingle2GraphicListBox,
    EAknCtDouble2GraphicLargeListBox,
// END OF NOTE

// Colour selection grid
    EAknCtColourSelectionDialogNoneField, // Deprecated
    EAknCtColourSelectionDialogGrid,      // Deprecated

// Find box in list query dialog
    EAknCtListQuerySearchControl,

// Derived query control for ip & fixed point queries
    EAknExtCtQuery,

// Derived multiline query control for ip&ip queries
    EAknExtCtMultilineQuery,

   EAknCtBitmapAnimation,
   
// button for 4.0
    EAknCtButton,
    
// new popuplists for 3.0    
    EAknCtSingle2GraphicPopupMenuListBox,
    EAknCtDoubleGraphicPopupMenuListBox,

// Colour selection grid control for rel 3.0.
    EAknCtColourSelectionGridDialog,
    
// This is the marker for the last AVKON control id: It does not construct a control
    EAknCtLastControlId = 0x2000 

//  Control ids beyond (or including) this value will be dealt with as custom controls.

//  However, to assist collaboration between applications, the following Series 60 -specific
//  application- related ranges are documented here:
//
//  Phonebook- related control ids = 0x2800 - 0x2C00
//
//  Applications which do not have to correlate with any other DLLs or apps, should use the
//  range 0x2001 - 0x2800
//

// Control ID range 0xF000 - 0xFFFF is reserved for licensee controls.

    };

// Navi decorator control types
enum
    {
    ENaviDecoratorControlNone,
    ENaviDecoratorControlTabGroup,
    ENaviDecoratorLabel,
    ENaviDecoratorImage,
    ENaviDecoratorToolTip // Hint text
    };

// Navigation pane tab width specifiers


enum
    {
    EAknTabWidthWithTwoTabs = 36,
    EAknTabWidthWithTwoLongTabs = 74,
    EAknTabWidthWithThreeTabs = 22,
    EAknTabWidthWithThreeLongTabs = 67,
    EAknTabWidthWithFourTabs = 18,
    EAknTabWidthWithOneTab = 107
    };

// Note dialog constants
enum
    {
    EGeneralLayout = 1,
    EConfirmationLayout,
    EProgressLayout,
    ENoteWithImageLayout,
    ETextualNotificationLayout,
    ENotificationWithGraphicsLayout,
    EAknNoteControlAnimationCreationFailed,
    EWaitLayout
    };

enum
    {
    EAknInvalidBorderCount = 1,
    EAknInvalidDimensionArrayOutOfBounds,
    EAknNoNoteControlDimensionDefined,
    EAknNoteControlNullArgumentUsed
    };

enum EAknNoteItemType {
    EAknNoteNoType = 1,
    EAknNoteBitmap,
    EAknNoteNumberTypeIndication,
    EAknNoteProgressBar,
    EAknNoteLabel,
    EAknNoteAnimation
};

enum TNoteCtrlIds
    {
    EGeneralNote = 1
    };


//Note control flags



// Query dialog constants

// These are query types, and each type must be mapped
// to a layout (e.g. code layout or data layout)
// If you add or change a type here you need to update
// KQueryTypeToLayout in aknquerycontrol.cpp otherwise
// query control will crash with EAknPanicNotSupported
// because the layout is not known
enum
    {
    EDataLayout=1,
    EConfirmationQueryLayout,
    ECodeLayout,
    ETimeLayout,
    EDateLayout,
    EListLayout,
    EMultiListLayout,
    EMultiLineDataLayout,
    EDurationLayout,
    ETextSoftNotification,
    ENumberLayout,
    EPinLayout,
    EPhoneLayout,
    EMessageQueryLayout,
    EStaticSizeDataLayout,
    EFloatingPointLayout,
    EIpLayout,
    EFixedPointLayout,
    EDataLayoutAllowEmptyInput
    };

enum
    {
    ETextEditor=1,
    ESecretEditor,
    ETimeEditor,
    EDateEditor,
    ENumberEditor,
    EDurationEditor,
    EIpEditor,
    EFixedPointEditor
    };

enum EAknMultilineDataLayout
{
    EMultiDataFirstEdwin = 1,
    EMultiDataSecondEdwin,
    EMultiDataFirstSecEd,
    EMultiDataSecondSecEd,
    EMultiDataFirstTimeEd,
    EMultiDataSecondTimeEd,
    EMultiDataFirstDateEd,
    EMultiDataSecondDateEd,
    EMultiDataFirstDurEd,
    EMultiDataSecondDurEd,
    EMultiDataFirstPhoneEd,
    EMultiDataSecondPhoneEd,
    EMultiDataFirstNumEd,
    EMultiDataSecondNumEd,
    EMultiDataFirstPinEd,
    EMultiDataSecondPinEd,
    EMultiDataFirstIpEd,
    EMultiDataSecondIpEd
    };

enum TQueryCtrlIds
    {
    EGeneralQuery = 0x500,
    EMultilineFirstLine,
    EMultilineSecondLine
    };

enum TListQueryCtrlIds
    {
    EListQueryMainInboxList = 1,
    EListQueryMainDraftList,
    EListQueryMainOutboxList,
    EListQueryAppViewLabel,
    EListQueryMsgEditField,
    EListQueryHeading,
    EListQueryControl
    };

enum TSelectionListCtrlIds
    {
    ESelectionListControl = 1,
    EFindControl,
    EAknEmptyListControl,
    EAknListQueryMessageBox
    };


// layout commands (for lists currently)
enum
    {
    ELayoutNoCmd =1,
    ELayoutOptional, // optional items
    ELayoutRightSeparator, // adds separator
    ELayoutTransparentCell,  // transparent cell for formattedlistbox only
    ELayoutItemMark,         // sets column/subcell to handle item mark in multiselection lists

    ELayoutNotFilled,
    ELayoutNotAlwaysDrawn
    };

enum {
    ELayoutAlignNone,
    ELayoutAlignCenter,
    ELayoutAlignLeft,
    ELayoutAlignRight,
    ELayoutAlignBidi
     };

// Editor alignment
enum
    {
    EAknEditorAlignNone = ELayoutAlignNone,
    EAknEditorAlignCenter = ELayoutAlignCenter,
    EAknEditorAlignLeft = ELayoutAlignLeft,
    EAknEditorAlignRight = ELayoutAlignRight,
    EAknEditorAlignBidi = ELayoutAlignBidi
     };

// Editor character case
enum
    {
    EAknEditorCharactersUpperCase = 0x1 ,
    EAknEditorCharactersLowerCase = 0x2 ,
    EAknEditorCharactersTextCase = 0x4 
    };

/**
*
* Series 60 pre-2.8 Font Ids
*
*/

enum {
    EFontNone = 0,
    ELatinPlain12,
    ELatinBold12,
    ELatinBold13,
    ELatinBold17,
    ELatinBold19,
    ENumberPlain5,
    EClockBold30,
    ELatinClock14,
    EFontCustom,
    EPlain12,
    EPlain16,
    EApacPlain12 = EPlain12,
    EApacPlain16 = EPlain16,
    EBold18,
    EBold24,
    ELatinBold16,
    ECalcBold21,
    ECalcOperBold21,
    ECalcOperBold13,
	EHighestFixedLayoutFont = ((0x100-1) ) 
    };

/**
* Series 60 Logical Fonts
*
* @since Series 60 2.6
*/
enum TAknLogicalFontId
	{
	EAknLogicalFontPrimaryFont = ((0x100-1) )   + 1 ,
	EAknLogicalFontSecondaryFont= ((0x100-1) )  + 2 ,
	EAknLogicalFontTitleFont= ((0x100-1) )  + 3 ,
	EAknLogicalFontPrimarySmallFont= ((0x100-1) )  + 4 ,
	EAknLogicalFontDigitalFont= ((0x100-1) )  + 5 ,
	EAknHighestLogicalFont = ((0x1000) -1) 
	};



// For layout resource of findbox
enum
{
    EAknLayoutFindPaneIconId = 1,
    EAknLayoutFindPaneEditorId,
    EAknLayoutMainPaneListGenPane,
    EAknLayoutMainPaneListGenPaneWithFindBox,
    EAknLayoutMainPaneFindPane,

    EAknListSinglePane,
    EAknListSingleNumberPane,
    EAknListSingleHeadingPane,
    EAknListSingleGraphicPane,
    EAknListSingleGraphicHeadingPane,
    EAknListSingleNumberHeadingPane,
    EAknListSingleLargeGraphicPane,
    EAknListDoubleNumberPane,
    EAknListDoubleTimePane,
    EAknListDoubleLargeGraphicPane,
    EAknListDoubleGraphicPane,
    EAknListSettingNumberPane,


    EAknLayoutFindPaneVShadow,
    EAknLayoutFindPaneHShadow,
    EAknLayoutFindPaneOutlineRect,


    EAknLayoutEmptyList1stLine,
    EAknLayoutEmptyList2ndLine
};


// Status pane pane Uids










// POPUP_FIELD flags





enum
    {
    EAknDialogQueryValueTextEditor1 = 1,
    EAknDialogQueryValueNumberEditor1,
    EAknDialogQueryValuePhoneEditor1,
    EAknDialogQueryValueTimeEditor1,
    EAknDialogQueryValueDateEditor1,
    EAknDialogQueryValueDurationEditor1,
    EAknFormQueryControl,
    EAknPopupFieldEmptyListNote
    };

// POPUP_SETTING_LIST flags
enum
    {
    EAknPopupSettingListFlagAllowsUserDefinedEntry = 0x001,
    EAknPopupSettingListFlagInitialised = 0x002
    };

// FORM default menu constants
enum
    {
    EAknFormCmdEdit = 950,
    EAknFormCmdAdd,
    EAknFormCmdSave,
    EAknFormCmdLabel,
    EAknFormCmdDelete,
    EAknFormMaxDefault
    };

// Slider layouts
enum
    {
    EAknFormSliderLayout1 =1,
    EAknFormSliderLayout2,
    EAknFormSliderLayout3,
    EAknSettingsItemSliderLayout,
    EAknSettingsItemSliderLayoutWithGraphics,
    EAknMIDPFormSliderLayout
    };

// Slider value types
enum
    {
    EAknSliderValueNone,
    EAknSliderValueBareFigure,
    EAknSliderValuePercentage,
    EAknSliderValueFraction,
    EAknSliderValueDecimal
    };

// Volume layouts
enum
    {
    ESettingsVolumeControl,
    ENaviPaneVolumeControl,
    EPopupVolumeControl
    };

// Small indicator pane indicators & navi pane editor indicators
enum
    {
    // Status pane's indicator pane's indicators.
    EAknIndicatorIrActive,
    EAknIndicatorEnvelope,
    EAknIndicatorEmailMessage,
    EAknIndicatorDataMessage,
    EAknIndicatorFaxMessage,
    EAknIndicatorOtherMessage,
    EAknIndicatorSecuredConnection,
    EAknIndicatorVoiceMailWaiting,
    EAknIndicatorVoiceMailWaitingOnLine1,
    EAknIndicatorVoiceMailWaitingOnLine2,
    EAknIndicatorVoiceMailWaitingOnBothLines,
    EAknIndicatorHomeZone,
    EAknIndicatorBluetooth,
    EAknIndicatorBluetoothModuleOn,
    EAknIndicatorOutbox,
    EAknIndicatorKeyguard,
    EAknIndicatorAllCallsForwarded,
    EAknIndicatorCallsForwardedOnLine1,
    EAknIndicatorCallsForwardedOnLine2,
    EAknIndicatorCallsForwardedOnBothLines,
    EAknIndicatorCallsForwardedToVoiceMailbox,
    EAknIndicatorSelectedLine,
    EAknIndicatorAlarmClock,
    EAknIndicatorHeadset,
    EAknIndicatorLoopset,
    EAknIndicatorIHFActive,
    EAknIndicatorInstantMessage,
    EAknIndicatorTTY,
    EAknIndicatorUSBConnection,
    EAknIndicatorMissedCalls,
    EAknIndicatorRemoteMailboxEmailMessage,
    EAknIndicatorLocation,
    EAknIndicatorHeadsetUnavailable,
    EAknIndicatorRoaming,
    EAknIndicatorLocationOn,
    EAknIndicatorLocationOff,
    EAknIndicatorPoC,
    EAknIndicatorWlanAvailable,
 	  EAknIndicatorWlanActive,
	  EAknIndicatorWlanActiveSecure,
	  EAknIndicatorCarkit,
    EAknIndicatorPoCDnD,
    EAknIndicatorPoCMissed,
    EAknIndicatorTvOut,
    EAknIndicatorVoIP, 
    EAknIndicatorTARM, 
    EAknIndicatorTARMModuleOn, 
    EAknIndicatorMobileTV, 
    EAknIndicatorMobileTVRec, 

    // Navi pane editor indicators
    EAknNaviPaneEditorIndicatorT9 = 100,
    EAknNaviPaneEditorIndicatorUpperCase,
    EAknNaviPaneEditorIndicatorLowerCase,
    EAknNaviPaneEditorIndicatorTextCase,
    EAknNaviPaneEditorIndicatorNumberCase,
    EAknNaviPaneEditorIndicatorQuery,
    EAknNaviPaneEditorIndicatorQwertyShift,

    EAknNaviPaneEditorIndicatorArabicCase = 130,
    EAknNaviPaneEditorIndicatorHebrewCase,
    EAknNaviPaneEditorIndicatorArabicIndicNumberCase,

    EAknNaviPaneEditorIndicatorDevanagariCase = 140,
    EAknNaviPaneEditorIndicatorDevanagariIndicNumberCase,

    // APAC
    EAknNaviPaneEditorIndicatorPinyin = 150,
    EAknNaviPaneEditorIndicatorZhuyin,
    EAknNaviPaneEditorIndicatorStroke, // Stroke editor indicator for simplified Chinese.

    EAknNaviPaneEditorIndicatorZhuyinFind,
    EAknNaviPaneEditorIndicatorStrokeFind,

    EAknNaviPaneEditorIndicatorCangJie,
    // THAI
    EAknNaviPaneEditorIndicatorThai = 170,


    EAknNaviPaneEditorIndicatorStrokeTrad,  // Stroke editor indicator for traditional Chinese.

    //FARSI aND URDU
    EAknNaviPaneEditorIndicatorFarsiAndUrduCase,

    // Application indicators to editor indicator pane
    EAknNaviPaneEditorIndicatorAudioAttached = 200,
    EAknNaviPaneEditorIndicatorImageAttached,
    EAknNaviPaneEditorIndicatorObjects,
    EAknNaviPaneEditorIndicatorAttachment,
    EAknNaviPaneEditorIndicatorMessageLength,
    EAknNaviPaneEditorIndicatorMessageInfo,

    // Extra application indicators to editor indicator pane in small statuspane
    EAknNaviPaneEditorIndicatorWaitBar,
    EAknNaviPaneEditorIndicatorProgressBar,
    EAknNaviPaneEditorIndicatorSecuredConnection,
    EAknNaviPaneEditorIndicatorWmlWaitGlobe,
    EAknNaviPaneEditorIndicatorGprs,
    EAknNaviPaneEditorIndicatorFileSize,
    EAknNaviPaneEditorIndicatorWmlWindows,
    EAknNaviPaneEditorIndicatorMcePriorityHigh,
    EAknNaviPaneEditorIndicatorMcePriorityLow,
    EAknNaviPaneEditorIndicatorWmlWindowsText,

    // Navi pane editor indicators for Japanese
    EAknNaviPaneEditorIndicatorJapaneseHiraganaKanji = 300, // Hiragana/Kanji
    EAknNaviPaneEditorIndicatorJapaneseHiragana,            // Hiragana
    EAknNaviPaneEditorIndicatorJapaneseFullKatakana,        // Full-width Katakana
    EAknNaviPaneEditorIndicatorJapaneseHalfKatakana,        // Half-width Katakana
    EAknNaviPaneEditorIndicatorJapaneseFullUpperAlphabet,   // Full-width Upper Case alphabet
    EAknNaviPaneEditorIndicatorJapaneseFullLowerAlphabet,   // Full-width Lower Case alphabet
    EAknNaviPaneEditorIndicatorJapaneseFullTextAlphabet,    // Full-width Text Case alphabet
    EAknNaviPaneEditorIndicatorJapaneseFullNumeric,         // Full-width numeric

    // Navi Pane editor indicators for Vietnamese
    EAknNaviPaneEditorIndicatorVietnameseUpperCase = 400,
    EAknNaviPaneEditorIndicatorVietnameseLowerCase,
    EAknNaviPaneEditorIndicatorVietnameseTextCase
    };


enum  // Indicator states
    {
    EAknIndicatorStateOff,
    EAknIndicatorStateOn,
    EAknIndicatorStateAnimate
    };

enum // Signal indicator states
    {
    // GPRS indicator states
    EAknSignalGprsIndicatorOff = 0,
    EAknSignalGprsIndicatorAvailable,  // Same as EAknSignalGprsIndicatorOff
    EAknSignalGprsIndicatorAttached,
    EAknSignalGprsIndicatorEstablishingContext,
    EAknSignalGprsIndicatorContext,
    EAknSignalGprsIndicatorSuspended,
    EAknSignalGprsIndicatorMultipdp,

     // Common packet data indicator states
    EAknSignalCommonPacketDataIndicatorOff = 0x100,
    EAknSignalCommonPacketDataIndicatorAvailable,  // Same as EAknSignalCommonPacketDataIndicatorOff
    EAknSignalCommonPacketDataIndicatorAttached,
    EAknSignalCommonPacketDataIndicatorEstablishingContext,
    EAknSignalCommonPacketDataIndicatorContext,
    EAknSignalCommonPacketDataIndicatorSuspended,
    EAknSignalCommonPacketDataIndicatorMultipdp,

     // WCDMA indicator states
    EAknSignalWcdmaIndicatorOff = 0x200,
    EAknSignalWcdmaIndicatorAvailable,
    EAknSignalWcdmaIndicatorAttached,
    EAknSignalWcdmaIndicatorEstablishingContext,
    EAknSignalWcdmaIndicatorContext,
    EAknSignalWcdmaIndicatorMultipdp,
    EAknSignalWcdmaIndicatorSuspended,


    // CDMA indicator states
    EAknSignalCdmaIndicatorOff = 0x300,
    EAknSignalCdmaIndicatorNoService,
    EAknSignalCdmaIndicatorAvailable,
    EAknSignalCdmaIndicatorNotAvailable,
    EAknSignalCdmaIndicatorActive,
    EAknSignalCdmaIndicatorDormant,
    EAknSignalCdmaIndicatorSending,
    EAknSignalCdmaIndicatorReceiving,

     // EDGE indicator states
    EAknSignalEdgeIndicatorOff = 0x400,
    EAknSignalEdgeIndicatorAvailable, // Same as EAknSignalEdgeIndicatorOff
    EAknSignalEdgeIndicatorAttached,
    EAknSignalEdgeIndicatorEstablishingContext, 
    EAknSignalEdgeIndicatorContext,
    EAknSignalEdgeIndicatorSuspended,
    EAknSignalEdgeIndicatorMultipdp
    };



// Grid layout flags






// Chooses right-to-left or left-to-right depending on language's writing direction
// You MUST use CurrentDataIndex() and CurrentItemIndex() from grid correctly if you
// choose this flag! (Most cases you want to use CurrentDataIndex(), not CurrentItemIndex!)









// Bitmap Animation flags





// Defining colors in resource file (this is for defining your own color schemes, do not use in applications!) (num is the number from LAF specification!) This is used in eikcolor.ra to setup color lists.



// This is used in eikcolor.ra



//
// defines for different listbox layouts
// gfx = index to the image in icon array in A column
// heading = heading text in B column
// text = main text for C column
// optgfx1 = index to optional image in D column
// optgfx2 = index to 2nd optional image in D column
//
// Example:  LB_SINGLE_GRAPHIC_HEADING(1, qgn_icon_1, qtn_text_1)







// SCT enumerations
enum  // ids in SCT_CHAR_SET resource.
    {
    EAknSCTLowerCase,
    EAknSCTUpperCase,
    EAknSCTNumeric,
    EAknSCTFullCase,
    EAknSCTHalfCase,
	EAknSCTQwerty,	       
    EAknSCTChiChineseInputModes = EAknSCTUpperCase,
    EAknSCTChiLatinInputModes = EAknSCTLowerCase
    };

// For Setting Page resource so that no number is displayed


// Flag for Setting Item resource




enum TSCTQueryCtrlIds
    {
    EAknSCTQueryHeadingId = 1,
    EAknSCTQueryContentId
    };

// Enemerations for Avkon KeySounds

enum TAvkonSKeyRepeatType
    {
    ESKeyTypeShort,
    ESKeyTypeLong,
    ESKeyTypeRepeat
    };

// These SID numbers should specify all AVKON defined SID's
// The first item starts from 1000 so there are no clashes
// with application defined SID's (which should start at 0)
enum TAvkonSystemSID
    {
    EAvkonSIDNoSound = 1000,
    EAvkonSIDDefaultSound = 1001,
    EAvkonSIDStandardKeyClick = 1002,
    EAvkonSIDConfirmationTone = 1003,
    EAvkonSIDInformationTone = 1003,
    EAvkonSIDWarningTone = 1004,
    EAvkonSIDErrorTone = 1005,
    EAvkonSIDVolumeMaxTone = 1006,
    EAvkonSIDVolumeMinTone = 1007,
    EAvkonSIDReadialCompleteTone = 1008,
    EAvkonSIDBatteryLowTone = 1009,
    EAvkonSIDChargingBatteryTone  = 1010,
    EAvkonSIDConnectionStartTone  = 1011,
    EAvkonSIDConnectionStopTone  = 1012,
    EAvkonSIDOperationReadyTone  = 1013,
    EAvkonSIDNameDiallerStartTone  = 1014,
    EAvkonSIDNameDiallerErrorTone  = 1015,
    EAvkonSIDNameDiallerAbortTone  = 1016,
    EAvkonSIDPowerOffTone  = 1017,
    EAvkonSIDPowerOnTone  = 1018,
    EAvkonSIDRechargeBatteryTone = 1019,
    EAvkonSIDVoiceRecordingTone  = 1020,
    EAvkonSIDVoiceRecordingStartTone  = 1021,
    EAvkonSIDVoiceRecordingStopTone  = 1022,
    EAvkonSIDWrongCharger  = 1023,
    EAvkonSIDNetBusy = 1024,
    EAvkonSIDNetCallWaiting = 1025,
    EAvkonSIDNetReorder = 1026,
    EAvkonSIDNetCongestion = 1027,
    EAvkonSIDNetSpecialInformation = 1028,
    EAvkonSIDNetRadioNotAvailable = 1029,
    EAvkonSIDIHFActive = 1030,
    EAvkonSIDRadioPathAcknowledge = 1031,
    EAvkonSIDDial = 1032,
    EAvkonSIDRingGoing = 1033,
    EAvkonSIDLocationRequest = 1034
    };

// These are used as priority values for Media Server
enum TAvkonSoundsPriority
    {
    EAvkonKeyClickPriority = 45, // contains DTMF and camera
    EAvkonPoweringTonesPriority = 50,
    EAvkonLowLevelNotesPriority = 55,
    EAvkonConfirmationNotePriority = 55,
    EAvkonVolumeSettingTonesPriority = 60,
    EAvkonConnectionTonesPriority = 65,
    EAvkonHighLevelNotesPriority = 70,
    EAvkonErrorNotePriority = 70,
    EAvkonWarningNotePriority = 70,
    EAvkonVoiceDiallingTonesPriority = 73,
    EAvkonNetworkingTonesPriority = 75,
    EAvkonRecordingTonesPriority = 80,
    EAvkonChargingTonesPriority = 85,
    EAvkonNewMessageTonesPriority = 90,
    EAvkonAlarmTonesPriority = 95,
    EAvkonRingingTonesPriority = 100
    };

// These are used as priority preference values for Media Server
enum TAvkonSoundsPreference
    {
    EAknAudioPrefIncomingCall       =0x01340001,
    EAknAudioPrefIncomingCallLine2  =0x01330001,
    EAknAudioPrefIncomingDataCall       =0x01320001,
    EAknAudioPrefAutoRedialCompleted    =0x01310001,

    EAknAudioPrefAlarmClock         =0x01220001,
    EAknAudioPrefCalendarAlarm      =0x01210001,

// Message Preferences
    EAknAudioPrefNewSMS         =0x01120001,
    EAknAudioPrefNewSpecialMessage  =0x01110001,

// Battery preferences
    EAknAudioPrefBatteryLow         =0x01030001,
    EAknAudioPrefRechargeBattery        =0x01020001,
    EAknAudioPrefWrongCharger       =0x01010001,

// Recording Tone preferences
    EAknAudioPrefVoiceRecStart      =0x00930001,
    EAknAudioPrefVoiceRecRecording  =0x00920001,
    EAknAudioPrefVoiceRecStop       =0x00910001,

// Network preferences
    EAknAudioPrefRadioPathAck       =0x00890001,
    EAknAudioPrefDial               =0x00880001,
    EAknAudioPrefRingGoing          =0x00870001,
    EAknAudioPrefBusy               =0x00860001,
    EAknAudioPrefCallWaiting        =0x00850001,
    EAknAudioPrefReorder            =0x00840001,
    EAknAudioPrefCongestion         =0x00830001,
    EAknAudioPrefSpecialInformation =0x00820001,
    EAknAudioPrefRadioNotAvailable  =0x00810001,

// High level preferences
    EAknAudioPrefError      =0x00720001,
    EAknAudioPrefWarning        =0x00710001,

// Voice Dialling preferences
    EAknAudioPrefVoiceStarting  =0x00630001,
    EAknAudioPrefVoiceError     =0x00620001,
    EAknAudioPrefVoiceAbort     =0x00610001,

// Connection preferences
    EAknAudioPrefConnectionStart    =0x00530001,
    EAknAudioPrefConnectionStop =0x00520001,
    EAknAudioPrefOperationReady =0x00510001,

// Volume / IHF preferences /Previews
    EAknAudioPrefFilePreview    =0x00440001,
    EAknAudioPrefAudioVolumeMin =0x00430001,
    EAknAudioPrefAudioVolumeMax =0x00420001,
    EAknAudioPrefIHFActive      =0x00410001,

// Low Level preferences
    EAknAudioPrefCharging       =0x00330001,
    EAknAudioPrefConfirmation   =0x00320001,
    EAknAudioPrefDefaultTone    =0x00310001,

// Phone power preferences
    EAknAudioPrefPowerOn        =0x00220001,
    EAknAudioPrefPowerOff       =0x00210001,

// Key press / camera click preferences
    EAknAudioPrefKeyPressNonDTMF    =0x00140001,
    EAknAudioPrefKeyDownDTMF    =0x00130001,
    EAknAudioPrefKeyUpDTMF      =0x00120001,
    EAknAudioPrefCamera     =0x00110001,

// Record and VoCos Preferences
    EAknAudioPrefVoiceRec       =0x00060001,
    EAknAudioPrefComposer       =0x00050001,
    EAknAudioPrefRingFilePreview    =0x00040001,
    EAknAudioPrefVocosPlayback  =0x00030001,
    EAknAudioPrefVocosTrain     =0x00020001,
    EAknAudioPrefVocosRecog     =0x00010001,

    EAvkonKeyClickPreference = 0x00140001,
    EAvkonConfirmationNotePreference = 0x00320001,
    EAvkonErrorNotePreference = 0x00720001,
    EAvkonWarningNotePreference = 0x00710001
    };

// Constants for integer editor widths; may be required in resource


enum TMessageQueryCtrlIds
    {
    EAknMessageQueryHeaderId = 1,
    EAknMessageQueryContentId
    };

// CAknIncallBubble flags
    /**
    *   Bit             Explanation
    *   0 and 1         Call state
    *   2               No Ciphering
    *   3               Is muted
    *   4               Is Visible
    *   5               Is a Fax
    *   6               Is a Data call
    *   7               Is a Hscsd data call
    *   8               Line 2 indication
    *   9               Is a Video call
    *   10              Is an emergency call
    *   11              Is a VoIP call
    *   ...
    *   ...
    *   31              Reserved for incall bubble notifier internal use. 
    */
enum TAvkonIncallStatusBubbleFlags
    {
    EAknStatusBubbleAlerting     = 0x00, //  One of
    EAknStatusBubbleActive       = 0x01, //  these
    EAknStatusBubbleOnHold       = 0x02, //  to set
    EAknStatusBubbleDisconnected = 0x03, //  the call state
    EAknStatusBubbleNoCiphering  = 0x04,
    EAknStatusBubbleMuted        = 0x08,
    EAknStatusBubbleVisible      = 0x10,
    EAknStatusBubbleFax      = 0x20,
    EAknStatusBubbleData      = 0x40,
    EAknStatusBubbleDataHscsd      = 0x80,
    EAknStatusBubbleLine2      = 0x100,
    EAknStatusBubbleVideo      = 0x200,
    EAknStatusBubbleEmergency  = 0x400,
    EAknStatusBubbleVoIP       = 0x800,  
     
    EAknStatusBubbleReservedInternal = 0x80000000   
    }; 


/**
 * Timeouts for note dialog wrappers
 * These should be kept same as CAknNoteDialog::TTimeout.
 */
enum TAknNoteDialogTimeout
    {
    EAknNoteDialogUndefinedTimeout = 0,
    EAknNoteDialogShortTimeout = 1500000,
    EAknNoteDialogLongTimeout = 3000000,

    // Change these if Notes UI spec changes:
    EAknConfirmationNoteTimeout = EAknNoteDialogShortTimeout,
    EAknInformationNoteTimeout = EAknNoteDialogLongTimeout,
    EAknErrorNoteTimeout = EAknNoteDialogLongTimeout,
    EAknWarningNoteTimeout = EAknNoteDialogLongTimeout
    };

/**
 * Tones for note dialog wrappers
 * These should be kept same as CAknNoteDialog::TTone.
 */
enum TAknNoteDialogTone
    {
    EAknNoteDialogNoTone = 0,
    EAknNoteDialogConfirmationTone = EAvkonSIDConfirmationTone,
    EAknNoteDialogWarningTone = EAvkonSIDWarningTone,
    EAknNoteDialogErrorTone  = EAvkonSIDErrorTone
    };


enum TAknSelectionListDialogFlags
    {
    EAknFlagFind = 0x0001
    };

enum TAknIconArrayType
    {
    EAknIconArrayNone,
    EAknIconArraySimple,
    EAknIconArrayComplex
    };

// This enum is to distinguish at runtime whether you are in
// European or APAC build variant.
enum EVariantFlag
   {
   EEuropeanVariant=0,
   EApacVariant
   };

//
// This can be extended for additional subvariants.  Routine in AknUtils should be updated
// to return the correct sub-variant.
//
//
enum ESubVariantFlag
   {
   ENoSubVariant = 0,
   EPrcSubVariant,
   EHongKongSubVariant,
   ETaiwanSubVariant
   };


// This is the max length of the screensaver text


// These flags are to be used in conjunction with CAknForm



//
// Deprecated






enum TAknKeyBlockMode
    {
    EDefaultBlockMode=0,
    ENoKeyBlock
    };

// Separator line.
enum TAknSeparatorLinePosition
    {
    ENoLine,
    EAColumn,
    EABColumn,
    EBCColumn
    };
enum TAknColourSelectionDialogIds {
    ECtrlGridType = 100,
    EColourGridPopup,
    EColourGridPopupNone
};


// custom text cursor constants

enum TAknCustomTextCursorIds
    {
    KAknCustomTextCursorIdLeftToRight = 0x101fdf03,
    KAknCustomTextCursorIdRightToLeft = 0x101fdf04,
    KAknCustomTextCursorIdLeftToRightThin = 0x101fe51c,
    KAknCustomTextCursorIdRightToLeftThin = 0x101fe51d
    };

enum TAknNaviPaneBackgroundTypes
    {
    EAknNaviPaneBackgroundTypeNotDefined  = 0,
    EAknNaviPaneBackgroundTypeWipe        = 1,
    EAknNaviPaneBackgroundTypeSolid       = 2
    };
// common bitmap file definitions (for .rss etc.)







// End key closing event. If this window server event is received,
// by default application will be closed unless it is system application.






# 22 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\avkon.rh" 2











STRUCT AVKON_SYSTEM_FONT
	{
	LONG fontuid; // a value from fontids.hrh
	STRUCT named_font; // this must be a NAMED_FONT structure type
	}

STRUCT NAVI_DECORATOR
	{
	WORD type=ENaviDecoratorControlNone;
	STRUCT control;
	}

STRUCT TITLE_PANE
	{
	LTEXT txt="";
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	}

STRUCT CONTEXT_PANE
	{
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	}

STRUCT TAB_GROUP
	{
	WORD tab_width = EAknTabWidthWithTwoTabs;  // two tabs
	WORD active = 0; // index of the first active tab
	STRUCT tabs[];
	}

STRUCT TAB
	{
	WORD id=0;
	LTEXT txt="";
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	}

STRUCT NAVI_LABEL
	{
	LTEXT txt="";
	}

STRUCT NAVI_IMAGE
	{
   	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	}

STRUCT SIGNAL_PANE
	{
	WORD level = 0;
	}

STRUCT BATTERY_PANE
	{
	WORD level = 0;
	}

STRUCT INDICATOR_PANE
	{
	STRUCT indicators[];
	}

STRUCT INDICATOR
	{
	WORD uid;
	WORD npriority=0xffff;
	WORD wpriority=0xffff;
   	LTEXT bmpfile="";
	STRUCT states[];
	}

STRUCT INDICATOR_STATE
	{
	WORD id;
	STRUCT icons[];
	}

STRUCT INDICATOR_ICON
	{
	WORD narrowbmpid=0xffff;
	WORD narrowbmpmask=0xffff;
	WORD widebmpid=0xffff;
	WORD widebmpmask=0xffff;
	}

STRUCT AVKON_VIEW
    {
    BYTE version=0; // resource struct version, do not change.
    LLINK hotkeys=0;
    LLINK menubar=0;
    LLINK cba=0;
    LLINK toolbar=0;
    LLINK extension=0; // reserved for future extensions
    }

STRUCT AVKON_NOTE
	{
	WORD layout;
	LTEXT singular_label;
	LTEXT plural_label="";
	LTEXT imagefile="";
	WORD imageid=0xffff;
	WORD imagemask=0xffff;
	LTEXT iconfile="";
	WORD iconid=0xffff;
	WORD iconmask=0xffff;
	LLINK animation=0;
	WORD flags=0; //TNoteFlags (defined in aknnoteattributes.h)
	}

STRUCT NOTE_COMPONENT
	{
	WORD component_type;
	WORD x;
	WORD y;
	WORD width;
	WORD height;
	}

STRUCT NOTE
	{
	STRUCT components[];
	}

STRUCT AVKON_CONFIRMATION_QUERY
	{
	WORD layout;
	LTEXT label;
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	LLINK animation=0; 
	}

STRUCT AVKON_DATA_QUERY
	{
	WORD layout;
	LTEXT label;
	STRUCT control;
	}

STRUCT AVKON_MESSAGE_QUERY
    {
    LTEXT message = "";
    }

STRUCT AVKON_MULTI_LINE_DATA_QUERY
	{
	WORD layout;   
	LTEXT firstlabel;
	LLINK firstcontrol;
	LTEXT secondlabel;
	LLINK secondcontrol;
	}


// use this with CAknQueryDialog.
// This is for one dialog DLG line with EAknCtListQueryControl
//
// The list query window type is determined from EAknCtSinglePopupMenuListBox - if you want to
// have your own borders, derive from CAknListQueryDialog and override some methods.

STRUCT AVKON_LIST_QUERY_CONTROL
	{
	WORD listtype; // type of the list, for example EAknCtSinglePopupMenuListBox (window type is determined from this)
	STRUCT listbox; // LISTBOX resource, use AVKON_LIST_QUERY_LIST or AVKON_MULTISELECTION_LIST_QUERY_LIST
	LTEXT heading = "";
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	LLINK animation=0;
	}

STRUCT AVKON_HEADING
	{
	LTEXT label = "";  // default is no heading
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	LLINK animation=0;
	LLINK headinglayout = 0x8cc00ca ;
	}

// CAknPopupField
STRUCT POPUP_FIELD
	{
	WORD flags=0; // permitted flags: EAknPopupFieldFlagAllowsUserDefinedEntry
	WORD width=0; // in characters
	LTEXT other=""; // if non-empty, replaces 
	LLINK emptynote = 0; // note to be displayed if popupfield is empty
	LTEXT empty=""; // text to display if popupfield is empty
	LTEXT invalid = ""; // text to display if popupfield is invalid
	}

STRUCT POPUP_FIELD_TEXT
	{
	STRUCT popupfield; // only use POPUP_FIELD here
	LLINK textarray = 0; // resource ID of an array of text
	WORD active = 0; // zero based, item in textarray to be initially selected
	}

STRUCT POPUP_FIELD_SELECTION_BMP
	{
	LTEXT bmpfile="";
	WORD bmpidon=0xffff;
	WORD bmpmaskon=0xffff;
	WORD bmpidoff=0xffff;
	WORD bmpmaskoff=0xffff;
	}

STRUCT SLIDER
	{
	WORD layout;
	WORD minvalue;
	WORD maxvalue;
	WORD step;
	WORD valuetype=EAknSliderValueBareFigure;
	WORD decimalplaces=0;
	LTEXT singular_valuelabel = "";
	LTEXT minlabel="";
	LTEXT maxlabel="";
	LTEXT valuelabel="";
    LLINK graphical_image=0;
	}

STRUCT VOLUME
	{
	WORD flags;
	WORD value;
	LTEXT bmpfile="";
	WORD bmpid=0xffff;
	WORD bmpmask=0xffff;
	}

STRUCT NUMSECRETED
	{
 	WORD num_code_chars=8;
	}

STRUCT GRID_STYLE
	{
	WORD layoutflags = 0;
	WORD primaryscroll = 0;
	WORD secondaryscroll = 0;
	WORD itemsinprimaryorient = 0;
	WORD itemsinsecondaryorient = 0;
	WORD height = 0;
	WORD width = 0;
	WORD gapwidth = 0;
	WORD gapheight = 0;
	}

STRUCT GRID
	{
	BYTE version = 0;
	LONG flags = 0;
	WORD height = 5;		// in items
	WORD width = 10;		// in chars
	LLINK array_id = 0;
	LTEXT emptytext = "";
	LLINK style = 0; 
	}

STRUCT AVKON_SETTING_PAGE
	{
	WORD number = (-16380) ;
	LTEXT label;
	LTEXT hint_text;
	LLINK softkey_resource = 0;
	LLINK menubar= 0;
	WORD type=0xffff;
	LLINK editor_resource_id = 0;
	LLINK invalid_contents_softkey_resource = 0;
	LLINK extension = 0;
	}

STRUCT AVKON_SETTING_ITEM_LIST
	{
	WORD flags = 0; // Allowed values: EAknSettingItemListNumberedStyle 
	LTEXT title = "";
	WORD initial_number = 1;
	STRUCT items[];
	}


STRUCT AVKON_SETTING_ITEM
	{
	WORD identifier = 0;
	LTEXT name ="";
	LLINK setting_page_resource = 0;
	WORD type=0xffff;  
	LLINK setting_editor_resource = 0;
	LLINK associated_resource = 0;
	LTEXT empty_item_text = "";
	LTEXT compulsory_ind_string = "";
	LLINK reserved = 0;  // Added for extensions
	}

STRUCT AVKON_PASSWORD_CONFIRMATION
	{
	LLINK password_query_resource = 0;
	LLINK success_note_resource = 0;
	LLINK failure_note_resource = 0;
	}

STRUCT AVKON_ENUMERATED_TEXT
	{
	WORD value = 0 ;
	LTEXT text;
	}

STRUCT AVKON_POPUP_SETTING_TEXTS
	{
	WORD flags = 0;
	LLINK setting_texts_resource = 0;
	LLINK popped_up_texts_resource = 0;
	LLINK reserved = 0;
	}

STRUCT AVKON_SEARCH_FIELD
	{
	}

STRUCT AVKON_LISTBOX_LINES
	{
	STRUCT vline1;
	STRUCT vline2;
	STRUCT hline;
	}

// Bitmap Animation
STRUCT BMPANIM_DATA
	{
    BYTE identifier = 0; // Do not change, used to identify the used resource struct.
	WORD frameinterval	= -1;
	WORD playmode		= 0;
	BYTE flash		= 0;
	LTEXT bmpfile		= "";
	LLINK frames		= 0;
	LLINK backgroundframe	= 0;
 	}

STRUCT BMPANIM_FRAME
	{
	WORD time		= -1;
	WORD posx		= 0;
	WORD posy		= 0;
	WORD bmpid		= -1;
	WORD maskid		= -1;
	}

// Resource-based layout resources (Deprecated)
// These does not define absolute coordinates => resizing these layouts is possible!
//
// (those which does not mention column or form in their name could be used later by
// other controls for their layout - there will be generic resource reader for these)

// this is used with listboxes
STRUCT AVKON_LB_LAYOUT_POS
	{
	WORD l= (-16380) ; 
	WORD t= (-16380) ; 
	WORD r= (-16380) ; 
	WORD b= (-16380) ; 
	WORD W= (-16380) ; 
	WORD H= (-16380) ;
	}

// this is used with listboxes too
STRUCT AVKON_LB_LAYOUT_GFX
	{
	WORD id;
	WORD C= (-16380) ;
	WORD l= (-16380) ;
	WORD t= (-16380) ;
	WORD r= (-16380) ;
	WORD b= (-16380) ;
	WORD W= (-16380) ;
	WORD H= (-16380) ;
	}

// This is used with listboxes
STRUCT AVKON_LB_LAYOUT_TEXT
	{
	WORD id;
	WORD font; WORD C; WORD lm; WORD rm;  WORD B; WORD W; WORD align;
	}

// This is used with CAknLayoutUtils::LayoutControl()
STRUCT AVKON_LAYOUT_CONTROL
	{
	WORD C= (-16380) ;
	WORD l= (-16380) ;
	WORD t= (-16380) ;
	WORD r= (-16380) ;
	WORD b= (-16380) ;
	WORD W= (-16380) ;
	WORD H= (-16380) ;
	}

// This is used with TAknLayoutRect
STRUCT AVKON_LAYOUT_RECT
	{
	WORD C= (-16380) ;
	WORD l= (-16380) ;
	WORD t= (-16380) ;
	WORD r= (-16380) ;
	WORD b= (-16380) ;
	WORD W= (-16380) ;
	WORD H= (-16380) ;
	}

// this is used with AknListBoxLayouts::LayoutImage


STRUCT AVKON_LAYOUT_TEXT
	{
	WORD font;
	WORD C;
	WORD l;
	WORD r;
	WORD B;
	WORD W;
	WORD J;
	}

// This should be used with CAknLayoutUtils::LayoutEdwin
STRUCT AVKON_LAYOUT_EDWIN
	{
	WORD font;
	WORD C;
	WORD l;
	WORD r;
	WORD B;
	WORD W;
	WORD J;
	WORD lines=1;   // Number of lines shown
	WORD nextB=0;   // Second row's baseline position
	}

// This can be used with CAknLayoutUtils::LayoutLabel to layout CEikLabel's.
STRUCT AVKON_LAYOUT_LABEL
	{
	WORD font;
	WORD C;
	WORD l;
	WORD r;
	WORD B;
	WORD W;
	WORD J;
	WORD nextB=0;
	}

// This is used with CAknLayoutUtils::LayoutPopupField()
STRUCT AVKON_LAYOUT_FORM_FIELD_POPUP_PANE
	{
	WORD font;
	WORD C = (-16380) ;
	WORD l = (-16380) ;
	WORD t = (-16380) ;
	WORD r = (-16380) ;
	WORD b = (-16380) ;
	WORD W = (-16380) ;
	WORD H1 = (-16380) ;
	WORD H2 = (-16380) ;
	WORD H3 = (-16380) ;
	WORD H4 = (-16380) ;
	WORD H5 = (-16380) ;
	WORD H6 = (-16380) ;
	WORD lines = 1;   // Number of lines shown
	}

STRUCT AVKON_LB_LAYOUT_AREA
	{
	WORD id;
	WORD x1; WORD y1;
	WORD x2; WORD y2;
	}

STRUCT AVKON_LB_LAYOUT_COMMAND
	{
	WORD id;
	WORD cmd;
	WORD data=0;	
	}

// This is only for listboxes, use 
// AknLayoutUtils/TAknLayoutText/TAknLayoutRect for getting correct layouts.
STRUCT AVKON_LAYOUT_MANAGER
	{
	STRUCT gfx[];  // AVKON_LAYOUT_GFX
	STRUCT text[]; // AVKON_LAYOUT_TEXT
	STRUCT cmds[]; // AVKON_LAYOUT_COMMAND
	STRUCT area[]; // AVKON_LAYOUT_AREA
	STRUCT pos;     // AVKON_LAYOUT_POS
	}

// This should be used with AknEditUtils::ConstructEditingL
STRUCT AVKON_CONFIG_EDWIN
	{
	WORD editingSpace;	  // Max number of character in a editor field
	WORD editingWindow;   // Maximum number of lines in a editor field
	WORD characterCase;   // EAknEditorCharactersUpperCase,	EAknEditorCharactersLowerCase, EAknEditorCharactersTextCase
	WORD justification;   // EAknEditorAlignNone, EAknEditorAlignCenter, EAknEditorAlignLeft, EAknEditorAlignRight
	WORD allowedToMoveInsertionPoint; 
	WORD cursorYesNo;
	WORD overflowYesNo;
	}


STRUCT AVKON_CBA_BUTTON
	{
	BYTE version= 10 ;
	WORD id=0;
	WORD longpressid=0;
   	LTEXT txt="";
	LTEXT bmpfile="";
  	WORD bmpid=0xffff;		// -1
	WORD bmpmask=0xffff;	// -1
	}
// Special Character Table

STRUCT SPECIAL_CHAR_TABLE
	{
	STRUCT contents[]; // SCT_CHAR_SET
	}


STRUCT SCT_CHAR_SET
	{
	WORD id;
	LTEXT chars = "";
	}



// ---------------------------------------------------------
// Resource structure for CAknIconArray class.
// ---------------------------------------------------------
//
STRUCT AKN_ICON_ARRAY
	{
	WORD type = EAknIconArraySimple;
	// Bitmap file name
	LTEXT bmpfile;
	// Array of AKN_ICON structures.
	STRUCT icons[];
	}

STRUCT AKN_ICON_ARRAY_ARRAY
	{
	WORD type = EAknIconArrayComplex;
	STRUCT iconarrays[]; // AKN_ICON_ARRAY
	}


// ---------------------------------------------------------
// One icon in AKN_ICON_ARRAY
// ---------------------------------------------------------
//
STRUCT AKN_ICON
	{
	// Id of the icon bitmap
	LONG iconId;
	// Id of the mask bitmap
	LONG maskId;
	}

// Avkon KeySounds


STRUCT AVKON_SOUND_INFO
	{
	WORD signature1 = 0x1234;
	WORD signature2 = 0x5678;
	WORD sid;
	WORD priority = EAvkonKeyClickPriority;
	LONG preference = EAvkonKeyClickPreference;
	LTEXT file="";
	WORD frequency=0;
	LONG ms=0;
	WORD sequencelength=0;
	BYTE sequence[];
	BYTE volume=9; // default to loudest value
	}

STRUCT AVKON_SOUND_INFO_LIST 
	{
	STRUCT list[];
	}

STRUCT AVKON_SKEY_INFO
	{
	WORD sid = EAvkonSIDDefaultSound;
	WORD key;
	BYTE type = ESKeyTypeShort;
	}

STRUCT AVKON_SKEY_LIST
	{
	STRUCT list[];
	}

STRUCT PHONE_NUMBER_EDITOR
	{
	WORD maxChars=30;
	WORD maxLines;
	LTEXT truncation="";
	STRUCT formats[];
	}

STRUCT PHONE_NUMBER_EDITOR_FORMAT
	{
	WORD top;
	WORD left;
	WORD bottom;
	WORD right;
	LONG border=0;
	WORD lines;
	WORD bottomBaselineOffset;
	WORD baselineSeparation;
	WORD font;
	}

// CAknPopupSettingList
STRUCT POPUP_SETTING_LIST
	{
	WORD flags; // permitted flags: EAknPopupSettingListFlagAllowsUserDefinedEntry
	WORD width; // in characters.
	LTEXT other="Other ...";
	LTEXT empty_text="No options";
	LLINK new_item_setting_page_resource = 0; // These 3 to allow setting page launch from list pane
	WORD type=0xffff;				
	LLINK new_item_setting_editor_resource = 0;
	}

// This comes from DIALOG resource and must be updates accordingly when
// DIALOG resource is changed. see eikon.rh
STRUCT AVKON_LIST_QUERY
    {
    LONG flags = ((0x001 | 0x200  | 0x040  | 0x2000 ) ) ;
    LTEXT xxxx1 = "";
    LLINK xxxx2 = 0;
    LLINK softkeys = 0x8cc0014 ; // CBA
    STRUCT items[]; // AVKON_LIST_QUERY_DLG_LINE
    LLINK xxxx3 = 0;
    }

STRUCT AVKON_MULTISELECTION_LIST_QUERY
    {
    LONG flags = ((0x001 | 0x200  | 0x040  | 0x2000 ) ) ;
    LTEXT xxxx1 = "";
    LLINK xxxx2 = 0;
    LLINK softkeys = 0x8cc0014 ; // CBA
    STRUCT items[]; // AVKON_MULTISELECTION_LIST_QUERY_DLG_LINE
    LLINK xxxx3 = 0;
    }

STRUCT AVKON_LIST_QUERY_DLG_LINE
    {
    WORD type = EAknCtListQueryControl;
    LTEXT prompt = "";
    WORD id = EListQueryControl;
    LONG itemflags = 0;
    STRUCT control; // AVKON_LIST_QUERY_CONTROL
    LTEXT trailer = "";
    LTEXT bmpfile = "";
    WORD bmpid = 0xffff;
    WORD bmpmask = 0xffff;
    LTEXT tooltip = "";
    }

STRUCT AVKON_MULTISELECTION_LIST_QUERY_DLG_LINE
    {
    WORD type = EAknCtListQueryControl;
    LTEXT prompt = "";
    WORD id = EListQueryControl;
    LONG itemflags = 0;
    STRUCT control; // AVKON_LIST_QUERY_CONTROL
    LTEXT trailer = "";
    LTEXT bmpfile = "";
    WORD bmpid = 0xffff;
    WORD bmpmask = 0xffff;
    LTEXT tooltip = "";
    }


STRUCT AVKON_LIST_QUERY_LIST
    {
    BYTE version = 0;
    LONG flags = ((0x0080 ) ) ;
    WORD height = 5;
    WORD width = 10;
    LLINK array_id = 0;
    }

STRUCT AVKON_MULTISELECTION_LIST_QUERY_LIST
    {
    BYTE version = 0;
    LONG flags = ((0x0080 )  | 0x0001   | 0x2000 ) ;
    WORD height = 5;
    WORD width = 10;
    LLINK array_id = 0;    
    }
 
// LISTBOX resources:
STRUCT AVKON_SELECTION_LIST
    {
    BYTE version = 0;
    LONG flags = ((0x0080 ) ) ;
    WORD height = 5;
    WORD width = 10;
    LLINK array_id = 0;
    }
STRUCT AVKON_MARKABLE_LIST
    {
    BYTE version = 0;
    LONG flags = ((0x0080 )  | 0x0001   | 0x4000 ) ;
    WORD height = 5;
    WORD width = 10;
    LLINK array_id = 0;
    }

STRUCT AVKON_MULTISELECTION_LIST
    {
    BYTE version = 0;
    LONG flags = ((0x0080 )  | 0x0001   | 0x2000 ) ;
    WORD height = 5;
    WORD width = 10;
    LLINK array_id = 0;
    }

STRUCT AVKON_MENU_LIST
    {
    BYTE version = 0;
    LONG flags = ((0x0080 ) ) ;
    WORD height = 5;
    WORD width = 10;
    LLINK array_id = 0;
    }

STRUCT AVKON_INTEGER_EDWIN
	{
    WORD maxlength = 0;
	LONG min =			 -99999999;
	LONG max =			  99999999;
	LONG unset_value=	-100000000;
	LLINK extension1 = 0;
	LLINK extension2 = 0;
	}

 
 
// Avkon note wrappers


STRUCT AKN_NOTE_WRAPPER
    {
    // Id of the dialog resource for this note
    LLINK resId;
    // see TAknNoteDialogTimeout
    LONG timeout;
    // see TAknNoteDialogTone
    WORD  tone=-1;
    // Prompt text
    LTEXT text="";
    }

STRUCT AVKON_GROUPED_NOTE_TEXTS
  {
  LLINK singular_label; // must point to resource of type TBUF 
  LLINK plural_label; // must point to resource of type TBUF 
  WORD flags=0; // for future proofing
  }



// DEPRECATED.
STRUCT AKN_SELECTION_LIST
    {
    WORD flags = 0; // see avkon.hrh / EAknFlagFind etc..
    WORD listbox = EAknCtSingleListBox; // listbox layout 
    LLINK menubar = 0; // MENUBAR resource 
    LLINK dialogresource = 0x8cc0099 ;
    }

STRUCT AVKON_UNUSED
	{
	}


STRUCT AVKON_COLOUR_SELECTION_GRID
    {
    LTEXT nonetext = "";
    STRUCT gridcontrol;  // GRID
    }
        
        


# 18 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.hrh" 1
# 1236 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\eikon.hrh"

# 19 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "C:\\Symbian\\9.1\\S60_3rd\\\\epoc32\\include\\appinfo.rh" 1
// APPINFO.RH
//
// Copyright (c) Symbian Software Ltd 2004-2005. All rights reserved.
//



//
// contains definitions for the Application Information resource files

/**
@publishedAll
@released
*/


//////////////////////////////////////////////////////////////////
// DEFINITIONS FOR THE NON-LOCALISABLE REGISTRATION RESOURCE FILE
//////////////////////////////////////////////////////////////////

/**
@publishedAll
@released
*/


/**
@publishedAll
@released
*/
// constants for capability.hidden



/**
@publishedAll
@released
*/
// constants for capability.newfile



/**
@publishedAll
@released
*/
// constants for capability.embeddability






/**
@publishedAll
@released
*/
// constants for capability.launch



/**
@publishedAll
@released
*/
// bit flags for capability.attributes




/**
@publishedAll
@released
*/
// application group name



/**
@publishedAll
@released
*/
STRUCT APP_REGISTRATION_INFO
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LTEXT app_file(256 ) = ""; // name of application binary file (not including extension) - mandatory
	LONG attributes = 0;
	//
	LTEXT localisable_resource_file(256 ) = ""; // path (not including drive) and name of localisable resource file
	LONG localisable_resource_id = 1;
	//
	BYTE hidden = 0 ;
	BYTE embeddability = 0 ;
	BYTE newfile = 0 ;
	BYTE launch = 0 ;
	LTEXT group_name(16 ) = "";
	//
	BYTE default_screen_number = 0;
	//
	LEN WORD STRUCT datatype_list[]; // DATATYPE
	//
	LEN WORD STRUCT file_ownership_list[]; // FILE_OWNERSHIP_INFO
	//
	LEN WORD STRUCT service_list[]; // SERVICE_INFO
	//
	LLINK opaque_data = 0;	// use for opaque data to send to non-native application launchers i.e. MIDlet id
	}


/**
@publishedAll
@released
*/


/**
@publishedAll
@released
*/
enum
	{
	EDataTypePrioritySystem = 0xFFF9,
	EDataTypePriorityHigh = 10000,
	EDataTypePriorityNormal = 0,
	EDataTypePriorityLow = -10000,
	EDataTypePriorityLastResort = -20000
	};

/**
@publishedAll
@released
*/
STRUCT DATATYPE
	{
	LONG priority;
	LTEXT8 type(256 );
	}

/**
@publishedAll
@released
*/
STRUCT FILE_OWNERSHIP_INFO
	{
	LTEXT file_name(256 );
	}

/**
@publishedAll
@released
*/
STRUCT SERVICE_INFO
	{
	LONG uid = 0;
	LEN WORD STRUCT datatype_list[];
	LLINK opaque_data = 0;
	}


/////////////////////////////////////////////////
// DEFINITIONS FOR THE LOCALISABLE RESOURCE FILE
/////////////////////////////////////////////////

/**
@publishedAll
@released
*/


/**
@publishedAll
@released
*/
STRUCT LOCALISABLE_APP_INFO
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LTEXT short_caption(256 ) = "";
	STRUCT caption_and_icon; // CAPTION_AND_ICON_INFO
	//
	LEN WORD STRUCT view_list[]; // VIEW_DATA
	//
	LTEXT group_name(16 ) = "";
	}

/**
@publishedAll
@released
*/
STRUCT CAPTION_AND_ICON_INFO
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LTEXT caption(256 ) = "";
	//
	WORD number_of_icons = 0; // each icon must be a bitmap/mask pair
	LTEXT icon_file(256 ) = "";
	}

/**
@publishedAll
@released
*/
STRUCT VIEW_DATA
	{
	LONG reserved_long = 0; // reserved for future use, do not use
	LLINK reserved_llink = 0; // reserved for future use, do not use
	//
	LONG uid = 0;
	//
	LONG screen_mode = 0;
	//
	STRUCT caption_and_icon; // CAPTION_AND_ICON_INFO
	}



# 20 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "c:\\Projects\\_next2friends\\_aaf_new\\inc\\Aaf.hrh" 1
/*
============================================================================
 Name        : Aaf.hrh
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Enumerations used in the application UI
============================================================================
*/






// Aaf enumerate command codes
enum TAafIds
	{
	ESetIAP = 1,
	EHelp,
	EAbout,
	
	// General CBA
	EAafSoftkeyContinue,
	
	// Main view listbox itmes
	EAskQuestion,
	EMakePhoto,
	EUseMainCamera,
	EUseSecondaryCamera,
	EFileBrowserView,
	EImageCartView,
	EPrivateQuestions,
	EResponses,
	ECredentialsView,
	EInviteFriend,
	
	// "Upload Images" view menu items
	EUploadImagEAafload,
	EUploadImagesFileBrowser,
	
	// "File Browser" view menu items
	EFileBrowserGotoUpload,
	EFileBrowserAddtoUpload,
	EFileBrowserOpen,
	EFileBrowserFileProperties,
	EFileBrowserPreview,
	EFileBrowserSelect,
	
	// "Upload Image Card" view menu items
	EUploadImageAskQuestion,
	EUploadImageCartFileBrowser,
	EUploadImageCartPreview,
	EUploadImageCartSelect,
	EUploadImageCartDeselect,
	EUploadImageCartRemoveSelected,
	EUploadImageCartRemoveAll,
	
	// "Make a photo" view menu items
	EMakePhotoCapture,
	EMakePhotoNew,
	EMakePhotoSave,
	EMakePhotoZoomIn,
	EMakePhotoZoomOut,
	EMakePhotoResolution,
	EMakePhotoRes640x480,
	EMakePhotoRes480x320,
	EMakePhotoRes320x240,
	
	// Private questions view
	EPQuestionsRefresh,
	EPQuestionDetails,
	
	// Responses view
	EMyQuestionsRefresh,
	EMyQuestionDetails,
	EMyQuestionComments,
	
	// Comments view
	EQuestionCommentsRefresh,
	EQuestionCommentDetails,
	
	// Ask a question form
	EAskQuestionSubmit,
	EAskQuestionNext
	};
	
	
enum TQuestionForm
    {
    EQuestionText = 0x200,
    EQuestionType,
    EQuestionDuration,
    EQuestionPrivate,
    ECustomResponseA,
    ECustomResponseB
    };
 
enum TDataViewDialog
	{
	EPrivateQuestionText = 1,
	EPrivateQuestionDate,
	EPrivateQuestionNickname,
	
	EUserQuestionText,
	
	EQuestionCommentText,
	EQuestionCommentNickname,
	EQuestionCommentData	
	};
	
// File browser engine enums
enum TFileBrowserDirectories
    {
    EFileBrowserSounds = 1,
	EFileBrowserPictures,
	EFileBrowserVideos,
	EFileBrowserDirNoChange
    };

enum TFileBrowserSizeDate
    {
    EFileBrowserSize = 1,
	EFileBrowserDate,
	EFileBrowserToggle,
	EFileBrowserSizeDateNoChange
    };

// __AAF_HRH__
# 21 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "c:\\Projects\\_next2friends\\_aaf_new\\inc\\Aaf.rsg" 1
























































































# 22 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rls" 1
/*
============================================================================
 Name        : Aaf.rls
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : String localization file
============================================================================
*/

//  LOCALISATION STRINGS

//d:Caption string for app.
rls_string qtn_aaf_caption_string "N2F Ask-A-Friend"

//d:Short caption string for app.
rls_string qtn_aaf_short_caption_string "N2F Ask-a-friend"

//d:Set internet access point
rls_string qtn_aaf_set_iap "Set access point"

//d:Details
rls_string qtn_aaf_item_details "Details"

//d:Third item in "Options" menu pane, "Help" event
rls_string qtn_help "Help"

//d:Forth item in "Options" menu page, "About" event
rls_string qtn_about "About"

//d:Third item in "Options" menu pane, "exit" event.
rls_string qtn_aaf_exit "Exit"


// Main view items
rls_string qtn_ask_question "0\tAsk A Question\t\t"

rls_string qtn_make_photo "1\tTake a new photo\t\t"

rls_string qtn_filebrowser "2\tFile Browser\t\t"

rls_string qtn_upload_imagecart "3\tImage Cart\t\t"

rls_string qtn_private_questions "4\tPrivate Questions\t\t"

rls_string qtn_responses "5\tResponses\t\t"

rls_string qtn_credentials "6\tCredentials\t\t"

rls_string qtn_invite_friend "7\tInvite a Friend\t\t"

rls_string qtn_exit "8\tExit\t\t"

// Without camera
rls_string qtn_ask_question_without_camera "0\tAsk A Question\t\t"

rls_string qtn_filebrowser_without_camera "1\tFile Browser\t\t"

rls_string qtn_upload_imagecart_without_camera "2\tImage Cart\t\t"

rls_string qtn_private_questions_without_camera "3\tPrivate Questions\t\t"

rls_string qtn_responses_without_camera "4\tResponses\t\t"

rls_string qtn_credentials_without_camera "5\tCredentials\t\t"

rls_string qtn_invite_friend_without_camera "6\tInvite a Friend\t\t"

rls_string qtn_exit_without_camera "7\tExit\t\t"

// Question form strings
rls_string qtn_question_text_label "Question Text"

rls_string qtn_question_type_label "Response Type"

rls_string qtn_question_duration_label "Question Duration"

rls_string qtn_question_private_label "Mark as private"

rls_string qtn_question_response_a "Custom response A"

rls_string qtn_question_response_b "Custom response B"

// Question type
rls_string qtn_question_type_01 "Yes or No"

rls_string qtn_question_type_02 "Answer A or B"

rls_string qtn_question_type_03 "Rate 1..10"

rls_string qtn_question_type_04 "Multiple Select"

// Question duration
rls_string qtn_question_duration_01 "3 min"

rls_string qtn_question_duration_02 "15 min"

rls_string qtn_question_duration_03 "60 min"

rls_string qtn_question_duration_04 "1 day"

// Question privacy
rls_string qtn_question_private_yes "Yes"

rls_string qtn_question_private_no "No"

// Question form menu items
rls_string qtn_question_form_submit "Submit"

rls_string qtn_question_form_next "Next"

rls_string qtn_question_form_responses "Responses"

rls_string qtn_question_form_pquestions "Private Questions"

// Upload images cart view menu
rls_string qtn_aaf_select "Select"

rls_string qtn_upload_images_ask_question "Ask A Question"

rls_string qtn_upload_images_file_browser "File Browser"

rls_string qtn_upload_images_deselect "Deselect"

rls_string qtn_upload_images_remove_selected "Remove selected"

rls_string qtn_upload_images_remove_all "Remove all"

// File browser view menu
rls_string qtn_filebrowser_goto_upload "Go to Upload Cart"

rls_string qtn_filebrowser_addto_upload "Add to Upload Cart"

rls_string qtn_filebrowser_open_dir "Open"

rls_string qtn_filebrowser_preview "Preview"

// File browser view auxilary strings
rls_string qtn_filebrowser_phonememory " (phone memory)"

rls_string qtn_filebrowser_cardmemory " (card memory)"

rls_string qtn_select_folder_dialog "Select folder"

// Generally used strings
rls_string qtn_aaf_back "Back"

rls_string qtn_aaf_cancel "Cancel"

rls_string qtn_aaf_menu "Menu"

rls_string qtn_aaf_continue "Continue"

rls_string qtn_aaf_more "More"

rls_string qtn_aaf_refresh "Refresh"

rls_string qtn_aaf_details "Details"

rls_string qtn_aaf_comments "Comments"

// Make a photo menu items
rls_string qtn_aaf_make_photo_capture "Capture"

rls_string qtn_aaf_make_photo_save "Save photo"

rls_string qtn_aaf_make_photo_new "New photo"

rls_string qtn_aaf_make_photo_zoomin "Zoom in"

rls_string qtn_aaf_make_photo_zoomout "Zoom out"

rls_string qtn_aaf_make_photo_resolution "Set photo size"

rls_string qtn_aaf_make_photo_res1 "jpeg 640x480"

rls_string qtn_aaf_make_photo_res2 "jpeg 480x320"

rls_string qtn_aaf_make_photo_res3 "jpeg 320x240"

// Login view labels
rls_string qtn_aaf_login_label "Login"

rls_string qtn_aaf_password_label "Password"

// Connection in process dialog
rls_string qtn_aaf_login_waitnote "Connecting, please wait..."

rls_string qtn_aaf_general_waitnote "Please wait..."

// Connection failure note dialog
rls_string qtn_aaf_login_failure_dialog "Posting credentials to server failed! Please, try later."

// Empty credentials warning
rls_string qtn_aaf_login_empty_credentials "Login or password couldn't be empty!"

// Empty question text warning
rls_string qtn_aaf_empty_question_text "Question text has to be non-empty!.."

// Empty custom response A warning
rls_string qtn_aaf_empty_custom_responsea "Custom response A text has to be non-empty!.."

// Empty custom response B warning
rls_string qtn_aaf_empty_custom_responseb "Custom response B text has to be non-empty!.."

// Connection initialization error
rls_string qtn_aaf_connection_not_ready "Error has occured while initializating connection"

// Upload in process dialog
rls_string qtn_aaf_upload_waitnote "Submitting question to server..."

// Photos uploading failure
rls_string qtn_aaf_photos_upload_failure "Failed to upload images..."

// Privates question retrieving dialog
rls_string qtn_aaf_pquestions_waitnote "Retrieving private questions from server..."

// Upload failure note dialog
rls_string qtn_aaf_upload_failure_dialog "Failed to upload..."

// Private questions retrieving failure dialog
rls_string qtn_aaf_pquestions_failure_dialog "Failed to retrieve private questions..."

// My questions retrieving dialog
rls_string qtn_aaf_myquestions_waitnote "Retrieving your questions from server..."

// My questions retrieving failure dialog
rls_string qtn_aaf_myquestions_failure_dialog "Failed to retrieve your questions..."

// Questions comments retrieving dialog
rls_string qtn_aaf_comments_waitnote "Retrieving responses from server..."

// Questions comments retrieving failure dialog
rls_string qtn_aaf_comments_failure_dialog "Failed to retrieve responses from server..."


// Photo save dialog
rls_string qtn_aaf_filesave_dialog "Enter filename to save photo:"

// Phone selection query
rls_string qtn_aaf_phone_selection_query "Choose number:"

// Credentials are not valid
rls_string qtn_aaf_upload_failure_credentials "Credentials are not valid"

// There is no image to upload
rls_string qtn_aaf_upload_no_image "No image has been selected to attach the question. Select up to 3 files."

rls_string qtn_aaf_empty_image_cart "Image cart is empty.\nUse file browser to choose pictures"

// Private questions
rls_string qtn_aaf_empty_private_questions_view "You haven't got any private question"

// Own questions
rls_string qtn_aaf_empty_own_questions_view "You haven't asked any question yet"

// No mobile phone number is specified fot the contact
rls_string qtn_aaf_empty_phone_number "No mobile phone number is specified for the contact"

// Invite a friend sms editor text
rls_string qtn_aaf_sms_text "Let's keep in touch via Next2Friends service, register at http://www.next2friends.com"

//d:About dialog title
rls_string qtn_about_dialog_title "About"

//d:About dialog text
rls_string qtn_about_dialog_text "Ask-A-Friend Version 1.0.0\n\nAuthor: Next2Friends\n\nSupport: contact@next2friends.com\n\n(c) Next2Friends, 2008.\n\nAsk-A-Friend is a dynamic new way to get instant feedback from all your friends, or if you choose, the entire Next2Friends community. It works like this: You snap photos to create an instant opinion poll to send to whomever you choose for a vote. Your friends will receive notification via their mobile device or their Next2Friends profile that an opinion poll has been created. They choose their favorites among the pictures and the results are then sent directly back to you. Ask-A-Friend gives you real time results when you need them the most; in the real world!"

//d:Privacy Statement file name
rls_string qtn_privacy_statement_filename "PrivacyStatement.txt"

//d:Privacy Statement dialog header
rls_string qtn_privacy_statement_header "AAF Privacy Statement"

//d:Settings file name
rls_string qtn_settings_filename "aafsettings.dat"

// End of File
# 23 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2

# 1 "c:\\Projects\\_next2friends\\_aaf_new\\inc\\aaf.mbg" 1
// Aaf.mbg
// Generated by BitmapCompiler
// Copyright (c) 1998-2001 Symbian Ltd.  All rights reserved.
//

enum TMbmAaf
	{
	EMbmAafAsk,
	EMbmAafAskmask,
	EMbmAafCamera,
	EMbmAafCameramask,
	EMbmAafCart,
	EMbmAafCartmask,
	EMbmAafFile,
	EMbmAafFilemask,
	EMbmAafFolder,
	EMbmAafFoldermask,
	EMbmAafN2f_commercelogo_1,
	EMbmAafN2f_commercelogo_2,
	EMbmAafN2f_commercelogo_3,
	EMbmAafPrivate,
	EMbmAafPrivatemask,
	EMbmAafQuestions,
	EMbmAafQuestionsmask,
	EMbmAafSelected,
	EMbmAafSelectedmask,
	EMbmAafSettings,
	EMbmAafSettingsmask,
	EMbmAafSms,
	EMbmAafSmsmask,
	EMbmAafUnselected,
	EMbmAafUnselectedmask,
	EMbmAafCard,
	EMbmAafCardmask,
	EMbmAafFolderup,
	EMbmAafFolderupmask,
	EMbmAafPhone,
	EMbmAafPhonemask
	};
# 24 "c:\\Projects\\_next2friends\\_aaf_new\\data\\Aaf.rss" 2


//  RESOURCE DEFINITIONS

// -----------------------------------------------------------------------------
//
//    Define the resource file signature
//    This resource should be empty.
//
// -----------------------------------------------------------------------------
//
RESOURCE RSS_SIGNATURE
	{
	}

// -----------------------------------------------------------------------------
//
//    Default Document Name
//
// -----------------------------------------------------------------------------
//
RESOURCE TBUF r_default_document_name
	{
	buf="AAF";
	}

// -----------------------------------------------------------------------------
//
//    Define default menu and CBA key.
//
// -----------------------------------------------------------------------------
//
RESOURCE EIK_APP_INFO
	{
	menubar = r_aaf_menubar;
	cba = 0x8cc001a ;
	}


// -----------------------------------------------------------------------------
//
//    Localizable application information
//
// -----------------------------------------------------------------------------
//
RESOURCE LOCALISABLE_APP_INFO r_aaf_localisable_app_info
	{
	short_caption = qtn_aaf_short_caption_string;
	caption_and_icon = 
	CAPTION_AND_ICON_INFO
		{
		caption = qtn_aaf_caption_string;
		number_of_icons = 1;
		icon_file = "\\resource\\apps\\aaf_reg.mif";
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_menubar
//   Menubar for Aaf example
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_menu
//   Menu for "Options"
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_menu
	{
	items =
		{
		// added the new Options menu command here
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},				
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}
		};
	}

// -----------------------------------------------------------------------------
//
//    Define standart CBA key.
//
// -----------------------------------------------------------------------------
//
RESOURCE CBA r_aaf_cba_standart
	{
	buttons = 
		{
		CBA_BUTTON { txt = qtn_aaf_menu; id = EAknSoftkeyOptions; },
		CBA_BUTTON { txt = qtn_aaf_back; id = EAknSoftkeyBack; }
		}; 
	}

// -----------------------------------------------------------------------------
//
//    Define start CBA key.
//
// -----------------------------------------------------------------------------
//
RESOURCE CBA r_aaf_cba_start
	{
	buttons = 
		{
		CBA_BUTTON { txt = qtn_aaf_continue; id = EAafSoftkeyContinue;},
		CBA_BUTTON { txt = qtn_aaf_more; id = EAknSoftkeyOptions; }		
		}; 
	}
	
// -----------------------------------------------------------------------------
//
//    Define credentials edit CBA key.
//
// -----------------------------------------------------------------------------
//
RESOURCE CBA r_aaf_cba_credentials
	{
	buttons = 
		{
		CBA_BUTTON { txt = qtn_aaf_continue; id = EAafSoftkeyContinue;},
		CBA_BUTTON { txt = qtn_aaf_cancel; id = EAknSoftkeyCancel; }		
		}; 
	}
	
// -----------------------------------------------------------------------------
//
//    Define CBA key for the main view
//
// -----------------------------------------------------------------------------
//
RESOURCE CBA r_aaf_cba_mainview
	{
	buttons = 
		{
		CBA_BUTTON { txt = qtn_aaf_more; id = EAknSoftkeyOptions; },
		CBA_BUTTON { id = EAknSoftkeyEmpty; }
		}; 
	}
	
// -----------------------------------------------------------------------------
//
//   r_aaf_loginview_menubar
//   Menubar for the login view
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_loginview_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_loginview_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_loginview_menu
//   Menu for 'More' (login view)
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_loginview_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = ESetIAP;
			txt = qtn_aaf_set_iap;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}		
		};
	}
	
// -----------------------------------------------------------------------------
//
//   r_aaf_mainview_menubar
//   Menubar for the main view screen
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_mainview_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_mainview_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_mainview_menu
//   Menu for 'More' (main view screen)
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_mainview_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EAknSoftkeySelect;
			txt = qtn_aaf_select;
			},
		MENU_ITEM
			{
			command = ESetIAP;
			txt = qtn_aaf_set_iap;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}		
		};
	}
	
// -----------------------------------------------------------------------------
//
//	Define CBA key for the question form
//
// -----------------------------------------------------------------------------
//
RESOURCE CBA r_aaf_cba_question_form
	{
	buttons = 
		{
		CBA_BUTTON { txt = qtn_aaf_menu; id = EAknSoftkeyOptions; },
		CBA_BUTTON { txt = qtn_aaf_back; id = EAknSoftkeyBack; }
		}; 
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_filebrowser_menubar
//   Menubar for 'File Browser'
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_filebrowser_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_filebrowser_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_filebrowser_menu
//   Menu for 'Menu' ('File Browser')
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_filebrowser_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EFileBrowserGotoUpload;
			txt = qtn_filebrowser_goto_upload;
			},				
		MENU_ITEM
			{
			command = EFileBrowserAddtoUpload;
			txt = qtn_filebrowser_addto_upload;
			},
		MENU_ITEM
			{
			command = EFileBrowserOpen;
			txt = qtn_filebrowser_open_dir;
			},
		MENU_ITEM
			{
			command = EFileBrowserPreview;
			txt = qtn_filebrowser_preview;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}		
		};
	}


// -----------------------------------------------------------------------------
//
//   r_aaf_upload_image_cart_menubar
//   Menubar for 'Upload Image Cart'
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_upload_image_cart_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_upload_image_cart_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_upload_image_cart_menu
//   Menu for 'Menu' ('Upload Image Cart')
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_upload_image_cart_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EAskQuestion;
			txt = qtn_upload_images_ask_question;
			},				
		MENU_ITEM
			{
			command = EUploadImageCartFileBrowser;
			txt = qtn_upload_images_file_browser;
			},
		MENU_ITEM
			{
			command = EUploadImageCartPreview;
			txt = qtn_filebrowser_preview;
			},
		MENU_ITEM
			{
			command = EUploadImageCartSelect;
			txt = qtn_aaf_select;
			},
		MENU_ITEM
			{
			command = EUploadImageCartDeselect;
			txt = qtn_upload_images_deselect;
			},
		MENU_ITEM
			{
			command = EUploadImageCartRemoveSelected;
			txt = qtn_upload_images_remove_selected;
			},
		MENU_ITEM
			{
			command = EUploadImageCartRemoveAll;
			txt = qtn_upload_images_remove_all;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}			
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_private_questions_menubar
//   Menubar for 'Private Questions' view
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_private_questions_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_private_questions_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_private_questions_menu
//   Menu for 'Menu' ('Private Questions')
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_private_questions_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EPQuestionsRefresh;
			txt = qtn_aaf_refresh;
			},
		MENU_ITEM
			{
			command = EPQuestionDetails;
			txt = qtn_aaf_item_details;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}			
		};
	}
	
// -----------------------------------------------------------------------------
//
//   r_aaf_my_questions_menubar
//   Menubar for 'Responses' view
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_my_questions_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_my_questions_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_my_questions_menu
//   Menu for 'Menu' ('Responses')
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_my_questions_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EMyQuestionsRefresh;
			txt = qtn_aaf_refresh;
			},
		MENU_ITEM
			{
			command = EMyQuestionDetails;
			txt = qtn_aaf_details;
			},
		MENU_ITEM
			{
			command = EMyQuestionComments;
			txt = qtn_aaf_comments;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}			
		};
	}
	
// -----------------------------------------------------------------------------
//
//   r_aaf_my_questions_menubar
//   Menubar for 'Responses' view
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_question_comments_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_question_comments_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_question_comments_menu
//   Menu for 'Menu' ('Comments')
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_question_comments_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EQuestionCommentsRefresh;
			txt = qtn_aaf_refresh;
			},
		MENU_ITEM
			{
			command = EQuestionCommentDetails;
			txt = qtn_aaf_details;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}			
		};
	}
	
// -----------------------------------------------------------------------------
//
//   r_aaf_camera_menubar
//   Menubar for 'Make a photo'
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_BAR r_aaf_camera_view_menubar
	{
	titles =
		{
		MENU_TITLE { menu_pane = r_aaf_camera_menu; }
		};
	}

// -----------------------------------------------------------------------------
//
//   r_aaf_camera_menu
//   Menu for 'Menu' ('Make a photo')
//
// -----------------------------------------------------------------------------
//
RESOURCE MENU_PANE r_aaf_camera_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EMakePhotoCapture;
			txt = qtn_aaf_make_photo_capture;
			},
		MENU_ITEM
			{
			command = EMakePhotoZoomIn;
			txt = qtn_aaf_make_photo_zoomin;
			},
		MENU_ITEM
			{
			command = EMakePhotoZoomOut;
			txt = qtn_aaf_make_photo_zoomout;
			},
		MENU_ITEM
			{
			command = EMakePhotoNew;
			txt = qtn_aaf_make_photo_new;
			},
		MENU_ITEM
			{
			command = EMakePhotoSave;
			txt = qtn_aaf_make_photo_save;
			},
		MENU_ITEM
			{
			command = EMakePhotoResolution;
			cascade = r_resolutions_list_menu;
			txt = qtn_aaf_make_photo_resolution;
			},
		MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}			
		};
	}
	
RESOURCE MENU_PANE r_resolutions_list_menu
	{
	items =
		{
		// added the new 'Menu' menu command here
		MENU_ITEM
			{
			command = EMakePhotoRes640x480;
			flags = 0x0010 ;
			txt = qtn_aaf_make_photo_res1;
			},
		MENU_ITEM
			{
			command = EMakePhotoRes480x320;
			txt = qtn_aaf_make_photo_res2;
			},
		MENU_ITEM
			{
			command = EMakePhotoRes320x240;
			flags = 0x0040 ;
			txt = qtn_aaf_make_photo_res3;
			}
		};
	}
	
//----------------------------------------------------
//
//	  Generic data view dialog
//
//----------------------------------------------------
RESOURCE DIALOG r_dataview_dialog
  {
	flags = 0x001  | 0x1000  | 0x040  | 0x200  | 0x2000 ;
	buttons = 0x8cc0012 ;
  }
  
//----------------------------------------------------
//   
//    Ask a question view page items
//
//----------------------------------------------------
//
RESOURCE DIALOG r_ask_question_form_dialog
{
	flags = 0x200  | 0x1000  | 0x001  | 1  | 4 ;
	buttons = 0x277c400f ;
	form = r_ask_question_form;	
}

RESOURCE FORM r_ask_question_form
	{
	flags =  8 ;
	items =
		{
			DLG_LINE
			{
				type = EEikCtEdwin;
				prompt = qtn_question_text_label;
				id = EQuestionText;
				itemflags = 0x10  | 0x20 ;
				control = EDWIN 
					{
					width = 50; 
					maxlength = 256;
					lines = 5;
					};
			},
	        DLG_LINE
            {
            type = EAknCtPopupFieldText;
            prompt = qtn_question_duration_label;
            id = EQuestionDuration;
            itemflags = 0x10 ;
            control = POPUP_FIELD_TEXT
                {
                textarray = r_question_duration_popup;
                popupfield = POPUP_FIELD {};
                active = 0;
                };
			},
			DLG_LINE
            {
            type = EAknCtPopupFieldText;
            prompt = qtn_question_private_label;
            id = EQuestionPrivate;
            itemflags = 0x10 ;
            control = POPUP_FIELD_TEXT
                {
                textarray = r_question_private_popup;
				popupfield = POPUP_FIELD {};
                active = 1;
                };
            },
            DLG_LINE
            {
            type = EAknCtPopupFieldText;
            prompt = qtn_question_type_label;
            id = EQuestionType;
            itemflags = 0x10 ;
            control = POPUP_FIELD_TEXT
                {
                textarray = r_question_type_popup;
                popupfield = POPUP_FIELD {};
                active = 0;
                };
			}            		
		};
}

RESOURCE ARRAY r_question_type_popup
    {
    items = 
        { 
        LBUF { txt = qtn_question_type_01;},
        LBUF { txt = qtn_question_type_02;},
        LBUF { txt = qtn_question_type_03;},
        LBUF { txt = qtn_question_type_04;}
        };
    }
   
 RESOURCE ARRAY r_question_duration_popup
    {
    items = 
        { 
        LBUF { txt = qtn_question_duration_01;},
        LBUF { txt = qtn_question_duration_02;},
        LBUF { txt = qtn_question_duration_03;},
        LBUF { txt = qtn_question_duration_04;}
        };
    }
    
 RESOURCE ARRAY r_question_private_popup
    {
    items = 
        { 
        LBUF { txt = qtn_question_private_yes;},
        LBUF { txt = qtn_question_private_no;}
        };
    }
     
  
// Create the menu resource
RESOURCE MENU_BAR r_question_form_menubar
   {
   titles =
      {
      MENU_TITLE {menu_pane = r_question_form_menu; txt = "";}
      };
   }

RESOURCE MENU_PANE r_question_form_menu
   {
   items=
      {
      MENU_ITEM
			{
			command = EAskQuestionSubmit;
			txt = qtn_question_form_submit;
			},
	  MENU_ITEM
			{
			command = EResponses;
			txt = qtn_question_form_responses;
			},
	  MENU_ITEM
			{
			command = EPrivateQuestions;
			txt = qtn_question_form_pquestions;
			},
      MENU_ITEM
			{
			command = EHelp;
			txt = qtn_help;
			},
		MENU_ITEM
			{
			command = EAbout;
			txt = qtn_about;
			},
		MENU_ITEM
			{
			command = EAknSoftkeyExit;
			txt = qtn_aaf_exit;
			}			
      };
   }
   
// -----------------------------------------------------------------------------
//
// Login view username text field.
//
// -----------------------------------------------------------------------------
//
RESOURCE EDWIN r_login_box
{
	width = 20;
	lines = 1;
	maxlength = 20;
	flags = 0x00000080  | 0x00040000 ;
	default_input_mode = 0x1 ;
}

// -----------------------------------------------------------------------------
//
// Login view password text field.
//
// -----------------------------------------------------------------------------
//
RESOURCE SECRETED r_password_box
{ 
    num_letters = 20; 
} 

// -----------------------------------------------------------------------------
//
// Main view listbox control.
//
// -----------------------------------------------------------------------------
//
RESOURCE LISTBOX r_mainview_listbox
    {
    flags = ((0x0080 ) ) ;
    array_id = r_mainview_listbox_item_array;
    }

RESOURCE LISTBOX r_mainview_listbox_without_camera
    {
    flags = ((0x0080 ) ) ;
    array_id = r_mainview_listbox_item_array_without_camera;
    }
    
// -----------------------------------------------------------------------------
//
// Main view listbox items.
//
// -----------------------------------------------------------------------------
//
RESOURCE ARRAY r_mainview_listbox_item_array
    {
    items = 
        { 
        LBUF { txt = qtn_ask_question;},
        LBUF { txt = qtn_make_photo;},
        LBUF { txt = qtn_filebrowser;},
        LBUF { txt = qtn_upload_imagecart;},
        LBUF { txt = qtn_private_questions;},
        LBUF { txt = qtn_responses;},
        LBUF { txt = qtn_credentials;},
        LBUF { txt = qtn_invite_friend;}
        };
    }

RESOURCE ARRAY r_mainview_listbox_item_array_without_camera
    {
    items = 
        { 
        LBUF { txt = qtn_ask_question_without_camera;},
        LBUF { txt = qtn_filebrowser_without_camera;},
        LBUF { txt = qtn_upload_imagecart_without_camera;},
        LBUF { txt = qtn_private_questions_without_camera;},
        LBUF { txt = qtn_responses_without_camera;},
        LBUF { txt = qtn_credentials_without_camera;},
        LBUF { txt = qtn_invite_friend_without_camera;}
        };
    }
 
// -----------------------------------------------------------------------------
//
// Generally used waitnote dialog with 'Cancel' button
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_waitnote
{
	flags = (((0x200  | 0x040  | 0x2000  )  | 0x002 ) )  | 0x002 ;
	buttons = 0x8cc001c ;
	items =
	{
	  DLG_LINE
	  {
		  type = EAknCtNote;
		  id = EGeneralNote;
		  control = AVKON_NOTE
		  {
			  layout = EWaitLayout;
			  animation = 0x8cc0140 ;
		  };
	  }
	};
}

// -----------------------------------------------------------------------------
//
// Generally used waitnote dialog with empty buttons
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_waitnote_blocking
{
	flags = (((0x200  | 0x040  | 0x2000  )  | 0x002 ) )  | 0x002 ;
	buttons = 0x8cc0010 ;
	items =
	{
	  DLG_LINE
	  {
		  type = EAknCtNote;
		  id = EGeneralNote;
		  control = AVKON_NOTE
		  {
			  layout = EWaitLayout;
			  singular_label = qtn_aaf_general_waitnote;
			  animation = 0x8cc0140 ;
		  };
	  }
	};
}

// -----------------------------------------------------------------------------
//
// Generally used failure dialog.
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_dialog_failure
{
	flags = ((0x200  | 0x040  | 0x2000  )  | 0x400000 ) ;
	buttons = 0x8cc0010 ;
	items =
	{
		DLG_LINE
		{
			type = EAknCtNote;
			id = EGeneralNote;
			control = AVKON_NOTE
			{
				layout = EGeneralLayout;
				animation = 0x8cc013a ;
			};
		}
	};
}

// -----------------------------------------------------------------------------
//
// General query dialog
//
// -----------------------------------------------------------------------------
//
RESOURCE AVKON_LIST_QUERY r_query_dialog
{
   flags = ((0x001 | 0x200  | 0x040  | 0x2000 ) ) ;
   softkeys = 0x8cc0014 ;
   items =
   {
      AVKON_LIST_QUERY_DLG_LINE
      {
         control = AVKON_LIST_QUERY_CONTROL
         {
            listtype = EAknCtSinglePopupMenuListBox;
            listbox = AVKON_LIST_QUERY_LIST
            {
            };
            
            heading = qtn_aaf_phone_selection_query;
         };
      }
   };
}

// -----------------------------------------------------------------------------
//
// Filename query dialog
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_filename_query 
    { 
    flags = ((0x001 | 0x200  | 0x040  | 0x2000 ) ) ; 
    buttons = 0x8cc0014 ; 
    items = 
        { 
        DLG_LINE 
            { 
            type = EAknCtQuery; 
            id = EGeneralQuery; 
            control = AVKON_DATA_QUERY 
                { 
                layout = EDataLayout; 
                label = qtn_aaf_filesave_dialog;  // prompt text 
                control = EDWIN 
                    { 
						width = 5; 
						lines = 1; 
						maxlength = 255; 
                    }; 
                }; 
            } 
        }; 
    }
    
// -----------------------------------------------------------------------------
//
// Main view listbox items icons.
//
// -----------------------------------------------------------------------------
//
RESOURCE AKN_ICON_ARRAY r_mainview_icons 
    { 
    type = EAknIconArraySimple; 
    bmpfile = "\\resource\\apps\\aaf.mbm";
 
    icons = 
        {
        AKN_ICON 
            { 
            iconId = EMbmAafAsk;
            maskId = EMbmAafAskmask;
            },
        AKN_ICON
			{
			iconId = EMbmAafCamera;
			maskId = EMbmAafCameramask;
			},
        AKN_ICON 
            { 
            iconId = EMbmAafFolder;
            maskId = EMbmAafFoldermask;
            },
        AKN_ICON 
            { 
            iconId = EMbmAafCart;
            maskId = EMbmAafCartmask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafPrivate;
            maskId = EMbmAafPrivatemask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafQuestions;
            maskId = EMbmAafQuestionsmask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafSettings;
            maskId = EMbmAafSettingsmask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafSms;
            maskId = EMbmAafSmsmask;
            }
        }; 
    }
    
RESOURCE AKN_ICON_ARRAY r_mainview_icons_without_camera 
    { 
    type = EAknIconArraySimple; 
    bmpfile = "\\resource\\apps\\aaf.mbm";
 
    icons = 
        {
        AKN_ICON 
            { 
            iconId = EMbmAafAsk;
            maskId = EMbmAafAskmask;
            },
        AKN_ICON 
            { 
            iconId = EMbmAafFolder;
            maskId = EMbmAafFoldermask;
            },
        AKN_ICON 
            { 
            iconId = EMbmAafCart;
            maskId = EMbmAafCartmask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafPrivate;
            maskId = EMbmAafPrivatemask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafQuestions;
            maskId = EMbmAafQuestionsmask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafSettings;
            maskId = EMbmAafSettingsmask;
            },
       AKN_ICON 
            { 
            iconId = EMbmAafSms;
            maskId = EMbmAafSmsmask;
            }
        }; 
    }
    
// -----------------------------------------------------------------------------
//
// File browser view listbox items icons.
//
// -----------------------------------------------------------------------------
//
RESOURCE AKN_ICON_ARRAY r_browserview_icons 
    { 
    type = EAknIconArraySimple; 
    bmpfile = "\\resource\\apps\\aaf.mbm";
 
    icons = 
        { 
        AKN_ICON 
            { 
            iconId = EMbmAafFolder;
            maskId = EMbmAafFoldermask;
            }, 
        AKN_ICON 
            { 
            iconId = EMbmAafFile;
            maskId = EMbmAafFilemask;
            },
        AKN_ICON 
            { 
            iconId = EMbmAafFolderup;
            maskId = EMbmAafFolderupmask;
            },
        AKN_ICON 
            { 
            iconId = EMbmAafPhone;
            maskId = EMbmAafPhonemask;
            },
         AKN_ICON 
            { 
            iconId = EMbmAafCard;
            maskId = EMbmAafCardmask;
            }            
        }; 
    }
    
// -----------------------------------------------------------------------------
//
// Upload images cart view listbox items icons.
//
// -----------------------------------------------------------------------------
//
RESOURCE AKN_ICON_ARRAY r_cartview_icons 
    { 
    type = EAknIconArraySimple; 
    bmpfile = "\\resource\\apps\\aaf.mbm";
 
    icons = 
        { 
        AKN_ICON 
            { 
            iconId = EMbmAafFile;
            maskId = EMbmAafFilemask;
            }
        }; 
    }

RESOURCE AKN_ICON_ARRAY r_pquestionsview_icons 
    { 
    type = EAknIconArraySimple; 
    bmpfile = "\\resource\\apps\\aaf.mbm";
 
    icons = 
        { 
        AKN_ICON 
            { 
            iconId = EMbmAafSelected;
            maskId = EMbmAafSelectedmask;
            }
        }; 
    }

RESOURCE AKN_ICON_ARRAY r_userquestionsview_icons 
    { 
    type = EAknIconArraySimple; 
    bmpfile = "\\resource\\apps\\aaf.mbm";
 
    icons = 
        { 
        AKN_ICON 
            { 
            iconId = EMbmAafUnselected;
            maskId = EMbmAafUnselectedmask;
            }
        }; 
    }
    
// -----------------------------------------------------------------------------
//
// About dialog resource.
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_about_query_dialog
	{
	flags = ((0x001 | 0x200  | 0x040  | 0x2000 ) )  | 0x4000  | 0x8000 ;
	buttons = 0x8cc0012 ;
	items=
		{
		DLG_LINE 
			{
			type = EAknCtPopupHeadingPane;
			id = EAknMessageQueryHeaderId;
			itemflags = 0x80 ;
			control = AVKON_HEADING
				{
				};
			},
		DLG_LINE
			{
			type = EAknCtMessageQuery;
			id = EAknMessageQueryContentId;
			control = AVKON_MESSAGE_QUERY
				{
				};
			}
		};
	}

// -----------------------------------------------------------------------------
//
// Privacy statement dialog resource.
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_privstmt_dialog {
	flags = (0x001 | 0x200  | 0x040  | 0x2000 ) ;
	buttons = 0x8cc0024 ;
	items = {
		DLG_LINE {
			type = EAknCtPopupHeadingPane;
			id = EAknMessageQueryHeaderId;
			itemflags = 0x80 ;
			control = AVKON_HEADING {
				label = qtn_privacy_statement_header;
			};
		},
		DLG_LINE {
			type = EAknCtMessageQuery;
			id = EAknMessageQueryContentId;
			control = AVKON_MESSAGE_QUERY {
			};
		}
	};
}	

// -----------------------------------------------------------------------------
//
// File properties dialog resource.
//
// -----------------------------------------------------------------------------
//
RESOURCE DIALOG r_fileproperties_dialog
 {
	flags = (0x001 | 0x200  | 0x040  | 0x2000 ) ;
	buttons = 0x8cc0012 ;
	items = {
		DLG_LINE {
			type = EAknCtPopupHeadingPane;
			id = EAknMessageQueryHeaderId;
			itemflags = 0x80 ;
			control = AVKON_HEADING {
				label = qtn_privacy_statement_header;
			};
		},
		DLG_LINE {
			type = EAknCtMessageQuery;
			id = EAknMessageQueryContentId;
			control = AVKON_MESSAGE_QUERY {
			};
		}
	};
}	

// -----------------------------------------------------------------------------
//
// Resources for messages.
//
// -----------------------------------------------------------------------------
//
RESOURCE TBUF32 r_about_dialog_title { buf=qtn_about_dialog_title; }
RESOURCE TBUF r_about_dialog_text { buf=qtn_about_dialog_text; }
RESOURCE TBUF r_privacy_statement_filename { buf = qtn_privacy_statement_filename; }
RESOURCE TBUF r_settings_filename { buf = qtn_settings_filename; }
RESOURCE TBUF r_login_waitnote { buf = qtn_aaf_login_waitnote;}
RESOURCE TBUF r_login_failure_note { buf = qtn_aaf_login_failure_dialog; }
RESOURCE TBUF r_login_empty_fields_note { buf = qtn_aaf_login_empty_credentials; }
RESOURCE TBUF r_sms_empty_phone_note { buf = qtn_aaf_empty_phone_number; }
RESOURCE TBUF r_connection_not_ready { buf = qtn_aaf_connection_not_ready; }
RESOURCE TBUF r_upload_waitnote { buf = qtn_aaf_upload_waitnote; }
RESOURCE TBUF r_pquestions_waitnote { buf = qtn_aaf_pquestions_waitnote; }
RESOURCE TBUF r_upload_failure_note { buf = qtn_aaf_upload_failure_dialog; }
RESOURCE TBUF r_pquestions_failure_note { buf = qtn_aaf_pquestions_failure_dialog; }
RESOURCE TBUF r_myquestions_waitnote { buf = qtn_aaf_myquestions_waitnote; }
RESOURCE TBUF r_myquestions_failure_note { buf = qtn_aaf_myquestions_failure_dialog; }
RESOURCE TBUF r_comments_waitnote { buf = qtn_aaf_comments_waitnote; }
RESOURCE TBUF r_comments_failure_note { buf = qtn_aaf_comments_failure_dialog; }
RESOURCE TBUF r_upload_failure_credentials { buf = qtn_aaf_upload_failure_credentials; }
RESOURCE TBUF r_upload_noimage_note { buf = qtn_aaf_upload_no_image; }

RESOURCE TBUF r_empty_question_note { buf = qtn_aaf_empty_question_text; }
RESOURCE TBUF r_empty_custom_responsea_note { buf = qtn_aaf_empty_custom_responsea; }
RESOURCE TBUF r_empty_custom_responseb_note { buf = qtn_aaf_empty_custom_responseb; }

// -----------------------------------------------------------------------------
//
// Resources for labels etc
//
// -----------------------------------------------------------------------------
//
RESOURCE TBUF r_login_label { buf = qtn_aaf_login_label; }
RESOURCE TBUF r_password_label { buf = qtn_aaf_password_label; }
RESOURCE TBUF r_filebrowser_phonememory { buf = qtn_filebrowser_phonememory; }
RESOURCE TBUF r_filebrowser_cardmemory { buf = qtn_filebrowser_cardmemory; }
RESOURCE TBUF r_select_folder_dialog { buf = qtn_select_folder_dialog; }
RESOURCE TBUF r_back { buf = qtn_aaf_back; }
RESOURCE TBUF r_sms_body_text { buf = qtn_aaf_sms_text; }
RESOURCE TBUF r_responsea_label { buf = qtn_question_response_a; }
RESOURCE TBUF r_responseb_label { buf = qtn_question_response_b; }
RESOURCE TBUF r_empty_image_cart_text { buf = qtn_aaf_empty_image_cart; }
RESOURCE TBUF r_no_private_questions_text { buf = qtn_aaf_empty_private_questions_view; }
RESOURCE TBUF r_no_own_questions_text { buf = qtn_aaf_empty_own_questions_view; }
