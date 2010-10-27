package com.genetibase.askafriend.camera;

public class SnapshotEncodingWrapper {
	private String encName;
	private int height, width;
	private static String ENCODING = "encoding=";
	private static String HEIGHT = "&height=";
	private static String WIDTH = "&width=";
	private static String AMPERS = "&";
	private static String EMPTY = " ";
	private static String X = "x";
	
	public SnapshotEncodingWrapper(String encoding) {
		parseWrapper(encoding); 
	}
	
	public SnapshotEncodingWrapper(String encoding, int width, int height) {
		this.encName = encoding;
		this.width = width;
		this.height = height;
	}
	
	private void parseWrapper(String encoding) {
		String enc = encoding;
		
		int index1 = enc.indexOf(ENCODING); 
		int ampIndex = enc.indexOf(AMPERS, index1+1)<0? encoding.length(): enc.indexOf(AMPERS, index1+1);
		this.encName = enc.substring(index1+ENCODING.length(), ampIndex).trim();
		index1 = -1;
		index1 = enc.indexOf(WIDTH);
		if (index1 > -1) {
			ampIndex = enc.indexOf(AMPERS, index1+1)<0? encoding.length(): enc.indexOf(AMPERS, index1+1);
			this.width = Integer.parseInt(enc.substring(index1+WIDTH.length(), ampIndex).trim());
		}
		index1 = -1;
		index1 = enc.indexOf(HEIGHT);
		if (index1 > -1) {
			ampIndex = enc.indexOf(AMPERS, index1+1)<0? encoding.length(): enc.indexOf(AMPERS, index1+1);
			this.height = Integer.parseInt(enc.substring(index1+HEIGHT.length(), ampIndex).trim());
		}
	}

	public String getEncName() {
		return encName;
	}
	public void setEncName(String encName) {
		this.encName = encName;
	}
	public int getHeight() {
		return height;
	}
	public void setHeight(int height) {
		this.height = height;
	}
	public int getWidth() {
		return width;
	}
	public void setWidth(int width) {
		this.width = width;
	} 
	
	public String toString() 
	{
		StringBuffer sb = new StringBuffer(ENCODING).append(encName);
		if (this.height > 0)
			sb.append(HEIGHT).append(height);
		if (this.width > 0)
			sb.append(WIDTH).append(width);
		return sb.toString();
	}
	
	public String getDescription() {
		StringBuffer sb = new StringBuffer();
		sb.append(encName);
		if (height>0 && (width>0))
			sb.append(EMPTY).append(height).append(X).append(width);
		else 
			sb.append(EMPTY).append("(device specific)");
		return sb.toString(); 
	}
	
	public boolean equals(Object obj)
	{
		if (this.getClass() != obj.getClass())
			return false;
		
		SnapshotEncodingWrapper otherEncoding = (SnapshotEncodingWrapper)obj; 
		
		if (!this.encName.equals(otherEncoding.getEncName()))
			return false;
		if (this.height != otherEncoding.getHeight())
			return false;
		if (this.width != otherEncoding.getWidth())
			return false;
		
		return true;
	}

}
