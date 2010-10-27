package n2f.contacts;

import com.genetibase.askafriend.common.utils.Serializable;


/**
 * 
 * The contact interface
 * @author Zyl
 *
 */
public interface Contact extends Serializable {
	/**
	 * Attribute classifying a data value as related to AUTO
	 */
	int ATTRIBUTE_AUTO = javax.microedition.pim.Contact.ATTR_AUTO;
	/**
	 * Attribute classifying a data value as related to FAX
	 */
	int ATTRIBUTE_FAX = javax.microedition.pim.Contact.ATTR_FAX;
	/**
	 * Attribute classifying a data value as related to HOME
	 */
	int ATTRIBUTE_HOME = javax.microedition.pim.Contact.ATTR_HOME;
	/**
	 * Attribute classifying a data value as related to MOBILE
	 */
	int ATTRIBUTE_MOBILE = javax.microedition.pim.Contact.ATTR_MOBILE;

	/**
	 * UNKNOWN Attribute 
	 */
	int ATTRIBUTE_UNKNOWN = javax.microedition.pim.Contact.ATTR_NONE;
	
	/**
	 * UNSUPPORTED Attribute 
	 */
	int ATTRIBUTE_UNSUPPORTED = -1;
	
	/**
	 * Attribute classifying a data value as related to WORK
	 */
	int ATTRIBUTE_WORK = javax.microedition.pim.Contact.ATTR_WORK;
	
	/**
	 * This method gets the NAME_FAMILY of a contact
	 * @return contact lastName  
	 */
	String getLastName();
	/**
	 * This method gets the NAME_GIVEN of a contact
	 * @return contact firstName  
	 */
	String getFirstName();
	/**
	 * This method gets the Attributes of a contact
	 * @return contact attribute  
	 */
	int getAttributes();
	/**
	 * This method gets the name of a contact (concatenated)
	 * @return contact name  
	 */
    String getContact();
    /**
     * This method gets the number of a contact
     * @param attribute - specified attribute
     * @return number if number by specified attribute is exists, otherwise null
     */
    String getNumber(int attribute);
    
    /**
     * This method returns an array of numbers of specified contact
     * @return numbers
     */
    String[] getNumbers();
    
}
