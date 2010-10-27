package n2f.sms;

import java.io.IOException;

import javax.microedition.io.Connector;
import javax.wireless.messaging.MessageConnection;
import javax.wireless.messaging.TextMessage;

import n2f.tag.debug.Debug;

public class SMSSender {

	/**
	 * 
	 * @param message
	 * @param targetNumber
	 */
	public static void sendMessage(String message, String targetNumber){
		MessageConnection conn = null;
		try {
Debug.println("send to number:"+targetNumber);
//Debug.println("message:"+message);
			conn = (MessageConnection)Connector.open("sms://"+targetNumber);
			
			if (conn != null) {
				TextMessage msg = null;
				msg = (TextMessage)conn.newMessage(MessageConnection.TEXT_MESSAGE);
				msg.setPayloadText(message);
				conn.send(msg);
Debug.println("SENT SMS successfully");
			}
		} catch (IOException e) {
			e.printStackTrace();
Debug.println("send exception:"+e.getMessage());
		} finally {
			close(conn);
			conn = null;
		}
	}
	
	public static void close(MessageConnection c){
		if (c != null)
			try {
				c.close();
			} catch (IOException e) {
				e.printStackTrace();
			} finally {
				c = null;
			}
	}

}
