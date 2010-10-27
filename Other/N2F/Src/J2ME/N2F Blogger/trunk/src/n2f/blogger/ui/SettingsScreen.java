
package n2f.blogger.ui;

import genetibase.java.microedition.componentmodel.Resources;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Spacer;
import javax.microedition.lcdui.TextField;

import n2f.blogger.App;

class SettingsScreen
	extends AbstractWindow
{
    private Form mainForm;
    private Command cmdBack,  cmdSubmit;
    private TextField edLogin,  edPassword;
    private SettingsBean bean;

    SettingsScreen(String id, Resources resoursable)
    {
	super(id, resoursable);
	bean = (SettingsBean)UIManager.getInstance().getBean(getId());
	if (bean == null)
	{
	    bean = new SettingsBean(App.getCurrentApp().getSettings());
	    UIManager.getInstance().putBean(UIManager.SCREEN_SETTINGS, bean);
	}
	cmdSubmit = new Command(resoursable.get("continueCommand"),
				Command.SCREEN, 1);
	edLogin = new TextField(resoursable.get("login"), bean.getLogin(), 32,
				TextField.ANY);
	edPassword = new TextField(resoursable.get("pass"), bean.getPassword(),
				   32, TextField.ANY | TextField.PASSWORD);

	this.mainForm = new Form(resoursable.get("settings"));

	this.mainForm.append(new Spacer(getFormWidth(), getFormHeight() / 10));
	this.mainForm.append(edLogin);
	this.mainForm.append(edPassword);
	boolean exit = (bean.getLogin() == null && bean.getPassword() == null);

	cmdBack = new Command(exit ? resoursable.get("exitCommand") : resoursable.get("backCommand"), exit
			      ? Command.EXIT : Command.CANCEL, 1);
	this.mainForm.addCommand(cmdBack);
	this.mainForm.addCommand(cmdSubmit);
	UIManager.getInstance().putBean(getId(), bean);
	this.mainForm.setCommandListener(this);
    }

    public void show()
    {
	getDisplay().setCurrent(mainForm);
    }

    public int getFormHeight()
    {
	return mainForm.getHeight();
    }

    public int getFormWidth()
    {
	return mainForm.getWidth();
    }

    protected void refresh()
    {
	hideBusy();
	bean =
		(SettingsBean)UIManager.getInstance().getBean(UIManager.SCREEN_SETTINGS);
	bean.saveBean(App.getCurrentApp().getSettings());
	UIManager.getInstance().show(UIManager.SCREEN_MAIN, false);
    }

    private static boolean isValid(String login, String pass)
    {
	return login != null && pass != null && login.length() > 0 && pass.length() > 0;
    }

    public void commandAction(Command c, Displayable d)
    {
	super.commandAction(c, d);
	if (c == cmdBack)
	{
	    if (c.getCommandType() == Command.EXIT)
	    {
		App.getCurrentApp().destroyApp(false);
	    }
	    else
	    {
		UIManager.getInstance().show(UIManager.SCREEN_MAIN, false);
	    }
	}
	else if (c == cmdSubmit)
	{
	    String login, pass;
	    if (!isValid(login = edLogin.getString(), pass =
			 edPassword.getString()))
	    {
		showAlert(getResoursable().get("alertCredentials"),
			  AlertType.CONFIRMATION, this);
	    }
	    else
	    {
		bean.setLogin(login);
		bean.setPassword(pass);
		showBusy(null);
		App.getCurrentApp().getWebServiceIteractor().getWebMemberId(this);
	    }
	}
	else if (c == Alert.DISMISS_COMMAND)
	{
	    getDisplay().setCurrent(mainForm);
	}
    }

    protected Displayable getScreen()
    {
	return mainForm;
    }

}
