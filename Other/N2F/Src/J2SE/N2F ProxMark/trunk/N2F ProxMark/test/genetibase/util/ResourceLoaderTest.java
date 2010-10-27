/* ------------------------------------------------
 * ResourceLoaderTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util;

import java.util.MissingResourceException;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public final class ResourceLoaderTest
{
    @Test(expected = IllegalArgumentException.class)
    public void loadImage_NullArg0()
    {
	ResourceLoader.loadImage(null, "key");
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void loadImage_NullArg1()
    {
	ResourceLoader.loadImage(ResourceLoaderTest.class, null);
    }
    
    @Test(expected = MissingResourceException.class)
    public void loadImage_NoResource()
    {
	ResourceLoader.loadImage(ResourceLoader.class, "resource");
    }
}
