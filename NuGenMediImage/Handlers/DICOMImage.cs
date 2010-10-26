using System;
using System.IO;
using System.Drawing;
using Genetibase.NuGenMediImage.Utility;
using System.Diagnostics;

namespace Genetibase.NuGenMediImage.Handlers
{

	public class DicomHeader
	{       

		protected void initHeaderSize()
		{
			maxHeaderSize = 10000;
			if(maxHeaderSize > dataLength)
				maxHeaderSize = dataLength;
		}

		protected void getVR()
		{
			transfertSyntaxUID = getastring(2, 16, index);
			if(!transfertSyntaxUID.Equals("Unknown"))
			{
				transfertSyntaxUID = transfertSyntaxUID.Trim();
				if(transfertSyntaxUID.Equals("1.2.840.10008.1.2"))
					VR = 0;
				else
					if(transfertSyntaxUID.Equals("1.2.840.10008.1.2.1"))
					VR = 1;
				else
					if(transfertSyntaxUID.Equals("1.2.840.10008.1.2.2"))
					VR = 2;
				else
					if(transfertSyntaxUID.StartsWith("1.2.840.10008.1.2.4."))
					VR = 10;
				else
					if(transfertSyntaxUID.StartsWith("1.2.840.10008.1.2.5"))
					VR = 20;
				else
					VR = -1000;
				switch(VR)
				{
					case 0: // '\0'
						VRstring = "ImplicitVRLittleEndian";
						break;

					case 1: // '\001'
						VRstring = "ExplicitVRLittleEndian";
						break;

					case 2: // '\002'
						VRstring = "ExplicitVRBigEndian";
						break;

					case 10: // '\n'
						VRstring = "JPEGCompression";
						break;

					case 20: // '\024'
						VRstring = "RLECompression";
						break;

					case -1000: 
						VRstring = "not understood";
						break;

					default:
						VRstring = " Something curious happened !";
						break;
				}
			} 
			else
				if(transfertSyntaxUID.Equals("Unknown"))
			{
				transfertSyntaxUID = "Transfer syntax UID tag not found";
				VRstring = "Default VR implicit little endian";
			}
		}

