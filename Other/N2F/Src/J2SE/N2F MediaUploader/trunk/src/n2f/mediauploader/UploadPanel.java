/* ------------------------------------------------
 * UploadPanel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Graphics;
import java.awt.GridLayout;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.swing.BorderFactory;
import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JProgressBar;
import n2f.mediauploader.drawing.GrfxUtils;
import static n2f.mediauploader.resources.AppletResources.*;

/**
 * This panel is displayed while uploading photos to the web-service.
 * @author Alex Nesterov
 */
final class UploadPanel
	extends JPanel
{
    public static final String CANCEL_UPLOAD_PROPERTY =
	    "UploadPanel.cancelUpload";
    private static Logger _logger =
	    Logger.getLogger(UploadPanel.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    /** Used to render a thumbnail. */
    private class ThumbnailPanel
	    extends JPanel
    {
	private BufferedImage _thumbnail;

	public ThumbnailPanel(Dimension thumbnailDimension)
	{
	    setBorder(BorderFactory.createLineBorder(AppletColorScheme.ThumbnailNormalBorderColor));
	    setMaximumSize(thumbnailDimension);
	    setMinimumSize(thumbnailDimension);
	    setPreferredSize(thumbnailDimension);
	}

	public void setImage(BufferedImage value)
	{
	    if (_thumbnail != value)
	    {
		_thumbnail = value;
		repaint();
	    }
	}

	@Override
	public void paintComponent(Graphics g)
	{
	    super.paintComponent(g);

	    if (_thumbnail != null)
	    {
		int width = getWidth();
		int height = getHeight();
		
		Dimension thumbnailSize = new Dimension(_thumbnail.getWidth(),
							_thumbnail.getHeight());
		Rectangle targetRect = new Rectangle(0, 0, width, height);
		Rectangle destRect = GrfxUtils.scaleToFit(targetRect,
							  thumbnailSize);

		g.drawImage(_thumbnail,
			    destRect.x, destRect.y,
			    destRect.width, destRect.height,
			    null);
	    }
	}

    }

    private class UploadDataLabel
	    extends JLabel
    {
	public UploadDataLabel(float alignmentX)
	{
	    this("", alignmentX);
	}

	public UploadDataLabel(String text, float alignmentX)
	{
	    super(text);
	    setAlignmentX(alignmentX);
	    setBorder(BorderFactory.createEmptyBorder(0, 5, 0, 5));
	}

    }

    private class UploadMonitorPropertyChangeListener
	    implements PropertyChangeListener
    {
	public void propertyChange(PropertyChangeEvent e)
	{
	    String propertyName = e.getPropertyName();
	    _logger.log(Level.INFO, propertyName);
	    String propertyValue = e.getNewValue().toString();

	    if (UploadMonitor.ELAPSED_TIME_CHANGED.equals(propertyName))
	    {
		_elapsedTimeLabel.setText(propertyValue);
	    }
	    else if (UploadMonitor.REMAINING_TIME_CHANGED.equals(propertyName))
	    {
		_remainingTimeLabel.setText(propertyValue);
	    }
	    else if (UploadMonitor.TRANSFERRED_CHANGED.equals(propertyName))
	    {
		_transferredLabel.setText(propertyValue);
	    }
	    else if (UploadMonitor.UPLOAD_RATE_CHANGED.equals(propertyName))
	    {
		_uploadRateLabel.setText(propertyValue);
	    }
	}

    }

    private JButton _cancelButton;
    private JLabel _progressLabel;
    private JProgressBar _progressBar;
    private PropertyChangeSupport _changeSupport;
    private ThumbnailPanel _thumbnailPanel;
    private UploadDataLabel _elapsedTimeLabel;
    private UploadDataLabel _remainingTimeLabel;
    private UploadDataLabel _transferredLabel;
    private UploadDataLabel _uploadRateLabel;
    private UploadMonitor _uploadMonitor;

    /**
     * Creates a new instance of the <tt>UploadPanel</tt> class.
     * 
     * @param	thumbnailDimension
     *		Specifies the width and height for the thumbnanil to display
     *		during upload.
     */
    public UploadPanel(Dimension thumbnailDimension)
    {
	super(new BorderLayout());

	_changeSupport = new PropertyChangeSupport(this);
	_uploadMonitor = new UploadMonitor();
	_uploadMonitor.addPropertyChangeListener(new UploadMonitorPropertyChangeListener());

	_cancelButton = new JButton(CancelButtonText);
	_cancelButton.addActionListener(
		new ActionListener()
		{
		    public void actionPerformed(ActionEvent e)
		    {
			_changeSupport.firePropertyChange(CANCEL_UPLOAD_PROPERTY,
							  null, null);
		    }

		});

	_progressLabel = new JLabel();

	_progressBar = new JProgressBar();
	_progressBar.setValue(0);

	JPanel progressPanel = new JPanel(new FlowLayout(FlowLayout.CENTER));
	progressPanel.add(_cancelButton);
	progressPanel.add(_progressBar);
	progressPanel.add(_progressLabel);
	progressPanel.setMaximumSize(new Dimension(Integer.MAX_VALUE,
						   _cancelButton.getMaximumSize().height));

	_thumbnailPanel = new ThumbnailPanel(thumbnailDimension);

	JPanel centerContainer = new JPanel();
	centerContainer.setLayout(new BoxLayout(centerContainer,
						BoxLayout.Y_AXIS));
	centerContainer.add(_thumbnailPanel);

	JPanel bottomContainer = new JPanel();
	centerContainer.add(bottomContainer);
	bottomContainer.setLayout(new GridLayout(0, 2));

	JPanel leftPanel = new JPanel();
	leftPanel.setLayout(new BoxLayout(leftPanel, BoxLayout.Y_AXIS));
	leftPanel.add(new UploadDataLabel(ElapsedTimeText, 1.0f));
	leftPanel.add(new UploadDataLabel(RemainingTimeText, 1.0f));
	leftPanel.add(new UploadDataLabel(UploadRateText, 1.0f));
	leftPanel.add(new UploadDataLabel(TransferredText, 1.0f));

	JPanel rightPanel = new JPanel();
	rightPanel.setLayout(new BoxLayout(rightPanel, BoxLayout.Y_AXIS));

	_elapsedTimeLabel = new UploadDataLabel(0.0f);
	_remainingTimeLabel = new UploadDataLabel(0.0f);
	_uploadRateLabel = new UploadDataLabel(0.0f);
	_transferredLabel = new UploadDataLabel(0.0f);

	rightPanel.add(_elapsedTimeLabel);
	rightPanel.add(_remainingTimeLabel);
	rightPanel.add(_uploadRateLabel);
	rightPanel.add(_transferredLabel);

	bottomContainer.add(leftPanel);
	bottomContainer.add(rightPanel);

	add(progressPanel, BorderLayout.NORTH);
	add(centerContainer, BorderLayout.CENTER);
    }

    @Override
    public void addPropertyChangeListener(PropertyChangeListener listener)
    {
	_changeSupport.addPropertyChangeListener(listener);
    }

    public void fileTransferred(long dataTransferred)
    {
	_uploadMonitor.dataTransferred(dataTransferred);

	_progressBar.setIndeterminate(false);
	_progressBar.setValue(_progressBar.getValue() + 1);

	if (_progressBar.getValue() == _progressBar.getMaximum())
	    _uploadMonitor.stop();
    }

    public void uploadFinished()
    {
	_uploadMonitor.stop();
    }

    public void uploadStarted(long totalDataAmount, int fileCount)
    {
	_progressBar.setMaximum(fileCount);
	_progressBar.setValue(0);
	_progressBar.setIndeterminate(true);
	_uploadMonitor.start(totalDataAmount);
    }

    public void setThumbnail(BufferedImage bufferedImage)
    {
	_thumbnailPanel.setImage(bufferedImage);
	_progressLabel.setText(String.format("%s / %s",
					     _progressBar.getValue() + 1,
					     _progressBar.getMaximum()));
    }

}
