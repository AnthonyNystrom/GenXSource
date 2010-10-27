package n2f.contacts;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import n2f.contacts.pimutils.PIMUtilities;



public class ContactImpl implements Contact {
	private static int hashCode(Object[] array) {
		final int PRIME = 31;
		if (array == null)
			return 0;
		int result = 1;
		for (int index = 0; index < array.length; index++) {
			result = PRIME * result + (array[index] == null ? 0 : array[index].hashCode());
		}
		return result;
	}

	public static final int SUPPORTED_TELS = 6;
	private String lastName; // the contact name
    private String firstName; // the contact name
    private int attribute; // the contact name
    
    private String[] numbers; //the contact number

    /**
     * default constructor
     */
    public ContactImpl() {
    }

    /**
     * A constructor with parameters
     * @param lastName - contact lastName
     * @param firstName - contact firstName 
     * @param attribute - contact attribute
     * @param numbers - contact TEL numbers
     */
	
	public ContactImpl(String lastName, String firstName, int attribute, String[] numbers) {
		super();
		this.lastName = lastName;
		this.firstName = firstName;
		this.attribute = attribute;
		this.numbers = numbers;
	}

	public int getAttributes() {
		return attribute;
	}

	public String getContact() {
		StringBuffer sb = new StringBuffer();
		if (lastName != null && !PIMUtilities.EMPTY_STRING.equals(lastName)) {
			sb.append(lastName);
		}
		if (firstName != null && !PIMUtilities.EMPTY_STRING.equals(firstName)) {
			if (lastName != null && !PIMUtilities.EMPTY_STRING.equals(lastName))
				sb.append(' ');
			sb.append(firstName);
		}
		return sb.toString(); 
	}

	public String getFirstName() {
		return firstName;
	}

	/**
	 * This method gets the NAME_FAMILY of a contact 
	 */
	public String getLastName() {
		return lastName;
	}

	public String getNumber(int attribute) {
		int index = getIndexByAttribute(attribute);
		return numbers[index];
	}
	
	public static final int getIndexByAttribute(int attribute) {
		int ret = -1;
//		switch (attribute) {
//		case Contact.ATTRIBUTE_MOBILE:
//			ret = 0;
//			break;
//		case Contact.ATTRIBUTE_AUTO:
//			ret = 1;
//			break;
//		case Contact.ATTRIBUTE_FAX:
//			ret = 2;
//			break;
//		case Contact.ATTRIBUTE_HOME:
//			ret = 3;
//			break;
//		case Contact.ATTRIBUTE_WORK:
//			ret = 4;
//			break;
//		case Contact.ATTRIBUTE_UNKNOWN:
//			ret = 5;
//			break;
//		}
		if ((attribute & Contact.ATTRIBUTE_MOBILE) == Contact.ATTRIBUTE_MOBILE) {
			ret = 0;
		} else if ((attribute & Contact.ATTRIBUTE_AUTO) == Contact.ATTRIBUTE_AUTO) {
			ret = 1;
		} else if ((attribute & Contact.ATTRIBUTE_FAX) == Contact.ATTRIBUTE_FAX) {
			ret = 2;
		} else if ((attribute & Contact.ATTRIBUTE_HOME) == Contact.ATTRIBUTE_HOME) {
			ret = 3;
		} else if ((attribute & Contact.ATTRIBUTE_WORK) == Contact.ATTRIBUTE_WORK) {
			ret = 4;
		} else if ((attribute & Contact.ATTRIBUTE_UNKNOWN) == Contact.ATTRIBUTE_UNKNOWN) {
			ret = 5;
		} else {
			ret = -1;
		}
		return ret;
		
	}

	public String[] getNumbers() {
		return numbers;
	}

	public void readObject(DataInputStream in) throws IOException {
		boolean b = in.readBoolean();
		if (b) {
			this.lastName = in.readUTF();
		}
		b = in.readBoolean();
		if (b) {
			this.firstName = in.readUTF();
		}		
		this.attribute = in.readInt();
		int size = in.readByte();
		numbers = new String[size];
		for (int i = 0; i < size; i++) {
			b = in.readBoolean();			
			if (b) {
				numbers[i] = in.readUTF();
			}
		}
		
	}

	public void writeObject(DataOutputStream out) throws IOException {
		out.writeBoolean(lastName != null);
		if (lastName != null) {
			out.writeUTF(lastName);
		}
		out.writeBoolean(firstName != null);
		if (firstName != null) {
			out.writeUTF(firstName);
		}
		out.writeInt(attribute);
		out.writeByte(numbers.length);
		for (int i = 0; i< numbers.length; i++) {
			out.writeBoolean(numbers[i] != null);				
			if (numbers[i] != null) {
				out.writeUTF(numbers[i]);
			}
		}
	}
	
	public String toString() {
		StringBuffer sb = new StringBuffer("ContactImpl");
		sb.append("lastname=").append(lastName).
		append(" firstname=").append(firstName).
		append(" attribute=").append(attribute).
		append('[');
		for (int i = 0; i< numbers.length; i++) {
			sb.append(numbers[i]).append(',');
		}
		sb.append(']');
		return sb.toString();
	}

	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + attribute;
		result = PRIME * result + ((firstName == null) ? 0 : firstName.hashCode());
		result = PRIME * result + ((lastName == null) ? 0 : lastName.hashCode());
		result = PRIME * result + ContactImpl.hashCode(numbers);
		return result;
	}

	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final ContactImpl other = (ContactImpl) obj;
		if (attribute != other.attribute)
			return false;
		if (firstName == null) {
			if (other.firstName != null)
				return false;
		} else if (!firstName.equals(other.firstName))
			return false;
		if (lastName == null) {
			if (other.lastName != null)
				return false;
		} else if (!lastName.equals(other.lastName))
			return false;
		if (numbers != other.numbers)
			return false;
		return true;
	}
	
}
