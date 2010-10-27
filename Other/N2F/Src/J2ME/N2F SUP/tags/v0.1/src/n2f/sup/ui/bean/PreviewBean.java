package n2f.sup.ui.bean;

import n2f.sup.core.ItemLight2;

public class PreviewBean extends AbstractBean {
	private ItemLight2 item;
	
	public void clean(){
		item = null;
	}

	public ItemLight2 getItem() {
		return item;
	}

	public void setItem(ItemLight2 item) {
		this.item = item;
	}
}
