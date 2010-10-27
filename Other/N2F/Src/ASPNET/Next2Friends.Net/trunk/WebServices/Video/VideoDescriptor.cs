using System;
using System.Collections.Generic;
using System.Drawing;

namespace Next2Friends.WebServices.Video
{
    public sealed class VideoDescriptor
    {
        public string WebVideoID { get; set; }
        public string Title { get; set; }
        public string ThumbnailBase64String { get; set; }
        public string DateTime { get; set; }

        public static VideoDescriptor[] CreateWSVideos(List<Next2Friends.Data.Video> Videos)
        {
            int NumberOfVideos = (Videos.Count >= 10) ? 10 : Videos.Count;

            VideoDescriptor[] WSVideos = new VideoDescriptor[NumberOfVideos];

            for (int i = 0; i < Videos.Count; i++)
            {
                if (i == 10)
                {
                    break;
                }

                WSVideos[i] = new VideoDescriptor();
                WSVideos[i].WebVideoID = Videos[i].WebVideoID;
                WSVideos[i].Title = Videos[i].Title;
                Image Thumbnail;
                try
                {
                    string Path = Videos[i].ThumbnailResourceFile.GetDiskPath();
                    Thumbnail = new Bitmap(Path);
                }
                catch (Exception) { }

                Thumbnail = new Bitmap(121, 91);
                WSVideos[i].ThumbnailBase64String = Next2Friends.Data.Photo.ImageToBase64String(Thumbnail);
                WSVideos[i].DateTime = Videos[i].DTCreated.Ticks.ToString();
            }

            return WSVideos;
        }
    }
}
