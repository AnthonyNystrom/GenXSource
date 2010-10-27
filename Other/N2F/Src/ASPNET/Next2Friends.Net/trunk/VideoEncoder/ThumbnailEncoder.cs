using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Next2Friends.Data;


namespace VideoEncoder
{
    public class ThumbnailEncoder
    {
        public ThumbnailEncoder()
        {
            Thread t = new Thread(new ThreadStart(StartEncodeProcess));
            t.Start();
        }

        public void StartEncodeProcess()
        {
            // Threads to get new videos from the VideoProcess table check for 
            while (true)
            {
                try
                {
                    LiveThumbnailEncoderQueue ThumbEnc = LiveThumbnailEncoderQueue.GetNextThumbnailToEncode();

                    if (ThumbEnc != null)
                    {
                        //EncodeNextThumb(ThumbEnc);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        //public void EncodeNextThumb(LiveThumbnailEncoderQueue ThumbEnc)
        //{
        //    Video video = null;

        //    try
        //    {
        //        video = new Video(ThumbEnc.VideoID);

        //        DateTime Start = DateTime.Now;

        //        Console.WriteLine("Begin creating thumb");
        //        string UserDirectory = @"\\www\user\";

        //        string VideoInputFileName = UserDirectory + ThumbEnc.VideoInputFile;
        //        string ThumbnailOutputFile = UserDirectory + ThumbEnc.ThumbnailOutputFile;

        //        Turbine.TVE3 tve = new Turbine.TVE3();

        //        // set license keys:
        //        tve.Key1 = 215362640;
        //        tve.Key2 = 1596049317;

        //        if (System.IO.File.Exists(VideoInputFileName))
        //        {
        //            tve.InfoOpen(VideoInputFileName);

        //            tve.InfoSaveInputFrame(2000, ThumbnailOutputFile, 100);

        //            // create a Resourcefile
        //            ResourceFile PhotoResourceFile = new ResourceFile();
        //            PhotoResourceFile.WebResourceFileID = Next2Friends.Misc.UniqueID.NewWebID();
        //            PhotoResourceFile.ResourceType = (int)ResourceFileType.VideoThumbnail;
        //            PhotoResourceFile.Path = ThumbnailOutputFile;
        //            PhotoResourceFile.FileName = ThumbnailOutputFile;
        //            PhotoResourceFile.Save();

        //            video.ThumbnailResourceFileID = PhotoResourceFile.ResourceFileID;

        //            Console.WriteLine("Successfully created thumbnail");
        //        }
        //        else
        //        {
        //            ThumbEnc.Status = (int)ThumbnailEncoderStatus.VideoFileNotFound;
        //            Console.WriteLine("input video not found");
        //        }

        //        tve.Dispose();

        //        ThumbEnc.Save();

        //        video.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        ThumbEnc.Status = (int)ThumbnailEncoderStatus.Errored;
        //        ThumbEnc.Error = ex.ToString();

        //        ThumbEnc.Save();

        //        if (video != null)
        //        {
        //            video.Status = (int)ThumbnailEncoderStatus.Errored;
        //            video.Save();
        //        }
        //    }
        //    finally
        //    {

        //    }

        //    Console.WriteLine("Finished Encoding");
        //}
    }
}
