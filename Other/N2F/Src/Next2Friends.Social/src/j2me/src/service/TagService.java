package service;
import java.io.IOException;

public class TagService extends BaseService
{
    public TagConfirmation[] UploadTags(String nickname, String password, TagUpdate tags) throws IOException
    {
        prepare(23);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeTagupdate(tags);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        TagConfirmation[] res = pis.readTagconfirmationArray();
        return res;
    }

}