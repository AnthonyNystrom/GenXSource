/* ------------------------------------------------
 * MediaUploaderModelTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import n2f.mediauploader.drawing.RotateType;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.io.File;
import java.util.LinkedList;
import java.util.List;
import n2f.mediauploader.common.KeyValuePair;
import org.jmock.*;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class MediaUploaderModelTest
{
    private static final String[] _validExtensions = new String[] { "jpg" };
    private IRotateTypeProcessor _rotateTypeProcessor;
    private MediaUploaderModel _model;
    private Mockery _context;

    @Before
    public void setUp()
    {
	_context = new Mockery();
	_rotateTypeProcessor = _context.mock(IRotateTypeProcessor.class);
	_model = new MediaUploaderModel(new PhotoProcessorStubAdapter(),
					_rotateTypeProcessor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam0()
    {
	new MediaUploaderModel(null, _rotateTypeProcessor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam1()
    {
	new MediaUploaderModel(new PhotoProcessorStubAdapter(), null);
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void addPropertyChangeListener_NullArg()
    {
	_model.addPropertyChangeListener(null);
    }

    @Test
    public void deselectAll()
    {
	List<KeyValuePair<File>> fileList = createFileList();
	populateFileList(fileList);
	final KeyValuePair<File> selectedFile = fileList.get(0);
	final KeyValuePair<File> selectedFile2 = fileList.get(1);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileList);
	_model.select(selectedFile);
	_model.select(selectedFile2);

	List<KeyValuePair<File>> selectedFileList = _model.getSelectedFileList();
	assertEquals(2, selectedFileList.size());
	assertEquals(selectedFile, selectedFileList.get(0));
	assertEquals(selectedFile2, selectedFileList.get(1));

	_model.selectNone();

	selectedFileList = _model.getSelectedFileList();
	assertEquals(0, selectedFileList.size());
    }

    @Test
    public void deselect() throws PhotoProcessorException
    {
	List<KeyValuePair<File>> fileListOriginal = createFileList();
	populateFileList(fileListOriginal);
	final KeyValuePair<File> selectedFile = fileListOriginal.get(0);
	final KeyValuePair<File> selectedFile2 = fileListOriginal.get(1);

	_model = new MediaUploaderModel(
		new PhotoProcessorStubAdapter()
		{
		    @Override
		    public void uploadPhotos(List<KeyValuePair<File>> fileList)
			    throws PhotoProcessorException
		    {
			assertEquals(1, fileList.size());
			assertEquals(selectedFile, fileList.get(0));
		    }

		},
		_rotateTypeProcessor);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileListOriginal);
	_model.select(selectedFile);
	_model.select(selectedFile2);
	_model.deselect(selectedFile2);
	_model.uploadSelected();
    }

    @Test(expected = IllegalArgumentException.class)
    public void deselectIllegalArgumentException()
    {
	_model.deselect(null);
    }

    @Test
    public void rotate()
    {
	List<KeyValuePair<File>> fileListOriginal = createFileList();
	populateFileList(fileListOriginal);
	final KeyValuePair<File> fileToRotate = fileListOriginal.get(0);
	final RotateType[] values = new RotateType[] { RotateType.RotateNone,
							RotateType.Rotate90,
							RotateType.Rotate180,
							RotateType.Rotate270
	};

	_model = new MediaUploaderModel(
		new PhotoProcessorStubAdapter(),
		new IRotateTypeProcessor()
		{
		    public RotateType combineRotateTypes(RotateType x,
							  RotateType y)
		    {
			assertEquals(_oldRotateType, x);
			assertEquals(values[_currentIndex], y);
			_oldRotateType = y;
			_currentIndex++;

			return y;
		    }

		    private RotateType _oldRotateType = RotateType.RotateNone;
		    private int _currentIndex = 0;
		});

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileListOriginal);

	for (final RotateType rotateType : values)
	{
	    _model.rotate(fileToRotate, rotateType);
	}
    }

    @Test(expected = IllegalArgumentException.class)
    public void rotateIllegalArgumentExceptionParam0()
    {
	_model.rotate(null, RotateType.RotateNone);
    }

    @Test(expected = IllegalArgumentException.class)
    public void rotateIllegalArgumentExceptionParam1()
    {
	_model.rotate(createFile(), null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void selectIllegalArgumentException()
    {
	_model.select(null);
    }

    @Test
    public void selectSingleSelection() throws PhotoProcessorException
    {
	List<KeyValuePair<File>> fileListOriginal = createFileList();
	populateFileList(fileListOriginal);
	final KeyValuePair<File> selectedFile = fileListOriginal.get(0);

	_model = new MediaUploaderModel(
		new PhotoProcessorStubAdapter()
		{
		    @Override
		    public void uploadPhotos(List<KeyValuePair<File>> fileList)
			    throws PhotoProcessorException
		    {
			assertEquals(1, fileList.size());
			assertEquals(selectedFile, fileList.get(0));
		    }

		},
		_rotateTypeProcessor);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileListOriginal);
	_model.select(selectedFile);
	_model.uploadSelected();
    }

    @Test
    public void selectSame()
    {
	List<KeyValuePair<File>> fileList = createFileList();
	final KeyValuePair<File> selectedFile = createFile();
	fileList.add(selectedFile);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileList);
	_model.selectAll();
	_model.select(selectedFile);

	assertEquals(1, _model.getSelectedFileList().size());
    }

    @Test
    public void selectMultipleSelection() throws PhotoProcessorException
    {
	List<KeyValuePair<File>> fileListOriginal = createFileList();
	populateFileList(fileListOriginal);
	final KeyValuePair<File> selectedFile = fileListOriginal.get(0);
	final KeyValuePair<File> selectedFile2 = fileListOriginal.get(1);

	_model = new MediaUploaderModel(
		new PhotoProcessorStubAdapter()
		{
		    @Override
		    public void uploadPhotos(List<KeyValuePair<File>> fileList)
			    throws PhotoProcessorException
		    {
			assertEquals(2, fileList.size());
			assertEquals(selectedFile, fileList.get(0));
			assertEquals(selectedFile2, fileList.get(1));
		    }

		},
		_rotateTypeProcessor);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileListOriginal);
	_model.select(selectedFile);
	_model.select(selectedFile2);
	_model.uploadSelected();
    }

    @Test
    public void selectAll()
    {
	List<KeyValuePair<File>> fileList = createFileList();
	populateFileList(fileList);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileList);
	_model.selectAll();

	List<KeyValuePair<File>> selectedFileList = _model.getSelectedFileList();
	verifyFilteredFileList(selectedFileList);
    }

    @Test
    public void setFolderEmptyFileList()
    {
	List<KeyValuePair<File>> fileList = createFileList();

	_model.addPropertyChangeListener(
		new PropertyChangeListener()
		{
		    public void propertyChange(PropertyChangeEvent e)
		    {
			List<KeyValuePair<File>> fileList =
				(List<KeyValuePair<File>>)e.getNewValue();
			assertEquals(0, fileList.size());
		    }

		});
	_model.setFolder(fileList);
    }

    @Test(expected = IllegalArgumentException.class)
    public void setFolderIllegalArgumentException()
    {
	_model.setFolder(null);
    }

    @Test
    public void setFolderOnlyImages()
    {
	List<KeyValuePair<File>> fileList = createFileList();
	populateFileList(fileList);

	_model.setValidExtensions(_validExtensions);
	_model.addPropertyChangeListener(
		new PropertyChangeListener()
		{
		    public void propertyChange(PropertyChangeEvent e)
		    {
			verifyFilteredFileList((List<KeyValuePair<File>>)e.getNewValue());
		    }

		});
	_model.setFolder(fileList);
    }

    @Test
    public void setFolderClearsSelectedFileList()
    {
	List<KeyValuePair<File>> fileList = createFileList();
	populateFileList(fileList);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileList);

	_model.selectAll();
	assertEquals(2, _model.getSelectedFileList().size());

	_model.setFolder(fileList);
	assertEquals(0, _model.getSelectedFileList().size());
    }

    @Test(expected = IllegalArgumentException.class)
    public void setValidExtensionsIllegalArgumentException()
    {
	_model.setValidExtensions(null);
    }

    @Test
    public void setValidExtensionsClearsSelectedFileList() throws InterruptedException
    {
	List<KeyValuePair<File>> fileList = createFileList();
	populateFileList(fileList);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileList);

	_model.selectAll();
	assertEquals(2, _model.getSelectedFileList().size());

	_model.setValidExtensions(_validExtensions);
	assertEquals(0, _model.getSelectedFileList().size());
    }

    @Test
    public void uploadSelected() throws PhotoProcessorException
    {
	List<KeyValuePair<File>> fileList = createFileList();
	populateFileList(fileList);

	_model = new MediaUploaderModel(
		new PhotoProcessorStubAdapter()
		{
		    @Override
		    public void uploadPhotos(List<KeyValuePair<File>> fileList)
			    throws PhotoProcessorException
		    {
			verifyFilteredFileList(fileList);
		    }

		},
		_rotateTypeProcessor);

	_model.setValidExtensions(_validExtensions);
	_model.setFolder(fileList);
	_model.selectAll();
	_model.uploadSelected();
    }

    private static KeyValuePair<File> createFile()
    {
	return new KeyValuePair<File>("image", new File("C:\\image.jpg"));
    }

    private static List<KeyValuePair<File>> createFileList()
    {
	return new LinkedList<KeyValuePair<File>>();
    }

    private static void populateFileList(List<KeyValuePair<File>> fileList)
    {
	fileList.add(new KeyValuePair<File>("image", new File("C:\\image.jpg")));
	fileList.add(new KeyValuePair<File>("image2", new File("C:\\image2.jpg")));
	fileList.add(new KeyValuePair<File>("foo", new File("foo.bar")));
    }

    private static void verifyFilteredFileList(List<KeyValuePair<File>> fileList)
    {
	assertEquals(2, fileList.size());

	KeyValuePair<File> item = fileList.get(0);
	KeyValuePair<File> item2 = fileList.get(1);

	assertEquals("image", item.key);
	assertEquals("C:\\image.jpg", item.value.getAbsolutePath());

	assertEquals("image2", item2.key);
	assertEquals("C:\\image2.jpg", item2.value.getAbsolutePath());
    }

}
