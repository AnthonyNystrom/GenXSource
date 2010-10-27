package com.genetibase.askafriend.common.utils;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

import javax.microedition.io.Connection;
import javax.microedition.lcdui.Image;

import com.genetibase.askafriend.utils.Debug;

public class Utils {
	/**
	 * @param c
	 */
	public static void close(Connection c) {
		if (c != null)
		try {
			c.close();
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			c = null;
		}
	}
	/**
	 * Closes this input stream and releases any system resources associated with the stream
	 * 
	 * @param is
	 */
	public static void close (InputStream is) {
		if (is != null) 
		try {
			is.close();
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			is = null;
		}
	} 
	/**
	 * Closes this output stream and releases any system resources associated with the stream
	 * @param os
	 */
	public static void close (OutputStream os) {
		if (os != null)		
		try {
			os.close();
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			os = null;
		}
	}
	
	public static int microToSec(long micro) {
        return (int) (micro / 1000000);
    }	
	
	/**
	 * Creates an immutable image from decoded image data obtained from an
	 * InputStream.
	 * 
	 * @param is -
	 *            the name of the resource containing the image data in one of
	 *            the supported image formats
	 * @return created image
	 */
	public static final Image createImage(InputStream is) {
		Image ret = null;
		if (is != null) {
			try {
				ret = Image.createImage(is);
			} catch (Throwable ex) {
				if (Debug.isDebug()) {
					Debug.println("createImage is Error");
					Debug.error(ex.getMessage(), ex);
				}
				ex.printStackTrace();
			}
			close(is);
		}
		return ret;
	}
	
	public static final Image createImage(byte[] imageBytes) {
		Image ret = null;
		if (imageBytes != null) {
			try {
				ret = Image.createImage(imageBytes, 0, imageBytes.length);
			} catch (Throwable ex) {
				if (Debug.isDebug()) {
					Debug.println("createImage is Error");
					Debug.error(ex.getMessage(), ex);
				}
				ex.printStackTrace();
			}
		}
		return ret;
	}

}
