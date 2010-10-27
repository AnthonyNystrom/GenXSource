package n2f.sup.core;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Vector;

import javax.microedition.rms.RecordEnumeration;
import javax.microedition.rms.RecordStore;
import javax.microedition.rms.RecordStoreException;

import n2f.sup.common.Deallocatable;
import n2f.sup.common.rms.RMSUtils;
import n2f.sup.common.utils.Serializable;
import n2f.sup.common.utils.Utils;


/**
 * @author Zyl
 * 
 *	This class is responsible for System data keeping. 
 *	!!It can't be stored in File System!! RMS should be used ONLY!
 *
 */
public class SystemStorage implements Deallocatable {
	/**
	 * There could be first start application date
	 * isFlexible rms
	 * Initial frame size
	 * url pool
	 */
	public static final String ID = "-SystemStorage-";
	public static final String KEY_INITIAL_DATE = "-date-";
	public static final String KEY_FRAME_SIZE = "-frame size-";
	private static final String RMS_SUFIX = "-=-RMS";
	private Hashtable systemData = new Hashtable();
	private boolean isFirstLaunche = false;
	
	public SystemStorage() {
		free();
		load();
		deleteUnusefullRMS();
	}

	public Hashtable getSystemData() {
		return systemData;
	}
	
	public void put(String key, String value) {
		if (key != null && (value!= null)) {
			systemData.put(key, value);
		}
	}
	
	public String get(String name) {
		String ret = null;
		if (name != null) {
			ret = (String) systemData.get(name);
		}
		return ret;
	}

	public Enumeration keys() {
		return systemData.keys();
	}

	public boolean store() {
		boolean isStored = false;
		RMSUtils.deleteRMS(ID);
		RecordStore rms = RMSUtils.openRecStore(ID);
		if (rms != null) {
			isStored = true;
			ByteArrayOutputStream baos = new ByteArrayOutputStream();
			DataOutputStream dos = new DataOutputStream(baos);
			try {
				dos.writeInt(systemData.size());
				for (Enumeration enumm = systemData.keys(); enumm.hasMoreElements();) {
					String key = (String) enumm.nextElement();
					String value = (String) systemData.get(key);
					dos.writeUTF(key);
					dos.writeUTF(value);
				}
				byte[] data = baos.toByteArray();
				rms.addRecord(data, 0, data.length);
			} catch (Exception e) {
				isStored = false;
				e.printStackTrace();
			} finally {
				Utils.close(dos);
				Utils.close(baos);
				RMSUtils.closeRecStore(rms);
			}
		}
		return isStored;
	}


	public boolean isFirstTimeLaunched() {
		return this.isFirstLaunche;
	}

	public boolean load() {
//		Debug.println("SystemStorage.load()");
		boolean isLoaded = true;
		RecordStore rms = RMSUtils.openRecStore(ID);
		if (rms != null) {
			RecordEnumeration enumm = null;
			try {
				boolean hasRecords = false;
				for (enumm = rms.enumerateRecords(null, null, false); enumm.hasNextElement();) {
					hasRecords = true;
					int recordId = enumm.nextRecordId();
					byte[] data = rms.getRecord(recordId);
					if (data != null) {
						ByteArrayInputStream bais = new ByteArrayInputStream(data);
						DataInputStream dis = new DataInputStream(bais);
						try {
							int quantity = dis.readInt();
							for (int i = 0; i < quantity; i++) {
								String key = dis.readUTF();
								String value = dis.readUTF();
								this.systemData.put(key, value);
							}
						} catch (IOException e) {
							isLoaded = false;
							e.printStackTrace();
						} finally {
							Utils.close(dis);
							Utils.close(bais);
						}
					}
				}
				if (!hasRecords) {
					firstLaunch(rms);
					rms = null;
				} else {
					this.isFirstLaunche = false;
				}
			} catch (RecordStoreException e) {
				isLoaded = false;
				e.printStackTrace();
			} finally {
				RMSUtils.closeRecStore(rms);
				if (enumm != null)
					enumm.destroy();
			}
		} else {
			isLoaded = false;
		}
		return isLoaded;
	}

	private void deleteUnusefullRMS() {
		Vector keys = new Vector();
		for (Enumeration enumm = systemData.keys(); enumm.hasMoreElements();) {
			String str = (String) enumm.nextElement();
			if (str.startsWith(RMS_SUFIX)) {
				RMSUtils.deleteRMS(str);
				keys.addElement(str);
			}
		}
		for (Enumeration enumm = keys.elements(); enumm.hasMoreElements();) {
			systemData.remove(enumm.nextElement());
		}
		if (keys.size() > 0) store();
	}

	private void firstLaunch(RecordStore rms) {
		this.isFirstLaunche = true;
		systemData.put(KEY_INITIAL_DATE, String.valueOf(System.currentTimeMillis() / 86400000));
		int initialSize = 51000;//rms.getSizeAvailable();
		systemData.put(KEY_FRAME_SIZE, String.valueOf(initialSize));
		RMSUtils.closeRecStore(rms);
		store();
	}

	public String toString() {
		StringBuffer sb = new StringBuffer(ID);
		sb.append('[');
		for (Enumeration enumm = systemData.keys(); enumm.hasMoreElements();) {
			String key = (String) enumm.nextElement();
			sb.append(key);
			sb.append('=');
			sb.append((String) systemData.get(key));
		}
		sb.append(']');
		return sb.toString();
	}

	/* (non-Javadoc)
	 * @see com.musiwave.de.Deallocatable#free()
	 */
	public void free() {
		systemData.clear();
	}

	public void setProperty(String key, Serializable value) {
		System.out.println("SAVE BEAN");
		RMSUtils.deleteRMS(key);
		RecordStore rms = RMSUtils.openRecStore(key);
		if (rms != null) {
			ByteArrayOutputStream baos = new ByteArrayOutputStream();
			DataOutputStream dos = new DataOutputStream(baos);
			try {
				value.writeObject(dos);
				byte[] data = baos.toByteArray();
				rms.addRecord(data, 0, data.length);
				System.out.println("IN PROGRESS");
			} catch (Exception e) {
				e.printStackTrace();
			} finally {
				Utils.close(dos);
				Utils.close(baos);
				RMSUtils.closeRecStore(rms);
				System.out.println("SAVE BEAN = DONE");
			}
		}
	}

	public void getProperty(String key, Serializable value) {
		RecordStore rms = RMSUtils.openRecStore(key);
		if (rms != null) {
			System.out.println("get BEAN");
			
			RecordEnumeration enumm = null;
			try {
//				boolean hasRecords = false;
				for (enumm = rms.enumerateRecords(null, null, false); enumm.hasNextElement();) {
//					hasRecords = true;
					int recordId = enumm.nextRecordId();
					byte[] data = rms.getRecord(recordId);
					if (data != null) {
						ByteArrayInputStream bais = new ByteArrayInputStream(data);
						DataInputStream dis = new DataInputStream(bais);
						try {
							value.readObject(dis);
						} catch (IOException e) {
							e.printStackTrace();
						} finally {
							Utils.close(dis);
							Utils.close(bais);
							System.out.println("IN PROGRESS");
						}
					}
				}
			} catch (RecordStoreException e) {
				e.printStackTrace();
			} finally {
				System.out.println("DONE");
				RMSUtils.closeRecStore(rms);
				if (enumm != null)
					enumm.destroy();
			}
		}
	} 
}
