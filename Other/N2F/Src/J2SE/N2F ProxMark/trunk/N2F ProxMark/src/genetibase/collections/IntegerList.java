/* ------------------------------------------------
 * IntegerList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.collections;

/**
 * Represents a growable list of integer values.
 *
 * @author Alex Nesterov
 */
public class IntegerList
{
    /* ------ Methods.Public ------ */
    
    /**
     * Adds the specified element to the end of this list, increasing its size
     * by one.
     *
     * @param element	Specifies the element to add.
     */
    public void addElement(int element)
    {
	IntegerListItem item = new IntegerListItem(_headItem, element);
	_headItem.setNextItem(item);
	_headItem = item;
	
	_size++;
    }
    
    /**
     * Tests if the specified <tt>element</tt> is an item in this list.
     *
     * @param element	Specifies the element to check.
     * @return	<code>true</code> if the specified <tt>element</tt> is an item
     *		in this list; <code>false</code> otherwise.
     */
    public boolean contains(int element)
    {
	IntegerListItem bufferItem = _headItem;
	
	while (bufferItem != _emptyItem)
	{
	    int currentValue = bufferItem.getValue();
	    
	    if (currentValue == element)
	    {
		return true;
	    }
	    
	    bufferItem = bufferItem.getPreviousItem();
	}
	
	return false;
    }
    
    /**
     * Copies the items of this list into the specified <tt>array</tt>. The size
     * of the array should be not less than the size of the list.
     *
     * @param array Specifies the array to copy the items of this list to.
     *
     * @throws	IllegalArgumentException if the specified <tt>array</tt> length
     *		is less than the size of this list.
     * @throws	NullPointerException if the specified <tt>array</tt> is
     *		<code>null</code>.
     */
    public void copyTo(int[] array)
    {
	if (array == null)
	{
	    throw new NullPointerException("array");
	}
	
	if (array.length < size())
	{
	    throw new IllegalArgumentException(
		    "The specified array length should be not less than the size of this list, but was "
		    + array.length
		    );
	}
	
	IntegerListItem bufferItem = _emptyItem.getNextItem();
	int currentIndex = 0;
	
	while (bufferItem != null)
	{
	    array[currentIndex++] = bufferItem.getValue();
	    bufferItem = bufferItem.getNextItem();
	}
    }
    
    /**
     * Returns the element at the specified <tt>index</tt>.
     *
     * @param index Specifies the index to retrieve an element at.
     * @return	The element at the specified <tt>index</tt>.
     * @throws	ArrayIndexOutOfBoundsException if the specified <tt>index</tt>
     *		is invalid.
     */
    public int elementAt(int index)
    {
	return itemAt(index).getValue();
    }
    
    /**
     * Returns an enumeration of the elements of this list.
     * @return	An enumeration of the elements of this list.
     */
    public IntegerEnumeration elements()
    {
	int[] array = new int[size()];
	copyTo(array);
	return new IntegerEnumeration(array);
    }
    
    /**
     * Tests if this list has no items.
     */
    public boolean isEmpty()
    {
	return size() == 0;
    }
    
    /**
     * Removes all elements from this list and sets its size to zero.
     */
    public void removeAllElements()
    {
	_headItem = _emptyItem;
	
	_headItem.setNextItem(null);
	_headItem.setPreviousItem(null);
	_headItem.setValue(0);
	
	_size = 0;
    }
    
    /**
     * Removes the first occurrence of the argument from this list. If the
     * <tt>element</tt> is found in this list, each item in the list with an
     * index greater or equal to the element's index is shifted downward to have
     * an index one smaller than the value it had previously.
     *
     * @return	<code>true</code> if the specified element was an item in this
     *		list; <code>false</code> otherwise.
     */
    public boolean removeElement(int element)
    {
	IntegerListItem bufferItem = _headItem;
	
	while (bufferItem != _emptyItem)
	{
	    int currentValue = bufferItem.getValue();
	    
	    if (currentValue == element)
	    {
		IntegerListItem previousItem = removeItem(bufferItem);
		
		if (bufferItem == _headItem)
		{
		    _headItem = previousItem;
		}
		
		_size--;
		return true;
	    }
	    
	    bufferItem = bufferItem.getPreviousItem();
	}
	
	return false;
    }
    
