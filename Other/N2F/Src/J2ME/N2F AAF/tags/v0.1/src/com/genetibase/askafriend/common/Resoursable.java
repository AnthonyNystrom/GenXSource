package com.genetibase.askafriend.common;

import javax.microedition.midlet.MIDlet;

import com.genetibase.askafriend.common.utils.Serializable;

/**
 */
public interface Resoursable {
	String getProperty(String key);
	void setProperty(String key, String value);	
	String getProperty(String key, String defaultValue) ;
	String getAppProperty(String key);
	String getLocale(String key);
	MIDlet getMidlet();
	
	void setProperty(String key, Serializable value);
	void getProperty(String key, Serializable value);
	boolean isTouch();
}

