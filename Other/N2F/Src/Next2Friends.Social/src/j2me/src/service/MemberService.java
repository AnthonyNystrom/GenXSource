package service;
import java.io.IOException;

public class MemberService extends BaseService
{
    public boolean CheckUserExists(String nickname, String password) throws IOException
    {
        prepare(1);

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
  
        boolean res = pis.readBoolean();
        return res;
    }

    public String GetEncryptionKey(String nickname, String password) throws IOException
    {
        prepare(2);

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
  
        String res = pis.readString();
        return res;
    }

    public String GetMemberID(String nickname, String password) throws IOException
    {
        prepare(3);

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
  
        String res = pis.readString();
        return res;
    }

    public String GetTagID(String nickname, String password) throws IOException
    {
        prepare(4);

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
  
        String res = pis.readString();
        return res;
    }

    public void RemindPassword(String emailAddress) throws IOException
    {
        prepare(5);

        pos.writeString(emailAddress);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
    }
    
    public void SetMemberStatusText(String nickname, String password, String statusText) throws IOException
    {
        prepare(24);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeString(statusText);
        
        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
    }
    
    public String GetMemberStatusText(String nickname, String password) throws IOException
    {
        prepare(25);
        
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
        
        return pis.readString();
    }
}