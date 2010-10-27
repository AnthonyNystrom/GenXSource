package n2f.sup.core;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import javax.microedition.lcdui.Image;

import n2f.sup.common.utils.Serializable;


public class ItemBasis implements Serializable {
	private String title; // file or dir name//ok
	private Image icon;
	private int idOnForm;
	private boolean isSelected;

	public ItemBasis() {
	}
	
	public ItemBasis(String title) {
		this.title = title;
	}
	
	public ItemBasis(String title, Image icon){
		this(title);
		this.icon = icon;
	}
	
	public String getTitle() {
		return this.title;
	}
	public Image getIcon() {
		return this.icon;
	}
	
	public void setIcon(Image icon) {
		this.icon = icon;
	}

	public void setIdOnForm(int idOnForm) {
		this.idOnForm = idOnForm;
	}
	
	public int getIdOnForm() {
		return idOnForm;
	}

	public boolean isSelected() {
		return isSelected;
	}
	public void setSelected(boolean isSelected) {
		this.isSelected = isSelected;
	}

	public int hashCode() {
		final int PRIME = 31;
		int result = 1;
		result = PRIME * result + ((icon == null) ? 0 : icon.hashCode());
		result = PRIME * result + idOnForm;
		result = PRIME * result + (isSelected ? 1231 : 1237);
		result = PRIME * result + ((title == null) ? 0 : title.hashCode());
		return result;
	}

	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		final ItemBasis other = (ItemBasis) obj;
		if (icon == null) {
			if (other.icon != null)
				return false;
		} else if (!icon.equals(other.icon))
			return false;
//		if (idOnForm != other.idOnForm)
//			return false;
//		if (isSelected != other.isSelected)
//			return false;
		if (title == null) {
			if (other.title != null)
				return false;
		} else if (!title.equals(other.title))
			return false;
		return true;
	}

	public void writeObject(DataOutputStream out) throws IOException {
		out.writeUTF(title);
		out.writeInt(idOnForm);
		out.writeBoolean(isSelected);
	}

	public void readObject(DataInputStream in) throws IOException {
		this.title = in.readUTF();
		this.idOnForm = in.readInt();
		this.isSelected = in.readBoolean();
	}
	
}
