package n2f.sup.common.file;

public class FileSourceWrapper extends FileInfoWrapper {
	private byte[] source = null;
	private boolean overwrite = false;
	
	public FileSourceWrapper(String fileName, boolean isDirectory, boolean isHidden, boolean isRoot, byte[] source) {
		super(fileName, "", isDirectory, isHidden, isRoot);
		this.source = source;
	}
	
	public byte[] getSource() {
		return this.source;
	}
	
	public boolean isOverwrite() {
		return overwrite;
	}

	public void setOverwrite(boolean overwrite) {
		this.overwrite = overwrite;
	}
	

}
