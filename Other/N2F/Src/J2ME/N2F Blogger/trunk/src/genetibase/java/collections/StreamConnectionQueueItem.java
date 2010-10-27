/* ------------------------------------------------
 * StreamConnectionQueueItem.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.collections;

import javax.microedition.io.StreamConnection;

/**
 * Represents an item in a StreamConnectionQueue.
 *
 * @author Alex Nesterov
 */
class StreamConnectionQueueItem
{
    /* ------ Methods.Public ------- */ 
    
    private StreamConnectionQueueItem _nextItem;
    
    /**
     * Returns the next item for this item in a StreamConnectionQueue.
     * @return	The next item for this item in a StreamConnectionQueue.
     */
    public StreamConnectionQueueItem getNextItem()
    {
	return _nextItem;
    }
    
    /**
     * Sets the next item for this item in a StreamConnectionQueue.
     * @param nextItem	Specifies the next item for this item.
     */
    public void setNextItem(StreamConnectionQueueItem nextItem)
    {
	_nextItem = nextItem;
    }
    
    private StreamConnectionQueueItem _previousItem;
    
    /**
     * Returns the previous item for this item in a StreamConnectionQueue.
     * @return	The previous item for this item in a StreamConnectionQueue.
     */
    public StreamConnectionQueueItem getPreviousItem()
    {
	return _previousItem;
    }
    
    /**
     * Sets the previous item for this item in a StreamConnectionQueue.
     * @param previousItem  Specifies the previous item for this item.
     */
    public void setPreviousItem(StreamConnectionQueueItem previousItem)
    {
	_previousItem = previousItem;
    }
    
    private StreamConnection _value;
    
    /**
     * Returns the value this item contains.
     * @return	The value this item contains.
     */
    public StreamConnection getValue()
    {
	return _value;
    }
    
    /**
     * Sets the value for this item.
     * @param value Specifies the value for this item.
     */
    public void setValue(StreamConnection value)
    {
	_value = value;
    }
    
    /* ------ Methods.Public.Static ------ */
    
    /**
     * Returns a new empty StreamConnectionQueueItem instance. It is used to
     * determine whether the iterator is at the beginning or at the end of the
     * queue, or the queue is empty.
     */
    public static StreamConnectionQueueItem createEmptyItem()
    {
	return new StreamConnectionQueueItem();
    }
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of StreamConnectionQueueItem.
     */
    private StreamConnectionQueueItem()
    {
    }
    
    /**
     * Creates a new instance of StreamConnectionQueueItem.
     *
     * @param previousItem  Specifies the previous item for this item.
     *			    Can be <code>null</code>.
     * @param nextItem	    Specifies the next item for this item.
     *			    Can be <code>null</code>.
     * @param value	    Specifies the value for this item.
     *			    Can be <code>null</code>.
     */
    public StreamConnectionQueueItem(
	    StreamConnectionQueueItem previousItem
	    , StreamConnectionQueueItem nextItem
	    , StreamConnection value
	    )
    {
	_previousItem = previousItem;
	_nextItem = nextItem;
	_value = value;
    }
}
