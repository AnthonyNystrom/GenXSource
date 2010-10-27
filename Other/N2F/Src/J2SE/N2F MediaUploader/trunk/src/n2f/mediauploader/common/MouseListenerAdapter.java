/* ------------------------------------------------
 * MouseListenerAdapter.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

/**
 * Default MouseListener interface implementation. Allows to implement only
 * the methods you need.
 *
 * @author Alex Nesterov
 */
public abstract class MouseListenerAdapter
	implements MouseListener
{
    public void mouseClicked(MouseEvent e)
    {
    }

    public void mousePressed(MouseEvent e)
    {
    }

    public void mouseReleased(MouseEvent e)
    {
    }

    public void mouseEntered(MouseEvent e)
    {
    }

    public void mouseExited(MouseEvent e)
    {
    }
}
