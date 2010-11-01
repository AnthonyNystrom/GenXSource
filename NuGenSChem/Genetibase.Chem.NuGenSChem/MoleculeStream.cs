using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
//UPGRADE_TODO: The package 'java.nio.channels' could not be found. If it was not included in the conversion, there may be compiler issues. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1262'"
// using java.nio.channels;

namespace Genetibase.Chem.NuGenSChem
{
	
	/*
	Handles reading and writing of molecules to and from streams. Two formats are supported: native, which is a direct translation of
	the underlying data content; and a subset of MDL MOL, using only the fields that are relevant to */
	
	public class MoleculeStream
	{
		// special implementation of the reader for when the format is not known a-priori, or might be a combination-of-two formats
		// as used by the clipboard; do some extra work to try to pull out the SketchEl file preferentially
		public static Molecule ReadUnknown(System.IO.Stream istr)
		{
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.StreamReader.StreamReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
// 			return ReadUnknown(new System.IO.StreamReader(new System.IO.StreamReader(istr, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(istr, System.Text.Encoding.Default).CurrentEncoding));
            using (StreamReader rdr = new StreamReader(istr))
            {
                return ReadUnknown(rdr);
            }
		}

        public static Molecule ReadUnknown(StreamReader input)
		{
			Molecule mdlmol = null, elmol = null;
			const int BUFFMAX = 100000;
			//UPGRADE_ISSUE: Method 'java.io.StreamReader.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReadermark_int'"
			// in_Renamed.mark(BUFFMAX);
			try
			{
				mdlmol = ReadMDLMOL(input);
				if (mdlmol != null)
				{
					//UPGRADE_ISSUE: Method 'java.io.StreamReader.mark' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReadermark_int'"
					// in_Renamed.mark(BUFFMAX);

				} // so the SketchEl version could follow
			}
			catch (System.IO.IOException e)
			{
				mdlmol = null;
				//UPGRADE_ISSUE: Method 'java.io.StreamReader.reset' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderreset'"
                // TODO: Check to see if this is reused, the reset could be expected 
                // input.reset();
			}
			
			try
			{
				elmol = ReadNative(input);
			}
			catch (System.IO.IOException e)
			{
				elmol = null;
			}
			
			if (elmol != null)
				return elmol;
			if (mdlmol != null)
				return mdlmol;
			
			throw new System.IO.IOException("Unknown or invalid format.");
		}
		
