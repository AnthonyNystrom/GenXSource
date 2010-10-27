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
    public class TurbineEncode
    {
        public TurbineEncode()
        {
            Thread t = new Thread(new ThreadStart(StartEncodeProcess));
            t.Start();

            //   try
            //    {
            //StartEncodeProcess();
            //   }
            //                       catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());


        }




        public void StartEncodeProcess()
        {
            // Threads to get new videos from the VideoProcess table check for 
            while (true)
            {
                try
                {
                    VideoEncoderQueue videoEnc = VideoEncoderQueue.GetNextVideoToEncode();

                    if (videoEnc != null)
                    {

                        //AddEvent("About to Encode Video " + videoEnc.VideoInputFile);
                        EncodeNextVideo(videoEnc);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        public void AddEvent(string Event)
        {
            //Form1.txtEvents.Text += Event + "\r\n";
        }

        public void EncodeNextVideo(VideoEncoderQueue videoEnc)
        {
            Video video = null;

            try
            {
                video = new Video(videoEnc.VideoID);

                DateTime Start = DateTime.Now;

                Console.WriteLine("Begin encoding");
                string UserDirectory = @"\\www\user\";
                //string UserDirectory = @"d:\Source3\Web\user\";

                string InputFileName = UserDirectory + videoEnc.VideoInputFile;
                string OutputFileName = UserDirectory + videoEnc.VideoOutputFile;
                string ThumbnailLocation = UserDirectory + videoEnc.ThumbnailOutputFile;

                Turbine.TVE3 tve = new Turbine.TVE3();

                // set license keys:
                tve.Key1 = 215362640;
                tve.Key2 = 1596049317;

                // load the base preset settings:
                tve.LoadSettings(@"c:\presets\300K_Broadband.settings");

                tve.VideoMethod = "VBR2";

                tve.VideoBitRate = 300000;
                // set output type (flv or swf) and output location:
                tve.OutputFormat = "fla";
                tve.SetOutputFile(OutputFileName);

                // encode source synchronously:
                tve.EncodeAsync(InputFileName);

                DateTime encStart = DateTime.Now;
                bool EncodeOKAY = true;
                while (tve.EncodeAsyncIsEncoding && EncodeOKAY)
                {
                    Console.WriteLine(tve.EncodeAsyncPercentage.ToString() + "% Complete");
                    Application.DoEvents();

                    if (encStart > DateTime.Now.AddMinutes(1) && tve.EncodeAsyncPercentage == 1)
                    {
                        EncodeOKAY = false;
                    }
                    Thread.Sleep(300);
                }

                // done - flush encoding:
                tve.EncodeFlush();

                tve.InfoOpen(InputFileName);

                decimal Duration = 0.0M;

                try
                {
                    int DurationMS = (int)tve.InfoGet("totalDurationMs");

                    TimeSpan DurationSpan = new TimeSpan(0, 0, 0, 0, DurationMS);

                    Duration = decimal.Parse(DurationSpan.Minutes + "." + DurationSpan.Seconds);
                }
                catch { }

                tve.InfoSaveInputFrame(2000, ThumbnailLocation, 100);

                Photo.ResizeTo124x91(videoEnc.ThumbnailOutputFile);

                // open video again to make sure no changes have been missed
                video = new Video(videoEnc.VideoID);

                if (EncodeOKAY)
                {
                    videoEnc.Status = (int)VideoEncoderStatus.Completed;

                    video.Status = (int)VideoStatus.Active;
                }
                else
                {

                    videoEnc.Status = (int)VideoEncoderStatus.Errored;
                    videoEnc.Error = "Encoder Stalled";
                    video.Status = (int)VideoStatus.EncoderError;
                }

                video.Duration = Convert.ToDecimal(Duration);

                DateTime End = DateTime.Now;
                TimeSpan EncodingTime = End - Start;

                Console.WriteLine("Successfully encoded " + Duration.ToString() + " in " + EncodingTime.ToString());

                tve.Dispose();

                videoEnc.Save();
                video.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                videoEnc.Status = (int)VideoEncoderStatus.Errored;
                videoEnc.Error = ex.ToString();


                videoEnc.Save();

                if (video != null)
                {
                    video.Status = (int)VideoStatus.EncoderError;
                    video.Save();
                }
            }
            finally
            {

            }


            Console.WriteLine("Finished Encoding");

        }
    }
}
