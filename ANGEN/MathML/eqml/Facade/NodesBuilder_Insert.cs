using Fonts;
using Nodes;

namespace Facade
{
    public partial class NodesBuilder
    {
        public void InvisibleTimes()
        {
            insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&InvisibleTimes;</mo></math>");
        }

        public
            void insertEntity_Identifier(Glyph entity, bool bItalic, bool bBold)
        {
            this.insertEntity_Identifier(entity.Name, bItalic, bBold);
        }

        public
            void insertEntity_Identifier(string entityName, bool bItalic, bool bBold)
        {
            if (!bItalic && !bBold)
            {
                this.insertMathML(
                    "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd='' mathvariant=\"normal\">&" +
                    entityName + ";</mi></math>");
            }
            else if (!bItalic && bBold)
            {
                this.insertMathML(
                    "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd='' mathvariant=\"bold\">&" +
                    entityName + ";</mi></math>");
            }
            else if (bItalic && bBold)
            {
                this.insertMathML(
                    "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd='' mathvariant=\"bold-italic\">&" +
                    entityName + ";</mi></math>");
            }
            else
            {
                this.insertMathML("<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd=''>&" +
                                  entityName +
                                  ";</mi></math>");
            }
        }

        public
            void InsertFraction()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac><mrow nugenCursor=''/><mrow/></mfrac></math>");
        }

