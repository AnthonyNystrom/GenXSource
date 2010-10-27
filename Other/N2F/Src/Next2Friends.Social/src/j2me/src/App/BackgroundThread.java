package App;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.IOException;
import java.util.Vector;
import service.*;

public class BackgroundThread implements Runnable
{
    Thread thread;
    public int commentsCount;
    public boolean getSend;
    private boolean firstRun;
//    static final String[] months = new String[]
//    {
//        "Jan", "Feb", "Mar", "Arp", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
//    };

    public BackgroundThread()
    {
        thread = new Thread(this);
        thread.start();
        thread.setPriority(Thread.MIN_PRIORITY);
        getSend = false;
        firstRun = true;
    }

    public String convertDate(String date)
    {
        String ago = null;
        int year = Integer.parseInt(date.substring(0, 4));
        int month = Integer.parseInt(date.substring(4, 6));
        int day = Integer.parseInt(date.substring(6, 8));
        int hour = Integer.parseInt(date.substring(8, 10));
        int minute = Integer.parseInt(date.substring(10, 12));
        String ampm;
        if(hour == 0)
        {
            hour = 12;
            ampm = "AM";
        }
        else if(hour > 0 && hour < 12)
        {
            ampm = "AM";
        }
        else if(hour == 12)
        {
            ampm = "PM";
        }
        else
        {
            hour -= 12;
            ampm = "PM";
        }

        String nul = "";
        if(minute < 10)
        {
            nul = "0";
        }

        ago = "" + /*months[month-1]*/month + "/" + day + "/" + year + " " + hour + ":" + nul + minute + ampm;

        return ago;
    }

    public void updateDashboard() throws IOException
    {
        //load old
        Vector oldItems = new Vector();
        byte[] oldData = Core.storage.readData("dash", 1);
        if (oldData != null)
        {
            ByteArrayInputStream is = new ByteArrayInputStream(oldData);
            DataInputStream dis = new DataInputStream(is);
            int size = is.read();
            for (int i = 0; i < size; ++i)
            {
                DashboardItem item = new DashboardItem();
                item.deserialize(dis);
                oldItems.addElement(item);
            }
            ((ScreenDashboard) Core.dashboard).dashItems = oldItems;
        }


        Vector newItems = new Vector();
        DashboardService dash = new DashboardService();
        DashboardNewFriend[] friends = dash.GetNewFriends(Core.storage.login, Core.storage.password);
        int size = friends.length;
        for (int i = 0; i < size; ++i)
        {
            DashboardItem item = new DashboardItem();
            item.iconFrame = 3;
            item.fullDate = friends[i].datetime;
            item.date = convertDate(friends[i].datetime);
            item.title = friends[i].nickname1;
            item.text = friends[i].nickname1 + " and " + friends[i].nickname2 + " are now friends!";
            newItems.addElement(item);
        }

        DashboardWallComment[] comments = dash.GetWallComments(Core.storage.login, Core.storage.password);
        size = comments.length;
        for (int i = 0; i < size; ++i)
        {
            DashboardItem item = new DashboardItem();
            item.iconFrame = 0;
            item.fullDate = comments[i].datetime;
            item.date = convertDate(comments[i].datetime);
            item.title = comments[i].nickname1;
            item.text = comments[i].nickname1 + " has written on " + comments[i].nickname2 + "'s wall: \"" + comments[i].text + "\"";
            newItems.addElement(item);
        }

        DashboardPhoto[] photos = dash.GetPhotos(Core.storage.login, Core.storage.password);
        size = photos.length;
        for (int i = 0; i < size; ++i)
        {
            DashboardItem item = new DashboardItem();
            item.iconFrame = 1;
            item.fullDate = photos[i].datetime;
            item.date = convertDate(photos[i].datetime);
            item.title = photos[i].nickname;
            item.text = photos[i].nickname + " has added a new photo '" + photos[i].title + "'. " + photos[i].text;
            newItems.addElement(item);
        }

        DashboardVideo[] videos = dash.GetVideos(Core.storage.login, Core.storage.password);
        size = videos.length;
        for (int i = 0; i < size; ++i)
        {
            DashboardItem item = new DashboardItem();
            item.iconFrame = 2;
            item.fullDate = videos[i].datetime;
            item.date = convertDate(videos[i].datetime);
            item.title = videos[i].nickname;
            item.text = videos[i].nickname + " has added a new video '" + videos[i].title + "'. " + videos[i].text;
            newItems.addElement(item);
        }

        ((ScreenDashboard) Core.dashboard).dashItems = newItems;

        //save dashboard
        ByteArrayOutputStream os = new ByteArrayOutputStream();
        size = newItems.size();
        os.write(size);
        for (int i = 0; i < size; ++i)
        {
            byte[] data = ((DashboardItem) newItems.elementAt(i)).serialize();
            os.write(data, 0, data.length);
        }
        Core.storage.writeData(os.toByteArray(), "dash", 1);

        ScreenMainmenu.dashboardCount = size;
        if (Core.screenManager.activeScreen == Core.mainMenu)
        {
            ((ScreenMainmenu) Core.mainMenu).updateCounts();
        }
    }

