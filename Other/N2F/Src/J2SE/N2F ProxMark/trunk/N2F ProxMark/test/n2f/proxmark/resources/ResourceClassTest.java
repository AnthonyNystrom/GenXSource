/* ------------------------------------------------
 * ResourceClassTester.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.resources;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class ResourceClassTest
{
    private static final Logger _logger = Logger.getLogger(ResourceClassTest.class.getName());
    
    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }
    
    @Test
    public void resourceStrings() throws IllegalArgumentException, IllegalAccessException
    {
	testResourceClass(ApplicationResources.class);
    }
    
    private static void testResourceClass(Class<?> resourceClass) throws IllegalArgumentException, IllegalAccessException
    {
	_logger.log(Level.INFO, String.format("------ %s ------", resourceClass.getName()));
	
	for (final Field field : resourceClass.getFields())
	{
	    int fieldModifiers = field.getModifiers();
	    
	    if (Modifier.isPublic(fieldModifiers) && Modifier.isStatic(fieldModifiers))
	    {
		_logger.log(Level.INFO, field.getName());
		assertNotNull(field.get(null));
	    }
	}
    }
}
