/* ------------------------------------------------
 * IDirectoryTreeListener.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.directorytree;

import java.io.File;
import java.util.List;
import n2f.mediauploader.common.KeyValuePair;

/**
 * Indicates that the implementors will receive notifications when a new path
 * is selected on a {@link DirectoryTreeView} instance.
 * @author Alex Nesterov
 */
public interface IDirectoryTreeListener
{
    /** 
     * Invoked when the path on the <tt>DirectoryTreeView</tt> changes and a new
     * file list has been retrieved.
     * @param	files
     *		A list of files for the new path.
     */
    void pathChanged(List<KeyValuePair<File>> files);
}
