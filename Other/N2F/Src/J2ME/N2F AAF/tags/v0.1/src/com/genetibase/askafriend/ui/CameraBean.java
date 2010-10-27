package com.genetibase.askafriend.ui;

import com.genetibase.askafriend.camera.SnapshotEncodingWrapper;
import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.CommonKeys;

public class CameraBean extends AbstractBean 
{
	private SnapshotEncodingWrapper wrapper;
	
	public CameraBean(Resoursable resoursable) {
		super();
		readParameters(resoursable);
	}

	public void saveBean(Resoursable resoursable) {
		writeParameters(resoursable);
	}
	
	public SnapshotEncodingWrapper getWrapper() {
		return wrapper;
	}

	public void setWrapper(SnapshotEncodingWrapper wrapper) {
		this.wrapper = wrapper;
	}

	private void readParameters(Resoursable resoursable) 
	{
		String imageType = resoursable.getProperty(CommonKeys.KEY_IMAGE_TYPE);
		String value = resoursable.getProperty(CommonKeys.KEY_IMAGE_HEIGHT);
		
    	int imageH = 0;
    	if (value != null) imageH = Integer.parseInt(value);
    	
    	int imageW = 0;
    	value = resoursable.getProperty(CommonKeys.KEY_IMAGE_WIDTH);
    	
    	if (value != null) imageW = Integer.parseInt(value);
		wrapper = new SnapshotEncodingWrapper(imageType, imageW, imageH);
	}

	private void writeParameters(Resoursable resoursable) 
	{
		resoursable.setProperty(CommonKeys.KEY_SETTINGS_CAMERA, CommonKeys.YES);
		if (wrapper !=null) {
			resoursable.setProperty(CommonKeys.KEY_IMAGE_TYPE, wrapper.getEncName());
			resoursable.setProperty(CommonKeys.KEY_IMAGE_HEIGHT, String.valueOf(wrapper.getHeight()));
			resoursable.setProperty(CommonKeys.KEY_IMAGE_WIDTH, String.valueOf(wrapper.getWidth()));
		}
	}
}
