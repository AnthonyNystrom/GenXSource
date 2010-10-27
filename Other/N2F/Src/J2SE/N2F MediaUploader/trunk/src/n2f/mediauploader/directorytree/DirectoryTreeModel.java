/* ------------------------------------------------
 * DirectoryTreeModel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.directorytree;

import java.io.File;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import javax.swing.filechooser.FileSystemView;
import javax.swing.tree.DefaultTreeModel;
import n2f.mediauploader.MediaUploaderModel;
import n2f.mediauploader.common.KeyValuePair;
import n2f.mediauploader.util.Argument;

/**
 * Represents the File Explorer like tree-model.
 * @author Alex Nesterov
 */
public class DirectoryTreeModel
	extends DefaultTreeModel
{
    private static final FileSystemView _fsv;

    static
    {
	_fsv = FileSystemView.getFileSystemView();
    }

    /**
     * Creates a new instance of the <tt>DirectoryTreeModel</tt> class.
     */
    public DirectoryTreeModel()
    {
	super(DirectoryTreeNode.createEmptyTreeNode());

	List<KeyValuePair<File>> fileRoots = getPathChoices(null);
	DirectoryTreeNode rootTreeNode = (DirectoryTreeNode)root;

	for (final KeyValuePair<File> fileRoot : fileRoots)
	{
	    DirectoryTreeNode newTreeNode = new DirectoryTreeNode(fileRoot);
	    rootTreeNode.add(newTreeNode);
	    newTreeNode.add(DirectoryTreeNode.createEmptyTreeNode());
	}
    }

    /**
     * Retrieves the directories available at the specified path.
     * @param	path
     *		Specifies the path to retrieve directories for.
     * @return	List of available directories at the specified path. If the
     *		specified <tt>path</tt> is <code>null</code> or is empty root
     *		directories are returned. It is <tt>Desktop</tt> for Windows.
     */
    public static List<KeyValuePair<File>> getPathChoices(DirectoryTreeNode[] path)
    {
	if (path == null || path.length == 0)
	{
	    File[] fileRoots = _fsv.getRoots();

	    List<KeyValuePair<File>> pathChoices =
		    new LinkedList<KeyValuePair<File>>();
	    for (File fileRoot : fileRoots)
	    {
		if (_fsv.isHiddenFile(fileRoot))
		    continue;

		String systemName = _fsv.getSystemDisplayName(fileRoot);
		if (systemName.length() == 0)
		    systemName = fileRoot.getAbsolutePath();

		KeyValuePair<File> rootPair = new KeyValuePair<File>(
			systemName, fileRoot);
		rootPair.set(MediaUploaderModel.ICON_PROPERTY, _fsv.getSystemIcon(fileRoot));
		pathChoices.add(rootPair);
	    }

	    return pathChoices;
	}

	DirectoryTreeNode lastTreeNode = path[path.length - 1];
	File lastFile = lastTreeNode.getAssociatedLeaf().value;

	if (!lastFile.exists())
	    return new ArrayList<KeyValuePair<File>>();

	if (!lastFile.isDirectory())
	    return null;

	List<KeyValuePair<File>> pathChoices =
		new LinkedList<KeyValuePair<File>>();

	for (final File child : lastFile.listFiles())
	{
	    /* Ignore regular files and hidden directories. */
	    if (!child.isDirectory())
		continue;
	    if (_fsv.isHiddenFile(child))
		continue;

	    String childFileName = _fsv.getSystemDisplayName(child);
	    if (childFileName == null || childFileName.isEmpty())
		childFileName = child.getName();

	    KeyValuePair<File> pair = new KeyValuePair<File>(childFileName,
							     child);
	    pair.set(MediaUploaderModel.ICON_PROPERTY, _fsv.getSystemIcon(child));
	    pathChoices.add(pair);
	}

	return pathChoices;
    }

    /**
     * Retrieves the files at the specified path.
     * @param	path
     *		Specifies the path to retrieve files for.
     * @return	List of files at the specified path. If the specified <tt>path</tt>
     *		is <code>null</code> or is empty an empty list is returned.
     */
    public static List<KeyValuePair<File>> getLeafs(DirectoryTreeNode[] path)
    {
	if (path == null || path.length == 0)
	    return new ArrayList<KeyValuePair<File>>();

	DirectoryTreeNode lastTreeNode = path[path.length - 1];
	File lastFile = lastTreeNode.getAssociatedLeaf().value;

	if (!lastFile.exists() || !lastFile.isDirectory())
	    return new ArrayList<KeyValuePair<File>>();

	List<KeyValuePair<File>> leafs = new LinkedList<KeyValuePair<File>>();

	for (File child : lastFile.listFiles())
	{
	    /* Ignore directories and hidden directories. */
	    if (child.isDirectory())
		continue;
	    if (_fsv.isHiddenFile(child))
		continue;
	    String childFileName =
		    Argument.getShortFileName(child.getAbsolutePath());

	    if ((childFileName == null) || childFileName.isEmpty())
		childFileName = child.getName();

	    KeyValuePair<File> pair = new KeyValuePair<File>(childFileName,
							     child);
	    pair.set(MediaUploaderModel.ICON_PROPERTY, _fsv.getSystemIcon(child));
	    leafs.add(pair);
	}

	return leafs;
    }

}
