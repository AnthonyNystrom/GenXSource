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

public class BTLocator implements DiscoveryListener, Runnable
{

    public static boolean isEnabled;
    public long lastUpdate;
    public long sleepTime;
    private Vector devices;
    private LocalDevice device;
    private DiscoveryAgent agent;
    private UUID[] uuid;
    private Thread thread;
    private Vector urls;
    private boolean isRunning;
    private TagStorage tagStorage;

    public BTLocator()
    {
        isEnabled = false;
        UUID id = new UUID(App.Const.BT_UUID, false);
        urls = new Vector();
        uuid = new UUID[]
                {
                    id
                };
        devices = new Vector();

        try
        {
            thread = new Thread(this);
            thread.start();
        } catch (Exception ex)
        {
            ex.printStackTrace();
        }
    }

    public void run()
    {
        while (true)
        {
            if (isEnabled)
            {
                devices.removeAllElements();
                urls.removeAllElements();
                try
                {
                    device = LocalDevice.getLocalDevice();
                    agent = device.getDiscoveryAgent();
                    device.setDiscoverable(DiscoveryAgent.GIAC);
                } catch (Exception ex)
                {
                    ex.printStackTrace();
                    try
                    {
                        Thread.sleep(5000);
                    } catch (Exception exx)
                    {
                        exx.printStackTrace();
                    }
                    continue;
                }

                tagStorage = new TagStorage();
                search();
                while (isRunning)
                {
                    try
                    {
                        Thread.sleep(1000);
                    } catch (Exception ex)
                    {
                        ex.printStackTrace();
                    }
                }

                lastUpdate = System.currentTimeMillis();
                App.Core.storage.readWriteSettings(false);

                if (tagStorage.tagsCount > 0)
                {
                    //App.Core.message = "completed" + tagStorage.tagsCount;
                    TagService service = new TagService();
                    try
                    {
                        TagConfirmation[] confirmations = service.UploadTags(App.Core.storage.login, App.Core.storage.password, tagStorage.getTagUpdate());
                        //System.out.println("send tag:" + confirmations[0].confirmedbyserver);
                        App.Core.message = "send tag:" + confirmations[0].confirmedbyserver;
                    } catch (Exception ex)
                    {
                        ex.printStackTrace();
                    }
                }
                tagStorage = null;
            } else
            {
                try
                {
                    device.setDiscoverable(DiscoveryAgent.NOT_DISCOVERABLE);
                } catch (Exception ex)
                {
                }

            }
            try
            {
                Thread.sleep(App.Const.TAG_WAITTIME);
            } catch (Exception ex)
            {
                ex.printStackTrace();
            }
        }
    }

    public void search()
    {
        isRunning = true;
        try
        {
            App.Core.message = "start search";
            boolean res = agent.startInquiry(DiscoveryAgent.GIAC, this);
        } catch (Exception ex)
        {
            ex.printStackTrace();
        }
    }

    public void deviceDiscovered(RemoteDevice btDevice, DeviceClass cod)
    {
        App.Core.message = "device discovered";
        devices.addElement(btDevice);
    }

    public void inquiryCompleted(int discType)
    {
        App.Core.message = "inquiryCompleted";

        boolean willWork = false;
        int size = devices.size();
        for (int i = 0; i < size; ++i)
        {
            App.Core.message = "inquiryCompleted: hasMoreElements";
            RemoteDevice remoteDevice = (RemoteDevice) devices.elementAt(i);
            willWork = true;
            try
            {
                App.Core.message = "start searchServices";
                agent.searchServices(null, uuid, remoteDevice, this);
            } catch (Exception ex)
            {
                ex.printStackTrace();
            }
        }
        if (!willWork)
        {
            isRunning = false;
        }
    }

    public void serviceSearchCompleted(int transID, int respCode)
    {
        //System.out.println("BT found services:" + urls.size());
        App.Core.message = "BT found services:" + urls.size();
        int size = urls.size();
        for (int i = 0; i < size; ++i)
        {
            StreamConnection conn = null;
            DataOutputStream os = null;
            DataInputStream is = null;
            try
            {
                //System.out.println("BT locator connecting");
                App.Core.message = "BT locator connecting";
                conn = (StreamConnection) Connector.open((String) urls.elementAt(i), Connector.READ_WRITE, true);
                //System.out.println("BT locator connected");
                App.Core.message = "BT locator connected";
                os = conn.openDataOutputStream();
                is = conn.openDataInputStream();

                App.Core.message = "00";
                //1
                os.writeUTF(App.Core.storage.tagId);
                os.flush();
                App.Core.message = "11";

                //2
                String validationString = is.readUTF();
                String remoteTagId = is.readUTF();
                tagStorage.add(validationString, remoteTagId);
                App.Core.message = "22";

                String encryptedVal = App.Core.encryptor.encrypt(App.Core.encryptor.makeValidationString(remoteTagId));
                //String encryptedVal = App.Core.encryptor.encrypt("610d3ada-415e-4764-bec7-8544c2633511968035620000");
                //3
                os.writeUTF(encryptedVal);
                os.flush();
                App.Core.message = "33";

            } catch (Exception ex)
            {
                //System.out.println("BT locator exception");
                App.Core.message = "BT locator exception";
                ex.printStackTrace();
            } finally
            {
                try
                {
                    is.close();
                    os.close();
                    conn.close();
                } catch (Exception ex)
                {
                }
            }
        }
        isRunning = false;
    }

    public void servicesDiscovered(int transID, ServiceRecord[] servRecord)
    {
        App.Core.message = "BT found devices:" + servRecord.length;
        //System.out.println("BT found devices:" + servRecord.length);
        for (int i = 0; i < servRecord.length; ++i)
        {
            String url = servRecord[i].getConnectionURL(ServiceRecord.NOAUTHENTICATE_NOENCRYPT, false);
            urls.addElement(url);
        }
    }
}
