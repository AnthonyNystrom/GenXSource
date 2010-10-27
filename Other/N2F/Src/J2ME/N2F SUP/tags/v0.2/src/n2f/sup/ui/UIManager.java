package n2f.sup.ui;

import java.util.Stack;

import javax.microedition.io.file.FileSystemListener;
import javax.microedition.lcdui.AlertType;

import n2f.sup.common.AbstractErrorManager;
import n2f.sup.common.Deallocatable;
import n2f.sup.common.ErrorEvent;
import n2f.sup.common.ErrorListener;
import n2f.sup.common.Resoursable;
import n2f.sup.core.Engine;
import n2f.sup.utils.Console;



public class UIManager extends AbstractErrorManager implements Deallocatable, FileSystemListener, ErrorListener {

	private static UIManager INSTANCE = null;
	private Stack uiStack = new Stack();   
	private String currentID;
	private AbstractWindow currentUI = null; //current UI
	private Resoursable resoursable; 

//	public static final String SCREEN_WAIT = LocaleUI.FORM_WAIT_CAPTION;
	public static final String SCREEN_CARD = LocaleUI.SCREEN_PROCEED;	
	
	public static final String SCREEN_FILE_SYSTEM = LocaleUI.FORM_FILE_SELECTOR_CAPTION;
//	public static final String SCREEN_IMAGE_SAVER = LocaleUI.FORM_FILE_SAVER_CAPTION;
	public static final String SCREEN_PREVIEW = LocaleUI.FORM_PREVIEW_CAPTION;
//    public static final String SCREEN_CAMERA = LocaleUI.FORM_FOCUS_CAPTION;	
//	public static final String SCREEN_SPLASH = LocaleUI.FORM_SPLASH_CAPTION;
    public static final String SCREEN_START = LocaleUI.FORM_START_CAPTION;
//    public static final String SCREEN_QUESTION = LocaleUI.FORM_ASK_A_QUESTION_CAPTION;
//    public static final String SCREEN_DURATION = LocaleUI.FORM_OPTIONS_CAPTION;
//    public static final String SCREEN_ANSWER_OPTIONS = LocaleUI.FORM_INPUT_SCREEN_CAPTION;
//    public static final String SCREEN_RESPONSES = LocaleUI.FORM_RESPONSE_CAPTION;
//    public static final String SCREEN_RESPONSE_DETAILS = LocaleUI.FORM_ANSWER_DETAILS_CAPTION;
//    public static final String SCREEN_RESPONSE_COMMENTS = LocaleUI.FORM_COMMENTS_CAPTION;
    public static final String SCREEN_SETTINGS = LocaleUI.FORM_SETTINGS_CAPTION;
    public static final String SCREEN_CONSOLE = "Console";
//    public static final String SCREEN_CAMERA_SETTINGS = LocaleUI.FORM_CAMERA_SETTINGS_CAPTION; 
//    public static final String SCREEN_PRIVATE_QUESTIONS = LocaleUI.FORM_PRIVATE_QUESTIONS_CAPTION;
//    public static final String SCREEN_SAVE = LocaleUI.FORM_SAVE;
    //User Interfase for SUP
    public static final String SCREEN_SUP_START = "START";    
    public static final String SCREEN_SUP_SPLASH = "FORM_SUP_SPLASH";    
    public static final String SCREEN_SUP_CARD = "FORM_SUP_CARD";  
    public static final String SCREEN_SUP_BROWSER = "FORM_SUP_BROWSER";
    public static final String SCREEN_SUP_SETTINGS = "FORM_SUP_SETTINGS";    

    public void setDefaultID(String id){
    	this.defaultID = id;
    }
    
    private String defaultID = SCREEN_START;
	
	private UIManager() {
		uiStack = new Stack();
	}

	/**
	 * Returns an instance of UIManager class
	 *
	 * @return
	 */
	public static UIManager getInstance() {
		if (INSTANCE == null) {
			INSTANCE = new UIManager();	
		}
		return INSTANCE;
	}

	/* (non-Javadoc)
	 * @see com.musiwave.de.ui.UIManager#show()
	 */
	public void show(String id) {
		show(id, true);
	}
	
