using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IAP_Core
{
    public class IAPImage : Image
    {
        private class AdaptiveHuffmanTree
        {
            private const int maxnodes = 516;
            private const string nytkey = "NYT";
            private const string eoskey = "EOS";

            private AdaptiveHuffmanTreeNode root;
            private Hashtable nodetable;

            public AdaptiveHuffmanTree()
            {
                this.root = new AdaptiveHuffmanTreeNode(AdaptiveHuffmanTreeNodeType.Internal, 0, 1, maxnodes);
                this.root.LeftChild = new AdaptiveHuffmanTreeNode(AdaptiveHuffmanTreeNodeType.NYT, 0, 0, maxnodes - 2);
                this.root.RightChild = new AdaptiveHuffmanTreeNode(AdaptiveHuffmanTreeNodeType.EndOfStream, 0, 1, maxnodes - 1);

                this.root.LeftChild.Parent = this.root;
                this.root.RightChild.Parent = this.root;

                this.nodetable = new Hashtable(maxnodes, .5f);

                this.nodetable[nytkey] = this.root.LeftChild;
                this.nodetable[eoskey] = this.root.RightChild;
            }

            public void UpdateTree(byte value)
            {
                AdaptiveHuffmanTreeNode current = GetValueNode(value);
                if (current == null) current = AddValueNode(value, 0);

                do
                {
                    AdaptiveHuffmanTreeNode highest = HighestWithSameCountFGK(current);

                    if (current != highest && current.Parent != highest)
                        SwapNodes(current, highest);

                    current.Count++;
                    current = current.Parent;

                } while (current != null);
            }

            private void SwapNodes(AdaptiveHuffmanTreeNode node1, AdaptiveHuffmanTreeNode node2)
            {
                int num = node1.Number;
                node1.Number = node2.Number;
                node2.Number = num;

                AdaptiveHuffmanTreeNode oldLeft1 = node1.Parent.LeftChild, oldLeft2 = node2.Parent.LeftChild;
                if (oldLeft1 == node1)
                    node1.Parent.LeftChild = node2;
                else
                    node1.Parent.RightChild = node2;
                if (oldLeft2 == node2)
                    node2.Parent.LeftChild = node1;
                else
                    node2.Parent.RightChild = node1;

                AdaptiveHuffmanTreeNode parent = node1.Parent;
                node1.Parent = node2.Parent;
                node2.Parent = parent;
            }

            private AdaptiveHuffmanTreeNode AddValueNode(byte value, int initialCount)
            {
                AdaptiveHuffmanTreeNode oldNYT = GetNYTNode();

                oldNYT.Type = AdaptiveHuffmanTreeNodeType.Internal;

                oldNYT.LeftChild =
                    new AdaptiveHuffmanTreeNode(AdaptiveHuffmanTreeNodeType.NYT, 0, 0, oldNYT.Number - 2);
                oldNYT.LeftChild.Parent = oldNYT;

                oldNYT.RightChild =
                    new AdaptiveHuffmanTreeNode(AdaptiveHuffmanTreeNodeType.Value, value, initialCount, oldNYT.Number - 1);
                oldNYT.RightChild.Parent = oldNYT;

                this.nodetable[nytkey] = oldNYT.LeftChild;
                this.nodetable[value] = oldNYT.RightChild;

                return oldNYT.RightChild;
            }

            private AdaptiveHuffmanTreeNode HighestWithSameCountFGK(AdaptiveHuffmanTreeNode current)
            {
                AdaptiveHuffmanTreeNode highest = current;

                if (current.Parent != null)
                {
                    AdaptiveHuffmanTreeNode parent = current.Parent;
                    
                    if (parent.LeftChild == current && parent.RightChild.Count == current.Count)
                        highest = parent.RightChild;

                    if (parent.Parent != null)
                    {
                        AdaptiveHuffmanTreeNode grandparent = parent.Parent;

                        if (grandparent.LeftChild == parent && grandparent.RightChild.Count == current.Count)
                            highest = grandparent.RightChild;
                        else if (grandparent.RightChild == parent && grandparent.LeftChild.Count == current.Count)
                            highest = grandparent.LeftChild;
                    }
                }

                return highest;
            }

            public int[] GetBitEncoding(AdaptiveHuffmanTreeNode node)
            {
                ArrayList list = new ArrayList();

                while (node.Parent != null)
                {
                    list.Add(node.Parent.LeftChild == node ? 0 : 1);
                    node = node.Parent;
                }

                list.Reverse();
                return (int[])list.ToArray(typeof(int));
            }
            public AdaptiveHuffmanTreeNode GetValueNode(byte value)
            {
                return (AdaptiveHuffmanTreeNode)nodetable[value];
            }
            public AdaptiveHuffmanTreeNode GetNYTNode()
            {
                return (AdaptiveHuffmanTreeNode)nodetable[nytkey];
            }
            public AdaptiveHuffmanTreeNode GetEOSNode()
            {
                return (AdaptiveHuffmanTreeNode)nodetable[eoskey];
            }

            public AdaptiveHuffmanTreeNode RootNode
            {
                get { return this.root; }
            }
        }

        private class AdaptiveHuffmanTreeNode
        {
            public int Number, Count;
            public byte Value;
            public AdaptiveHuffmanTreeNodeType Type;
            public AdaptiveHuffmanTreeNode Parent, LeftChild, RightChild;

            public AdaptiveHuffmanTreeNode(AdaptiveHuffmanTreeNodeType type, byte value, int count, int number)
            {
                Value = value;
                Number = number;
                Count = count;
                Type = type;
            }
        }

        private enum AdaptiveHuffmanTreeNodeType
        {
            NYT,
            EndOfStream,
            Value,
            Internal
        }

        private AdaptiveHuffmanTree tree;

        private Stream bitstream;
        private byte bitbuffer;
        private int curpos;

        public IAPImage(System.Drawing.Bitmap bitmap)
            : base(bitmap)
        {
        }
        public IAPImage(string filename)
        {
            BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open));

            // Read format

            this.Create(br.ReadInt32(), br.ReadInt32());

            // Read palette

            System.Drawing.Imaging.ColorPalette palette = this.Bitmap.Palette;

            for (int i = 0; i < palette.Entries.Length; i++)
                palette.Entries[i] = System.Drawing.Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());

            this.Bitmap.Palette = palette;

            // Read data

            byte[] bytes = new byte[br.ReadInt32()];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = br.ReadByte();

            bytes = Decompress(bytes);

            this.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            int index = 0;
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    this.SetPixel(x, bytes[index]);

                    index++;
                }

                this.IncLine();
            }

            this.CloseData();

            br.Close();
        }

        public override void Save(string filename)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create));

            // Store format

            bw.Write((int)this.Width);
            bw.Write((int)this.Height);

            // Store palette

            System.Drawing.Color[] colortable = this.Bitmap.Palette.Entries;
            for (int i = 0; i < colortable.Length; i++)
            {
                bw.Write((byte)colortable[i].R);
                bw.Write((byte)colortable[i].B);
                bw.Write((byte)colortable[i].G);
            }

            // Store data

            byte[] bytes = new byte[this.Width * this.Height];
            
            this.OpenData(System.Drawing.Imaging.ImageLockMode.ReadOnly, false);

            int index=0;
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    bytes[index] = (byte)this.GetPixel(x);
                
                    index++;
                }

                this.IncLine();
            }

            this.CloseData();

            bytes = Compress(bytes);

            bw.Write((int)bytes.Length);

            foreach (byte b in bytes)
                bw.Write((byte)b);

            bw.Close();
        }

        public void WriteBit(int bit)
        {
            bitbuffer = (byte)(bitbuffer | (bit << (7 - curpos)));

            if (++curpos == 8)
            {
                bitstream.WriteByte(bitbuffer);
                bitbuffer = 0;
                curpos = 0;
            }
        }
        private void WriteByteBits(byte value)
        {
            for (int i = 0; i < 8; i++)
            {
                int bit = ((1 << (7 - i)) & value) > 0 ? 1 : 0;

                WriteBit(bit);
            }
        }
        private void WriteByte(byte value)
        {
            AdaptiveHuffmanTreeNode node = tree.GetValueNode(value);
            
            if (node == null)
                node = tree.GetNYTNode();

            int[] bits = tree.GetBitEncoding(node);
            WriteBits(bits, 0, bits.Length);

            if (node.Type == AdaptiveHuffmanTreeNodeType.NYT)
                WriteByteBits(value);

            tree.UpdateTree(value);
        }
        private void Write(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
                WriteByte(buffer[i]);
        }
        private void WriteBits(int[] bits, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
                WriteBit(bits[i]);
        }

        private int ReadBit()
        {
            int bit, aux;

            if (curpos == 0)
            {
                if ((aux = bitstream.ReadByte()) == -1)
                {
                    bitbuffer = 0;
                    return -1;
                }
                else
                    bitbuffer = (byte)aux;
            }

            bit = ((1 << (7 - curpos)) & bitbuffer) > 0 ? 1 : 0;
            curpos = (curpos + 1) % 8;

            return bit;
        }
        private int ReadByteBits()
        {
            byte aux = 0;

            for (int i = 0; i < 8; i++)
            {
                int bit = ReadBit();

                if (bit < 0)
                    return bit;

                aux = (byte)(aux | (bit << (7 - i)));
            }

            return aux;
        }
        private int ReadByte()
        {
            AdaptiveHuffmanTreeNode node = this.tree.RootNode;

            while (node.LeftChild != null)
            {
                int bit = ReadBit();
                
                if (bit < 0)
                    return -1;

                if (bit == 0)
                    node = node.LeftChild;
                else if (bit == 1)
                    node = node.RightChild;
            }

            if (node.Type == AdaptiveHuffmanTreeNodeType.EndOfStream)
                return -1;

            int readvalue = (node.Type == AdaptiveHuffmanTreeNodeType.NYT) ? ReadByteBits() : node.Value;

            if (readvalue < 0)
                return readvalue;

            this.tree.UpdateTree((byte)readvalue);

            return readvalue;
        }
        private int Read(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++)
            {
                int readByte = ReadByte();

                if (readByte >= 0)
                    buffer[i] = (byte)readByte;
                else
                    return i - offset;
            }

            return count;
        }

        private byte[] Compress(byte[] bytes)
        {
            MemoryStream from = new MemoryStream(bytes);
            MemoryStream to = new MemoryStream();

            bitstream = to;
            bitbuffer = 0;
            curpos = 0;

            this.tree = new AdaptiveHuffmanTree();

            int read;
            byte[] buffer = new byte[2048];
            while ((read = from.Read(buffer, 0, 2048)) > 0)
                Write(buffer, 0, read);

            int[] bits = this.tree.GetBitEncoding(this.tree.GetEOSNode());
            WriteBits(bits, 0, bits.Length);

            if (curpos != 0)
                bitstream.WriteByte(bitbuffer);

            bitstream = null;
            bitbuffer = 0;
            curpos = 0;

            return to.ToArray();
        }
        private byte[] Decompress(byte[] bytes)
        {
            MemoryStream from = new MemoryStream(bytes);
            MemoryStream to = new MemoryStream();

            bitstream = from;
            bitbuffer = 0;
            curpos = 0;

            this.tree = new AdaptiveHuffmanTree();

            int read;
            byte[] buffer = new byte[2048];
            while ((read = Read(buffer, 0, 2048)) > 0)
                to.Write(buffer, 0, read);

            bitstream = null;
            bitbuffer = 0;
            curpos = 0;

            return to.ToArray();
        }
    }
}
