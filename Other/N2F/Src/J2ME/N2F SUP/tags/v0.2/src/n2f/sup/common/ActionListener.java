package n2f.sup.common;

public interface ActionListener {
	int STATUS_OK = 200;
	int STATUS_FAILED = -1;
	

    public void actionPerformed(Object action, int statusResult);
}