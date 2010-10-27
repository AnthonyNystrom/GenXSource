/* ------------------------------------------------
 * Resources.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.microedition.componentmodel;

import java.io.*;
import genetibase.util.Argument;
import genetibase.collections.*;

/**
 * Provides functionality to load resources corresponding to the current locale.
 *
 * @author Alex Nesterov
 */
public class Resources
{
    /* ------ Methods.Public ------ */
    
    /** Get the value for the specified resource. */
    public String get(String resourceName)
    {
	if (!_load)
	{
	    /* If the resource file haven't been loaded yet, then load it. */
	    if (!load())
	    {
		/* 
		 * If the attempt to load the resource file failed, return an
		 * empty string.
		 */
		
		return "";
	    }
	}
	
	String value = (String)_resContents.get(resourceName);
	
	if (value == null)
	{
	    /* If the specified resource was not found... */
	    return "";
	}
	else
	{
	    return value;
	}
    }
    
    /**
     * Tries to load the resource file specified in the constructor.
     *
     * @return	<code>true</code> if the resource file was loaded successfully;
     *		otherwise, <code>false</code>.
     */
    public boolean load()
    {
	/*
	 * If the previous attempt to load the resource file ended up with
	 * an error, return false.
	 */
	if (_error)
	{
	    return false;
	}
	
	DataInputStream in = null;
	
	try
	{
	    /* Retrieve resource file stream. */
	    InputStream inputStream = getInputStream();
	    
	    if (inputStream == null)
	    {
		return false;
	    }
	    
	    in = new DataInputStream(inputStream);
	    _resContents = new StringHashtable();
	    _endOfFile = false;
	    _tempBuffer = new StringBuffer();
	    
	    /* Read the file string by string and populate the hashtable. */
	    
	    String key;
	    
	    while ((key = readKey(in)) != null)
	    {
		if (!Argument.isEmpty(key))
		{
		    String value = readValue(in);
		    
		    if (value != null)
		    {
			_resContents.put(key, value.trim());
		    }
		}
	    }
	    
	    in.close();
	    _load = true; /* The resource file was loaded successfully. */
	    return true;
	}
	catch (Exception io)
	{
	    _error = true; /* There was an error while loading the resource file. */
	    unload();
	    return false;
	}
	finally
	{
	    if (in != null)
	    {
		try
		{
		    in.close();
		}
		catch (IOException ioException)
		{
		    /* Do nothing. */
		}
	    }
	}
    }

    /**
     * Frees all associated resources.
     */
    public void unload()
    {
	_resContents = null;
	
	if (_fileName != null)
	{
	    _name = null;
	    _extension = null;
	    _directory = null;
	}
	
	_load = false;
	_endOfFile = true;
	_tempBuffer = null;
    }
    
    /* ------ Methods.Private ------ */
    
    /** 
     * Reads a single byte from the specified stream.
     *
     * @throws	UTFDataFormatException if the end of file or carriage return
     *		will be met.
     */
    private int getByte(InputStream in) throws IOException, UTFDataFormatException
    {
	int r = in.read();
	
	if (r < 0 || r == 0x0A)
	{
	    _endOfFile = true;
	    throw new UTFDataFormatException();
	}
	
	return r;
    }
    
     private InputStream getInputStream()
    {
	/*
	 * If the previous attempt to load the file ended up with an error,
	 * null is returned.
	 */
	
	if (_error)
	{
	    return null;
	}
	
	if (_fileName == null)
	{
	    /*
	     * Opening the resource file which corresponds to the current
	     * locale. If the current language for the phone is American
	     * English then locale will contain "en-US", if Russian, then
	     * "ru-RU".
	     */
	    InputStream in = null;
	    String locale = System.getProperty("microedition.locale");
	    
	    if (locale != null)
	    {
		locale = locale.replace('-', '_');
		
		/* The possible output will be "fileName_en_US". */
		_fileName = _directory + _name + "_" + locale + "." + _extension;
		in = getClass().getResourceAsStream(_fileName);
		
		if (in != null)
		{
		    return in;
		}
		else
		{
		    /* The possible output will be "fileName_en". */
		    int i = locale.indexOf('_');
		    
		    if (i != -1)
		    {
			locale = locale.substring(0, i);
			_fileName = _directory + _name + "_" + locale + "." + _extension;
			in = getClass().getResourceAsStream(_fileName);
			
			if (in != null)
			{
			    return in;
			}
		    }
		}
	    }
	    
	    /*
	     * If a resource file was not found, trying to locate the default
	     * resource file.
	     */
	    
	    _fileName = _directory + _name + "." + _extension;
	    in = getClass().getResourceAsStream(_fileName);
	    
	    if (in == null)
	    {
		/* Resource file was not found. */
		_error = true;
		unload();
		return null;
	    }
	    else
	    {
		/* Resource file found. */
		return in;
	    }
	}
	else
	{
	    /*
	     * The resource file was loaded previously. So just return
	     * the stream.
	     */
	    
	    return getClass().getResourceAsStream(_fileName);
	}
    }
    
