/* ------------------------------------------------
 * IRotateTypeProcessor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import n2f.mediauploader.drawing.RotateType;

/**
 * Indicates that the implementor can process {@link RotateType} operands.
 * @author Alex Nesterov
 */
public interface IRotateTypeProcessor
{
    /**
     * Combines the specified arguments and returns resulting RotateType.
     * 
     * @param	x
     *		The first argument to combine.
     * @param	y
     *		The second argument to combine.
     * @return	Resulting RotateType.
     */
    RotateType combineRotateTypes(RotateType x, RotateType y);
}
