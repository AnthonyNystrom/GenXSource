package App;

import java.io.*;
import java.util.Vector;

public class Question
{

    public String   question;
    public int	    responseType;
    public int	    duration;
    public boolean  isPrivate;
    public String   responseA;
    public String   responseB;
    public Vector   photoNames;
    public int	    recordId;
    public boolean  sendNow;

    public Question()
    {
	question = "";
	responseType = 0;
	duration = 0;
	isPrivate = false;
	responseA = "";
	responseB = "";
	photoNames = new Vector();
	recordId = -1;
	sendNow = false;
    }
    
    public void delete(boolean fromDrafts, int index)
    {
	String storage;
	if(fromDrafts)
	{
	    storage = "drafts";
	}
	else
	{
	    storage = "outbox";
	}
	
	recordId = Core.storage.getRecordId(storage, index);
	Core.storage.deleteData(storage, recordId);
    }

    public void read(boolean fromDrafts, int index)
    {
	String storage;
	if(fromDrafts)
	{
	    storage = "drafts";
	}
	else
	{
	    storage = "outbox";
	}
	
	recordId = Core.storage.getRecordId(storage, index);
	byte[] data = Core.storage.readData(storage, recordId);
	ByteArrayInputStream is = new ByteArrayInputStream(data);
	DataInputStream dis = new DataInputStream(is);

	try
	{
	    sendNow	    = dis.readBoolean();
	    question	    = dis.readUTF();
	    responseType    = dis.readInt();
	    duration	    = dis.readInt();
	    isPrivate	    = dis.readBoolean();
	    responseA	    = dis.readUTF();
	    responseB	    = dis.readUTF();
	    int size	    = dis.readInt();
	    for(int i = 0; i < size; ++i)
	    {
		photoNames.addElement(dis.readUTF());
	    }
	    
	    dis.close();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }

    public void save(boolean toDrafts/*else to outbox*/)
    {
	ByteArrayOutputStream os = new ByteArrayOutputStream(Const.QUESTION_MAXSIZE);
	DataOutputStream dos = new DataOutputStream(os);

	try
	{
	    dos.writeBoolean(sendNow);
	    dos.writeUTF(question);
	    dos.writeInt(responseType);
	    dos.writeInt(duration);
	    dos.writeBoolean(isPrivate);
	    dos.writeUTF(responseA);
	    dos.writeUTF(responseB);
	    dos.writeInt(photoNames.size());
	    for(int i = 0; i < photoNames.size(); ++i)
	    {
		dos.writeUTF((String) photoNames.elementAt(i));
	    }
	    
	    dos.close();

	    String storage;
	    if(toDrafts)
	    {
		storage = "drafts";
	    }
	    else
	    {
		storage = "outbox";
	    }

	    Core.storage.writeData(os.toByteArray(), storage, recordId);
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }

    public static String[] getQuestionHeaders(String storeName)
    {
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
		boolean sendNow = dis.readBoolean();
		String header = dis.readUTF();
		out[i] = header.substring(0, Math.min(header.length(), 15)) +(sendNow ? " (Sending)" : "");
	    }
	    catch(Exception ex)
	    {
		ex.printStackTrace();
	    }
	}

	return out;
    }
}
