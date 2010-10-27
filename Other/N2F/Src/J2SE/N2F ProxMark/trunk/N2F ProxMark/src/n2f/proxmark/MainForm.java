/* ------------------------------------------------
 * MainForm.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import genetibase.util.Environment;
import java.awt.BorderLayout;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.lang.Thread.UncaughtExceptionHandler;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.bluetooth.ServiceRecord;
import javax.swing.AbstractButton;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import javax.swing.JSplitPane;
import javax.swing.JToggleButton;
import javax.swing.JToolBar;
import n2f.proxmark.resources.ApplicationResources;
import n2f.proxmark.wireless.IDeviceDiscovererListener;

/**
 * @author Alex Nesterov
 */
final class MainForm
	extends JFrame
{
    private static final Dimension _thumbnailMaximumSize =
	    new Dimension(320, 240);
    private static final Logger _logger =
	    Logger.getLogger(MainForm.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private class Toolbar
	    extends JToolBar
	    implements ActionListener
    {
	public Toolbar()
	{
	    add(_newButton);
	    add(_deleteButton);
	    add(_publishButton);

	    for (Component component : getComponents())
	    {
		if (component instanceof AbstractButton)
		{
		    AbstractButton button = (AbstractButton)component;
		    button.addActionListener(this);
		    button.setRolloverEnabled(true);
		}
	    }
	}

	public void actionPerformed(ActionEvent e)
	{
	    Object source = e.getSource();

	    if (source == _deleteButton)
	    {
		_advertManager.deleteAdvert(_advertManager.getCurrentAdvert());
	    }
	    else if (source == _newButton)
	    {
		_advertManager.createAdvert();
	    }
	    else if (source == _publishButton)
	    {
		_publishModel.setIsPublishing(_publishButton.isSelected());
	    }
	}

    }

    private class ApplicationUncaughtExceptionHandler
	    implements UncaughtExceptionHandler
    {
	public void uncaughtException(Thread t, Throwable e)
	{
	    if (e instanceof ThreadDeath)
		return;

	    _logger.log(Level.SEVERE, e.getMessage(), e);

	    if (e instanceof Exception)
		showAlertDialog(String.format(ApplicationResources.UnknownError,
					      Environment.NewLine,
					      e.getMessage()));
	}

    }

    private class AdListPropertyChangeListener
	    implements PropertyChangeListener
    {
	public void propertyChange(PropertyChangeEvent e)
	{
	    Advert model = (Advert)e.getNewValue();
	    _advertManager.setCurrentAdvert(model);
	}

    }

    private class AdManagerAdListListener
	    implements IAdvertListListener
    {
	public void advertListChanged(AdvertListEvent e)
	{
	    _advertView.setEnabled(_advertManager.getCurrentAdvert() != null);

	    switch (e.getEventType())
	    {
		case Created:
		{
		    Advert advert = e.getAdvert();
		    _advertView.setModel(advert);
		    _publishModel.addAdvert(advert);
		    break;
		}
		case Deleted:
		{
		    _publishModel.removeAdvert(e.getAdvert());
		    break;
		}
	    }
	}

    }

    private class AdManagerPropertyChangeListener
	    implements PropertyChangeListener
    {
	public void propertyChange(PropertyChangeEvent e)
	{
	    String propertyName = e.getPropertyName();

	    if (AdvertManager.CAN_CREATE_ADVERT_CHANGED.equals(propertyName))
	    {
		_newButton.setEnabled(((Boolean)e.getNewValue()).booleanValue());
	    }
	    else if (AdvertManager.CAN_DELETE_ADVERT_CHANGED.equals(propertyName))
	    {
		_deleteButton.setEnabled(((Boolean)e.getNewValue()).booleanValue());
	    }
	    else if (AdvertManager.CURRENT_ADVERT_CHANGED.equals(propertyName))
	    {
		_advertView.setModel((Advert)e.getNewValue());
	    }
	}

    }

    private class AdPublisherPropertyChangeListener
	    implements PropertyChangeListener
    {
	public void propertyChange(PropertyChangeEvent e)
	{
	    String propertyName = e.getPropertyName();

	    if (AdvertPublishModel.CAN_PUBLISH_CHANGED.equals(propertyName))
	    {
		_publishButton.setEnabled(((Boolean)e.getNewValue()).booleanValue());
	    }
	}

    }

    private class DeviceDiscovererListener
	    implements IDeviceDiscovererListener
    {
	public void cannotInitializeBluetooth()
	{
	    showAlertDialog(ApplicationResources.CannotInitializeBluetooth);
	    resetPublisher();
	}

	public void cannotStartDeviceSearch()
	{
	    showAlertDialog(ApplicationResources.CannotStartDeviceSearch);
	    resetPublisher();
	}

	private void resetPublisher()
	{
	    if (_publishButton.isSelected())
		_publishButton.doClick();
	}

	public void tagDevice(ServiceRecord serviceRecord)
	{
	    _publishModel.tagDevice(serviceRecord);
	}

    }

    private AdvertListView _advertListView;
    private AdvertManager _advertManager;
    private AdvertView _advertView;
    private JButton _deleteButton;
    private JButton _newButton;
    private JToggleButton _publishButton;
    private JSplitPane _splitter;
    private AdvertPublishModel _publishModel;
    private Toolbar _toolbar;

    /**
     * Creates a new instance of the <tt>MainForm</tt> class.
     */
    public MainForm()
    {
	Thread.setDefaultUncaughtExceptionHandler(new ApplicationUncaughtExceptionHandler());
	initializeComponent();
    }

    private void initializeComponent()
    {
	setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	setLayout(new BorderLayout());
	setSize(new Dimension(800, 600));
	setTitle(ApplicationResources.ProductName);

	_advertListView = new AdvertListView();
	_advertListView.addPropertyChangeListener(new AdListPropertyChangeListener());

	_advertView = new AdvertView();
	_advertView.setEnabled(false);

	_publishModel = new AdvertPublishModel(new DeviceDiscovererListener(),
					       _thumbnailMaximumSize);
	_publishModel.addPropertyChangeListener(
		new AdPublisherPropertyChangeListener());

	_advertManager = new AdvertManager(_thumbnailMaximumSize);
	_advertManager.addAdvertListListener(new AdManagerAdListListener());
	_advertManager.addAdvertListListener(_advertListView);
	_advertManager.addPropertyChangeListener(new AdManagerPropertyChangeListener());

	_splitter = new JSplitPane();
	_splitter.setDividerLocation(getWidth() / 4);
	_splitter.setOneTouchExpandable(true);
	_splitter.setLeftComponent(_advertListView);
	_splitter.setRightComponent(_advertView);

	_deleteButton = new JButton(ApplicationResources.Delete_Text);
	_deleteButton.setDisabledIcon(new ImageIcon(ApplicationResources.Delete_Disabled));
	_deleteButton.setEnabled(false);
	_deleteButton.setIcon(new ImageIcon(ApplicationResources.Delete_Normal));
	_deleteButton.setRolloverIcon(new ImageIcon(ApplicationResources.Delete_Hot));
	_deleteButton.setToolTipText(ApplicationResources.Delete_Tooltip);

	_newButton = new JButton(ApplicationResources.New_Text);
	_newButton.setDisabledIcon(new ImageIcon(ApplicationResources.New_Disabled));
	_newButton.setEnabled(false);
	_newButton.setIcon(new ImageIcon(ApplicationResources.New_Normal));
	_newButton.setRolloverIcon(new ImageIcon(ApplicationResources.New_Hot));
	_newButton.setToolTipText(ApplicationResources.New_Tooltip);

	_publishButton = new JToggleButton(ApplicationResources.Publish_Text);
	_publishButton.setDisabledIcon(new ImageIcon(ApplicationResources.Publish_Disabled));
	_publishButton.setEnabled(false);
	_publishButton.setIcon(new ImageIcon(ApplicationResources.Publish_Normal));
	_publishButton.setRolloverIcon(new ImageIcon(ApplicationResources.Publish_Hot));
	_publishButton.setToolTipText(ApplicationResources.Publish_Tooltip);

	_toolbar = new Toolbar();

	add(_toolbar, BorderLayout.NORTH);
	add(_splitter, BorderLayout.CENTER);

	_deleteButton.setEnabled(_advertManager.canDeleteAdvert());
	_newButton.setEnabled(_advertManager.canCreateAdvert());

	_advertManager.createAdvert();

	addWindowListener(
		new WindowAdapter()
		{
		    @Override
		    public void windowClosing(WindowEvent e)
		    {
			_publishModel.destroy();
		    }

		});
    }

    private void showAlertDialog(String message)
    {
	JOptionPane.showMessageDialog(this,
				      message,
				      ApplicationResources.ProductName,
				      JOptionPane.ERROR_MESSAGE);
    }

}
