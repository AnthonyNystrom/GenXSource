/* ------------------------------------------------
 * AdvertListView.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import java.awt.BorderLayout;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import javax.swing.DefaultListModel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.ListSelectionModel;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
final class AdvertListView
	extends JPanel
	implements IAdvertListListener
{
    public static final String SELECTED_VALUE_CHANGED =
	    "AdListPane.selectedValueChanged";

    private class AdListSelectionListener
	    implements ListSelectionListener
    {
	public void valueChanged(ListSelectionEvent e)
	{
	    Advert selectedModel =
		    (Advert)_list.getSelectedValue();
	    _changeSupport.firePropertyChange(SELECTED_VALUE_CHANGED,
					      null,
					      selectedModel);
	}

    }

    private class AdvertListener
	    implements PropertyChangeListener
    {
	public void propertyChange(PropertyChangeEvent e)
	{
	    String propertyName = e.getPropertyName();

	    if (Advert.IMAGE_PATH_CHANGED.equals(propertyName))
		repaint();
	    else if (Advert.TEXT_CHANGED.equals(propertyName))
		repaint();
	}

    }

    private JList _list;
    private DefaultListModel _listModel;
    private PropertyChangeListener _advertListener;
    private PropertyChangeSupport _changeSupport;

    /**
     * Creates a new instance of the <tt>AdvertListView</tt> class.
     */
    public AdvertListView()
    {
	super(new BorderLayout());

	_changeSupport = new PropertyChangeSupport(this);
	_advertListener = new AdvertListener();

	_listModel = new DefaultListModel();
	_list = new JList(_listModel);
	_list.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
	_list.addListSelectionListener(new AdListSelectionListener());

	JScrollPane scrollPane = new JScrollPane(_list);
	add(scrollPane, BorderLayout.CENTER);
    }
    
    /**
     * Adds the specified listener to receive property change related events
     * from this component.
     * @param	l
     *		Specifies the listener to add.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    @Override
    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "l"));
	_changeSupport.addPropertyChangeListener(l);
    }
    
    /**
     * Removes the specified listener so that it no longer receives property
     * change related events from this component.
     * @param	l
     *		Specifies the listener to remove.
     */
    @Override
    public void removePropertyChangeListener(PropertyChangeListener l)
    {
	_changeSupport.removePropertyChangeListener(l);
    }

    public void advertListChanged(AdvertListEvent e)
    {
	switch (e.getEventType())
	{
	    case Created:
	    {
		Advert model = e.getAdvert();
		model.addPropertyChangeListener(_advertListener);
		_listModel.addElement(model);
		_list.setSelectedValue(model, true);
		break;
	    }
	    case Deleted:
	    {
		Advert model = e.getAdvert();
		model.removePropertyChangeListener(_advertListener);
		
		int modelIndex = _listModel.indexOf(model);
		_listModel.removeElement(model);
		int listSize = _listModel.size();
		
		if (modelIndex >= listSize)
		    modelIndex = listSize - 1;
		
		if (modelIndex > -1)
		    _list.setSelectedValue(
			    _listModel.getElementAt(modelIndex), true);
		
		break;
	    }
	}
    }

}
