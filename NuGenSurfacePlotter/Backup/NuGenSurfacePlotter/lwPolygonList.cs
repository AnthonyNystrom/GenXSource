using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.Lwo
{
    class lwPolygonList : IDisposable
    {
        public int count;
        public int offset;              /* only used during reading */
        public int vcount;              /* total number of vertices */
        public int voffset;             /* only used during reading */
        public lwPolygon[] pol;         /* array of polygons */

        #region IDisposable Members

        public void Dispose()
        {
            //if (pol != null)
            //{
                //for (int i = 0; i < count; i++)
                //{
                //    if (pol[i].v != null)
                //    {
                //        for (int j = 0; j < pol[i].nverts; j++)
                //            if (pol[i].v[j].vm)
                //                free(pol[i].v[j].vm);
                //    }
                //}
                //if (pol[0].v != null)
                //    free(pol[0].v);
            //}
        }

        #endregion

        /// <summary>
        /// Allocate or extend the polygon arrays to hold new records.
        /// </summary>
        public bool AllocPolygons(int npols, int nverts)
        {
            offset = count;
            count += npols;
            if (pol != null)
                Array.Resize<lwPolygon>(ref pol, count);
            else
                pol = new lwPolygon[count];

            voffset = vcount;
            vcount += nverts;
            if (pol[0].v != null)
                Array.Resize<lwPolVert>(ref pol[0].v, vcount);
            else
                pol[0].v = new lwPolVert[vcount];

            /* fix up the old vertex pointers */

            //for (i = 1; i < offset; i++)
            //    pol[i].v = pol[i - 1].v + pol[i - 1].nverts;

            return true;
        }

        /// <summary>
        /// Read polygon records from a POLS chunk in an LWO2 file.  The polygons
        /// are added to the array in the lwPolygonList.
        /// </summary>
        public bool GetPolygons(FileStream fp, int cksize, ref lwPolygonList plist, int ptoffset)
        {
            BinaryReader read = new BinaryReader(fp);
            int pp;
            lwPolVert pv;
            byte[] buf;
            int bp;
            int j, flags, nv, nverts, npols;
            uint type;

            if (cksize == 0)
                return true;

            /* read the whole chunk */
            int flen = 0;
            type = lwio.getU4(read, ref flen);
            buf = lwio.getbytes(read, cksize - 4, ref flen);
            if (cksize != flen)
                goto Fail;

            /* count the polygons and vertices */

            nverts = 0;
            npols = 0;
            bp = 0;

            while (bp < cksize - 4)
            {
                nv = lwio.sgetU2(buf, ref bp, ref flen);
                nv &= 0x03FF;
                nverts += nv;
                npols++;
                for (int i = 0; i < nv; i++) ;
                    j = lwio.sgetVX(buf, ref bp, ref flen);
            }

            AllocPolygons(npols, nverts);

            /* fill in the new polygons */
            bp = 0;
            pp = plist.offset;
            pv = plist.pol[0].v[plist.voffset];

            for (int i = 0; i < npols; i++)
            {
                nv = lwio.sgetU2(buf, ref bp, ref flen);
                flags = nv & 0xFC00;
                nv &= 0x03FF;

                plist.pol[pp].nverts = nv;
                plist.pol[pp].flags = flags;
                plist.pol[pp].type = type;
                //if (plist.pol[pp].v == null)
                //    plist.pol[pp].v = pv;
                //for (int j = 0; j < nv; j++)
                //    plist.pol[pp].v[j].index = lwio.sgetVX(buf, ref bp, ref flen) + ptoffset;

                pp++;
                //pv += nv;
            }

            return true;

        Fail:
            return false;
        }

        /// <summary>
        /// Convert tag indexes into actual lwSurface pointers.  If any polygons
        /// point to tags for which no corresponding surface can be found, a
        /// default surface is created.
        /// </summary>
        public bool ResolvePolySurfaces(ref lwTagList tlist, ref LinkedList<lwSurface> surf, ref int nsurfs)
        {
            //lwSurface st;
            lwSurface[] s;
            int index;

            if (tlist.count == 0)
                return true;

            s = new lwSurface[tlist.count];
            for (int i = 0; i < tlist.count; i++)
            {
                LinkedListNode<lwSurface> st = surf.First;
                while (st != null)
                {
                    if (st.Value.name.CompareTo(tlist.tag[i]) == 0)
                    {
                        s[i] = st.Value;
                        break;
                    }
                    st = st.Next;
                }
            }

            //for (int i = 0; i < count; i++)
            //{
            //    index = (int)pol[i].surf;
            //    if (index < 0 || index > tlist.count)
            //        return false;
            //    if (!s[ index ])
            //    {
            //        s[ index ] = lwDefaultSurface();
            //        if ( !s[ index ] )
            //            return false;
            //        s[ index ]->name = malloc( strlen( tlist->tag[ index ] ) + 1 );
            //        if ( !s[ index ]->name )
            //            return false;
            //        strcpy( s[ index ]->name, tlist->tag[ index ] );
            //        lwListAdd( surf, s[ index ] );
            //        *nsurfs = *nsurfs + 1;
            //    }
            //    polygon->pol[ i ].surf = s[ index ];
            //}

            s = null;
            return true;
        }

        /// <summary>
        /// Read polygon records from a POLS chunk in an LWOB file.  The polygons
        /// are added to the array in the lwPolygonList.
        /// </summary>
        /// <param name="fp"></param>
        /// <param name="cksize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool GetPolygons5(FileStream fp, uint cksize, int ptoffset)
        {
            lwPolygon pp;
            lwPolVert pv;
            int nv, nverts, npols;

            if (cksize == 0)
                return true;

            /* read the whole chunk */

            int flen = 0;
            byte[] buf = new byte[cksize];
            fp.Read(buf, 0, (int)cksize);

            /* count the polygons and vertices */

            nverts = 0;
            npols = 0;
            int bp = 0;

            while (bp < cksize)
            {
                nv = BitConverter.ToInt16(buf, bp);
                nverts += nv;
                npols++;
                bp += 2 * nv;
                int i = lwio.sgetI2(buf, ref bp, ref flen);
                if (i < 0)
                    bp += 2;      /* detail polygons */
            }


            if (!AllocPolygons(npols, nverts))
                goto Fail;

            /* fill in the new polygons */

            bp = 0;
            pp = pol[offset];
            pv = pol[0].v[voffset];

            //for (int i = 0; i < npols; i++)
            //{
            //    nv = lwio.sgetU2(buf, ref bp, ref flen);

            //    pp.nverts = nv;
            //    pp.type = LWID.ID_FACE;
            //    if (pp.v == null)
            //        pp.v = pv;
            //    for (int j = 0; j < nv; j++)
            //        pv[j].index = lwio.sgetU2(buf, ref bp, ref flen) + ptoffset;
            //    j = lwio.sgetI2(ref buf, ref bp, ref flen);
            //    if (j < 0)
            //    {
            //        j = -j;
            //        bp += 2;
            //    }
            //    j -= 1;
            //    pp.surf = ( lwSurface * ) j;

            //    pp++;
            //    pv += nv;
            //}
            return true;

        Fail:
            //if ( buf ) free( buf );
            //lwFreePolygons( plist );
            return false;
        }
    }
}
