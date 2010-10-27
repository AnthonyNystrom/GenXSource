package com.genetibase.askafriend.common;

/**
 * This class substitutes long string keys to simple digits 
 * that has unique character as string keys. 
 */
public class Code {
	
	/** Generated digital key for long string key. */
	private int value;
	
	/**
	 * Constructor.
	 * saves digital key for string key and then sets it to "value" field.
	 * 
	 * @param value - string key.
	 */
	public Code(String value) {
	    this.value = value.hashCode();
	}
	
	public Code(int code) {
		this.value = code;
	}

	/**
	 * Returns generated digital key. 
	 * @return generated digital key.
	 */
	public int hashCode() {
		return value;
	}
	
	/**
	 * Compares this Code to the specified object. 
	 * @param obj - the object to compare this Code against.
	 * @return true if the Code are equal; false otherwise.
	 */
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		//if (getClass() != obj.getClass()) return false;
		final Code other = (Code) obj;   		
        return value == other.value;
    }
	
}