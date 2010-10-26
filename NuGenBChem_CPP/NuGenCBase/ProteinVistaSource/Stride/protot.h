/*
 * Stride has been included in Qmol with the kind permission of 
 * 
 * Dmitrij Frishman, PhD
 * Institute for Bioinformatics
 * GSF - Forschungszentrum f? Umwelt und Gesundheit, GmbH
 * Ingolst?ter Landstra? 1,
 * D-85764 Neuherberg, Germany
 *
 * Telephone: +49-89-3187-4201
 * Fax: +49-89-31873585
 * e-mail: d.frishman@gsf.de
 * WWW: http://mips.gsf.de/mips/staff/frishman/
 *
 * Stride copyright (see http://www.embl-heidelberg.de/stride/stride.html):
 *
 * All rights reserved, whether the whole  or  part  of  the  program  is
 * concerned.  Permission  to use, copy, and modify this software and its
 * documentation is granted for academic use, provided that:
 *
 *
 * i.	this copyright notice appears in all copies of the software  and
 *		related documentation;
 *
 * ii.  the reference given below (Frishman and  Argos,  1995)  must  be
 *		cited  in any publication of scientific results based in part or
 *		completely on the use of the program;
 *
 * iii.  bugs will be reported to the authors.
 *
 * The use of the  software  in  commercial  activities  is  not  allowed
 * without a prior written commercial license agreement.
 * 
 * WARNING: STRIDE is provided "as-is" and without warranty of any  kind,
 * express,  implied  or  otherwise,  including  without  limitation  any
 * warranty of merchantability or fitness for a particular purpose. In no
 * event will the authors be liable for any special, incidental, indirect
 * or consequential damages  of  any  kind,  or  any  damages  whatsoever
 * resulting  from loss of data or profits, whether or not advised of the
 * possibility of damage, and on any theory of liability, arising out  of
 * or in connection with the use or performance of this software.
 * 
 * For calculation of the residue solvent accessible area the program NSC
 * [3,4]   is   used   and   was  kindly  provided  by  Dr.  F.Eisenhaber
 * (EISENHABER@EMBL-HEIDELBERG.DE). Please direct to  him  all  questions
 * concerning specifically accessibility calculations.
 * 
 * Stride References:
 * 
 * 1.	Frishman,D & Argos,P. (1995) Knowledge-based secondary structure
 * 		assignment.  Proteins:  structure, function and genetics, 23,
 * 		566-579.
 * 
 * 2.	Kabsch,W. & Sander,C. (1983)  Dictionary  of  protein  secondary
 * 		structure:    pattern   recognition   of   hydrogen-bonded   and
 * 		geometrical features. Biopolymers, 22: 2577-2637.
 * 
 * 3.	Eisenhaber,  F.  and  Argos,  P.  (1993)  Improved  strategy  in
 * 		analytic  surface calculation for molecular systems: handling of
 * 		singularities and computational efficiency. J. comput. Chem. 14,
 * 		1272-1280.
 * 
 * 4.	Eisenhaber, F., Lijnzaad, P., Argos, P., Sander, C., and Scharf,
 * 		M.  (1995) The double cubic lattice method: efficient approaches
 * 		to numerical integration of surface area and volume and  to  dot
 * 		surface contouring of molecular assemblies. J. comput. Chem. 16,
 * 		273-284.
 * 
 * 5.	Bernstein, F.C., Koetzle, T.F.,  Williams,  G.J.,  Meyer,  E.F.,
 * 		Brice,  M.D.,  Rodgers,  J.R., Kennard, O., Shimanouchi, T., and
 * 		Tasumi, M.  (1977)  The  protein  data  bank:  a  computer-based
 * 		archival  file for macromolecular structures. J. Mol. Biol. 112,
 * 		535-542.
 * 
 * 6.	Kraulis, P.J.  (1991)  MOLSCRIPT:  a  program  to  produce  both
 * 		detailed  and  schematic  plots  of protein structures. J. Appl.
 * 		Cryst. 24, 946-950.
 * 
 * 7.	Pearson, W.R. (1990) Rapid  and  sensitive  sequence  comparison
 * 		with FASTP and FASTA. Methods. Enzymol. 183, 63-98.
 * 
 */
STRIDE_BOOL ChInStr(char *String, char Char);
STRIDE_BOOL ExistsSecStr(CHAIN **Chain, int NChain);
STRIDE_BOOL ExistSSBond(CHAIN **Chain,int NChain,int Cn1,int Cn2,char *Res1,char *Res2);
STRIDE_BOOL IsHydrogen(char *AtomName) ;
STRIDE_BOOL Specified(char **List, int ListLength, char Option);

