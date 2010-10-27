package n2f.contacts.strategies;

import java.util.Vector;

import n2f.tag.core.ErrorListener;

/**
 * This interface represents Strategy design pattern 
 * @author Zyl
 */

public interface ContactsReader{
	Vector getContacts(javax.microedition.pim.PIM pim, ErrorListener listener);
	void interrupt();
}
