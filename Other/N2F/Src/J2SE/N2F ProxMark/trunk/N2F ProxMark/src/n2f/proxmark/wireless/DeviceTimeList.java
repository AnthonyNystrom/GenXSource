/* ------------------------------------------------
 * DeviceTimeList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.Vector;

/**
 * @author Alex Nesterov
 */
public class DeviceTimeList
{
    /* ------ Methods.Public ------ */
    
    /**
     * Adds the specified <tt>bucket</tt> to the end of this list, increasing
     * its size by one.
     *
     * @param bucket	Specifies the DeviceTimeBucket to add.
     */
    public void addBucket(DeviceTimeBucket bucket)
    {
	_internalList.addElement(bucket);
    }
    
    /**
     * Returns the DeviceTimeBucket at the specified index.
     *
     * @param index Specifies the index in this list to locate the DeviceTimeBucket at.
     * @return	The DeviceTimeBucket at the specified index.
     * @throws	ArrayIndexOutOfBoundsException if the specified <tt>index</tt>
     *		is invalid.
     */
    public DeviceTimeBucket bucketAt(int index)
    {
	Object element = _internalList.elementAt(index);
	
	if (element != null)
	{
	    return (DeviceTimeBucket)element;
	}
	
	return null;
    }
    
    /**
     * Returns an enumeration of buckets in this list.
     * @return	An enumeration of buckets in this list.
     */
    public IDeviceTimeBucketEnumeration buckets()
    {
	return new DeviceTimeBucketEnumeration(_internalList.elements());
    }
    
    /**
     * Tests if the specified <tt>bucket</tt> is contained within this list.
     *
     * @return	<code>true</code> if the specified <tt>bucket</tt> is contained
     *		within this list; otherwise, <code>false</code>.
     */
    public boolean contains(DeviceTimeBucket bucket)
    {
	return _internalList.contains(bucket);
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
    public void copyTo(DeviceTimeBucket[] array)
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
	    array[i] = bucketAt(i);
	}
    }
    
    /**
     * Returns the first bucket in this list.
     *
     * @return	The first bucket in this list.
     * @throws NoSuchElementException if this list cas no buckets.
     */
    public DeviceTimeBucket firstBucket()
    {
	Object element = _internalList.firstElement();
	
	if (element != null)
	{
	    return (DeviceTimeBucket)element;
	}
	
	return null;
    }
    
    /**
     * Searches for the first occurence of the given argument, testing for
     * equality using <tt>equals</tt> method.
     *
     * @param bucket	Specifies the instance to retrieve an index for.
     * @return	The index of the first occurence of the specified
     *		<tt>bucket</tt>, or -1 if the specified
     *		<tt>bucket</tt> is not in the list.
     */
    public int indexOf(DeviceTimeBucket bucket)
    {
	return _internalList.indexOf(bucket);
    }
    
    /**
     * Inserts the specified <tt>bucket</tt> in this list at the specified
     * <tt>index</tt>. Each bucket in this list with an index greater or equal
     * to the specified <tt>index</tt> is shifted upward to have an index one
     * greater than the value it had previously.
     *
     * The <tt>index</tt> must be a value greater than or equal to 0 and less
     * than or equal to the current size of the list.
     *
     * @param bucket	Specifies the bucket to insert.
     * @param index	Specifies the index to insert the <tt>bucket</tt> at.
     *
     * @throws ArrayIndexOutOfBoundsException if the <tt>index</tt> is invalid.
     */
    public void insertBucketAt(DeviceTimeBucket bucket, int index)
    {
	_internalList.insertElementAt(bucket, index);
    }
    
    /**
     * Tests if this list has no buckets.
     *
     * @return	<code>true</code> if this list has no buckets;
     *		<code>false</code> otherwise.
     */
    public boolean isEmpty()
    {
	return _internalList.isEmpty();
    }
    
    /**
     * Returns the last bucket in this list.
     *
     * @return	The last bucket in this list, i.e. the bucket at index
     *		<code>size() - 1</code>.
     *
     * @throws NoSuchElementException if this list is empty.
     */
    public DeviceTimeBucket lastBucket()
    {
	Object element = _internalList.lastElement();
	
	if (element != null)
	{
	    return (DeviceTimeBucket)element;
	}
	
	return null;
    }
    
    /**
     * Removes all the buckets from this list and sets its size to 0.
     */
    public void removeAllBuckets()
    {
	_internalList.removeAllElements();
    }
    
    /**
     * Removes the first occurrence of the specified <tt>bucket</tt> from this
     * list. If the object is found in this list, each entry in the list with an
     * index greater or equal to the object's index is shifted downward to have
     * an index one smaller than the value it had previously.
     *
     * @param bucket	Specifies the bucket to remove.
     * @return	<code>true</code> if the specified <tt>bucket</tt> was an item
     *		in this list; <code>false</code> otherwise.
     */
    public boolean removeBucket(DeviceTimeBucket bucket)
    {
	return _internalList.removeElement(bucket);
    }
    
    /**
     * Deletes the bucket at the specified <tt>index</tt>. Each bucket in this
     * list with an index greater or equal to the specified <tt>index</tt> is
     * shifted downward to have an index one smaller than the value it had
     * previously.
     *
     * The <tt>index</tt> must be a value greater than or equal to 0 and less
     * than the current size of the list.
     *
     * @param index Specifies the index to remove a bucket at.
     * @throws ArrayIndexOutOfBoundsException if the <tt>index</tt> is invalid.
     */
    public void removeBucketAt(int index)
    {
	_internalList.removeElementAt(index);
    }
    
    /**
     * Returns the number of buckets in the list.
     * @return	The number of buckets in the list.
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
     * Creates a new instance of DeviceTimeList.
     */
    public DeviceTimeList()
    {
	_internalList = new Vector();
    }
    
    /**
     * Creates a new instance of DeviceTimeList.
     */
    public DeviceTimeList(int initialCapacity)
    {
	_internalList = new Vector(initialCapacity);
    }
}