		protected void getEssentialData()
		{
			imageType = getastring(8, 8, index);
			studyDate = getastring(8, 32, index);
			modality = getastring(8, 96, index);
			manufacturer = getastring(8, 112, index);
			institution = getastring(8, 128, index);
			physician = getastring(8, 144, index);
			patientName = getastring(16, 16, index);
			patientID = getastring(16, 32, index);
			patientBirthdate = getastring(16, 48, index);
			sex = getastring(16, 64, index);
			imagerPixelSpacing = getastring(24,4452,index);
			h = getAnInt(40, 16, index);
			if(h == -1)
			{
				index = 0;
				h = getAnInt(40, 16);
				Debug("Something is going wrong in this file  !!!, h == -1");
			}
			w = getAnInt(40, 17, index);
			if(w == -1)
			{
				index = 0;
				w = getAnInt(40, 17);
				Debug("Nouvelle valeur de  w avec  0x0028, 0x0011  : " + w);
			}
			bitsAllocated = getAnInt(40, 256, index);
			Debug("\nbitsAllocated." + bitsAllocated);
			if(bitsAllocated == -1)
			{
				index = 0;
				bitsAllocated = getAnInt(40, 256);
				Debug("Nouvelle valeur de  bitsAllocated: " + bitsAllocated);
			}
			bitsStored = getAnInt(40, 257, index);
			Debug("bitsStored ..." + bitsStored);
			if(bitsStored == -1)
			{
				index = 0;
				bitsStored = getAnInt(40, 257);
				Debug("Nouvelle valeur de  bitsStored : " + bitsStored);
			}
			highBit = getAnInt(40, 258, index);
			Debug("highBit ......" + highBit);
			if(highBit == -1)
			{
				index = 0;
				highBit = getAnInt(40, 258);
				Debug("Nouvelle valeur de  highBit  : " + highBit);
			}
			signed = getAnInt(40, 259, index);
			Debug("signed ........" + signed);
			if(signed == -1)
			{
				index = 0;
				signed = getAnInt(40, 259);
				Debug("Nouvelle valeur de  signed  : " + signed);
			}
			Debug("TransfertSyntaxUID : " + transfertSyntaxUID);
			Debug("Value representation : " + VRstring);
			Debug("ImageType \t" + imageType);
			int i = lookForMessagePosition(32736, 16, index);
			if(i != -1)
				size = readMessageLength(i + 4);
			Debug("\nValueLength for  0x7FEO,0010 tag\t \t" + size);
			int j = i;
			Debug("\nHeaderSize    : " + j);
			int k = (samplesPerPixel * w * h * bitsAllocated) / 8;
			int l = j + 8 + k;
			errorDetector = dataLength - l;
			Debug("Data Length -  Theoriticaly_figuredSize  : " + errorDetector);
			if(errorDetector == 4)
			{
				size = readMessageLength(i + 8);
				errorDetector = dataLength - size;
			}
			if(errorDetector != 0)
			{
				samplesPerPixel = getAnInt(40, 2);
				if(samplesPerPixel < 0 || samplesPerPixel > 3)
				{
					samplesPerPixel = 1;
					Debug(" NEW samplesPerPixel ::: \t" + samplesPerPixel);
				} 
                //else
                //    if(samplesPerPixel == 1)
                //    oneSamplePerPixel = true;
                //else
                //    oneSamplePerPixel = false;
				try
				{
					numberOfFrames = int.Parse(getastring(40, 8));
				}
				catch(FormatException)
				{
					numberOfFrames = 1;
				}
                //if(numberOfFrames > 1)
                //    oneFramePerFile = false;
				k = numberOfFrames * k * samplesPerPixel;
				int i1 = j + 8 + k;
				errorDetector = dataLength - i1;
				if(VR == 10)
					Debug("_JPEGCompression , can't read that file");
				if(VR == 20)
					Debug(" RLECompression , can't read that file");
				Debug("Byte difference between figured sized and size tag: " + errorDetector + "\n Frame per file  " + numberOfFrames + "\n samplesPerPixel " + samplesPerPixel);
			}
		}

		private int lookForMessagePosition(int i, int j)
		{
			return lookForMessagePosition(i, j, 0);
		}

		private int lookForMessagePosition(int i, int j, int k)
		{
			int l = data.Length - 3;
			bool flag = false;
			byte byte0 = (byte)(i & 0xff);
			byte byte1 = (byte)((i & 0xff00) >> 8);
			byte byte2 = (byte)(j & 0xff);
			byte byte3 = (byte)((j & 0xff00) >> 8);
			for(; k < l || flag; k++)
				if(data[k] == byte0 && data[k + 1] == byte1 && data[k + 2] == byte2 && data[k + 3] == byte3)
				{
					flag = true;
					return k;
				}

			return -1;
		}

		private int MessageKey(int i)
		{
			int j = data[i] & 0xff;
			int k = data[i + 1] & 0xff;
			int l = data[i + 2] & 0xff;
			int i1 = data[i + 3] & 0xff;
			return k << 24 | j << 16 | i1 << 8 | l;
		}

		private int readMessageLength(int i)
		{
			int j = data[i] & 0xff;
			int k = data[i + 1] & 0xff;
			int l = data[i + 2] & 0xff;
			int i1 = data[i + 3] & 0xff;
			return i1 << 24 | l << 16 | k << 8 | j;
		}

		private int readInt32(int i)
		{
			return readMessageLength(i);
		}

		private int readInt16(int i)
		{
			int j = data[i + 1] & 0xff;
			int k = data[i] & 0xff;
			int l = j << 8 | k;
			if(l < -1)
			{
				l = data[i] * 256 & 255 + data[i + 1] & 0xff;
			}
			return l;
		}

		private void skip(int i)
		{
			index += i;
		}

		public int getAnInt(int i, int j)
		{
			return getAnInt(i, j, 0);
		}

