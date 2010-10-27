/* ------------------------------------------------
 * IntegerListItem.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.collections;

/**
 * Represents an item in an IntegerList.
 * 
 * @author Alex Nesterov
 */
final class IntegerListItem
{
    /* ------ Methods.Public ------ */
    
    private IntegerListItem _previousItem;
    
    /**
     * Returns the previous item for this item in IntegerList.
     * @return	The previous item for this item in IntegerList.
     */
    public IntegerListItem getPreviousItem()
    {
	return _previousItem;
    }
    
    /**
     * Sets the previous item for this item in IntegerList.
     * @param previousItem  Specifies the previous item for this item.
     */
    public void setPreviousItem(IntegerListItem previousItem)
    {
	_previousItem = previousItem;
    }
    
    private IntegerListItem _nextItem;
    
    /**
     * Gets the next item for this item in IntegerList.
     * @return	The next item for this item in IntegerList.
     */
    public IntegerListItem getNextItem()
    {
	return _nextItem;
    }
    
    /**
     * Sets the next item for this item in IntegerList.
     * @param nextItem	Specifies the next item for this item.
     */
    public void setNextItem(IntegerListItem nextItem)
    {
	_nextItem = nextItem;
    }
    
    private int _value;
    
    /**
     * Returns the value this item contains.
     * @return	The value this item contains.
     */
    public int getValue()
    {
	return _value;
    }
    
    /**
     * Sets the value for this item.
     * @param value Specifies the value for this item.
     */
    public void setValue(int value)
    {
	_value = value;
    }
    
    /* ------ Methods.Public.Static ------ */
    
    /**
     * Returns a new empty IntegerListItem instance. It is used to determine
     * whether the iterator is at the beginning or at the end of the list, or
     * the list is empty.
     *
     * @return	A new empty IntegerListItem instance.
     */
    public static IntegerListItem createEmptyItem()
    {
	return new IntegerListItem();
    }
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of IntegerListItem.
     */
    private IntegerListItem()
    {
    }
    
    /**
     * Creates a new instance of IntegerListItem.
     *
     * @param previousItem  Specifies the previous item for this item.
     *			    Can be <code>null</code>.
     * @param value	    Specifies the value for this item.
     */
    public IntegerListItem(IntegerListItem previousItem, int value)
    {
	_previousItem = previousItem;
	_value = value;
    }
}
