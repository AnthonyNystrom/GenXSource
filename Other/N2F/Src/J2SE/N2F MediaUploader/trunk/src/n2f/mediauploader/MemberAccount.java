/* ------------------------------------------------
 * MemberAccount.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Encapsulates Next2Friends member account data.
 * 
 * @author Alex Nesterov
 */
public class MemberAccount
{
    /**
     * Creates a new instance of <tt>MemberAccount</tt> class.
     * @param	encryptedMemberID
     * @throws	IllegalArgumentException
     *		If the specified <tt>encryptedMemberID</tt> is <code>null</code>, or
     *		if the specified <tt>galleryModel</tt> is <code>null</code>.
     */
    public MemberAccount(String encryptedMemberID, GalleryModel galleryModel)
    {
	if (encryptedMemberID == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "encryptedMemberID"));
	if (galleryModel == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "galleryModel"));

	_encryptedMemberID = encryptedMemberID;
	_galleryModel = galleryModel;
    }

    private String _encryptedMemberID;

    /**
     * Returns the associated encrypted MemberID.
     * @return The associated encrypted MemberID.
     */
    public String getEncryptedMemberID()
    {
	return _encryptedMemberID;
    }

    private GalleryModel _galleryModel;

    /**
     * Returns the injected gallery model.
     * @return	The injected gallery model.
     */
    public GalleryModel getGalleryModel()
    {
	return _galleryModel;
    }

}