		public static Molecule ReadNative(System.IO.Stream istr)
		{
			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.StreamReader.StreamReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			return ReadNative(new System.IO.StreamReader(new System.IO.StreamReader(istr, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(istr, System.Text.Encoding.Default).CurrentEncoding));
		}
		public static Molecule ReadNative(System.IO.StreamReader in_Renamed)
		{
			Molecule mol = new Molecule();
			//UPGRADE_NOTE: Final was removed from the declaration of 'GENERIC_ERROR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			System.String GENERIC_ERROR = "Invalid SketchEl file.";
			
			try
			{
				System.String line = in_Renamed.ReadLine();
				if (!line.StartsWith("SketchEl!"))
					throw new System.IO.IOException("Not a SketchEl file.");
				int p1 = line.IndexOf('('), p2 = line.IndexOf(','), p3 = line.IndexOf(')');
				if (p1 == 0 || p2 == 0 || p3 == 0)
					throw new System.IO.IOException(GENERIC_ERROR);
				
				int numAtoms = System.Int32.Parse(line.Substring(p1 + 1, (p2) - (p1 + 1)).Trim());
				int numBonds = System.Int32.Parse(line.Substring(p2 + 1, (p3) - (p2 + 1)).Trim());
				for (int n = 0; n < numAtoms; n++)
				{
					line = in_Renamed.ReadLine();
                    // TODO: verify java's split method syntax: "[\\=\\,\\;]"
					System.String[] bits = line.Split('=', ',', ';');
					if (bits.Length < 5)
						throw new System.IO.IOException(GENERIC_ERROR);
					int num = mol.AddAtom(bits[0], System.Double.Parse(bits[1].Trim()), System.Double.Parse(bits[2].Trim()), System.Int32.Parse(bits[3].Trim()), System.Int32.Parse(bits[4].Trim()));
					if (bits.Length >= 6 && bits[5].Length > 0 && bits[5][0] == 'e')
						mol.SetAtomHExplicit(num, System.Int32.Parse(bits[5].Substring(1)));
				}
				for (int n = 0; n < numBonds; n++)
				{
					line = in_Renamed.ReadLine();
					System.String[] bits = line.Split('-', '=', ','); // "[\\-\\=\\,]");
					if (bits.Length < 4)
						throw new System.IO.IOException(GENERIC_ERROR);
					mol.AddBond(System.Int32.Parse(bits[0].Trim()), System.Int32.Parse(bits[1].Trim()), System.Int32.Parse(bits[2].Trim()), System.Int32.Parse(bits[3].Trim()));
				}
				line = in_Renamed.ReadLine();
				if (String.CompareOrdinal(line, "!End") != 0)
					throw new System.IO.IOException(GENERIC_ERROR);
			}
			catch (System.Exception)
			{
				throw new System.IO.IOException(GENERIC_ERROR);
			}
			
			return mol;
		}
		
		public static void  WriteNative(System.IO.Stream ostr, Molecule mol)
		{
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			WriteNative(new System.IO.StreamWriter(new System.IO.StreamWriter(ostr, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(ostr, System.Text.Encoding.Default).Encoding), mol);
		}
		public static void  WriteNative(System.IO.StreamWriter output, Molecule mol)
		{
			//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			//UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			// DecimalFormat fmt = new DecimalFormat("0.0000");
            string fmtStr = "N4";
			
			output.Write("SketchEl!(" + mol.NumAtoms() + "," + mol.NumBonds() + ")\n");
			for (int n = 1; n <= mol.NumAtoms(); n++)
			{
				System.String hy = mol.AtomHExplicit(n) != Molecule.HEXPLICIT_UNKNOWN?("e" + mol.AtomHExplicit(n)):("i" + mol.AtomHydrogens(n));
                output.Write(mol.AtomElement(n) + "=" + mol.AtomX(n).ToString(fmtStr) + "," + mol.AtomY(n).ToString(fmtStr) + ";" + mol.AtomCharge(n) + "," + mol.AtomUnpaired(n) + "," + hy + "\n");
			}
			for (int n = 1; n <= mol.NumBonds(); n++)
			{
				output.Write(mol.BondFrom(n) + "-" + mol.BondTo(n) + "=" + mol.BondOrder(n) + "," + mol.BondType(n) + "\n");
			}
			output.Write("!End\n");
			
			output.Flush();
		}
		
		public static Molecule ReadMDLMOL(StreamReader in_Renamed)
		{
			Molecule mol = new Molecule();
			//UPGRADE_NOTE: Final was removed from the declaration of 'GENERIC_ERROR '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			System.String GENERIC_ERROR = "Invalid MDL MOL file.";
			
			try
			{
				System.String line = null;
				for (int n = 0; n < 4; n++)
					line = in_Renamed.ReadLine();
				if (line == null || !line.Substring(34, (39) - (34)).Equals("V2000"))
					throw new System.IO.IOException(GENERIC_ERROR);
				int numAtoms = System.Int32.Parse(line.Substring(0, (3) - (0)).Trim());
				int numBonds = System.Int32.Parse(line.Substring(3, (6) - (3)).Trim());
				
				for (int n = 0; n < numAtoms; n++)
				{
					line = in_Renamed.ReadLine();
					
					double x = System.Double.Parse(line.Substring(0, (10) - (0)).Trim());
					double y = System.Double.Parse(line.Substring(10, (20) - (10)).Trim());
					System.String el = line.Substring(31, (33) - (31)).Trim();
					int chg = System.Int32.Parse(line.Substring(36, (39) - (36)).Trim()), rad = 0;
					
					if (chg <= 3)
					{
					}
					else if (chg == 4)
					{
						chg = 0; rad = 2;
					}
					else
						chg = 4 - chg;
					
					mol.AddAtom(el, x, y, chg, rad);
				}
				for (int n = 0; n < numBonds; n++)
				{
					line = in_Renamed.ReadLine();
					
					int from = System.Int32.Parse(line.Substring(0, (3) - (0)).Trim()), to = System.Int32.Parse(line.Substring(3, (6) - (3)).Trim());
					int type = System.Int32.Parse(line.Substring(6, (9) - (6)).Trim()), stereo = System.Int32.Parse(line.Substring(9, (12) - (9)).Trim());
					
					if (from == to || from < 1 || from > numAtoms || to < 1 || to > numAtoms)
						throw new System.IO.IOException(GENERIC_ERROR);
					
					int order = type >= 1 && type <= 3?type:1;
					int style = Molecule.BONDTYPE_NORMAL;
					if (stereo == 1)
						style = Molecule.BONDTYPE_INCLINED;
					else if (stereo == 6)
						style = Molecule.BONDTYPE_DECLINED;
					else if (stereo == 3 || stereo == 4)
						style = Molecule.BONDTYPE_UNKNOWN;
					
					mol.AddBond(from, to, order, style);
				}
				while (true)
				{
					line = in_Renamed.ReadLine();
					if (line.StartsWith("M  END"))
						break;
					
					int type = 0;
					if (line.StartsWith("M  CHG"))
						type = 1;
					else if (line.StartsWith("M  RAD"))
						type = 2;
					if (type > 0)
					{
						int len = System.Int32.Parse(line.Substring(6, (9) - (6)).Trim());
						for (int n = 0; n < len; n++)
						{
							int apos = System.Int32.Parse(line.Substring(9 + 8 * n, (13 + 8 * n) - (9 + 8 * n)).Trim());
							int aval = System.Int32.Parse(line.Substring(13 + 8 * n, (17 + 8 * n) - (13 + 8 * n)).Trim());
							if (type == 1)
								mol.SetAtomCharge(apos, aval);
							else
								mol.SetAtomUnpaired(apos, aval);
						}
					}
				}
			}
			catch (System.Exception)
			{
				throw new System.IO.IOException(GENERIC_ERROR);
			}
			
			
			return mol;
		}
        internal static Molecule ReadXML(FileStream istr)
        {
            Molecule mol = new Molecule();

            const string NAMESPACE = "http://www.xml-cml.org/schema/cml2/core";
            const string ATTRIBUTENS = ""; 
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(istr);

            XPathNavigator navigator = xmlDoc.CreateNavigator();

            if (navigator.MoveToFirstChild())
            {
                navigator.MoveToChild("molecule", NAMESPACE);                
                //navigator.MoveToFirstChild();
                
                if (navigator.MoveToChild("atomArray", NAMESPACE))
                {
                    if (navigator.MoveToChild("atom", NAMESPACE))
                    {
                        do
                        {
                            string elementType= navigator.GetAttribute("elementType", ATTRIBUTENS);

                            string x2String = navigator.GetAttribute("x2", ATTRIBUTENS);
                            if (!string.IsNullOrEmpty(x2String))
                            {
                                double x2 = Utility.SafeDouble(x2String);
                                double y2 = Utility.SafeDouble(navigator.GetAttribute("y2", ATTRIBUTENS));
                                int hydrogenCount = Utility.SafeInt(navigator.GetAttribute("hydrogenCount", ATTRIBUTENS));

                                // TODO: How do we handle hydrogen count? 
                                mol.AddAtom(elementType, x2, y2);
                            }
                            else
                            {
                                double x3 = Utility.SafeDouble(navigator.GetAttribute("x3", ATTRIBUTENS));
                                double y3 = Utility.SafeDouble(navigator.GetAttribute("y3", ATTRIBUTENS));
                                int hydrogenCount = Utility.SafeInt(navigator.GetAttribute("hydrogenCount", ATTRIBUTENS));

                                // TODO: How do we handle hydrogen count? 
                                mol.AddAtom(elementType, x3, y3);
                            }

                        } while (navigator.MoveToNext("atom", NAMESPACE));

                        navigator.MoveToParent();
                    }

                    navigator.MoveToParent();
                }
                if (navigator.MoveToChild("bondArray", NAMESPACE))
                {
                    if (navigator.MoveToChild("bond", NAMESPACE))
                    {
                        do
                        {
                            string atomRefs = navigator.GetAttribute("atomRefs2", ATTRIBUTENS);
                            string[] parts = atomRefs.Split(' ');

                            if (parts.Length == 2)
                            {
                                int from = Utility.SafeInt(parts[0].Substring(1));
                                int to = Utility.SafeInt(parts[1].Substring(1));
                                int order = Utility.SafeInt(navigator.GetAttribute("order", ATTRIBUTENS));
                                int type = 0; // Where or is type stored? 

                                // TODO: How do we handle hydrogen count? 
                                mol.AddBond(from, to, order, type);
                            }

                        } while (navigator.MoveToNext("bond", NAMESPACE));

                        navigator.MoveToParent();
                    }

                    navigator.MoveToParent();
                }
            }

            return mol; 
        }
		
		public static void  WriteMDLMOL(System.IO.Stream ostr, Molecule mol)
		{
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			WriteMDLMOL(new System.IO.StreamWriter(new System.IO.StreamWriter(ostr, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(ostr, System.Text.Encoding.Default).Encoding), mol);
		}
		public static void  WriteMDLMOL(System.IO.StreamWriter output, Molecule mol)
		{
			//UPGRADE_ISSUE: Class 'java.text.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			//UPGRADE_ISSUE: Constructor 'java.text.DecimalFormat.DecimalFormat' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javatextDecimalFormat'"
			// DecimalFormat fmt = new DecimalFormat("0.0000");
            string fmtStr = "N4"; 
			
			output.Write("\nNuGenChem MOLfile\n\n");
			output.Write(intrpad(mol.NumAtoms(), 3) + intrpad(mol.NumBonds(), 3) + "  0  0  0  0  0  0  0  0999 V2000\n");
			
			System.String line;
			
			for (int n = 1; n <= mol.NumAtoms(); n++)
			{
                string str = mol.AtomX(n).ToString(fmtStr);
				line = rep(" ", 10 - str.Length) + str;
                str = mol.AtomY(n).ToString(fmtStr);
				line += (rep(" ", 10 - str.Length) + str);
				line += "    0.0000 ";
				str = mol.AtomElement(n);
				line += (str + rep(" ", 4 - str.Length) + "0");
				
				int chg = mol.AtomCharge(n), spin = mol.AtomUnpaired(n);
				if (chg >= - 3 && chg <= - 1)
					chg = 4 - chg;
				else if (chg == 0 && spin == 2)
					chg = 4;
				else if (chg < 1 || chg > 3)
					chg = 0;
				line += (intrpad(chg, 3) + "  0  0  0  0  0  0  0  0  0  0");
				
				output.Write(line + "\n");
			}
			
			for (int n = 1; n <= mol.NumBonds(); n++)
			{
				int type = mol.BondOrder(n);
				if (type < 1 || type > 3)
					type = 1;
				int stereo = mol.BondType(n);
				if (stereo == Molecule.BONDTYPE_NORMAL)
				{
				}
				else if (stereo == Molecule.BONDTYPE_INCLINED)
				{
					stereo = 1; type = 1;
				}
				else if (stereo == Molecule.BONDTYPE_DECLINED)
				{
					stereo = 6; type = 1;
				}
				else if (stereo == Molecule.BONDTYPE_UNKNOWN)
				{
					stereo = 4; type = 1;
				}
				else
					stereo = 0;
				
				output.Write(intrpad(mol.BondFrom(n), 3) + intrpad(mol.BondTo(n), 3) + intrpad(type, 3) + intrpad(stereo, 3) + "  0  0  0\n");
			}
			
			int count = 0;
			line = "";
			for (int n = 1; n <= mol.NumAtoms(); n++)
				if (mol.AtomCharge(n) != 0)
				{
					line += (intrpad(n, 4) + intrpad(mol.AtomCharge(n), 4));
					count++;
					if (count == 8)
					{
						output.Write("M  CHG" + intrpad(count, 3) + line + "\n");
						count = 0; line = "";
					}
				}
			if (count > 0)
				output.Write("M  CHG" + intrpad(count, 3) + line + "\n");
			
			count = 0;
			line = "";
			for (int n = 1; n <= mol.NumAtoms(); n++)
				if (mol.AtomUnpaired(n) != 0)
				{
					line += (intrpad(n, 4) + intrpad(mol.AtomUnpaired(n), 4));
					count++;
					if (count == 8)
					{
						output.Write("M  RAD" + intrpad(count, 3) + line + "\n");
						count = 0; line = "";
					}
				}
			if (count > 0)
				output.Write("M  RAD" + intrpad(count, 3) + line + "\n");
			
			output.Write("M  END\n");
			output.Flush();
		}
		
		public static void  WriteCMLXML(System.IO.Stream ostr, Molecule mol)
		{
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
            using (StreamWriter writer = new System.IO.StreamWriter(ostr, System.Text.Encoding.Default))
            {
                WriteCMLXML(writer, mol);
            }
		}
		public static void  WriteCMLXML(System.IO.StreamWriter output, Molecule mol)
		{
			output.Write("<cml>\n");
			output.Write("  <molecule xmlns=\"http://www.xml-cml.org/schema/cml2/core\">\n");
			
			output.Write("    <atomArray>\n");
			for (int n = 1; n <= mol.NumAtoms(); n++)
			{
				output.Write("      <atom id=\"a" + n + "\" elementType=\"" + mol.AtomElement(n) + "\"" + " x2=\"" + mol.AtomX(n) + "\" y2=\"" + mol.AtomY(n) + "\" hydrogenCount=\"" + mol.AtomHydrogens(n) + "\"/>\n");
			}
			output.Write("    </atomArray>\n");
			
			output.Write("    <bondArray>\n");
			for (int n = 1; n <= mol.NumBonds(); n++)
			{
				output.Write("      <bond id=\"b" + n + "\" atomRefs2=\"a" + mol.BondFrom(n) + " a" + mol.BondTo(n) + "\" order=\"" + mol.BondOrder(n) + "\"/>\n");
			}
			output.Write("    </bondArray>\n");
			
			output.Write("  </molecule>\n");
			output.Write("</cml>\n");
			output.Flush();
		}
		
		// examines the beginning of a file and decides whether it can be considered a database of structures which this class is capable
		// of reading...
		// (NB: currently this includes MDL SD-files, and nothing else)
		internal static bool ExamineIsDatabase(System.IO.FileStream istr)
		{
			bool isdb = FindNextPosition(istr, 0) >= 0;
			// istr.getChannel().position(0);
            istr.Seek(0, SeekOrigin.Begin); 

			return isdb;
		}
		
        //Returns -1 if the file does not end with $$$$\n or if it can not be read from MoleculeStream as MDLMOL
		internal static long FindNextPosition(System.IO.FileStream istr, long startpos)
		{
            //FileChannel fch = istr.getChannel();
            //fch.position(startpos);
            istr.Seek(startpos, SeekOrigin.Begin);

            long pos = startpos; 
            long size = istr.Length; // fch.Count
            long nextpos = -1;
			
			System.String rec = "";
			while (nextpos < size)
			{
				int inp = istr.ReadByte();
				pos++;
				if (inp < 0)
					break;
				char ch = (char) inp;
				if (ch == '\r')
					continue;
				rec = System.String.Concat(rec, System.Convert.ToString(ch));
				if (rec.EndsWith("$$$$\n"))
				{
					nextpos = pos; break;
				}
			}
			if (nextpos < 0)
				return - 1;
			
			try
			{
				//UPGRADE_ISSUE: Constructor 'java.io.StreamReader.StreamReader' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javaioBufferedReaderBufferedReader_javaioReader'"
                using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(rec)))
                {
                    using (StreamReader input = new StreamReader(stream))
                    {
                        Molecule mol = ReadMDLMOL(input);
                        if (mol == null)
                            nextpos = -1;
                    }
                }
			}
			catch (System.IO.IOException)
			{
				nextpos = - 1;
			}
			
			return nextpos;
		}
		
		internal static Molecule FetchFromPosition(System.IO.FileStream istr, long pos)
		{
			// istr.getChannel().position(pos);
            istr.Seek(pos, SeekOrigin.Begin); 

			//UPGRADE_TODO: The differences in the expected value  of parameters for constructor 'java.io.StreamReader.StreamReader'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
			//UPGRADE_WARNING: At least one expression was used more than once in the target code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1181'"
			//return ReadMDLMOL(new System.IO.StreamReader(istr, System.Text.Encoding.Default).BaseStream, new System.IO.StreamReader(istr, System.Text.Encoding.Default).CurrentEncoding);
            using (StreamReader rdr = new StreamReader(istr))
            {
                return ReadMDLMOL(rdr);
            }
		}
		
		// miscellaneous help
		
		internal static System.String intrpad(int Val, int Len)
		{
			System.String str = System.Convert.ToString(Val);
			str = rep(" ", Len - str.Length) + str;
			if (str.Length > Len)
				str = str.Substring(0, (Len) - (0));
			return str;
		}
		internal static System.String rep(System.String Ch, int Len)
		{
			if (Len <= 0)
				return "";
			System.String str = Ch;
			while (str.Length < Len)
				str = str + Ch;
			return str;
		}

    }
}