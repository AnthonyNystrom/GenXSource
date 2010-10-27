package n2f.contacts.strategies;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.pim.Contact;
import javax.microedition.pim.ContactList;
import javax.microedition.pim.PIMException;

import com.genetibase.askafriend.common.ErrorEvent;
import com.genetibase.askafriend.common.ErrorListener;

import n2f.contacts.ContactImpl;
import n2f.contacts.pimutils.PIMUtilities;


public class ContactsReaderSimple implements ContactsReader {
	public static final String EMPTY_STRING = PIMUtilities.EMPTY_STRING;
	private boolean isInterrupted = false;
	private StringBuffer sb = new StringBuffer(); 
	
	
	public Vector getContacts(javax.microedition.pim.PIM pim, ErrorListener listener) {
		if (pim == null) 
			return null;
		Vector contacts = null;
		ContactList clist = null;
		
		try {
			try {
				clist = (ContactList) pim.openPIMList(
						javax.microedition.pim.PIM.CONTACT_LIST,
						javax.microedition.pim.PIM.READ_ONLY);
			} catch (Exception e) {
				listener.actionPerformed(new ErrorEvent(null, e, e.getMessage(), ErrorEvent.GET_PIM_CONTACTS));
//				e.printStackTrace();
				return null;
			}
			contacts = new Vector();
			
			Contact contact = null;
			String[] numbers = new String[ContactImpl.SUPPORTED_TELS];
			String firstname, lastname;
			int attribute = 0;
			
			for (Enumeration items = clist.items(); items.hasMoreElements() && !isInterrupted; ) {
				firstname = EMPTY_STRING; lastname = EMPTY_STRING; numbers = new String[ContactImpl.SUPPORTED_TELS];attribute = 0;

				contact = (Contact) items.nextElement();
				int[] fields = contact.getFields();
				for (int i = 0; i < fields.length; i++) {
					int fieldIndex = fields[i];
					int dataType = clist.getFieldDataType(fieldIndex);
					switch (fieldIndex) {
					case Contact.TEL:
						int size = contact.countValues(fieldIndex);
						for (int j = 0; j < size; j++) {
//							if (clist.isSupportedAttribute(fieldIndex, j)) {//uncomment for good
								int attr = contact.getAttributes(fieldIndex, j);
								try {
									int index = ContactImpl.getIndexByAttribute(attr);
									if (index != ContactImpl.ATTRIBUTE_UNSUPPORTED) {
										numbers[index] = contact.getString(fieldIndex, j);
										attribute = attribute|attr;
									}
								} catch (Exception e) {
									listener.actionPerformed(new ErrorEvent(null, e, e.getMessage(), ErrorEvent.GET_PIM_CONTACTS));
								}
//							}
						}
						break;
					case Contact.NAME:
						if (EMPTY_STRING.equals(lastname )) {						
							if (dataType == Contact.STRING_ARRAY) {
								String arrayStr = getNamesAsArray(contact, fieldIndex);
								if (!lastname.equals(arrayStr))
									lastname += arrayStr;
							} else {
								lastname += getName(clist, contact, fieldIndex);
							}
						}
						break;
					case Contact.NAME_FAMILY:
						lastname = getName(clist, contact, fieldIndex);
						break;
					case Contact.NAME_GIVEN:
						firstname = getName(clist, contact, fieldIndex);
						break;
					case Contact.FORMATTED_NAME:
						if (EMPTY_STRING.equals(lastname )) {
							if (dataType == Contact.STRING_ARRAY) {
								String arrayStr = getNamesAsArray(contact, fieldIndex);
								if (!lastname.equals(arrayStr))
									lastname += arrayStr;
							} else {
								lastname += getName(clist, contact, fieldIndex);
							}
						}
						break;
					default:
				break;
					}

				}
				n2f.contacts.Contact _contact = new ContactImpl(lastname, firstname, attribute, numbers);
				contacts.addElement(_contact);
//				fireError(_contact.toString());
			}
//			Debug.println(contacts.toString());
			

		} catch (Exception e) {
			listener.actionPerformed(new ErrorEvent(null, e, e.getMessage(), ErrorEvent.GET_PIM_CONTACTS));
		} finally {
			if (isInterrupted) {
				contacts.removeAllElements();
				System.gc();
			}
			isInterrupted = false;
			if (clist != null) {
				try {
					clist.close();
				} catch (PIMException e) {
					e.printStackTrace();
				}
			}
		}
		return contacts;
	}
	
	private String getName(ContactList clist, Contact contact, int fieldIndex){
		String retString = EMPTY_STRING;
		int size = contact.countValues(fieldIndex);
		for (int j = 0; j < size; j++) {
//			if (clist.isSupportedAttribute(fieldIndex, j)) {//COMMENTED FOR GOOD
//				int attr = contact.getAttributes(fieldIndex, j);
				try {
					retString += contact.getString(fieldIndex, j);
				} catch (Exception e) {
//					fireError("name:" + e + ":\n");
				}
//			}
		}
		return retString;
	}
	
	private String getNamesAsArray(Contact contact, int fieldIndex) {
		String ret = EMPTY_STRING;
		int count = 0;
		try {
			count = contact.countValues(fieldIndex);
		} catch (Exception e) {
		}
		for (int j = 0; j< count; j++) {
			try {
				String[] data = contact.getStringArray(fieldIndex, j);
				for (int i = 0;i< data.length; i++) {
					if (data[i] != null) {
						if (!ret.equals(EMPTY_STRING)) {
							sb.append(' ');
						}
						sb.append(data[i]);
					}
				}
			} catch (Exception e) {
			} finally{
				ret += sb.toString();
				sb.setLength(0);
			}
		}
		return ret;
	}
	
	public void interrupt() {
		isInterrupted = true;
	}

//
//	public boolean isInterrupted() {
//		return isInterrupted;
//	}
	
}
