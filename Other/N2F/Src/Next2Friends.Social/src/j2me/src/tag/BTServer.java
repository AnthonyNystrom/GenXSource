package tag;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.util.Enumeration;
import javax.bluetooth.*;
import java.util.Hashtable;
import java.util.Vector;
import javax.microedition.io.Connector;
import javax.microedition.io.StreamConnectionNotifier;
import javax.microedition.io.StreamConnection;
import service.TagConfirmation;
import service.TagService;

public class BTServer implements Runnable
{
    private Thread thread;

    public BTServer()
    {
	thread = new Thread(this);
	thread.start();
    }

    public void run()
    {
	while(true)
	{
	    if(BTLocator.isEnabled)
	    {
		String url = "btspp://localhost:" + App.Const.BT_UUID;
		try
		{
		    TagStorage storage = new TagStorage();

                    App.Core.message = "BT server started";
                    //System.out.println("BT server started");
		    StreamConnectionNotifier connectionNotifier = (StreamConnectionNotifier) Connector.open(url);
		    StreamConnection connection = (StreamConnection) connectionNotifier.acceptAndOpen();
                    
                    //System.out.println("BT server accepted");
                    App.Core.message = "BT server accepted";

		    DataInputStream is = connection.openDataInputStream();
		    DataOutputStream os = connection.openDataOutputStream();

                    App.Core.message = "0";
		    //1
		    String clientTagId = is.readUTF();
                    App.Core.message = "1";

		    String encryptedVal = App.Core.encryptor.encrypt(App.Core.encryptor.makeValidationString(clientTagId));
		    //2
		    os.writeUTF(encryptedVal);
		    os.writeUTF(App.Core.storage.tagId);
                    os.flush();
                    App.Core.message = "2";

		    //3
		    String tagValidationString = is.readUTF();
                    App.Core.message = "3";

		    storage.add(tagValidationString, clientTagId);
		    TagService service = new TagService();

		    try
		    {
			TagConfirmation[] confirmations = service.UploadTags(App.Core.storage.login, App.Core.storage.password, storage.getTagUpdate());
		    }
		    catch(Exception ex)
		    {
			ex.printStackTrace();
		    }

		    connection.close();
		    connectionNotifier.close();
		}
		catch(Exception ex)
		{
                    App.Core.message = "BT server ex" + ex.toString();
		}
	    }
	    else
	    {
		try
		{
		    Thread.sleep(5000);
		}
		catch(Exception ex)
		{
		    
		}
	    }
	}
    }
}
