package n2f.sms.ui;

import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.ItemStateListener;
import javax.microedition.lcdui.StringItem;
import javax.microedition.lcdui.TextField;

import n2f.contacts.Contact;
import n2f.contacts.ui.ContactsBean;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.ui.AbstractWindow;
import com.genetibase.askafriend.ui.LocaleUI;
import com.genetibase.askafriend.ui.UIManager;
//import com.genetibase.askafriend.utils.Debug;
import com.genetibase.askafriend.utils.Debug;



public class SMSPreviewScreen extends AbstractWindow implements ItemStateListener {
	private static final String EMPTY = "";
	private static final String COLON = ":";
	private static final String SPACE = " ";
	private Form form;
	private ChoiceGroup cgPhones;
	private TextField edPhone;
	private Command cmdBack, cmdCancel, cmdSend, cmdEditMessage;
	private SMSBean bean;
	private StringItem /*invitationPreview,*/ edName;
		
	public SMSPreviewScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
			form = new Form(getLocalizedText(id));
			ContactsBean cBean = (ContactsBean) Engine.getEngine().getBean(UIManager.SCREEN_CONTACTS);
			if (cBean != null) {
				bean = (SMSBean) Engine.getEngine().getBean(getId());
				if (bean == null) {
					bean = new SMSBean(cBean.getContact());
					Engine.getEngine().putBean(getId(), bean);
					SMSBean.setInfo(getLocalizedText(LocaleUI.SMS_PREVIEW_INPUT_DEAR_LABEL)+ SPACE+bean.getName()+getLocalizedText(LocaleUI.SMS_PREVIEW_DEFAULT_MESSAGE));
				} else {
					if (bean.setContact(cBean.getContact())) {
						bean.setPhoneIndex(0);
						SMSBean.setInfo(getLocalizedText(LocaleUI.SMS_PREVIEW_INPUT_DEAR_LABEL)+ SPACE+bean.getName()+getLocalizedText(LocaleUI.SMS_PREVIEW_DEFAULT_MESSAGE));
					}
				}
				
				edName = new StringItem(getLocalizedText(LocaleUI.SMS_PREVIEW_INPUT_TO_LABEL), bean.getName());
				form.append(edName);
				cgPhones = new ChoiceGroup(getLocalizedText(LocaleUI.SMS_PREVIEW_CHOICE_PHONES_LABEL),ChoiceGroup.POPUP);
				edPhone = new TextField(getLocalizedText(LocaleUI.SMS_PREVIEW_INPUT_PHONE_LABEL), EMPTY, 20, TextField.PHONENUMBER);
//				
				fillChoiceGroup();
//				if (cgPhones.size() > 0)
//					cgPhones.setSelectedIndex(bean.getPhoneIndex(), true);
				form.append(cgPhones);
				
//				setPhone(cgPhones);
				
				form.append(edPhone);
				form.append(new StringItem(getLocalizedText(LocaleUI.SMS_PREVIEW_MESSAGE),null));
				
				form.append(/*invitationPreview = new StringItem(null, */SMSBean.getInfo()/*)*/);
				form.addCommand(cmdBack = new Command(getLocalizedText(LocaleUI.SMS_PREVIEW_COMMAND_BACK), Command.BACK, 2));
				form.addCommand(cmdCancel = new Command(getLocalizedText(LocaleUI.SMS_PREVIEW_COMMAND_CANCEL), Command.CANCEL, 3));
				form.addCommand(cmdSend = new Command(getLocalizedText(LocaleUI.SMS_PREVIEW_COMMAND_SEND), Command.SCREEN, 0));	
				form.addCommand(cmdEditMessage = new Command(getLocalizedText(LocaleUI.SMS_PREVIEW_COMMAND_EDIT), Command.SCREEN, 1));
				form.setItemStateListener(this);
			}
	}

	protected Displayable getScreen() {
		return form;
	}

	public void show() {
		form.setCommandListener(this);
        getDisplay().setCurrent(form);
	}
	
	public int getFormHeight() {
		return form.getHeight();
	}

	public int getFormWidth() {
		return form.getWidth();
	}

	protected void refresh() {
		hideBusy();
		UIManager.getInstance().showDefault();
	}
	
	private void doCancel() {
		UIManager.getInstance().showDefault();		
	}

	private void doSend() {
		if (bean != null) {
			bean.setSelectedPhoneIndex(cgPhones.getSelectedIndex());
//			bean.setName(edName.getString());
			bean.setPhoneNumber(edPhone.getString());
			showBusy(null);
			Engine.getEngine().sendSMS(bean, this);
		}		
	}
	private void doEditMessage() {
		UIManager.getInstance().show(UIManager.SCREEN_SMS_EDIT);
	} 
	
	private void doBack() {
		UIManager.getInstance().showPrevious();		
	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == cmdBack) {
			doBack();
		} else if (c == cmdSend) {
			doSend();
		} else if (c == cmdCancel) {
			doCancel();
		} else if (c == cmdEditMessage) {
			doEditMessage();
		}
	}

	
	private static final int getColonIndex(String selected) {
		return selected.indexOf(COLON);
	}
	public void itemStateChanged(Item item) {
		if (item instanceof ChoiceGroup) {
//			System.out.println("state changed");		
			ChoiceGroup cg = (ChoiceGroup) item;
			setPhone(cg);
//			System.out.println("----state changed---");			
		}
	}
	
	public void setPhone(ChoiceGroup cg) {
		if (cgPhones.size() > 0) {
			String selected = cg.getString(bean.getPhoneIndex());
			if (selected != null) {
				try {
					int colonIndex = getColonIndex(selected);
//				System.out.println("Constructor" + selected + " " + colonIndex); 
//				System.out.println("selected.substring(index)");					
					edPhone.setString(colonIndex>0 ? selected.substring(colonIndex): selected);
				} catch (Exception e) {
					Debug.println("Format exception " + e.getMessage());
				}
			}
		}
	}	
	
	public void fillChoiceGroup()
	{
		Contact contact = bean.getContact();
		int attributes = contact.getAttributes();
		//TODO: use pics for the numbers
		String text = null;
		if ((attributes & Contact.ATTRIBUTE_MOBILE) == Contact.ATTRIBUTE_MOBILE) {
			text = contact.getNumber(Contact.ATTRIBUTE_MOBILE);
			if (text !=null) {
				cgPhones.append("Mobile:"+text, null);
				if (edPhone.getString().length()==0)
					edPhone.setString(text);
			}
		}
		if ((attributes & Contact.ATTRIBUTE_HOME) == Contact.ATTRIBUTE_HOME) {
			text = contact.getNumber(Contact.ATTRIBUTE_HOME);
			if (text !=null) {
				cgPhones.append("Home:"+text, null);
				if (edPhone.getString().length()==0)
					edPhone.setString(text);
			}
		}
		if ((attributes & Contact.ATTRIBUTE_FAX) == Contact.ATTRIBUTE_FAX) {
			text = contact.getNumber(Contact.ATTRIBUTE_FAX);
			if (text !=null) {
				cgPhones.append("Fax:"+text, null);
				if (edPhone.getString().length()==0)
					edPhone.setString(text);
			}
		}
		if ((attributes & Contact.ATTRIBUTE_WORK) == Contact.ATTRIBUTE_WORK) {
			text = contact.getNumber(Contact.ATTRIBUTE_WORK);
			if (text !=null) {
				cgPhones.append("Work:"+text, null);
				if (edPhone.getString().length()==0)
					edPhone.setString(text);
			}
		}
		if ((attributes & Contact.ATTRIBUTE_UNKNOWN) == Contact.ATTRIBUTE_UNKNOWN) {
			text = contact.getNumber(Contact.ATTRIBUTE_UNKNOWN);
			if (text !=null) {
				cgPhones.append(text, null);
				if (edPhone.getString().length()==0)
					edPhone.setString(text);
			}
		}
		if ((attributes & Contact.ATTRIBUTE_AUTO) == Contact.ATTRIBUTE_AUTO) {
			text = contact.getNumber(Contact.ATTRIBUTE_AUTO);
			if (text !=null) {
				cgPhones.append(text, null);
				if (edPhone.getString().length()==0)
					edPhone.setString(text);
			}
		}
	}

}
