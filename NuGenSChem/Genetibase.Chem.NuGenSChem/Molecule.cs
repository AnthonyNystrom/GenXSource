using System;
using System.Collections.Generic;
namespace Genetibase.Chem.NuGenSChem
{
	
	/*
	Internal molecule datastructure, which captures the basic information about a molecule as envisaged by the traditional diagram
	notation. The core data is stored as a list of labelled nodes (atoms) and labelled edges (bonds). Besides very basic editing and
	convenient object manipulation, a few computed properties are available, some of which are cached.*/
	
	public class Molecule
    {
        #region Missing properties
        private int _pSize;

        public int PSize
        {
            get { return _pSize; }
            set { _pSize = value; }
        }

        private int _capacity;

        public int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }


        private int[] _path;

        public int[] Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private int _rblk;

        public int RBlk
        {
            get { return _rblk; }
            set { _rblk = value; }
        }

        private List<object> _rings;

        public List<object> Rings
        {
            get { return _rings; }
            set { _rings = value; }
        }
        #endregion 


        // ring hunter: recursive step; finds, compares and collects
        void RecursiveRingFind(int[] Path, int PSize, int Capacity, int RBlk, List<Object> Rings)
        {
            int last;

            // not enough atoms yet, so look for new possibilities
            if (PSize < Capacity)
            {
                last = Path[PSize - 1];
                for (int n = 0; n < graph[last - 1].Length; n++)
                {
                    int adj = graph[last - 1][n] + 1;
                    if (ringID[adj - 1] != RBlk)
                        continue;
                    bool found = false;
                    for (int i = 0; i < PSize; i++)
                        if (Path[i] == adj)
                        {
                            found = true; break;
                        }
                    if (!found)
                    {
                        int[] newPath = (int[])Path.Clone();
                        newPath[PSize] = adj;
                        RecursiveRingFind(newPath, PSize + 1, Capacity, RBlk, Rings);
                    }
                }
                return;
            }

            // path is full, so make sure it eats its tail
            last = Path[PSize - 1];
            bool fnd = false;
            for (int n = 0; n < graph[last - 1].Length; n++)
                if (graph[last - 1][n] + 1 == Path[0])
                {
                    fnd = true; break;
                }
            if (!fnd)
                return;

            // reorder the array then look for duplicates
            int first = 0;
            for (int n = 1; n < PSize; n++)
                if (Path[n] < Path[first])
                    first = n;
            int fm = (first - 1 + PSize) % PSize, fp = (first + 1) % PSize;
            bool flip = Path[fm] < Path[fp];
            if (first != 0 || flip)
            {
                int[] newPath = new int[PSize];
                for (int n = 0; n < PSize; n++)
                    newPath[n] = Path[(first + (flip ? PSize - n : n)) % PSize];
                Path = newPath;
            }

            for (int n = 0; n < Rings.Count; n++)
            {
                int[] look = (int[])Rings[n];
                bool same = true;
                for (int i = 0; i < PSize; i++)
                    if (look[i] != Path[i])
                    {
                        same = false; break;
                    }
                if (same)
                    return;
            }

            Rings.Add(Path);
        }

  

