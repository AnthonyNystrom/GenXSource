package n2f.contacts.ui;

import n2f.contacts.ContactsManager;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.utils.RunnableTaskAdapter;


public class ContactsReaderTask extends RunnableTaskAdapter {
	private boolean isInterrupted;
	private Resoursable resoursable;
	public ContactsReaderTask(Resoursable resoursable) {
		this.resoursable = resoursable;
	}

	protected void logic() throws Exception {
		//TODO: should we try Unsupported state here? 
		ContactsManager.getInstance().updateContactsFromAddressBook();
		if (!isInterrupted) {
			ContactsManager.getInstance().serializeContacts(resoursable);
		}
	}

	public int getType() {
		// TODO Auto-generated method stub
		return 0;
	}

	public void interrupt() {
		super.interrupt();
		this.isInterrupted = true;
		ContactsManager.getInstance().interrupt();
	}
	
	
}
