package n2f.contacts.pimutils;

import java.io.InputStream;
import java.io.OutputStream;
import java.util.Hashtable;
import java.util.Vector;

import javax.microedition.pim.Contact;
import javax.microedition.pim.ContactList;
import javax.microedition.pim.PIMException;
import javax.microedition.pim.PIMList;

import n2f.tag.utils.Utils;

public class PIMUtilities {
	public static final String EMPTY_STRING = "";

	/**
	 * This method fills hashtable
	 * keys - index in array from ContactList.getSupportedFields()
	 * value - ContactList.getFieldDataType
	 * @param contacts
	 * @return {@link Hashtable}
	 */
	
	public static Hashtable getSupportedFields(ContactList contacts) {
		Hashtable hash = null;
		if (contacts != null) {
			int[] fields = contacts.getSupportedFields();
			hash = new Hashtable(fields.length * 2);
			for (int i = 0; i< fields.length; i++) {
				Integer key = new Integer(fields[i]);
				Integer attribute = new Integer(contacts.getFieldDataType(fields[i]));                          
				hash.put(key, attribute);
			}
		}
		return hash; 
	}	
	
	/**
	 * casts object to Strings array
	 * @param obj
	 * @return
	 */
	
	public static String[] convertObjToStringArray(Object obj) {
		String[] source = null;
		if (obj instanceof String[]) {
			source = (String[]) obj;
		} else if (obj instanceof String) {
			source = new String[] {(String) obj};
		} else if (obj instanceof byte[]) {
			source = new String[] {new String((byte[]) obj)};
		}
		return source;
	}
	
	public static Object dispatchMethod(Contact contact, int field, int contactFieldType, int index) {
		Object obj = null;
		try {
			if (contactFieldType == Contact.STRING_ARRAY){
				obj = contact.getStringArray(field, Contact.ATTR_NONE);
			} else if (contactFieldType == Contact.BINARY || contactFieldType == Contact.ATTR_NONE) {
				obj = contact.getBinary(field, 0);                              
			} else if (contactFieldType == Contact.STRING) {
//				System.out.print(" Field=" + field );
				obj = new String[index];
				
				for(int k=0; k<index; k++){
					try{
						((String[])obj)[k] = contact.getString(field,k);
					}catch(Exception  e){
//						e.printStackTrace();
					}
				}
			} 
		} catch (RuntimeException e) {
			e.printStackTrace();
		}
		return obj;
	}

	/**
	 * true
	 * This method checks if contact contain all requested fields. i.e each contact should have NAME and TEL 
	 * @param contact
	 * @param neccessaryFields
	 * @return
	 */
	public static boolean isContactRelevant(Contact contact, int[] neccessaryFields) {
		boolean retValue = true;
		if (contact != null) {
			int[] savedFields = contact.getFields();
			for (int i = 0; i < neccessaryFields.length; i++) {
				if (!isArrayContainItem(neccessaryFields[i], savedFields)) {
					retValue = false;
					break;
				}
			}
		}
		//System.out.println("Contact has all necessary items " + contact.toString());            
		return retValue;
	}
           
	private static final boolean isArrayContainItem(int item, int[] array) {
		boolean retValue = false;
		for (int i = 0; i< array.length; i++) {
			if (array[i] == item) {
				retValue = true;
				break;
			}
		}
		return retValue;
	}
           
	public static final void closePIMList(PIMList contacts) {
		if (contacts != null ) {
			try {
				contacts.close();
			} catch (PIMException e) {
				e.printStackTrace();
			}
		}
		
	}
	
	/**
	 * Closes this input stream and releases any system resources associated with the stream
	 * 
	 * @param is
	 */
	public static void close (InputStream is) {
		Utils.close(is);
	} 
	/**
	 * Closes this output stream and releases any system resources associated with the stream
	 * @param os
	 */
	public static void close (OutputStream os) {
		Utils.close(os);
	}	
	
	public static final void sort(Vector src, Comparator c) {
		Object[] _src = new Object[src.size()];
		Object[] _srcdest = new Object[_src.length];
		src.copyInto(_src);
		System.arraycopy(_src, 0, _srcdest, 0, _src.length);
		
		sort(_srcdest, _src, 0, _src.length, 0, c);
		src.removeAllElements();
		for (int i = 0; i < _src.length; i++) {
			src.addElement(_src[i]);
		}
	}
	
	/**
	 * @param src - is the source array that starts at index 0
	 * @param dest - is the (possibly larger) array destination with a possible offset
	 * @param low - is the index in dest to start sorting
	 * @param high - is the end index in dest to end sorting
	 * @param off - is the offset into src corresponding to low in dest
	 * @param c - comparator of the sortable object
	 */
	public static void sort(Object src[], Object dest[], int low, int high, int off, Comparator c) {
		int length = high - low;

		// Insertion sort on smallest arrays
		if (length < 7) {
			for (int i = low; i < high; i++)
				for (int j = i; j > low && c.compare(dest[j - 1], dest[j]) > 0; j--)
					swap(dest, j, j - 1);
			return;
		}

		// Recursively sort halves of dest into src
		int destLow = low;
		int destHigh = high;
		low += off;
		high += off;
		int mid = (low + high) >> 1;
		sort(dest, src, low, mid, -off, c);
		sort(dest, src, mid, high, -off, c);

		// If list is already sorted, just copy from src to dest. This is an
		// optimization that results in faster sorts for nearly ordered lists.
		if (c.compare(src[mid - 1], src[mid]) <= 0) {
			System.arraycopy(src, low, dest, destLow, length);
			return;
		}

		// Merge sorted halves (now in src) into dest
		for (int i = destLow, p = low, q = mid; i < destHigh; i++) {
			if (q >= high || p < mid && c.compare(src[p], src[q]) <= 0)
				dest[i] = src[p++];
			else
				dest[i] = src[q++];
		}
    }	
	
    private static void swap(Object srcArray[], int index1, int index2) {
    	Object t = srcArray[index1];
    	srcArray[index1] = srcArray[index2];
    	srcArray[index2] = t;
    }    
}
