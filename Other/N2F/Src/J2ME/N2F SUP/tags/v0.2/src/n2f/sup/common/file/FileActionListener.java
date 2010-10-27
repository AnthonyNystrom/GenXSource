package n2f.sup.common.file;

import n2f.sup.common.ActionListener;

public interface FileActionListener extends ActionListener {
	int STATUS_FILE_FAILED_READ_WRITE = 100;
	int STATUS_FILE_FAILED_READ = 101;
	int STATUS_FILE_FAILED_WRITE = 102;
	int STATUS_FILE_FAILED_SECURITY = 103;
	int STATUS_FILE_FAILED_EXIST = 104;
	int STATUS_FILE_FAILED_CLOSE = 105;
	int STATUS_FILE_FAILED_NOT_ENOUGHT_SPACE = 106;
	int STATUS_FILE_FAILED_CREATE = 107;
	int STATUS_FILE_FAILED_OPEN = 108;
	int STATUS_FILE_FAILED_DELETE = 109;
	int STATUS_FILE_FAILED = 110;
	
	public static final int STATUS_PERMISSION_DENIED = 0;
	public static final int STATUS_PERMISSION_ALLOWED = 1;
	public static final int STATUS_PERMISSION_UNKNOWN = -1;
	
	
}
