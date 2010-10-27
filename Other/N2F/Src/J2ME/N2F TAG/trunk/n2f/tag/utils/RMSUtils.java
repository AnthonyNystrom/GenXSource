package n2f.tag.utils;

import java.util.Vector;

import javax.microedition.rms.RecordStore;
import javax.microedition.rms.RecordStoreException;
import javax.microedition.rms.RecordStoreNotOpenException;

/**
 * @author Zyl
 * 
 * A collection of methods to deal with various RMS related activities
 */
public class RMSUtils{
	private static Vector rmsNames = new Vector();
	static {
		String[] rms = RecordStore.listRecordStores();
		for (int i = 0; rms != null && i < rms.length; i++) {
			rmsNames.addElement(rms[i]);
		}
	}
	
	public static final int UNKNOWN = -1;

	private RMSUtils() {
	}
	/**
	 * Open RMS (creates it if not exists yet!!)
	 * @param rmsId
	 * @return
	 */
	public static RecordStore openRecStore(String rmsId) {
		return openRecStore(rmsId, true);
	}

	public static RecordStore openRecStore(String rmsId, boolean create) 
	{
		RecordStore ret = null;
		
		addRMS2Names(rmsId);
			
		if (!nullOrBlank(rmsId)) {
			try {
				ret = RecordStore.openRecordStore(rmsId, create);
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
		return ret;
	}
	
	private static boolean nullOrBlank(String name) 
	{
		return (name == null) || (name.length()==0);
	}
	
	public static void closeRecStore(RecordStore rms) {
		if (rms != null) {
			try {
				rms.closeRecordStore();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	public static void deleteRMS(String id)
	{
		if (!nullOrBlank(id)) {
			if (rmsNames.contains(id)) {
				rmsNames.removeElement(id);
				
				try {
					RecordStore.deleteRecordStore(id);
				} catch (RecordStoreException e) {
					e.printStackTrace();
				}
			}
		}
	}
	
	public static void deleteRMS(RecordStore recStore)
	{
		String id = null;
		if (recStore != null) {
			try {
				id = recStore.getName();
			} catch (RecordStoreNotOpenException e) {
				e.printStackTrace();
			}
		} 
			
		if (!nullOrBlank(id)) {
			while (rmsNames.contains(id)) {
				rmsNames.removeElement(id);
				closeRecStore(recStore);
			}
			try {
				RecordStore.deleteRecordStore(id);
			} catch (RecordStoreException e) {
				e.printStackTrace();
			}

		}
	}

	static void addRMS2Names(String locator){
		if (!rmsNames.contains(locator)) {
			rmsNames.addElement(locator);
		}
	}
	
	public static void deleteRecord(RecordStore rec, int id) {
		if (rec != null) {
			try {
				rec.deleteRecord(id);
			} catch (RecordStoreException e) {
				e.printStackTrace();
			}
		}
	}

	public static void deleteAllRecStores() {
		String[] recNames = RecordStore.listRecordStores();
		rmsNames.removeAllElements();
		if(recNames != null) {
			for (int i = 0; i < recNames.length; i++) {
				deleteRMS(recNames[i]);
			}
		}
	}

	public static int getNumRecords(RecordStore rec) {
		int retValue = UNKNOWN;
		if (rec != null) {
			try {
				retValue = rec.getNumRecords();
			} catch (RecordStoreNotOpenException e) {
				e.printStackTrace();
			}
		}
		return retValue;
	}
}