	public void show(String id, boolean add2stack) {
//		System.out.println("SHOW:"+id);
		boolean clean = true;
//		if (id == SCREEN_CAMERA) {
//			currentUI = new CameraFocusScreen(id, getResoursable());
//		} else if (id == SCREEN_FILE_SYSTEM) {
//			currentUI = new FileSelectorScreen2(id, getResoursable());
////			currentUI = new FileSelectorUI(id, getResoursable());
//		} else if (id == SCREEN_IMAGE_SAVER) {
//			currentUI = new FileSaverScreen(id, getResoursable());
//		} else if (id == SCREEN_SPLASH) {
//			currentUI = new SplashScreen(id, null);
//			add2stack = false;
//			clean = false;
//		} else if (id == SCREEN_START) {
//			currentUI = new StartScreen2(id, getResoursable());
//		} else if (id == SCREEN_QUESTION) {
//			currentUI = new AskAQuestionScreen(id, getResoursable());
//		} else if (id == SCREEN_DURATION) {
//			currentUI = new OptionsScreen(id, getResoursable());
//		} else if (id == SCREEN_ANSWER_OPTIONS) {
//			currentUI = new InputScreen(id, getResoursable());
//		} else if (id == SCREEN_WAIT) {
//			currentUI = new WaitScreen(id, getResoursable());
//		} else if (id == SCREEN_RESPONSES) {
//			currentUI = new QuestionListScreen(id, getResoursable());
//		} else 
		if (id == SCREEN_CONSOLE) {
			showConsole();
			return;
//		} else if (id == SCREEN_RESPONSE_DETAILS) {
//			currentUI = new AnswerDetailsScreen(id, getResoursable());
//		} else if (id == SCREEN_RESPONSE_COMMENTS) {
//			currentUI = new CommentsByUserScreen(id, getResoursable());
		} else if (id == SCREEN_SETTINGS) {
			currentUI = new SettingsScreen(id, getResoursable());
//		} else if (id == SCREEN_CAMERA_SETTINGS) {
//			currentUI = new CameraSettingsScreen(id, getResoursable());
		} else if (id == SCREEN_PREVIEW ){
			currentUI = new PreviewScreen(id, getResoursable());
//		} else if (id == SCREEN_PRIVATE_QUESTIONS ){
//			currentUI = new PrivateQuestionListScreen(id, getResoursable());
//		} else if (id == SCREEN_CARD){
//			currentUI = new CartScreen(id, getResoursable());			
//		} else if (id == SCREEN_SAVE){
//			currentUI = new SaveScreen(id, getResoursable());
		} else if (id == SCREEN_SUP_BROWSER){
			currentUI = new FileBrowserScreen(id, getResoursable());			
		} else if (id == SCREEN_SUP_CARD){
			currentUI = new SUPCartScreen(id, getResoursable());			
		} else if (id == SCREEN_SUP_SPLASH){
			currentUI = new SUPSplash(id, getResoursable());
		} else if (id == SCREEN_SUP_SETTINGS){
			currentUI = new SUPSettingsScreen(id, getResoursable());
		} else if (id == SCREEN_SUP_START) {
			currentUI = new SUPStartScreen(id, getResoursable());
		} else {
			throw new IllegalArgumentException("UNKNOWN UI ID");
		}
		if (currentUI != null) {
			show(currentUI, add2stack, clean);
		}
	}

	public void showPrevious() {
		if (uiStack.empty())
			showDefault();
		else
			show((String) uiStack.pop(), false);
	}

	public void showCurrent() {
		if (uiStack.empty())
			showDefault();
		else
			show(currentUI, false, true);
	}

	public void showConsole() {
		show(Console.getInstance(), true, false);
	}
	
//	public String getPreviousUI() {
//		return uiStack.empty() ? defaultID : (String) uiStack.peek();
//	}
	
	public void cleanStack() {
		this.uiStack.removeAllElements();
	}
	
	private void cleanResources() {
		Engine.getEngine().disposePreviewCache();
//		CustomItemCacher.removeAll();
	}
	
	private void show(AbstractWindow ui, boolean add2stack, boolean clean) {
		if (clean){
			cleanResources();			
		}
		
		ui.show();
		if (add2stack && currentID != null) {
			uiStack.push(currentID);
		}

		currentID = ui.getId();
	}
	
	public void showDefault() {		
		show(defaultID);
	}
	
	public void showAlert(String title, String message, AlertType alertType){
	}
	
	public void showAlert(String message, AlertType alertType) {
		showAlert(null, message, alertType);
	}
	
	
	/* (non-Javadoc)
	 * @see com.musiwave.de.Deallocatable#free()
	 */
	public void free() {
		INSTANCE = null;
	}

	/* (non-Javadoc)
	 * @see com.musiwave.de.listener.ErrorListener#actionPerformed(com.musiwave.de.listener.ErrorEvent)
	 */
	public void actionPerformed(int errorType) {
		showAlert(null, null, null);
	}
	
//	/**
//	 * @return Returns the defaultUI.
//	 */
//	public String getDefaultID() {
//		return defaultID;
//	}

