package com.genetibase.askafriend.core;

import java.io.IOException;
import java.io.InputStream;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Vector;

import javax.microedition.lcdui.Image;
import javax.microedition.midlet.MIDlet;

import com.genetibase.askafriend.common.AbstractErrorManager;
import com.genetibase.askafriend.common.Deallocatable;
import com.genetibase.askafriend.common.ErrorEvent;
import com.genetibase.askafriend.common.ErrorListener;
import com.genetibase.askafriend.common.PauseListener;
import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.file.FileOperation;
import com.genetibase.askafriend.common.file.FileSourceWrapper;
import com.genetibase.askafriend.common.file.FileUtils;
import com.genetibase.askafriend.common.utils.GraphicsUtils;
import com.genetibase.askafriend.common.utils.LinkedHashTable;
import com.genetibase.askafriend.common.utils.Properties;
import com.genetibase.askafriend.common.utils.RunnableTask;
import com.genetibase.askafriend.common.utils.Serializable;
import com.genetibase.askafriend.common.utils.Utils;
import com.genetibase.askafriend.common.utils.WorkerThread;
import com.genetibase.askafriend.ui.GUIListener;
import com.genetibase.askafriend.utils.MemoryDispatcher;

public class ResourceManager extends AbstractErrorManager implements Resoursable, Deallocatable, PauseListener {
	private Properties config = null; 
	private Properties language = null;
	private static Hashtable cache = new Hashtable();
	private static LinkedHashTable previewCache = new LinkedHashTable();//(ImageWraper, Image)
	private static int PREVIEW_VECTOR_SIZE = 6;
	private SystemStorage systemStorage;
	private WorkerThread wt = new WorkerThread();
	private boolean isTouch;
//	private Object mutex = new Object();
	
	private Properties loadConfigFromRes(String locator) {
		Properties prop = new Properties();
		try {
			prop.loadProperties(this.getClass().getResourceAsStream(locator));
			
		} catch (IOException e) {
			fireError(e.getMessage(), 
					new ErrorEvent(this, 
							new RuntimeException("Sorry, MediaException happened in player"),
							"Sorry, MediaException happened in player", ErrorEvent.OPERATION_FAILED));

			e.printStackTrace();
		} return prop;
	}
	
	public ResourceManager() {
		this.config = loadConfigFromRes("/res/settings.properties");
		this.language = loadConfigFromRes("/res/locale.engb.properties");
//		System.out.println(language);
		systemStorage = new SystemStorage();
	}

	public void removeDuplicateProperties(SystemStorage systemStorage) {
		this.systemStorage = systemStorage;
		for (Enumeration enumer = systemStorage.keys(); enumer.hasMoreElements(); ) {
			String key  = (String)enumer.nextElement();
			if (config.getProperty(key) != null) {
				config.removeProperty(key);
			}
		}
	}

	public void setProperty(String key, String value) {
		this.config.removeProperty(key);
		this.systemStorage.put(key, value);
		this.systemStorage.store();
	}
	
	
	/* (non-Javadoc)
	 * @see com.tbox.core.Resoursable#getProperty(java.lang.Object)
	 */
	public String getProperty(String key) {
		String ret = null;
		if (config == null) {
			//TODO: write to log ("Unable to load default properties");
		} else {
			if ((systemStorage != null) && (ret =(String)systemStorage.get(key)) == null) {
				ret = config.getProperty(key);
			}
		}
		return ret;
	}

	/* (non-Javadoc)
	 * @see com.tbox.core.Resoursable#getProperty(java.lang.String, java.lang.String)
	 */
	public String getProperty(String key, String defaultValue) {
		String val = getProperty(key);
		return (val == null) ? defaultValue : val;
	}
	
    public String getAppProperty (String key) {
        return getMidlet().getAppProperty(key);
    }
    
    public MIDlet getMidlet() {
    	return Engine.getEngine().getMidlet();
    }
    
	/* (non-Javadoc)
	 * @see com.tbox.core.common.Deallocatable#free()
	 */
	public void free() {
		cache.clear();
		cache = null;
		config = null; 
		language = null;
		systemStorage.free();
		systemStorage = null;	
		FileUtils.free();
		removeAllErrorListeners();
		wt.free();
		wt = null;
	}

	public String getLocale(String key) {
		String ret = null;
		if (language == null) {
			//TODO: write to log ("Unable to load default properties");
		}
		else {
//			if ((systemStorage != null)&&(ret = (String)systemStorage.get(key)) == null) {
				ret = language.getProperty(key);
//			}
		}
		return ret;
	}

	public Image getImage(String name) {
		return getImage(name, true);
	}
	
	public Image getImage(String name, boolean cacheImage) {
		
		Image retImage =(Image) cache.get(name);
		if (retImage != null) {
			return retImage;
		}
		
		InputStream is = this.getClass().getResourceAsStream(name);
		
		if (is != null) {
			retImage = Utils.createImage(is);
			if (retImage != null && cacheImage) cache.put(name, retImage); 
		}
		return retImage;
	}
	
