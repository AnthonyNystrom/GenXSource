package com.genetibase.askafriend.core;

import java.io.InputStream;

public class StreamWrapper extends DataWrapper{
	private InputStream inputStream = null;
	public StreamWrapper(String id) {
		super(id);
	}
	public InputStream getInputStream() {
		return inputStream;
	}
	public void setInputStream(InputStream inputStream) {
		this.inputStream = inputStream;
	}

}