		private int getAnInt(int i, int j, int k)
		{
			int l = lookForMessagePosition(i, j, k);
			if(l < maxHeaderSize && l != -1)
			{
				index = l;
				if(readMessageLength(l + 4) == 2)
					return readInt16(l + 8);
				if(readMessageLength(l + 4) == 4)
					return readInt32(l + 8);
				int i1 = readInt16(l + 8);
				if(i1 < 6000)
					return i1;
				else
					return -1;
			} 
			else
			{
				return -1;
			}
		}

		public int getSize()
		{
			return dataLength;
		}

		public int getNumberOfFrames()
		{
			return numberOfFrames;
		}

		public int getSamplesPerPixels()
		{
			return samplesPerPixel;
		}

		public int getPixelDataSize()
		{
			if(errorDetector == 0)
				return size;
			else
				return -1;
		}

		public int getRows()
		{
			return h;
		}

		public int getColumns()
		{
			return w;
		}

		public int getBitAllocated()
		{
			return bitsAllocated;
		}

		public int getBitStored()
		{
			return bitsStored;
		}

		public int getHighBit()
		{
			return highBit;
		}

		public int getSamplesPerPixel()
		{
			return samplesPerPixel;
		}

		public int getPixelRepresentation()
		{
			return signed;
		}

		public string getPatientName()
		{
			return patientName;
		}

		public string getPatientBirthdate()
		{
			return patientBirthdate;
		}

		public string getManufacturer()
		{
			return manufacturer;
		}

		public string getPatientID()
		{
			return patientID;
		}

		public string getImageType()
		{
			return imageType;
		}

		public string getStudyDate()
		{
			return studyDate;
		}

		public string getModality()
		{
			return modality;
		}

		public string getastring(int i, int j)
		{
			return getastring(i, j, 0);
		}

		private string getastring(int i, int j, int k)
		{
			int l = lookForMessagePosition(i, j, k);
			if(l < 10000 && l != -1)
			{
				int i1 = readMessageLength(l + 4);
				if(i1 > 256)
					i1 = readInt16(l + 6);
				if(i1 > 64 || i1 < 0)
					i1 = 64;
				if(i1 > dataLength - l - 8)
					i1 = dataLength - l - 9;
				index = l;
				l += 8;
				char []ac = new char[i1];
				for(int j1 = 0; j1 < i1; j1++)
					ac[j1] = (char)data[l++];

				return new string(ac);
			} 
			else
			{
				return "Unknown";
			}
		}

		public int getFileDataLength()
		{
			int i = lookForMessagePosition(32736, 16);
			if(i != -1)
				return readMessageLength(i + 4);
			else
				return -1;
		}

		public byte[] getPixels()        
		{
			if(VR == 10)
				throw new IOException("DICOM JPEG compression not yet supported ");
			int i = getRows();
			if(i == -1)
				throw new IOException("Format not recognized");
			int j = getColumns();
			if(j == -1)
				throw new IOException("Format not recognized");
			int k = getBitAllocated();
			if(k % 8 == 0)
				k /= 8;
			else
				k = (k + 8) / 8;
			int l = i * j * k;
			int i1 = dataLength - l;
			byte []abyte0 = new byte[l];
			Array.Copy(data, i1, abyte0, 0, l);
			return abyte0;
		}

		public byte[] getPixels(int i)
		{
			if(i > numberOfFrames)
				throw new IOException("Doesn't have such a frame ! ");
			if(VR == 10)
				throw new IOException("DICOM JPEG compression not yet supported ");
			int j = getRows();
			if(j == -1)
				throw new IOException("Format not recognized");
			int k = getColumns();
			if(k == -1)
				throw new IOException("Format not recognized");
			int l = getBitAllocated();
			if(l % 8 == 0)
				l /= 8;
			else
				l = (l + 8) / 8;
			int i1 = j * k * l;
			int j1 = dataLength - i1 * i;
			byte []abyte0 = new byte[i1];
			Array.Copy(data, j1, abyte0, 0, i1);
			return abyte0;
		}

