
package n2f.blogger.core;

import n2f.blogger.webservice.NetworkServiceTaskAdapter;

/**
 * An event which indicates that a component-defined error action occurred. 
 * The event is passed to every ErrorListener object that registered to receive such set of events 
 * using the component's addActionListener method.
 * 
 */
public class ErrorEvent
{
    public static final int OPERATION_FAILED = 4;
    public static final int GET_CREDENTIALS =
	    NetworkServiceTaskAdapter.TYPE_GET_CREDENTIALS;
    private final Object source;
    private final Throwable throwable;
    private final String explanation;
    private int errorId;

    /**
     * Creates a new instance of the <tt>ErrorEvent</tt> class.
     * 
     * @param source      - the object that originated the event
     * @param throwable   - an exception
     * @param explanation - explanation of invoked exception
     */
    public ErrorEvent(Object source, Throwable throwable, String explanation,
		       int errorId)
    {
	this.source = source;
	this.throwable = throwable;
	this.explanation = explanation;
	this.errorId = errorId;
    }

    /**
     * Gets error source - the object that originated the event.
     * @return source - the object that originated the event.
     */
    public Object getSource()
    {
	return this.source;
    }

    /**
     * Gets error.
     * @return error - instance of Throwable. 
     */
    public Throwable getError()
    {
	return this.throwable;
    }

    /**
     * Gets error explanation.
     * @return error explanation.
     */
    public String getExplanation()
    {
	return this.explanation;
    }

    /**
     * Gets error identifier.
     * @return the errorId.
     */
    public int getErrorId()
    {
	return errorId;
    }

    /**
     * Returns string representation of ErrorEvent instances.
     */
    public String toString()
    {
	StringBuffer sb = new StringBuffer("Source=[");
	sb.append(source.toString());
	sb.append("] throwable=");
	sb.append(throwable.toString());
	sb.append(" Explanation=");
	sb.append(explanation);
	sb.append(" ErrorId=");
	sb.append(errorId);
	return sb.toString();
    }

}	
