//
// In order to convert some functionality to Visual C#, the Java Language Conversion Assistant
// creates "support classes" that duplicate the original functionality.  
//
// Support classes replicate the functionality of the original code, but in some cases they are 
// substantially different architecturally. Although every effort is made to preserve the 
// original architecture of the application in the converted project, the user should be aware that 
// the primary goal of these support classes is to replicate functionality, and that at times 
// the architecture of the resulting solution may differ somewhat.
//

using System;
using System.Collections;

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
internal class SupportClass
{
    /// <summary>
    /// SupportClass for the BitArray class.
    /// </summary>
    public class BitArraySupport
    {
        /// <summary>
        /// Sets the specified bit to true.
        /// </summary>
        /// <param name="bits">The BitArray to modify.</param>
        /// <param name="index">The bit index to set to true.</param>
        public static void Set(BitArray bits, System.Int32 index)
        {
            for (int increment = 0; index >= bits.Length; increment = +64)
            {
                bits.Length += increment;
            }

            bits.Set(index, true);
        }

        /// <summary>
        /// Returns a string representation of the BitArray object.
        /// </summary>
        /// <param name="bits">The BitArray object to convert to string.</param>
        /// <returns>A string representation of the BitArray object.</returns>
        public static string ToString(System.Collections.BitArray bits)
        {
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            if (bits != null)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    if (bits[i] == true)
                    {
                        if (s.Length > 0)
                            s.Append(", ");
                        s.Append(i);
                    }
                }

                s.Insert(0, "{");
                s.Append("}");
            }
            else
                s.Insert(0, "null");

            return s.ToString();
        }
    }

    /// <summary>
    /// The class performs token processing in strings
    /// </summary>
    public class Tokenizer : System.Collections.IEnumerator
    {
        /// Position over the string
        private long currentPos = 0;

        /// Include demiliters in the results.
        private bool includeDelims = false;

        /// Char representation of the string to tokenize.
        private char[] chars = null;

        //The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character and the form-feed character
        private string delimiters = " \t\n\r\f";

        /// <summary>
        /// Initializes a new class instance with a specified string to process
        /// </summary>
        /// <param name="source">string to tokenize</param>
        public Tokenizer(string source)
        {
            this.chars = source.ToCharArray();
        }

        /// <summary>
        /// Initializes a new class instance with a specified string to process
        /// and the specified token delimiters to use
        /// </summary>
        /// <param name="source">string to tokenize</param>
        /// <param name="delimiters">string containing the delimiters</param>
        public Tokenizer(string source, string delimiters)
            : this(source)
        {
            this.delimiters = delimiters;
        }


        /// <summary>
        /// Initializes a new class instance with a specified string to process, the specified token 
        /// delimiters to use, and whether the delimiters must be included in the results.
        /// </summary>
        /// <param name="source">string to tokenize</param>
        /// <param name="delimiters">string containing the delimiters</param>
        /// <param name="includeDelims">Determines if delimiters are included in the results.</param>
        public Tokenizer(string source, string delimiters, bool includeDelims)
            : this(source, delimiters)
        {
            this.includeDelims = includeDelims;
        }


        /// <summary>
        /// Returns the next token from the token list
        /// </summary>
        /// <returns>The string value of the token</returns>
        public string NextToken()
        {
            return NextToken(this.delimiters);
        }

        /// <summary>
        /// Returns the next token from the source string, using the provided
        /// token delimiters
        /// </summary>
        /// <param name="delimiters">string containing the delimiters to use</param>
        /// <returns>The string value of the token</returns>
        public string NextToken(string delimiters)
        {
            //According to documentation, the usage of the received delimiters should be temporary (only for this call).
            //However, it seems it is not true, so the following line is necessary.
            this.delimiters = delimiters;

            //at the end 
            if (this.currentPos == this.chars.Length)
                throw new System.ArgumentOutOfRangeException();
            //if over a delimiter and delimiters must be returned
            else if ((System.Array.IndexOf(delimiters.ToCharArray(), chars[this.currentPos]) != -1)
                     && this.includeDelims)
                return "" + this.chars[this.currentPos++];
            //need to get the token wo delimiters.
            else
                return nextToken(delimiters.ToCharArray());
        }

        //Returns the nextToken wo delimiters
        private string nextToken(char[] delimiters)
        {
            string token = "";
            long pos = this.currentPos;

            //skip possible delimiters
            while (System.Array.IndexOf(delimiters, this.chars[currentPos]) != -1)
                //The last one is a delimiter (i.e there is no more tokens)
                if (++this.currentPos == this.chars.Length)
                {
                    this.currentPos = pos;
                    throw new System.ArgumentOutOfRangeException();
                }

            //getting the token
            while (System.Array.IndexOf(delimiters, this.chars[this.currentPos]) == -1)
            {
                token += this.chars[this.currentPos];
                //the last one is not a delimiter
                if (++this.currentPos == this.chars.Length)
                    break;
            }
            return token;
        }


        /// <summary>
        /// Determines if there are more tokens to return from the source string
        /// </summary>
        /// <returns>True or false, depending if there are more tokens</returns>
        public bool HasMoreTokens()
        {
            //keeping the current pos
            long pos = this.currentPos;

            try
            {
                this.NextToken();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return false;
            }
            finally
            {
                this.currentPos = pos;
            }
            return true;
        }

        /// <summary>
        /// Remaining tokens count
        /// </summary>
        public int Count
        {
            get
            {
                //keeping the current pos
                long pos = this.currentPos;
                int i = 0;

                try
                {
                    while (true)
                    {
                        this.NextToken();
                        i++;
                    }
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    this.currentPos = pos;
                    return i;
                }
            }
        }

        /// <summary>
        ///  Performs the same action as NextToken.
        /// </summary>
        public object Current
        {
            get
            {
                return (object)this.NextToken();
            }
        }

        /// <summary>
        //  Performs the same action as HasMoreTokens.
        /// </summary>
        /// <returns>True or false, depending if there are more tokens</returns>
        public bool MoveNext()
        {
            return this.HasMoreTokens();
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Reset()
        {
            ;
        }
    }
    /*******************************/
    /// <summary>
    /// Writes the exception stack trace to the received stream
    /// </summary>
    /// <param name="throwable">Exception to obtain information from</param>
    /// <param name="stream">Output sream used to write to</param>
    public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
    {
        stream.Write(throwable.StackTrace);
        stream.Flush();
    }

}
