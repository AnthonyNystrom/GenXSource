/* ------------------------------------------------
 * DirectoryTreeNode.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.directorytree;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.TreeNode;
import n2f.mediauploader.common.KeyValuePair;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Represents a tree node for the {@link DirectoryTreeView} class.
 * @author Alex Nesterov
 */
public class DirectoryTreeNode
	extends DefaultMutableTreeNode
{
    /**
     * Creates a new instance of the <tt>DirectoryTreeNode</tt> class.
     * @param	associatedLeaf
     *		Specifies the leaf to associate with this tree node.
     * @throws	IllegalArgumentException
     *		If the specified <tt>associatedLeaf</tt> is <code>null</code>.
     */
    public DirectoryTreeNode(KeyValuePair<File> associatedLeaf)
    {
	if (associatedLeaf == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "associatedLeaf"));
	_associatedLeaf = associatedLeaf;
	setAllowsChildren(true);
	setUserObject(_associatedLeaf.key);
    }
    
    /**
     * Used to create an invisible root for <tt>DirectoryTreeNode</tt> or
     * stub nodes that are added to every child to make them expandable. Then
     * such childs are removed before a node is expanded and the parent node
     * is populated with actual items.
     * @return	New empty instance of the <tt>DirectoryTreeNode</tt> class.
     */
    public static DirectoryTreeNode createEmptyTreeNode()
    {
	return new DirectoryTreeNode(new KeyValuePair<File>("", new File("")));
    }

    private KeyValuePair<File> _associatedLeaf;

    /**
     * Gets the leaf associated with this tree node.
     * @return	The leaf associated with this tree node.
     */
    public KeyValuePair<File> getAssociatedLeaf()
    {
	return _associatedLeaf;
    }

    @Override
    public DirectoryTreeNode[] getPath()
    {
	TreeNode[] path = super.getPath();

	if (path == null)
	    return null;
	if (path.length == 0)
	    return new DirectoryTreeNode[] {};

	List<DirectoryTreeNode> nodes = new ArrayList<DirectoryTreeNode>();

	for (final TreeNode treeNode : path)
	    nodes.add((DirectoryTreeNode)treeNode);

	int nodesLength = nodes.size();
	DirectoryTreeNode[] result = new DirectoryTreeNode[nodesLength];

	for (int i = 0; i < nodesLength; i++)
	    result[i] = nodes.get(i);

	return result;
    }

}
