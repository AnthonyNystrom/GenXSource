package n2f.contacts.strategies;
//package n2f.sup.contacts.strategies;
//
//import java.util.Enumeration;
//import java.util.Hashtable;
//import java.util.Vector;
//import javax.microedition.pim.Contact;
//import javax.microedition.pim.ContactList;
//
//import n2f.sup.common.AbstractErrorManager;
//import n2f.sup.common.Deallocatable;
//
//public class ContactsReaderMPT implements ContactsReader {
//	private Hashtable supportedFieldsHashtable = new Hashtable();
//	private static final String EMPTY_STRING = PIMUtilities.EMPTY_STRING;
//
//	private int[] getContactIDs() {
//		int[] necessaryFields = new int[] {Contact.NAME, Contact.TEL};
//		if ( isSupportedFields(supportedFieldsHashtable, necessaryFields) ) {
//			System.out.println("Contact.NAME, Contact.TEL");
//		} 
//		//JUST FOR FUN IF BOTH CAN BE SUPPORTED!
//		/*else */if ( isSupportedFields(supportedFieldsHashtable, 
//				necessaryFields = new int[] {Contact.FORMATTED_NAME, Contact.TEL}) ) {
//			System.out.println("Contact.FORMATTED_NAME, Contact.TEL");			
//		} else necessaryFields = null;
//		return necessaryFields;
//	}
//	
//	public Vector getContacts(javax.microedition.pim.PIM pim, AbstractErrorManager errorManager ) {
//		Vector ret =  null;
//		ContactList contacts = null; //try to create some contacts
//		
//		try {
//			contacts = (ContactList) pim.openPIMList(javax.microedition.pim.PIM.CONTACT_LIST, javax.microedition.pim.PIM.READ_ONLY);
//			if (contacts != null) {
//				//gets supported fields and put them to hash. key is supported fiels and value it's type
//				supportedFieldsHashtable = PIMUtilities.getSupportedFields(contacts);
//				// this method returns array of contacts (PIM) that defines if NAME is STRUCTURED
//				int[] necessaryFields = getContactIDs();
//                                         ////////////////////////////////       
//				ret = new Vector();
//				int counter = 0;
//				for (Enumeration enumm = contacts.items(); enumm.hasMoreElements(); counter++) {
//					try {
//						Contact aContact = (Contact)enumm.nextElement();
//						// if contact is NULL or contact do not have NAME and TEL will be ignored 
//						if ( aContact == null || !PIMUtilities.isContactRelevant(aContact, necessaryFields) ){
//							// System.out.println("CONTINUE");
//							continue;
//						}
//                                                        
//						String firstName = null;
//						String lastName = null;
//						String[] values = null;
//						//an array for two elements {NAME, TEL} OR {NAME_FORMATTED, TEL}
//						for (int i = 0; i < necessaryFields.length; i++) {
//							//at first iteration KEY = NAME | FORMATTED_NAME
//							//teh second TEL
//							Integer key = new Integer(necessaryFields[i]);
//							//retieve type of CONTACT FIELD //STRING, ARRAY, BOOLEAN, etc.
//							Integer type = (Integer)supportedFieldsHashtable.get(key);
//							//check count values for NAME and TEL then
//							values = new String[aContact.countValues(key.intValue())];
//							
//							//System.out.println("field=" + key.intValue() + " type=" + type.intValue() + " count of values=" + values.length);
//							//we're CASTing contact to String|Binary|Array
//							Object obj = PIMUtilities.dispatchMethod(aContact, key.intValue(), type.intValue(), values.length);
//							//privodim k massivam strok
//							String[] source = PIMUtilities.convertObjToStringArray(obj);
//							if (source == null) {
//								System.out.println("----------=============NULL==============---------");
//								continue;
//							}
//							//if NAME is structured then we're defining FIRST_NAME and LAST_NAME
//                            if (key.intValue() == Contact.NAME) {
//								// firstName = concantData(source, TEXT_LENGHT, false, true);
//								if (source.length > Contact.NAME_FAMILY) {
//									lastName = source[Contact.NAME_FAMILY]; 
//								}	
//								if (lastName == null) {
//									lastName = EMPTY_STRING;
//								}
//								if (source.length > Contact.NAME_GIVEN) {
//									firstName = source[Contact.NAME_GIVEN];
//								}
//								if (firstName == null) {
//									firstName = EMPTY_STRING;
//								}
//                            } else if (key.intValue() == Contact.FORMATTED_NAME) {
//								firstName = source[0];
//								lastName = EMPTY_STRING;
//							}
//                            //if it is TEL we're making array of (fucking) numbers 
//                            else if (key.intValue() == Contact.TEL) {
//                            	values = new String[ContactImpl.SUPPORTED_TELS];
//        						System.out.println("Contact=" + firstName  + " l=" + lastName );
//								for (int k = 0; k < source.length; k++) {
//									int index = ContactImpl.getIndexByAttribute(aContact.getAttributes(key.intValue(), k));
//									values[index] = source[k];
//									System.out.print(" values=" + values[index]);
//								}
//							}
//						}
//                        //contacts adding
//						if ( (!EMPTY_STRING.equals(firstName) || !EMPTY_STRING.equals(lastName)) && values != null && values.length > 0) {
//							ret.addElement(new ContactImpl(lastName, firstName, 0, values)); 
//						}
//					} catch(Exception e) {
//						 e.printStackTrace();
//					}
//				}
//			}
//		} catch (Exception e2) {
//			e2.printStackTrace();
//		} catch(Throwable t) {
//			System.gc();
//		} finally {
//			PIMUtilities.closePIMList(contacts);
//			free();
//		}
//
//		return ret;
//	}
//	
//	
//	private static boolean isSupportedFields(Hashtable source, int[] necessaryFields) {
//		boolean retValue = true;
//		for (int i = 0; i< necessaryFields.length; i++) {
//			// System.out.println(" p=" + necessaryFields[i]);
//			if (!source.containsKey( new Integer(necessaryFields[i]))) {
//				retValue = false;
//				break;
//			}
//		}
//		return retValue;
//	}
//
//	
//	public void free() {
//		// TODO Auto-generated method stub
//		
//	}
//
//}
