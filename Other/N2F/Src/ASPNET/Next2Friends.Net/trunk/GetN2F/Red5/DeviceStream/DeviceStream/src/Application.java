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
//1. Nickname - the memberiD of the user (main user config screen)
//2. Password - the password of the user (main user config screen)
//4. StreamName - DateTimeNow as a string (handled behind scenes)
//4. Title - defaults to DateTime Now but can be edited by the user to be whatever they wish. MAX 100 Chars (editable/optional when live record has been selected) 
//5. isPrivate
public class Application extends ApplicationAdapter {

	private String ConnectionString = "jdbc:sqlserver://192.168.3.4;DatabaseName=Next2Friends";
	private String DBLogin = "N2FDBLogin8745";
	private String DBPassword = "59c42xMJH03t3fl83dk";
	private IScope appScope;
	private IServerStream serverStream;	
	private String NickName = "";
	private String Password = "";
	private String StreamName = "";
	private String Title = "";
        private Integer PrivacyFlag = 2;
	private String RootSavePath = "";
	private String VideSavePath = "";
	private boolean CanBroadcast = false;
        private String ThumbnailSavePath = "";
        private boolean IsDevice = false;
	 
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
			Red5ConnectionString Red5Conn = Red5ConnectionString.parseConnectionString((String)params[0]);
	
			if(Red5Conn!=null){
				
				NickName = Red5Conn.getNickname();
				Password = Red5Conn.getPassword();
				StreamName = Red5Conn.getStreamName();
				Title = Red5Conn.getTitle();
                                PrivacyFlag = Red5Conn.getIsPrivate();
				
				System.out.println("*************");
				System.out.println("Ready to start Live Stream width parameters:");
				System.out.println("NickName:" + NickName);
				System.out.println("Password:" + Password);
				System.out.println("StreamName:" + StreamName);
				System.out.println("Title:" + Title);
                                System.out.println("Privacy:" + PrivacyFlag);
				System.out.println("*************");
				
				
				System.out.println("*************");
				System.out.println("SUCCESSFULLY parsed : STRING=("+(String)params[0] + " )");
				System.out.println("*************");
                                
                                // authenticate here
				CanBroadcast = AuthenticateLogin();
                                
                                System.out.println("*************");
				System.out.println("Authenticated = "+CanBroadcast);
				System.out.println("*************");
                                
                                if(CanBroadcast){
                                    IsDevice = true;
                                }
                                
                                
				
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
                
                //if(CanBroadcast){   
                    //super.appConnect(conn, params);
                   // return true;
                //}else{
                //    return false;
                //}
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
			System.out.println("Registered live stream");
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
		System.out.println("StreamBroadcastClose Called");
		System.out.println("*************");

		//if( IsDevice == true){
                //    //finally call the database to end the Live broadcast
                //    EndLiveBroadcast();
                //}

         
                super.streamBroadcastClose(stream);
		
    }
    
       
    public boolean StartLiveBroadcast(){

        try {
     	   
 	   Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
 	   Connection conn = DriverManager.getConnection(ConnectionString, DBLogin,DBPassword ); 

 	   //String sql = "select 'v' as 'VideoPath', 't' as 'ThumbnailPath'" ;
     	   String sql = "HG_StartLiveStream @WebLiveBroadcastID='" + StreamName + "', @Email = '" +NickName + "',@Password = '"+Password+"', @Title = '" + Title+ "',@Description = '',@privacyFlag = "+PrivacyFlag;
     	   
     	   Statement stmt = conn.createStatement();
     	   
     	   ResultSet results =  stmt.executeQuery(sql);
     	   
     	   if(results.next()){
     		   
     		  VideSavePath = results.getString("VideoPath");
     		  ThumbnailSavePath = results.getString("ThumbnailPath"); 
     		  
     		  VideSavePath += StreamName;
     		  ThumbnailSavePath += StreamName + ".jpg";
     		  
     	   }else{
     		   
        	   	System.out.print("Couldnt return path from DB when regiserting video stream");
        	   	return false;
     	   }

     	   stmt.close();
     	   conn.close();
     	   
     	   
     	  System.out.println("*************");
     	  System.out.println("Create entry in DB");
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

           String sql = "HG_EndLiveStream @WebLiveBroadcastID='" + StreamName + "', @Nickname = " +NickName + ", @Title = '" + Title+ "',@Description = ''";

           Statement stmt = conn.createStatement();

           stmt.execute(sql);

           stmt.close();
           conn.close();

          System.out.println("*************");
          System.out.println("Closed entry in  DB: IsDevice = "+IsDevice);
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
    
        public boolean AuthenticateLogin(){

        int Valid = 0;
          
        try {

           Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
           Connection conn = DriverManager.getConnection(ConnectionString, DBLogin,DBPassword ); 

           String sql = "HG_RED5Login @Nickname = " +NickName + ", @Password = '" + Password+ "'";

           Statement stmt = conn.createStatement();
     	   
     	   ResultSet results =  stmt.executeQuery(sql);

     	   if(results.next()){
     		  Valid = results.getInt("Valid");
                  System.out.println("Successfully read DB Auth for '"+NickName + "' with password '"+Password+"' and valid = "+Valid);
     	   }else{
        	  System.out.println("Couldnt read DB Auth for "+NickName);
        	  return false;
     	   }

     	   stmt.close();
     	   conn.close();
    	  
        }
        catch (Exception e) 
        {
        	//return false;
        	System.out.println("*************");
     	   	System.out.println("DB ERROR: " + e.getMessage());
     	        System.out.println("*************");
     	   	return false;
        }


        if(Valid==1){
            return true;
        }else{
            return false;
        }
 }
   

	/** {@inheritDoc} */
        @Override
	public void appDisconnect(IConnection conn) {

		if (appScope == conn.getScope() && serverStream != null) {

			serverStream.close();
		}

                //if( IsDevice == true){
                //    EndLiveBroadcast();
                //}

                System.out.println("*************");
		System.out.println("appDisconnect Called");
		System.out.println("*************");
		
		super.appDisconnect(conn);
	}
}


