package App;

import java.util.*;
import javax.microedition.io.*;
import service.*;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

public class NetworkSystem
{

    private String sessionCookie;

    public NetworkSystem()
    {
    }

    public InputStream makeRequest(String url, OutputStream pos) throws IOException
    {
	HttpConnection conn = null;
	ByteArrayOutputStream bos = null;
	DataInputStream is = null;
	DataOutputStream os = null;
	InputStream pis = null;

	try
	{
	    conn = (HttpConnection) Connector.open(url, Connector.READ_WRITE);
	    conn.setRequestMethod(HttpConnection.POST);
	    conn.setRequestProperty("Content-Type", "application/octet-stream");
	    conn.setRequestProperty("Accept", "application/octet-stream");

	    if(sessionCookie == null)
	    {
		conn.setRequestProperty("version", "???");
	    }
	    else
	    {
		conn.setRequestProperty("cookie", sessionCookie);
	    }


	    // Getting the output stream may flush the headers
	    os = conn.openDataOutputStream();
	    os.write(pos.getBytes());
	    os.close();

	    int responseCode;
	    try
	    {
		responseCode = conn.getResponseCode();
	    }
	    catch(IOException e)
	    {
		throw new IOException("No response from " + url);
	    }

	    if(responseCode != HttpConnection.HTTP_OK)
	    {
		throw new IllegalArgumentException();
	    }

	    bos = new ByteArrayOutputStream();
	    {
		is = conn.openDataInputStream();
		String sc = conn.getHeaderField("set-cookie");
		if(sc != null)
		{
		    sessionCookie = sc;
		}

		while(true)
		{
		    int ch = is.read();
		    if(ch == -1)
			break;

		    bos.write(ch);
		}

		is.close();
		is = null;

		conn.close();
		conn = null;
	    }
	    pis = new InputStream(bos.toByteArray());

	    bos.close();
	    bos = null;
	}
	catch(Exception e)
	{
	    e.printStackTrace();
	    try
	    {
		if(conn != null)
		{
		    conn.close();
		    conn = null;
		}

		if(bos != null)
		{
		    bos.close();
		    bos = null;
		}

		if(is != null)
		{
		    is.close();
		    is = null;
		}
	    }
	    catch(Exception exc)
	    {
	    }
	    throw new IOException();
	}

	return pis;
    }
}
