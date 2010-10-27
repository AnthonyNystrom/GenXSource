package DeviceStream;

import org.red5.server.adapter.ApplicationAdapter;
import org.red5.server.api.IBandwidthConfigure;
import org.red5.server.api.IConnection;
import org.red5.server.api.IScope;
import org.red5.server.api.stream.IServerStream;
import org.red5.server.api.stream.IStreamCapableConnection;
import org.red5.server.api.stream.support.SimpleConnectionBWConfig;
import java.io.*;
import org.red5.server.api.stream.*;
import java.sql.*;
//1. MemberID - the memberiD of the user (main user config screen)
//2. Password - the password of the user (main user config screen)
//4. StreamName - DateTimeNow as a string (handled behind scenes)
//4. Name - defaults to DateTime Now but can be edited by the user to be whatever they wish. MAX 100 Chars (editable/optional when live record has been selected) 

public class Application extends ApplicationAdapter {

	private String ConnectionString = "jdbc:sqlserver://69.21.114.99;DatabaseName=Next2Friends";
	private String DBLogin = "N2FDBLogin8745";
	private String DBPassword = "59c42xMJH03t3fl83dk";
	private IScope appScope;
	private IServerStream serverStream;	
	private String MemberID = "";
	private String Password = "";
	private String StreamName = "";
	private String Title = "";
	private String RootSavePath = "Y:\\";
	private String VideSavePath = "";
	private String ThumbnailSavePath = "";
	private boolean CanBroadcast = false;
	 
	/** {@inheritDoc} */
    @Override
	public boolean appStart(IScope app) {
		appScope = app;
		return true;
	}

