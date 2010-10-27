package n2f.contacts.strategies;

import java.util.Vector;

//import com.genetibase.askafriend.common.AbstractErrorManager;
import com.genetibase.askafriend.common.ErrorListener;

/**
 * This interface represents Strategy design pattern 
 * @author Zyl
 */

public interface ContactsReader{
	Vector getContacts(javax.microedition.pim.PIM pim, ErrorListener listener);
	void interrupt();
}
