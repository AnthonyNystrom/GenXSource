/* ------------------------------------------------
 * MainForm.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.ui;

import genetibase.java.microedition.componentmodel.Resources;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.ItemStateListener;

import n2f.tag.App;
import n2f.tag.core.Settings;
import n2f.tag.webservice.WebServiceInteractor;
import n2f.tag.wireless.IDeviceDiscovererListener;
import n2f.tag.wireless.TagKeeper;

/**
 * @author Alex Nesterov
 */
final class MainForm
	extends AbstractWindow
	implements CommandListener, ItemStateListener, IDeviceDiscovererListener
{
    /* ------ Methods.Public ------ */
    
    public void commandAction(Command command, Displayable displayable)
    {
	super.commandAction(command, displayable);
	if (command == _exitCommand)
	{
	    App currentApp = App.getCurrentApp();
	    currentApp.destroyApp(false);
	}
	else if (command == _synchronizeCommand)
	{
	    System.out.println("MainForm::Synchronize");
	    if (TagKeeper.getInstance().hasRecords())
		App.getCurrentApp().getWebServiceIteractor().uploadTagList(null);
	    else
		System.out.println("there's nothing to UPLOAD again");
	    
	}
	else if (command == _settingsCommand)
	{
	    UIManager.getInstance().show(UIManager.SCREEN_SETTINGS);
	}
	else if  (command == Alert.DISMISS_COMMAND)
	{
	    refresh();
	}
    }
    
    /**
     * Listens to changes on MainForm items.
     */
    public void itemStateChanged(Item item)
    {
	if (item == _quickSettingsGroup)
	{
	    setVisible(isVisible());
	    setTaggingOn(isTaggingOn());
	}
	else if (item == _synchronizeGroup)
	{
	    setSynchronizationMode(getSynchronizationMode());
	}
    }
    
    public void saveSettings()
    {
	Settings settings = App.getCurrentApp().getSettings();
	
	settings.setSynchronize(getSynchronizationMode());
	settings.setTaggingOn(isTaggingOn());
	settings.setVisible(isVisible());
    }
    
    /* ------ Methods.Private ------ */
    
    private ChoiceGroup createQuickSettingsGroup(Resources resources)
    {
	String[] quickOptions = new String[]
	{
	    resources.get("visibleOption")
	    , resources.get("taggingOnOption")
	};
	
	ChoiceGroup quickSettingsGroup = new ChoiceGroup(
		resources.get("quickSettingsGroup")
		, ChoiceGroup.MULTIPLE
		, quickOptions
		, null
		);
	
	boolean isVisible = App.getCurrentApp().getSettings().getVisible();
	quickSettingsGroup.setSelectedIndex(0, isVisible);
	
	boolean isTaggingOn = App.getCurrentApp().getSettings().getTaggingOn();
	quickSettingsGroup.setSelectedIndex(1, isTaggingOn);
	
	return quickSettingsGroup;
    }
    
    private ChoiceGroup createSynchronizeGroup(Resources resources)
    {
	String[] syncOptions = new String[]
	{
	    resources.get("every5minOption")
	    , resources.get("everyHourOption")
	    , resources.get("everyDayOption")
	    , resources.get("manuallyOption")
	};
	
	ChoiceGroup synchronizeGroup = new ChoiceGroup(
		resources.get("synchronizeGroup")
		, ChoiceGroup.EXCLUSIVE
		, syncOptions
		, null
		);
	
	int mode = App.getCurrentApp().getSettings().getSynchronize();
	synchronizeGroup.setSelectedIndex(mode, true);
	setSynchronizationMode(mode);
	
	return synchronizeGroup;
    }
    
    public void discovererCannotStartTaggingService()
    {
	_quickSettingsGroup.setSelectedIndex(0, false);
	setVisible(false);
	showCannotStartTaggingServiceAlert();
    }
    
    public void discovererCannotStartDeviceSearch()
    {
	_quickSettingsGroup.setSelectedIndex(1, false);
	setTaggingOn(false);
	showCannotStartDeviceSearchAlert();
    }
    
    private Form form;
    
    private void initializeComponent()
    {
	Resources resources = getResoursable();
	this.form = new Form("N2F TAG");
	
	_settingsCommand = new Command(resources.get("settingsCommand"), Command.SCREEN, 50);
	form.addCommand(_settingsCommand);
	
	if (App.getCurrentApp().getWebServiceIteractor().getEncryptionKey() !=null)
	{
	    _quickSettingsGroup = createQuickSettingsGroup(resources);
	    form.append(_quickSettingsGroup);
	    
	    _synchronizeGroup = createSynchronizeGroup(resources);
	    form.append(_synchronizeGroup);
	}
	addConsole(form);
	
	_exitCommand = new Command(resources.get("exitCommand"), Command.EXIT, 100);
	form.addCommand(_exitCommand);
	
	form.setItemStateListener(this);
	form.setCommandListener(this);
    }
    
    private int getSynchronizationMode()
    {
	if (_synchronizeGroup != null)
	{
	    boolean[] flags = new boolean[_synchronizeGroup.size()];
	    _synchronizeGroup.getSelectedFlags(flags);
	    
	    for (int i = 0; i < flags.length; i++)
	    {
		if (flags[i])
		{
		    return i;
		}
	    }
	}
	else
	{
	    return App.getCurrentApp().getSettings().getSynchronize();
	}
	
	return 0;
    }
    
    private boolean isTaggingOn()
    {
	if (_quickSettingsGroup != null)
	{
	    boolean[] flags = new boolean[_quickSettingsGroup.size()];
	    _quickSettingsGroup.getSelectedFlags(flags);
	    return flags[1];
	}
	else
	{
	    return App.getCurrentApp().getSettings().getTaggingOn();
	}
    }
    
    private boolean isVisible()
    {
	if (_quickSettingsGroup != null)
	{
	    boolean[] flags = new boolean[_quickSettingsGroup.size()];
	    _quickSettingsGroup.getSelectedFlags(flags);
	    return flags[0];
	}
	else
	{
	    return App.getCurrentApp().getSettings().getVisible();
	}
    }
    
    private void setSynchronizationMode(int value)
    {
	System.out.println("MainForm::setSynchronizationMode(" + value + ")");
	WebServiceInteractor ws = App.getCurrentApp().getWebServiceIteractor();
	switch (value)
	{
	    case 0:
		if (_synchronizeCommand !=null)
		{
		    form.removeCommand(_synchronizeCommand);
		    _synchronizeCommand = null;
		}
		ws.changeInstensity(WebServiceInteractor.FIVEMIN);
		break;
	    case 1:
		if (_synchronizeCommand !=null)
		{
		    form.removeCommand(_synchronizeCommand);
		    _synchronizeCommand = null;
		}
		ws.changeInstensity(WebServiceInteractor.HOURLY);
		break;
	    case 2:
		if (_synchronizeCommand !=null)
		{
		    form.removeCommand(_synchronizeCommand);
		    _synchronizeCommand = null;
		}
		ws.changeInstensity(WebServiceInteractor.DAILY);
		break;
	    case 3:
		_synchronizeCommand = new Command(getResoursable().get("synchronizeCommand"), Command.EXIT, 50);
		form.addCommand(_synchronizeCommand);
		ws.changeInstensity(System.currentTimeMillis());
		break;
	    default:
		break;
	}
	saveSettings();
    }
    
    private void setTaggingOn(boolean value)
    {
	System.out.println("MainForm::setTaggingOn(" + value + ")");
	if (App.getCurrentApp().getWebServiceIteractor().getEncryptionKey() != null)
	{
	    App.getCurrentApp().getDeviceDiscoverer(this).setTaggingOn(value);
	    saveSettings();
	}
    }
    
    private void setVisible(boolean value)
    {
	System.out.println("MainForm::setVisible(" + value + ")");
	
	if (App.getCurrentApp().getWebServiceIteractor().getEncryptionKey() != null)
	{
	    System.out.println("MainForm::setVisible - getEncryptionKey != null");
	    System.out.println("MainForm::setVisible - getDeviceDiscoverer(this).setVisible(" + value + ")");
	    App.getCurrentApp().getDeviceDiscoverer(this).setVisible(value);
	    
	    System.out.println("MainForm::setVisible - Invoke saveSettings();");
	    saveSettings();
	}
    }
    
    private void showAlert(
	    String alertTitleResourceString
	    , String alertTextResourceString
	    , String alternateAlertTitle
	    , String alternateAlertText
	    )
    {
	Resources resources = getResoursable();
	
	String alertTitle = null;
	String alertText = null;
	
	alertTitle = resources.get(alertTitleResourceString);
	alertText = resources.get(alertTextResourceString);
	
	Alert alert = new Alert(alertTitle, alertText, null, AlertType.ERROR);
	alert.setTimeout(Alert.FOREVER);
	App.getDisplay().setCurrent(alert);
    }
    
    private void showCannotStartTaggingServiceAlert()
    {
	showAlert(
		"alertTitle"
		, "bluetoothInitializationAlert"
		, "Alert"
		, "Error occured while Bluetooth initialization. Please, contact Next2Friends support team."
		);
    }
    
    private void showCannotStartDeviceSearchAlert()
    {
	showAlert(
		"alertTitle"
		, "cannotStartDeviceSearchAlert"
		, "Alert"
		, "Cannot start device search. Please, contact Next2Friends support team."
		);
    }
    
    /* ------ Declarations ------ */
    
    private ChoiceGroup _quickSettingsGroup;
    private ChoiceGroup _synchronizeGroup;
    
    private Command _exitCommand;
    private Command _synchronizeCommand;
    private Command _settingsCommand;
    
//    private DeviceDiscoverer _discoverer;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of MainForm.
     */
    public MainForm(String id, Resources res)
    {
	super("N2F TAG", res);
    }
    
    protected Displayable getScreen()
    {
	return this.form;
    }
    
    public void show()
    {
	WebServiceInteractor ws = App.getCurrentApp().getWebServiceIteractor();
	if (ws != null)
	{
	    if (ws.getTagId() == null)
	    {
		SettingsBean bean = (SettingsBean) UIManager.getInstance().getBean(UIManager.SCREEN_SETTINGS);
		if (bean == null)
		{
		    bean = new SettingsBean(App.getCurrentApp().getSettings());
		    UIManager.getInstance().putBean(UIManager.SCREEN_SETTINGS, bean);
		}
		showBusy(null);
		ws.getTagId(null);
		ws.getEncryptionTag(null);
		ws.getBlockList(this);
	    }
	    else
	    {
		refresh();
	    }
	}
    }
    
    protected void refresh()
    {
	hideBusy();
	
	App currentApp = App.getCurrentApp();
	
	if (currentApp.getWebServiceIteractor().getEncryptionKey() != null)
	{
	    currentApp.getDeviceDiscoverer(this);
	}
	currentApp.getWebServiceIteractor().init();
	
	initializeComponent();
	
	App.getDisplay().setCurrent(form);
	
	System.out.println("MainForm::refresh");
	
	boolean isVisible = currentApp.getSettings().getVisible();
	boolean isTaggingOn = currentApp.getSettings().getTaggingOn();
	
	System.out.println("MainForm::refresh - isVisible = " + isVisible);
	System.out.println("MainForm::refresh - isTaggingOn = " + isTaggingOn);
	
	setVisible(isVisible);
	setTaggingOn(isTaggingOn);
    }
}
