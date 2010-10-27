using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public enum VideoStatus { Active, EncoderQueue, EncoderError,Banned, Private }
    public partial class Video
    {
        private string _TimeAgo;


        public string DurationFormated
        {
            get
            {
                return this.Duration.ToString().Replace(".", ":");
            }
        }

        public string SEOUrl
        {
            get
            {
                string FormattedTitle = RegexPatterns.FormatHTMLTitle(this.Title);

                FormattedTitle = RegexPatterns.FormatStringForURL(FormattedTitle);

                string SEOUrl = "/video/" + FormattedTitle + "/" + this.WebVideoID;

                return SEOUrl;
            }
        }


        /// <summary>
        /// formats the tags into an HTML string
        /// </summary>
        /// <returns></returns>
        public string FormattedTags()
        {
            char[] sep = { ',' };

            string[] TagsList = this.Tags.Split(sep);

            string FormatedTags = string.Empty;
            for (int i = 0; i < TagsList.Length; i++)
            {
                if (TagsList[i].Trim() != string.Empty)
                {
                    FormatedTags += "" + TagsList[i];

                    if (i < TagsList.Length-1)// dont include the comma on the last iteration
                    {
                        FormatedTags += ", ";
                    }
                }
            }

            return FormatedTags;
        }

        /// <summary>
        /// The amount of time ago that the Video first published
        /// </summary>
        public string TimeAgo
        {
            get {

                if (_TimeAgo == null)
                {
                    TimeSpan ts = DateTime.Now - this.DTCreated;


                    if ((int)ts.TotalMinutes == 0)
                    {
                        _TimeAgo = ts.Seconds + " seconds ago";
                    }
                    else if ((int)ts.TotalHours == 0)
                    {
                        _TimeAgo = ts.Minutes + " minutes ago";
                    }
                    else if ((int)ts.TotalDays == 0)
                    {
                        _TimeAgo = ts.Hours + " hours ago";
                    }
                    else if ((int)ts.TotalDays > 0)
                    {
                        _TimeAgo = ts.Days + " days ago";
                    }

                }
                return _TimeAgo;
            }
        }


        /// <summary>
        /// Saves the video file to disk
        /// </summary>
        /// <returns></returns>
        public static void QueueVideoForEncoding(Video video, Stream FLVStream,string Extension, Member member, string VideoTitle)
        {
            if (VideoTitle.Length > 35)
            {
                VideoTitle = VideoTitle.Substring(0, 35);
            }

            string VideoFileName = UniqueID.NewWebID();
            string VideoPreprocessedInputFile = OSRegistry.GetDiskUserDirectory() + member.NickName + @"\video\" + VideoFileName + "." + Extension;

            string VideoInputFile = member.NickName + @"\video\" + VideoFileName + "." + Extension;
            string VideoOutputFile = member.NickName + @"\video\" + VideoFileName + ".flv";

            int Length = 256;
            Byte[] buffer = new Byte[256];
            int bytesRead = FLVStream.Read(buffer, 0, Length);

            FileStream fs = new FileStream(VideoPreprocessedInputFile, FileMode.Create);

            // write the required bytes
            while (bytesRead > 0)
            {
                fs.Write(buffer, 0, bytesRead);
                bytesRead = FLVStream.Read(buffer, 0, Length);
            }

            FLVStream.Close();

            fs.Flush();
            fs.Close();

            ResourceFile VideoResourceFile = new ResourceFile();
            VideoResourceFile.WebResourceFileID = UniqueID.NewWebID();
            VideoResourceFile.FileName = VideoFileName + ".flv";
            VideoResourceFile.Path = @"/" + member.NickName + @"/video/";
            VideoResourceFile.ResourceType = (int)ResourceFileType.Video;
            VideoResourceFile.Save();

            string ThumbnailName = UniqueID.NewWebID() + ".jpg";
            string ThumbnailSavePath = member.NickName + @"\vthmb\" + ThumbnailName;

            ResourceFile ThumbnailResourceFile = new ResourceFile();
            ThumbnailResourceFile.WebResourceFileID = UniqueID.NewWebID();
            ThumbnailResourceFile.FileName = ThumbnailName;
            ThumbnailResourceFile.Path = member.NickName + @"/vthmb/";
            ThumbnailResourceFile.ResourceType = (int)ResourceFileType.VideoThumbnail;
            ThumbnailResourceFile.Save();

            
            video.MemberID = member.MemberID;
            video.WebVideoID = UniqueID.NewWebID();
            video.Category = 1;
            video.DTCreated = DateTime.Now;
            video.VideoResourceFileID = VideoResourceFile.ResourceFileID;
            video.ThumbnailResourceFileID = ThumbnailResourceFile.ResourceFileID;
            video.Status = (int)VideoStatus.EncoderQueue;
            video.Save();

            // update the number of photos
            MemberProfile memberProfile = member.MemberProfile[0];
            memberProfile.NumberOfVideos++;
            memberProfile.Save();

            VideoEncoderQueue VideoEncode = new VideoEncoderQueue();

            VideoEncode.VideoID = video.VideoID;
            VideoEncode.VideoInputFile = VideoInputFile;
            VideoEncode.VideoOutputFile = VideoOutputFile;
            VideoEncode.ThumbnailOutputFile = ThumbnailSavePath;
            VideoEncode.Status = (int)VideoEncoderStatus.Ready;
            VideoEncode.Save();

        }

        public static void ProcessVideo(Stream FLVStream, Member member, string VideoTitle)
        {
            if (VideoTitle.Length > 35)
            {
                VideoTitle = VideoTitle.Substring(0, 35);
            }

            string VideoFileName = UniqueID.NewWebID() + ".flv";

            string SavePath = OSRegistry.GetDiskUserDirectory() + member.NickName + @"\video\" + VideoFileName;

            int Length = 256;
            Byte[] buffer = new Byte[256];
            int bytesRead = FLVStream.Read(buffer, 0, Length);

            FileStream fs = new FileStream(SavePath, FileMode.Create);

            // write the required bytes
            while (bytesRead > 0)
            {
                fs.Write(buffer, 0, bytesRead);
                bytesRead = FLVStream.Read(buffer, 0, Length);
            }

            FLVStream.Close();

            fs.Flush();
            fs.Close();

            ResourceFile VideoResourceFile = new ResourceFile();
            VideoResourceFile.WebResourceFileID = UniqueID.NewWebID();
            VideoResourceFile.FileName = VideoFileName;
            VideoResourceFile.Path = @"/" + member.NickName + @"/video/";
            VideoResourceFile.ResourceType = (int)ResourceFileType.Video;
            VideoResourceFile.Save();


            Process FFMpegProcess;
            FFMpegProcess = new System.Diagnostics.Process();

            string ThumbnailName = UniqueID.NewWebID();
            string ThumbnailSavePath = OSRegistry.GetDiskUserDirectory() + member.NickName + @"\vthmb\";

            if (ThumbnailName.Length > 21)
            {
                ThumbnailName = ThumbnailName.Substring(0, 20);
            }

            string FullSavePath = ThumbnailSavePath + ThumbnailName;
            string arg = "-i " + SavePath + " -an -ss 00:00:07 -t 00:00:01 -r 1 -y -s 160x120 " + FullSavePath + "%d.jpg";
            string cmd = @"c:\ffmpeg.exe";

            FFMpegProcess = System.Diagnostics.Process.Start(cmd, arg);
            FFMpegProcess.WaitForExit();
            FFMpegProcess.Close();


            //ffmpeg must add a 1 to the end of the file
            ThumbnailName += "1.jpg";

            ResourceFile ThumbnailResourceFile = new ResourceFile();
            ThumbnailResourceFile.WebResourceFileID = UniqueID.NewWebID();
            ThumbnailResourceFile.FileName = ThumbnailName;
            ThumbnailResourceFile.Path = member.NickName + @"/vthmb/";
            ThumbnailResourceFile.ResourceType = (int)ResourceFileType.VideoThumbnail;
            ThumbnailResourceFile.Save();

            Video video = new Video();
            video.MemberID = member.MemberID;
            video.WebVideoID = UniqueID.NewWebID();
            video.Title = VideoTitle;
            video.Description = "No Description";
            video.DTCreated = DateTime.Now;
            video.VideoResourceFileID = VideoResourceFile.ResourceFileID;
            video.ThumbnailResourceFileID = ThumbnailResourceFile.ResourceFileID;
            video.Save();

            // update the number of photos
            MemberProfile memberProfile = member.MemberProfile[0];
            memberProfile.NumberOfVideos++;
            memberProfile.Save();

        }

        // <summary>
        /// Lawrence: This method appears to be obselete and was used as an interim solution for video encoder... 
        /// </summary>
        /// <returns></returns>
        public static void ProxyProcessVideo(Stream FLVStream, Member member, string UNCPathToUserDirectory, string VideoTitle)
        {
            if (VideoTitle.Length > 35)
            {
                VideoTitle = VideoTitle.Substring(0, 35);
            }

            string VideoFileName = UniqueID.NewWebID() + ".flv";

            string SavePath = UNCPathToUserDirectory + member.NickName + @"\video\" + VideoFileName;

            int Length = 256;
            Byte[] buffer = new Byte[256];
            int bytesRead = FLVStream.Read(buffer, 0, Length);

            FileStream fs = new FileStream(SavePath, FileMode.Create);

            // write the required bytes
            while (bytesRead > 0)
            {
                fs.Write(buffer, 0, bytesRead);
                bytesRead = FLVStream.Read(buffer, 0, Length);
            }

            FLVStream.Close();

            fs.Flush();
            fs.Close();

            ResourceFile VideoResourceFile = new ResourceFile();
            VideoResourceFile.WebResourceFileID = UniqueID.NewWebID();
            VideoResourceFile.FileName = VideoFileName;
            VideoResourceFile.Path = @"/" + member.NickName + @"/video/";
            VideoResourceFile.ResourceType = (int)ResourceFileType.Video;
            VideoResourceFile.Save();


            Process FFMpegProcess;
            FFMpegProcess = new System.Diagnostics.Process();

            string ThumbnailName = UniqueID.NewWebID();
            string ThumbnailSavePath = OSRegistry.GetDiskUserDirectory() + member.NickName + @"\vthmb\";

            if (ThumbnailName.Length > 21)
            {
                ThumbnailName = ThumbnailName.Substring(0, 20);
            }

            string FullSavePath = ThumbnailSavePath + ThumbnailName;
            string arg = "-i " + SavePath + " -an -ss 00:00:07 -t 00:00:01 -r 1 -y -s 160x120 " + FullSavePath + "%d.jpg";
            string cmd = @"c:\ffmpeg.exe";

            FFMpegProcess = System.Diagnostics.Process.Start(cmd, arg);
            FFMpegProcess.WaitForExit();
            FFMpegProcess.Close();


            //ffmpeg must add a 1 to the end of the file
            ThumbnailName += "1.jpg";

            ResourceFile ThumbnailResourceFile = new ResourceFile();
            ThumbnailResourceFile.WebResourceFileID = UniqueID.NewWebID();
            ThumbnailResourceFile.FileName = ThumbnailName;
            ThumbnailResourceFile.Path = member.NickName + @"/vthmb/";
            ThumbnailResourceFile.ResourceType = (int)ResourceFileType.VideoThumbnail;
            ThumbnailResourceFile.Save();

            Video video = new Video();
            video.MemberID = member.MemberID;
            video.WebVideoID = UniqueID.NewWebID();
            video.Title = VideoTitle;
            video.Description = "No Description";
            video.DTCreated = DateTime.Now;
            video.VideoResourceFileID = VideoResourceFile.ResourceFileID;
            video.ThumbnailResourceFileID = ThumbnailResourceFile.ResourceFileID;
            video.Save();

            // update the number of photos
            MemberProfile memberProfile = member.MemberProfile[0];
            memberProfile.NumberOfVideos++;
            memberProfile.Save();

        }
    }
}
