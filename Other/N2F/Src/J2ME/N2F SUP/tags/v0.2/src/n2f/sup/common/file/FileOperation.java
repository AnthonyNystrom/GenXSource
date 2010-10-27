package n2f.sup.common.file;

import n2f.sup.common.utils.RunnableTaskAdapter;
import n2f.sup.core.Engine;
import n2f.sup.core.ImageWrapper;
import n2f.sup.ui.GUIListener;


public class FileOperation extends RunnableTaskAdapter
{
	private int operationCode;
	private Object parameter;
	public static final int INIT = 10;
	public static final int OPEN = 11;
	public static final int DELETE = 12;
	public static final int RENAME = 13;
	public static final int MKDIR = 14;
	public static final int PREVIEW = 15;
	public static final int CREATE = 16;
	public static final int MODIFY = 17;
	public static final int SAVE_DATA = 18;
	
	public FileOperation(int operCode, GUIListener lis) {
		this.operationCode = operCode;
		this.listener = lis;
	}
	
	public FileOperation(int operCode, Object param, GUIListener lis) {
		this.operationCode = operCode;
		this.parameter = param;
		this.listener = lis;
	}
	
	protected void logic() throws Exception
	{
	    switch (operationCode) {
	    case INIT:
	    	FileUtils.initDir();
	   	  	break;
	    case OPEN:
	    	FileUtils.openSelected((String)parameter);
	    	break;
//	    case DELETE:
//	    	FileUtils.deleteCurrent((String)parameter);
//	    	break;
        case RENAME:
	        break;
//	    case MKDIR:
//    		FileUtils.createNewDir((String)parameter);
//    		break;
	    case PREVIEW:
			ImageWrapper imageWrapper = (ImageWrapper)parameter;
	    	Engine.getEngine().getImageFromFile(imageWrapper.getId(), imageWrapper.getIdOnForm());
	    	break;
	    case CREATE:
	    	FileUtils.createNewFile((String)parameter);
	    	break;
		case MODIFY:
			//FileUtils.createNewFile((String)parameter);
			break;
		case SAVE_DATA:
			FileUtils.saveData((FileSourceWrapper)parameter);
			break;
	    }
	}
	

	public int getType() {
		return operationCode;
	}

}
