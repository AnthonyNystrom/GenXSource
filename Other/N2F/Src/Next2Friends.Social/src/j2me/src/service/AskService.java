package service;
import java.io.IOException;

public class AskService extends BaseService
{
    public int AddComment(String nickname, String password, AskComment newComment) throws IOException
    {
        prepare(6);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeAskcomment(newComment);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        int res = pis.readInt();
        return res;
    }

    public void AttachPhoto(String nickname, String password, String askQuestionID, int indexOrder, byte[] photoBase64String) throws IOException
    {
        prepare(7);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeString(askQuestionID);
        pos.writeInt(indexOrder);
        pos.writeByteArray(photoBase64String);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
    }

    public void CompleteQuestion(String nickname, String password, String askQuestionID) throws IOException
    {
        prepare(8);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeString(askQuestionID);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
    }

    public AskComment GetComment(String nickname, String password, int commentID) throws IOException
    {
        prepare(9);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(commentID);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        AskComment res = pis.readAskcomment();
        return res;
    }

    public int[] GetCommentIDs(String nickname, String password, int questionID, int lastCommentID) throws IOException
    {
        prepare(10);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(questionID);
        pos.writeInt(lastCommentID);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        int[] res = pis.readIntArray();
        return res;
    }

    public AskQuestion GetQuestion(String nickname, String password, int questionID) throws IOException
    {
        prepare(11);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(questionID);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        AskQuestion res = pis.readAskquestion();
        return res;
    }

    public int[] GetQuestionIDs(String nickname, String password) throws IOException
    {
        prepare(12);

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
  
        int[] res = pis.readIntArray();
        return res;
    }

    public AskResponse GetResponse(String nickname, String password, int questionID) throws IOException
    {
        prepare(13);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(questionID);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        AskResponse res = pis.readAskresponse();
        return res;
    }

    public boolean HasNewComments(String nickname, String password, int questionID, int lastCommentID) throws IOException
    {
        prepare(14);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(questionID);
        pos.writeInt(lastCommentID);

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

    public void SkipQuestion(String nickname, String password, int questionID) throws IOException
    {
        prepare(15);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(questionID);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
    }

    public AskQuestionConfirm SubmitQuestion(String nickname, String password, String questionText, int numberOfPhotos, int responseType, String[] customResponses, int durationType, boolean isPrivate) throws IOException
    {
        prepare(16);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeString(questionText);
        pos.writeInt(numberOfPhotos);
        pos.writeInt(responseType);
        pos.writeStringArray(customResponses);
        pos.writeInt(durationType);
        pos.writeBoolean(isPrivate);

        try
        {
            commit();
        }
        catch(IOException ex)
        {
            throw ex;
        }
  
        AskQuestionConfirm res = pis.readAskquestionconfirm();
        return res;
    }

    public void VoteForQuestion(String nickname, String password, int questionID, int result) throws IOException
    {
        prepare(17);

        pos.writeString(nickname);
        pos.writeString(password);
        pos.writeInt(questionID);
        pos.writeInt(result);

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