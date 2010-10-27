package App;

public class Const
{
    public static final boolean DEBUG = true;

//#ifdef MOTOROLA
//#     public static final int KEY_POSITIVE = -21;
//#     public static final int KEY_NEGATIVE = -22;
//#     public static final int KEY_OK = -20;
//#     public static final int KEY_UP = -1;
//#     public static final int KEY_DOWN = -6;
//#     public static final int KEY_LEFT = -2;
//#     public static final int KEY_RIGHT = -5;
//#     public static final int KEY_STAR = 42;
//#     public static final int KEY_POUND = 35;
//#elifdef NOKIA
//#     public static final int KEY_POSITIVE = -6;
//#     public static final int KEY_NEGATIVE = -7;
//#     public static final int KEY_OK = -5;
//#     public static final int KEY_UP = -1;
//#     public static final int KEY_DOWN = -2;
//#     public static final int KEY_LEFT = -3;
//#     public static final int KEY_RIGHT = -4;
//#     public static final int KEY_STAR = 42;
//#     public static final int KEY_POUND = 35;
//#elifdef BLACKBERRY
//#     public static final int KEY_POSITIVE = 268566528;
//#     public static final int KEY_NEGATIVE = 1769472;
//#     public static final int KEY_OK = 655360;
//#     public static final int KEY_UP = 1;
//#     public static final int KEY_DOWN = 6;
//#     public static final int KEY_LEFT = 2;
//#     public static final int KEY_RIGHT = 5;
//#     
//#     public static final int KEY_DEL = 524288;
//#     public static final int KEY_STAR = 1310720;
//#     public static final int KEY_POUND = 17104896;
//#     public static final int KEY_SPACE = 2097152;
//#     public static final int KEY_ALT_ON = 16842753;
//#     public static final int KEY_ALT_OFF = 16842752;
    //#ifdef _8120
//#         public static final int KEY_Q = 5308416;
//#         public static final int KEY_E = 4521984;
//#         public static final int KEY_T = 5505024;
//#         public static final int KEY_U = 5570560;
//#         public static final int KEY_O = 5177344;
//#         public static final int KEY_A = 4259840;
//#         public static final int KEY_D = 4456448;
//#         public static final int KEY_G = 4653056;
//#         public static final int KEY_J = 4849664;
//#         public static final int KEY_L = 4980736;
//#         public static final int KEY_Z = 5898240;
//#         public static final int KEY_C = 4390912;
//#         public static final int KEY_B = 4325376;
//#         public static final int KEY_M = 5046272;
   //#elifdef _8320
//#         public static final int KEY_Q = 5308416;
//#         public static final int KEY_W = 5701632;
//#         public static final int KEY_E = 4521984;
//#         public static final int KEY_R = 5373952;
//#         public static final int KEY_T = 5505024;
//#         public static final int KEY_Y = 5832704;
//#         public static final int KEY_U = 5570560;
//#         public static final int KEY_I = 4784128;
//#         public static final int KEY_O = 5177344;
//#         public static final int KEY_P = 5242880;
//#         public static final int KEY_A = 4259840;
//#         public static final int KEY_S = 5439488;
//#         public static final int KEY_D = 4456448;
//#         public static final int KEY_F = 4587520;
//#         public static final int KEY_G = 4653056;
//#         public static final int KEY_H = 4718592;
//#         public static final int KEY_J = 4849664;
//#         public static final int KEY_K = 4915200;
//#         public static final int KEY_L = 4980736;
//#         public static final int KEY_Z = 5898240;
//#         public static final int KEY_X = 5767168;
//#         public static final int KEY_C = 4390912;
//#         public static final int KEY_V = 5636096;
//#         public static final int KEY_B = 4325376;
//#         public static final int KEY_N = 5111808;
//#         public static final int KEY_M = 5046272;
//# 
   //#endif
//#  
//#endif
    
    public static final int KEY_0 = 48;
    public static final int KEY_1 = 49;
    public static final int KEY_2 = 50;
    public static final int KEY_3 = 51;
    public static final int KEY_4 = 52;
    public static final int KEY_5 = 53;
    public static final int KEY_6 = 54;
    public static final int KEY_7 = 55;
    public static final int KEY_8 = 56;
    public static final int KEY_9 = 57;
    
    
    public static final int MAX_CONTROLS = 20;
    public static final int MAX_SCREEN_STACK = 10;
    
//-------------------   Image constants   ---------------------
    static final int SPA_TP_DG = 0;
    static final int SPA_TP_CLIP = 1;
    static final int SPA_TP_SIMPLE = 2;
    static final int SPA_TYPE = SPA_TP_CLIP;
    static final boolean SPA_IS_ZRLE = true;
    static final byte SPA_MP_HFLIP = 1;
    static final byte SPA_MP_VFLIP = 2;
    static final byte SPA_MP_ROT90 = 4;
    
