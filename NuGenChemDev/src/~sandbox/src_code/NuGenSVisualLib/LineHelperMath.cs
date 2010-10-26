using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace NuGenSVisualLib
{
    public class LineHelperMath
    {
        /// <summary>
        /// Computes a set of lines to form a set of dashes in between 2 points
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="dashSize"></param>
        /// <param name="lines"></param>
        public static void CalcDashedLineLinear(Vector3 pointA, Vector3 pointB, float dashSize,
                                                out Vector3[] lines)
        {
            Vector3 distUV = pointB - pointA;
            // calc num of lines (dashes)
            float length = distUV.Length();
            distUV.Normalize();
            float numLinesAb = length / (dashSize * 2f);
            int numLines = (int)numLinesAb;
            // round up if needed
            if (numLines != numLinesAb)
                numLines++;

            lines = new Vector3[numLines * 2];

            // calc each dash/line
            int lineIdx = 0;
            for (int line = 0; line < numLines; line++)
            {
                float startLen = line * dashSize * 2f;
                Vector3 startPos = pointA + (distUV * startLen);
                Vector3 endPos = startPos + (distUV * dashSize);

                // clip if needed
                if (line == numLines - 1 && startLen + dashSize > length)
                    endPos = startPos + (distUV * (length - startLen));

                lines[lineIdx++] = startPos;
                lines[lineIdx++] = endPos;
            }
        }

        /// <summary>
        /// Computes a set of lines to form a set of dashes in between 2 points, including where
        /// on the line any breaks should be forced, in effect spliting the dashes at certain points.
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <param name="dashSize"></param>
        /// <param name="breaks"></param>
        /// <param name="lines"></param>
        /// <param name="lineSections"></param>
        public static void CalcDashedLineSections(Vector3 pointA, Vector3 pointB, float dashSize,
                                                  float[] breaks, out Vector3[] lines,
                                                  out int[] lineSections)
        {
            // get normal lines
            Vector3[] oLines;
            CalcDashedLineLinear(pointA, pointB, dashSize, out oLines);
            int numLines = (int)(oLines.Length / 2f);

            // calc how many breaks req splits
            float[] splits = new float[numLines];
            bool[] changes = new bool[numLines];
            int numSplits = 0;
            for (int brk = 0; brk < breaks.Length; brk++)
            {
                float lineArea = breaks[brk] / dashSize;
                int line = (int)lineArea;
                if (lineArea < (float)line + 0.5f && lineArea != (float)line)
                {
                    numSplits++;
                    // NOTE: assume 1 break max per line
                    splits[line] = lineArea - (float)line;
                }
                else
                {
                    changes[line] = true;
                }
            }

            lines = new Vector3[oLines.Length + (numSplits * 2)];
            lineSections = new int[numLines + numSplits];

            // copy & split lines for final lines
            int lineIdx = 0;
            int oLineIdx = 0;
            int sectIdx = 0;
            int fLineIdx = 0;
            for (int line = 0; line < numLines; line++)
            {
                if (splits[line] != 0)
                {
                    // split line
                    lines[lineIdx++] = oLines[oLineIdx++];
                    Vector3 lineDist = oLines[oLineIdx] - oLines[oLineIdx - 1];
                    Vector3 lineUV = Vector3.Normalize(lineDist);
                    Vector3 brkPoint = oLines[oLineIdx - 1] + (lineUV * splits[line]);

                    lines[lineIdx++] = brkPoint;
                    lineSections[fLineIdx++] = sectIdx;
                    sectIdx++;

                    lines[lineIdx++] = brkPoint;
                    lines[lineIdx++] = oLines[oLineIdx++];
                    lineSections[fLineIdx++] = sectIdx;
                }
                else
                {
                    if (changes[line])
                        sectIdx++;

                    // just copy line
                    lines[lineIdx++] = oLines[oLineIdx++];
                    lines[lineIdx++] = oLines[oLineIdx++];
                    lineSections[fLineIdx++] = sectIdx;
                }
            }
        }
    }
}