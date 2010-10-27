package n2f.sup.core;

import java.io.InputStream;

public class StreamWrapper extends DataWrapper{
	private InputStream inputStream = null;
	private long creationDate = 0; 
	public StreamWrapper(String id) {
		super(id);
	}
	public InputStream getInputStream() {
		return inputStream;
	}
	public void setInputStream(InputStream inputStream) {
		this.inputStream = inputStream;
	}
	public long getCreationDate() {
		return creationDate;
	}
	public void setCreationDate(long creationDate) {
		this.creationDate = creationDate;
	}

}
