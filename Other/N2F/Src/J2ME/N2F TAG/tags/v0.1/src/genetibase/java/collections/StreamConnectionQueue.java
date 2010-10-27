/* ------------------------------------------------
 * StreamConnectionQueue.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.collections;

import java.util.NoSuchElementException;
import javax.microedition.io.StreamConnection;

/**
 * Represents a first-in, first-out collection of StreamConnection instances.
 *
 * @author Alex Nesterov
 */
public class StreamConnectionQueue
{
    /* ------- Methods.Public ------ */
    
    /**
     * Tests if the specified <tt>element</tt> is an item in this queue.
     *
     * @param element	Specifies the element to check.
     * @return	<code>true</code> if the specified <tt>element</tt> is an item
     *		in this queue; <code>false</code> otherwise.
     */
    public boolean contains(StreamConnection element)
    {
	StreamConnectionQueueItem item = _tailItem.getPreviousItem();
	
	while (item != _headItem)
	{
	    if (element == item.getValue())
	    {
		return true;
	    }
	    
	    item = item.getPreviousItem();
	}
	
	return false;
    }
    
    /**
     * Removes and returns a StreamConnection instance at the beginning of this
     * queue.
     *
     * @return A StreamConnection instance at the beginning of this queue.
     * @throws	NoSuchElementException if this queue is empty.
     */
    public StreamConnection dequeue()
    {
	if (isEmpty())
	{
	    throw new NoSuchElementException();
	}
	
	StreamConnectionQueueItem item = _tailItem.getPreviousItem();
	StreamConnectionQueueItem previousItem = item.getPreviousItem();
	
	previousItem.setNextItem(_tailItem);
	_tailItem.setPreviousItem(previousItem);
	_size--;
	
	return item.getValue();
    }
    
    /**
     * Adds the specified <tt>connection</tt> to the end of this queue.
     *
     * @param connection    Specifies the StreamConnection instance to add to
     *			    the end of this queue.
     */
    public void enqueue(StreamConnection connection)
    {
	StreamConnectionQueueItem nextItem = _headItem.getNextItem();
	StreamConnectionQueueItem item = new StreamConnectionQueueItem(_headItem, nextItem, connection);
	_headItem.setNextItem(item);
	nextItem.setPreviousItem(item);
	
	_size++;
    }
    
    /**
     * Tests if the queue contains no elements.
     * @return	<code>true</code> of this queue contains no items;
     *		<code>false</code> otherwise.
     */
    public boolean isEmpty()
    {
	if (_headItem.getNextItem() == _emptyItem)
	{
	    return _tailItem.getPreviousItem() == _emptyItem;
	}
	
	return false;
    }
    
    /**
     * Returns a StreamConnection instance at the beginning of this queue
     * without removing it.
     *
     * @return	A StreamConnection instance at the beginning of this queue
     *		without removing it.
     */
    public StreamConnection peek()
    {
	if (isEmpty())
	{
	    throw new NoSuchElementException();
	}
	
	return _tailItem.getPreviousItem().getValue();
    }
    
    /**
     * Removes all the elements from this queue and sets its size to 0.
     */
    public void removeAllElements()
    {
	if (!isEmpty())
	{
	    initializeComponent();
	}
    }
    
    private int _size;
    
    /**
     * Returns the number of elements in this queue.
     * @return	The number of elements in this queue.
     */
    public int size()
    {
	return _size;
    }
    
    /* ------ Methods.Private ------ */
    
    private void initializeComponent()
    {
	_emptyItem = StreamConnectionQueueItem.createEmptyItem();
	_headItem = _tailItem = _emptyItem;
	_headItem.setNextItem(_tailItem);
	_tailItem.setPreviousItem(_headItem);
	_size = 0;
    }
    
    /* ------- Declarations ------ */
    
    private StreamConnectionQueueItem _emptyItem;
    private StreamConnectionQueueItem _headItem;
    private StreamConnectionQueueItem _tailItem;
    
    /* ------- Constructors ------ */
    
    /**
     * Creates a new instance of StreamConnectionQueue.
     */
    public StreamConnectionQueue()
    {
	initializeComponent();
    }
}
