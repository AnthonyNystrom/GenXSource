using System;
using CPI.Plot3D;

namespace Examples
{
    class Dominoes
    {
        private float distanceBetweenDominoes;

        private float angleBetweenDominoes;

        private const int framesBeforeConnection = 5;

        private Domino[] dominoArray;

        public Dominoes(int dominoCount, float dominoWidth, float dominoHeight, float dominoDepth, float distanceBetweenDominoes)
        {
            dominoArray = new Domino[dominoCount];

            for (int i = 0; i < dominoArray.Length; i++)
            {
                dominoArray[i] = new Domino(dominoWidth, dominoHeight, dominoDepth);
            }

            this.distanceBetweenDominoes = distanceBetweenDominoes;

            // Store the angle of a falling domino when it first connects with the domino after it.
            // (store it in degrees.)
            this.angleBetweenDominoes = (float)((Math.Acos(distanceBetweenDominoes / dominoHeight) * 180 / Math.PI));

        }

        public void Render(Plotter3D p)
        {
            foreach (Domino d in dominoArray)
            {
                d.Render(p);

                p.IsPenDown = false;
                p.Forward(distanceBetweenDominoes);
                p.IsPenDown = true;
            }
        }

        public delegate void PositionChangedEventHandler(object sender, EventArgs e);

        public event PositionChangedEventHandler PositionChanged;

        public void FallOver()
        {
            for (int lastFallingDomino = 0; lastFallingDomino < dominoArray.Length; lastFallingDomino++)
            {
                for (int frame = 0; frame < framesBeforeConnection; frame++)
                {
                    dominoArray[lastFallingDomino].FallAngle = 90 - ((90 - angleBetweenDominoes) * frame / framesBeforeConnection);

                    for (int parentDomino = lastFallingDomino - 1; parentDomino >= 0; parentDomino--)
                    {
                        dominoArray[parentDomino].FallAngle = (float)(MathUtil.FindInnerAngle(dominoArray[parentDomino + 1].FallAngle, dominoArray[0].Height, dominoArray[0].Depth, distanceBetweenDominoes));
                    }

                    if (PositionChanged != null)
                        PositionChanged(this, EventArgs.Empty);
                }
            }


            Domino lastDomino = dominoArray[dominoArray.Length - 1];

            float fallAngle = lastDomino.FallAngle;

            float distancePerFrame = angleBetweenDominoes / framesBeforeConnection;


            while (fallAngle > 0)
            {
                fallAngle -= distancePerFrame;

                lastDomino.FallAngle = Math.Max(fallAngle, 0);

                for (int parentDomino = dominoArray.Length - 2; parentDomino >= 0; parentDomino--)
                {
                    dominoArray[parentDomino].FallAngle = (float)(MathUtil.FindInnerAngle(dominoArray[parentDomino + 1].FallAngle, dominoArray[0].Height, dominoArray[0].Depth, distanceBetweenDominoes));
                }

                if (PositionChanged != null)
                    PositionChanged(this, EventArgs.Empty);
            }

        }
    }
}