	public byte[] getImageForSend(String imgName) {
		byte[] ret = null;
		StreamWrapper streamWrapper = new StreamWrapper(imgName);
		FileUtils.getStreamData(streamWrapper);
		InputStream is = streamWrapper.getInputStream();
		if (is != null) {
			long size = streamWrapper.getSize();
			ret = new byte[(int) size];
			try {
				is.read(ret);
			} catch (IOException e) {
				fireError(e.getMessage(), new ErrorEvent(this, 
								new RuntimeException("Sorry, File system error on getting picture"),
								"Sorry, File system error on getting picture", ErrorEvent.PREVIEW));


			}
		}
		return ret;
	}
	
	public ImageWrapper getImageFromFile(String id, int idOnForm) throws Exception{
		ImageWrapper ret = null;
		//1. if we have necessary element - return it.
		if ((ret = (ImageWrapper)previewCache.get(id)) == null) {
			//2.//TODO: do not forget to update size!!!
			//3. if we do not have this image: load it and save to cache
			//3a. FileUtils.getImage(descr.getImageName())
			boolean wasLoaded = FileUtils.loadImage(ret = new ImageWrapper(id, idOnForm));
			if (wasLoaded) {
				Image resized = resizeImageBestFit(ret.getImage());
				ret.setPreviewLoaded(resized != null);
				if (resized != null) {
					ret.setImage(resized);
				}
			} else {
				ret.setPreviewLoaded(wasLoaded);
				ret.setImage(getImage("/nopreview.png"));
			}
			if (previewCache.size() > PREVIEW_VECTOR_SIZE-1) {
				if (((ImageWrapper)previewCache.get(previewCache.getFirstKey())).getIdOnForm() > idOnForm) {
//					move backward: remove last element
					previewCache.remove(previewCache.getLastKey());
					previewCache.put(id, ret, 0);
				} else {
//					move forward: remove 1st element
					previewCache.remove(previewCache.getFirstKey());
					previewCache.put(id, ret);
				}
				MemoryDispatcher.gc();
			} else {
				previewCache.put(id, ret);
			} 
		} 
		return ret;
	}

	public String getFileRoot() {
		return FileUtils.getUrl();
	}
	
	private static Image resizeImageBestFit(Image img) {
		Image ret = null;
	
		try {
			double scaleH = (double)46/img.getHeight();
			double scaleW = (double)43/img.getWidth();
			double scale = Math.min(scaleH,scaleW);
			ret = GraphicsUtils.scaleImage(img, scale);
			
		} catch (Throwable e) {
			e.printStackTrace();
		}
		return ret;
	}
	public void executeInWorkerThread(RunnableTask runnableTask) {
//		synchronized (mutex) {
			if (wt != null) {
				wt.put(runnableTask);
//			}
		}
	}

	public void openInit(GUIListener lis) {
		executeInWorkerThread(new FileOperation(FileOperation.INIT, lis));
	}
	
	public void delete(String selected, GUIListener listener) {
		executeInWorkerThread(new FileOperation(FileOperation.DELETE, selected, listener));
	}
	
	public void open(String selected, GUIListener listener) {
		wt.removeAllTasks();
		executeInWorkerThread(new FileOperation(FileOperation.OPEN, selected, listener));
	}
	
	public void save(String filename, byte[] source, GUIListener listener) {
		FileSourceWrapper fileWrapper = new FileSourceWrapper(filename, false, false, false, source);
		executeInWorkerThread(new FileOperation(FileOperation.SAVE_DATA, fileWrapper, listener));
	}
	
	public void preview(ImageWrapper descr, GUIListener listener) {
		executeInWorkerThread(new FileOperation(FileOperation.PREVIEW, descr, listener));
	}

	public Vector getCurrentFiles(String selectedFiel) {
		return FileUtils.getCurrentRootNames(selectedFiel);
	}

	public void disposePreviewCache() {
		previewCache.clear();
	}
	
	public void addErrorListener(ErrorListener errorListener) {
		super.addErrorListener(errorListener);
		if (wt != null) {
			wt.addErrorListener(errorListener);
		}
	}

	public void notify(int state) {
//		synchronized (mutex) {
			if (wt != null) {
				wt.free();
				wt = null;
			}
//		}
		
		if (state == STATE_RESTORE) {
			wt = new WorkerThread();
		}
	}

	public void setProperty(String key, Serializable value) {
		systemStorage.setProperty(key, value);
	}

	public void getProperty(String key, Serializable value) {
		systemStorage.getProperty(key, value);
	}

	public boolean isTouch() {
		return isTouch;
	}
	
	public void setTouch( boolean isTouch){
		this.isTouch = isTouch;
	}
}
