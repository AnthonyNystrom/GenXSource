package com.genetibase.askafriend.common.utils;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Enumeration;
import java.util.Hashtable;


/**
 * The Properties class represents a persistent set of
 * properties. The Properties can be saved to a stream
 * or loaded from a stream. Each key and its corresponding value in
 * the property list is a string.
 * <p>
 * A property list can contain another property list as its
 * "defaults"; this second property list is searched if
 * the property key is not found in the original property list.
 * <p>
 */
public class Properties{
	private Hashtable data = new Hashtable();
	
	/**
     * Searches for the property with the specified key in this property list.
     * The method returns <code>null</code> if the property is not found.
     *
     * @param   key   the property key.
     * @return  the value in this property list with the specified key value.
     * @see     #setProperty
	 */
	public String getProperty(String key) {
		return (String)data.get(key);
	}

	/**
     * Calls the <tt>Hashtable</tt> method <code>put</code>. Provided for
     * parallelism with the <tt>getProperty</tt> method. Enforces use of
     * strings for property keys and values. The value returned is the
     * result of the <tt>Hashtable</tt> call to <code>put</code>.
     *
     * @param key the key to be placed into this property list.
     * @param value the value corresponding to <tt>key</tt>.
     * @return     the previous value of the specified key in this property
     *             list, or <code>null</code> if it did not have one.
     * @see #getProperty
	 */
	public void setProperty(String key, String value) {
		data.put(key, value);
	}

	/**
	 * removes property with the specified key in this property list.
	 * 
	 * @param key the key in Hashtable
	 */
	public void removeProperty(String key) {
		data.remove(key);
	}
	
	/**
	 * merges incomming Hashtable(source) and enclosed Hashtable
	 * @param source source Hashtable 
	 */
	public void merge(Hashtable source) {
		for (Enumeration e = source.keys(); e.hasMoreElements();){
			String key = (String)e.nextElement();
			data.put(key, source.get(key));
		}
	}
	
	/**
     * Searches for the property with the specified key in this property list.
     * The method returns the default value argument if the property is not found.
     *
     * @param   key            the hashtable key.
     * @param   defaultValue   a default value.
     *
     * @return  the value in this property list with the specified key value.
     * @see     #setProperty
	 */
	
	public String getProperty(String key, String defaultValue) {
		String val = getProperty(key);
		return (val == null) ? defaultValue : val;
	}
	
	/**
     * Reads a property list (key and element pairs) from the input
     * stream.  
     * <p>
     * The key contains all of the characters in the line starting
     * with the first non-white space character and up to, but not
     * including, the first <code>'='</code>.
     * <p>
     * @param      inputStream   the input stream.
     * @exception  IOException  if an error occurred when reading from the
     *               input stream.
	 */
	
	public void loadProperties( InputStream inputStream) throws IOException {
		if (inputStream == null ) throw new NullPointerException();
		DataInputStream is = new DataInputStream(inputStream);
		int ch;
		StringBuffer sb = new StringBuffer();
		String key = null;
		String value = null;
		boolean skip = false;
		System.out.print("START\n");
		while ((ch = is.read()) != -1) {
//			System.out.print((char)ch);
			if (skip) {
				if (ch == '\n')
					skip = false;
				continue;
			}
			if (ch == '\n') {
          		if (key != null) {
          			value = sb.toString();
          			data.put(key, value);
          			sb.setLength(0);
          			key = null; value = null;
          		}
          	} else if (ch == '=') {
          		key = sb.toString();
          		sb.setLength(0);
          	} else if (ch == '\r') {
          		//pass cr       
          	} else if (ch == '\t') {
          		//pass cr
          	} else if (ch == '#') {
          		skip = true;
          	} else{// if (ch != ' ') {
				sb.append((char)ch);
          	} 
          	
    	}
		if (value == null) value = sb.toString();
		if (key != null && value != null) {
//			System.out.println("key=" + key + " value=" + value );
  			data.put(key, value);			
		}
	}

	/**
     * Returns an enumeration of all the keys in this property list,
     * including distinct keys in the default property list if a key
     * of the same name has not already been found from the main
     * properties list.
     *
     * @return  an enumeration of all the keys in this property list, including
     *          the keys in the default property list.
     * @see     java.util.Enumeration
	 */
	
	public Enumeration propertyNames() {
		return data.keys();
	}

	
	/**
     * Writes this property list (key and element pairs) in this
     * <code>Properties</code> table to the output stream in a format suitable
     * for loading into a <code>Properties</code> table using the
     * {@link #loadProperties(inputStream) load} method.
     * @param   out      an output stream.
     * @exception  IOException if writing this property list to the specified
     *             output stream throws an <tt>IOException</tt>.
	 */
	public void store (OutputStream os) throws IOException {
//		DataOutputStream dos = new DataOutputStream(os);
//		dos.writeInt(data.size());
//        for (Enumeration e = propertyNames(); e.hasMoreElements();) {
//            String key = (String)e.nextElement();
//            String val = getProperty(key);
//        }
	}
	
	/* (non-Javadoc)
	 * @see java.lang.Object#toString()
	 */

	public String toString() {
		StringBuffer sb = new StringBuffer ("Properties[");
		for (Enumeration e = propertyNames(); e.hasMoreElements(); ) {
			String key = (String)e.nextElement();
			String value = getProperty(key);
			sb.append(key);
			sb.append('=');
			sb.append(value);
			sb.append('\n');
		}
		return sb.toString();
	}

	/* (non-Javadoc)
	 * @see com.tbox.core.common.Serializable#writeObject(java.io.DataOutputStream)
	 */
	public void writeObject(DataOutputStream out) throws IOException {
		out.writeInt(data.size());
		for (Enumeration e = propertyNames(); e.hasMoreElements(); ) {
			String key = (String)e.nextElement();
			String value = getProperty(key);
			out.writeUTF(key);
			out.writeUTF(value);			
		}		
	}

	/* (non-Javadoc)
	 * @see com.tbox.core.common.Serializable#readObject(java.io.DataInputStream)
	 */
	public void readObject(DataInputStream in) throws IOException {
		data.clear();
		int size = in.readInt();
		for (int i = 0; i < size; i++) {
			String key = in.readUTF();
			data.put(key, in.readUTF());
		}
	}
}