    public void updateInbox() throws IOException
    {
        AskService service = new AskService();
        int[] ids = service.GetQuestionIDs(Core.storage.login, Core.storage.password);
        int size = ids.length;
        ((ScreenInbox) Core.inbox).list.items.removeAllElements();
        for (int i = 0; i < size; ++i)
        {
            AskQuestion question = service.GetQuestion(Core.storage.login, Core.storage.password, ids[i]);
            ((ScreenInbox) Core.inbox).addQuestion(question.question, question.id);
        }

        ScreenMainmenu.inboxCount = size;
        if (Core.screenManager.activeScreen == Core.mainMenu)
        {
            ((ScreenMainmenu) Core.mainMenu).updateCounts();
        }
    }

    public void run()
    {
        while (true)
        {
            if (firstRun)
            {
                try
                {
                    updateDashboard();
                    updateInbox();
                } catch (IOException ex)
                {
                }
                firstRun = false;
            }
            if (getSend)
            {
                try
                {
                    updateDashboard();

                    updateInbox();

                    AskService service = new AskService();
                    //send from outbox
                    int unsentComments = Core.storage.countData("comments");
                    if (unsentComments > 0)
                    {
                        Comment com = new Comment();
                        com.read(0);
                        AskComment askComment = new AskComment();
                        askComment.askquestionid = com.questionId;
                        askComment.dtcreated = "";
                        askComment.id = 0;
                        askComment.nickname = com.login;
                        askComment.text = com.text;

                        service.AddComment(Core.storage.login, Core.storage.password, askComment);

                        com.delete(0);
                        ScreenMainmenu.outboxCount--;
                        if (Core.screenManager.activeScreen == Core.mainMenu)
                        {
                            ((ScreenMainmenu) Core.mainMenu).updateCounts();
                        }
                    }

                    int unsentOutbox = Core.storage.countData("outbox");
                    if (unsentOutbox > 0)
                    {
                        Question qu = new Question();
                        qu.read(false, 0);

                        int photoCount = qu.photoNames.size();
                        String[] customResponses = new String[]
                        {
                            qu.responseA, qu.responseB
                        };
                        AskQuestionConfirm confirm = service.SubmitQuestion(Core.storage.login,
                                Core.storage.password, qu.question, photoCount,
                                qu.responseType, customResponses, qu.duration, qu.isPrivate);
                        String questionId = confirm.askquestionid;

                        for (int i = 0; i < photoCount; ++i)
                        {
                            byte[] photo = Core.storage.readBytes((String) qu.photoNames.elementAt(i));
                            service.AttachPhoto(Core.storage.login, Core.storage.password, questionId, i + 1, photo);
                        }

                        service.CompleteQuestion(Core.storage.login, Core.storage.password, questionId);
                        qu.delete(false, 0);

                        ScreenMainmenu.outboxCount--;
                        if (Core.screenManager.activeScreen == Core.mainMenu)
                        {
                            ((ScreenMainmenu) Core.mainMenu).updateCounts();
                        }
                    }
                    
                    int unsentUpload = Core.storage.countData("upload");
                    if(unsentUpload > 0)
                    {
                        UploadedImage image = new UploadedImage();
                        image.read(0);
                        byte[] photo = Core.storage.readBytes(image.photoName);
                        SnapUpService snapService = new SnapUpService();
                        snapService.DeviceUploadPhoto(Core.storage.login, Core.storage.password, photo, "");
                        
                        UploadedImage.delete();
                        ScreenMainmenu.outboxCount--;
                        if (Core.screenManager.activeScreen == Core.mainMenu)
                        {
                            ((ScreenMainmenu) Core.mainMenu).updateCounts();
                        }
                    }                    

                    if(unsentComments + unsentOutbox + unsentUpload == 0)
                    {
                        getSend = false;
                    }
                } catch (IOException ex)
                {
                    Core.message = "Network error";
                }
            }
            try
            {
                Thread.sleep(Const.NETWORK_SLEEP);
            } catch (Exception ex)
            {
            };
        }
    }
}
