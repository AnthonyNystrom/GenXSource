/* ------------------------------------------------
 * DirectoryTreeView.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.directorytree;

import java.awt.Color;
import java.io.File;
import java.util.LinkedList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.swing.ImageIcon;
import javax.swing.JTree;
import javax.swing.UIManager;
import javax.swing.event.TreeExpansionEvent;
import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;
import javax.swing.event.TreeWillExpandListener;
import javax.swing.tree.ExpandVetoException;
import javax.swing.tree.TreeModel;
import javax.swing.tree.TreeSelectionModel;
import n2f.mediauploader.common.KeyValuePair;
import static n2f.mediauploader.resources.AppletResources.*;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
public class DirectoryTreeView
	extends JTree
{
    private static Logger _logger =
	    Logger.getLogger(DirectoryTreeView.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private class DirectoryTreeWillExpandListener
	    implements TreeWillExpandListener
    {
	public void treeWillCollapse(TreeExpansionEvent e) throws ExpandVetoException
	{
	}

	/** 
	 * We remove a stub empty item on <tt>lastTreeNode</tt> and populate
	 * it with actual items.
	 */
	public void treeWillExpand(TreeExpansionEvent e) throws ExpandVetoException
	{
	    DirectoryTreeNode lastTreeNode = (DirectoryTreeNode)e.getPath().
		    getLastPathComponent();
	    _logger.log(Level.INFO, "{0} expanded.", lastTreeNode);
	    lastTreeNode.removeAllChildren();

	    DirectoryTreeNode[] lastTreeNodePath = lastTreeNode.getPath();

	    for (final KeyValuePair<File> path : DirectoryTreeModel.getPathChoices(lastTreeNodePath))
	    {
		DirectoryTreeNode newTreeNode = new DirectoryTreeNode(path);
		newTreeNode.add(DirectoryTreeNode.createEmptyTreeNode());
		lastTreeNode.add(newTreeNode);
	    }
	}

    }

    /** 
     * Retrieve file list on directory selection and notify the listeners.
     */
    private class DirectoryTreeSelectionListener
	    implements TreeSelectionListener
    {
	public void valueChanged(TreeSelectionEvent e)
	{
	    DirectoryTreeNode lastTreeNode = (DirectoryTreeNode)e.getPath().
		    getLastPathComponent();
	    List<KeyValuePair<File>> leafs = DirectoryTreeModel.getLeafs(lastTreeNode.getPath());
	    
	    StringBuffer leafsBuffer = new StringBuffer();
	    for (final KeyValuePair<File> leaf : leafs)
		leafsBuffer.append(" --> ").append(leaf.key);
	    
	    _logger.log(Level.INFO, "leafs = {0}", leafsBuffer);
	    invokeDirectoryTreeListeners(leafs);
	}

    }

    static
    {
	UIManager.put("Tree.collapsedIcon", new ImageIcon(TreeCollapsed));
	UIManager.put("Tree.expandedIcon", new ImageIcon(TreeExpanded));
	UIManager.put("Tree.hash", new Color(128, 128, 128));
	UIManager.put("Tree.lineTypeDashed", true);
    }

    private TreeModel _model;

    /**
     * Creates a new instance of the <tt>DirectoryTreeView</tt> class.
     */
    public DirectoryTreeView()
    {
	_model = new DirectoryTreeModel();

	addTreeSelectionListener(new DirectoryTreeSelectionListener());
	addTreeWillExpandListener(new DirectoryTreeWillExpandListener());
	getSelectionModel().setSelectionMode(TreeSelectionModel.SINGLE_TREE_SELECTION);
	setCellRenderer(new DirectoryTreeRenderer());
	setExpandsSelectedPaths(true);
	setModel(_model);
	setRootVisible(false);
	setScrollsOnExpand(true);
    }

    private List<IDirectoryTreeListener> _directoryTreeListeners;
    
    private List<IDirectoryTreeListener> getDirectoryTreeListeners()
    {
	if (_directoryTreeListeners == null)
	    _directoryTreeListeners = new LinkedList<IDirectoryTreeListener>();
	return _directoryTreeListeners;
    }
    
    /**
     * Adds the specified listener to receive <tt>DirectoryTreeView</tt> related
     * events.
     * @param	l
     *		Specifies the <tt>DirectoryTreeView</tt> related event listener.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addDirectoryTreeListener(IDirectoryTreeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "l"));

	getDirectoryTreeListeners().add(l);
    }
    
    /**
     * Removes the specified listener so that it no longer receives 
     * <tt>DirectoryTreeView</tt> related events.
     * @param	l
     *		Specifies the <tt>DirectoryTreeView</tt> related event listener
     *		to remove.
     */
    public void removeDirectoryTreeListener(IDirectoryTreeListener l)
    {
	getDirectoryTreeListeners().remove(l);
    }
    
    private void invokeDirectoryTreeListeners(List<KeyValuePair<File>> leafs)
    {
	for (final IDirectoryTreeListener l : getDirectoryTreeListeners())
	    l.pathChanged(leafs);
    }
}
