package n2f.sup.common;

/**
 * The listener interface for receiving action events. 
 * The class that is interested in processing an error event implements this interface, 
 * and the object created with that class is registered with a component, 
 * using the component's addErrorListener method. 
 * When the error event occurs, that object's actionPerformed method is invoked 
 * 
 */

public interface ErrorListener {

	/**
	 * Invoked when an error occurs
	 * @param errorEvent
	 */
    void actionPerformed(ErrorEvent errorEvent);
	
}
