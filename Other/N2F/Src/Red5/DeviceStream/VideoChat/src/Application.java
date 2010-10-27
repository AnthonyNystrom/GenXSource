package VideoChat;

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
	private boolean CanBroadcast = true;

	 
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
}


