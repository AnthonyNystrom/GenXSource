package com.genetibase.askafriend.camera;

import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Image;
import javax.microedition.media.MediaException;
import javax.microedition.media.control.VideoControl;

import com.genetibase.askafriend.common.ActionListener;
import com.genetibase.askafriend.common.Deallocatable;
import com.genetibase.askafriend.common.ErrorEvent;

public class VideoImpl extends PlayerImpl implements Deallocatable, VideoPlayer {
	private static final String VIDEO_CONTROL = "VideoControl";
	private static final String CAPTURE_VIDEO = "capture://video";
	private SnapshotEncodingWrapper preferredSettings;
	private VideoControl videoControl = null;
	
	public void free (){
		super.free();
		videoControl = null;
	}

	private void getSnapShotData(final ActionListener actionListener) {
		if (videoControl == null)
			throw new NullPointerException("VideoPlayer.getSnapShotData() == null");
			new Thread() {
				public void run(){
					byte[] imageData = null;
					try {
						log("snaphot=" + preferredSettings);
						imageData  = videoControl.getSnapshot(preferredSettings.toString()/*"encoding=jpeg&width=640&height=480"*/);
					} catch (Exception e) {
						fireError("VideoImpl.getSnapShotData=" + e.getMessage(), new ErrorEvent(this, e, e.getMessage(), ErrorEvent.MEDIA_FAILURE));
						if (isDebug()) {
							log("VideoImpl.getSnapShotData=" + e.getMessage());
							e.printStackTrace();
						}
					} finally {
						close();
						if (actionListener != null && imageData != null) {
							actionListener.actionPerformed(imageData, ActionListener.STATUS_OK);
						}
						
					} 
				}
			}.start();
	}
	
	public Image getSnapShotImage(byte[] imageData) {
		if (imageData == null) 
			throw new NullPointerException("VideoPlayer.SnapShotData == null");
		return Image.createImage(imageData, 0, imageData.length);
	}
	
	public void showCamera(Canvas canvas) {
	    try {
	    	createPlayer(CAPTURE_VIDEO);
	    	if (player != null) {
	    		prefetchPlayer();
		    	videoControl = (VideoControl)player.getControl(VIDEO_CONTROL);
	    	}
	      
			int width = canvas.getWidth();
			int height = canvas.getHeight();
		    
			videoControl.initDisplayMode(VideoControl.USE_DIRECT_VIDEO, canvas);
			try {
				videoControl.setDisplayLocation(2, 2);
				videoControl.setDisplaySize(width - 4, height - 4);
			} catch (MediaException me) {
				if (isDebug()) {
					log("VideoImpl.showCamera=" + me.getMessage());
					me.printStackTrace();
				}
				try { 
					videoControl.setDisplayFullScreen(true); 
				} catch (MediaException me2) {
					if (isDebug()) {
						log("VideoImpl.showCamera2=" + me2.getMessage());
						me2.printStackTrace();
					}
				}
			}
			videoControl.setVisible(true);
			player.start();
	    } catch (MediaException me) { 
			if (isDebug()) {
				log("VideoImpl.showCamera3=" + me.getMessage());
				me.printStackTrace();
			}
	    } catch (Exception e) {
			if (isDebug()) {
				log("VideoImpl.showCamera4=" + e.getMessage());
				e.printStackTrace();
			}
		}
	}	

	public void capture(ActionListener actionListener) {
		getSnapShotData(actionListener);
	}

	public void stopCamera() {
		stopAndClose();
		videoControl = null;
	}

	public void setPrefferedSettings(SnapshotEncodingWrapper wrapper) {
		this.preferredSettings = wrapper;
	}

	public void notify(int state) {
		if (state == STATE_PAUSE) {
			stopCamera();
		}
	}
	
}