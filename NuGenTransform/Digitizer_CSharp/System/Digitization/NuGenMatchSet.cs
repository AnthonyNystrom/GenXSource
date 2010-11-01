using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenTransform
{
    public class MatchPoint
    {
        Point point;
        bool accepted;
        bool visible = false;

        public MatchPoint(Point p, bool a)
        {
            point = p;
            accepted = a;
        }

        public Point Point
        {
            get
            {
                return point;
            }
        }

        public bool Accepted
        {
            get
            {
                return accepted;
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
        }

        public void Accept()
        {
            accepted = true;
            visible = true;
        }

        public void Reject()
        {
            accepted = false;
            visible = true;
        }
    }

    //Used for point matching, holds a list of match points and allows
    // user iteration through them, accepting, or rejecting the points as they advance.
    public class NuGenMatchSet
    {
        private List<MatchPoint> matchList = new List<MatchPoint>();
        private PointMatchSettings settings;

        private MatchPoint current;

        public NuGenMatchSet(PointMatchSettings settings)
        {
            this.settings = settings;
        }

        public void AddCreatedPoints(List<PointMatchTriplet> list, Point first)
        {
            foreach(PointMatchTriplet t in list)
            {
                matchList.Add(new MatchPoint(new Point(t.x, t.y), false));
            }

            foreach (MatchPoint p in Matches)
            {
                if (Math.Abs(p.Point.X - first.X) <= 5 && Math.Abs(p.Point.Y - first.Y) <= 5)
                {
                    p.Accept();
                    MatchPoint temp = Matches[0];
                    int index = Matches.IndexOf(p);
                    Matches[0] = p;
                    Matches[index] = temp;
                    current = Matches[1];
                    return;
                }
            }
        }

        public MatchPoint Current
        {
            get
            {
                return current;
            }
        }

        public bool MoveNext()
        {
            if (Matches.IndexOf(current) == (Matches.Count - 1))
                return false;

            current = Matches[Matches.IndexOf(current) + 1];

            return true;
        }

        public bool MovePrev()
        {
            if (Matches.IndexOf(current) == 1)
                return false;

            current = Matches[Matches.IndexOf(current) - 1];

            return true;
        }

        public List<MatchPoint> Matches
        {
            get
            {
                return matchList;
            }
        }
    }
}
