using System;
using CPI.Plot3D;

namespace Examples
{
    class Domino
    {
        private float width;
        private float height;
        private float depth;

        private float fallAngle;

        public Domino(float width, float height, float depth) : this(width, height, depth, 90) { }

        public Domino(float width, float height, float depth, float fallAngle)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.fallAngle = fallAngle;
        }

        public double Width
        {
            get
            {
                return width;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }
        }

        public double Depth
        {
            get
            {
                return depth;
            }
        }

        public float FallAngle
        {
            get
            {
                return fallAngle;
            }
            set
            {
                if (value < 0 || value > 90)
                    throw new ArgumentOutOfRangeException("Fall angle must be between 0 and 90 degrees.");

                fallAngle = value;
            }
        }

        public void Render(Plotter3D p)
        {
            Point3D startLocation = p.Location;
            Orientation3D startOrientation = p.Orientation.Clone();
            bool startPenDown = p.IsPenDown;
            AngleMeasurement startAngleMeasurement = p.AngleMeasurement;

            p.AngleMeasurement = AngleMeasurement.Degrees;

            // Move to the back edge of the domino
            p.IsPenDown = false;
            p.Forward(depth);
            p.TurnUp(90);
            p.IsPenDown = startPenDown;

            // Tilt the domino accordingly
            p.Orientation.RollLeft(90 - fallAngle);

            // Draw the back surface of the domino
            p.Forward(width);
            p.TurnLeft(90);
            p.Forward(height);
            p.TurnLeft(90);
            p.Forward(width);
            p.TurnLeft(90);
            p.Forward(height);
            p.TurnLeft(90);

            // Draw the middle bits of the domino
            p.TurnUp(90);
            p.Forward(depth);
            p.TurnDown(90);
            p.IsPenDown = false;
            p.Forward(width);
            p.TurnDown(90);
            p.IsPenDown = startPenDown;
            p.Forward(depth);
            p.TurnUp(90);
            p.TurnLeft(90);
            p.IsPenDown = false;
            p.Forward(height);
            p.TurnUp(90);
            p.IsPenDown = startPenDown;
            p.Forward(depth);
            p.TurnDown(90);
            p.TurnLeft(90);
            p.IsPenDown = false;
            p.Forward(width);
            p.TurnDown(90);
            p.IsPenDown = startPenDown;
            p.Forward(depth);
            p.IsPenDown = false;
            p.TurnUp(180);
            p.Forward(depth);
            p.TurnUp(90);
            p.Orientation.RollRight(180);
            p.IsPenDown = startPenDown;

            // Draw the front of the domino
            p.Forward(width);
            p.TurnRight(90);
            p.Forward(height);
            p.TurnRight(90);
            p.Forward(width);
            p.TurnRight(90);
            p.Forward(height);

            // Return the the start orientation and location, then advance to the back edge of the domino.
            p.IsPenDown = false;
            p.Orientation = startOrientation;
            p.Location = startLocation;
            p.Forward(depth);
            p.IsPenDown = startPenDown;
            p.AngleMeasurement = startAngleMeasurement;
        }
    }
}