	/** {@inheritDoc} */
    @Override
	public boolean appConnect(IConnection conn, Object[] params) {

		// Trigger calling of "onBWDone", required for some FLV players

		measureBandwidth(conn);

		if (conn instanceof IStreamCapableConnection) {

			IStreamCapableConnection streamConn = (IStreamCapableConnection) conn;
			SimpleConnectionBWConfig bwConfig = new SimpleConnectionBWConfig();
			bwConfig.getChannelBandwidth()[IBandwidthConfigure.OVERALL_CHANNEL] = 1024 * 1024;
			bwConfig.getChannelInitialBurst()[IBandwidthConfigure.OVERALL_CHANNEL] =	128 * 1024;
			streamConn.setBandwidthConfigure(bwConfig);
		}
		
		System.out.println("*************");
		System.out.println("About to start Live Stream");
		System.out.println("*************");
			

		try
		{
			Bucket bucket = Bucket.parseMemberID((String)params[0]);
	
			if(bucket!=null){
				
				MemberID = bucket.getMemberID();
				Password = bucket.getPassword();
				StreamName = bucket.getStreamName();
				Title = bucket.getName();
				
				System.out.println("*************");
				System.out.println("Ready to start Live Stream width parameters:");
				System.out.println("MemberID:" + MemberID);
				System.out.println("Password:" + Password);
				System.out.println("StreamName:" + StreamName);
				System.out.println("Title:" + Title);
				System.out.println("*************");
				
				// authenticate here
				CanBroadcast = true;
				
				System.out.println("*************");
				System.out.println("SUCCESSFULLY parsed : STRING=("+(String)params[0] + " )");
				System.out.println("*************");
				
	    	}
		}
		catch(NullPointerException ex)
		{
			CanBroadcast = false;
			
			System.out.println("*************");
			System.out.println("Failed to parse : STRING=("+(String)params[0] + " )");
			System.out.println("*************");
		}
		catch(IllegalArgumentException ex)
		{
			CanBroadcast = false;
			
			System.out.println("*************");
			System.out.println("Failed to parse : STRING=("+(String)params[0] + " )");
			System.out.println("*************");
		}
		

    	return super.appConnect(conn, params);
	} 
    
    
    @Override
    public void streamBroadcastStart(IBroadcastStream stream)
    {
    	
   
    	if(CanBroadcast){
			System.out.println("*************");
			System.out.println("Live Stream Started");
			System.out.println("*************");
			
			// create a live stream and video entry in the db
			StartLiveBroadcast();
			System.out.println("*************");
			System.out.println("Resgistered live stream");
			System.out.println("*************");
			
			
			// return the nickname and video directory and append the stream name (from param)
			String FileName = RootSavePath + VideSavePath;
			
			try
			{
				
				stream.saveAs(FileName, false);			
				System.out.println("*************");
				System.out.println("FILE SAVED TO: "+ FileName);
				System.out.println("*************");
			
			}catch(ResourceExistException ex){}
			catch(IOException ex)
			{
				System.out.println("*************");
				System.out.println("SAVE ERROR: "+ FileName + " : " + ex.getMessage());
				System.out.println("*************");
				
			}
			catch(ResourceNotFoundException ex){}
			
			super.streamBroadcastStart(stream);
    	}
    }

    
    @Override
    public void streamBroadcastClose(IBroadcastStream stream){

		System.out.println("*************");
		System.out.println("Live Stream Ended");
		System.out.println("*************");

		
		//finally call the database to end the Live broadcast
		EndLiveBroadcast();

		super.streamBroadcastClose(stream);
		
    }
    
       
    public boolean StartLiveBroadcast(){
	      
    	
    	
        try {
     	   
 	       Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
 	       Connection conn = DriverManager.getConnection(ConnectionString, DBLogin,DBPassword ); 

 	       //String sql = "select 'v' as 'VideoPath', 't' as 'ThumbnailPath'" ;
     	   String sql = "HG_StartLiveStream @WebLiveBroadcastID='" + StreamName + "', @Email = '" +MemberID + "',@Password = '"+Password+"', @Title = '" + Title+ "',@Description = ''";
     	   
     	   Statement stmt = conn.createStatement();
     	   
     	   ResultSet results =  stmt.executeQuery(sql);
     	   
     	   if(results.next()){
     		   
     		  VideSavePath = results.getString("VideoPath");
     		  ThumbnailSavePath = results.getString("ThumbnailPath"); 
     		  
     		  VideSavePath += StreamName + ".flv";
     		  ThumbnailSavePath += StreamName + ".jpg";
     		  
     	   }else{
     		   
        	   	System.out.print("Couldnt return path from DB when regiserting video stream");
        	   	return false;
     	   }

     	   stmt.close();
     	   conn.close();
     	   
     	   
     	  System.out.println("*************");
     	  System.out.println("Updated DB");
     	  System.out.println("*************");
     	  
        }
        catch (Exception e) 
        {
        	//return false;
        	System.out.println("*************");
     	   	System.out.println("DB ERROR: " + e.getMessage());
     	    System.out.println("*************");
     	   	return false;
        }
        
        return true;
 }
    
    
    public boolean EndLiveBroadcast(){
	       
        try {
     	   
 	       Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
 	       Connection conn = DriverManager.getConnection(ConnectionString, DBLogin,DBPassword ); 

     	   String sql = "HG_EndLiveStream @WebLiveBroadcastID='" + StreamName + "', @MemberID = " +MemberID + ", @Title = '" + Title+ "',@Description = ''";
     	   
     	   Statement stmt = conn.createStatement();
     	   
     	   stmt.execute(sql);
     	   
     	   stmt.close();
     	   conn.close();
     	   
     	  System.out.println("*************");
     	  System.out.println("Updated DB");
     	  System.out.println("*************");
     	  
        }
        catch (Exception e) 
        {
        	//return false;
        	System.out.println("*************");
     	   	System.out.println("DB ERROR ON MESSAGE END: " + e.getMessage());
     	    System.out.println("*************");
     	   	return false;
        }
        
        return true;
 }
   
 
        
	/** {@inheritDoc} */
    @Override
	public void appDisconnect(IConnection conn) {

		if (appScope == conn.getScope() && serverStream != null) {

			serverStream.close();
		}
		
		
		//EndLiveBroadcast();
		
		super.appDisconnect(conn);
	}
}


