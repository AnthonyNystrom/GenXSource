/* ------------------------------------------------
 * AdvertManagerTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import java.awt.Dimension;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import n2f.proxmark.AdvertListEvent.EventType;
import org.jmock.Expectations;
import org.jmock.Mockery;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public final class AdvertManagerTest
{
    private Mockery _context;
    private AdvertManager _manager;
    private IAdvertListListener _adListListener;
    private PropertyChangeListener _propertyChangeListener;

    @Before
    public void setUp()
    {
	_context = new Mockery();
	_adListListener = _context.mock(IAdvertListListener.class);
	_propertyChangeListener = _context.mock(PropertyChangeListener.class);
	_manager = new AdvertManager(new Dimension(320, 240));
    }

    @Test
    public void canCreateAdvert_DefaultState()
    {
	assertTrue(_manager.canCreateAdvert());
    }

    @Test
    public void canDeleteAdvert_DefaultState()
    {
	assertFalse(_manager.canDeleteAdvert());
    }

    @Test
    public void createAdvert_CheckInvokationCount()
    {
	_context.checking(
		new Expectations()
		{
		    {
			exactly(1).of(_adListListener).advertListChanged(with(any(AdvertListEvent.class)));
			exactly(2).of(_propertyChangeListener).propertyChange(with(any(PropertyChangeEvent.class)));
		    }

		});

	_manager.addPropertyChangeListener(_propertyChangeListener);
	_manager.addAdvertListListener(_adListListener);

	_manager.createAdvert();
	_context.assertIsSatisfied();
    }

    @Test
    public void createAdvert_CheckParameters()
    {
	_manager.addAdvertListListener(
		new IAdvertListListener()
		{
		    public void advertListChanged(AdvertListEvent e)
		    {
			e.getEventType().equals(EventType.Created);
		    }

		});
	_manager.createAdvert();
    }

    @Test
    public void setCurrent_CheckInvokationCount()
    {
	_context.checking(new Expectations()
		  {
		      {
			  exactly(2).of(_propertyChangeListener).propertyChange(with(any(PropertyChangeEvent.class)));
		      }
		  });
	_manager.addPropertyChangeListener(_propertyChangeListener);
	_manager.createAdvert();
	_context.assertIsSatisfied();
    }

    @Test
    public void setCurrent_CheckParameters()
    {
	final String[] parameters =
		new String[] { AdvertManager.CURRENT_ADVERT_CHANGED,
			       AdvertManager.CAN_DELETE_ADVERT_CHANGED
	
	
	
	};
	_manager.addPropertyChangeListener(
		new PropertyChangeListener()
		{
		    private int _invokeCount;

		    public void propertyChange(PropertyChangeEvent e)
		    {
			assertEquals(parameters[_invokeCount++],
				     e.getPropertyName());
		    }

		});
	_manager.createAdvert();
    }

}
