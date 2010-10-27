/* ------------------------------------------------
 * AdvertView.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import genetibase.drawing.GrfxUtils;
import genetibase.util.Argument;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.io.File;
import java.util.concurrent.ExecutionException;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.swing.*;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.filechooser.FileFilter;
import n2f.proxmark.resources.ApplicationResources;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
final class AdvertView
	extends JPanel
{
    private static final int _margin = 5;
    private static final Logger _logger =
	    Logger.getLogger(AdvertView.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private class AdvertTextPane
	    extends JPanel
    {
	public AdvertTextPane()
	{
	    super(new BorderLayout());

	    setMinimumSize(new Dimension(0, 200));
	    setPreferredSize(new Dimension(0, 200));

	    JLabel textLabel = new JLabel(ApplicationResources.Text_Text);
	    textLabel.setBorder(
		    BorderFactory.createEmptyBorder(0, 0, _margin, 0));

	    add(textLabel, BorderLayout.NORTH);
	    add(new JScrollPane(_advertTextArea), BorderLayout.CENTER);
	}

    }

    private class AdvertImagePane
	    extends JPanel
    {
	public AdvertImagePane()
	{
	    super(new BorderLayout());
	    setBorder(
		    BorderFactory.createEmptyBorder(_margin, 0, 0, 0));

	    JLabel imageLabel = new JLabel(ApplicationResources.Image_Text);
	    imageLabel.setBorder(
		    BorderFactory.createEmptyBorder(0, 0, 0, _margin));
	    BrowsePanel browsePanel = new BrowsePanel();

	    JPanel topPanelContainer = new JPanel(new BorderLayout());
	    JPanel topPanel = new JPanel(new BorderLayout());
	    topPanel.add(imageLabel, BorderLayout.WEST);
	    topPanel.add(browsePanel, BorderLayout.CENTER);
	    topPanelContainer.add(topPanel, BorderLayout.CENTER);

	    JPanel thumbnailPanelContainer = new JPanel(new BorderLayout());
	    thumbnailPanelContainer.setBorder(
		    BorderFactory.createEmptyBorder(_margin, 0, 0, 0));
	    thumbnailPanelContainer.add(_thumbnailPanel, BorderLayout.CENTER);

	    add(topPanelContainer, BorderLayout.NORTH);
	    add(thumbnailPanelContainer, BorderLayout.CENTER);
	}

    }

    private class BrowsePanel
	    extends JPanel
    {
	public BrowsePanel()
	{
	    super(new BorderLayout());

	    JPanel browseButtonContainer = new JPanel(new BorderLayout());
	    browseButtonContainer.add(_browseButton, BorderLayout.CENTER);
	    browseButtonContainer.setBorder(
		    BorderFactory.createEmptyBorder(0, 2, 0, 0));

	    add(browseButtonContainer, BorderLayout.EAST);
	    add(_pathTextField, BorderLayout.CENTER);
	}

    }

    private class ImageFilter
	    extends FileFilter
    {
	@Override
	public boolean accept(File file)
	{
	    if (file.isDirectory())
	    {
		return true;
	    }

	    String extension = Argument.getExtension(file.getPath());

	    if (extension != null)
	    {
		for (final String validExtension : ApplicationResources.ValidExtensions)
		{
		    if (validExtension == null)
			continue;
		    if (validExtension.equalsIgnoreCase(extension))
			return true;
		}
	    }

	    return false;
	}

	@Override
	public String getDescription()
	{
	    return ApplicationResources.ImageFilterDescription;
	}

    }

    private class ThumbnailPanel
	    extends JPanel
    {
	public ThumbnailPanel()
	{
	    setBorder(BorderFactory.createLineBorder(AppColorScheme.NormalBorderColor));
	}

	private Image _image;

	public void setImage(Image value)
	{
	    if (_image != value)
	    {
		_image = value;
		repaint();
	    }
	}

	@Override
	public void paintComponent(Graphics g)
	{
	    super.paintComponent(g);

	    if (_image != null)
	    {
		int width = getWidth();
		int height = getHeight();

		int imageWidth = _image.getWidth(null);
		int imageHeight = _image.getHeight(null);

		Rectangle targetRect = new Rectangle(0, 0, width, height);
		Dimension imageSize =
			new Dimension(imageWidth, imageHeight);
		Rectangle imageRect =
			GrfxUtils.scaleToFit(targetRect, imageSize);
		g.drawImage(_image,
			    imageRect.x, imageRect.y,
			    imageRect.width, imageRect.height,
			    null);
	    }
	}

    }

    private class ButtonListener
	    implements ActionListener
    {
	public void actionPerformed(ActionEvent e)
	{
	    Object source = e.getSource();

	    if (source == _browseButton)
	    {
		if (JFileChooser.APPROVE_OPTION == getFileChooser().showDialog(AdvertView.this,
									       ApplicationResources.Open_Text))
		    _model.setImagePath(getFileChooser().getSelectedFile().
					getAbsolutePath());
	    }
	}

    }

    private class ModelPropertyChangeListener
	    implements PropertyChangeListener
    {
	public void propertyChange(final PropertyChangeEvent e)
	{
	    String propertyName = e.getPropertyName();

	    if (Advert.IMAGE_PATH_CHANGED.equals(propertyName))
	    {
		String imagePath = (String)e.getNewValue();
		_pathTextField.setText(imagePath);
		createThumbnail(imagePath);
	    }
	}

    }

    private class PathTextFieldListener
	    implements DocumentListener
    {
	public void insertUpdate(DocumentEvent e)
	{
	    updateModelImagePath();
	}

	public void removeUpdate(DocumentEvent e)
	{
	    updateModelImagePath();
	}

	public void changedUpdate(DocumentEvent e)
	{
	    updateModelImagePath();
	}

	private void updateModelImagePath()
	{
	    if (!_lockChanges && _model != null)
		_model.setImagePath(_pathTextField.getText());
	}

    }

    private class TextAreaListener
	    implements DocumentListener
    {
	public void changedUpdate(DocumentEvent e)
	{
	    updateModelText();
	}

	public void insertUpdate(DocumentEvent e)
	{
	    updateModelText();
	}

	public void removeUpdate(DocumentEvent e)
	{
	    updateModelText();
	}

	private void updateModelText()
	{
	    if (!_lockChanges && _model != null)
		_model.setText(_advertTextArea.getText());
	}

    }

    private JButton _browseButton;
    private JTextArea _advertTextArea;
    private JTextField _pathTextField;
    private PropertyChangeListener _modelPropertyChangeListener;
    private ThumbnailPanel _thumbnailPanel;

    /**
     * Creates a new instance of the <tt>AdvertView</tt> class.
     */
    public AdvertView()
    {
	super(new BorderLayout());

	_modelPropertyChangeListener = new ModelPropertyChangeListener();
	_thumbnailPanel = new ThumbnailPanel();

	_browseButton = new JButton(ApplicationResources.Browse_Text);
	_browseButton.addActionListener(new ButtonListener());
	_browseButton.setToolTipText(ApplicationResources.Browse_Tooltip);

	_advertTextArea = new JTextArea();
	_advertTextArea.getDocument().addDocumentListener(new TextAreaListener());

	_pathTextField = new JTextField();
	_pathTextField.getDocument().addDocumentListener(new PathTextFieldListener());
	_pathTextField.setEditable(false);

	setBorder(BorderFactory.createEmptyBorder(_margin, _margin,
						  _margin, _margin));
	add(new AdvertTextPane(), BorderLayout.NORTH);
	add(new AdvertImagePane(), BorderLayout.CENTER);
    }

    private Advert _model;

    public Advert getModel()
    {
	return _model;
    }

    private boolean _lockChanges;

    public void setModel(Advert value)
    {
	if (_model != value)
	{
	    _lockChanges = true;

	    if (_model != null)
	    {
		_model.removePropertyChangeListener(_modelPropertyChangeListener);

		_pathTextField.setText("");
		_advertTextArea.setText("");
		_thumbnailPanel.setImage(null);
	    }

	    _model = value;

	    if (_model != null)
	    {
		_model.addPropertyChangeListener(_modelPropertyChangeListener);

		_pathTextField.setText(_model.getImagePath());
		_advertTextArea.setText(_model.getText());
		createThumbnail(_model.getImagePath());
	    }

	    _lockChanges = false;
	}
    }

    @Override
    public void setEnabled(boolean value)
    {
	_browseButton.setEnabled(value);
	_advertTextArea.setEnabled(value);
	_pathTextField.setEnabled(value);
	_thumbnailPanel.setEnabled(value);
    }

    private JFileChooser _fileChooser;

    private JFileChooser getFileChooser()
    {
	if (_fileChooser == null)
	{
	    _fileChooser = new JFileChooser();
	    _fileChooser.addChoosableFileFilter(new ImageFilter());
	    _fileChooser.setAcceptAllFileFilterUsed(false);
	}

	return _fileChooser;
    }

    private void createThumbnail(final String imagePath)
    {
	new SwingWorker<Image, Void>()
	{
	    @Override
	    protected Image doInBackground() throws Exception
	    {
		return ImageProcessor.createThumbnail(
			imagePath,
			_model.getThumbnailMaximumSize());
	    }

	    @Override
	    protected void done()
	    {
		try
		{
		    _thumbnailPanel.setImage(get());
		}
		catch (InterruptedException e)
		{
		    _logger.log(Level.SEVERE, e.getMessage(), e);
		}
		catch (ExecutionException e)
		{
		    _logger.log(Level.SEVERE, e.getMessage(), e);
		}
	    }

	}.execute();
    }

}
