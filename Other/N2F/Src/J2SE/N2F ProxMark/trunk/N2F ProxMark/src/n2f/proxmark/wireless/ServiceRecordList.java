/* ------------------------------------------------
 * ServiceRecordList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.Vector;
import javax.bluetooth.ServiceRecord;

/**
 * @author Alex Nesterov
 */
public class ServiceRecordList
{
    /* ------ Methods.Public ------ */
    
    /**
     * Adds the specified <tt>record</tt> to the end of this list, increasing
     * its size by one.
     *
     * @param record	Specifies the ServiceRecord to add.
     */
    public void addRecord(ServiceRecord record)
    {
	_internalList.addElement(record);
    }
    
    /**
     * Tests if the specified <tt>record</tt> is contained within this list.
     *
     * @return	<code>true</code> if the specified <tt>record</tt> is contained
     *		within this list; otherwise, <code>false</code>.
     */
    public boolean contains(ServiceRecord record)
    {
	return _internalList.contains(record);
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
    public void copyTo(ServiceRecord[] array)
    {
	if (array == null)
	{
	    throw new NullPointerException("array");
	}
	
	int size = size();
	
	if (array.length < size)
	{
	    throw new IllegalArgumentException(
		    "The specified array length should be not less than the size of this list, but was "
		    + array.length
		    );
	}
	
	for (int i = 0; i < size; i++)
	{
	    array[i] = recordAt(i);
	}
    }
    
    /**
     * Returns the first record in this list.
     *
     * @return	The first record in this list.
     * @throws NoSuchElementException if this list cas no records.
     */
    public ServiceRecord firstRecord()
    {
	Object element = _internalList.firstElement();
	
	if (element != null)
	{
	    return (ServiceRecord)element;
	}
	
	return null;
    }
    
    /**
     * Searches for the first occurence of the given argument, testing for
     * equality using <tt>equals</tt> method.
     *
     * @param record	Specifies the instance to retrieve an index for.
     * @return	The index of the first occurence of the specified
     *		<tt>record</tt>, or -1 if the specified
     *		<tt>record</tt> is not in the list.
     */
    public int indexOf(ServiceRecord record)
    {
	return _internalList.indexOf(record);
    }
    
    /**
     * Inserts the specified <tt>record</tt> in this list at the specified
     * <tt>index</tt>. Each record in this list with an index greater or equal
     * to the specified <tt>index</tt> is shifted upward to have an index one
     * greater than the value it had previously.
     *
     * The <tt>index</tt> must be a value greater than or equal to 0 and less
     * than or equal to the current size of the list.
     *
     * @param record	Specifies the record to insert.
     * @param index Specifies the index to insert the <tt>record</tt> at.
     *
     * @throws ArrayIndexOutOfBoundsException if the <tt>index</tt> is invalid.
     */
    public void insertRecordAt(ServiceRecord record, int index)
    {
	_internalList.insertElementAt(record, index);
    }
    
    /**
     * Tests if this list has no records.
     *
     * @return	<code>true</code> if this list has no records;
     *		<code>false</code> otherwise.
     */
    public boolean isEmpty()
    {
	return _internalList.isEmpty();
    }
    
    /**
     * Returns the last record in this list.
     *
     * @return	The last record in this list, i.e. the record at index
     *		<code>size() - 1</code>.
     *
     * @throws NoSuchElementException if this list is empty.
     */
    public ServiceRecord lastRecord()
    {
	Object element = _internalList.lastElement();
	
	if (element != null)
	{
	    return (ServiceRecord)element;
	}
	
	return null;
    }
    
    /**
     * Returns the ServiceRecord at the specified index.
     *
     * @param index Specifies the index in this list to locate the ServiceRecord at.
     * @return	The ServiceRecord at the specified index.
     * @throws	ArrayIndexOutOfBoundsException if the specified <tt>index</tt>
     *		is invalid.
     */
    public ServiceRecord recordAt(int index)
    {
	Object element = _internalList.elementAt(index);
	
	if (element != null)
	{
	    return (ServiceRecord)element;
	}
	
	return null;
    }
    
    /**
     * Returns an enumeration of records in this list.
     * @return	An enumeration of records in this list.
     */
    public IServiceRecordEnumeration records()
    {
	return new ServiceRecordEnumeration(_internalList.elements());
    }
    
    /**
     * Removes all the records from this list and sets its size to 0.
     */
    public void removeAllRecords()
    {
	_internalList.removeAllElements();
    }
    
    /**
     * Removes the first occurrence of the specified <tt>record</tt> from this
     * list. If the object is found in this list, each entry in the list with an
     * index greater or equal to the object's index is shifted downward to have
     * an index one smaller than the value it had previously.
     *
     * @param record	Specifies the record to remove.
     * @return	<code>true</code> if the specified <tt>record</tt> was an item
     *		in this list; <code>false</code> otherwise.
     */
    public boolean removeRecord(ServiceRecord record)
    {
	return _internalList.removeElement(record);
    }
    
    /**
     * Deletes the record at the specified <tt>index</tt>. Each record in this
     * list with an index greater or equal to the specified <tt>index</tt> is
     * shifted downward to have an index one smaller than the value it had
     * previously.
     *
     * The <tt>index</tt> must be a value greater than or equal to 0 and less
     * than the current size of the list.
     *
     * @param index Specifies the index to remove a record at.
     * @throws ArrayIndexOutOfBoundsException if the <tt>index</tt> is invalid.
     */
    public void removeRecordAt(int index)
    {
	_internalList.removeElementAt(index);
    }
    
    /**
     * Returns the number of records in the list.
     * @return	The number of records in the list.
     */
    public int size()
    {
	return _internalList.size();
    }
    
    /**
     * Returns a string representation of this list.
     * @return A string representation of this list.
     */
    @Override
    public String toString()
    {
	return _internalList.toString();
    }
    
    /* ------ Declarations ------ */
    
    private Vector _internalList;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of ServiceRecordList.
     */
    public ServiceRecordList()
    {
	_internalList = new Vector();
    }
    
    /**
     * Creates a new instance of ServiceRecordList.
     *
     * @param initialCapacity	Specifies the initial capacity for this list.
     * @throws	IllegalArgumentException if the specified <tt>capacity</tt>
     *		is negative.
     */
    public ServiceRecordList(int initialCapacity)
    {
	_internalList = new Vector(initialCapacity);
    }
}