FILE *efopen(char *file, char *mode, char *progname);

char **AllocAsn(CHAIN **Chain, int NChain);
char **CharMatrix(int M, int N);
char *OneToThree(char One);
char SpaceToDash(char Id);
char *Tim(void);
char ThreeToOne(char *Three);
char *tolostr(char *InputString);
char *Translate(char Code);

double GetAtomRadius(char *AtomType);

float Ang(float *Coord1, float *Coord2, float *Coord3);
float **DefaultHelixMap(COMMAND *Cmd);
float **DefaultSheetMap(COMMAND *Cmd);
float Dist(float *Coord1, float *Coord2);
float ***FloatCube(int M, int N, int K);
float **FloatMatrix(int M, int N);
float PercentCorrect(char *TestAsn, char *KnownAsn, int Length);
float SecStrContent(CHAIN *Chain, int *HelAlp, int *HelPI, int *Hel310, int *Sheet, int *Turn);
float Torsion(float *Coord1, float *Coord2, float *Coord3, float *Coord4);
float VectorProduct(float *Vector1, float *Vector2, float *Product);
float factrl(int n);

int AssessCorr(QUALITY *Qual);
int AssessPerc(QUALITY *Qual);
int Boundaries(char *Asn, int L, char SecondStr, int (*Bound)[2]);
int CheckAtom(char *At);
int CheckChain(CHAIN *Chain, COMMAND *Cmd);
int CheckRes(char *Res);
int CompareElements(char *Asn1, char *Asn2, char *Asn3, int Length, 
		   char SecStrType, int *Better, int *Worse);
int CompPdbDssp(CHAIN *Chain, DSSP *Dssp);
int CollectOptions(char **List, int ListLength, int Stream, int *Options);
int DefineAcceptor(CHAIN *Chain, ACCEPTOR **Acc, int *ac, int Res, enum HYBRID Hybrid,  
		   enum GROUP Group, float HB_Radius, int N);
int DefineDnr(CHAIN *Chain, DONOR **Dnr, int *dc, int Res, enum HYBRID Hybrid, 
	      enum GROUP Group,  float HB_Radius, int N);
int Delete(char *String, char From);
int Difference(char *TestAsn, char *KnownAsn, int Length, char SecStrType, QUALITY *Qual);
int escape(int RetVal, char *format, ... );
int FindAcc(CHAIN *Chain, ACCEPTOR **Acc, int *NAcc, COMMAND *Cmd);

/*
 * The function FindAtom is already defined by windows
 */
int stride_FindAtom(CHAIN *Chain, int ResNumb, char *Atom, int *AtNumb);

int FindBnd(HBOND **HBond, RESIDUE *Res1, RESIDUE *Res2);
int FindChain(CHAIN **Chain, int NChain, char ChainId);
int FindDnr(CHAIN *Chain, DONOR **Dnr, int *NDnr, COMMAND *Cmd);
int FindHydrogenBonds(CHAIN **Chain, int NChain, HBOND **HBond, COMMAND *Cmd,
					  DONOR ***Dnr, ACCEPTOR ***Acc);
int FindPolInt(HBOND **HBond, RESIDUE *Res1, RESIDUE *Res2);
int FullElement(char *Asn1, char *Asn2, char *Asn3, int Length, char SecStrType, 
		       int ElemLength, char EditChar, int *YYN, int *NYY, int *YNN, int *NNY);
int GetPdbChain(CHAIN **Chain, FILE *Db, long int Start);
int ***IntCube(int M, int N, int K);
int **IntMatrix(int M, int N);
int Link(HBOND **HBond, CHAIN **Chain, int Cn1, int Cn2, RESIDUE *Res1_1, 
	 RESIDUE *Res1_2, RESIDUE *Res2_2, RESIDUE *Res2_1, RESIDUE *CRes1, RESIDUE *CRes2,
	 float **PhiPsiMap, STRIDE_PATTERN **Pattern, int *NumPat, char *Text, float Treshold, 
	 COMMAND *Cmd, int Test);
int MakeEnds(int *Beg1, int ResBeg1, int NeiBeg1, char *Beg1Cn, char ResBeg1Cn, 
		    int *End1, int ResEnd1, int NeiEnd1, char ResEnd1Cn, int *Beg2, 
		    int ResBeg2, int NeiBeg2, char *Beg2Cn, char ResBeg2Cn, int *End2, 
		    int ResEnd2, int NeiEnd2, char ResEnd2Cn, STRIDE_PATTERN **Pat, int NPat);
