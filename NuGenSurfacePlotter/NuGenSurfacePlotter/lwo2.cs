using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.Lwo
{
    class LWID
    {
        #region Chunk IDs

        public const uint ID_FORM = 0x464f524d;// ID('F','O','R','M');
        public const uint ID_LWO2 = 0x4c574f32;//ID('L','W','O','2');
        public const uint ID_LWOB = 0x4c574f42;//ID('L','W','O','B');

        /* top-level chunks */
        public const uint ID_LAYR = 0x4c415952;//ID('L','A','Y','R');
        public const uint ID_TAGS = 0x54414753;//ID('T','A','G','S');
        public const uint ID_PNTS = 0x504e5453;//ID('P','N','T','S');
        public const uint ID_BBOX = 0x42424f58;//ID('B','B','O','X');
        //public const uint ID_VMAP = 0x564d4150;//ID('V','M','A','P');
        public const uint ID_VMAD = 0x564d4144;//ID('V','M','A','D');
        public const uint ID_POLS = 0x504f4c53;//ID('P','O','L','S');
        public const uint ID_PTAG = 0x50544147;//ID('P','T','A','G');
        public const uint ID_ENVL = 0x454e564c;//ID('E','N','V','L');
        public const uint ID_CLIP = 0x434c4950;//ID('C','L','I','P');
        //public const uint ID_SURF = 0x53555246;//ID('S','U','R','F');
        public const uint ID_DESC = 0x44455343;//ID('D','E','S','C');
        public const uint ID_TEXT = 0x54455854;//ID('T','E','X','T');
        public const uint ID_ICON = 0x49434f4e;//ID('I','C','O','N');

        /* polygon types */
        public const uint ID_FACE = 0x46414345;//ID('F','A','C','E');
        public const uint ID_CURV = 0x43555256;//ID('C','U','R','V');
        public const uint ID_PTCH = 0x50544348;//ID('P','T','C','H');
        public const uint ID_MBAL = 0x4d42414c;//ID('M','B','A','L');
        public const uint ID_BONE = 0x424f4e45;//ID('B','O','N','E');

        /* polygon tags */
        public const uint ID_SURF = 0x53555246;//ID('S','U','R','F');
        public const uint ID_PART = 0x50415254;//ID('P','A','R','T');
        public const uint ID_SMGP = 0x534d4750;//ID('S','M','G','P');

        /* envelopes */
        public const uint ID_PRE = 0x50524520;//ID('P','R','E',' ');
        public const uint ID_POST = 0x504f5354;//ID('P','O','S','T');
        public const uint ID_KEY = 0x4b455920;//ID('K','E','Y',' ');
        public const uint ID_SPAN = 0x5350414e;//ID('S','P','A','N');
        public const uint ID_TCB = 0x54434220;//ID('T','C','B',' ');
        public const uint ID_HERM = 0x4845524d;//ID('H','E','R','M');
        public const uint ID_BEZI = 0x42455a49;//ID('B','E','Z','I');
        public const uint ID_BEZ2 = 0x42455a32;//ID('B','E','Z','2');
        //public const uint ID_LINE = 0x4c494e45;//ID('L','I','N','E');
        public const uint ID_STEP = 0x53544550;//ID('S','T','E','P');

        /* clips */
        public const uint ID_STIL = 0x5354494c;//ID('S','T','I','L');
        public const uint ID_ISEQ = 0x49534551;//ID('I','S','E','Q');
        public const uint ID_ANIM = 0x414e494d;//ID('A','N','I','M');
        public const uint ID_XREF = 0x58524546;//ID('X','R','E','F');
        public const uint ID_STCC = 0x53544343;//ID('S','T','C','C');
        public const uint ID_TIME = 0x54494d45;//ID('T','I','M','E');
        public const uint ID_CONT = 0x434f4e54;//ID('C','O','N','T');
        public const uint ID_BRIT = 0x42524954;//ID('B','R','I','T');
        public const uint ID_SATR = 0x53415452;//ID('S','A','T','R');
        public const uint ID_HUE = 0x48554520;//ID('H','U','E',' ');
        public const uint ID_GAMM = 0x47414d4d;//ID('G','A','M','M');
        public const uint ID_NEGA = 0x4e454741;//ID('N','E','G','A');
        public const uint ID_IFLT = 0x49464c54;//ID('I','F','L','T');
        public const uint ID_PFLT = 0x50464c54;//ID('P','F','L','T');

        /* surfaces */
        public const uint ID_COLR = 0x434f4c52;//ID('C','O','L','R');
        public const uint ID_LUMI = 0x4c554d49;//ID('L','U','M','I');
        public const uint ID_DIFF = 0x44494646;//ID('D','I','F','F');
        public const uint ID_SPEC = 0x53504543;//ID('S','P','E','C');
        public const uint ID_GLOS = 0x474c4f53;//ID('G','L','O','S');
        public const uint ID_REFL = 0x5245464c;//ID('R','E','F','L');
        public const uint ID_RFOP = 0x52464f50;//ID('R','F','O','P');
        public const uint ID_RIMG = 0x52494d47;//ID('R','I','M','G');
        public const uint ID_RSAN = 0x5253414e;//ID('R','S','A','N');
        public const uint ID_TRAN = 0x5452414e;//ID('T','R','A','N');
        public const uint ID_TROP = 0x54524f50;//ID('T','R','O','P');
        public const uint ID_TIMG = 0x54494d47;//ID('T','I','M','G');
        public const uint ID_RIND = 0x52494e44;//ID('R','I','N','D');
        public const uint ID_TRNL = 0x54524e4c;//ID('T','R','N','L');
        public const uint ID_BUMP = 0x42554d50;//ID('B','U','M','P');
        public const uint ID_SMAN = 0x534d414e;//ID('S','M','A','N');
        public const uint ID_SIDE = 0x53494445;//ID('S','I','D','E');
        public const uint ID_CLRH = 0x434c5248;//ID('C','L','R','H');
        public const uint ID_CLRF = 0x434c5246;//ID('C','L','R','F');
        public const uint ID_ADTR = 0x41445452;//ID('A','D','T','R');
        public const uint ID_SHRP = 0x53485250;//ID('S','H','R','P');
        public const uint ID_LINE = 0x4c494e45;//ID('L','I','N','E');
        public const uint ID_LSIZ = 0x4c53495a;//ID('L','S','I','Z');
        public const uint ID_ALPH = 0x414c5048;//ID('A','L','P','H');
        public const uint ID_AVAL = 0x4156414c;//ID('A','V','A','L');
        public const uint ID_GVAL = 0x4756414c;//ID('G','V','A','L');
        public const uint ID_BLOK = 0x424c4f4b;//ID('B','L','O','K');

        /* texture layer */
        public const uint ID_TYPE = 0x54595045;//ID('T','Y','P','E');
        public const uint ID_CHAN = 0x4348414e;//ID('C','H','A','N');
        public const uint ID_NAME = 0x4e414d45;//ID('N','A','M','E');
        public const uint ID_ENAB = 0x454e4142;//ID('E','N','A','B');
        public const uint ID_OPAC = 0x4f504143;//ID('O','P','A','C');
        public const uint ID_FLAG = 0x464c4147;//ID('F','L','A','G');
        public const uint ID_PROJ = 0x50524f4a;//ID('P','R','O','J');
        public const uint ID_STCK = 0x5354434b;//ID('S','T','C','K');
        public const uint ID_TAMP = 0x54414d50;//ID('T','A','M','P');

        /* texture coordinates */
        public const uint ID_TMAP = 0x544d4150;//ID('T','M','A','P');
        public const uint ID_AXIS = 0x41584953;//ID('A','X','I','S');
        public const uint ID_CNTR = 0x434e5452;//ID('C','N','T','R');
        public const uint ID_SIZE = 0x53495a45;//ID('S','I','Z','E');
        public const uint ID_ROTA = 0x524f5441;//ID('R','O','T','A');
        public const uint ID_OREF = 0x4f524546;//ID('O','R','E','F');
        public const uint ID_FALL = 0x46414c4c;//ID('F','A','L','L');
        public const uint ID_CSYS = 0x43535953;//ID('C','S','Y','S');

        /* image map */
        public const uint ID_IMAP = 0x494d4150;//ID('I','M','A','P');
        public const uint ID_IMAG = 0x494d4147;//ID('I','M','A','G');
        public const uint ID_WRAP = 0x57524150;//ID('W','R','A','P');
        public const uint ID_WRPW = 0x57525057;//ID('W','R','P','W');
        public const uint ID_WRPH = 0x57525048;//ID('W','R','P','H');
        public const uint ID_VMAP = 0x564d4150;//ID('V','M','A','P');
        public const uint ID_AAST = 0x41415354;//ID('A','A','S','T');
        public const uint ID_PIXB = 0x50495842;//ID('P','I','X','B');

        /* procedural */
        public const uint ID_PROC = 0x50524f43;//ID('P','R','O','C');
        //public const uint ID_COLR = 0x434f4c52;//ID('C','O','L','R');
        public const uint ID_VALU = 0x56414c55;//ID('V','A','L','U');
        public const uint ID_FUNC = 0x46554e43;//ID('F','U','N','C');
        public const uint ID_FTPS = 0x46545053;//ID('F','T','P','S');
        public const uint ID_ITPS = 0x49545053;//ID('I','T','P','S');
        public const uint ID_ETPS = 0x45545053;//ID('E','T','P','S');

        /* gradient */
        public const uint ID_GRAD = 0x47524144;//ID('G','R','A','D');
        public const uint ID_GRST = 0x47525354;//ID('G','R','S','T');
        public const uint ID_GREN = 0x4752454e;//ID('G','R','E','N');
        public const uint ID_PNAM = 0x504e414d;//ID('P','N','A','M');
        public const uint ID_INAM = 0x494e414d;//ID('I','N','A','M');
        public const uint ID_GRPT = 0x47525054;//ID('G','R','P','T');
        public const uint ID_FKEY = 0x464b4559;//ID('F','K','E','Y');
        public const uint ID_IKEY = 0x494b4559;//ID('I','K','E','Y');

        /* shader */
        public const uint ID_SHDR = 0x53484452;//ID('S','H','D','R');
        public const uint ID_DATA = 0x44415441;//ID('D','A','T','A');

        #endregion

        public const int BEH_RESET = 0;
        public const int BEH_CONSTANT = 1;
        public const int BEH_REPEAT = 2;
        public const int BEH_OSCILLATE = 3;
        public const int BEH_OFFSET = 4;
        public const int BEH_LINEAR = 5;

        public static uint ID(char a, char b, char c, char d)
        {
            return (uint)(((a)<<24)|((b)<<16)|((c)<<8)|(d));
        }
    }

    /* plug-in reference */
    class lwPlugin
    {
        public string ord;
        public string name;
        public int flags;
        public object data;
    }

    class lwKey
    {
        public float value;
        public float time;
        public uint shape;               /* ID_TCB, ID_BEZ2, etc. */
        public float tension;
        public float continuity;
        public float bias;
        public float[] param;
    }

    class lwEnvelope
    {
        public int index;
        public int type;
        public string name;
        public LinkedList<lwKey> key;              /* linked list of keys */
        public int nkeys;
        public int[] behavior;                     /* pre and post (extrapolation) */
        public LinkedList<lwPlugin> cfilter;       /* linked list of channel filters */
        public int ncfilters;
    }

    /* values that can be enveloped */

    class lwEParam
    {
        public float val;
        public int eindex;
    }

    class lwVParam
    {
        public float[] val;
        public int eindex;
    }

    /* clips */

    class lwClipStill
    {
        public string name;
    }

    class lwClipSeq
    {
        public string prefix;              /* filename before sequence digits */
        public string suffix;              /* after digits, e.g. extensions */
        public int digits;
        public int flags;
        public int offset;
        public int start;
        public int end;
    }

    class lwClipAnim
    {
        public string name;
        public string server;              /* anim loader plug-in */
        public object data;
    }

    class lwClipXRef {
        public string String;
        public int index;
        public lwClip clip;
    }

    class lwClipCycle
    {
       public string name;
       public int lo;
       public int hi;
    }

    class lwClip
    {
        public int index;
        public uint type;                /* ID_STIL, ID_ISEQ, etc. */
        public lwClipStill still;
        public lwClipSeq seq;
        public lwClipAnim anim;
        public lwClipXRef xref;
        public lwClipCycle cycle;
        public float start_time;
        public float duration;
        public float frame_rate;
        public lwEParam contrast;
        public lwEParam brightness;
        public lwEParam saturation;
        public lwEParam hue;
        public lwEParam gamma;
        public int negative;
        public lwPlugin ifilter;             /* linked list of image filters */
        public int nifilters;
        public lwPlugin pfilter;             /* linked list of pixel filters */
        public int npfilters;
    }

    /* textures */

    class lwTMap
    {
        public lwVParam size;
        public lwVParam center;
        public lwVParam rotate;
        public lwVParam falloff;
        public int fall_type;
        public string ref_object;
        public int coord_sys;
    }

    class lwImageMap
    {
        public int cindex;
        public int projection;
        public string vmap_name;
        public int axis;
        public int wrapw_type;
        public int wraph_type;
        public lwEParam wrapw;
        public lwEParam wraph;
        public float aa_strength;
        public int aas_flags;
        public int pblend;
        public lwEParam stck;
        public lwEParam amplitude;
    }

    class lwProcedural
    {
        public int axis;
        public float[] value;
        public string name;
        public object data;
    }

    class lwGradKey
    {
        public float value;
        public float[] rgba;
    }

    class lwGradient
    {
        public string paramname;
        public string itemname;
        public float start;
        public float end;
        public int repeat;
        public LinkedList<lwGradKey> key;                  /* array of gradient keys */
        public short[] ikey;                               /* array of interpolation codes */
    }

    class lwTexture
    {
        public string ord;
        public uint type;
        public uint chan;
        public lwEParam opacity;
        public short opac_type;
        public short enabled;
        public short negative;
        public short axis;
        public lwImageMap imap;
        public lwProcedural proc;
        public lwGradient grad;
        public lwTMap tmap;
    }

    /* values that can be textured */

    class lwTParam
    {
        public float val;
        public int eindex;
        public LinkedList<lwTexture> tex;                 /* linked list of texture layers */
    }

    class lwCParam
    {
        public float[] rgb;
        public int eindex;
        public LinkedList<lwTexture> tex;                 /* linked list of texture layers */
    }


    /* surfaces */

    class lwGlow
    {
        public short enabled;
        public short type;
        public lwEParam intensity;
        public lwEParam size;
    }

    class lwRMap
    {
        public lwTParam val;
        public int options;
        public int cindex;
        public float seam_angle;
    }

    class lwLine
    {
        public short enabled;
        public ushort flags;
        public lwEParam size;
    }

    class lwSurface
    {
        public string name;
        public string srcname;
        public lwCParam color;
        public lwTParam luminosity;
        public lwTParam diffuse;
        public lwTParam specularity;
        public lwTParam glossiness;
        public lwRMap reflection;
        public lwRMap transparency;
        public lwTParam eta;
        public lwTParam translucency;
        public lwTParam bump;
        public float smooth;
        public int sideflags;
        public float alpha;
        public int alpha_mode;
        public lwEParam color_hilite;
        public lwEParam color_filter;
        public lwEParam add_trans;
        public lwEParam dif_sharp;
        public lwEParam glow;
        public lwLine line;
        public LinkedList<lwPlugin> shader;              /* linked list of shaders */
        public int nshaders;
    }

    /* vertex maps */

    class lwVMap : IDisposable
    {
        public string name;
        public uint type;
        public int dim;
        public int nverts;
        public int  perpoly;
        public int[] vindex;              /* array of point indexes */
        public int[] pindex;              /* array of polygon indexes */
        public float[][] val;

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }

    class lwVMapPt
    {
        public lwVMap vmap;
        public int index;               /* vindex or pindex element */
    }

    /* points and polygons */

    class lwPoint
    {
        public float[] pos;
        public int npols;               /* number of polygons sharing the point */
        public int[] pol;                 /* array of polygon indexes */
        public int nvmaps;
        public lwVMapPt[] vm;                  /* array of vmap references */
    }

    class lwPolVert
    {
        public int index;               /* index into the point array */
        public float[] norm;
        public int nvmaps;
        public lwVMapPt[] vm;                  /* array of vmap references */
    }

    class lwPolygon
    {
        public lwSurface surf;
        public int part;                /* part index */
        public int smoothgrp;           /* smoothing group */
        public int flags;
        public uint type;
        public float[] norm;
        public int nverts;
        public lwPolVert[] v;                   /* array of vertex records */
    }

    /* geometry layers */

    class lwLayer : IDisposable
    {
        public string name;
        public int index;
        public int parent;
        public int flags;
        public float[] pivot;
        public float[] bbox;
        public lwPointList point;
        public lwPolygonList polygon;
        public int nvmaps;
        public LinkedList<lwVMap> vmap;                /* linked list of vmaps */

        #region IDisposable Members

        public void Dispose()
        {
            if (point != null)
                point.Dispose();
            if (polygon != null)
                polygon.Dispose();
            if (vmap != null)
            {
                LinkedList<lwVMap>.Enumerator iter = vmap.GetEnumerator();
                while (iter.MoveNext())
                {
                    iter.Current.Dispose();
                }
            }
        }

        #endregion
    }

    /* tag strings */

    class lwTagList
    {
        public int count;
        public int offset;                  /* only used during reading */
        public string[] tag;                /* array of strings */

        /// <summary>
        /// Read tag strings from a TAGS chunk in an LWO2 file.  The tags are
        /// added to the lwTagList array.
        /// </summary>
        /// <param name="fp"></param>
        /// <param name="cksize"></param>
        /// <param name="tlist"></param>
        /// <param name="flen"></param>
        /// <returns></returns>
        public static bool GetTags(BinaryReader fp, int cksize, ref lwTagList tlist, ref int flen)
        {
            byte[] buf;
            int len, ntags;

            if (cksize == 0)
                return true;

            /* read the whole chunk */

            flen = 0;
            buf = lwio.getbytes(fp, cksize, ref flen);
            if (buf == null)
                return false;

            /* count the strings */

            ntags = 0;
            int bp = 0;
            while (bp < cksize)
            {
                string s = BitConverter.ToString(buf, bp);
                len = s.Length + 1;
                len += len & 1;
                bp += len;
                ++ntags;
            }

            /* expand the string array to hold the new tags */

            tlist.offset = tlist.count;
            tlist.count += ntags;
            tlist.tag = new string[tlist.count];
            if (tlist.tag == null)
                goto Fail;
            //memset( &tlist->tag[ tlist->offset ], 0, ntags * sizeof( char * ));

            /* copy the new tags to the tag array */

            bp = 0;
            for (int i = 0; i < ntags; i++)
                tlist.tag[i + tlist.offset] = lwio.sgetS0(buf, ref bp, ref flen);
            return true;

Fail:
            return false;
        }
    }
}