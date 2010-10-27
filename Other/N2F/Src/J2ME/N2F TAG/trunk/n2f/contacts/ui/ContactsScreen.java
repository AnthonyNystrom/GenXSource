package n2f.contacts.ui;

import genetibase.java.microedition.componentmodel.Resources;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.List;
import javax.microedition.lcdui.Ticker;

import n2f.contacts.Contact;
import n2f.tag.App;
import n2f.tag.ui.AbstractWindow;
import n2f.tag.ui.Interruptable;
import n2f.tag.ui.UIManager;



public class ContactsScreen extends AbstractWindow implements CommandListener {
	//	private final Form form;
    private final List contacts;  
	private final Command selectCommand;
	private final Command updateCommand;
	private final Command backCommand;
	
	public ContactsScreen(String id, Resources resoursable) {
		super(id, resoursable);
		contacts = new List(resoursable.get("contactsScreenTitle"), List.IMPLICIT);
		selectCommand = new Command(resoursable.get("contactsSelect"), Command.SCREEN, 0);		
		updateCommand = new Command(resoursable.get("contactsUpdate"), Command.SCREEN, 1);
		backCommand = new Command(resoursable.get("contactsBack"), Command.BACK, 0);
		
		ContactsBean bean = (ContactsBean)UIManager.getInstance().getBean(getId());
		if (bean == null) {
			UIManager.getInstance().putBean(id, new ContactsBean());
		}
		contacts.addCommand(updateCommand);
		contacts.addCommand(backCommand);
		contacts.setCommandListener(this);
		contacts.setTicker(new Ticker(resoursable.get("contactsTicker")));		
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
		ContactsBean bean = (ContactsBean)UIManager.getInstance().getBean(getId());
		Vector contacts = App.getCurrentApp().getContacts();
		Contact contact = (Contact)contacts.elementAt(index);
		bean.setContacts(contact);
//		Debug.println("Index=" + index + " " + contact.toString());
		UIManager.getInstance().show(UIManager.SCREEN_SMS);
	}
	
	private void doUpdate(){
		showBusy(new Interruptable(){
			public void interrupted() {
				App.getCurrentApp().interruptContactsReading();
				refresh();
			}
		});
		App.getCurrentApp().updateContacts(this);
	}
	
	protected void refresh() {
//		hideBusy();
		show();
	}

	public void show() {
		contacts.deleteAll();
//		contacts.removeCommand(selectCommand);
		Vector contactslist = App.getCurrentApp().getContacts();
		if (contactslist.size() > 0) {
//			contacts.addCommand(selectCommand);
			contacts.setSelectCommand(selectCommand);
		}
		for (Enumeration e = contactslist.elements(); e.hasMoreElements(); ) {
			Contact c = (Contact)e.nextElement();
			contacts.append(c.getContact(), null);
		}
		ContactsBean bean = (ContactsBean)UIManager.getInstance().getBean(getId());
		if (bean.getContact() != null) {
			int index = contactslist.indexOf(bean.getContact());
			if (index > 0) {
				contacts.setSelectedIndex(index, true);
			}
		}
        getDisplay().setCurrent(contacts);
	}

}