int MolScript(CHAIN **Chain, int NChain, COMMAND *Cmd);
int Near(int Res1, int Res2, int Res3, int Res4, int Res5, int Res6, int Res7, int Res8,
	 char Cn1, char Cn2, char Cn3, char Cn4, int *DistBest, int *DistWorst);
int NearPar(int Res1, int Res2, int Res3, int Res4, int Res5, int Res6, int Res7, int Res8,
	 char Cn1, char Cn2, char Cn3, char Cn4, int *DistBest, int *DistWorst);
int NoDoubleHBond(HBOND **HBond, int NHBond);
int OutSeq(CHAIN **Chain, int NChain, COMMAND *Cmd);
int Parse(char **List, int ListLength, char *Option);
int PdbN2SeqN(CHAIN *Chain, char *PdbN, int *SeqN);
int PlaceHydrogens(CHAIN *Chain);
int Presnell(char *Asn1, int L1, char *Asn2, int L2, char SecStr, float Threshold, 
	       float *Q2, float *O);
int Process_ATOM(BUFFER Buffer, CHAIN **Chain, int *ChainNumber, 
		 STRIDE_BOOL *First_ATOM, COMMAND *Cmd);
int Process_COMPND(BUFFER Buffer, enum METHOD *Method);
int Process_ENDMDL(BUFFER Buffer, CHAIN **Chain, int *ChainNumber);
int Process_EXPDTA(BUFFER Buffer, enum METHOD *Method);
int Process_HELIX(BUFFER Buffer, CHAIN **Chain, int *ChainNumber, COMMAND *Cmd);
int Process_JRNL(BUFFER Buffer, STRIDE_BOOL *Published);
int Process_MODEL(enum METHOD *Method);
int Process_REMARK(BUFFER Buffer, enum METHOD *Method, float *Resolution, STRIDE_BOOL *DsspAssigned);
int Process_SHEET(BUFFER Buffer, CHAIN **Chain, int *ChainNumber, COMMAND *Cmd);
int Process_SSBOND(BUFFER Buffer, CHAIN **Chain, int *ChainNumber, COMMAND *Cmd);
int Process_TER(BUFFER Buffer, CHAIN **Chain, int *ChainNumber);
int Process_TURN(BUFFER Buffer, CHAIN **Chain, int *ChainNumber, COMMAND *Cmd);
int ReadDSSP(CHAIN **Chain, DSSP **Dssp, COMMAND *Cmd);
int ReadPDBFile(CHAIN **Chain, int *NChain, COMMAND *Cmd);
int ReadPhiPsiMap(char *MapFile, float ***PhiPsiMap, COMMAND *Cmd);
int Replace(char *String, char From, char To);
int ResInSecondStr(int ResNumb, int (*Bound)[2], int N, int *StrNumb);
int RightSide(int LnkA, int LnkD, int I1A, int I1D, int I2A, int I2D );
int RightSide2(int L_A1, int L_D1, int LnkD, int LnkA, int I1A, int I1D, int I2A, int I2D);
int RightSidePar(int LnkA, int LnkD, int I1A, int I1D, int I2A, int I2D );
int SplitString(char *Buffer, char **Fields, int MaxField);
int SSBond(CHAIN **Chain, int NChain);
int TorsBracket(float Torsion, float Min, float Max);
int TurnCondition(float Phi2,float Phi2S,float Psi2,float Psi2S,
		  float Phi3,float Phi3S,float Psi3,float Psi3S,
		  float Range1,float Range2);
int Uniq(char **List, int ListLength);
void Alias(int *D1,int *A1,int *D2,int *A2,char *D1Cn,char *A1Cn,char *D2Cn,char *A2Cn,
	  STRIDE_PATTERN *Pat);
void AllocChain(CHAIN **Chain);
void Area(CHAIN **Chain, int NChain, COMMAND *Cmd);
void BackboneAngles(CHAIN **Chain, int NChain);
void BetaTurn(CHAIN **Chain, int Cn);
void Bridge(char *Asn1, char *Asn2, CHAIN **Chain, int Cn1, int Cn2, STRIDE_PATTERN **Pat, int NPat);
void *ckalloc(size_t bytes);
void CorrectAsn(char *Asn, int Length, char SecStrType, char EditChar, int MaxLength);
void CorrectAsnDouble(char *Asn1, char *Asn2, char *KnownAsn, int Length, 
		      char SecStrType, char EditChar);
void DeallocAcc(DONOR **Acc, int AccNumber);
void DeallocDnr(DONOR **Dnr, int DonNumber);
void DefaultCmd(COMMAND *Cmd, int max_num_res);
void die(char *format, ... );
void DiscrPhiPsi(CHAIN **Chain, int NChain, COMMAND *Cmd);
void DistMatrix(CHAIN *Chain);
void DSSP_Energy(float *Dummy, float *C, float *O, float *H, float *N, COMMAND *Cmd, 
				 HBOND *HBond);
