using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.Lwo
{
    /* an object */

    class lwObject : IDisposable
    {
        public LinkedList<lwLayer> layer;               /* linked list of layers */
        public LinkedList<lwEnvelope> env;                 /* linked list of envelopes */
        public LinkedList<lwClip> clip;                /* linked list of clips */
        public LinkedList<lwSurface> surf;                /* linked list of surfaces */
        public lwTagList taglist;
        public int nlayers;
        public int nenvs;
        public int nclips;
        public int nsurfs;

        #region IDs

        /* IDs specific to LWOB */

        public const uint ID_SRFS = 0x53524653;//ID(('S','R','F','S')
        public const uint ID_FLAG = 0x464c4147;//ID(('F','L','A','G')
        public const uint ID_VLUM = 0x564c554d;//ID(('V','L','U','M')
        public const uint ID_VDIF = 0x56444946;//ID(('V','D','I','F')
        public const uint ID_VSPC = 0x56535043;//ID(('V','S','P','C')
        public const uint ID_RFLT = 0x52464c54;//ID(('R','F','L','T')
        public const uint ID_BTEX = 0x42544558;//ID(('B','T','E','X')
        public const uint ID_CTEX = 0x43544558;//ID(('C','T','E','X')
        public const uint ID_DTEX = 0x44544558;//ID(('D','T','E','X')
        public const uint ID_LTEX = 0x4c544558;//ID(('L','T','E','X')
        public const uint ID_RTEX = 0x52544558;//ID(('R','T','E','X')
        public const uint ID_STEX = 0x53544558;//ID(('S','T','E','X')
        public const uint ID_TTEX = 0x54544558;//ID(('T','T','E','X')
        public const uint ID_TFLG = 0x54464c47;//ID(('T','F','L','G')
        public const uint ID_TSIZ = 0x5453495a;//ID(('T','S','I','Z')
        public const uint ID_TCTR = 0x54435452;//ID(('T','C','T','R')
        public const uint ID_TFAL = 0x5446414c;//ID(('T','F','A','L')
        public const uint ID_TVEL = 0x5456454c;//ID(('T','V','E','L')
        public const uint ID_TCLR = 0x54434c52;//ID(('T','C','L','R')
        public const uint ID_TVAL = 0x5456414c;//ID(('T','V','A','L')
        public const uint ID_TAMP = 0x54414d50;//ID(('T','A','M','P')
        public const uint ID_TIMG = 0x54494d47;//ID(('T','I','M','G')
        public const uint ID_TAAS = 0x54414153;//ID(('T','A','A','S')
        public const uint ID_TREF = 0x54524546;//ID(('T','R','E','F')
        public const uint ID_TOPC = 0x544f5043;//ID(('T','O','P','C')
        public const uint ID_SDAT = 0x53444154;//ID(('S','D','A','T')
        public const uint ID_TFP0 = 0x54465030;//ID(('T','F','P','0')
        public const uint ID_TFP1 = 0x54465031;//ID(('T','F','P','1')

        #endregion

        /// <summary>
        /// Returns the contents of an LWOB, given its filename, or NULL if the
        /// file couldn't be loaded.  On failure, failID and failpos can be used
        /// to diagnose the cause.
        /// 
        /// 1.  If the file isn't an LWOB, failpos will contain 12 and failID will
        /// be unchanged.
        ///
        /// 2.  If an error occurs while reading an LWOB, failID will contain the
        /// most recently read IFF chunk ID, and failpos will contain the
        /// value returned by ftell() at the time of the failure.
        /// 
        /// 3.  If the file couldn't be opened, or an error occurs while reading
        /// the first 12 bytes, both failID and failpos will be unchanged.
        ///
        /// If you don't need this information, failID and failpos can be NULL.
        /// </summary>
        public static lwObject GetObject5(string filename, out uint failID, out int failpos)
        {
            FileStream fp = null;
            BinaryReader br = null;
            lwObject obj;
            lwLayer layer;
            lwSurface node;
            uint id, formsize, type, cksize;
            int flen;

            /* open the file */

            try
            {
                br = new BinaryReader(fp = new FileStream(filename, FileMode.Open));
            }
            catch { failID = 0; failpos = 0; return null; }

            /* read the first 12 bytes */

            flen = 0;
            id = lwio.getU4(br, ref flen);
            formsize = lwio.getU4(br, ref flen);
            type = lwio.getU4(br, ref flen);
            if (12 != flen)
            {
                br.Close();
                fp.Close();
                failID = 0; failpos = 0;
                return null;
            }

            /* LWOB? */

            if (id != LWID.ID_FORM || type != LWID.ID_LWOB)
            {
                br.Close();
                fp.Close();
                failpos = 12;
                failID = 0;
                return null;
            }

            /* allocate an object and a default layer */

            obj = new lwObject();

            layer = new lwLayer();
            obj.layer = new LinkedList<lwLayer>();
            obj.layer.AddFirst(layer);
            obj.nlayers = 1;

            /* get the first chunk header */

            id = lwio.getU4(br, ref flen);
            cksize = lwio.getU4(br, ref flen);
            if (0 > flen)
                goto Fail;

            /* process chunks as they're encountered */

            while (true)
            {
                cksize += cksize & 1;

                switch (id)
                {
                    case LWID.ID_PNTS:
                        if (!lwPointList.GetPoints(fp, (int)cksize, ref layer.point, ref flen))
                            goto Fail;
                        break;

                    case LWID.ID_POLS:
                        if (!layer.polygon.GetPolygons5(fp, cksize, layer.point.offset))
                            goto Fail;
                        break;

                    case ID_SRFS:
                        if (!lwTagList.GetTags(br, (int)cksize, ref obj.taglist, ref flen))
                            goto Fail;
                        break;

                    case LWID.ID_SURF:
                        node = obj.GetSurface5(br, (int)cksize, ref flen);
                        if (node == null) goto Fail;
                        obj.surf.AddLast(node);
                        obj.nsurfs++;
                        break;

                    default:
                        fp.Seek(cksize, SeekOrigin.Current);
                        break;
                }

                /* end of the file? */

                if (formsize <= (uint)fp.Position - 8) break;

                /* get the next chunk header */

                flen = 0;
                id = lwio.getU4(br, ref flen);
                cksize = lwio.getU4(br, ref flen);
                if (8 != flen) goto Fail;
            }

            br.Close();
            fp.Close();
            fp = null;

            layer.point.GetBoundingBox(ref layer.bbox);
            layer.point.GetPolyNormals(ref layer.polygon);
            if (!layer.point.GetPointPolygons(ref layer.polygon)) goto Fail;
            if (!layer.polygon.ResolvePolySurfaces(ref obj.taglist,
                                                     ref obj.surf, ref obj.nsurfs)) goto Fail;
            layer.point.GetVertNormals(ref layer.polygon);

            failID = 0; failpos = 0;
            return obj;

        Fail:
            failID = id;
            if (fp != null)
            {
                failpos = (int)fp.Position;
                fp.Close();
            }
            else
                failpos = 0;
            obj.Dispose();
            return null;
        }

        lwSurface GetSurface5(BinaryReader fp, int cksize, ref int flen)
        {
            lwSurface surf;
            lwTexture tex = null;
            lwPlugin shdr = null;
            string s;
            float[] v = new float[3];
            uint id, flags;
            ushort sz;
            long pos;
            int i = 0;

            /* allocate the Surface structure */

            surf = new lwSurface();

            /* non-zero defaults */

            surf.color.rgb[0] = 0.78431f;
            surf.color.rgb[1] = 0.78431f;
            surf.color.rgb[2] = 0.78431f;
            surf.diffuse.val    = 1.0f;
            surf.glossiness.val = 0.4f;
            surf.bump.val       = 1.0f;
            surf.eta.val        = 1.0f;
            surf.sideflags      = 1;

            /* remember where we started */

            flen = 0;
            pos = fp.BaseStream.Position;

            /* name */

            surf.name = lwio.getS0(fp, ref flen);

            /* first subchunk header */

            id = lwio.getU4(fp, ref flen);
            sz = lwio.getU2(fp, ref flen);
            if (0 > flen)
                goto Fail;

            /* process subchunks as they're encountered */

            while (true)
            {
                sz += (ushort)(sz & 1);
                flen = 0;

                switch (id)
                {
                case LWID.ID_COLR:
                    surf.color.rgb[0] = lwio.getU1(fp, ref flen) / 255.0f;
                    surf.color.rgb[1] = lwio.getU1(fp, ref flen) / 255.0f;
                    surf.color.rgb[2] = lwio.getU1(fp, ref flen) / 255.0f;
                    break;

                case ID_FLAG:
                    flags = lwio.getU2(fp, ref flen);
                    if ((flags & 4) > 0) surf.smooth = 1.56207f;
                    if ((flags & 8) > 0) surf.color_hilite.val = 1.0f;
                    if ((flags & 16) > 0) surf.color_filter.val = 1.0f;
                    if ((flags & 128) > 0) surf.dif_sharp.val = 0.5f;
                    if ((flags & 256) > 0) surf.sideflags = 3;
                    if ((flags & 512) > 0) surf.add_trans.val = 1.0f;
                    break;

                case LWID.ID_LUMI:
                    surf.luminosity.val = lwio.getI2(fp, ref flen) / 256.0f;
                    break;

                case ID_VLUM:
                    surf.luminosity.val = lwio.getF4(fp, ref flen);
                    break;

                case LWID.ID_DIFF:
                    surf.diffuse.val = lwio.getI2(fp, ref flen) / 256.0f;
                    break;

                case ID_VDIF:
                    surf.diffuse.val = lwio.getF4(fp, ref flen);
                    break;

                case LWID.ID_SPEC:
                    surf.specularity.val = lwio.getI2(fp, ref flen) / 256.0f;
                    break;

                case ID_VSPC:
                    surf.specularity.val = lwio.getF4(fp, ref flen);
                    break;

                case LWID.ID_GLOS:
                    surf.glossiness.val = (float)Math.Log(lwio.getU2(fp, ref flen)) / 20.7944f;
                    break;

                case LWID.ID_SMAN:
                    surf.smooth = lwio.getF4(fp, ref flen);
                    break;

                case LWID.ID_REFL:
                    surf.reflection.val.val = lwio.getI2(fp, ref flen) / 256.0f;
                    break;

                case ID_RFLT:
                    surf.reflection.options = lwio.getU2(fp, ref flen);
                    break;

                case LWID.ID_RIMG:
                    s = lwio.getS0(fp, ref flen);
                    surf.reflection.cindex = add_clip(ref s, clip, ref nclips);
                    surf.reflection.options = 3;
                    break;

                case LWID.ID_RSAN:
                    surf.reflection.seam_angle = lwio.getF4(fp, ref flen);
                    break;

                case LWID.ID_TRAN:
                    surf.transparency.val.val = lwio.getI2(fp, ref flen) / 256.0f;
                    break;

                case LWID.ID_RIND:
                    surf.eta.val = lwio.getF4(fp, ref flen);
                    break;

                case ID_BTEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.bump.tex.AddLast(tex);
                    break;

                case ID_CTEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.color.tex.AddLast(tex);
                    break;

                case ID_DTEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.diffuse.tex.AddLast(tex);
                    break;

                case ID_LTEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.luminosity.tex.AddLast(tex);
                    break;

                case ID_RTEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.reflection.val.tex.AddLast(tex);
                    break;

                case ID_STEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.specularity.tex.AddLast(tex);
                    break;

                case ID_TTEX:
                    s = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    tex = get_texture(s);
                    surf.transparency.val.tex.AddLast(tex);
                    break;

                case ID_TFLG:
                    flags = lwio.getU2(fp, ref flen);

                    if ((flags & 1) > 0) i = 0;
                    if ((flags & 2) > 0) i = 1;
                    if ((flags & 4) > 0) i = 2;
                    tex.axis = (short)i;
                    if (tex.type == LWID.ID_IMAP)
                        tex.imap.axis = i;
                    else
                        tex.proc.axis = i;

                    if ((flags & 8) > 0) tex.tmap.coord_sys = 1;
                    if ((flags & 16) > 0) tex.negative = 1;
                    if ((flags & 32) > 0) tex.imap.pblend = 1;
                    if ((flags & 64) > 0)
                    {
                        tex.imap.aa_strength = 1.0f;
                        tex.imap.aas_flags = 1;
                    }
                    break;

                case ID_TSIZ:
                    for (i = 0; i < 3; i++)
                        tex.tmap.size.val[i] = lwio.getF4(fp, ref flen);
                    break;

                case ID_TCTR:
                    for (i = 0; i < 3; i++)
                        tex.tmap.center.val[i] = lwio.getF4(fp, ref flen);
                    break;

                case ID_TFAL:
                    for (i = 0; i < 3; i++)
                        tex.tmap.falloff.val[i] = lwio.getF4(fp, ref flen);
                    break;

                case ID_TVEL:
                    for (i = 0; i < 3; i++)
                        v[i] = lwio.getF4(fp, ref flen);
                    tex.tmap.center.eindex = add_tvel(tex.tmap.center.val, v, env, ref nenvs);
                    break;

                case ID_TCLR:
                    if (tex.type == LWID.ID_PROC)
                        for (i = 0; i < 3; i++)
                            tex.proc.value[i] = lwio.getU1(fp, ref flen) / 255.0f;
                    break;

                case ID_TVAL:
                    tex.proc.value[0] = lwio.getI2(fp, ref flen) / 256.0f;
                    break;

                case ID_TAMP:
                    if (tex.type == LWID.ID_IMAP)
                        tex.imap.amplitude.val = lwio.getF4(fp, ref flen);
                    break;

                case ID_TIMG:
                    s = lwio.getS0(fp, ref flen);
                    tex.imap.cindex = add_clip(ref s, clip, ref nclips);
                    break;

                case ID_TAAS:
                    tex.imap.aa_strength = lwio.getF4(fp, ref flen);
                    tex.imap.aas_flags = 1;
                    break;

                case ID_TREF:
                    tex.tmap.ref_object = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen), sz);
                    break;

                case ID_TOPC:
                    tex.opacity.val = lwio.getF4(fp, ref flen);
                    break;

                case ID_TFP0:
                    if (tex.type == LWID.ID_IMAP)
                        tex.imap.wrapw.val = lwio.getF4(fp, ref flen);
                    break;

                case ID_TFP1:
                    if (tex.type == LWID.ID_IMAP)
                        tex.imap.wraph.val = lwio.getF4(fp, ref flen);
                    break;

                case LWID.ID_SHDR:
                    shdr = new lwPlugin();
                    shdr.name = BitConverter.ToString(lwio.getbytes(fp, sz, ref flen));
                    surf.shader.AddLast(shdr);
                    surf.nshaders++;
                    break;

                case ID_SDAT:
                    shdr.data = lwio.getbytes(fp, sz, ref flen);
                    break;

                default:
                    break;
                }

                /* error while reading current subchunk? */

                if (flen < 0 || flen > sz)
                    goto Fail;

                /* skip unread parts of the current subchunk */

                if (flen < sz)
                    fp.BaseStream.Seek(sz - flen, SeekOrigin.Current);

                /* end of the SURF chunk? */

                if (cksize <= fp.BaseStream.Position - pos )
                    break;

                /* get the next subchunk header */

                flen = 0;
                id = lwio.getU4(fp, ref flen);
                sz = lwio.getU2(fp, ref flen);
                if (6 != flen)
                    goto Fail;
            }

            return surf;

Fail:
            //if ( surf ) lwFreeSurface( surf );
            return null;
        }

        /// <summary>
        /// Add a clip to the clip list.  Used to store the contents of an RIMG or
        /// TIMG surface subchunk.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="clist"></param>
        /// <param name="nclips"></param>
        /// <returns></returns>
        private static int add_clip(ref string s, LinkedList<lwClip> clist, ref int nclips)
        {
            lwClip clip;
            int p;

            clip = new lwClip();

            clip.contrast.val = 1.0f;
            clip.brightness.val = 1.0f;
            clip.saturation.val = 1.0f;
            clip.gamma.val = 1.0f;

            if ((p = s.IndexOf("(sequence)")) != -1)
            {
                s = s.Substring(0, p - 1);
                clip.type = LWID.ID_ISEQ;
                clip.seq.prefix = s;
                clip.seq.digits = 3;
            }
            else
            {
                clip.type = LWID.ID_STIL;
                clip.still.name = s;
            }

            nclips++;
            clip.index = nclips;

            clist.AddLast(clip);

            return clip.index;
        }

        /// <summary>
        /// Create a new texture for BTEX, CTEX, etc. subchunks.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static lwTexture get_texture(string s)
        {
            lwTexture tex = new lwTexture();

            tex.tmap.size.val[0] =
            tex.tmap.size.val[1] =
            tex.tmap.size.val[2] = 1.0f;
            tex.opacity.val = 1.0f;
            tex.enabled = 1;

            if (s.Contains("Image Map"))
            {
                tex.type = LWID.ID_IMAP;
                if (s.Contains("Planar")) tex.imap.projection = 0;
                else if (s.Contains("Cylindrical")) tex.imap.projection = 1;
                else if (s.Contains("Spherical")) tex.imap.projection = 2;
                else if (s.Contains("Cubic")) tex.imap.projection = 3;
                else if (s.Contains("Front")) tex.imap.projection = 4;
                tex.imap.aa_strength = 1.0f;
                tex.imap.amplitude.val = 1.0f;
            }
            else
            {
                tex.type = LWID.ID_PROC;
                tex.proc.name = s;
            }

            return tex;
        }

        /// <summary>
        /// Add a triple of envelopes to simulate the old texture velocity
        /// parameters.
        /// </summary>
        static int add_tvel(float[] pos, float[] vel, LinkedList<lwEnvelope> elist, ref int nenvs)
        {
            lwEnvelope env = null;
            lwKey key0, key1;

            for (int i = 0; i < 3; i++)
            {
                env = new lwEnvelope();
                env.key = new LinkedList<lwKey>();
                key0 = new lwKey();
                key1 = new lwKey();

                key0.value = pos[i];
                key0.time = 0.0f;
                key1.value = pos[i] + vel[i] * 30.0f;
                key1.time = 1.0f;
                key0.shape = key1.shape = LWID.ID_LINE;

                env.index = nenvs + i + 1;
                env.type = 0x0301 + i;
                env.name = String.Format("{0}{1}", "Position.", 'X' + i);

                env.key.AddFirst(key0);
                env.key.AddLast(key1);
                env.nkeys = 2;
                env.behavior[0] = LWID.BEH_LINEAR;
                env.behavior[1] = LWID.BEH_LINEAR;

                elist.AddLast(env);
            }

            nenvs += 3;
            return env.index - 2;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (layer != null)
            {
                foreach (lwLayer lay in layer)
                {
                    lay.Dispose();
                }
                //          lwListFree( object->env, lwFreeEnvelope );
                //lwListFree( object->clip, lwFreeClip );
                //lwListFree( object->surf, lwFreeSurface );
                //lwFreeTags( &object->taglist );
            }
        }

        #endregion
    }
}