		internal class Atom
		{
			public Atom(Molecule enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Molecule enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Molecule enclosingInstance;
			public Molecule Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal System.String Element;
			internal double X, Y;
			internal int Charge, Unpaired;
			internal int HExplicit;
		}
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Bond' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		internal class Bond
		{
			public Bond(Molecule enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Molecule enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Molecule enclosingInstance;
			public Molecule Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal int From, To;
			internal int Order, Type;
		}
		
		public const int HEXPLICIT_UNKNOWN = - 1;
		
		public const int BONDTYPE_NORMAL = 0;
		public const int BONDTYPE_INCLINED = 1;
		public const int BONDTYPE_DECLINED = 2;
		public const int BONDTYPE_UNKNOWN = 3;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'ELEMENTS'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly System.String[] ELEMENTS = new System.String[]{null, "H", "He", "Li", "Be", "B", "C", "N", "O", "F", "Ne", "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar", "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr", "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe", "Cs", "Ba", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu", "Hf", "Ta", "W", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn", "Fr", "Ra", "Ac", "Th", "Pa", "U", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr"};
		
		internal bool invalMinMax = false;
		internal double minX = 0, minY = 0, maxX = 0, maxY = 0;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<Atom> atoms = new List<Atom>();
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<Bond> bonds = new List<Bond>();
		
		// computed graph topology properties
		internal int[][] graph = null;
		internal int[] ringID = null, compID = null, priority = null;
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		List<Object> ring5 = null, ring6 = null, ring7 = null; // common ring sizes
		
		// computed stereochemistry properties
		public const int STEREO_NONE = 0; // topology does not allow any stereoisomers
		public const int STEREO_POS = 1; // R or Z, depending on type
		public const int STEREO_NEG = 2; // S or E, depending on type
		public const int STEREO_UNKNOWN = 3; // topology allows stereoisomers, but wedges/geometry do not resolve which
		internal int[] chiral = null; // (per atom)
		internal int[] cistrans = null; // (per bond)
		
		// data for calculation of implicit hydrogens
		//UPGRADE_NOTE: Final was removed from the declaration of 'HYVALENCE_EL'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly System.String[] HYVALENCE_EL = new System.String[]{"C", "N", "O", "S", "P"};
		//UPGRADE_NOTE: Final was removed from the declaration of 'HYVALENCE_VAL'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		internal static readonly int[] HYVALENCE_VAL = new int[]{4, 3, 2, 2, 3};
		
		// ------------------ public functions --------------------
		
		public Molecule()
		{
            
		}
		
		// returns an equivalent replica of the molecule
		public virtual Molecule Clone()
		{
			Molecule mol = new Molecule();
			for (int n = 1; n <= NumAtoms(); n++)
			{
				Atom a = atoms[n - 1];
				int num = mol.AddAtom(a.Element, a.X, a.Y, a.Charge, a.Unpaired);
				mol.SetAtomHExplicit(num, AtomHExplicit(n));
			}
			for (int n = 1; n <= NumBonds(); n++)
			{
				Bond b = bonds[n - 1];
				mol.AddBond(b.From, b.To, b.Order, b.Type);
			}
			return mol;
		}
		
		// ----- fetching information directly from the datastructures ----
		
		public virtual int NumAtoms()
		{
			return atoms.Count;
		}
		public virtual int NumBonds()
		{
			return bonds.Count;
		}
		
		public virtual System.String AtomElement(int N)
		{
			return atoms[N - 1].Element;
		}
		public virtual double AtomX(int N)
		{
			return atoms[N - 1].X;
		}
		public virtual double AtomY(int N)
		{
			return atoms[N - 1].Y;
		}
		public virtual int AtomCharge(int N)
		{
			return atoms[N - 1].Charge;
		}
		public virtual int AtomUnpaired(int N)
		{
			return atoms[N - 1].Unpaired;
		}
		public virtual int AtomHExplicit(int N)
		{
			return atoms[N - 1].HExplicit;
		}
		
		public virtual int BondFrom(int N)
		{
			return bonds[N - 1].From;
		}
		public virtual int BondTo(int N)
		{
			return bonds[N - 1].To;
		}
		public virtual int BondOrder(int N)
		{
			return bonds[N - 1].Order;
		}
		public virtual int BondType(int N)
		{
			return bonds[N - 1].Type;
		}
		
		// ----- modifying information in the datastructures ----
		
		// adds an atom to the end of the molecule; it is fairly reasonable to assume that the new one will be at position==NumAtoms(),
		// and that all other atoms will be in the same places, but the position is returned for convenience
		public virtual int AddAtom(System.String Element, double X, double Y)
		{
			return AddAtom(Element, X, Y, 0, 0);
		}
		public virtual int AddAtom(System.String Element, double X, double Y, int Charge, int Unpaired)
		{
			if (X < minX || NumAtoms() == 0)
				minX = X;
			if (X > maxX || NumAtoms() == 0)
				maxX = X;
			if (Y < minY || NumAtoms() == 0)
				minY = Y;
			if (Y > maxY || NumAtoms() == 0)
				maxY = Y;
			
			Atom a = new Atom(this);
			a.Element = Element;
			a.X = X;
			a.Y = Y;
			a.Charge = Charge;
			a.Unpaired = Unpaired;
			a.HExplicit = HEXPLICIT_UNKNOWN;
			atoms.Add(a);
			TrashGraph();
			return atoms.Count;
		}
		public virtual void  SetAtomElement(int N, System.String Element)
		{
			atoms[N - 1].Element = Element; TrashPriority();
		}
		public virtual void  SetAtomPos(int N, double X, double Y)
		{
			atoms[N - 1].X = X;
			atoms[N - 1].Y = Y;
			invalMinMax = true;
			TrashStereo();
		}
		public virtual void  SetAtomCharge(int N, int Charge)
		{
			atoms[N - 1].Charge = Charge; TrashPriority();
		}
		public virtual void  SetAtomUnpaired(int N, int Unpaired)
		{
			atoms[N - 1].Unpaired = Unpaired; TrashPriority();
		}
		public virtual void  SetAtomHExplicit(int N, int HExplicit)
		{
			atoms[N - 1].HExplicit = HExplicit; TrashPriority();
		}
		
		// adds an atom the end of the molecule; the position of the new bond is returned for convenience
		public virtual int AddBond(int From, int To, int Order)
		{
			return AddBond(From, To, Order, BONDTYPE_NORMAL);
		}
		public virtual int AddBond(int From, int To, int Order, int Type)
		{
			Bond b = new Bond(this);
			b.From = From;
			b.To = To;
			b.Order = Order;
			b.Type = Type;
			bonds.Add(b);
			TrashGraph();
			return bonds.Count;
		}
		
		public virtual void  SetBondFromTo(int N, int From, int To)
		{
			bonds[N - 1].From = From;
			bonds[N - 1].To = To;
			TrashGraph();
		}
		public virtual void  SetBondOrder(int N, int Order)
		{
			bonds[N - 1].Order = Order; TrashPriority();
		}
		public virtual void  SetBondType(int N, int Type)
		{
			bonds[N - 1].Type = Type; TrashStereo();
		}
		
		// deletes the indicated atom, and also any bonds which were connected to it
		public virtual void  DeleteAtomAndBonds(int N)
		{
			for (int n = 0; n < NumBonds(); )
			{
				Bond b = bonds[n];
				if (b.From == N || b.To == N)
					bonds.RemoveAt(n);
				else
				{
					if (b.From > N)
						b.From--;
					if (b.To > N)
						b.To--;
					n++;
				}
			}
			atoms.RemoveAt(N - 1);
			invalMinMax = true;
			TrashGraph();
		}
		public virtual void  DeleteBond(int N)
		{
			bonds.RemoveAt(N - 1);
			TrashGraph();
		}
		
		// ----- fetching information which is computed from the underlying data, usually cached -----
		
		public virtual double MinX()
		{
			if (invalMinMax)
				DetermineMinMax();
			return minX;
		}
		public virtual double MaxX()
		{
			if (invalMinMax)
				DetermineMinMax();
			return maxX;
		}
		public virtual double MinY()
		{
			if (invalMinMax)
				DetermineMinMax();
			return minY;
		}
		public virtual double MaxY()
		{
			if (invalMinMax)
				DetermineMinMax();
			return maxY;
		}
		public virtual double RangeX()
		{
			return maxX - minX;
		}
		public virtual double RangeY()
		{
			return maxY - minY;
		}
		
		// return the index of the bond, if any, which connects the two indicated atoms
		public virtual int FindBond(int A1, int A2)
		{
			for (int n = 1; n <= NumBonds(); n++)
			{
				if (BondFrom(n) == A1 && BondTo(n) == A2)
					return n;
				if (BondTo(n) == A1 && BondFrom(n) == A2)
					return n;
			}
			return 0;
		}
		
		// for a bond, returns the end which is not==Ref; return value will be From,To or 0    
		public virtual int BondOther(int N, int Ref)
		{
			if (BondFrom(N) == Ref)
				return BondTo(N);
			if (BondTo(N) == Ref)
				return BondFrom(N);
			return 0;
		}
		
		// returns whether an atom logically ought to be drawn "explicitly";if false, it is a carbon which should be just a dot
		public virtual bool AtomExplicit(int N)
		{
			if (atoms[N - 1].Element.CompareTo("C") != 0 || atoms[N - 1].Charge != 0 || atoms[N - 1].Unpaired != 0)
				return true;
			for (int n = 0; n < bonds.Count; n++)
				if (bonds[n].From == N || bonds[n].To == N)
					return false;
			return true;
		}
		
		// uses either explicit or computed number to determine how many hydrogens the atom has; the field for explicit hydrogens takes
		// absolute preference, if it has its default value of 'unknown', the number is computed by looking up the hydrogen capacity for
		// the element (most of which are zero), subtracting the total of bond orders, then returning the difference, or zero; the calculation
		// tends to err on the side of too few, since the concept is just an aid to drawing organic structures, not a cheminformatic attempt
		// to compensate for 2 1/2 decades of bad file formats
		// (note: returns "implicit"+"explicit", but does NOT count "actual" hydrogens, i.e. those which have their own atom nodes)
		public virtual int AtomHydrogens(int N)
		{
			int hy = AtomHExplicit(N);
			if (hy != HEXPLICIT_UNKNOWN)
				return hy;
			
			for (int n = 0; n < HYVALENCE_EL.Length; n++)
				if (String.CompareOrdinal(HYVALENCE_EL[n], AtomElement(N)) == 0)
				{
					hy = HYVALENCE_VAL[n]; break;
				}
			if (hy == HEXPLICIT_UNKNOWN)
				return 0;
			int ch = AtomCharge(N);
			if (String.CompareOrdinal(AtomElement(N), "C") == 0)
				ch = - System.Math.Abs(ch);
			hy += ch - AtomUnpaired(N);
			for (int n = 1; n <= NumBonds(); n++)
				if (BondFrom(n) == N || BondTo(n) == N)
					hy -= BondOrder(n);
			return hy < 0?0:hy;
		}
		
		// returns the numerical ID of the ring block in which the atom resides, or 0 if it is not in a ring   
		public virtual int AtomRingBlock(int N)
		{
			if (graph == null)
				BuildGraph();
			if (ringID == null)
				BuildRingID();
			return ringID[N - 1];
		}
		// returns whether or not a bond resides in a ring (i.e. ring block of each end is the same and nonzero)
		public virtual bool BondInRing(int N)
		{
			int r1 = AtomRingBlock(BondFrom(N)), r2 = AtomRingBlock(BondTo(N));
			return r1 > 0 && r1 == r2;
		}
		
		// returns the connected component ID of the atom, always 1 or more
		public virtual int AtomConnComp(int N)
		{
			if (graph == null)
				BuildGraph();
			if (compID == null)
				BuildConnComp();
			return compID[N - 1];
		}
		
		// returns the number of neighbours of an atom
		public virtual int AtomAdjCount(int N)
		{
			if (graph == null)
				BuildGraph();
			return graph[N - 1].Length;
		}
		
		// returns the actual adjacency list of an atom; unfortunately this has to make a clone of the array, since the numbers are converted
		// to 1-based indices, and we would not want callers modifying it anyway
		public virtual int[] AtomAdjList(int N)
		{
			if (graph == null)
				BuildGraph();

            if (graph.Length > N - 1)
            {
                int[] adj = (int[])graph[N - 1].Clone();
                for (int n = 0; n < adj.Length; n++)
                    adj[n]++;
                return adj;
            }

            return null; 
		}
		
		// returns atom-based priority according to the Cahn-Ingold-Prelog rules
		public virtual int AtomPriority(int N)
		{
			if (graph == null)
				BuildGraph();
			if (compID == null)
				BuildConnComp();
			if (priority == null)
				BuildPriority();
			return priority[N - 1];
		}
		
		// return the tetrahedral chirality of the given atom
		public virtual int AtomChirality(int N)
		{
			if (graph == null)
				BuildGraph();
			if (compID == null)
				BuildConnComp();
			if (priority == null)
				BuildPriority();
			if (chiral == null)
				BuildChirality();
			return chiral[N - 1];
		}
		
		// return the cis/trans style stereochemistry of the given bond
		public virtual int BondStereo(int N)
		{
			if (graph == null)
				BuildGraph();
			if (compID == null)
				BuildConnComp();
			if (priority == null)
				BuildPriority();
			if (cistrans == null)
				BuildCisTrans();
			return cistrans[N - 1];
		}
		
		// returns _all_ rings of indicated size; each item in the array list is an array of int[Size], a consecutively ordered array of atom 
		// numbers; uses a recursive depth first search, which must be bounded above by Size being small in order to avoid exponential blowup
		public virtual int[][] FindRingSize(int Size)
		{
            List<object> rings = null; 

			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			if (Size == 5 && ring5 != null)
				rings = ring5;
			if (Size == 6 && ring6 != null)
				rings = ring6;
			if (Size == 7 && ring7 != null)
				rings = ring7;
			
			if (rings == null)
			{
				if (graph == null)
					BuildGraph();
				if (ringID == null)
					BuildRingID();
				
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				rings = new List<Object>();
				for (int n = 1; n <= NumAtoms(); n++)
					if (ringID[n - 1] > 0)
					{
						int[] path = new int[Size];
						path[0] = n;
						RecursiveRingFind(path, 1, Size, ringID[n - 1], rings);
					}
				
				if (Size == 5)
					ring5 = rings;
				if (Size == 6)
					ring6 = rings;
				if (Size == 7)
					ring7 = rings;
			}
			
            //int[][] ret = new int[rings.Count][];
            //for (int n = 0; n < rings.Count; n++)
            //{
            //    int count = ((int[]) rings[n]).Length; 
            //    //ret[n] = new int[][count];
            //    ret[n] = new int[count][];
            //    ((int[]) rings[n]).CopyTo((int[]) ret[n], 0);
            //}
            //return ret;
            int[][] ret=new int[rings.Count][];
		    for (int n=0;n<rings.Count;n++) 
                ret[n]=(int[])((int[])rings[n]).Clone();

		    return ret;
		}
		
		// compare to another molecule instance; null is equivalent to empty; return values:
		//     -1 if Mol < Other
		//	    0 if Mol == Other
		//	    1 if Mol > Other 
		//  can be used to sort molecules, albeit crudely; does not do any kind of canonical preprocessing
		public virtual int CompareTo(Molecule Other)
		{
			if (Other == null)
				return NumAtoms() == 0?0:1; // null is equivalent to empty
			if (NumAtoms() < Other.NumAtoms())
				return - 1;
			if (NumAtoms() > Other.NumAtoms())
				return 1;
			if (NumBonds() < Other.NumBonds())
				return - 1;
			if (NumBonds() > Other.NumBonds())
				return 1;
			
			for (int n = 1; n <= NumAtoms(); n++)
			{
				int c = String.CompareOrdinal(AtomElement(n), Other.AtomElement(n));
				if (c != 0)
					return c;
				if (AtomX(n) < Other.AtomX(n))
					return - 1;
				if (AtomX(n) > Other.AtomX(n))
					return 1;
				if (AtomY(n) < Other.AtomY(n))
					return - 1;
				if (AtomY(n) > Other.AtomY(n))
					return 1;
				if (AtomCharge(n) < Other.AtomCharge(n))
					return - 1;
				if (AtomCharge(n) > Other.AtomCharge(n))
					return 1;
				if (AtomUnpaired(n) < Other.AtomUnpaired(n))
					return - 1;
				if (AtomUnpaired(n) > Other.AtomUnpaired(n))
					return 1;
				if (AtomHExplicit(n) < Other.AtomHExplicit(n))
					return - 1;
				if (AtomHExplicit(n) > Other.AtomHExplicit(n))
					return 1;
			}
			for (int n = 1; n <= NumBonds(); n++)
			{
				if (BondFrom(n) < Other.BondFrom(n))
					return - 1;
				if (BondFrom(n) > Other.BondFrom(n))
					return 1;
				if (BondTo(n) < Other.BondTo(n))
					return - 1;
				if (BondTo(n) > Other.BondTo(n))
					return 1;
				if (BondOrder(n) < Other.BondOrder(n))
					return - 1;
				if (BondOrder(n) > Other.BondOrder(n))
					return 1;
				if (BondType(n) < Other.BondType(n))
					return - 1;
				if (BondType(n) > Other.BondType(n))
					return 1;
			}
			
			return 0;
		}
		
		// lookup the atomic number for the element, or return 0 if not in the periodic table
		internal virtual int AtomicNumber(int N)
		{
			return AtomicNumber(AtomElement(N));
		}
		internal virtual int AtomicNumber(System.String El)
		{
			for (int n = 1; n < ELEMENTS.Length; n++)
				if (String.CompareOrdinal(ELEMENTS[n], El) == 0)
					return n;
			return 0;
		}
		
		// convenient debugging mechanism
		public virtual void  Dump()
		{
			System.Console.Out.WriteLine("#Atoms=" + NumAtoms() + " #Bonds=" + NumBonds());
			for (int n = 0; n < NumAtoms(); n++)
			{
				Atom a = atoms[n];
				System.Console.Out.WriteLine(" A" + (n + 1) + ": " + a.Element + "=" + a.X + "," + a.Y + ";" + a.Charge + "," + a.Unpaired);
			}
			for (int n = 0; n < NumBonds(); n++)
			{
				Bond b = bonds[n];
				System.Console.Out.WriteLine(" B" + (n + 1) + ": " + b.From + "-" + b.To + "=" + b.Order + "," + b.Type);
			}
		}
		
		// ------------------ private functions --------------------
		
		// for when the connectivity changes somehow; abandon graph properties
		internal virtual void  TrashGraph()
		{
			graph = null;
			ringID = null;
			ring5 = null;
			ring6 = null;
			ring7 = null;
			compID = null;
			TrashPriority();
		}
		internal virtual void  TrashPriority()
		{
			priority = null;
			TrashStereo();
		}
		internal virtual void  TrashStereo()
		{
			chiral = null;
			cistrans = null;
		}
		
		// update the cache of atom range
		internal virtual void  DetermineMinMax()
		{
			invalMinMax = false;
			if (NumAtoms() == 0)
			{
				minX = maxX = minY = maxY = 0;
				return ;
			}
			minX = maxX = AtomX(1);
			minY = maxY = AtomY(1);
			for (int n = 2; n <= NumAtoms(); n++)
			{
				double x = AtomX(n), y = AtomY(n);
				minX = System.Math.Min(minX, x);
				maxX = System.Math.Max(maxX, x);
				minY = System.Math.Min(minY, y);
				maxY = System.Math.Max(maxY, y);
			}
		}
		
		// update the cache of the molecular graph, in adjacency format, rather than the edge list format; the former is much better for
		// graph algorithms, the latter much more suited to capturing sketcher-ish information content; note that the graph indices are
		// zero-based
		internal virtual void  BuildGraph()
		{
			graph = new int[NumAtoms()][];
			for (int n = 1; n <= NumBonds(); n++)
			{
				int bf = BondFrom(n) - 1, bt = BondTo(n) - 1;
                if (bf == bt || graph.Length <= bt)
					continue; // ouch!
				int lf = graph[bf] == null?0:graph[bf].Length, lt = graph[bt] == null?0:graph[bt].Length;
				int[] bl = new int[lf + 1];
				for (int i = 0; i < lf; i++)
					bl[i] = graph[bf][i];
				bl[lf] = bt;
				graph[bf] = bl;
				bl = new int[lt + 1];
				for (int i = 0; i < lt; i++)
					bl[i] = graph[bt][i];
				bl[lt] = bf;
				graph[bt] = bl;
			}
			for (int n = 0; n < NumAtoms(); n++)
				if (graph[n] == null)
					graph[n] = new int[0];
			
			/*for (int n=0;n<NumAtoms();n++) 
			{
			System.out.print("#"+n+":");
			for (int i=0;i<graph[n].length;i++) System.out.print(" "+graph[n][i]);
			System.out.println();
			}*/
		}
		
		// passes over the graph establishing which component each atom belongs to
		internal virtual void  BuildConnComp()
		{
			compID = new int[NumAtoms()];
			for (int n = 0; n < NumAtoms(); n++)
				compID[n] = 0;
			int comp = 1;
			compID[0] = comp;
			
			// (not very efficient, should use a stack-based depth first search)
			while (true)
			{
				bool anything = false;
				for (int n = 0; n < NumAtoms(); n++)
					if (compID[n] > 0)
					{
						for (int i = 0; i < graph[n].Length; i++)
							if (compID[graph[n][i]] == 0)
							{
								compID[graph[n][i]] = comp;
								anything = true;
							}
					}
				
				if (!anything)
				{
					for (int n = 0; n < NumAtoms(); n++)
						if (compID[n] == 0)
						{
							compID[n] = ++comp; anything = true; break;
						}
					if (!anything)
						break;
				}
			}
		}
		
		// generates Cahn-Ingold-Prelog priority for each atom, where degeneracies are indicated by having the same number
		internal virtual void  BuildPriority()
		{
			// build a graph representation which has entries replicated according to bond order
			int[][] cipgr = new int[NumAtoms()][];
			for (int n = 0; n < NumAtoms(); n++)
				if (cipgr[n] == null)
				{
					cipgr[n] = new int[AtomHydrogens(n + 1)];
					for (int i = 0; i < cipgr[n].Length; i++)
						cipgr[n][i] = - 1;
				}
			for (int n = 1; n <= NumBonds(); n++)
			{
				int bf = BondFrom(n) - 1, bt = BondTo(n) - 1, bo = BondOrder(n);
				if (bf == bt || bo == 0)
					continue;
				int lf = cipgr[bf].Length, lt = cipgr[bt].Length;
				int[] bl = new int[lf + bo];
				for (int i = 0; i < lf; i++)
					bl[i] = cipgr[bf][i];
				for (int i = 0; i < bo; i++)
					bl[lf++] = bt;
				cipgr[bf] = bl;
				bl = new int[lt + bo];
				for (int i = 0; i < lt; i++)
					bl[i] = cipgr[bt][i];
				for (int i = 0; i < bo; i++)
					bl[lt++] = bf;
				cipgr[bt] = bl;
			}
			
			// seed the priorities with atomic number
			priority = new int[NumAtoms()];
			bool anyActualH = false;
			for (int n = 0; n < NumAtoms(); n++)
			{
				priority[n] = AtomicNumber(n + 1);
				if (priority[n] == 1)
					anyActualH = true;
			}
			
			// pass through and reassign priorities as many times as necessary, until no change
			int[][] prigr = new int[NumAtoms()][];
			while (true)
			{
				// make an equivalent to cipgr which has priorities instead of indices
				for (int n = 0; n < NumAtoms(); n++)
				{
					prigr[n] = new int[cipgr[n].Length];
					for (int i = 0; i < prigr[n].Length; i++)
						if (cipgr[n][i] < 0)
							prigr[n][i] = 1;
						else
							prigr[n][i] = priority[cipgr[n][i]];
					int p = 0;
					while (p < prigr[n].Length - 1)
					{
						if (prigr[n][p] < prigr[n][p + 1])
						{
							int i = prigr[n][p]; prigr[n][p] = prigr[n][p + 1]; prigr[n][p + 1] = i;
							if (p > 0)
								p--;
						}
						else
							p++;
					}
				}
				
				// divide each priority category into groups, then for each of these groups, split the contents out and reassign
				int[][] groups = SortAndGroup(priority);
				int nextpri = anyActualH?0:1;
				bool repartitioned = false;
				
				for (int n = 0; n < groups.Length; n++)
				{
					// sort the groups according to their cipgr contents
					int p = 0;
					while (p < groups[n].Length - 1)
					{
						int i1 = groups[n][p], i2 = groups[n][p + 1];
						int cmp = 0, sz = System.Math.Max(prigr[i1].Length, prigr[i2].Length);
						//System.out.println("n="+n+" p="+p+" i1="+i1+" i2="+i2+" cmp="+cmp);
						for (int i = 0; i < sz; i++)
						{
							int v1 = i < prigr[i1].Length?prigr[i1][i]:0, v2 = i < prigr[i2].Length?prigr[i2][i]:0;
							if (v1 < v2)
							{
								cmp = - 1; break;
							}
							if (v1 > v2)
							{
								cmp = 1; break;
							}
						}
						//System.out.println(" cmp="+cmp);
						if (cmp > 0)
						{
							groups[n][p] = i2; groups[n][p + 1] = i1;
							if (p > 0)
								p--;
						}
						else
							p++;
					}
					
					/*System.out.println("g="+n);
					for (int i=0;i<groups[n].length;i++) 
					{
					System.out.print("   i="+groups[n][i]+"/"+AtomElement(groups[n][i]+1)+" ");
					for (int j=0;j<prigr[groups[n][i]].length;j++)
					{
					System.out.print(prigr[groups[n][i]][j]+" ");
					}
					System.out.println();
					}*/
					
					//System.out.println("group="+n+" len="+groups[n].length);
					for (int i = 0; i < groups[n].Length; i++)
					{
						if (i == 0)
							nextpri++;
						else if (prigr[groups[n][i]].Length != prigr[groups[n][i - 1]].Length)
						{
							nextpri++; repartitioned = true;
						}
						else
						{
							for (int j = 0; j < prigr[groups[n][i]].Length; j++)
								if (prigr[groups[n][i]][j] != prigr[groups[n][i - 1]][j])
								{
									nextpri++; repartitioned = true; break;
								}
						}
						
						priority[groups[n][i]] = nextpri;
					}
				}
				// ...
				
				
				/*for (int n=0;n<groups.length;n++)
				{
				System.out.print("n="+n+": ");
				for (int i=0;i<groups[n].length;i++) System.out.print("("+groups[n][i]+AtomElement(groups[n][i]+1)+") ");
				System.out.println();
				}*/
				
				if (!repartitioned)
					break;
			}
		}
		
		// compute the chirality values for each atom centre
		internal virtual void  BuildChirality()
		{
			chiral = new int[NumAtoms()];
			
			bool[] haswedge = new bool[NumAtoms()];
			for (int n = 0; n < NumAtoms(); n++)
				haswedge[n] = false;
			for (int n = 1; n <= NumBonds(); n++)
				if (BondType(n) == BONDTYPE_INCLINED || BondType(n) == BONDTYPE_DECLINED)
					haswedge[BondFrom(n) - 1] = true;
			
			int[] pri = new int[4];
			double[] x = new double[4], y = new double[4], z = new double[4];
			
			for (int n = 0; n < NumAtoms(); n++)
			{
				chiral[n] = STEREO_NONE;
				if (!(graph[n].Length == 4 || (graph[n].Length == 3 && AtomHydrogens(n + 1) == 1)))
					continue;
				
				for (int i = 0; i < graph[n].Length; i++)
				{
					pri[i] = priority[graph[n][i]];
					x[i] = AtomX(graph[n][i] + 1) - AtomX(n + 1);
					y[i] = AtomY(graph[n][i] + 1) - AtomY(n + 1);
					z[i] = 0;
					if (haswedge[n])
						for (int j = 1; j <= NumBonds(); j++)
							if (BondFrom(j) == n + 1 && BondTo(j) == graph[n][i] + 1)
							{
								if (BondType(j) == BONDTYPE_INCLINED)
									z[i] = 1;
								if (BondType(j) == BONDTYPE_DECLINED)
									z[i] = - 1;
								break;
							}
				}
				if (graph[n].Length == 3)
				{
					pri[3] = 0; x[3] = 0; y[3] = 0; z[3] = 0;
				}
				
				int p = 0;
				while (p < 3)
				{
					if (pri[p] > pri[p + 1])
					{
						int i; double d;
						i = pri[p]; pri[p] = pri[p + 1]; pri[p + 1] = i;
						d = x[p]; x[p] = x[p + 1]; x[p + 1] = d;
						d = y[p]; y[p] = y[p + 1]; y[p + 1] = d;
						d = z[p]; z[p] = z[p + 1]; z[p + 1] = d;
						if (p > 0)
							p--;
					}
					else
						p++;
				}
				if ((pri[0] == 0 && pri[1] == 1) || pri[0] == pri[1] || pri[1] == pri[2] || pri[2] == pri[3])
					continue; // no topological chirality
				
				chiral[n] = STEREO_UNKNOWN;
				if (z[0] == 0 && z[1] == 0 && z[2] == 0 && z[3] == 0)
					continue; // all atoms are in-plane
				
				bool sane = true;
				for (int i = 0; i < 4; i++)
					if (pri[0] != 0)
					{
						double r = x[i] * x[i] + y[i] * y[i] + z[i] * z[i];
						if (r < 0.01 * 0.01)
						{
							sane = false; break;
						}
						r = 1 / System.Math.Sqrt(r);
						x[i] = x[i] * r; y[i] = y[i] * r; z[i] = z[i] * r;
					}
				if (!sane)
					continue;
				
				if (pri[0] == 0)
				// build a position for the implicit H
				{
					x[0] = - (x[1] + x[2] + x[3]);
					y[0] = - (y[1] + y[2] + y[3]);
					z[0] = - (z[1] + z[2] + z[3]);
					double r = x[0] * x[0] + y[0] * y[0] + z[0] * z[0];
					if (r < 0.01 * 0.01)
					{
						sane = false; break;
					}
					r = 1 / System.Math.Sqrt(r);
					x[0] = x[0] * r; y[0] = y[0] * r; z[0] = z[0] * r;
				}
				if (!sane)
					continue;
				
				double R = 0, S = 0;
				for (int i = 1; i <= 6; i++)
				{
					int a = 0, b = 0;
					if (i == 1)
					{
						a = 1; b = 2;
					}
					else if (i == 2)
					{
						a = 2; b = 3;
					}
					else if (i == 3)
					{
						a = 3; b = 1;
					}
					else if (i == 4)
					{
						a = 2; b = 1;
					}
					else if (i == 5)
					{
						a = 3; b = 2;
					}
					else if (i == 6)
					{
						a = 1; b = 3;
					}
					double xx = y[a] * z[b] - y[b] * z[a] - x[0];
					double yy = z[a] * x[b] - z[b] * x[a] - y[0];
					double zz = x[a] * y[b] - x[b] * y[a] - z[0];
					if (i <= 3)
						R += xx * xx + yy * yy + zz * zz;
					else
						S += xx * xx + yy * yy + zz * zz;
				}
				chiral[n] = R > S?STEREO_POS:STEREO_NEG;
			}
		}
		
		// computer the cis/trans stereochemistry for each bond
		internal virtual void  BuildCisTrans()
		{
			cistrans = new int[NumBonds()];
			int[] sf = new int[2], st = new int[2];
			
			for (int n = 0; n < NumBonds(); n++)
			{
				cistrans[n] = STEREO_NONE;
				int bf = BondFrom(n + 1) - 1, bt = BondTo(n + 1) - 1;
				if (BondOrder(n + 1) != 2 || graph[bf].Length <= 1 || graph[bt].Length <= 1 || graph[bf].Length > 3 || graph[bt].Length > 3)
					continue;
				
				int nf = 0, nt = 0;
				for (int i = 0; i < graph[bf].Length; i++)
					if (graph[bf][i] != bt)
						sf[nf++] = graph[bf][i];
				for (int i = 0; i < graph[bt].Length; i++)
					if (graph[bt][i] != bf)
						st[nt++] = graph[bt][i];
				
				if (nf == 1)
				{
					if (AtomHydrogens(bf + 1) != 1 || priority[sf[0]] == 1)
						continue;
				}
				else
				{
					if (priority[sf[0]] == priority[sf[1]])
						continue;
					if (priority[sf[0]] < priority[sf[1]])
					{
						int i = sf[0]; sf[0] = sf[1]; sf[1] = i;
					}
				}
				
				if (nt == 1)
				{
					if (AtomHydrogens(bt + 1) != 1 || priority[st[0]] == 1)
						continue;
				}
				else
				{
					if (priority[st[0]] == priority[st[1]])
						continue;
					if (priority[st[0]] < priority[st[1]])
					{
						int i = st[0]; st[0] = st[1]; st[1] = i;
					}
				}
				
				cistrans[n] = STEREO_UNKNOWN;
				
				double xa = AtomX(bf + 1), ya = AtomY(bf + 1), xb = AtomX(bt + 1), yb = AtomY(bt + 1);
				double tha0 = System.Math.Atan2(yb - ya, xb - xa), thb0 = System.Math.PI + tha0;
				double tha1 = System.Math.Atan2(AtomY(sf[0] + 1) - ya, AtomX(sf[0] + 1) - xa);
				double tha2 = nf == 2?System.Math.Atan2(AtomY(sf[1] + 1) - ya, AtomX(sf[1] + 1) - xa):ThetaObtuse(tha0, tha1);
				double thb1 = System.Math.Atan2(AtomY(st[0] + 1) - yb, AtomX(st[0] + 1) - xb);
				double thb2 = nt == 2?System.Math.Atan2(AtomY(st[1] + 1) - yb, AtomX(st[1] + 1) - xb):ThetaObtuse(thb0, thb1);
				tha1 -= tha0; tha2 -= tha0; thb1 -= thb0; thb2 -= thb0;
				tha1 += (tha1 < - System.Math.PI?2 * System.Math.PI:0) + (tha1 > System.Math.PI?(- 2) * System.Math.PI:0);
				tha2 += (tha2 < - System.Math.PI?2 * System.Math.PI:0) + (tha2 > System.Math.PI?(- 2) * System.Math.PI:0);
				thb1 += (thb1 < - System.Math.PI?2 * System.Math.PI:0) + (thb1 > System.Math.PI?(- 2) * System.Math.PI:0);
				thb2 += (thb2 < - System.Math.PI?2 * System.Math.PI:0) + (thb2 > System.Math.PI?(- 2) * System.Math.PI:0);
				
				//UPGRADE_NOTE: Final was removed from the declaration of 'SMALL '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
				double SMALL = 5 * System.Math.PI / 180; // basically colinear
				if (System.Math.Abs(tha1) < SMALL || System.Math.Abs(tha2) < SMALL || System.Math.Abs(thb1) < SMALL || System.Math.Abs(thb2) < SMALL)
					continue;
				if (System.Math.Abs(tha1) > System.Math.PI - SMALL || System.Math.Abs(tha2) > System.Math.PI - SMALL)
					continue;
				if (System.Math.Abs(thb1) > System.Math.PI - SMALL || System.Math.Abs(thb2) > System.Math.PI - SMALL)
					continue;
				tha1 = Math.Sign(tha1); tha2 = Math.Sign(tha2); thb1 = Math.Sign(thb1); thb2 = Math.Sign(thb2);
				if (tha1 == tha2 || thb1 == thb2)
					continue;
				if (tha1 * thb1 < 0)
					cistrans[n] = STEREO_POS;
				if (tha1 * thb1 > 0)
					cistrans[n] = STEREO_NEG;
			}
		}
		
		
		// generally useful function which takes a list of numbers and sorts them, then bins the unique values into sub-arrays
		// (note: inefficient implementation, but could be improved easily enough)
		public static int[][] SortAndGroup(int[] Val)
		{
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			List<int> unique = new List<int>();
			for (int n = 0; n < Val.Length; n++)
				unique.Add(Val[n]);
			// Collections.sort(unique);
            unique.Sort(); 
			int p = 0;
			while (p < unique.Count - 1)
			{
				if (unique[p] == unique[p + 1])
					unique.RemoveAt(p);
				else
					p++;
			}
			
			int[][] ret = new int[unique.Count][];
			
			for (int n = 0; n < Val.Length; n++)
			{
				int grp = unique.IndexOf(Val[n]);
				int[] cnt = new int[ret[grp] == null?1:ret[grp].Length + 1];
				for (int i = 0; i < cnt.Length - 1; i++)
					cnt[i] = ret[grp][i];
				cnt[cnt.Length - 1] = n;
				ret[grp] = cnt;
			}
			
			return ret;
		}
		
		// update the ring-block-identifier for each atom
		internal virtual void  BuildRingID()
		{
			ringID = new int[NumAtoms()];
			if (NumAtoms() == 0)
				return ;
			bool[] visited = new bool[NumAtoms()];
			for (int n = 0; n < NumAtoms(); n++)
			{
				ringID[n] = 0;
				visited[n] = false;
			}
			
			int[] path = new int[NumAtoms() + 1];
			int plen = 0, numVisited = 0;
			bool rewound = false;
			while (true)
			{
				int last, current;
				
				if (plen == 0)
				// find an element of a new component to visit
				{
					last = - 1;
					for (current = 0; visited[current]; current++)
					{
					}
				}
				else
				{
					last = path[plen - 1];
					current = - 1;
					for (int n = 0; n < graph[last].Length; n++)
						if (!visited[graph[last][n]])
						{
							current = graph[last][n]; break;
						}
				}
				
				/*System.out.print("numVisited="+numVisited+" last="+last+" current="+current+" path=");
				for (int n=0;n<plen;n++) System.out.print(path[n]+" ");
				System.out.println();*/
				
				if (current >= 0 && plen >= 2)
				// path is at least 2 items long, so look for any not-previous visited neighbours
				{
					int back = path[plen - 1];
					//System.out.println(" back="+back);
					for (int n = 0; n < graph[current].Length; n++)
					{
						int join = graph[current][n];
						//System.out.println(" join="+join+" visited[join]="+visited[join]);
						if (join != back && visited[join])
						{
							//System.out.print(" path:");
							
							path[plen] = current;
							for (int i = plen; i == plen || path[i + 1] != join; i--)
							{
								//System.out.print(" "+path[i]);
								
								int id = ringID[path[i]];
								if (id == 0)
									ringID[path[i]] = last;
								else if (id != last)
								{
									for (int j = 0; j < NumAtoms(); j++)
										if (ringID[j] == id)
											ringID[j] = last;
								}
							}
							//System.out.println();
						}
					}
				}
				if (current >= 0)
				// can mark the new one as visited
				{
					visited[current] = true;
					path[plen++] = current;
					numVisited++;
					rewound = false;
					
					// !!
					//ringID[current]=numVisited;
				}
				// otherwise, found nothing and must rewind the path
				else
				{
					plen--;
					rewound = true;
				}
				
				if (numVisited == NumAtoms())
					break;
			}
			
			// the ring ID's are not necessarily consecutive, so reassign them to 0=none, 1..NBlocks
			int nextID = 0;
			for (int i = 0; i < NumAtoms(); i++)
				if (ringID[i] > 0)
				{
					nextID--;
					for (int j = NumAtoms() - 1; j >= i; j--)
						if (ringID[j] == ringID[i])
							ringID[j] = nextID;
				}
			for (int i = 0; i < NumAtoms(); i++)
				ringID[i] = - ringID[i];
		}
		

		
		// returns the angle maximally equidistant from Th1 and Th2
		internal virtual double ThetaObtuse(double Th1, double Th2)
		{
			double dth = Th2 - Th1;
			while (dth < - System.Math.PI)
				dth += 2 * System.Math.PI;
			while (dth > System.Math.PI)
				dth -= 2 * System.Math.PI;
			return dth > 0?Th1 - 0.5 * (2 * System.Math.PI - dth):Th1 + 0.5 * (2 * System.Math.PI + dth);
		}
	}
}