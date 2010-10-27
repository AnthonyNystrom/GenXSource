package service;

import java.io.*;

public class InputStream
{

    protected ByteArrayInputStream is = null;

    public InputStream(byte bytes[])
    {
	is = new ByteArrayInputStream(bytes);
    }

    public void close()
    {
	if(is != null)
	{
	    try
	    {
		is.close();
	    }
	    catch(Exception ioe)
	    {
	    }
	}
    }
    
    public boolean readBoolean()
    {
	byte res = (byte) is.read();
	return res == 0 ? false : true;
    }
    public boolean[] readBooleanArray()
    {
        int count = readInt();
	boolean[] booleans = new boolean[count];
	for(int i = 0; i < count; ++i)
	{
	    booleans[i] = readBoolean();
	}
	return booleans;
    }

    public byte readByte()
    {
	return (byte) is.read();
    }

    public byte[] readByteArray()
    {
        int count = readInt();
	byte[] bytes = new byte[count];
	for(int i = 0; i < count; ++i)
	{
	    bytes[i] = readByte();
	}
	return bytes;
    }

    public short readShort()
    {
	return (short) (is.read() | (is.read() << 8));
    }
    public short[] readShortArray()
    {
        int count = readInt();
	short[] shorts = new short[count];
	for(int i = 0; i < count; ++i)
	{
	    shorts[i] = readShort();
	}
	return shorts;
    }

    public int readInt()
    {
	return is.read() | (is.read() << 8) | (is.read() << 16) | (is.read() << 24);
    }

    public long readLong()
    {
	return ((long) is.read()) | ((long) is.read() << 8) | 
		((long) is.read() << 16) | ((long) is.read() << 24) | 
		((long) is.read() << 32) | ((long) is.read() << 40) | 
		((long) is.read() << 48) | ((long) is.read() << 56);
    }

    public String readString()
    {
	StringBuffer sb = new StringBuffer();

	short ch = -1;
	while(ch != 0)
	{
	    ch = readShort();

	    if(ch != 0)
	    {
		sb.append((char) ch);
	    }
	}
	return sb.toString();
    }

    public AskQuestionConfirm readAskquestionconfirm()
    {
        AskQuestionConfirm askquestionconfirm = new AskQuestionConfirm();
        
        
        askquestionconfirm.adverturl = readString();
        askquestionconfirm.advertimage = readString();
        askquestionconfirm.askquestionid = readString();

        return askquestionconfirm;
    }

    public AskQuestion readAskquestion()
    {
        AskQuestion askquestion = new AskQuestion();
        
        
        askquestion.id = readInt();
        askquestion.question = readString();
        askquestion.dtcreated = readString();

        return askquestion;
    }

    public int[] readIntArray()
    {
        int count = readInt();
        int[] ints = new int[count];
       
        for(int i = 0; i < count; ++i)
	{
	    ints[i] = readInt();
	}

        return ints;
    }

    public AskComment readAskcomment()
    {
        AskComment askcomment = new AskComment();
        
        
        askcomment.id = readInt();
        askcomment.askquestionid = readInt();
        askcomment.nickname = readString();
        askcomment.text = readString();
        askcomment.dtcreated = readString();

        return askcomment;
    }

    public AskResponse readAskresponse()
    {
        AskResponse askresponse = new AskResponse();
        
        
        askresponse.askquestionid = readInt();
        askresponse.question = readString();
        askresponse.photobase64binary = readByteArray();
        askresponse.responsevalues = readIntArray();
        askresponse.average = readInt();
        askresponse.responsetype = readInt();
        askresponse.customresponses = readStringArray();

        return askresponse;
    }

    public StringArray readStringarray()
    {
        StringArray stringarray = new StringArray();
        
        

        return stringarray;
    }

    public DashboardVideo[] readDashboardvideoArray()
    {
        int count = readInt();
        DashboardVideo[] dashboardvideos = new DashboardVideo[count];
       
        for(int i = 0; i < count; ++i)
	{
	    dashboardvideos[i] = readDashboardvideo();
	}

        return dashboardvideos;
    }

