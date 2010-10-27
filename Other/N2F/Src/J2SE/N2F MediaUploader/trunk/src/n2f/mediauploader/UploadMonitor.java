/* ------------------------------------------------
 * UploadMonitor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.Calendar;
import java.util.Timer;
import java.util.TimerTask;
import n2f.mediauploader.util.DateConverter;
import n2f.mediauploader.util.TimeInterval;
import static n2f.mediauploader.resources.AppletResources.*;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
final class UploadMonitor
{
    /** Occurs when elapsed time changes. */
    public static final String ELAPSED_TIME_CHANGED =
	    "UploadMonitor.elapsedTimeChanged";
    /** Occurs when remaining time changse. */
    public static final String REMAINING_TIME_CHANGED =
	    "UploadMonitor.remainingTimeChanged";
    /** Occurs when upload rate changes. */
    public static final String UPLOAD_RATE_CHANGED =
	    "UploadMonitor.uploadRateChanged";
    /** Occurs when transferred amount of data changes. */
    public static final String TRANSFERRED_CHANGED =
	    "UploadMonitor.transferredChanged";
    private static final long _timerRate = 1000;

    private class ElapsedTimerTask
	    extends TimerTask
    {
	@Override
	public void run()
	{
	    TimeInterval elapsedTimeInterval =
		    DateConverter.subtract(Calendar.getInstance().
					   getTimeInMillis(), _startTimeMillis);
	    _changeSupport.firePropertyChange(ELAPSED_TIME_CHANGED, null,
					      elapsedTimeInterval);
	}

    }

    private PropertyChangeSupport _changeSupport;
    private long _remainingTimeMillis;
    private long _startTimeMillis;
    private long _totalDataAmount;
    private long _totalTransferred;
    private Timer _timer;
    private TimerTask _currentTimerTask;

    public UploadMonitor()
    {
	_changeSupport = new PropertyChangeSupport(this);
	_timer = new Timer(true);
    }

    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "l"));
	_changeSupport.addPropertyChangeListener(l);
    }

    public void dataTransferred(long dataTransferred)
    {
	if (_currentTimerTask != null)
	{
	    _totalTransferred += dataTransferred;
	    _changeSupport.firePropertyChange(TRANSFERRED_CHANGED, null,
					      formatTransferredData(_totalDataAmount,
								    _totalTransferred));
	    long currentTimeMillis = Calendar.getInstance().getTimeInMillis();
	    long interval = currentTimeMillis - _startTimeMillis;
	    /* Upload rate in bytes per second. */
	    long uploadRate = (long)(_totalTransferred / (float)interval * 1000);
	    _changeSupport.firePropertyChange(UPLOAD_RATE_CHANGED, null,
					      formatUploadRate(uploadRate));
	    /* Remaining time. */
	    long dataToTransfer = _totalDataAmount - _totalTransferred;
	    _remainingTimeMillis = interval * dataToTransfer / _totalTransferred;
	    TimeInterval remainingTimeInterval =
		    new TimeInterval(_remainingTimeMillis);
	    _changeSupport.firePropertyChange(REMAINING_TIME_CHANGED, null,
					      remainingTimeInterval.toString());
	}
    }

    public void start(long totalDataAmount)
    {
	_totalDataAmount = totalDataAmount;
	_totalTransferred = 0;
	_changeSupport.firePropertyChange(ELAPSED_TIME_CHANGED,
					  null, new TimeInterval(0));
	_changeSupport.firePropertyChange(REMAINING_TIME_CHANGED, null, "\u221E");
	_changeSupport.firePropertyChange(TRANSFERRED_CHANGED, null,
					  formatTransferredData(_totalDataAmount,
								_totalTransferred));
	_changeSupport.firePropertyChange(UPLOAD_RATE_CHANGED, null, "\u221E");

	if (_currentTimerTask != null)
	    _currentTimerTask.cancel();

	_currentTimerTask = new ElapsedTimerTask();
	_startTimeMillis = Calendar.getInstance().getTimeInMillis();
	_timer.scheduleAtFixedRate(_currentTimerTask, 0, _timerRate);
    }

    public void stop()
    {
	if (_currentTimerTask != null)
	{
	    _currentTimerTask.cancel();
	    _currentTimerTask = null;
	}
    }

    private static String formatDataAmount(long dataAmount)
    {
	int divider = 1;
	String measureUnits = Byte;

	if (dataAmount > 1024 * 1024)
	{
	    divider = 1024 * 1024;
	    measureUnits = MByte;
	}
	else if (dataAmount > 1024)
	{
	    divider = 1024;
	    measureUnits = KByte;
	}

	return String.format("%s %s", dataAmount / divider, measureUnits);
    }

    private static String formatUploadRate(long uploadRate)
    {
	return String.format(PerSecond, formatDataAmount(uploadRate));
    }

    private static String formatTransferredData(long totalDataAmount,
						  long totalTransferred)
    {
	return String.format("%s / %s",
			     formatDataAmount(totalTransferred),
			     formatDataAmount(totalDataAmount));
    }

}
