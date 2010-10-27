package n2f.contacts.pimutils;

import n2f.contacts.Contact;



public class ContactLastnameComparator implements Comparator {

	public int compare(Object o1, Object o2) {
		Contact contact1 = (Contact) o1;
		Contact contact2 = (Contact) o2;
		return contact1.getLastName().compareTo(contact2.getLastName());
	}

}
