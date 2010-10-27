/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package n2f.mediauploader;

import java.util.ArrayList;
import java.util.List;
import org.jmock.Mockery;
import org.junit.*;
import static org.junit.Assert.*;

/**
 *
 * @author Administrator
 */
public class MemberAccountTest
{
    private static final List<String> _gettersTestData =
	    new ArrayList<String>();

    static
    {
	_gettersTestData.add("id");
	_gettersTestData.add("id2");
    }
    
    private MemberAccount _memberAccount;
    private GalleryModel _galleryModel;
    
    @Before
    public void setUp()
    {
	_galleryModel = new GalleryModel();
	_memberAccount = new MemberAccount("123", _galleryModel);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctor_NullArg0()
    {
	new MemberAccount(null, _galleryModel);
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void ctor_NullArg1()
    {
	new MemberAccount("123", null);
    }

    @Test
    public void getEncryptedMemberID_StdCall()
    {
	for (final String id : _gettersTestData)
	{
	    _memberAccount = new MemberAccount(id, _galleryModel);
	    assertEquals(id, _memberAccount.getEncryptedMemberID());
	}
    }

    @Test
    public void getGalleryModel_StdCall()
    {
	assertEquals(_galleryModel, _memberAccount.getGalleryModel());
    }
}
