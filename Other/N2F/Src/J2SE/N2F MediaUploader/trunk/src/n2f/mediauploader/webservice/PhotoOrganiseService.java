/* ------------------------------------------------
 * PhotoOrganiseService.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.webservice;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import n2f.mediauploader.util.Environment;
import static n2f.mediauploader.webservice.resources.PhotoOrganiseServiceResources.*;

/**
 * Represents Next2Friends PhotoOrganise Web-Service wrapper.
 * 
 * @author Alex Nesterov
 */
public class PhotoOrganiseService
{
    /**
     * Uploads the specified photo to Next2Friends PhotoOrganise Web-Service.
     * 
     * @param	encryptedWebMemberID
     * @param	galleryID
     *		Specifies the gallery to upload the photo to.
     * @param	base64PhotoString
     *		Image in Base64 representation.
     * @param	dateTime
     *		Date when the photo was last modified.
     * @throws	WebServiceException
     *		If error occures during photo upload.
     */
    public void uploadPhoto(String encryptedWebMemberID,
			     String galleryID,
			     String base64PhotoString,
			     String dateTime)
	    throws WebServiceException
    {
	HttpRequestBuilder requestBuilder;

	try
	{
	    requestBuilder = buildRequest(encryptedWebMemberID,
					  galleryID,
					  base64PhotoString,
					  dateTime);
	}
	catch (UnsupportedEncodingException e)
	{
	    throw new WebServiceException(e.getMessage(), e);
	}

	URL url;

	try
	{
	    url = new URL(PhotoOrganiseURL);
	}
	catch (MalformedURLException e)
	{
	    throw new WebServiceException(e.getMessage(), e);
	}

	URLConnection connection;

	try
	{
	    connection = url.openConnection();
	}
	catch (IOException e)
	{
	    throw new WebServiceException(String.format(CannotOpenConnection,
							url.getPath()),
					  e);
	}

	configureConnection(connection);

	try
	{
	    sendRequest(connection, requestBuilder.getRequest());
	    getResponse(connection);
	}
	catch (IOException e)
	{
	    throw new WebServiceException(CannotAccessWebService, e);
	}
    }

    private static HttpRequestBuilder buildRequest(String encryptedWebMemberID,
						     String galleryID,
						     String base64PhotoString,
						     String dateTime)
	    throws UnsupportedEncodingException
    {
	HttpRequestBuilder requestBuilder = new HttpRequestBuilder();

	requestBuilder.addParam("EncryptedWebMemberID", encryptedWebMemberID);
	requestBuilder.addParam("WebPhotoCollectionID", galleryID);
	requestBuilder.addParam("Base64StringPhoto", base64PhotoString);
	requestBuilder.addParam("DateTime", dateTime);

	return requestBuilder;
    }

    private static void configureConnection(URLConnection connection)
    {
	connection.setDoOutput(true);
	connection.setDoInput(true);
	connection.setUseCaches(false);
	connection.setRequestProperty("Content-Type",
				      "application/x-www-form-urlencoded");
    }

    private static void sendRequest(URLConnection connection, String request)
	    throws IOException
    {
	OutputStreamWriter streamWriter = null;

	try
	{
	    streamWriter = new OutputStreamWriter(connection.getOutputStream());
	    streamWriter.write(request);
	    streamWriter.flush();
	}
	finally
	{
	    if (streamWriter != null)
	    {
		try
		{
		    streamWriter.close();
		}
		catch (IOException ignored)
		{
		}
	    }
	}
    }

    private static String getResponse(URLConnection connection)
	    throws IOException
    {
	BufferedReader bufferedReader = null;
	StringBuffer stringBuffer = null;

	try
	{
	    bufferedReader =
		    new BufferedReader(new InputStreamReader(connection.getInputStream()));
	    stringBuffer = new StringBuffer();
	    String line;
	    while ((line = bufferedReader.readLine()) != null)
	    {
		stringBuffer.append(line);
		stringBuffer.append(Environment.NewLine);
	    }
	}
	finally
	{
	    if (bufferedReader != null)
	    {
		try
		{
		    bufferedReader.close();
		}
		catch (IOException ignored)
		{
		}
	    }
	}

	if (stringBuffer != null)
	{
	    return stringBuffer.toString();
	}

	return null;
    }

}
