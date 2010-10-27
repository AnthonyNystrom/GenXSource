package n2f.sup.common;

public interface PauseListener {
	int STATE_PAUSE = -10000;
	int STATE_RESTORE = 10000;
	
	void notify(int state);
}
