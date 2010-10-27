package App;

import javax.microedition.rms.*;
import java.io.*;
import javax.microedition.io.file.*;
import javax.microedition.io.*;
import java.util.Enumeration;
import java.util.Vector;
import javax.microedition.lcdui.Image;
import tag.BTLocator;

//#ifdef BLACKBERRY
//# import net.rim.device.api.math.Fixed32;
//# import net.rim.device.api.system.Bitmap;
//# import net.rim.device.api.system.EncodedImage;
//# import net.rim.device.api.system.JPEGEncodedImage;
//#endif

public class Storage
{

    public String login;
    public String password;
    public String tagId;
    public String key;

    public Storage()
    {
	login = new String();
	password = new String();
	tagId = new String();
	key = new String();
    }

    public Enumeration getImagesList()
    {
	Enumeration photos = null;
	try
	{
//#ifdef BLACKBERRY
//#             FileConnection file = (FileConnection) Connector.open("file:///store/home/user/pictures/", Connector.READ);
//#elifdef NOKIA
//# 	    FileConnection file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos"), Connector.READ);
//#elifdef MOTOROLA
//#         FileConnection file = (FileConnection) Connector.open("file:///c/mobile/picture/", Connector.READ);
//#endif
	    photos = file.list("*.jpg", false);
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
	return photos;
    }
    
    public byte[] readBytes(String fileName)
    {
	byte[] bytes = null;
	try
	{
//#ifdef BLACKBERRY
//#             FileConnection file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + fileName, Connector.READ);
//#elifdef NOKIA
//# 	    FileConnection file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + fileName, Connector.READ);
//#elifdef MOTOROLA            
//#         FileConnection file = (FileConnection) Connector.open("file:///c/mobile/picture/"+fileName, Connector.READ);
//#endif            
	    if(!file.exists())
	    {
		return null;
	    }
	    InputStream is = file.openInputStream();
	    int size = (int)file.fileSize();
	    bytes = new byte[size];
	    is.read(bytes);
	    
	    is.close();
	    file.close();
	}
	catch(Exception ex)
	{
	    System.out.println("Cannot open file " + fileName);
	    ex.printStackTrace();
	}
	finally
	{
	    return bytes;
	}
    }

//#ifdef BLACKBERRY
//#     public Image readImage(String fileName, int scaleY)
//#else
    public Image readImage(String fileName)
//#endif                
    {
	Image img = null;
	try
	{
//#ifdef BLACKBERRY
//#             FileConnection file = (FileConnection) Connector.open("file:///store/home/user/pictures/" + fileName, Connector.READ);
//#elifdef NOKIA
//# 	    FileConnection file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + fileName, Connector.READ);
//#elifdef MOTOROLA           
//#         FileConnection file = (FileConnection) Connector.open("file:///c/mobile/picture/"+fileName, Connector.READ);
//#endif            
	    if(!file.exists())
	    {
		return null;
	    }
	    InputStream is = file.openInputStream();
//#ifdef BLACKBERRY
//#             int size = (int)file.fileSize();
//#             byte[] buff = new byte[size];
//#             is.read(buff, 0, size);
//#             EncodedImage jpeg = EncodedImage.createEncodedImage(buff, 0, size);
//#             int bigHeightFP = Fixed32.toFP(jpeg.getHeight());
//#             int scaleYFP = Fixed32.toFP(scaleY);
//#             int scaleCoefFP = Fixed32.div(bigHeightFP, scaleYFP);
//#             EncodedImage outJpeg = jpeg.scaleImage32(scaleCoefFP, scaleCoefFP);
//#             jpeg = null;
//#             Bitmap bitmap = outJpeg.getBitmap();
//#             int w = bitmap.getWidth();
//#             int h = bitmap.getHeight();
//#             int[] argb = new int[w*h];
//#             bitmap.getARGB(argb, 0, w, 0, 0, w, h);
//#        
//# 	    img = Image.createRGBImage(argb, w, h, false);
//#else
	    img = Image.createImage(is);
//#endif            
	    is.close();
	    file.close();
	}
	catch(Exception ex)
	{
	    System.out.println("Cannot open file " + fileName);
	    ex.printStackTrace();
	}
	finally
	{
	    return img;
	}
    }

    public String writeImage(byte[] img)
    {
	String fileName = null;
	try
	{
	    int counter = 0;
	    FileConnection file = null;
	    do
	    {
		counter++;
//#ifdef BLACKBERRY
//#                 file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + "img" + counter + ".jpg");
//#elifdef NOKIA                
//# 		file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + "img" + counter + ".jpg");
//#elifdef MOTOROLA                           
//#             file = (FileConnection) Connector.open("file:///c/mobile/picture/"+ "img" + counter + ".jpg");
//#endif                
	    }
	    while(file.exists());
	    file.create();

	    OutputStream os = file.openOutputStream();
	    os.write(img);
	    os.close();
	    file.close();
	    fileName = "img" + counter + ".jpg";
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
	finally
	{
	    return fileName;
	}
    }

    public void readWriteSettings(boolean read)
    {
	RecordStore rs;
	try
	{
	    rs = RecordStore.openRecordStore("n2f", true);
	    int numRecords = rs.getNumRecords();


	    if(read)
	    {
		if(numRecords == 0)
		{
		    return;
		}
		int size = rs.getRecordSize(1);
		byte store[] = new byte[size];
		rs.getRecord(1, store, 0);
		ByteArrayInputStream bais = new ByteArrayInputStream(store);
		DataInputStream dis = new DataInputStream(bais);

		login = dis.readUTF();
		password = dis.readUTF();
		Core.btLocator.isEnabled = dis.readBoolean();
		//Core.btLocator.lastUpdate = dis.readLong();
		//Core.btLocator.sleepTime = dis.readLong();
	    }
	    else
	    {
		ByteArrayOutputStream baus = new ByteArrayOutputStream();
		DataOutputStream dos = new DataOutputStream(baus);

		dos.writeUTF(login);
		dos.writeUTF(password);
		dos.writeBoolean(Core.btLocator.isEnabled);
		//dos.writeLong(Core.btLocator.lastUpdate);
		//dos.writeLong(Core.btLocator.sleepTime);

		byte[] store = baus.toByteArray();
		int size = store.length;

		if(numRecords > 0)
		{
		    rs.setRecord(1, store, 0, size);
		}
		else
		{
		    rs.addRecord(store, 0, size);
		}
	    }

	    rs.closeRecordStore();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }

    public void writeData(byte[] data, String storeName, int recordId)
    {
	RecordStore rs;
	try
	{
	    rs = RecordStore.openRecordStore(storeName, true);
	    int size = data.length;
	    if(recordId == -1)
	    {
		rs.addRecord(data, 0, size);
	    }
	    else
	    {
		try
		{
		    rs.setRecord(recordId, data, 0, size);
		}
		catch(Exception ex)
		{
		    rs.addRecord(data, 0, size);
		}
	    }
	    rs.closeRecordStore();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }
    
    public int getRecordId(String storeName, int recordNum)
    {
	RecordStore rs;
	int recordId = -1;
	try
	{
	    rs = RecordStore.openRecordStore(storeName, true);
	    RecordEnumeration enumer = rs.enumerateRecords(null, null, false);
	    while(recordNum > 0)
	    {
		enumer.nextRecordId();
		recordNum--;
	    }
	    recordId = enumer.nextRecordId();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
	return recordId;
    }
    
    public byte[] readData(String storeName, int recordId)
    {
	RecordStore rs;
	byte[] out = null;
	try
	{
	    rs = RecordStore.openRecordStore(storeName, false);
	    out = rs.getRecord(recordId);
	}
	catch(Exception ex)
	{
	    //ex.printStackTrace();
	}
	
	return out;
    }
    
    public void deleteData(String storeName, int recordId)
    {
	RecordStore rs;
	try
	{
	    rs = RecordStore.openRecordStore(storeName, true);
	    rs.deleteRecord(recordId);
	    rs.closeRecordStore();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }
    
    public int countData(String storeName)    
    {
	RecordStore rs;
	int recordCount;
	try
	{
	    rs = RecordStore.openRecordStore(storeName, true);
	    recordCount = rs.getNumRecords();
	    
	    rs.closeRecordStore();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	    recordCount = 0;
	}
	return recordCount;
    }
    
}
