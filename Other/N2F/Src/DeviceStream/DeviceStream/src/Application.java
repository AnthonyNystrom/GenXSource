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
	private boolean CanBroadcast = true;
        private String ThumbnailSavePath = "";
        private boolean IsDevice = false;
        private boolean PerformSave = false;
	 
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

		try
		{
			Red5ConnectionString Red5Conn = Red5ConnectionString.parseConnectionString((String)params[0]);
	
			if(Red5Conn!=null){
				
                        NickName = Red5Conn.getNickname();
                        Password = Red5Conn.getPassword();
                        StreamName = Red5Conn.getStreamName();
                        Title = Red5Conn.getTitle();
                        PrivacyFlag = Red5Conn.getIsPrivate();

                        VideSavePath = NickName + "\\video\\" + "\\" + StreamName;

                        // authenticate here
                        CanBroadcast = true;
                        
                        if(Password.compareTo("save")==0)
                        {
                            PerformSave = true;   
                            
                            System.out.println("*************");
                            System.out.println("Save stream");
                            System.out.println("*************");
                        }
                        else
                        {
                            PerformSave = false;
                            
                            System.out.println("*************");
                            System.out.println("No Save stream");
                            System.out.println("*************");
                        }
                        
                        System.out.println("*************");
                        System.out.println("Broadcast ready to go:"+NickName);
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
    	
        /** {@inheritDoc} */
    	if(CanBroadcast){

                // return the nickname and video directory and append the stream name (from param)
                String FileName = RootSavePath + VideSavePath;

                if(PerformSave){
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
                }

                super.streamBroadcastStart(stream);
    	}
    }

    
    @Override
    public void streamBroadcastClose(IBroadcastStream stream){

            System.out.println("*************");
            System.out.println("StreamBroadcastClose Called");
            System.out.println("*************");

            super.streamBroadcastClose(stream);
		
    }
    
  
   

	/** {@inheritDoc} */
        @Override
	public void appDisconnect(IConnection conn) {

		if (appScope == conn.getScope() && serverStream != null) {

			serverStream.close();
		}

		super.appDisconnect(conn);
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
}


