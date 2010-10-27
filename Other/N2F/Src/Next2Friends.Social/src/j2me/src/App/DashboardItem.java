package App;

import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;

public class DashboardItem
{

    public int iconFrame;
    public String title;
    public String date;
    public String fullDate;
    public String text;

    public byte[] serialize()
    {
	ByteArrayOutputStream os = new ByteArrayOutputStream();
	DataOutputStream dos = new DataOutputStream(os);

	try
	{
	    dos.writeInt(iconFrame);
	    dos.writeUTF(title);
	    dos.writeUTF(date);
            dos.writeUTF(fullDate);
	    dos.writeUTF(text);
	}
	catch(Exception ex){};
	
	return os.toByteArray();
    }
    
    public void deserialize(DataInputStream is)
    {
	try
	{
	    iconFrame = is.readInt();
	    title = is.readUTF();
	    date = is.readUTF();
            fullDate = is.readUTF();
	    text = is.readUTF();
	}
	catch(Exception ex){};
    }
}
