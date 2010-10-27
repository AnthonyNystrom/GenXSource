/* ------------------------------------------------
 * MainForm.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import genetibase.microedition.componentmodel.Resources;
import genetibase.util.Base64;
import genetibase.util.GrfxUtils;
import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.ImageItem;
import javax.microedition.lcdui.StringItem;
import javax.microedition.midlet.MIDletStateChangeException;
import n2f.proxmark.wireless.DeviceDiscoverer;
import n2f.proxmark.wireless.IDeviceDiscovererListener;

/**
 * @author Alex Nesterov
 */
final class MainForm
	extends Form
	implements IDeviceDiscovererListener
{
    private class MainFormCommandListener
	    implements CommandListener
    {
	public void commandAction(Command cmd, Displayable displayable)
	{
	    if (_exitCommand == cmd)
	    {
		try
		{
		    if (_deviceDiscoverer != null)
		    {
			_deviceDiscoverer.free();
			_deviceDiscoverer = null;
		    }

		    Program.getCurrentApp().destroyApp(false);
		}
		catch (MIDletStateChangeException e)
		{
		    e.printStackTrace();
		}
	    }
	}

    }

    private Command _exitCommand;
    private DeviceDiscoverer _deviceDiscoverer;

    public MainForm(String title)
    {
	super(title);

	Resources res = Program.getResources();
	_exitCommand = new Command(res.get("ExitText"), Command.EXIT, 0);
	addCommand(_exitCommand);
	setCommandListener(new MainFormCommandListener());
	_deviceDiscoverer = new DeviceDiscoverer(this);
	_deviceDiscoverer.setVisible(true);
    }

    public void advertReceived(String advertText, String imageBase64String)
    {
	System.out.println("DeviceDiscoverer::advertReceived");
	Program.getDisplay().vibrate(3000);
	deleteAll();
	
	if (advertText != null && !advertText.equals(""))
	{
	    StringItem stringItem = new StringItem("", advertText);
	    append(stringItem);
	}
	
	if (imageBase64String != null && !imageBase64String.equals(""))
	{
	    byte[] imageBytes = Base64.decode(imageBase64String);
	    Image advertImage = GrfxUtils.createImage(imageBytes);
	    StringItem imageLengthString = new StringItem("Image Length: ", String.valueOf(imageBytes.length));
	    append(imageLengthString);
	    ImageItem imageItem = new ImageItem("",
						advertImage,
						ImageItem.LAYOUT_CENTER,
						"");
	    append(imageItem);
	}
    }

    public void cannotStartTaggingService()
    {
	Resources res = Program.getResources();
	Alert alert = new Alert(res.get("ProductName"),
				res.get("CannnotInitializeBluetooth"), null,
				AlertType.ERROR);
	Program.getDisplay().setCurrent(alert, MainForm.this);
    }

}
