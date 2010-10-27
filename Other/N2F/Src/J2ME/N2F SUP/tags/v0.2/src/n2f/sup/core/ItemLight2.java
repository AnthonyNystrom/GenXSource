package n2f.sup.core;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import javax.microedition.lcdui.Image;

public class ItemLight2 extends ItemBasis {
	public final static Image IMAGE_FOLDER = Engine.getEngine().getImage("/folder.png");
	public final static Image IMAGE_FILE = Engine.getEngine().getImage("/file.png");
	public static final byte TYPE_FILE = -1;
	public static final byte TYPE_FOLDER = 1;
	
	private String path; 
	private long size;
	private byte type;

	public ItemLight2 (String title, String path, Image icon, byte type) {
		super(title, icon);
		this.path = path;
		this.type = type;
	}
	
	public ItemLight2() {
		super();
	}
	
	
	public long getSize() {
		return size;
	}
	public void setSize(long size) {
		this.size = size;
	}
	public String getPath() {
		return path;
	}

	public byte getType() {
		return type;
	}
	
	public int hashCode() {
		final int PRIME = 31;
		int result = super.hashCode();
		result = PRIME * result + (int) (size ^ (size >>> 32));
		result = PRIME * result + ((path == null) ? 0 : path.hashCode());
		result = PRIME * result + type;
		return result;
	}
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (!super.equals(obj))
			return false;
		if (getClass() != obj.getClass())
			return false;
		final ItemLight2 other = (ItemLight2) obj;
		if (size != other.size)
			return false;
		if (path == null) {
			if (other.path != null)
				return false;
		} else if (!path.equals(other.path))
			return false;
		if (type != other.type)
			return false;
		return true;
	}

	public void writeObject(DataOutputStream out) throws IOException {
		super.writeObject(out);
		out.writeUTF(path);
		out.writeLong(size);
		out.writeByte(type);
	}

	public void readObject(DataInputStream in) throws IOException {
		super.readObject(in);
		this.path = in.readUTF();
		this.size = in.readLong();
		this.type = in.readByte();
		if (type == TYPE_FILE) {
			setIcon(IMAGE_FILE);
		} else if (type == TYPE_FOLDER) {
			setIcon(IMAGE_FOLDER);
		}
	}
}