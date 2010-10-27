/* ------------------------------------------------
 * DefaultRotateTypeProcessor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import n2f.mediauploader.drawing.RotateType;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Default <tt>IRotateTypeProcessor</tt> implementation.
 * @author Alex Nesterov
 */
final class DefaultRotateTypeProcessor
	implements IRotateTypeProcessor
{
    /**
     * Combines the specified arguments and returns resulting RotateType.
     * 
     * @param	x
     *		The first argument to combine.
     * @param	y
     *		The second argument to combine.
     * @return	Resulting RotateType.
     * @throws	IllegalArgumentException
     *		If the specified <tt>x</tt> is <code>null</code>, or 
     *		if the specified <tt>y</tt> is <code>null</code>.
     */
    public RotateType combineRotateTypes(RotateType x, RotateType y)
    {
	if (x == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "x"));
	if (y == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "y"));
	
	return getRotateTypeFromAngle(getAngleFromRotateType(x) + getAngleFromRotateType(y));
    }

    private static int getAngleFromRotateType(RotateType rotateType)
    {
	int angle = 0;

	switch (rotateType)
	{
	    case Rotate90:
	    {
		angle = 90;
		break;
	    }
	    case Rotate180:
	    {
		angle = 180;
		break;
	    }
	    case Rotate270:
	    {
		angle = 270;
		break;
	    }
	}

	return angle;
    }
    
    private static RotateType getRotateTypeFromAngle(int angle)
    {
	int qualifiedAngle = getQualifiedAngle(angle);
	
	if (qualifiedAngle == 90)
	{
	    return RotateType.Rotate90;
	}
	
	if (qualifiedAngle == 180)
	{
	    return RotateType.Rotate180;
	}
	
	if (qualifiedAngle == 270)
	{
	    return RotateType.Rotate270;
	}
	
	return RotateType.RotateNone;
    }

    private static int getQualifiedAngle(int angle)
    {
	return angle - 360 * (angle / 360);
    }

}
