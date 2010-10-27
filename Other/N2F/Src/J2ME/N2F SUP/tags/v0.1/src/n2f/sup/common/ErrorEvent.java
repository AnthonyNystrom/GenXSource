package n2f.sup.common;

import n2f.sup.common.file.FileOperation;
import n2f.sup.common.network.NetworkServiceTaskAdapter;

/**
 * An event which indicates that a component-defined error action occurred. 
 * The event is passed to every ErrorListener object that registered to receive such set of events 
 * using the component's addActionListener method.
 * 
 */
public class ErrorEvent {
//	public static final int MEDIA_FAILURE = 0;
//	public static final int NEW_REPLY = 1;
//	public static final int NEW_PRIVATE_QUESTION = 2;
	public static final int OPERATION_FAILED = 4;
	
	public static final int INIT = FileOperation.INIT;
	public static final int OPEN = FileOperation.OPEN;
	public static final int DELETE = FileOperation.DELETE;
	public static final int RENAME = FileOperation.RENAME;
	public static final int MKDIR = FileOperation.MKDIR;
	public static final int PREVIEW = FileOperation.PREVIEW;
	public static final int CREATE = FileOperation.CREATE;
	public static final int MODIFY = FileOperation.MODIFY;
	public static final int SAVE_DATA = FileOperation.SAVE_DATA;
	public static final int SCALE_IMAGE = FileOperation.TYPE_SCALE_IMAGE;
	 
	public static final int GET_CREDENTIALS = NetworkServiceTaskAdapter.TYPE_GET_CREDENTIALS;
	public static final int UPLOAD_PHOTO = NetworkServiceTaskAdapter.TYPE_SUP_UPLOAD_PHOTO;
	
    private final Object source;
    private final Throwable throwable;
    private final String explanation;
	private int errorId;

    /**
     * constructor
     * @param source      - the object that originated the event
     * @param throwable   - an exception
     * @param explanation - explanation of invoked exception
     */
    public ErrorEvent(Object source, Throwable throwable, String explanation, int errorId) {
        this.source = source;
        this.throwable = throwable;
		this.explanation = explanation;
		this.errorId = errorId;
    }
	
	/**
	 * Gets error source - the object that originated the event.
	 * @return source - the object that originated the event.
	 */
    public Object getSource() {
        return this.source;
    }
	
	/**
	 * Gets error.
	 * @return error - instance of Throwable. 
	 */
    public Throwable getError() {
        return this.throwable;
    }
	
	/**
	 * Gets error explanation.
	 * @return error explanation.
	 */
    public String getExplanation() {
        return this.explanation;
    }

	/**
	 * Gets error identifier.
	 * @return the errorId.
	 */
	public int getErrorId() {
		return errorId;
	}
    
	/**
	 * Returns string representation of ErrorEvent instances.
	 */
    public String toString() {
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