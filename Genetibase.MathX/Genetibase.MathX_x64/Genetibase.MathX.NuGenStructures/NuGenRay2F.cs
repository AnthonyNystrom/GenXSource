using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenRay2F.
    /// </summary>
    public struct NuGenRay2F
    {

        public NuGenRay2F(NuGenPnt2F p, NuGenVec2F v)
        {
            this.p = p;
            this.v = v;
        }

        public NuGenPnt2F Point
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
            }
        }

        public NuGenVec2F Direction
        {
            get
            {
                return v;
            }
            set
            {
                v = value;
            }
        }

        public static explicit operator NuGenRay2F(NuGenRay2D r)
        {
            return new NuGenRay2F((NuGenPnt2F)r.p, (NuGenVec2F)r.v);
        }

        public NuGenPnt2F GetPointOnRay(float t)
        {
            return p + v * t;
        }

        internal NuGenPnt2F p;
        internal NuGenVec2F v;

    }
    

}
