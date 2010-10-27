
package n2f.blogger.webservice;

import genetibase.java.microedition.Deallocatable;
import n2f.blogger.App;
import n2f.blogger.core.AbstractErrorManager;
import n2f.blogger.core.ErrorListener;
import n2f.blogger.ui.GUIListener;
import n2f.blogger.ui.SettingsBean;
import n2f.blogger.ui.UIManager;
import n2f.blogger.webservice.utils.RunnableTask;
import n2f.blogger.webservice.utils.WorkerThread;

public class WebServiceInteractor
	extends AbstractErrorManager
	implements Deallocatable
{
    private WorkerThread wt;
    public final static long FIVEMIN = 300000;
    public final static long HOURLY = 3600000;
    public final static long DAILY = 24 * 3600000;

    public WebServiceInteractor()
    {
	wt = new WorkerThread();
	App.getCurrentApp().addToDeallocatableList(this);
    }

    public void init()
    {
    }

    private class UploadTimer
    {
	private boolean running = true;
	private volatile long lastExec;
	private volatile long intensity = FIVEMIN;

	public void start()
	{
	    this.running = true;
	    this.lastExec = System.currentTimeMillis();

	}

	public void changeIntensity(long newIntensity)
	{
	    if (running)
	    {
		this.intensity = newIntensity;
	    }
	}

	public void stop()
	{
	    running = false;
	}
    }

    public void addErrorListener(ErrorListener errorListener)
    {
	super.addErrorListener(errorListener);
	wt.addErrorListener(errorListener);
    }

    public void getWebMemberId(GUIListener lis)
    {
	GetUserIdsTask task =
		new GetUserIdsTask(GetUserIdsTask.TYPE_GET_CREDENTIALS, lis,
				   this);
	executeInWorkerThread(task);
    }
    
    public void analyzeServiceResponse(Object resultObj,
					NetworkServiceTaskAdapter owner)
    {
	if (owner != null)
	{
	    switch (owner.getType())
	    {
		case NetworkServiceTaskAdapter.TYPE_GET_CREDENTIALS:
		    if (resultObj != null)
		    {
			SettingsBean bean = (SettingsBean)UIManager.getInstance().
				getBean(UIManager.SCREEN_SETTINGS);
			bean.setWebMemberId((String)resultObj);
		    }
		    break;

		
		default:
		    break;
	    }
	}
    }

    public void executeInWorkerThread(RunnableTask runnableTask)
    {
	if (wt != null)
	    wt.put(runnableTask);
    }

    public void free()
    {
	if (wt != null)
	    wt.free();
	wt = null;
    }

}
