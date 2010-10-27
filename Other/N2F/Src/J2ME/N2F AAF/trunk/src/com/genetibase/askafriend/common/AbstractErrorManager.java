package com.genetibase.askafriend.common;

import java.util.Enumeration;
import java.util.Vector;

import com.genetibase.askafriend.utils.Debug;


/**
* This abstract class is the base of Error Management System 
*/
public abstract class AbstractErrorManager 
{
	protected Vector errorListenerList = new Vector();
	
	public void addErrorListener(ErrorListener errorListener) {
		if (errorListener != null && !errorListenerList.contains(errorListener)) {
			errorListenerList.addElement(errorListener);
		}
	}

	public void removeErrorListener(ErrorListener errorListener) {
		if (errorListener != null)
			errorListenerList.removeElement(errorListener);
	}
	
	public void removeAllErrorListeners() {
		errorListenerList.removeAllElements();
	}

	protected void fireError(String concoleComment, ErrorEvent event) {
		if (event != null)
			showErrorInLog(concoleComment, event.getError());
		for (Enumeration enumm = errorListenerList.elements(); enumm.hasMoreElements();) {
			((ErrorListener) enumm.nextElement()).actionPerformed(event);
		}
	}

	protected void showErrorInLog(String comment, Throwable t) {
		if (Debug.isDebug()) {
			Debug.error(comment, t);
		}
	}
	
	protected boolean isDebug() {
		return Debug.isDebug();
	}
	
	protected void log(String str) {
		Debug.println(str);
	}
	
	/**
	 * Returns string representation for AbstractErrorManager instances.
	 */
	public String toString() {
		StringBuffer sb = new StringBuffer("AbstractErrorManager[\n");
		for (Enumeration enumm = errorListenerList.elements(); enumm.hasMoreElements();) {
			sb.append(enumm.nextElement().toString());
			sb.append('\n');
		}
		sb.append(']');
		return sb.toString();
	}
	
}