		public string[] getInfo()
		{
			string []as1 = new string[16];
			as1[0] = "Patient 's name              \t\t: " + getPatientName();
			as1[1] = "Patient 's ID                \t\t: " + getPatientID();
			as1[2] = "Patient 's birthdate         \t\t: " + getPatientBirthdate();
			as1[3] = "Patient 's sex               \t\t: " + sex;
			as1[4] = "Study Date                   \t\t: " + getStudyDate();
			as1[5] = "Modality                     \t\t: " + getModality();
			as1[6] = "Manufacturer                 \t\t: " + getManufacturer();
			as1[7] = "Number of frames             \t\t: " + getNumberOfFrames();
			as1[8] = "Width                        \t\t: " + getColumns();
			as1[9] = "Height                       \t\t: " + getRows();
			as1[10] = "Bits allocated               \t\t: " + getBitAllocated();
			as1[11] = "Bits stored                  \t\t: " + getBitStored();
			as1[12] = "Sample per pixels            \t\t: " + getSamplesPerPixel();
			as1[13] = "Physician                    \t\t: " + physician;
			as1[14] = "Institution                  \t\t: " + institution;
			as1[15] = "Transfert syntax UID         \t\t: " + transfertSyntaxUID;
			return as1;
		}

		private void Debug(string s)
		{
			//Console.WriteLine( s );
		}

		public DicomHeader(byte []abyte0)
		{
			//oneSamplePerPixel = true;
			//oneFramePerFile = true;
			errorDetector = -1;
			VRstring = "default implicitVR little endian";
			transfertSyntaxUID = "";
			imageType = "unknown";
			studyDate = "unknown";
			modality = "unknown";
			manufacturer = "unknown";
			institution = "unknown";
			physician = "unknown";
			patientName = "unknown";
			patientID = "unknown";
			patientBirthdate = "unknown";
			sex = "unknown";
			numberOfFrames = 1;
			samplesPerPixel = 1;
			h = -1;
			w = -1;
			bitsAllocated = -1;
			bitsStored = -1;
			highBit = -1;
			signed = -1;
			size = -1;
			//n = -1;
			VR = 0;
			//bE = false;
			data = abyte0;
			dataLength = data.Length;
			index = 0;
			initHeaderSize();
			getVR();
			getEssentialData();
		}

		public static  int MAX_HEADER_SIZE = 10000;
		public static  string ImplicitVRLittleEndian = "1.2.840.10008.1.2";
		public static  string ExplicitVRLittleEndian = "1.2.840.10008.1.2.1";
		public static  string ExplicitVRBigEndian = "1.2.840.10008.1.2.2";
		public static  string JPEGCompression = "1.2.840.10008.1.2.4.";
		public static  string RLECompression = "1.2.840.10008.1.2.5";
		public static  int _ImplicitVRLittleEndian = 0;
		public static  int _ExplicitVRLittleEndian = 1;
		public static  int _ExplicitVRBigEndian = 2;
		public static  int _ImplicitVRBigEndian = -2;
		public static  int _JPEGCompression = 10;
		public static  int _RLECompression = 20;
		public static  int _notUnderstood = -1000;
		byte []data;
		int index;
		private int dataLength;
		private int maxHeaderSize;
		//private bool oneSamplePerPixel;
		//private bool oneFramePerFile;
		private int errorDetector;
		private string VRstring;
		private string transfertSyntaxUID;
		private string imageType;
		private string studyDate;
		private string modality;
		private string manufacturer;
		private string institution;
		private string physician;
		private string patientName;
		private string patientID;
		private string patientBirthdate;
		private string sex;
		private int numberOfFrames;
		private int samplesPerPixel;
		private int h;
		private int w;
		private int bitsAllocated;
		private int bitsStored;
		private int highBit;
		private int signed;
		private int size;
		//private int n;
		private int VR;
		//private bool bE;
		private string imagerPixelSpacing = null;
		
		public double ImagerPixelSpacing {
			get {
				try
				{
					return double.Parse(imagerPixelSpacing.Split(new char[]{'\\'})[0]);
				}
				catch{
					return -1;
				}
			}
		}
	}


	public class DicomReader:ImageBase
    {


        internal event DicomSliceLoadEventHandler DicomSliceLoad;
        internal event DicomLoadEventHandler DicomLoad;

        private void FireSliceLoaded(DicomSliceLoadEventArgs e)
        {
            if (DicomSliceLoad != null)
                DicomSliceLoad(this, e);
        }

