namespace JpegComment
{
    using System;
    using System.IO;
    using System.Text;

        public class CommentBlock
    {
        public CommentBlock()
        {
            this.start = 0;
            this.end = 0;
            this.comment = "";
        }

        public bool Valid()
        {
            bool r = false;
            if (((this.start > 0) && (this.end > this.start)) && (this.comment.Length > 0))
            {
                r = true;
            }
            return r;
        }


        public int start;
        public int end;
        public string comment;
    }

    public class CommentIO
    {
        public CommentIO()
        {
            this.filename = "";
            this.notEmpty = false;
        }

        public bool NotEmpty(string fileName)
        {
            int length = 0;
            bool result = false;
            FileStream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                length = (int) stream.Length;
                if (length > 0)
                {
                    this.filename = fileName;
                    this.notEmpty = true;
                    result = true;
                }
            }
            catch
            {
                this.filename = "";
            }
            try
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            catch
            {
            }
            return result;
        }

        public CommentBlock ReadComment()
        {
            CommentBlock commentBlock = null;
            int length = 0;
            string s = "";
            int i = 0;
            bool done = false;
            FileStream stream = null;
            BinaryReader reader = null;
            if (!this.notEmpty)
            {
                return null;
            }
            try
            {
                stream = new FileStream(this.filename, FileMode.Open, FileAccess.Read);
                length = (int) stream.Length;
                if (length <= 0)
                {
                    return commentBlock;
                }
                reader = new BinaryReader(stream);
                byte[] bytes = reader.ReadBytes(length);
                while ((i < length) && !done)
                {
                    if ((bytes[i] == 0xff) && (bytes[i + 1] == 0xfe))
                    {
                        if (commentBlock == null)
                        {
                            commentBlock = new CommentBlock();
                        }
                        if (commentBlock.start == 0)
                        {
                            commentBlock.start = i;
                        }
                        i += 4;
                        while ((bytes[i] != 0xff) && (i < length))
                        {
                            i++;
                        }
                        if ((bytes[i] == 0xff) && (commentBlock.end == 0))
                        {
                            commentBlock.end = i;
                            int len = (commentBlock.end - commentBlock.start) - 4;
                            int off = commentBlock.start + 4;
                            if (len > 0)
                            {
                                byte[] combytes = new byte[len + 1];
                                for (int j = 0; j < len; j++)
                                {
                                    combytes[j] = bytes[off + j];
                                }
                                s = Encoding.UTF8.GetString(combytes, 0, len);
                            }
                            else if (len == 0)
                            {
                                s = "";
                            }
                            if ((commentBlock != null) && (s != null))
                            {
                                commentBlock.comment = s;
                            }
                            done = true;
                        }
                    }
                    i++;
                }
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch
                {
                }
            }
            return commentBlock;
        }

        public void Save(string text)
        {
            if (this.notEmpty)
            {
                bool hasOld = false;
                CommentBlock commentBlock = null;
                int length = 0;
                int start = 0;
                int tLen = 0;
                FileStream stream = null;
                FileStream newStream = null;
                BinaryReader reader = null;
                try
                {
                    commentBlock = this.ReadComment();
                    if ((commentBlock != null) && commentBlock.Valid())
                    {
                        hasOld = true;
                        start = commentBlock.start;
                    }
                    else
                    {
                        start = this.jpegStart();
                    }
                    byte[] commentBytes = Encoding.UTF8.GetBytes(text);
                    length = commentBytes.Length;
                    if (length > 0xffff)
                    {
                        return;
                    }
                    stream = new FileStream(this.filename, FileMode.Open, FileAccess.Read);
                    tLen = (int) stream.Length;
                    reader = new BinaryReader(stream);
                    byte[] temp = reader.ReadBytes(start);
                    int oldLen = 0;
                    if (hasOld)
                    {
                        oldLen = commentBlock.end - commentBlock.start;
                        reader.ReadBytes(oldLen);
                    }
                    byte[] imageData = reader.ReadBytes((tLen - start) - oldLen);
                    stream.Close();

                    newStream = new FileStream(this.filename, FileMode.Create, FileAccess.Write);
                    newStream.Write(temp, 0, start);
                    if (length > 0)
                    {
                        newStream.WriteByte(Convert.ToByte('\x00ff'));
                        newStream.WriteByte(Convert.ToByte('\x00fe'));
                        if (length < 0xff)
                        {
                            newStream.WriteByte(Convert.ToByte('\0'));
                            newStream.WriteByte(Convert.ToByte((int) (length + 2)));
                        }
                        else if (length <= 0xffff)
                        {
                            newStream.WriteByte(Convert.ToByte((length + 2) / 0x100));
                            newStream.WriteByte(Convert.ToByte((length + 2) - ((length + 2) / 0x100 * 0x100)));
                        }
                        newStream.Write(commentBytes, 0, length);
                    }
                    newStream.Write(imageData, 0, (tLen - start) - oldLen);
                }
                finally
                {
                    try
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if (stream != null)
                        {
                            stream.Close();
                        }
                        if (newStream != null)
                        {
                            newStream.Close();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public int jpegStart()
        {
            int i = 0;
            int r = 0;
            int length = 0;
            bool done = false;
            FileStream stream = null;
            BinaryReader reader = null;
            try
            {
                stream = new FileStream(this.filename, FileMode.Open, FileAccess.ReadWrite);
                length = (int) stream.Length;
                if (length <= 0)
                {
                    return r;
                }
                reader = new BinaryReader(stream);
                byte[] bytes = reader.ReadBytes(length);
                if (bytes.Length <= 0)
                {
                    return r;
                }
                while (!done && (i < length))
                {
                    if ((!done && (bytes[i] == 0xff)) && ((bytes[i + 1] == 0xdb) && (i < length)))
                    {
                        r = i;
                        done = true;
                    }
                    i++;
                }
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch
                {
                }
            }
            return r;
        }


        public string filename;
        private bool notEmpty;
    }
}

