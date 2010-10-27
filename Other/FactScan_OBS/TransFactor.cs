namespace FacScan
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Windows.Forms;

    public class TransFactor
    {
        public TransFactor()
        {
            this.Fac = new DataTable("Fact");
            this.Seq = new DataTable("Seq");
            this.FPos = new DataTable("FacPos");
            this.calc = 0;
        }

        public void creatDataTables()
        {
            DataColumn column1 = this.Fac.Columns.Add("id", typeof(int));
            column1.AllowDBNull = false;
            column1.Unique = true;
            column1.AutoIncrement = true;
            column1.AutoIncrementSeed = 1;
            column1.AutoIncrementStep = 1;
            this.Fac.Columns.Add("fact", typeof(string));
            this.Fac.Columns.Add("site", typeof(string));
            this.Fac.Columns.Add("patt", typeof(string));
            this.Fac.Columns.Add("leng", typeof(string));
            column1 = this.Fac.Columns.Add("acce", typeof(string));
            column1.AllowDBNull = false;
            column1.Unique = true;
            column1.AutoIncrement = false;
            this.Fac.Columns.Add("cite", typeof(string));
            this.Fac.Columns.Add("spec", typeof(string));
            DataColumn[] columnArray1 = new DataColumn[] { this.Fac.Columns["acce"] };
            this.Fac.PrimaryKey = columnArray1;
            column1 = this.Seq.Columns.Add("id", typeof(int));
            column1.AllowDBNull = false;
            column1.Unique = true;
            column1.AutoIncrement = true;
            column1.AutoIncrementSeed = 1;
            column1.AutoIncrementStep = 1;
            this.Seq.Columns.Add("name", typeof(string));
            this.Seq.Columns["name"].Unique = true;
            columnArray1 = new DataColumn[] { this.Seq.Columns["name"] };
            this.Seq.PrimaryKey = columnArray1;
            this.Seq.Columns.Add("seq", typeof(string));
            column1 = this.FPos.Columns.Add("id", typeof(int));
            column1.AllowDBNull = false;
            column1.Unique = true;
            column1.AutoIncrement = true;
            column1.AutoIncrementSeed = 1;
            column1.AutoIncrementStep = 1;
            this.FPos.Columns.Add("gene", typeof(string));
            this.FPos.Columns.Add("acce", typeof(string));
            this.FPos.Columns.Add("pos", typeof(string));
            this.FPos.Columns.Add("uid", typeof(string));
            columnArray1 = new DataColumn[] { this.FPos.Columns["uid"] };
            this.FPos.PrimaryKey = columnArray1;
            this.ComFP = this.FPos.Clone();
        }

        public Array DataColumnToArray(DataTable dt, string column)
        {
            ArrayList list1 = new ArrayList();
            DataView view1 = dt.DefaultView;
            for (int num1 = 0; num1 < dt.Rows.Count; num1++)
            {
                list1.Add(view1[num1][column].ToString());
            }
            return list1.ToArray();
        }

        public string dup(string str, int n)
        {
            string text1 = "";
            for (int num1 = 0; num1 < n; num1++)
            {
                text1 = text1 + str;
            }
            return text1;
        }

        public DataTable findFac(string name, string s)
        {
            DataTable table1 = this.FPos.Clone();
            DataView view1 = this.Fac.DefaultView;
            try
            {
                for (int num1 = 0; num1 < this.Fac.Rows.Count; num1++)
                {
                    string text2 = view1[num1]["patt"].ToString().ToUpper();
                    for (int num2 = 0; num2 < (s.Length - text2.Length); num2++)
                    {
                        DataRow row1;
                        string text1 = s.Substring(num2, text2.Length).ToUpper();
                        if (this.matchFac(text1, text2))
                        {
                            row1 = table1.NewRow();
                            row1[1] = name;
                            row1[2] = view1[num1]["acce"];
                            row1[3] = "(+)" + ((num2 + 1)).ToString();
                            row1[4] = name + row1[2].ToString() + row1[3].ToString();
                            table1.Rows.Add(row1);
                        }
                        char[] chArray1 = text1.ToCharArray();
                        Array.Reverse(chArray1);
                        for (int num3 = 0; num3 < chArray1.Length; num3++)
                        {
                            if (chArray1[num3] == 'A')
                            {
                                chArray1[num3] = 'T';
                            }
                            else if (chArray1[num3] == 'T')
                            {
                                chArray1[num3] = 'A';
                            }
                            else if (chArray1[num3] == 'C')
                            {
                                chArray1[num3] = 'G';
                            }
                            else if (chArray1[num3] == 'G')
                            {
                                chArray1[num3] = 'C';
                            }
                        }
                        text1 = new string(chArray1);
                        if (this.matchFac(text1, text2))
                        {
                            row1 = table1.NewRow();
                            row1[1] = name;
                            row1[2] = view1[num1]["acce"];
                            row1[3] = "(-)" + ((num2 + 1)).ToString();
                            row1[4] = name + row1[2].ToString() + row1[3].ToString();
                            table1.Rows.Add(row1);
                        }
                    }
                    this.calc = num1;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Find Factor Error: " + exception1.Message);
            }
            return table1;
        }

        public bool isExist(DataRow dr, DataTable dt, int start)
        {
            int num1 = 0;
            if (dr.Table.Columns.Count != dt.Columns.Count)
            {
                throw new ColumnNumberNotEqualException();
            }
            for (int num2 = 0; num2 < dt.Rows.Count; num2++)
            {
                for (int num3 = start; num3 < dt.Columns.Count; num3++)
                {
                    if (dr[num3].ToString() == dt.Rows[num2][num3].ToString())
                    {
                        num1++;
                    }
                }
                if (num1 == dt.Columns.Count)
                {
                    return true;
                }
                num1 = 0;
            }
            return false;
        }

        private bool matchFac(string window, string pattern)
        {
            int num1 = 0;
            try
            {
                if (window.Length != pattern.Length)
                {
                    return false;
                }
                if (window == pattern)
                {
                    return true;
                }
                for (int num2 = 0; num2 < window.Length; num2++)
                {
                    char ch1 = pattern[num2];
                    char ch2 = window[num2];
                    switch (ch1)
                    {
                        case 'A':
                        case 'C':
                        case 'G':
                        case 'T':
                            if (ch1 == ch2)
                            {
                                num1++;
                            }
                            goto Label_015C;

                        case 'B':
                            if (ch2 != 'A')
                            {
                                num1++;
                            }
                            goto Label_015C;

                        case 'D':
                            if (ch2 != 'C')
                            {
                                num1++;
                            }
                            goto Label_015C;

                        case 'E':
                        case 'F':
                        case 'I':
                        case 'J':
                        case 'L':
                        case 'O':
                        case 'P':
                        case 'Q':
                        case 'U':
                        case 'X':
                            goto Label_015C;

                        case 'H':
                            if (ch2 != 'G')
                            {
                                num1++;
                            }
                            goto Label_015C;

                        case 'K':
                            switch (ch2)
                            {
                                case 'G':
                                case 'T':
                                    num1++;
                                    break;
                            }
                            goto Label_015C;

                        case 'M':
                            switch (ch2)
                            {
                                case 'C':
                                case 'A':
                                    num1++;
                                    break;
                            }
                            goto Label_015C;

                        case 'N':
                            goto Label_0158;

                        case 'R':
                            switch (ch2)
                            {
                                case 'A':
                                case 'G':
                                    num1++;
                                    break;
                            }
                            goto Label_015C;

                        case 'S':
                            switch (ch2)
                            {
                                case 'C':
                                case 'G':
                                    num1++;
                                    break;
                            }
                            goto Label_015C;

                        case 'V':
                            if (ch2 != 'T')
                            {
                                num1++;
                            }
                            goto Label_015C;

                        case 'W':
                            switch (ch2)
                            {
                                case 'A':
                                case 'T':
                                    num1++;
                                    break;
                            }
                            goto Label_015C;

                        case 'Y':
                            switch (ch2)
                            {
                                case 'C':
                                case 'T':
                                    num1++;
                                    break;
                            }
                            goto Label_015C;
                    }
                    goto Label_015C;
                Label_0158:
                    num1++;
                Label_015C:;
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Match Factor Error: " + exception1.Message);
            }
            if (num1 == window.Length)
            {
                return true;
            }
            return false;
        }

        public void searchCommon()
        {
            try
            {
                DataTable table1 = this.FPos.Copy();
                DataView view1 = table1.DefaultView;
                this.calc = 0;
                view1.Sort = "acce,gene";
                this.accAl = this.unique(view1.Table, "acce");
                this.seqAl = new ArrayList[this.accAl.Count];
                for (int num1 = 0; num1 < this.accAl.Count; num1++)
                {
                    this.seqAl[num1] = new ArrayList();
                }
                for (int num2 = 0; num2 < view1.Count; num2++)
                {
                    int num3 = this.accAl.IndexOf(view1[num2]["acce"]);
                    string text1 = view1[num2]["gene"].ToString();
                    this.seqAl[num3].Add(text1);
                }
                for (int num4 = 0; num4 < this.accAl.Count; num4++)
                {
                    this.calc = num4;
                    if (this.unique(this.seqAl[num4]).Count == 1)
                    {
                        for (int num5 = 0; num5 < view1.Count; num5++)
                        {
                            if (view1[num5]["acce"].ToString() == this.accAl[num4].ToString())
                            {
                                view1[num5].Delete();
                                num5--;
                            }
                        }
                    }
                }
                this.ComFP = view1.Table.Copy();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Search Common Factor Error: " + exception1.Message);
            }
        }

        public ArrayList unique(ArrayList multi)
        {
            ArrayList list1 = new ArrayList();
            for (int num1 = 0; num1 < multi.Count; num1++)
            {
                if (!list1.Contains(multi[num1]))
                {
                    list1.Add(multi[num1]);
                }
            }
            return list1;
        }

        public ArrayList unique(DataTable dt, string column)
        {
            ArrayList list1 = new ArrayList();
            for (int num1 = 0; num1 < dt.Rows.Count; num1++)
            {
                string text1 = dt.Rows[num1][column].ToString();
                if (!list1.Contains(text1))
                {
                    list1.Add(text1);
                }
            }
            return list1;
        }


        public ArrayList AcceList
        {
            get
            {
                return this.accAl;
            }
        }

        public int CalculationsDone
        {
            get
            {
                return this.calc;
            }
        }

        public DataTable CommonFactorPosition
        {
            get
            {
                return this.ComFP;
            }
            set
            {
                this.ComFP = value;
            }
        }

        public DataTable Factor
        {
            get
            {
                return this.Fac;
            }
            set
            {
                this.Fac = value;
            }
        }

        public DataTable FactorPosition
        {
            get
            {
                return this.FPos;
            }
            set
            {
                this.FPos = value;
            }
        }

        public ArrayList[] SeqList
        {
            get
            {
                return this.seqAl;
            }
        }

        public DataTable Sequence
        {
            get
            {
                return this.Seq;
            }
            set
            {
                this.Seq = value;
            }
        }

        public DataTable TFDPubMedRef
        {
            get
            {
                return this.TFDRef;
            }
        }


        private ArrayList accAl;
        private int calc;
        private DataTable ComFP;
        private DataTable Fac;
        private DataTable FPos;
        private DataTable Seq;
        private ArrayList[] seqAl;
        private DataTable TFDRef;
    }
}

