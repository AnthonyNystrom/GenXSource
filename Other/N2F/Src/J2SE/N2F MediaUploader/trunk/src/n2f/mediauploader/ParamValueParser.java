/* ------------------------------------------------
 * ParamValueParser.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.util.LinkedList;
import java.util.List;

import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Provides a convenient way to retrieve from HTML markup the params that represent
 * array items.
 * 
 * @author Alex Nesterov
 */
final class ParamValueParser
{
    /**
     * This class is immutable.
     */
    private ParamValueParser()
    {
    }

    /**
     * Suppose you need to pass a collection of values to an applet. Each item
     * is passed via a <tt>&lt;param&gt;</tt> tag with the <tt>name</tt> attribute
     * set to <tt>paramNameBase</tt> followed by <tt>paramDelimiter</tt> and
     * a number indicating the index of the element in the array. Note that
     * your applet should implement the {@link IParamValueProvider} interface
     * so the <tt>ParamValueParser</tt> could retrieve the parameters. The
     * basic implementation is rather simple:
     * <br />
     * <code><pre>
     * ...
     * public String getValueFor(String param)
     * {
     *	    return getParameter(param);
     * }
     * ...
     * </pre></code>
     * <br />
     * Consider the following example:
     * <br />
     * You need to pass to an applet a collection of image galleries available.
     * Then the markup will look like this:
     * <br />
     * <code><pre>
     * ...
     * &lt;param name="galleryName::0" value="Default Gallery" /&gt;
     * &lt;param name="galleryName::1" value="Custom Gallery" /&gt;
     * ...
     * </pre></code>
     * <br />
     * And the code to retrieve a collection of image galleries is shown below:
     * <br/>
     * <code>
     * String[] imageGalleries = ParamValueParser.getGenericParamValues(this, "galleryName", "::");
     * </code>
     * If no strings were found an empty array is returned. Not <code>null</code>.
     * @param	paramValueProvider
     *		Most commonly it is an applet that implements the <tt>IParamValueProvider</tt>
     *		interface and invokes its <tt>getParameter</tt> method.
     * @param	paramNameBase
     *		Specifies the common part for all the parameters from an array.
     * @param	paramDelimiter
     *		Specifies the delimiter between parameter base name and the index.
     * @return	An array of values for a set of parameters with the specified
     *		<tt>paramNameBase</tt>. If no values were found an empty array
     *		is returned. Not <code>null</code>.
     * @throws	IllegalArgumentException
     *		If the specified <tt>paramValueProvider</tt> is <code>null</code>, or
     *		if the specified <tt>paramNameBase</tt> is <code>null</code>, or
     *		if the specified <tt>paramDelimiter</tt> is <code>null</code>.
     */
    public static String[] getGenericParamValues(IParamValueProvider paramValueProvider,
						   String paramNameBase,
						   String paramDelimiter)
    {
	if (paramValueProvider == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "paramValueProvider"));
	if (paramNameBase == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "paramNameBase"));
	if (paramDelimiter == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "paramDelimiter"));

	List<String> galleryNames = new LinkedList<String>();
	int counter = 0;
	boolean shouldContinue = true;

	while (shouldContinue)
	{
	    String galleryNameParam = String.format(
		    "%s%s%s",
		    paramNameBase,
		    paramDelimiter,
		    String.valueOf(counter));

	    String value = paramValueProvider.getValueFor(galleryNameParam);

	    if (value == null)
		shouldContinue = false;
	    else
	    {
		galleryNames.add(value);
		counter++;
	    }
	}

	int galleryNameCount = galleryNames.size();
	String[] result = new String[galleryNameCount];

	for (int i = 0; i < galleryNameCount; i++)
	    result[i] = galleryNames.get(i);

	return result;
    }

}
