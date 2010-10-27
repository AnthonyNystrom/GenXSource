/* ------------------------------------------------
 * MediaUploaderModel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import n2f.mediauploader.drawing.RotateType;
import java.io.File;
import java.util.LinkedList;
import java.util.List;
import n2f.mediauploader.common.KeyValuePair;

import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
public final class MediaUploaderModel
{
    public static final String FILE_LIST_CHANGED_PROPERTY =
	    "MediaUploaderModel.fileListChanged";
    public static final String FILE_ROTATE_TYPE_PROPERTY =
	    "MediaUploaderModel.fileRotateType";
    public static final String ICON_PROPERTY = "MediaUploaderModel.icon";
    public static final String THUMBNAIL_PROPERTY =
	    "MediaUploaderModel.thumbnail";
    private List<KeyValuePair<File>> _fileListFiltered;
    private List<KeyValuePair<File>> _fileListOriginal;
    private List<KeyValuePair<File>> _selectedFileList;
    private String[] _validExtensions;
    private IPhotoProcessor _photoProcessor;
    private IRotateTypeProcessor _rotateTypeProcessor;
    private PropertyChangeSupport _changeSupport;

    /**
     * Creates a new instance of the <tt>MediaUploaderModel</tt> class.
     * 
     * @param	photoProcessor
     *		Specifies the instance that will process selected photos.
     * 
     * @throws	IllegalArgumentException
     *		If the specified <tt>photoProcessor</tt> is <code>null</code>, or
     *		if the specified <tt>rotateTypeProcessor</tt> is <code>null</code>.
     */
    public MediaUploaderModel(IPhotoProcessor photoProcessor,
			       IRotateTypeProcessor rotateTypeProcessor)
    {
	if (photoProcessor == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "photoProcessor"));
	if (rotateTypeProcessor == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "rotateTypeProcessor"));

	_changeSupport = new PropertyChangeSupport(this);
	_fileListFiltered = new LinkedList<KeyValuePair<File>>();
	_fileListOriginal = new LinkedList<KeyValuePair<File>>();
	_selectedFileList = new LinkedList<KeyValuePair<File>>();
	_photoProcessor = photoProcessor;
	_rotateTypeProcessor = rotateTypeProcessor;
	_validExtensions = new String[] { null };
    }

    /**
     * Adds the specified listener to receive property change events from this
     * component.
     * 
     * @param	l
     *		Specifies the property change listener.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "l"));
	_changeSupport.addPropertyChangeListener(l);
    }

    /**
     * Interrupts the upload.
     */
    public void cancelUploadPhotos()
    {
	_photoProcessor.cancelUploadPhotos();
    }

    /**
     * Removes selection from the specified file.
     * @param	file
     *		Specifies the file to remove selection from.
     * @throws	IllegalArgumentException
     *		If the specified <tt>file</tt> is <code>null</code>.
     */
    public void deselect(KeyValuePair<File> file)
    {
	if (file == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "file"));

	_selectedFileList.remove(file);
    }

    /**
     * Returns currently selected files.
     * @return	Currently selected files.
     */
    public List<KeyValuePair<File>> getSelectedFileList()
    {
	return _selectedFileList;
    }

    /**
     * Rotates the specified image at the specified angle.
     * 
     * @param fileToRotate
     * @param rotateType
     * 
     * @throws	IllegalArgumentException
     *		If the specified <tt>fileToRotate</tt> is <code>null</code>, or
     *		if the specified <tt>rotateType</tt> is <code>null</code>.
     */
    public void rotate(KeyValuePair<File> fileToRotate, RotateType rotateType)
    {
	if (fileToRotate == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "fileToRotate"));
	if (rotateType == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "rotateType"));

	RotateType currentRotateType = getRotateType(fileToRotate);
	RotateType newRotateType =
		_rotateTypeProcessor.combineRotateTypes(currentRotateType,
							rotateType);

	fileToRotate.set(FILE_ROTATE_TYPE_PROPERTY,
			 newRotateType);
    }

    /**
     * Selects all the files.
     */
    public void selectAll()
    {
	_selectedFileList.clear();

	for (final KeyValuePair<File> item : _fileListFiltered)
	    _selectedFileList.add(item);
    }

    /**
     * Removes selection from all the files.
     */
    public void selectNone()
    {
	_selectedFileList.clear();
    }

    /**
     * Selects the specified file.
     * @param	file
     *		Specifies the file to select.
     * @throws	IllegalArgumentException
     *		If the specified <tt>file</tt> is <code>null</code>.
     */
    public void select(KeyValuePair<File> file)
    {
	if (file == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "file"));

	if (_fileListFiltered.contains(file) && !_selectedFileList.contains(file))
	    _selectedFileList.add(file);
    }

    /**
     * Sets the currently active folder.
     * @param	fileList
     *		Specifies the list of files this <tt>MediaUploaderModel</tt>
     *		should currently operate.
     * @throws	IllegalArgumentException
     *		If the specified <tt>fileList</tt> is <code>null</code>.
     */
    public void setFolder(List<KeyValuePair<File>> fileList)
    {
	if (fileList == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "fileList"));

	_fileListOriginal = fileList;
	updateFileList();
    }

    /**
     * Specifies the list of valid extensions to determine which files should be
     * processed from the active folder.
     * 
     * @param	validExtensions
     *		Specifies the list of valid extensions like shown below.:
     *		<br/>
     *		<code>
     *		String[] validExtensions = new String[] { "jpg", "png" };
     *		</code>
     * @throws	IllegalArgumentException
     *		If the specified <tt>validExtensions</tt> is <code>null</code>.
     * @see	#setFolder(List<KeyValuePair<File>>)
     */
    public void setValidExtensions(String[] validExtensions)
    {
	if (validExtensions == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "validExtensions"));

	_validExtensions = validExtensions;
	updateFileList();
    }

    /**
     * Uploads currently selected photos.
     * @throws	n2f.mediauploader.PhotoProcessorException
     *		If error occures while uploading the selected photos.
     * @see	#selectNone()
     * @see	#deselect(KeyValuePair<File>)
     * @see	#selectAll()
     * @see	#select(KeyValuePair<File>)
     */
    public void uploadSelected()
	    throws PhotoProcessorException
    {
	_photoProcessor.uploadPhotos(_selectedFileList);
    }

    private void updateFileList()
    {
	_fileListFiltered.clear();
	_selectedFileList.clear();

	for (KeyValuePair<File> item : _fileListOriginal)
	{
	    if (ExtensionFilter.checkFile(
		    item.value.getAbsolutePath(), _validExtensions))
	    {
		_fileListFiltered.add(item);
	    }
	}

	_changeSupport.firePropertyChange(
		FILE_LIST_CHANGED_PROPERTY,
		null,
		_fileListFiltered);
    }

    private static RotateType getRotateType(KeyValuePair<File> file)
    {
	RotateType rotateType = RotateType.RotateNone;
	Object value = file.get(FILE_ROTATE_TYPE_PROPERTY);
	
	if (value != null)
	    rotateType = (RotateType)value;
	
	return rotateType;
    }

}
