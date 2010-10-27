package com.genetibase.askafriend.common.network;

public class ServiceResponse 
{
	private int requestType;
	private Object source;
	
	public ServiceResponse(int requestType, Object source) {
		this.requestType = requestType;
		this.source = source; 
	}

	public int getRequestType() {
		return requestType;
	}

	public Object getSource() {
		return source;
	}
}