    public DashboardVideo readDashboardvideo()
    {
        DashboardVideo dashboardvideo = new DashboardVideo();
        
        
        dashboardvideo.datetime = readString();
        dashboardvideo.nickname = readString();
        dashboardvideo.text = readString();
        dashboardvideo.title = readString();

        return dashboardvideo;
    }

    public DashboadMedia readDashboadmedia()
    {
        DashboadMedia dashboadmedia = new DashboadMedia();
        
        
        dashboadmedia.datetime = readString();
        dashboadmedia.nickname = readString();
        dashboadmedia.text = readString();
        dashboadmedia.title = readString();

        return dashboadmedia;
    }

    public DashboardPhoto[] readDashboardphotoArray()
    {
        int count = readInt();
        DashboardPhoto[] dashboardphotos = new DashboardPhoto[count];
       
        for(int i = 0; i < count; ++i)
	{
	    dashboardphotos[i] = readDashboardphoto();
	}

        return dashboardphotos;
    }

    public DashboardPhoto readDashboardphoto()
    {
        DashboardPhoto dashboardphoto = new DashboardPhoto();
        
        
        dashboardphoto.datetime = readString();
        dashboardphoto.nickname = readString();
        dashboardphoto.text = readString();
        dashboardphoto.title = readString();

        return dashboardphoto;
    }

    public DashboardWallComment[] readDashboardwallcommentArray()
    {
        int count = readInt();
        DashboardWallComment[] dashboardwallcomments = new DashboardWallComment[count];
       
        for(int i = 0; i < count; ++i)
	{
	    dashboardwallcomments[i] = readDashboardwallcomment();
	}

        return dashboardwallcomments;
    }

    public DashboardWallComment readDashboardwallcomment()
    {
        DashboardWallComment dashboardwallcomment = new DashboardWallComment();
        
        
        dashboardwallcomment.datetime = readString();
        dashboardwallcomment.nickname1 = readString();
        dashboardwallcomment.nickname2 = readString();
        dashboardwallcomment.text = readString();

        return dashboardwallcomment;
    }

    public DashboardNewFriend[] readDashboardnewfriendArray()
    {
        int count = readInt();
        DashboardNewFriend[] dashboardnewfriends = new DashboardNewFriend[count];
       
        for(int i = 0; i < count; ++i)
	{
	    dashboardnewfriends[i] = readDashboardnewfriend();
	}

        return dashboardnewfriends;
    }

    public DashboardNewFriend readDashboardnewfriend()
    {
        DashboardNewFriend dashboardnewfriend = new DashboardNewFriend();
        
        
        dashboardnewfriend.datetime = readString();
        dashboardnewfriend.nickname1 = readString();
        dashboardnewfriend.nickname2 = readString();

        return dashboardnewfriend;
    }

    public TagUpdate readTagupdate()
    {
        TagUpdate tagupdate = new TagUpdate();
        
        tagupdate.devicetagid = readStringArray();
        tagupdate.tagvalidationstring = readStringArray();
        

        return tagupdate;
    }

    public String[] readStringArray()
    {
        int count = readInt();
        String[] strings = new String[count];
       
        for(int i = 0; i < count; ++i)
	{
	    strings[i] = readString();
	}

        return strings;
    }

    public TagConfirmation[] readTagconfirmationArray()
    {
        int count = readInt();
        TagConfirmation[] tagconfirmations = new TagConfirmation[count];
       
        for(int i = 0; i < count; ++i)
	{
	    tagconfirmations[i] = readTagconfirmation();
	}

        return tagconfirmations;
    }

    public TagConfirmation readTagconfirmation()
    {
        TagConfirmation tagconfirmation = new TagConfirmation();
        
        
        tagconfirmation.deviceid = readString();
        tagconfirmation.confirmedbyserver = readBoolean();

        return tagconfirmation;
    }


}
