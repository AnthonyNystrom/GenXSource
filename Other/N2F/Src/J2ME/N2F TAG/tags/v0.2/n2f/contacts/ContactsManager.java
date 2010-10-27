package n2f.contacts;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Enumeration;
import java.util.Vector;

import n2f.contacts.core.Resoursable;
import n2f.contacts.pimutils.Comparator;
import n2f.contacts.pimutils.ContactLastnameComparator;
import n2f.contacts.pimutils.PIMUtilities;
import n2f.contacts.strategies.ContactsReader;
import n2f.contacts.strategies.ContactsReaderSimple;
import n2f.tag.core.AbstractErrorManager;
import n2f.tag.core.Deallocatable;
import n2f.tag.core.ErrorListener;
import n2f.tag.core.Serializable;

public class ContactsManager extends AbstractErrorManager implements Deallocatable, Serializable {
	private static final String SERIALIZABLE_KEY = "-=Contacts=-";
	private Vector contactsList = new Vector();
	
	private ErrorListener listener;
	
	private static ContactsManager INSTANCE = null;
	private ContactsReader contactsReader;
	private javax.microedition.pim.PIM pimInstance = null;
	
	/**
	 * returns an instance of the manager 
	 * @return
	 */
	public static ContactsManager getInstance(){
		if (INSTANCE == null) {
			INSTANCE = new ContactsManager();
		}
		return INSTANCE;
	}
	
	/**
	 * constructer
	 */
	private ContactsManager() {
		try {
			Class.forName("javax.microedition.pim.PIM");
			pimInstance = javax.microedition.pim.PIM.getInstance();
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
		contactsReader = new ContactsReaderSimple();
	}
	
	/**
	 * returns true if the system contantain PIM api
	 * @return
	 */
	public boolean isPIMPresents(){
		return pimInstance != null? true: false;
	}
	
	/**
	 * deternines necessary reader for PIM
	 * @param contactsReader
	 */
	public void setContactReaderStrategy(ContactsReader contactsReader) {
		if (contactsReader == null) {
			throw new IllegalArgumentException("ContactsReader can't be null");
		} else {
			this.contactsReader = contactsReader;
		}
	}
	
	/**
	 * contacts are sorted, by lastname if other comparator was not requested before
	 * @return
	 * @throws UnsupportedException
	 */
	public Vector getPIMContacts() throws UnsupportedException {
		return getPIMContacts(new ContactLastnameComparator());
	}
	/**
	 * 
	 * @param comparator - comparator for the contacts
	 * @return
	 * @throws UnsupportedException
	 */
	public Vector getPIMContacts(Comparator comparator) throws UnsupportedException {
		if (pimInstance == null)
			throw new UnsupportedException("PIM is not suported");
		Vector sorted = contactsReader.getContacts(pimInstance, listener);
		PIMUtilities.sort(sorted, comparator);
		return sorted;
	}
	
	/**
	 * reads contacts from address book (PIM) and sorts them by lastname
	 * @throws UnsupportedException
	 */
	public void updateContactsFromAddressBook() throws UnsupportedException {
		updateContactsFromAddressBook(new ContactLastnameComparator());
	}
	
	/**
	 * reads contacts from address book and sortes them according comparator
	 * @param comparator
	 * @throws UnsupportedException
	 */
	public void updateContactsFromAddressBook(Comparator comparator)  throws UnsupportedException {
		contactsList.removeAllElements();
		contactsList = getPIMContacts(comparator);
	}
	
	/**
	 * this method returns list of contacts
	 * @return
	 */
	public Vector getContacts() {
		return contactsList;
	}
	
	/**
	 * saves contacts to resources
	 * @param resoursable
	 * @throws IOException
	 */
	public void serializeContacts(Resoursable resoursable) throws IOException {
		resoursable.setProperty(SERIALIZABLE_KEY, this);
	}
	
	/**
	 * loads contacts from resources
	 * @param resoursable
	 * @throws IOException
	 */
	public void deserializeContacts(Resoursable resoursable) throws IOException {
		resoursable.getProperty(SERIALIZABLE_KEY, this);
	}

	/**
	 * releases memory
	 */
	public void free() {
		contactsList.removeAllElements();
		INSTANCE = null;
	}

	/**
	 * reads contacts from DataInputStream
	 */
	public void readObject(DataInputStream in) throws IOException {
		contactsList.removeAllElements();
		int size = in.readInt();
//		contactsList.setSize(size);
		for (int i = 0; i< size; i++) {
			ContactImpl c = new ContactImpl();
			c.readObject(in);
			contactsList.addElement(c);
		}
	}

	/**
	 * writes contacts to DataOutputStream
	 */
	public void writeObject(DataOutputStream out) throws IOException {
		out.writeInt(contactsList.size());
		for (Enumeration e = contactsList.elements(); e.hasMoreElements(); ) {
			Contact c = (Contact)e.nextElement();
			c.writeObject(out);
		}
	}
	
	public void interrupt(){
		if (this.contactsReader != null) {
			contactsReader.interrupt();
		}
	}

	public void setListener(ErrorListener listener) {
		this.listener = listener;
	}
}
