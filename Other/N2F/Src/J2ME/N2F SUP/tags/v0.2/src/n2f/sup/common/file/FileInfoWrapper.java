
package n2f.sup.common.file;

public class FileInfoWrapper {
	private String fileName;
	private boolean isDirectory;
	private boolean isHidden;
	private boolean isRoot;
	private String path;
	

	public FileInfoWrapper(String fileName, String filePath, boolean isDirectory, boolean isHidden, boolean isRoot) {
		this.fileName = fileName;
		this.path = filePath;
		this.isDirectory = isDirectory;
		this.isHidden = isHidden;
		this.isRoot = isRoot;
		
	}
	
	public String getPath() {
		return path;
	}

	public void setPath(String path) {
		this.path = path;
	}
	
	public String getFileName() {
		return fileName;
	}
	
	public void setFileName(String fileName) {
		this.fileName = fileName;
	}
	
	public boolean isDirectory() {
		return isDirectory;
	}
	
	public void setDirectory(boolean isDirectory) {
		this.isDirectory = isDirectory;
	}
	
	public boolean isHidden() {
		return isHidden;
	}
	
	public void setHidden(boolean isHidden) {
		this.isHidden = isHidden;
	}

	public boolean isRoot() {
		return isRoot;
	}

	public void setRoot(boolean isRoot) {
		this.isRoot = isRoot;
	}
	
	public String toString() {
		StringBuffer sb = new StringBuffer("FileInfoWrapper[");
		sb.append("path to file=").append(path).append(fileName);
		sb.append("isDirectory=").append(isDirectory());
		sb.append("isRoot=").append(isRoot());
		sb.append("]");
		return sb.toString();
	}


    /** @noinspection RedundantIfStatement*/
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        FileInfoWrapper that = (FileInfoWrapper) o;

        if (isDirectory != that.isDirectory) return false;
        //if (isHidden != that.isHidden) return false;
        //if (isRoot != that.isRoot) return false;
        if (fileName != null ? !fileName.equals(that.fileName) : that.fileName != null) return false;
        //if (path != null ? !path.equals(that.path) : that.path != null) return false;

        return true;
    }

    public int hashCode() {
        int result;
        result = (fileName != null ? fileName.hashCode() : 0);
        result = (path != null ? path.hashCode() : 0);
        result = 31 * result + (isDirectory ? 1 : 0);
        result = 31 * result + (isHidden ? 1 : 0);
        result = 31 * result + (isRoot ? 1 : 0);
        return result;
    }
}
