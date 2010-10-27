/* ------------------------------------------------
 * LineBorderPanel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import java.awt.Color;
import java.awt.Component;
import java.awt.LayoutManager;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import javax.swing.BorderFactory;
import javax.swing.JPanel;

import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Represents a panel with customizable border color in normal, hot, and
 * selected state.
 * 
 * @author Alex Nesterov
 */
public class LineBorderPanel
	extends JPanel
{
    /**
     * Used to track mouse position when it is over internal controls and
     * this panel does not receive notifications.
     */
    private class ChildMouseListener
	    extends MouseListenerAdapter
    {
	@Override
	public void mouseEntered(MouseEvent e)
	{
	    setBorderColor(getHotBorderColor());
	}
	
	@Override
	public void mouseExited(MouseEvent e)
	{
	    if (getMousePosition() == null)
	    {
		if (isSelected())
		    setBorderColor(getSelectedBorderColor());
		else
		    setBorderColor(getNormalBorderColor());
	    }
	}
    }
    
    /**
     * Tracks mouse position to change the state of this panel from normal or
     * selected to hot, or back to normal or selected, depending on the value
     * returned by the <tt>isSelected()</tt> method.
     */
    private class PanelMouseListener
	    extends MouseListenerAdapter
    {
	@Override
	public void mouseEntered(MouseEvent e)
	{
	    setBorderColor(getHotBorderColor());
	}

	@Override
	public void mouseExited(MouseEvent e)
	{
	    if (isSelected())
		setBorderColor(getSelectedBorderColor());
	    else
		setBorderColor(getNormalBorderColor());
	}

    }
    
    private MouseListener _childMouseListener;

    /**
     * Creates a new instance of the <tt>LineBorderPanel</tt> class.
     */
    public LineBorderPanel()
    {
	this(null);
    }

    /**
     * Creates a new instance of the <tt>LineBorderPanel</tt> class.
     * @param layoutManager
     */
    public LineBorderPanel(LayoutManager layoutManager)
    {
	super(layoutManager);

	_childMouseListener = new ChildMouseListener();
	addMouseListener(new PanelMouseListener());
	setBorderColor(getNormalBorderColor());
    }
    
    /**
     * Considers the specified <tt>component</tt> as transparent for mouse
     * events. Internal components may prevent this panel from defining proper state.
     * Inappropriate border color will be set as a result.
     * @param	component
     *		Specifies one of the internal components for this panel.
     * @throws	IllegalArgumentException
     *		If the specified <tt>component</tt> is <code>null</code>.
     */
    public void considerTransparent(Component component)
    {
	if (component == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "component"));
	
	component.addMouseListener(_childMouseListener);
    }

    private static final Color _defaultHotBorderColor = Color.RED;
    private Color _hotBorderColor;

    /**
     * Returns the border color that is used in hot state.
     * @return	The border color that is used in hot state.
     */
    public Color getHotBorderColor()
    {
	if (_hotBorderColor == null)
	    return _defaultHotBorderColor;
	return _hotBorderColor;
    }

    /**
     * Sets the border color to use in hot state.
     * @param	value
     *		Specifies the border color to use in hot state. If the <tt>value</tt>
     *		is <code>null</code> default border color will be used.
     */
    public void setHotBorderColor(Color value)
    {
	if (_hotBorderColor != value)
	{
	    _hotBorderColor = value;
	    setBorderColor(getHotBorderColor());
	}
    }

    private static final Color _defaultNormalBorderColor = Color.GRAY;
    private Color _normalBorderColor;

    /**
     * Returns the border color that is used in normal state.
     * @return	The border color that is used in normal state.
     */
    public Color getNormalBorderColor()
    {
	if (_normalBorderColor == null)
	    return _defaultNormalBorderColor;
	return _normalBorderColor;
    }

    /**
     * Sets the border color to use in normal state.
     * @param	value
     *		Specifies the border color to use in normal state. If the <tt>value</tt>
     *		is <code>null</code> default border color will be used.
     */
    public void setNormalBorderColor(Color value)
    {
	if (_normalBorderColor != value)
	{
	    _normalBorderColor = value;
	    setBorderColor(getNormalBorderColor());
	}
    }

    private static final Color _defaultSelectedBorderColor = Color.BLUE;
    private Color _selectedBorderColor;

    /**
     * Returns the border color that is used in selected state.
     * @return	The border color that is used in selected state.
     */
    public Color getSelectedBorderColor()
    {
	if (_selectedBorderColor == null)
	    return _defaultSelectedBorderColor;
	return _selectedBorderColor;
    }

    /**
     * Sets the border color to use in selected state.
     * @param	value
     *		Specifies the border color to use in selected state. If the <tt>value</tt>
     *		is <code>null</code> default border color will be used.
     */
    public void setSelectedBorderColor(Color value)
    {
	if (_selectedBorderColor != value)
	{
	    _selectedBorderColor = value;

	    if (isSelected())
		setBorderColor(getSelectedBorderColor());
	}
    }

    private boolean _isSelected;

    /**
     * Returns the value indicating whether this panel is selected.
     * @return	<code>true</code> if this panel is selected; <code>false</code> otherwise.
     */
    public boolean isSelected()
    {
	return _isSelected;
    }

    /**
     * Sets the value indicating whether this panel is in the selected state.
     * @param	value
     *		Specifies the value indicating whether this panel is in the
     *		selected state. 
     */
    public void setSelected(boolean value)
    {
	if (_isSelected != value)
	{
	    _isSelected = value;

	    if (getMousePosition() == null)
	    {
		if (_isSelected)
		    setBorderColor(getSelectedBorderColor());
		else
		    setBorderColor(getNormalBorderColor());
	    }
	}
    }

    private void setBorderColor(Color borderColor)
    {
	setBorder(BorderFactory.createLineBorder(borderColor));
    }

}
