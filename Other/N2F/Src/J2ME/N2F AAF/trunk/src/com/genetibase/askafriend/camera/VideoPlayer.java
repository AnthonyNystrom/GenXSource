package com.genetibase.askafriend.camera;

import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Image;

import com.genetibase.askafriend.common.ActionListener;

public interface VideoPlayer extends SimplePlayer {
	Image getSnapShotImage(byte[] imageData);
	void showCamera(Canvas canvas);
	void capture(ActionListener actionListener);
	void stopCamera();
	void setPrefferedSettings(SnapshotEncodingWrapper wrapper);
}
