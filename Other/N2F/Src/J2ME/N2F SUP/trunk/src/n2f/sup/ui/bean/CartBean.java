package n2f.sup.ui.bean;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Enumeration;
import java.util.Vector;

import n2f.sup.common.Resoursable;
import n2f.sup.common.utils.Serializable;
import n2f.sup.core.ItemLight2;


public class CartBean extends AbstractBean implements Serializable {
	private final String cardId;
	private boolean isUpdated = false;
	private boolean proceedCommandNameIsNext = false;
	public CartBean(Resoursable resoursable) {
//		if (resoursable != null) {
			String classname = resoursable.getMidlet().getClass().getName();
			int min = Math.min(classname.length(), 10);
			this.cardId = classname.substring(classname.length() - min, classname.length());
			resoursable.getProperty(cardId, this);
//		}
	}
	
	private Vector itemsLight = new Vector();
	
	public void addItem(ItemLight2 itemLight){
		if (itemLight != null && !itemsLight.contains(itemLight)) {
			itemsLight.addElement(itemLight);
			isUpdated = true;
		}
	}
	
	public Enumeration getItems(){
		return itemsLight.elements();
	}
	
	public Vector getItemsVector(){
		return itemsLight;
	}
	
	public boolean containsItem(ItemLight2 item) {
		boolean ret = false;
		if (item != null)
			ret = itemsLight.contains(item);
		return ret;
	}
	
	public int getSize(){
		return itemsLight.size();
	}
	public ItemLight2 getItem(int index){
		return (ItemLight2)itemsLight.elementAt(index);
	}
	
	public void removeItem(int index) {
		itemsLight.removeElementAt(index);
		isUpdated = true;
	}

	public void removeItem(ItemLight2 item) {
		if (itemsLight.contains(item)) {
			isUpdated = true;
			itemsLight.removeElement(item);			
		}
	}
	
	public void removeAll() {
		isUpdated = true;
		itemsLight.removeAllElements();
	}
	
	public void clean(){
		if (itemsLight.size() > 0 ) {
			itemsLight.removeAllElements();
			isUpdated = true;
		}
	}
	
	public void saveBean(Resoursable resoursable) {
		if (isUpdated) {
			resoursable.setProperty(cardId, this);			
		}
	}

	public void writeObject(DataOutputStream out) throws IOException {
		out.writeInt(itemsLight.size());
		for (Enumeration e = this.itemsLight.elements(); e.hasMoreElements(); ) {
			ItemLight2 item = (ItemLight2)e.nextElement();
			item.writeObject(out);
		}
		
	}

	public void readObject(DataInputStream in) throws IOException {
		int size = in.readInt();
		for (int i = 0; i < size; i++) {
			ItemLight2 item = new ItemLight2();
			item.readObject(in);
			itemsLight.addElement(item);
		}
	}
	
	public void setProceedCommandName(boolean next) {
		this.proceedCommandNameIsNext = next;
	}

	public boolean nextIsProceedCommandName() {
//		System.out.println("proceedCommandNameIsNext="+proceedCommandNameIsNext);
		return proceedCommandNameIsNext;
	}
}