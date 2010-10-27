package com.genetibase.askafriend.camera;

import java.util.Hashtable;
import java.util.Vector;

import com.genetibase.askafriend.common.utils.StringTokenizer;
import com.genetibase.askafriend.core.CommonKeys;
import com.genetibase.askafriend.utils.Debug;

public class MediaUtils 
{
	private static final String SUPPORTS_VIDEO_CAPTURE = "supports.video.capture";
	private static final String VIDEO_SNAPSHOT_ENCODINGS = "video.snapshot.encodings";
	private static String[] SNAPSHOT_ENCODINGS = getSnapshotEncodings(); 
	private static final String PLATFORM = getPlatform(); 
	public static final Hashtable ENCODING_WRAPPERS = initializeEncodingVector();
	
	private static String getPlatform() {
		return System.getProperty("microedition.platform");
	}

	public static boolean supportsCapturing() {
		String captureProperty = System.getProperty(SUPPORTS_VIDEO_CAPTURE);
		Debug.println(SUPPORTS_VIDEO_CAPTURE + "=" + captureProperty);
		if (captureProperty != null) {
			captureProperty = captureProperty.toLowerCase();
		}
		return captureProperty != null && (captureProperty.indexOf(CommonKeys.YES) >-1 ||
					captureProperty.indexOf(CommonKeys.TRUE) >-1);
		//TODO: uncomment for SE simulator
//		return true;
		
	}
	
	private static String[] getSnapshotEncodings() {
		String encodings = System.getProperty(VIDEO_SNAPSHOT_ENCODINGS);
		if (Debug.isDebug()) {
			Debug.println(VIDEO_SNAPSHOT_ENCODINGS + " " + encodings);		
		}
		String[] ret = null;
//TODO:on the real device and WTK22-25
		if (encodings != null) {
			Vector enc = new Vector();
			StringTokenizer tokenizer = new StringTokenizer(encodings, " ");
			for (;tokenizer.hasMoreTokens(); ){
				enc.addElement(tokenizer.nextToken());
			}
		
			ret = new String[enc.size()]; 
			for (int i = 0; i < enc.size(); i++) {
				ret[i] = (String)enc.elementAt(i);
			}
		}
//		TODO:SE simulator TEST
//		ret = new String[] {"encoding=jpeg", "encoding=png", "encoding=jpeg&width=480&height=640",
//				"encoding=jpeg&width=240&height=320"};
//		TODO:Nokia N series TEST
//		ret = new String[] {"encoding=image/jpg","encoding=image/jpeg", "encoding=image/png", "encoding=image/3peg"};
		return ret;
	}

	private static final Hashtable initializeEncodingVector() {
		
Debug.println("PLATFORM:"+PLATFORM);
		Hashtable ret = new Hashtable(); 
		if (SNAPSHOT_ENCODINGS == null)
			SNAPSHOT_ENCODINGS = getSnapshotEncodings();
		
		for (int i = 0; SNAPSHOT_ENCODINGS != null && i < SNAPSHOT_ENCODINGS.length; i++) {
			if (SNAPSHOT_ENCODINGS[i] != null) {
Debug.println("SNAPSHOT ENCODINGS"+SNAPSHOT_ENCODINGS[i]);	
				if (SNAPSHOT_ENCODINGS[i].indexOf("jp") > - 1) {
					SnapshotEncodingWrapper wrapper = new SnapshotEncodingWrapper(SNAPSHOT_ENCODINGS[i]);
					if (PLATFORM.toLowerCase().indexOf("nokia") > -1) {
						wrapper.setHeight(320);
						wrapper.setWidth(240);
						ret.put(wrapper.getDescription(), wrapper);
						wrapper.setHeight(480);
						wrapper.setWidth(320);
						ret.put(wrapper.getDescription(), wrapper);
						wrapper.setHeight(640);
						wrapper.setWidth(480);
						ret.put(wrapper.getDescription(), wrapper);
						break;
					} else {
						ret.put(wrapper.getDescription(), wrapper);
					}
				}
			}
		}
Debug.println("enc wrapper:"+ret);
		return ret; 
	}
}