    /**
     * Deletes the element at the specified index.
     *
     * @param index Specifies the index of the element to remove.
     * @throws	ArrayIndexOutOfBoundsException if the index was invalid.
     */
    public void removeElementAt(int index)
    {
	if (index < 0 || index >= size())
	{
	    throw new ArrayIndexOutOfBoundsException(
		    "The specified index should be greater or equal to 0 and less than "
		    + size()
		    + ", but was "
		    + index
		    );
	}
	
	IntegerListItem bufferItem = _emptyItem.getNextItem();
	int currentIndex = 0;
	
	while (bufferItem != null)
	{
	    if (currentIndex == index)
	    {
		IntegerListItem previousItem = removeItem(bufferItem);
		
		if (bufferItem == _headItem)
		{
		    _headItem = previousItem;
		}
		
		_size--;
		break;
	    }
	    
	    bufferItem = bufferItem.getNextItem();
	}
    }
    
    /**
     * Sets the item at the specified <tt>index</tt> of this list to be the
     * specified <tt>element</tt>. The previous item at that position is
     * discarded.
     * The <tt>index</tt> must be a value greater than or equal to 0 and less
     * than the current size of the list.
     *
     * @throws	ArrayIndexOutOfBoundsException if the specified <tt>index</tt>
     *		is invalid.
     */
    public void setElementAt(int element, int index)
    {
	itemAt(index).setValue(element);
    }
    
    private int _size;
    
    /**
     * Returns the number of elements in this list.
     * @return	The number of elements in this list.
     */
    public int size()
    {
	return _size;
    }
    
    /* ------ Methods.Private ------ */
    
    /**
     * Returns an IntegerListItem instance at the specified index.
     *
     * @return An IntegerListItem instance at the specified index.
     * @throws	ArrayIndexOutOfBoundsException if the specified <tt>index</tt>
     *		is invalid.
     */
    private IntegerListItem itemAt(int index)
    {
	if (index < 0 || index >= size())
	{
	    throw new ArrayIndexOutOfBoundsException(
		    "The specified index should be greater or equal to 0 and less than the size of this list, but was "
		    + index
		    );
	}
	
	IntegerListItem bufferItem = _emptyItem.getNextItem();
	int currentIndex = 0;
	
	while (bufferItem != null)
	{
	    if (currentIndex == index)
	    {
		return bufferItem;
	    }
	    
	    currentIndex++;
	    bufferItem = bufferItem.getNextItem();
	}
	
	throw new IllegalStateException(
		"An item must be found during traverse operation, but the operation haven't returned any value."
		);
    }
    
    /**
     * Removes the specified <tt>itemToRemove</tt> and returns the previous
     * item for the specified item.
     *
     * @param itemToRemove  Specifies the item to remove.
     * @return	Returns the previous item for the specified
     *		<tt>itemToRemove</tt>.
     */
    private IntegerListItem removeItem(IntegerListItem itemToRemove)
    {
	IntegerListItem nextItem = itemToRemove.getNextItem();
	IntegerListItem previousItem = itemToRemove.getPreviousItem();
	
	previousItem.setNextItem(nextItem);
	
	if (nextItem != null)
	{
	    nextItem.setPreviousItem(previousItem);
	}
	
	return previousItem;
    }
    
    /* ------ Declarations ------ */
    
    private IntegerListItem _emptyItem = IntegerListItem.createEmptyItem();
    private IntegerListItem _headItem = _emptyItem;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of IntegerList.
     */
    public IntegerList()
    {
    }
}
