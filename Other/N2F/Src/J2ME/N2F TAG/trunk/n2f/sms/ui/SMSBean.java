package n2f.sms.ui;

import n2f.contacts.Contact;
import n2f.tag.ui.AbstractBean;


public class SMSBean extends AbstractBean 
{
	private int phoneIndex;
	private Contact contact;  
	private String name;
	private String phoneNumber;
	private static String info = null; 
	
	public SMSBean(Contact c) {
		this.contact = c;
	}
	
	public Contact getContact() {
		return contact;
	}

	public boolean setContact(Contact c) {
		if (c != null && !c.equals(contact)) {
			this.contact = c;
			return true; 
		}
		return false;
	}
	
	public String getName()
	{
		name = contact.getFirstName()+ " "+ contact.getLastName();
		return name;
	}

	public String getPhoneNumber() {
		return phoneNumber;
	}

	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}

	public void setName(String name) {
		this.name = name;
	}
	
	public int getSelectedPhoneIndex() {
		return phoneIndex;
	}

	public void setSelectedPhoneIndex(int phoneIndex) {
		this.phoneIndex = phoneIndex;
	}
	
//	public String getInvitation() {
//		return "Dear, "+ getName()+ info;
//	}

	public static String getInfo() {
		return info;
	}

	public static void setInfo(String info) {
		SMSBean.info = info;
	}

	public int getPhoneIndex() {
		return phoneIndex;
	}

	public void setPhoneIndex(int phoneIndex) {
		this.phoneIndex = phoneIndex;
	}
	
}
