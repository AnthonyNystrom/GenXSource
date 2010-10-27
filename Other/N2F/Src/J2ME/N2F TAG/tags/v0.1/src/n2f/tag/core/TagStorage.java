package n2f.tag.core;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Hashtable;

import javax.microedition.rms.RecordEnumeration;
import javax.microedition.rms.RecordStore;
import javax.microedition.rms.RecordStoreException;

import n2f.tag.utils.RMSUtils;
import n2f.tag.utils.Utils;


public class TagStorage implements Deallocatable {
   
	private String rmsName;
	private RecordStore store = null;
 
	public TagStorage(String rmsName) {
		this.rmsName = rmsName;
		this.store = RMSUtils.openRecStore(rmsName);
		if (store == null) {
			throw new RuntimeException("TAG STORAGE WAS NOT CREATED, RMS NAME="+rmsName);
		}
	}
	
	synchronized public void write(String deviceTagID, String tagValidationString) {
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		DataOutputStream dos = new DataOutputStream(baos);
		if (store == null)
			store = RMSUtils.openRecStore(rmsName);
		try {
			if (store != null) {
				dos.writeUTF(deviceTagID);
				dos.writeUTF(tagValidationString);
				byte[] data = baos.toByteArray();
				store.addRecord(data, 0, data.length);
			}
		} catch (IOException e) {
			System.out.println("IOException: tag storage write");
			e.printStackTrace();
		} catch (RecordStoreException e) {
			System.out.println("RMSException: tag storage write");
			e.printStackTrace();
		} finally {
			Utils.close(dos);
			Utils.close(baos);
			dos = null;
			baos = null;
		}
	}
	
	synchronized public Hashtable readAllTags() {
		Hashtable table = new Hashtable();
		ByteArrayInputStream bais = null;
		DataInputStream dis = null;
		try {
			if (store != null) {
				RecordEnumeration en = store.enumerateRecords(null, null, false);
				for (;en.hasNextElement();) {
					 bais = new ByteArrayInputStream(en.nextRecord());
					 dis = new DataInputStream(bais);
					 table.put(dis.readUTF(), dis.readUTF());
					 Utils.close(dis);
					 Utils.close(bais);
				}
				en.destroy();
			}
		} catch (IOException e) {
			System.out.println("IOException: tag storage read");
			e.printStackTrace();
		} catch (RecordStoreException e) {
			System.out.println("RMSException: tag storage read");
			e.printStackTrace();
		} finally {
			Utils.close(dis);
			Utils.close(bais);
			bais = null;			
			dis = null;
			RMSUtils.deleteRMS(store);
			store = null;
		}
		return table;
	}

	public synchronized void free() {
		RMSUtils.deleteRMS(store);
		store = null;
	}
	
	
}
