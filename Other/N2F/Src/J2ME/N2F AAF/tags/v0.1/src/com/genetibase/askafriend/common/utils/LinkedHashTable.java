package com.genetibase.askafriend.common.utils;

import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Vector;

/**
 * This class implements behavior of linkedHashMap given in SDK.
 * This linked list defines the iteration ordering, which is normally the order in which keys were inserted into the map.
 *  
 * @author Zyl
 */
public class LinkedHashTable extends Hashtable {
	
	/**
	 * This object contains references to keys in order that they was put
	 */
	private Vector order = new Vector();

	/**
	 * Returns an enummeration of the values in this hashtable.
	 * Use the Enumeration methods on the returned object to fetch the elements sequentially
	 * This method be optimized because of it hasvy for memory!!!
	 */
	public Enumeration elements() {

		Vector values = new Vector();
		for (Enumeration enumm = order.elements(); enumm.hasMoreElements();) {
			Object key = enumm.nextElement();
			values.addElement(get(key));
		}
		return values.elements();
	}

	public void setIndexOrder(int index, Object key) {
		order.removeElement(key);
		order.insertElementAt(key, index);
	}
	
	/**
	 * Maps the specified key to the specified value in this hashtable
	 */
	public Object put(Object key, Object value) {

		Object ret = super.put(key, value);
		order.removeElement(key);
		order.addElement(key);
		return ret;
	}
	
	/**
	 * Maps the specified key to the specified value in this hashtable
	 */
	public Object put(Object key, Object value, int index) {
		Object ret = super.put(key, value);
		order.removeElement(key);
		order.insertElementAt(key, index);
		return ret;
	}
	

	/**
	 * Removes the key (and its corresponding value) from this hashtable.
	 * This method does nothing if the key is not in the hashtable
	 */
	public Object remove(Object key) {

		Object ret = super.remove(key);
		if (ret != null) {
			order.removeElement(key);
		}
		return ret;
	}
	
	/**
	 * Returns the first element in this keyList. 
	 * @return
	 */
	public Object getFirstKey() {
		return order.firstElement();	
	}
	
	/**
	 * Returns the last element in this leyList. 
	 * @return
	 */
	public Object getLastKey(){
		return order.lastElement();
	}

	/**
	 * Clears this hashtable so that it contains no keys.
	 */
	public void clear() {

		super.clear();
		order.removeAllElements();
	}

	/**
	 * Returns an enummeration of the keys in this hashtable. In order that they were added.
	 */
	public Enumeration keys() {

		return order.elements();
	}

	/**
	 * Returns a rather long string representation of this hashtable
	 */
	public String toString() {

		StringBuffer sb = new StringBuffer('[');
		for (Enumeration enumm = keys(); enumm.hasMoreElements();) {
			Object key = enumm.nextElement();
			sb.append(" key=");
			sb.append(key);
			sb.append(" value=");
			sb.append(get(key));
			sb.append('\n');
		}
		sb.append(']');
		return sb.toString();
	}
	
}
