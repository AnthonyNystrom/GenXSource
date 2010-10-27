package n2f.contacts.ui;

import n2f.contacts.ContactsManager;
import n2f.contacts.core.Resoursable;
import n2f.tag.webservice.utils.RunnableTaskAdapter;


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
		return PIM_TASK;
	}

	public void interrupt() {
		super.interrupt();
		this.isInterrupted = true;
		ContactsManager.getInstance().interrupt();
	}
	
	
}
