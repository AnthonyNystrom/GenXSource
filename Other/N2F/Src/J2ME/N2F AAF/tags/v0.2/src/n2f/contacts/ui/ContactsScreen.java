package n2f.contacts.ui;

import java.util.Enumeration;
import java.util.Vector;

//import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Ticker;
//import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.List;

import n2f.contacts.Contact;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.ui.AbstractWindow;
import com.genetibase.askafriend.ui.Interruptable;
import com.genetibase.askafriend.ui.LocaleUI;
import com.genetibase.askafriend.ui.UIManager;
//import com.genetibase.askafriend.utils.Debug;



public class ContactsScreen extends AbstractWindow implements CommandListener {
	//	private final Form form;
    private final List contacts;  
	private final Command selectCommand;
	private final Command updateCommand;
	private final Command backCommand;
	
	public ContactsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		contacts = new List(getLocalizedText(LocaleUI.CONTACTS_COMMAND_TITLE), List.IMPLICIT);
		selectCommand = new Command(getLocalizedText(LocaleUI.CONTACTS_COMMAND_SELECT), Command.SCREEN, 0);		
		updateCommand = new Command(getLocalizedText(LocaleUI.CONTACTS_COMMAND_UPDATE), Command.SCREEN, 1);
		backCommand = new Command(getLocalizedText(LocaleUI.CONTACTS_COMMAND_BACK), Command.BACK, 0);
		
		ContactsBean bean = (ContactsBean)Engine.getEngine().getBean(getId());
		if (bean == null) {
			Engine.getEngine().putBean(id, new ContactsBean());
		}
		contacts.addCommand(updateCommand);
		contacts.addCommand(backCommand);
		contacts.setCommandListener(this);
		contacts.setTicker(new Ticker(getLocalizedText(LocaleUI.CONTACTS_TICKER_TEXT)));		
//		addConsole(this.form);
	}

	public int getFormHeight() {
		return contacts.getHeight();
	}

	public int getFormWidth() {
		return contacts.getWidth();
	}

	protected Displayable getScreen() {
		return contacts;
	}

	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == selectCommand) {
			doAction();
		} else if (c == updateCommand) {
			doUpdate();
		} else if (c == backCommand) {
			UIManager.getInstance().showPrevious();
		}
	}
	
	private void doAction() {
		int index = contacts.getSelectedIndex();
		ContactsBean bean = (ContactsBean)Engine.getEngine().getBean(getId());
		Vector contacts = Engine.getEngine().getContacts();
		Contact contact = (Contact)contacts.elementAt(index);
		bean.setContacts(contact);
//		Debug.println("Index=" + index + " " + contact.toString());
		//TODO: show SMS
		UIManager.getInstance().show(UIManager.SCREEN_SMS);
	}
	
	private void doUpdate(){
		showBusy(new Interruptable(){
			public void interrupted() {
				Engine.getEngine().interruptContactsReading();
				refresh();
			}
		});
		Engine.getEngine().updateContacts(this);
	}
	
	protected void refresh() {
		hideBusy();
		show();
	}

	public void show() {
		contacts.deleteAll();
//		contacts.removeCommand(selectCommand);
		
		Vector contactslist = Engine.getEngine().getContacts();
		if (contactslist.size() > 0) {
//			contacts.addCommand(selectCommand);
			contacts.setSelectCommand(selectCommand);
		}
		for (Enumeration e = contactslist.elements(); e.hasMoreElements(); ) {
			Contact c = (Contact)e.nextElement();
			contacts.append(c.getContact(), null);
		}
		ContactsBean bean = (ContactsBean)Engine.getEngine().getBean(getId());
		if (bean.getContact() != null) {
			int index = contactslist.indexOf(bean.getContact());
			if (index > 0) {
				contacts.setSelectedIndex(index, true);
			}
		}
        getDisplay().setCurrent(contacts);
	}

}
