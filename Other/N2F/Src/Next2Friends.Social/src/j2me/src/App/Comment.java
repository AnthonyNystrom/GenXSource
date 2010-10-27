package App;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;

public class Comment
{
    public int questionId;
    public String login;
    public String text;
    
    public int recordId;
    
    private static String storage = "comments";
    
    public Comment()
    {
	login = Core.storage.login;
	text = "";
	recordId = -1;
    }
    
    public void delete(int index)
    {
	recordId = Core.storage.getRecordId(storage, index);
	Core.storage.deleteData(storage, recordId);
    }
    
    public void read(int index)
    {
	recordId = Core.storage.getRecordId(storage, index);
	byte[] data = Core.storage.readData(storage, recordId);
	ByteArrayInputStream is = new ByteArrayInputStream(data);
	DataInputStream dis = new DataInputStream(is);

	try
	{
	    questionId	    = dis.readInt();
	    login	    = dis.readUTF();
	    text	    = dis.readUTF();
	    	    
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
	    dos.writeInt(questionId);
	    dos.writeUTF(login);
	    dos.writeUTF(text);	  
	    
	    dos.close();

	    Core.storage.writeData(os.toByteArray(), storage, recordId);
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }
    
    public static String[] getCommentsHeaders()
    {
	int count = Core.storage.countData(storage);
	String[] out = new String[count];

	for(int i = 0; i < count; ++i)
	{
	    int recordId = Core.storage.getRecordId(storage, i);
	    byte[] data = Core.storage.readData(storage, recordId);
	    ByteArrayInputStream is = new ByteArrayInputStream(data);
	    DataInputStream dis = new DataInputStream(is);

	    try
	    {
		dis.readInt();
		dis.readUTF();
		String header = dis.readUTF();
		out[i] = header.substring(0, Math.min(header.length(), 15));
	    }
	    catch(Exception ex)
	    {
		ex.printStackTrace();
	    }
	}

	return out;
    }
}
