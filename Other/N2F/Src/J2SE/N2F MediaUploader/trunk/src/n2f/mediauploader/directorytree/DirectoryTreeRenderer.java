/* ------------------------------------------------
 * DirectoryTreeRenderer.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.directorytree;

import java.awt.Component;
import java.io.File;
import javax.swing.Icon;
import javax.swing.JTree;
import javax.swing.tree.DefaultTreeCellRenderer;
import n2f.mediauploader.MediaUploaderModel;
import n2f.mediauploader.common.KeyValuePair;

/**
 * Implements custom rendering for the <tt>DirectoryTreeView</tt>. Sets
 * system icons for all elements.
 * 
 * @author Alex Nesterov
 */
public class DirectoryTreeRenderer
	extends DefaultTreeCellRenderer
{
    @Override
    public Component getTreeCellRendererComponent(
	    JTree tree,
	    Object value,
	    boolean sel,
	    boolean expanded,
	    boolean leaf,
	    int row,
	    boolean hasFocus)
    {
	super.getTreeCellRendererComponent(tree,
					   value,
					   sel,
					   expanded,
					   leaf,
					   row,
					   hasFocus);

	if (value instanceof DirectoryTreeNode)
	{
	    DirectoryTreeNode treeNode = (DirectoryTreeNode)value;
	    KeyValuePair<File> associatedLeaf = treeNode.getAssociatedLeaf();
	    setIcon((Icon)associatedLeaf.get(MediaUploaderModel.ICON_PROPERTY));
	}

	return this;
    }

}
