package tag;
import java.util.Vector;
import service.*;

public class TagStorage
{
    private Vector valStrings;
    private Vector tagIds;
    public int tagsCount;

    public TagStorage()
    {
	valStrings = new Vector();
	tagIds = new Vector();
    }
    
    public void add(String valString, String tagId)
    {
	valStrings.addElement(valString);
	tagIds.addElement(tagId);
	tagsCount++;
    }
    
    public TagUpdate getTagUpdate()
    {
	TagUpdate tagUpdate = new TagUpdate();
	
	tagUpdate.tagvalidationstring = new String[tagsCount];
	for(int i = 0; i < tagsCount; ++i)
	{
	    tagUpdate.tagvalidationstring[i] = (String)valStrings.elementAt(i);
	}

	tagUpdate.devicetagid = new String[tagsCount];
	for(int i = 0; i < tagsCount; ++i)
	{
	    tagUpdate.devicetagid[i] = (String)tagIds.elementAt(i);
	}
	
	return tagUpdate;
    }
}