        public
            void InsertFraction_Bevelled()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac bevelled=\"true\"><mrow nugenCursor=''/><mrow/></mfrac></math>");
        }

        public
            void insertText()
        {
            this.insertMathML("<math xmlns='http://www.w3.org/1998/Math/MathML'><mtext nugenCursor=''> </mtext></math>");
        }

        public
            void InsertAction()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><maction actiontype='statusline' selection='1'><mrow nugenCursor=''/><mtext>..[Insert Text]...</mtext></maction></math>");
        }

        public
            void InsertFenced()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mfenced><mrow nugenCursor=''/><mrow/></mfenced></math>");
        }

        public
            void InsertPhantom()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mphantom><mrow nugenCursor=''/></mphantom></math>");
        }

        public void InsertEntity()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&sum;</mo></math>");
        }

        public void InsertEntity(string sEntity)
        {
            this.insertMathML("<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&" + sEntity +
                              ";</mo></math>");
        }

        public void InsertUnderover()
        {
            this.insertMathML(
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><munderover><mo>&prod;</mo><mrow nugenCursor=''/><mrow/></munderover></math>");
        }


        public void InsertSubscript ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><msub><mrow nugenCursor=''/><mrow/></msub></math>");
        }

        public void InsertSuperScript ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><msup><mrow nugenCursor=''/><mrow/></msup></math>");
        }

        public void InsertSubSup ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><msubsup><mrow nugenCursor=''/><mrow/><mrow/></msubsup></math>");
        }

        public void InsertSqrt ()
        {
            this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><msqrt><mrow nugenCursor=''/></msqrt></math>");
        }

        public void InsertRoot ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mroot><mrow nugenCursor=''/><mrow/></mroot></math>");
        }

        public void InsertFenced (string sCharL, string sCharR)
        {
            this.insertMathML (
                string.Concat (
                    new string[]
                        {
                            "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo>", sCharL,
                            "</mo><mrow nugenCursor=''/><mo>", sCharR, "</mo></mrow></math>"
                        }));
        }

        public void InsertFenced (string sEntityName_Left, string sEntityName_Right, bool bStretchy)
        {
            this.InsertFenced (true, sEntityName_Left, sEntityName_Right, bStretchy);
        }

        public void InsertFenced (bool bOnInsert, string sEntityName_Left, string sEntityName_Right, bool bStretchy)
        {
            try
            {
                if ((sEntityName_Left.Length == 0) && (sEntityName_Right.Length == 0))
                {
                    return;
                }
                if (sEntityName_Left.Length == 0)
                {
                    if (bStretchy)
                    {
                        this.insertMathML (bOnInsert,
                                   "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mrow nugenCursor=''/><mo>&" +
                                   sEntityName_Right + ";</mo></mrow></math>");
                    }
                    else
                    {
                        this.insertMathML (bOnInsert,
                                   "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mrow nugenCursor=''/><mo stretchy=\"false\">&" +
                                   sEntityName_Right + ";</mo></mrow></math>");
                    }
                }
                else if (sEntityName_Right.Length == 0)
                {
                    if (bStretchy)
                    {
                        this.insertMathML (bOnInsert,
                                   "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo>&" + sEntityName_Left +
                                   ";</mo><mrow nugenCursor=''/></mrow></math>");
                    }
                    else
                    {
                        this.insertMathML (bOnInsert,
                                   "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo stretchy=\"false\">&" +
                                   sEntityName_Left + ";</mo><mrow nugenCursor=''/></mrow></math>");
                    }
                }
                else if (bStretchy)
                {
                    this.insertMathML (bOnInsert,
                               string.Concat (
                                   new string[]
                                       {
                                           "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo>&", sEntityName_Left
                                           , ";</mo><mrow nugenCursor=''/><mo>&", sEntityName_Right, ";</mo></mrow></math>"
                                       }));
                }
                else
                {
                    this.insertMathML (bOnInsert,
                               string.Concat (
                                   new string[]
                                       {
                                           "<math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo stretchy=\"false\">&"
                                           , sEntityName_Left, ";</mo><mrow nugenCursor=''/><mo stretchy=\"false\">&",
                                           sEntityName_Right, ";</mo></mrow></math>"
                                       }));
                }
            }
            catch
            {
            }
        }

        public void InsertUnder ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><munder><mrow nugenCursor=''/><mrow/></munder></math>");
        }

        public void InsertOver ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mover><mrow nugenCursor=''/><mrow/></mover></math>");
        }

        public void InsertUnderOver ()
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><munderover><mrow nugenCursor=''/><mrow/><mrow/></munderover></math>");
        }

        public void InsertOverAccent (string sEntity)
        {
            if (sEntity.Length > 0)
            {
                string xml = "<math xmlns='http://www.w3.org/1998/Math/MathML'><mover><mrow nugenCursor=''/><mo>&";
                xml = xml + sEntity;
                xml = xml + ";</mo></mover></math>";
                this.insertMathML (xml);
            }
        }

        public void InsertUnderAccent (string sEntity)
        {
            if (sEntity.Length > 0)
            {
                string xml = "<math xmlns='http://www.w3.org/1998/Math/MathML'><munder><mrow nugenCursor=''/><mo>&";
                xml = xml + sEntity;
                xml = xml + ";</mo></munder></math>";
                this.insertMathML (xml);
            }
        }

        public void InsertPrime (string sEntity)
        {
            if (sEntity.Length > 0)
            {
                string xml = "<math xmlns='http://www.w3.org/1998/Math/MathML'><msup><mrow nugenCursor=''/><mo>&";
                xml = xml + sEntity;
                xml = xml + ";</mo></msup></math>";
                this.insertMathML (xml);
            }
        }

        public void InsertMatrix (int nRows, int nCols)
        {
            if (this.ScriptEligible () && ((nRows > 0) && (nCols > 0)))
            {
                int i = 0;
                int j = 0;
                string xml = "";
                xml = "<math xmlns='http://www.w3.org/1998/Math/MathML'><mtable>";
                for (i = 0; i < nRows; i++)
                {
                    xml = xml + "<mtr>";
                    for (j = 0; j < nCols; j++)
                    {
                        if ((i == 0) && (j == 0))
                        {
                            xml = xml + "<mtd><mrow nugenCursor=''/></mtd>";
                        }
                        else
                        {
                            xml = xml + "<mtd><mrow/></mtd>";
                        }
                    }
                    xml = xml + "</mtr>";
                }
                xml = xml + "</mtable></math>";
                this.insertMathML (xml);
            }
        }

        public void InsertStretchyArrow_Over (string entityName)
        {
            try
            {
                this.insertMathML ("<math xmlns=\"http://www.w3.org/1998/Math/MathML\"><mover><mo>&" + entityName +
                           ";</mo><mrow nugenCursor=''/></mover></math>");
            }
            catch
            {
            }
        }

        public void InsertStretchyArrow_Under (string entityName)
        {
            try
            {
                this.insertMathML ("<math xmlns=\"http://www.w3.org/1998/Math/MathML\"><munder><mo>&" + entityName +
                           ";</mo><mrow nugenCursor=''/></munder></math>");
            }
            catch
            {
            }
        }

        public void InsertStretchyArrow_UnderOver (string entityName)
        {
            try
            {
                this.insertMathML ("<math xmlns=\"http://www.w3.org/1998/Math/MathML\"><munderover><mo>&" + entityName +
                           ";</mo><mrow nugenCursor=''/><mrow/></munderover></math>");
            }
            catch
            {
            }
        }

        public void InsertEntitySmall (string entityName)
        {
            this.insertMathML ("<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd=''>&" + entityName +
                       ";</mi></math>");
        }

        public void InsertEntityBig (string entityName)
        {
            this.insertMathML (
                "<math xmlns='http://www.w3.org/1998/Math/MathML'><mi nugenCursorEnd='' mathvariant=\"normal\">&" +
                entityName + ";</mi></math>");
        }

        public void InsertEntityOperator (Glyph entity)
        {
            if (!this.StretchyBrackets && this.IsStretchy (entity))
            {
                this.insertMathML (
                    "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo stretchy=\"false\" nugenCursorEnd=''>&" +
                    entity.Name + ";</mo></math>");
            }
            else
            {
                this.insertMathML (
                    "<math xmlns='http://www.w3.org/1998/Math/MathML'><mo nugenCursorEnd=''>&" + entity.Name +
                           ";</mo></math>");
            }
        }

        public void InsertEntityOperator (string entityName)
        {
            Glyph glyph = null;
            try
            {
                glyph = this.entityManager.ByName (entityName);
            }
            catch
            {
            }
            if (glyph != null)
            {
                this.InsertEntityOperator (glyph);
            }
        }
    }
}