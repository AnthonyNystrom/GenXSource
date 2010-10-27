package com.genetibase.askafriend.core;

import javax.microedition.lcdui.Image;

public class ActionItem extends ItemBasis {
	private final byte action;
	
	public ActionItem(String title, Image icon, byte action) {
		super(title, icon);
		this.action = action;
	}
	
	public byte getAction() {
		return action;
	}

	public int hashCode() {
//		final int PRIME = 31;
		int result = super.hashCode();
		return result;
	}

	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (!super.equals(obj))
			return false;
		if (getClass() != obj.getClass())
			return false;
		final ActionItem other = (ActionItem) obj;
		if (action != other.action)
			return false;
		return true;
	}
	
	
	
}
