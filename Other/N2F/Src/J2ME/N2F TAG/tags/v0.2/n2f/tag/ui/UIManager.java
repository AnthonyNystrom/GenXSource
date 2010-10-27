package n2f.tag.ui;

import genetibase.java.microedition.componentmodel.Resources;

import java.util.Hashtable;
import java.util.Stack;

import javax.microedition.lcdui.AlertType;

import n2f.contacts.ui.ContactsScreen;
import n2f.sms.ui.SMSEditScreen;
import n2f.sms.ui.SMSPreviewScreen;
import n2f.tag.App;
import n2f.tag.core.AbstractErrorManager;
import n2f.tag.core.Deallocatable;
import n2f.tag.core.ErrorEvent;
import n2f.tag.core.ErrorListener;
import n2f.tag.debug.Console;

public class UIManager extends AbstractErrorManager implements ErrorListener, Deallocatable {

	private static UIManager INSTANCE = null;
	private Stack uiStack = new Stack();   
	private String currentID;
	private AbstractWindow currentUI = null; //current UI
	private Resources resoursable; 
	private Hashtable beans = new Hashtable();

	public static final String SCREEN_SETTINGS = "Settings";//LocaleUI.FORM_SETTINGS_CAPTION;
    public static final String SCREEN_MAIN = "N2F Tag";//LocaleUI.FORM_SETTINGS_CAPTION;
    public static final String SCREEN_CONSOLE = "Console";
    public static final String SCREEN_SPLASH = "Splash";
	public static final String SCREEN_CONTACTS = "Contacts";
	public static final String SCREEN_SMS = "Preview SMS";
	public static final String SCREEN_SMS_EDIT = "Edit SMS";


    private String defaultID = SCREEN_MAIN;
	
	private UIManager() {
		uiStack = new Stack();
		App.getCurrentApp().addToDeallocatableList(this);
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
		if (id == SCREEN_MAIN) {
			currentUI = new MainForm(id, getResoursable());
		} else if (id == SCREEN_CONSOLE) {
			showConsole();
			return;
		} else 	if (id == SCREEN_SETTINGS) {
			currentUI = new SettingsScreen(id, getResoursable());
		}  else if (id == SCREEN_SPLASH) {
			currentUI = new SplashScreen(id, null);
			add2stack = false;
			clean = false;
		} else if (id == SCREEN_CONTACTS) {
			currentUI = new ContactsScreen(id, getResoursable());
		} else if (id == SCREEN_SMS) {
			currentUI = new SMSPreviewScreen(id, getResoursable());
		} else if (id == SCREEN_SMS_EDIT) {
			currentUI = new SMSEditScreen(id, getResoursable());
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
	
	public void cleanStack() {
		this.uiStack.removeAllElements();
	}
	
	private void cleanResources() {
		uiStack.removeAllElements();
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
	
	/* (non-Javadoc)
	 * @see com.musiwave.de.Deallocatable#free()
	 */
	public void free() {
		getResoursable().free();
		uiStack.removeAllElements();
		INSTANCE = null;
	}

	/**
	 * @return Returns the defaultUI.
	 */
	public String getDefaultID() {
		return defaultID;
	}

	public String getCurrentID() {
		return currentID;
	}

	public AbstractWindow getCurrentUI() {
		return currentUI;
	}
	
	public void actionPerformed(ErrorEvent errorEvent) 
	{
		String text = null;
		if (getCurrentUI() != null) {
			switch (errorEvent.getErrorId()) {
			case ErrorEvent.UPLOAD_TAG:
				text = getResoursable().get("errorUploadTag");
				break;
			case ErrorEvent.OPERATION_FAILED:
				text = getResoursable().get("errorOperationFailed");
				break;
			case ErrorEvent.GET_BLOCKLIST:
				text = getResoursable().get("errorGetBlockList");
				break;
			case ErrorEvent.GET_TAG_ID:
				text = getResoursable().get("errorGetTag");
				break;
			case ErrorEvent.GET_ENCRYPTION:
				text = getResoursable().get("errorGetEncryptionKey");
				break;
			case ErrorEvent.GET_CREDENTIALS:
				text = getResoursable().get("errorGetCredentials");
				break;
			default:
				text = errorEvent.getExplanation();
				break;
			}
			
			if (text == null)
			{
			    text = "Not localized message.";
			}
			
			getCurrentUI().showAlert(text, AlertType.CONFIRMATION, getCurrentUI());
		}
	}

	private Resources getResoursable() {
		return resoursable;
	}

	public void setResoursable(Resources resoursable) {
		this.resoursable = resoursable;
	}

	public AbstractBean getBean( String key){
		return (AbstractBean)this.beans.get(key);
	}
	
	public void putBean(String key, AbstractBean bean) {
		this.beans.put(key, bean);
	} 
}