	public String getCurrentID() {
		return currentID;
	}

	public AbstractWindow getCurrentUI() {
		return currentUI;
	}
	
	public void rootChanged(int event, String root) 
	{
		Engine.getEngine().openInit(getCurrentID()==SCREEN_FILE_SYSTEM ||
//				getCurrentID() == SCREEN_IMAGE_SAVER
				getCurrentID() == SCREEN_SUP_BROWSER
				? getCurrentUI(): null);
	}

	public void actionPerformed(ErrorEvent errorEvent) 
	{
		String text = null;
		if (getCurrentUI() != null) {
//			if (ErrorEvent.NEW_REPLY == errorEvent.getErrorId()) {
//				if (getCurrentID() != SCREEN_RESPONSE_COMMENTS && getCurrentID() != SCREEN_RESPONSE_DETAILS) {
//					text = getLocalizedText(LocaleUI.REPLY_READY);
//					getCurrentUI().showAlert(text, AbstractWindow.COMMAND_REPLIES, AbstractWindow.COMMAND_CANCEL, getCurrentUI());
//				}
//			} else if (ErrorEvent.NEW_PRIVATE_QUESTION == errorEvent.getErrorId()) {
//				if (getCurrentID() != SCREEN_PRIVATE_QUESTIONS) {
//					text = getLocalizedText(LocaleUI.QUESTION_READY);
//					getCurrentUI().showAlert(text, AbstractWindow.COMMAND_QUESTIONS, AbstractWindow.COMMAND_CANCEL, getCurrentUI());
//				}
//			} else {
				switch (errorEvent.getErrorId()) {
//				case ErrorEvent.MEDIA_FAILURE:
//					text = getLocalizedText(LocaleUI.ERROR_CAMERA_TEXT);
//					break;
				case ErrorEvent.OPERATION_FAILED:
					text = getLocalizedText(LocaleUI.ERROR_GENERAL_TEXT);
					break;
				case ErrorEvent.INIT:
				case ErrorEvent.OPEN:
					text = getLocalizedText(LocaleUI.ERROR_OPENINIT_TEXT);
					break;
				case ErrorEvent.SAVE_DATA:
					text = getLocalizedText(LocaleUI.ERROR_SAVE_TEXT);
					break;
				case ErrorEvent.SCALE_IMAGE:
					text = getLocalizedText(LocaleUI.ERROR_SCALE_TEXT);
					break;				
//				case ErrorEvent.ATTACH_PHOTO:
//					text = getLocalizedText(LocaleUI.ERROR_ATTACH_TEXT);
//					break;
//				case ErrorEvent.COMPLETE_QUESTION:
//					text = getLocalizedText(LocaleUI.ERROR_COMPLETE_TEXT);
//					break;
//				case ErrorEvent.SUBMIT_QUESTION:
//					text = getLocalizedText(LocaleUI.ERROR_SUBMIT_TEXT);
//					break;
//				case ErrorEvent.GET_MYAAFCOMMENTS:
//				case ErrorEvent.GET_NEWCOMMENTS_QUEST:
//				case ErrorEvent.GET_NEWCOMMENTS:
//					text = getLocalizedText(LocaleUI.ERROR_COMMENTS_TEXT);
//					break;
//				case ErrorEvent.GET_MYAAFPRIVATE_QUEST:
//				case ErrorEvent.GET_MYAAFQUESTIONS:
//					text = getLocalizedText(LocaleUI.ERROR_QUESTIONS_TEXT);
//					break;
//				case ErrorEvent.GET_MYAAFRESPONSE_DETAILS:
//					text = getLocalizedText(LocaleUI.ERROR_DETAILS_TEXT);
//					break;
				case ErrorEvent.GET_CREDENTIALS:
					text = getLocalizedText(LocaleUI.ERROR_CREDENTIALS_TEXT);
					break;
				case ErrorEvent.UPLOAD_PHOTO:
					text = getLocalizedText(LocaleUI.ERROR_UPLOAD_TEXT);
					break;
				default:
					text = errorEvent.getExplanation();
					break;
				}
				getCurrentUI().showAlert(text, AlertType.CONFIRMATION, getCurrentUI());
//			}
		}
	}

	private Resoursable getResoursable() {
		return resoursable;
	}

	public void setResoursable(Resoursable resoursable) {
		this.resoursable = resoursable;
	}

	private String getLocalizedText(String key) {
		String retStr = null;
		if ((retStr = getResoursable().getLocale(key)) == null) {
			System.out.println("KEY=" + key);
			retStr = "UNKNOWN " + key;
		}
		return retStr;
	}
}
