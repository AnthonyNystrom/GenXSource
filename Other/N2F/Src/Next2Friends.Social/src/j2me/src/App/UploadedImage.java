
package App;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;

public class UploadedImage
{
    public String photoName;
    
    public static void delete()
    {
        String storage = "upload";
        int recordId = Core.storage.getRecordId(storage, 0);
	Core.storage.deleteData(storage, recordId);
    }
    
    public void read(int index)
    {
        String storage = "upload";
	
	int recordId = Core.storage.getRecordId(storage, index);
	byte[] data = Core.storage.readData(storage, recordId);
	ByteArrayInputStream is = new ByteArrayInputStream(data);
	DataInputStream dis = new DataInputStream(is);

	try
	{
            photoName = dis.readUTF();
	    dis.close();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }
    
    public void save()
    {
	ByteArrayOutputStream os = new ByteArrayOutputStream(Const.QUESTION_MAXSIZE);
	DataOutputStream dos = new DataOutputStream(os);

	try
	{
            dos.writeUTF(photoName);
	    dos.close();

	    String storage = "upload";
	    Core.storage.writeData(os.toByteArray(), storage, -1);
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }
    
    public static String[] getHeaders()
    {
        String storeName = "upload";
	int count = Core.storage.countData(storeName);
	String[] out = new String[count];

	for(int i = 0; i < count; ++i)
	{
	    int recordId = Core.storage.getRecordId(storeName, i);
	    byte[] data = Core.storage.readData(storeName, recordId);
	    ByteArrayInputStream is = new ByteArrayInputStream(data);
	    DataInputStream dis = new DataInputStream(is);

	    try
	    {
		out[i] = dis.readUTF();
	    }
	    catch(Exception ex)
	    {
		ex.printStackTrace();
	    }
	}

	return out;
    }
}
