package n2f.sup.core;

public class DataWrapper {
	private final String id;
	private long size;

	public DataWrapper(String id) {
		super();
		this.id = id;
	}
	
	public long getSize() {
		return size;
	}
	public void setSize(long size) {
		this.size = size;
	}
	public String getId() {
		return id;
	}
	
}
