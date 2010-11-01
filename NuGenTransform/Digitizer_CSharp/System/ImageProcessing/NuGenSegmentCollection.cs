using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{
    //
    //  Encapsulates the behavior of a set of segments within a document
    //
    //  Isolates the operations on a series of segments within one class    
    public class NuGenSegmentCollection
    {
        private List<NuGenSegment> segments = new List<NuGenSegment>();

        public List<NuGenSegment> Segments
        {
            get
            {
                return segments;
            }
        }

        int AdjacentRuns(bool[] columnBool, int yStart, int yStop, int height)
        {
          int runs = 0;
          bool inRun = false;
          for (int y = yStart - 1; y <= yStop + 1; y++)
          {
            if ((0 <= y) && (y < height))
            {
              if (!inRun && columnBool [y])
              {
                inRun = true;
                ++runs;
              }
              else if (inRun && !columnBool [y])
                inRun = false;
            }
          }

          return runs;
        }

        NuGenSegment AdjacentSegment(NuGenSegment[] lastSegment, int yStart, int yStop, int height)
        {
          for (int y = yStart - 1; y <= yStop + 1; y++)
            if ((0 <= y) && (y < height))
              if (lastSegment [y] != null)
                return lastSegment [y];

          return null;
        }

        int AdjacentSegments(NuGenSegment[] lastSegment, int yStart, int yStop, int height)
        {
          int segments = 0;
          bool inSegment = false;
          for (int y = yStart - 1; y <= yStop + 1; y++)
          {
            if ((0 <= y) && (y < height))
            {
              if (!inSegment && (lastSegment[y] != null))
              {
                inSegment = true;
                ++segments;
              }
              else if (inSegment && (lastSegment[y] == null))
                inSegment = false;
            }
          }

          return segments;
        }

        List<NuGenPoint> FillPoints(SegmentSettings seg)
        {
          List<NuGenPoint> list = new List<NuGenPoint>();
          
          foreach(NuGenSegment segment in segments)
          {
            list.AddRange(segment.FillPoints(seg));
          }

          return list;
        }
            
        void FinishRun(bool[] lastBool, bool[] nextBool,
          NuGenSegment[] lastSegment, NuGenSegment[] currSegment,
          int x, int yStart, int yStop, int height, SegmentSettings set)
        {
          // when looking at adjacent columns, include pixels that touch diagonally since
          // those may also diagonally touch nearby runs in the same column (which would indicate
          // a branch)
          
          // count runs that touch on the left
          if (AdjacentRuns(lastBool, yStart, yStop, height) > 1)
            return;
          // count runs that touch on the right
          if (AdjacentRuns(nextBool, yStart, yStop, height) > 1)
            return;
          
          NuGenSegment seg;
          if (AdjacentSegments(lastSegment, yStart, yStop, height) == 0)
          {
            // this is the start of a new segment
            seg = new NuGenSegment((int) (0.5 + (yStart + yStop) / 2.0));

            segments.Add(seg);
          }
          else
          {
            // this is the continuation of an existing segment
            seg = AdjacentSegment(lastSegment, yStart, yStop, height);

            seg.AppendColumn(x, (int) (0.5 + (yStart + yStop) / 2.0), set);
          }

          for (int y = yStart; y <= yStop; y++)
            currSegment [y] = seg;
        }

        void LoadBool(bool[] columnBool,
          BitmapData bmData, int x)
        {
          for (int y = 0; y < bmData.Height; y++)
              if (x < 0)
                  columnBool[y] = false;
              else
              {
                  columnBool[y] = NuGenDiscretize.ProcessedPixelIsOn(bmData, x, y);
              }
        }

        void LoadSegment(NuGenSegment[] columnSegment, int height)
        {
          for (int y = 0; y < height; y++)
            columnSegment [y] = null;
        }

        public void MakeSegments(Image imageProcessed, SegmentSettings seg)
        {
            segments.Clear();

          // for each new column of pixels, loop through the runs. a run is defined as
          // one or more colored pixels that are all touching, with one uncolored pixel or the
          // image boundary at each end of the set. for each set in the current column, count
          // the number of runs it touches in the adjacent (left and right) columns. here is
          // the pseudocode:
          //   if ((L > 1) || (R > 1))
          //     "this run is at a branch point so ignore the set"
          //   else
          //     if (L == 0)
          //       "this run is the start of a new segment"
          //     else
          //       "this run is appended to the segment on the left
          int width = imageProcessed.Width;
          int height = imageProcessed.Height;

          bool[] lastBool = new bool [height];          
          bool[] currBool = new bool [height];          
          bool[] nextBool = new bool [height];          
          NuGenSegment[] lastSegment = new NuGenSegment [height];
          NuGenSegment[] currSegment = new NuGenSegment [height];

          Bitmap b = new Bitmap(imageProcessed);
          BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);

          LoadBool(lastBool, bmData, -1);
          LoadBool(currBool, bmData, 0);
          LoadBool(nextBool, bmData, 1);
          LoadSegment(lastSegment, height);

          for (int x = 0; x < width; x++)
          {          

            MatchRunsToSegments(x, height, lastBool, lastSegment, currBool, currSegment, nextBool, seg);

            // get ready for next column
            ScrollBool(lastBool, currBool, height);
            ScrollBool(currBool, nextBool, height);
            if (x + 1 < width)
              LoadBool(nextBool, bmData, x + 1);
            ScrollSegment(lastSegment, currSegment, height);
          }

          b.UnlockBits(bmData);
        }

        void MatchRunsToSegments(int x, int height, bool[] lastBool, NuGenSegment[] lastSegment,
          bool[] currBool, NuGenSegment[] currSegment, bool[] nextBool, SegmentSettings seg)
        {
          LoadSegment(currSegment, height);

          int yStart = 0;
          bool inRun = false;
          for (int y = 0; y < height; y++)
          {
            if (!inRun && currBool [y])
            {
              inRun = true;
              yStart = y;
            }

            if ((y + 1 >= height) || !currBool [y + 1])
            {
              if (inRun)
                FinishRun(lastBool, nextBool, lastSegment, currSegment, x, yStart, y, height, seg);

              inRun = false;
            }
          }

          RemoveUnneededLines(lastSegment, currSegment, height, seg);
        }

        void RemoveUnneededLines(NuGenSegment[] lastSegment, NuGenSegment[] currSegment, int height, SegmentSettings seg)
        {
          NuGenSegment segLast = null;
          for (int yLast = 0; yLast < height; yLast++)
          {
            if ((lastSegment[yLast] != null) && (lastSegment [yLast] != segLast))
            {
              segLast = lastSegment [yLast];

              // if the segment is found in the current column then it is still in work so postpone processing
              bool found = false;
              for (int yCur = 0; yCur < height; yCur++)
                if (segLast == currSegment [yCur])
                {
                  found = true;
                  break;
                }

              if (!found)
              {
                if (segLast.Length < (seg.minPoints - 1) * seg.pointSeparation)
                {
                  segments.Remove(segLast); // autoDelete is on
                }
                else
                  // keep segment, but try to fold lines
                  segLast.RemoveUnneededLines();
              }
            }
          }
        }

        void ScrollBool(bool[] left, bool[] right, int height)
        {
          for (int y = 0; y < height; y++)
            left [y] = right [y];
        }

        void ScrollSegment(NuGenSegment[] left, NuGenSegment[] right, int height)
        {
          for (int y = 0; y < height; y++)
            left [y] = right [y];
        }
    }
}
