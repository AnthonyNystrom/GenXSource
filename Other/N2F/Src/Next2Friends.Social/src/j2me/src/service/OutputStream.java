package service;

import java.io.*;

public class OutputStream
{
    protected ByteArrayOutputStream os = null;

    public OutputStream()
    {
	os = new ByteArrayOutputStream();
    }

    public void close()
    {
	if(os != null)
	{
	    try
	    {
		os.close();
	    }
	    catch(Exception ioe)
	    {
	    }
	}
    }

    public int getLength()
    {
	return os.size();
    }

    public byte[] getBytes()
    {
	return os.toByteArray();
    }

    public void reset()
    {
	os.reset();
    }

    public void writeBoolean(boolean data)
    {
	int res = data == true ? 1 : 0;
	os.write(res);
    }
    public void writeBooleanArray(boolean[] data)
    {
        int count = data.length;
        writeInt(count);
        for(int i = 0; i < count; ++i)
        {
            writeBoolean(data[i]);
        }

    }

    public void writeByte(byte data)
    {
	os.write(data);
    }
    public void writeByteArray(byte[] data)
    {
        int count = data.length;
        writeInt(count);
        for(int i = 0; i < count; ++i)
        {
            writeByte(data[i]);
        }

    }

    public void writeShort(short data)
    {
	os.write(data & 0xff);
	os.write(data >>> 8 & 0xff);
    }
    public void writeShortArray(short[] data)
    {
        int count = data.length;
        writeInt(count);
        for(int i = 0; i < count; ++i)
        {
            writeShort(data[i]);
        }
    }

    public void writeInt(int data)
    {
	os.write(data & 0xff);
	os.write(data >>> 8 & 0xff);
	os.write(data >>> 16 & 0xff);
	os.write(data >>> 24 & 0xff);
    }
    public void writeIntArray(int[] data)
    {
        int count = data.length;
        writeInt(count);
        for(int i = 0; i < count; ++i)
        {
            writeInt(data[i]);
        }
    }

    public void writeLong(long data)
    {
	os.write((int) (data & 0xff));
	os.write((int) (data >>> 8 & 0xff));
	os.write((int) (data >>> 16 & 0xff));
	os.write((int) (data >>> 24 & 0xff));
	os.write((int) (data >>> 32 & 0xff));
	os.write((int) (data >>> 40 & 0xff));
	os.write((int) (data >>> 48 & 0xff));
	os.write((int) (data >>> 56 & 0xff));
    }
    public void writeLongArray(long[] data)
    {
        int count = data.length;
        writeInt(count);
        for(int i = 0; i < count; ++i)
        {
            writeLong(data[i]);
        }
    }

    public void writeString(String string)
    {
	int len = string.length();
	char chars[] = new char[len];
	string.getChars(0, len, chars, 0);

	for(int i = 0; i < len; ++i)
	{
	    writeShort((short) chars[i]);    
	}

	writeShort((short) 0);
    }
    public void writeStringArray(String[] data)
    {
        int count = data.length;
        writeInt(count);
        for(int i = 0; i < count; ++i)
        {
            writeString(data[i]);
        }
    }

    
    
    public void writeAskquestionconfirm(AskQuestionConfirm askquestionconfirm)
    {   
        writeString(askquestionconfirm.adverturl);
        writeString(askquestionconfirm.advertimage);
        writeString(askquestionconfirm.askquestionid);
    }
    
    public void writeAskquestion(AskQuestion askquestion)
    {   
        writeInt(askquestion.id);
        writeString(askquestion.question);
        writeString(askquestion.dtcreated);
    }
    
    
    
    public void writeAskcomment(AskComment askcomment)
    {   
        writeInt(askcomment.id);
        writeInt(askcomment.askquestionid);
        writeString(askcomment.nickname);
        writeString(askcomment.text);
        writeString(askcomment.dtcreated);
    }
    
    public void writeAskresponse(AskResponse askresponse)
    {   
        writeInt(askresponse.askquestionid);
        writeString(askresponse.question);
        writeByteArray(askresponse.photobase64binary);
        writeIntArray(askresponse.responsevalues);
        writeInt(askresponse.average);
        writeInt(askresponse.responsetype);
        writeStringArray(askresponse.customresponses);
    }
    
    public void writeStringarray(StringArray stringarray)
    {   
    }
    
    
    
    public void writeDashboardvideo(DashboardVideo dashboardvideo)
    {   
        writeString(dashboardvideo.datetime);
        writeString(dashboardvideo.nickname);
        writeString(dashboardvideo.text);
        writeString(dashboardvideo.title);
    }
    
    public void writeDashboadmedia(DashboadMedia dashboadmedia)
    {   
        writeString(dashboadmedia.datetime);
        writeString(dashboadmedia.nickname);
        writeString(dashboadmedia.text);
        writeString(dashboadmedia.title);
    }
    
    
    
    public void writeDashboardphoto(DashboardPhoto dashboardphoto)
    {   
        writeString(dashboardphoto.datetime);
        writeString(dashboardphoto.nickname);
        writeString(dashboardphoto.text);
        writeString(dashboardphoto.title);
    }
    
    
    
    public void writeDashboardwallcomment(DashboardWallComment dashboardwallcomment)
    {   
        writeString(dashboardwallcomment.datetime);
        writeString(dashboardwallcomment.nickname1);
        writeString(dashboardwallcomment.nickname2);
        writeString(dashboardwallcomment.text);
    }
    
    
    
    public void writeDashboardnewfriend(DashboardNewFriend dashboardnewfriend)
    {   
        writeString(dashboardnewfriend.datetime);
        writeString(dashboardnewfriend.nickname1);
        writeString(dashboardnewfriend.nickname2);
    }
    
    public void writeTagupdate(TagUpdate tagupdate)
    {   
	writeStringArray(tagupdate.devicetagid);
        writeStringArray(tagupdate.tagvalidationstring);
        
    }
    
    
    
    
    
    public void writeTagconfirmation(TagConfirmation tagconfirmation)
    {   
        writeString(tagconfirmation.deviceid);
        writeBoolean(tagconfirmation.confirmedbyserver);
    }
    

}
