/* ------------------------------------------------
 * ListEx.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.microedition.ui;

import javax.microedition.lcdui.*;

/**
 * List with enhanced functionality.
 *
 * @author Alex Nesterov
 */
public class ListEx
	extends List
{
    /**
     * Appends an element to the ListEx. <tt>imagePart</tt> is set to
     * <code>null</code>.
     *
     * @param stringPart    Specifies the string part of the element to be
     *			    added.
     * @throws NullPointerException if <tt>stringPart</tt> is <code>null</code>.
     */
    public void append(String stringPart)
    {
	append(stringPart, null);
    }
    
    /** 
     * Creates a new instance of ListEx.
     *
     * @param title	Specifies the title for the list.
     * @param listType	One of <tt>IMPLICIT</tt>, <tt>EXCLUSIVE</tt>, or
     *			<tt>MULTIPLE</tt>.
     */
    public ListEx(String title, int listType)
    {
	super(title, listType);
    }
    
    /**
     * Creates a new instance of ListEx.
     *
     * @param title		Specifies the title for the list.
     * @param listType		One of <tt>IMPLICIT</tt>, <tt>EXCLUSIVE</tt>, or
     *				<tt>MULTIPLE</tt>.
     * @param stringElements	Set of strings specifying the string parts of
     *				the List elements.
     * @param imageElements	Set of images specifying the image parts of the
     *				List elements.
     */
    public ListEx(
	    String title
	    , int listType
	    , String[] stringElements
	    , Image[] imageElements
	    )
    {
	super(title, listType, stringElements, imageElements);
    }
}