    /** Extracts a key from the specified stream. */
    private String readKey(DataInputStream in) throws IOException
    {
	if (_endOfFile)
	{
	    return null;
	}
	
	_tempBuffer.setLength(0); /* Empty the buffer. */
	
	int r = -1;
	
	/*
	 * Read until we don't meet the delimiter character, end of file,
	 * or a new line character.
	 *
	 * Carriage return:
	 *
	 * For Unix	- 0x0A
	 * For Windows	- 0x0D 0x0A
	 * For Mac	- 0x0D (though we do not intend to use this class under Mac).
	 */
	while ((r = in.read()) > 0 && r != 0x0A && r != DELIMITER)
	{
	    char c = (char)r;
	    _tempBuffer.append(c);
	}
	
	/* If we are at the end of the file set _endOfFile to true. */
	if (r == -1)
	{
	    _endOfFile = true;
	    return null;
	}
	
	return _tempBuffer.toString();
    }
    
    /** 
     * Extracts a value from the specified stream.
     *
     * @throws UTFDataFormatException if the resource file format is incompatible.
     */
    private String readValue(InputStream in) throws IOException, UTFDataFormatException
    {
	if (_endOfFile)
	{
	    return null;
	}

	_tempBuffer.setLength(0);
	int c, char2, char3;
	int r = 0;

	/* Read the stream... */
	while ((r = in.read()) > 0 && r != 0x0A)
	{
	    c =  r & 0xff;
	    
	    switch (c >> 4)
	    {
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		{
		    _tempBuffer.append((char)c);
		    break;
		}
		case 12:
		case 13:
		{
		    char2 = getByte(in);
		    
		    if ((char2 & 0xC0) != 0x80)
		    {
			throw new UTFDataFormatException();
		    }
		    
		    _tempBuffer.append((char)(((c & 0x1F) << 6) | (char2 & 0x3F)));
		    break;
		}
		case 14:
		{
		    char2 = getByte(in);
		    char3 = getByte(in);
		    
		    if (((char2 & 0xC0) != 0x80) || ((char3 & 0xC0) != 0x80))
		    {
			throw new UTFDataFormatException();
		    }

		    _tempBuffer.append((char)(((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0)));
		    break;
		}
		default:
		{
		    throw new UTFDataFormatException();
		}
	    }
	}
	
	return _tempBuffer.toString();
    }
    
    /* ------ Declarations ------ */
    
    /** Character that is used to separate resource key from its value. */
    public static final char DELIMITER = '=';
    
    /* Contains the contents of the associated resource file. */
    private StringHashtable _resContents;
    private StringBuffer _tempBuffer;
    
    private String _fileName;
    private String _directory;
    private String _extension;
    private String _name;
    
    private boolean _endOfFile;
    private boolean _load;
    private boolean _error;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of Resources class.
     *
     * @param directory	Specifies the directory a resource file resides in.
     *			Can be <code>null</code>.
     * @param name	Specifies the name of the file that contains resources.
     *			Cannot be <code>null</code>.
     * @param extension	Specifies the extension of the file that contains
     *			resources. Can be <code>null</code>.
     *
     * @throws NullPointerException if <tt>name</tt> is <code>null</code>.
     * @throws IllegalArgumentException if <tt>name</tt> is an empty string.
     */
    public Resources(String directory, String name, String extension)
    {
	if (name == null)
	{
	    throw new NullPointerException("name");
	}
	
	if (Argument.isEmpty(name))
	{
	    throw new IllegalArgumentException("name");
	}
	
	_tempBuffer = new StringBuffer();
	_directory = "/" + (Argument.isNullOrEmpty(directory) ? "" : (directory + "/"));
	_name = name;
	_extension = extension;
    }

	public void free() {
		unload();
		
	}
}
