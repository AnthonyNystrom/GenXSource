/* ------------------------------------------------
 * WorkerThread.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.util;

import java.util.LinkedList;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * It is a classical implementation of WorkerThread design pattern.
 * The main target of it is increasing of carrying capacity and minimization of average delay.
 * 
 * @author Alex Nesterov
 */
public class WorkerThread
{
    private static final Logger _logger =
	    Logger.getLogger(WorkerThread.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private class Worker
	    implements Runnable
    {
	public void run()
	{
	    while (_alive && Thread.currentThread().isAlive())
	    {
		_logger.log(Level.INFO, "Trying to retrieve the next task...");
		Runnable task = getNext();

		if (task != null)
		{
		    _logger.log(Level.INFO, "New task is not null. Running the task...");
		    task.run();
		}
	    }
	}

    }

    private volatile boolean _alive;
    private LinkedList<Runnable> _tasks;
    private Thread _thread;

    /**
     * Creates a new instance of the <tt>WorkerThread</tt> class.
     */
    public WorkerThread()
    {
	_alive = true;
	_tasks = new LinkedList<Runnable>();

	_logger.log(Level.INFO, "Creating Worker instance...");
	_thread = new Thread(new Worker());
	_thread.start();
    }

    public void put(Runnable task)
    {
	if (task == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "task"));
	synchronized (_tasks)
	{
	    _tasks.add(task);
	    _logger.log(Level.INFO, "New task was added.");
	    _tasks.notifyAll();
	}
    }

    /**
     * Removes all the tasks from the queue.
     */
    public void removeAllTasks()
    {
	synchronized (_tasks)
	{
	    _tasks.clear();
	    _logger.log(Level.INFO, "Tasks list cleared.");
	}
    }

    private Runnable getNext()
    {
	Runnable result = null;

	synchronized (_tasks)
	{
	    while (Thread.currentThread().isAlive() && _tasks.isEmpty())
	    {
		try
		{
		    _logger.log(Level.INFO,
				"Waiting for a new task to be added to the queue...");
		    _tasks.wait();
		}
		catch (InterruptedException ignored)
		{
		}
	    }

	    if (!_tasks.isEmpty())
		result = _tasks.poll();
	}

	return result;
    }

}
