package service;
import java.io.IOException;

public class DashboardService extends BaseService
{
    public DashboardNewFriend[] GetNewFriends(String nickname, String password) throws IOException
    {
        prepare(18);

        pos.writeString(nickname);
        pos.writeString(password);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        DashboardNewFriend[] res = pis.readDashboardnewfriendArray();
        return res;
    }

    public DashboardPhoto[] GetPhotos(String nickname, String password) throws IOException
    {
        prepare(19);

        pos.writeString(nickname);
        pos.writeString(password);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        DashboardPhoto[] res = pis.readDashboardphotoArray();
        return res;
    }

    public DashboardVideo[] GetVideos(String nickname, String password) throws IOException
    {
        prepare(20);

        pos.writeString(nickname);
        pos.writeString(password);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        DashboardVideo[] res = pis.readDashboardvideoArray();
        return res;
    }

    public DashboardWallComment[] GetWallComments(String nickname, String password) throws IOException
    {
        prepare(21);

        pos.writeString(nickname);
        pos.writeString(password);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        DashboardWallComment[] res = pis.readDashboardwallcommentArray();
        return res;
    }

}