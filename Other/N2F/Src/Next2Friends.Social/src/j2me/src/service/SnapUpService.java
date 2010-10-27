package service;
import java.io.IOException;

public class SnapUpService extends BaseService
{
    public void DeviceUploadPhoto(String nickname, String password, byte[] photoBase64String, String dateTime) throws IOException
    {
        prepare(22);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeByteArray(photoBase64String);
        pos.writeString(dateTime);
        
        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
    }
}
