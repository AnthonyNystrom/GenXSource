package n2f.sup.core;

import n2f.sup.ui.GUIListener;

public interface ActionHero 
{
	int ACTION_TYPE_OPEN_FOLDER = 0;
	int ACTION_TYPE_SELECTABLE = 1;
	int ACTION_TYPE_CAMERA = 2;
	int ACTION_TYPE_FILE_SYSTEM = 3;
	
	String getTitle();
	GUIListener getListener();
	int getActionType();
	
}