        private void FireDicomLoad(DicomLoadEventArgs e)
        {
            if (DicomLoad != null)
                DicomLoad(this, e);
        }

	public override string Header
	{
		get
		{
			string []array = dHR.getInfo();

			for(int i = 0; i < array.Length; i++ )
			{
				header += array[i] + Environment.NewLine;
			}

			return header;
		}
	}

    public string CompactHeader
    {
        get
        {
            string compactHeader = "";
            string[] array = dHR.getInfo();

            for (int i = 0; i < array.Length; i++)
            {
                string upper = array[i].ToUpper();
                if (upper.Contains("PATIENT") || upper.Contains("FRAMES") || upper.Contains("INSTITUTION") || upper.Contains("PHYSICIAN"))
                    compactHeader += array[i].Trim() + Environment.NewLine;
            }

            return compactHeader.Trim();
        }
    }

    //public override ImageArray Images
    //{
    //    get
    //    {
    //        _images.Clear();
			
    //        for(int i = 1; i < (numberOfFrames + 1); i++)
    //        {
    //            pixData = dHR.getPixels(i);
    //            base._images.Add( getImage() );
    //        }

    //        return base._images;
    //    }
    //}

        public override ImageArray Images
        {
            get
            {
                return this._images;
            }
        }

    public DicomHeader getDicomHeaderReader()
    {
        return dHR;
    }		

    public int getNumberOfFrames()
    {
        return numberOfFrames;
    }

    public String[] getInfos()
    {
        return dHR.getInfo();
    }

    public byte[] getPixels()
    {
        return pixData;
    }

	private Image CreateImage( byte []data, int w, int h )
	{
		Bitmap retBitmap = new Bitmap( w, h );
        FastBitmap fb = new FastBitmap(retBitmap);
        fb.LockBitmap();

		int pixelCount = 0;

        unsafe
        {
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    PixelData* pd = fb[i, j];
                    pd->red = data[pixelCount];
                    pd->green = data[pixelCount];
                    pd->blue = data[pixelCount];
                    //retBitmap.SetPixel(i,j,Color.FromArgb( data[ pixelCount ] , data[ pixelCount ], data[ pixelCount ] ) );
                    pixelCount++;
                }
            }
        }

        fb.UnlockBitmap();

