/* ------------------------------------------------
 * ThumbnailFlowLayout.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import java.awt.Component;
import java.awt.Container;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Insets;

/**
 * Enhanced <tt>FlowLayout</tt> with vertical scrolling support.
 * @author Alex Nesterov
 */
public class ThumbnailFlowLayout
	extends FlowLayout
{
    /**
     * Creates a new instance of the <tt>ThumbnailFlowLayout</tt> class.
     */
    public ThumbnailFlowLayout()
    {
	super();
    }

    /**
     * Creates a new instance of the <tt>ThumbnailFlowLayout</tt> class.
     * @param align
     */
    public ThumbnailFlowLayout(int align)
    {
	super(align);
    }

    /**
     * Creates a new instance of the <tt>ThumbnailFlowLayout</tt> class.
     * @param align
     * @param hgap
     * @param vgap
     */
    public ThumbnailFlowLayout(int align, int hgap, int vgap)
    {
	super(align, hgap, vgap);
    }

    @Override
    public Dimension minimumLayoutSize(Container target)
    {
	return calculateDimension(target, false);
    }

    @Override
    public Dimension preferredLayoutSize(Container target)
    {
	return calculateDimension(target, true);
    }

    private Dimension calculateDimension(Container target, boolean minimum)
    {
	synchronized (target.getTreeLock())
	{
	    int hGap = getHgap();
	    int vGap = getVgap();
	    int targetWidth = target.getWidth();

	    if (targetWidth == 0)
		targetWidth = Integer.MAX_VALUE;

	    Insets targetInsets = target.getInsets();

	    if (targetInsets == null)
		targetInsets = new Insets(0, 0, 0, 0);

	    int requiredWidth = 0;

	    int maxWidth =
		    targetWidth - (targetInsets.left + targetInsets.right + hGap * 2);
	    int componentCount = target.getComponentCount();
	    int x = 0;
	    int y = targetInsets.top;
	    int rowHeight = 0;

	    for (int i = 0; i < componentCount; i++)
	    {
		Component component = target.getComponent(i);

		if (component.isVisible())
		{
		    Dimension dimension = minimum
			    ? component.getMinimumSize()
			    : component.getPreferredSize();

		    if ((x == 0) || ((x + dimension.width) <= maxWidth))
		    {
			if (x > 0)
			{
			    x += hGap;
			}

			x += dimension.width;
			rowHeight = Math.max(rowHeight, dimension.height);
		    }
		    else
		    {
			x = dimension.width;
			y += vGap + rowHeight;
			rowHeight = dimension.height;
		    }

		    requiredWidth = Math.max(requiredWidth, x);
		}
	    }

	    y += rowHeight;
	    return new Dimension(requiredWidth + targetInsets.left + targetInsets.right,
				 y);
	}
    }

}
