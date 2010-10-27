using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
namespace Netron.Neon.OfficePickers
{
    static class CustomCaps
    {
        internal static ConnectionCap[] SelectableLineCaps =
            {  
                ConnectionCap.NoCap,
                ConnectionCap.DiamondLeft,
                ConnectionCap.DiamondRight,
                ConnectionCap.DiamondBoth,
                ConnectionCap.ArrowLeft,
                ConnectionCap.ArrowRight,
                ConnectionCap.ArrowBoth,
                ConnectionCap.RoundLeft,
                ConnectionCap.RoundRight,
                ConnectionCap.RoundBoth,
                ConnectionCap.SquareLeft,
                ConnectionCap.SquareRight,
                ConnectionCap.SquareBoth,
                ConnectionCap.Generalization
            };
    }
}
