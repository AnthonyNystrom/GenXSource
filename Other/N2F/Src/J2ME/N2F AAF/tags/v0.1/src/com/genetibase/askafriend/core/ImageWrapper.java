package com.genetibase.askafriend.core;

import javax.microedition.lcdui.Image;

public class ImageWrapper extends DataWrapper{
	private final int idOnForm;
	private boolean isPreviewLoaded;
	private Image image;
	
	public ImageWrapper(String id, int idOnForm) {
		super(id);
		this.idOnForm = idOnForm;
	}
	
	public int getIdOnForm() {
		return idOnForm;
	}

	public boolean isPreviewLoaded() {
		return isPreviewLoaded;
	}

	public void setPreviewLoaded(boolean isPreviewLoaded) {
		this.isPreviewLoaded = isPreviewLoaded;
	}
	
	public Image getImage() {
		return image;
	}
	public void setImage(Image image) {
		this.image = image;
	}
	
	
}
