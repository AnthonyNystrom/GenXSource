package n2f.sup.ui.bean;

import java.util.Enumeration;
import java.util.Vector;

import n2f.sup.core.ActionItem;


public class StartBean extends AbstractBean {

	private Vector items = new Vector(); 
	
	public StartBean(){
		super();
	}

	public void addItem(ActionItem itemBasis) {
		items.addElement(itemBasis);
	}
	
	public ActionItem getItem(int index) {
		return (ActionItem)items.elementAt(index);
	}
	
	public void removeItem(ActionItem itemBasis) {
		items.removeElement(itemBasis);
	}
	
	public void clear(){
		items.removeAllElements();
	} 
	
	public Enumeration getElements(){
		return items.elements();
	}
	
	
	
}