    //actions
    public static final int ACTION_NULL = Integer.MAX_VALUE;
    public static final int ACTION_GO = 0;
    public static final int ACTION_EXIT = ACTION_GO+1;
    public static final int ACTION_POPUP = ACTION_EXIT+1;
    public static final int ACTION_SELECT = ACTION_POPUP+1;
    public static final int ACTION_BACK = ACTION_SELECT+1;
    public static final int ACTION_HOMEURL = ACTION_BACK+1;
    public static final int ACTION_QUESTION = ACTION_HOMEURL+1;
    public static final int ACTION_QUOPTIONS = ACTION_QUESTION+1;
    public static final int ACTION_SHOWAB = ACTION_QUOPTIONS+1;
    public static final int ACTION_HIDEAB = ACTION_SHOWAB+1;
    public static final int ACTION_MAINMENU = ACTION_HIDEAB+1;
    public static final int ACTION_SETTINGS = ACTION_MAINMENU+1;
    public static final int ACTION_HIDEMESSAGE = ACTION_SETTINGS+1;
    public static final int ACTION_CAPTURE = ACTION_HIDEMESSAGE+1;
    public static final int ACTION_SHOWCAPTURE = ACTION_CAPTURE+1;
    public static final int ACTION_SHOWATTACH = ACTION_SHOWCAPTURE+1;
    public static final int ACTION_LOGIN = ACTION_SHOWATTACH + 1;
    public static final int ACTION_IMAGEVIEW = ACTION_LOGIN + 1;
    public static final int ACTION_SHOWATTACHFROMFILE = ACTION_IMAGEVIEW+1;
    public static final int ACTION_DELETECURRENT = ACTION_SHOWATTACHFROMFILE+1;
    public static final int ACTION_SAVETODRAFTS = ACTION_DELETECURRENT+1;
    public static final int ACTION_EDIT = ACTION_SAVETODRAFTS+1;
    public static final int ACTION_DRAFTS = ACTION_EDIT+1;
    public static final int ACTION_OUTBOX = ACTION_DRAFTS+1;
    public static final int ACTION_SAVETOOUTBOX = ACTION_OUTBOX+1;
    public static final int ACTION_SAVE = ACTION_SAVETOOUTBOX+1;
    public static final int ACTION_SEND = ACTION_SAVE+1;
    public static final int ACTION_REMIND = ACTION_SEND+1;
    public static final int ACTION_VIEW = ACTION_REMIND+1;
    public static final int ACTION_DASHBOARD = ACTION_VIEW+1;
    public static final int ACTION_GETSEND = ACTION_DASHBOARD+1;
    public static final int ACTION_SHOWTAG = ACTION_GETSEND+1;
    public static final int ACTION_SHOWINBOX = ACTION_SHOWTAG+1;
    public static final int ACTION_SHOWCOMMENT = ACTION_SHOWINBOX+1;
    public static final int ACTION_SHOWUPLOAD = ACTION_SHOWCOMMENT+1;
    public static final int ACTION_SHOWUPLOAD_ATTACHFROMFILE = ACTION_SHOWUPLOAD+1;
    public static final int ACTION_UPLOAD = ACTION_SHOWUPLOAD_ATTACHFROMFILE+1;
    public static final int ACTION_SHOWSTATUS = ACTION_UPLOAD+1;
    public static final int ACTION_SHOWUPLOAD_CAPTURE = ACTION_SHOWSTATUS+1;
            
    //skin
    public static final int SKIN_TYPE_1H = 0;
    public static final int SKIN_TYPE_3H = 1;
    
    public static final int COMMON_MARGIN = 4;
    
    //GUIImage
    public static final int GUIIMAGE_IMAGE = 0;
    public static final int GUIIMAGE_SPRITE = 1;
    
    //GUIText
    public static final int GUITEXT_MAXLEN = 256;
    public static final int GUITEXT_MAXPASSLEN = 256;
    public static final int GUITEXT_NONPRESSEDFRAMES = 15;
    public static final char CHAR_EMPTY = '%';
    public static final char CHAR_CURSOR = '|';
    public static final int GUITEXT_CURSOR_FLASH_FRAMES = 16;
    
    //camera
    public static final int THUMBNAIL_DY = 62;
    //public static final int THUMBNAIL_DY = 38;
    
    //stotage 
    public static final int QUESTION_MAXSIZE = 2048;
    
    //question options
    public static final int QUESTION_RESPONSE_YESNO = 1;
    public static final int QUESTION_RESPONSE_AB = 2;
    public static final int QUESTION_RESPONSE_RATE = 3;
    public static final int QUESTION_RESPONSE_MULTIPLE = 4;
    public static final int QUESTION_DURATION_3M = 1;
    public static final int QUESTION_DURATION_15M = 2;
    public static final int QUESTION_DURATION_50M = 3;
    public static final int QUESTION_DURATION_1D = 4;
    
    //drafts
    public static final int DRAFTS_HEADERS_LENGTH = 10;
    
    //network
    public static final int NETWORK_SLEEP = 50;
    
    // title 
    public static final int TITLE_SHOWTIME = 2000;
    
    //tag	
    public static final String BT_UUID = "57ad9d629fc011dc83140800200c9a66";
    public static final long TICKS_DOTNETINIT = 621355968000000000L;
    public static final long TAG_WAITTIME = 1*30*1000;
    public static final int TAG_BLOCKSIZE = 8;
}