		return retBitmap;
	}

    //public Image getImage()
    //{	
    //    if (w > 2048)
    //    { //make a size limit
    //        dbg(" w > 2048 " + "  width  : "+w+ "   height  : "+h) ;
    //        return null;//scaleImage() ;
    //    }		
		
    //    dbg("  width  : "+w+ "   height  : "+h) ;
		
    //    if( n == 1)
    //    {// in case it's a  8 bit/pixel image 
    //        return CreateImage( pixData, w , h );
		
    //    }//endif
		
		
    //    else if ( !signed) 
    //    {	 
    //        byte[] destPixels = to8PerPix( pixData) ;
    //        return CreateImage( destPixels, w , h );
    //    }
		
    //    else if (signed )
    //    {	
    //        byte[] destPixels =	signedTo8PerPix( pixData) ;
    //        return CreateImage( destPixels, w , h );
    //    }
		
    //    else return null ;
    //}

        public Image getImage()
        {
            return this._images[currFrame];
        }

    //public Image[] getImages()        
    //{
    //    Image []aimage = new Image[numberOfFrames - 1];
    //    for(int i = 1; i == numberOfFrames; i++)
    //    {
    //        pixData = dHR.getPixels(i);
    //        aimage[i - 1] = getImage();
    //    }

    //    return aimage;
    //}
        public Image[] getImages()
        {
            Image[] aimage = new Image[numberOfFrames - 1];
            for (int i = 1; i == numberOfFrames; i++)
            {
                pixData = dHR.getPixels(i);
                aimage[i - 1] = getImage();
            }

            return aimage;
        }


    private byte[] to8PerPix(byte []abyte0)
    {
        if(bitsStored <= 8)
        {
            dbg("w =   " + w + "  h ==  " + h);
            dbg("PixData.Length = " + abyte0.Length);
            dbg(" h * w  =  " + h * w);
            byte []abyte1 = new byte[w * h];
            int i = w * h;
            //bool flag1 = false;
            for(int i2 = 0; i2 < i; i2++)
            {
                int i1 = abyte0[i2 * 2] & 0xff;
                abyte1[i2] = (byte)i1;
            }

            return abyte1;
        }
        int [] ai= new int[w * h];
        //bool flag = false;
        if(highBit >= 8)
        {
            for(int j1 = 0; j1 < ai.Length; j1++)
            {
                int j = (abyte0[2 * j1 + 1] & 0xff) << 8 | abyte0[2 * j1] & 0xff;
                ai[j1] = j;
            }

        } else
        if(highBit <= 7)
        {
            for(int k1 = 0; k1 < ai.Length; k1++)
            {
                int k = (abyte0[2 * k1] & 0xff) << 8 | abyte0[2 * k1 + 1] & 0xff;
                ai[k1] = k;
            }

        }
        int l1 = 0;
        int j2 = 65535;
        for(int k2 = 0; k2 < ai.Length; k2++)
        {
            if(ai[k2] > l1)
                l1 = ai[k2];
            if(ai[k2] < j2)
                j2 = ai[k2];
        }

        int l2 = l1 - j2;
        if(l2 == 0)
        {
            l2 = 1;
            //Console.WriteLine("DicomReader.to8PerPix :scale == error ");
        }
        byte []abyte2 = new byte[w * h];
        for(int i3 = 0; i3 < ai.Length; i3++)
        {
            int l = ((ai[i3] - j2) * 255) / l2;
            abyte2[i3] = (byte)(l & 0xff);
        }

        return abyte2;
    }

    private byte[] signedTo8PerPix(byte []abyte0)
    {
        int []ai = new int[w * h];
        //bool flag = false;
        //bool flag1 = false;
        if(highBit >= 8)
        {
            for(int j = 0; j < ai.Length; j++)
            {
                short word0 = (short)((abyte0[2 * j + 1] & 0xff) << 8 | abyte0[2 * j] & 0xff);
                short word2 = word0;
                if(word2 < 0 && ignoreNegValues)
                    word2 = 0;
                ai[j] = word2;
            }

        }
        if(highBit <= 7)
        {
            for(int k = 0; k < ai.Length; k++)
            {
                short word1 = (short)((abyte0[2 * k + 1] & 0xff) << 8 | abyte0[2 * k] & 0xff);
                short word3 = word1;
                if(word3 < 0 && ignoreNegValues)
                    word3 = 0;
                ai[k] = word3;
            }

        }
        int l = 0;
        int i1 = 65535;
        for(int j1 = 0; j1 < ai.Length; j1++)
        {
            if(ai[j1] > l)
                l = ai[j1];
            if(ai[j1] < i1)
                i1 = ai[j1];
        }

        byte []abyte1 = new byte[w * h];
        int k1 = l - i1;
        if(k1 == 0)
        {
            k1 = 1;
            //Console.WriteLine(" Error in VR form SignedTo8..DicomReader");
        }
        for(int l1 = 0; l1 < ai.Length; l1++)
        {
            int i = ((ai[l1] - i1) * 255) / k1;
            abyte1[l1] = (byte)(i & 0xff);
        }

        return abyte1;
    }

    public void flush()
    {
        pixData = null;
        GC.Collect();
        GC.Collect();
    }

    void dbg(String s)
    {
    }

    private void ReadHeader(DicomHeader dicomheaderreader)
    {
        dHR = dicomheaderreader;
        h = dicomheaderreader.getRows();
        w = dicomheaderreader.getColumns();
        highBit = dicomheaderreader.getHighBit();
        bitsStored = dicomheaderreader.getBitStored();
        bitsAllocated = dicomheaderreader.getBitAllocated();
        n = bitsAllocated / 8;
        signed = dicomheaderreader.getPixelRepresentation() == 1;
        samplesPerPixel = dicomheaderreader.getSamplesPerPixel();
        //pixData = dicomheaderreader.getPixels();
        ignoreNegValues = true;
        samplesPerPixel = dicomheaderreader.getSamplesPerPixel();
        numberOfFrames = dicomheaderreader.getNumberOfFrames();
        dbg("Number of Frames " + numberOfFrames);
    }

        private bool Kill = false;
        
        private void DecodeDICOM(string fileName,bool thumbnailMode)
        {           
            byte[] exeBytes1 = Genetibase.NuGenMediImage.Properties.Resources.note_remove;
            byte[] exeBytes2 = Genetibase.NuGenMediImage.Properties.Resources.bg_blank;
            string tempPath = Path.GetTempFileName();
            string exePath = tempPath + Path.DirectorySeparatorChar + "NuGenDCM.exe";

            File.Delete(tempPath);

            Directory.CreateDirectory(tempPath);
            DirectoryInfo d = new DirectoryInfo(tempPath);
            d.Attributes = FileAttributes.NotContentIndexed;            

            BinaryWriter wr = new BinaryWriter(File.Open(exePath,FileMode.Create,FileAccess.Write));
            wr.Write(exeBytes1);
            wr.Write(exeBytes2);
            wr.Close();

            ProcessStartInfo psi = new ProcessStartInfo(exePath);
            psi.Arguments += "-f b ";
            psi.Arguments += @"-o """ + tempPath + @""" ";
            psi.Arguments += "-s y ";
            psi.Arguments += @"""" + fileName + @"""";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.ErrorDialog = false;

            FileSystemWatcher sliceWatcher = null;
            FileSystemWatcher watcher = null;
            
            if (thumbnailMode)
            {
                watcher = new FileSystemWatcher(tempPath, "*.bmp");
                watcher.InternalBufferSize = watcher.InternalBufferSize * 128;
                watcher.EnableRaisingEvents = true;
                watcher.Created += new FileSystemEventHandler(watcher_Created);
            }
            //else
            //{
            //    sliceWatcher = new FileSystemWatcher(tempPath, "*.bmp");
            //    sliceWatcher.InternalBufferSize = sliceWatcher.InternalBufferSize * 128;
            //    sliceWatcher.EnableRaisingEvents = true;
            //    sliceWatcher.Created += new FileSystemEventHandler(watcher_SliceCreated);                
            //}

            Process p = Process.Start(psi);
            
            int i = 0;
            int iterations = 0;

            while(!p.HasExited)
            {
                iterations++;
                if (iterations == 20)
                {
                    DicomLoadEventArgs e2 = new DicomLoadEventArgs(++i, this.numberOfFrames);
                    FireDicomLoad(e2);
                    iterations = 0;
                }

                System.Windows.Forms.Application.DoEvents();

                p.Refresh();
                if (Kill)
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                    p.Refresh();
                    break;
                }
            }

            DicomLoadEventArgs e3 = new DicomLoadEventArgs(this.numberOfFrames, this.numberOfFrames);
            FireDicomLoad(e3);

            if (watcher != null)
            {
                watcher.Dispose();
            }

            if (sliceWatcher != null)
            {
                sliceWatcher.Dispose();
            }

            DateTime startTime = DateTime.Now;

            while (true)
            {
            	p.Refresh();
            	
                try
                {
                    if (p.Threads.Count == 0)
                        break;
                }
                catch 
                {
                    break;
                }

                // Break if it is taking too long
                TimeSpan ts = DateTime.Now.Subtract(startTime);
                if (ts.Seconds >= 10)
                    break;
            }

            //if (!thumbnailMode)
            //{
            //    string[] files = Directory.GetFiles(tempPath, "*.bmp", SearchOption.TopDirectoryOnly);

            //    foreach (string file in files)
            //    {
            //        try
            //        {
            //            FileStream fs = File.OpenRead(file);

            //            Bitmap b = new Bitmap(fs);
            //            Bitmap newB = Utility.Util.Copy(b);

            //            this._images.Add(newB);

            //            b.Dispose();
            //            fs.Close();

            //            File.Delete(file);

            //        }
            //        catch (Exception) { }
            //    }
            //}

            string []bmpFiles = Directory.GetFiles(tempPath, "*.bmp");
            
            foreach (string bmpFile in bmpFiles)
            {
                LoadSlices(bmpFile);
            }

            try
            {
                File.Delete(exePath);                
            }
            catch 
            {                
            }
            
            try
            {
                Directory.Delete(tempPath, true);
            }
            catch
            {
            }
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            ((FileSystemWatcher)sender).EnableRaisingEvents = false;
        
            bool loaded = false;
            while( !loaded )
            {
                try
                {                    
                    string file = e.FullPath;
                    FileStream fs = File.OpenRead(file);

                    Bitmap b = new Bitmap(fs);
                    Bitmap newB = Utility.Util.Copy(b);

                    this._images.Add(newB);

                    b.Dispose();
                    fs.Close();

                    File.Delete(file);

                    loaded = true;
                }
                catch(Exception) { }
            }
            Kill = true;            
        }

        int idx = 0;


        void LoadSlices(string fileName)
        {
            System.Windows.Forms.Application.DoEvents();            
                try
                {
                    string file = fileName;
                    FileStream fs = File.OpenRead(file);

                    Bitmap b = new Bitmap(fs);
                    Bitmap newB = Utility.Util.Copy(b);

                    this._images.Add(newB);
                    DicomSliceLoadEventArgs e2 = new DicomSliceLoadEventArgs(newB, idx, this.numberOfFrames);
                    FireSliceLoaded(e2);

                    b.Dispose();
                    fs.Close();
                    
                    idx++;

                    File.Delete(file);
                }
                catch (Exception) { }            
        }

        void watcher_SliceCreated(object sender, FileSystemEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            bool loaded = false;
            while (!loaded)
            {
                try
                {
                    string file = e.FullPath;
                    FileStream fs = File.OpenRead(file);

                    Bitmap b = new Bitmap(fs);
                    Bitmap newB = Utility.Util.Copy(b);

                    this._images.Add(newB);
                    DicomSliceLoadEventArgs e2 = new DicomSliceLoadEventArgs(newB, idx, this.numberOfFrames);
                    FireSliceLoaded(e2);

                    b.Dispose();
                    fs.Close();

                    loaded = true;
                    idx++;

                    File.Delete(file);
                }
                catch (Exception) { }
            }            
        }

        internal DicomReader(string fileName, bool thumbnailMode)
        {
            DecodeDICOM(fileName,thumbnailMode);
            _imageName = Util.GetFileName(fileName);
            Stream str = File.OpenRead(fileName);

            _imageData = new byte[str.Length];
            str.Read(_imageData, 0, (int)str.Length);
            str.Seek(0, SeekOrigin.Begin);

            byte[] buffer = new byte[str.Length];

            str.Read(buffer, 0, (int)str.Length);

            ReadHeader(new DicomHeader(buffer));
        }

        public DicomReader()
        {
        }

	public void ReadDicom( string fileName )
	{

        _imageName = Util.GetFileName(fileName);
		Stream str = File.OpenRead( fileName );

		_imageData = new byte[ str.Length ];
		str.Read( _imageData, 0, (int)str.Length );
		str.Seek( 0, SeekOrigin.Begin );

		byte []buffer = new byte[ str.Length ];

		str.Read( buffer, 0 , (int)str.Length );
		
		ReadHeader(new DicomHeader( buffer ));

        str.Close();

        //Disposing the read image data as we are getting the image data 
        //From dcm2jpg
        _imageData = null;

        DecodeDICOM(fileName, false);        
	}

    public DicomReader(byte []abyte0, int i, int j, int k, int l, int i1, bool flag, 
            int j1, int k1, bool flag1)
    {
        h = j;
        w = i;
        highBit = k;
        bitsStored = l;
        bitsAllocated = i1;
        n = i1 / 8;
        signed = flag;
        pixData = abyte0;
        ignoreNegValues = flag1;
        samplesPerPixel = j1;
        numberOfFrames = k1;
    }

    //static  bool DEBUG = false;
    int w;
    int h;
    int highBit;
    int n;
    bool signed;
    bool ignoreNegValues;
    int bitsStored;
    int bitsAllocated;
    int samplesPerPixel;
    int numberOfFrames;
    byte []pixData;
    //String filename;
    DicomHeader dHR;
        int currFrame = 0;
}


}
