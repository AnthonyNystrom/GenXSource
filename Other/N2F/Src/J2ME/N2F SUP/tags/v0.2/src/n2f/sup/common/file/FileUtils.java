package n2f.sup.common.file;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.io.Connector;
import javax.microedition.io.file.ConnectionClosedException;
import javax.microedition.io.file.FileConnection;
import javax.microedition.io.file.FileSystemRegistry;
import javax.microedition.io.file.IllegalModeException;
import javax.microedition.lcdui.Image;

import n2f.sup.common.utils.Utils;
import n2f.sup.core.ImageWrapper;
import n2f.sup.core.StreamWrapper;
import n2f.sup.ui.UIManager;
import n2f.sup.utils.Debug;


public class FileUtils {
	private static Vector rootsList = new Vector();

	private final static String UPPER_DIR = "..";

//	private static long imageSize = 0;

	private final static String FILE_SEPARATOR ="/";

  // Stores the current root, if null we are showing all the roots
	private static FileConnection currentRoot = null;

	public static final String getUrl() {
		return currentRoot != null ? currentRoot.getURL(): null;
	}
	
	
	private static void loadRoots() {
		if (!rootsList.isEmpty()) {
	    	rootsList.removeAllElements();
	    }
		try {
	    	Enumeration roots = FileSystemRegistry.listRoots();
			while (roots.hasMoreElements()) {
				rootsList.addElement(new FileInfoWrapper(FILE_SEPARATOR
						+ (String) roots.nextElement(), currentRoot == null? "":currentRoot.getURL(), true, false, true));
	    	}
			FileSystemRegistry.addFileSystemListener(UIManager.getInstance());
	    } catch (Throwable e) {
	    	e.printStackTrace();
//	      show alert
	    }
	}
	
	public static Vector initDir() {
		String initDir = System.getProperty("fileconn.dir.photos");
    	loadRoots();
		if (initDir != null) {
    		try {
				currentRoot = (FileConnection) Connector.open(initDir,
						Connector.READ);
            } catch (Exception e) {
    	    	e.printStackTrace();
//    		      show alert
            	currentRoot = null;
            }
    	} else {
            currentRoot = null;
    	}
        return rootsList;
	}

	public static Vector getCurrentRootNames(String selectedFile) {
		Vector ret = new Vector();
		
		if  (currentRoot != null) {
			try {
				ret.addElement(new FileInfoWrapper(UPPER_DIR, currentRoot == null? "":currentRoot.getURL(), true, false, false));
				
				Enumeration listOfDirs = currentRoot.list("*", false);
				while (listOfDirs.hasMoreElements()) {
					String currentDir = (String) listOfDirs.nextElement();
					if (currentDir.endsWith(FILE_SEPARATOR)) {
						ret.addElement(new FileInfoWrapper(currentDir, currentRoot == null? "":currentRoot.getURL(), true, false,	false));
					}
				}
				Enumeration listOfFiles = currentRoot.list("*", false);
				
				while (listOfFiles.hasMoreElements()) {
					String currentFile = (String) listOfFiles.nextElement();
					if (!currentFile.endsWith(FILE_SEPARATOR)
							&& (currentFile.toLowerCase().endsWith(".png") || currentFile
									.toLowerCase().endsWith(".jpg"))) {
						ret.addElement(new FileInfoWrapper(currentFile, currentRoot == null? "":currentRoot.getURL(), false, false, false));
					}
				}
			} catch (Exception e) {
				e.printStackTrace();
			} 
		} else
			ret = rootsList;
		return ret;
	}
	
	public static void openSelected(String selectedFile) {
		if (selectedFile != null) {
			if (selectedFile.endsWith(FILE_SEPARATOR)) {
				try {
					if (currentRoot == null) {
						currentRoot = (FileConnection) Connector.open(
								"file:///" + selectedFile, Connector.READ);
					} else {
						currentRoot.setFileConnection(selectedFile);
					}
				} catch (IOException e)	{
			    	e.printStackTrace();
//				      show alert
				} catch (SecurityException e) {
			    	e.printStackTrace();
//				      show alert
				}
			} else if (selectedFile.equals(UPPER_DIR)) {
			
				if (!rootsList.contains(new FileInfoWrapper((currentRoot
						.getPath() + currentRoot.getName()).trim(), currentRoot == null? "":currentRoot.getURL(), true, false, true))) {
					try {
			       		currentRoot.setFileConnection(UPPER_DIR);
			       	} catch (IOException e) {
				    	e.printStackTrace();
//					      show alert
			       	}
			       	
			    } else
			    	currentRoot = null;
			 }
	    } 
	}
	
