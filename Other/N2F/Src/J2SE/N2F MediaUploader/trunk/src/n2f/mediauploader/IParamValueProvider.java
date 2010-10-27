/* ------------------------------------------------
 * IParamValueProvider.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

/**
 * Indicates that the implementor can return values for the specified parameters.
 * Most commonly you will need to implement this interface in an applet to work
 * with {@link ParamValueParser}.
 * @author Alex Nesterov
 */
interface IParamValueProvider
{
    /** 
     * Returns a value for the specified parameter.
     * @param	param
     *		Specifies the parameter name to return value for.
     * @return	Value for the specified parameter.
     */
    String getValueFor(String param);
}
