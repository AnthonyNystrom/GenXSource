/* ------------------------------------------------
 * AdvertPublishModelTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import n2f.proxmark.wireless.DefaultDeviceDiscovererListener;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public final class AdvertPublishModelTest
{
    private AdvertPublishModel _model;
    
    @Before
    public void setUp()
    {
	_model = new AdvertPublishModel(new DefaultDeviceDiscovererListener());
    }
}