	public static boolean getStreamData(StreamWrapper streamWrapper) {
		if (streamWrapper == null)
			throw new NullPointerException();
		boolean ret = true;
		try {
			FileConnection fileConn = (FileConnection) Connector.open(streamWrapper.getId(), Connector.READ);
			if (fileConn != null) {
				streamWrapper.setCreationDate(fileConn.lastModified());
				streamWrapper.setSize(fileConn.fileSize());
				streamWrapper.setInputStream(fileConn.openInputStream());
				
			}
		} catch(Throwable t) {
			ret = false;
			if (Debug.isDebug()) {
				Debug.println("streamWrapper="+streamWrapper);
				t.printStackTrace();
			}
		}
		return ret;
    }
	
	public static boolean loadImage(ImageWrapper imageWrapper) throws Exception {
		if (imageWrapper == null)
			throw new NullPointerException();
		boolean ret = true;
		try {
			FileConnection fileConn = (FileConnection) Connector.open(imageWrapper.getId(), Connector.READ);
			if (fileConn != null) {
				imageWrapper.setSize(fileConn.fileSize());
				//TODO: take a look at docs if we need to close InputStream;
				imageWrapper.setImage(Image.createImage(fileConn.openInputStream()));
			}
		} 
		catch(Throwable t) {
			ret = false;
			if (Debug.isDebug()) {
				Debug.println("loadImage="+imageWrapper.getId());
				t.printStackTrace();
			}
			throw new RuntimeException(t.getMessage());
		}
		return ret;
    }
	

	private static void close(FileConnection fileConnection) throws IOException {
		if (fileConnection != null && fileConnection.isOpen()) {
			fileConnection.close();
			fileConnection = null;
		}
	}

	public static void saveData(FileSourceWrapper fsw) {
		log((currentRoot == null? "null": "not null"));
		if (currentRoot != null) {
			FileConnection file = null;
			try {
				file = (FileConnection) Connector.open(currentRoot.getURL() + /*(fsw.getFileName().endsWith("/") ? "" : "/") + */fsw.getFileName(),
						Connector.READ_WRITE);
				if (file != null /* && create */) {
					if (file.exists()) {
						file.delete(); 
					}	
					file.create(); 
					// set hidden attribute
					if (fsw.isHidden()) {
						file.setHidden(true);
					}
					log("FileUtils.saveData() "+ fsw.getFileName());
					DataOutputStream dos = file.openDataOutputStream();
					dos.write(fsw.getSource());
					dos.flush();
					log("DONE!");
					dos.close();
				} else {
					log ("file is null");
				}
				
			} catch (IOException e) {
				log ("WRITE: " + e.getMessage());
				e.printStackTrace();
				// show alert
			} finally {
				try {
					close(file);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}
	
	public static void save(FileSourceWrapper fsw, FileActionListener fileActionListener){
		log((currentRoot == null? "null": "not null"));
		if (currentRoot != null) {
			FileConnection file = null;
			DataOutputStream dos = null;
//			int ret_status = FileActionListener.STATUS_FILE_FAILED_READ_WRITE;
			try {
				file = (FileConnection) Connector.open(currentRoot.getURL() + fsw.getFileName(), Connector.READ_WRITE);
				if (file.availableSize() <= fsw.getSource().length) {
//					ret_status = FileActionListener.STATUS_FILE_FAILED_NOT_ENOUGHT_SPACE;
					return;
				}
//				ret_status = FileActionListener.STATUS_FILE_FAILED_EXIST;
				if (!file.exists()) {
//					ret_status = FileActionListener.STATUS_FILE_FAILED_CREATE;
					file.create();
				} else if (!fsw.isOverwrite()){
					throw new IllegalArgumentException("File exist woudl you like overwrite?");
				}
				// set hidden attribute
				if (fsw.isHidden()) {
					file.setHidden(true);
				}
				dos = file.openDataOutputStream();
				dos.write(fsw.getSource());
				dos.flush();
			} catch (SecurityException e) {
				fileActionListener.actionPerformed(null, FileActionListener.STATUS_FILE_FAILED_SECURITY);
			} catch (IOException e) {
				fileActionListener.actionPerformed(e, FileActionListener.STATUS_FILE_FAILED);
			} catch (IllegalModeException e) {
				fileActionListener.actionPerformed(null, FileActionListener.STATUS_FILE_FAILED);
			} catch (ConnectionClosedException e) {
				fileActionListener.actionPerformed(null, FileActionListener.STATUS_FILE_FAILED_CLOSE);
			} catch (Exception e) {
				fileActionListener.actionPerformed(e, FileActionListener.STATUS_FILE_FAILED);
			} catch (Throwable e) {
				fileActionListener.actionPerformed(e, FileActionListener.STATUS_FAILED);
			} finally {
				Utils.close(dos);
				try {
					close(file);
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
		}
	}
	

	private static void log(String str) {
		if (Debug.isDebug()) {
			Debug.println(str);
		}
	}
	
	public static void createNewFile(String newFileURL) {
		// TODO;
	}

//	public static long getImageSize() {
//		return imageSize;
//	}
//
	public static void free() {
		
		FileSystemRegistry.removeFileSystemListener(UIManager.getInstance());
		
	}

}
