
package n2f.blogger.webservice.utils;

import java.util.Vector;

import n2f.blogger.core.AbstractErrorManager;
import n2f.blogger.core.ErrorEvent;
import n2f.blogger.debug.Debug;

/**
 * It is a classical implementation of WorkerThread design pattern.
 * The main target of it is increasing of carrying capacity and minimization of average delay.
 * 
 * @author zyl
 */
public class WorkerThread
	extends AbstractErrorManager
{
    protected Vector tasks;
    protected Thread thread;
    private volatile boolean alive = true;
    private RunnableTask current = null;

    /**
     * Constructor.
     * It is private for instanciation of this class throught getInstance() method.
     */
    public WorkerThread()
    {
	super();
	tasks = new Vector();
	thread = new Thread(new Worker());
	thread.start();
    }

    /**
     * This method should put RunnableTask into WorkerThread.
     *
     * @param runnableTask - see RunnableTask interface.
     */
    public void put(RunnableTask runnableTask)
    {
	synchronized (tasks)
	{
	    tasks.addElement(runnableTask);
	    tasks.notifyAll();
	}
    }

    /**
     * Gets next RunnableTask in queue.
     * @return RunnableTask - It will be invoked in WorkerThread
     */
    public RunnableTask getNext()
    {
	RunnableTask result = null;
	synchronized (tasks)
	{
	    while (Thread.currentThread().isAlive() && tasks.isEmpty())
	    {
		try
		{
		    tasks.wait();
		}
		catch (InterruptedException e)
		{
		    e.printStackTrace();
		}
	    }
	    if (!tasks.isEmpty())
	    {
		result = (RunnableTask)tasks.elementAt(0);
		tasks.removeElementAt(0);
	    }
	}
	return result;
    }

    /**
     * This method interrupts current task and then removes all tasks from queue. 
     */
    public void removeAllTasks()
    {
	synchronized (tasks)
	{
	    interruptTask();
	    tasks.removeAllElements();
	}
    }

    /**
     * This method removes first task in queue - if queue is not empty;
     * or interrupts current task - if queue is empty.  
     */
    public void interruptTask()
    {
	synchronized (tasks)
	{
	    if (!tasks.isEmpty())
	    {
		tasks.removeElementAt(0);
	    }
	    else
	    {
		if (current != null)
		{
		    current.interrupt();
		    current = null;
		}
	    }
	}

	if (current != null)
	{
	    Debug.println("interruptTask = " + current);
	    current.interrupt();
	    current = null;
	}
    }

    /**
     * Private class that implements Runnable interface.
     * It represents separate thread.
     *
     * @author Zyl
     */
    private class Worker
	    implements Runnable
    {
	/**
	 * Default constructor.
	 */
	public Worker()
	{
	}

	/**
	 * This method invokes tasks executing.
	 */
	public void run()
	{
	    for (; alive && Thread.currentThread().isAlive();)
	    {
		RunnableTask task = getNext();
		current = task;
		if (task != null)
		    try
		    {
			task.execute();
			current = null;
		    }
		    catch (Throwable e)
		    {
			Debug.println("wt failed:" + e.toString());
			fireError(e.getMessage(),
				  new ErrorEvent(this,
						 new RuntimeException("WorkerThread task failed" + e.getMessage()),
						 "Operation failed",
						 task.getType()));

			e.printStackTrace();
		    }
	    }
	}

    }

    public void free()
    {
	this.alive = false;
	removeAllErrorListeners();
    }

}
