package n2f.contacts.ui;

import n2f.contacts.Contact;
import n2f.tag.ui.AbstractBean;


public class ContactsBean extends AbstractBean {

	private Contact contacts = null;

	public Contact getContact() {
		return contacts;
	}

	public void setContacts(Contact contacts) {
		this.contacts = contacts;
	}

	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + ((contacts == null) ? 0 : contacts.hashCode());
		return result;
	}

	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final ContactsBean other = (ContactsBean) obj;
		if (contacts == null) {
			if (other.contacts != null)
				return false;
		} else if (!contacts.equals(other.contacts))
			return false;
		return true;
	}

	
	
}