void GRID_Energy(float *CA2, float *C, float *O, float *H, float *N, COMMAND *Cmd, 
				 HBOND *HBond);
void ExcludeObvious(char *Asn1, char *Asn2, char *KnownAsn, int Length);
void ExtractAsn(CHAIN **Chain, int Cn, char *Asn);
void ExtractPdbAsn(CHAIN **Chain, int Cn, char *Asn);
void ExtractDsspAsn(CHAIN **Chain, int Cn, char *Asn);
void FillAsnAntiPar(char *Asn1, char *Asn2, CHAIN **Chain, int Cn1, int Cn2, 
		    STRIDE_PATTERN **Pat, int NPat, COMMAND *Cmd);
void FillAsnPar(char *Asn1, char *Asn2, CHAIN **Chain, int Cn1, int Cn2, 
		STRIDE_PATTERN **Pat, int NPat, COMMAND *Cmd);
void FilterAntiPar(STRIDE_PATTERN **Pat, int NPat);
void FilterPar(STRIDE_PATTERN **Pat, int NPat);
void FreeCharMatrix(char **Matrix, int M);
void FreeFloatMatrix(float **Matrix, int M);
void FreeIntMatrix(int **Matrix, int M);
void GammaTurn(CHAIN **Chain, int Cn, HBOND **HBond);
void GetFileNameFromPath(char *Path, char *FileName);
void GetDsspAsn(CHAIN **Chain, int NChain, COMMAND *Cmd);
void GetPdbAsn(CHAIN **Chain, int NChain);
void Glue(char *String1, char *String2, FILE *Out);
void Helix(CHAIN **Chain, int Cn, HBOND **HBond, COMMAND *Cmd, float **PhiPsiMap);
void HBondToBins(HBOND **HBond, int NHBond, COMMAND *Cmd);
void InitAsn(CHAIN **Chain, int NChain);
void StrideInitChain(CHAIN **Chain, COMMAND *Cmd);
void InsertFirst(DSSP *Dssp, CHAIN *Chain);
void InsertLast(DSSP *Dssp, CHAIN *Chain);
void JoinNeighb(STRIDE_PATTERN **Nei, STRIDE_PATTERN *Pat, int *MinDB2, int DB, int *MinDW2, int DW);
void JoinNeighbours(int *Lnk1A, int Res1, int *Lnk1D, int Res2, STRIDE_PATTERN **Nei, 
		    STRIDE_PATTERN *Pat, int *MinDB1, int DB, int *MinDW1, int DW, int *Min, int j);
void Measure(CHAIN **Chain, int NChain, int El, COMMAND *Cmd, FILE *Out);
void MergePatternsPar(STRIDE_PATTERN **Pat, int NPat);
void MergePatternsAntiPar(STRIDE_PATTERN **Pat, int NPat);
int NotValid(CHAIN *Chain, char *Message);
void OMEGA(CHAIN *Chain, int Res);
void PHI(CHAIN *Chain, int Res);
void Place123_X(float *Coord1, float *Coord2, float *Coord3, float Dist3X, float Ang23X,  
		float *CoordX);
void PrepareBuffer(BUFFER Bf, CHAIN **Chain);
void PrintHydrBond(char *Text, HBOND *HBond);
void PrintPatterns(STRIDE_PATTERN **Pat, int NPat, CHAIN **Chain, int Cn1, int Cn2);
void Project4_123(float *Coord1, float *Coord2, float *Coord3, float *Coord4,  
		  float *Coord_Proj4_123);
void PSI(CHAIN *Chain, int Res);
void Report(CHAIN **Chain, int NChain, HBOND **HBond, COMMAND *Cmd);
void ReportDetailed(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd);
void ReportGeneral(CHAIN **Chain, FILE *Out);
void ReportHydrBonds(CHAIN **Chain, int NChain, HBOND **HBond, 
			    FILE *Out, COMMAND *Cmd);
void ReportShort(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd);
void ReportSSBonds(CHAIN **Chain, FILE *Out);
void ReportSummary(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd);
void ReportTurnTypes(CHAIN **Chain, int NChain, FILE *Out, COMMAND *Cmd);
void Sheet(CHAIN **Chain, int Cn1, int Cn2, HBOND **HBond, COMMAND *Cmd, float **PhiPsiMap);
void StringSort(char **Strings, int left, int right, int StrLen);
void StripPathFromLastExtention(char *Path, char *StrippedPath);





