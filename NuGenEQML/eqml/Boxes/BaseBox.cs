namespace Boxes
{
    using Rendering;
    using Nodes;
    using System;
    using System.Drawing;

    public enum PaintMode
    {
        FOREGROUND,
        BACKGROUND
    }

    public class BoxRect
    {
        public BoxRect()
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
            this.baseline = 0;
            this.accent = 0;
            this.dx = 0;
            this.dy = 0;
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
            this.baseline = 0;
        }

        public BoxRect(int W, int H, int B)
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
            this.baseline = 0;
            this.accent = 0;
            this.dx = 0;
            this.dy = 0;
            this.x = 0;
            this.y = 0;
            this.width = W;
            this.height = H;
            this.baseline = B;
        }

        public int x;
        public int y;
        public int width;
        public int height;
        public int baseline;
        public int accent;
        public int dx;
        public int dy;
    }

    public interface IBox
    {
        void UpdateChildPosition(Node childNode);
        void getSize(Node node);
        
        void Draw(Node node, PaintMode printMode, Color color);

        void setChildSize(Node childNode);
        
        void SetPosition(Node node);
        
        int X { get; set; }
        int Y { get; set; }

        int Width { get; set; }
        int Height { get; set; }

        int Baseline { get; set; }

        int Dx { get; set; }
        int Dy { get; set; }

        Painter Painter { set; }
    }

    public class BaseBox : IBox
    {
        public BaseBox()
        {
            this.background_ = Color.White;
            this.foreground_ = Color.Black;
        }

        public int Baseline
        {
            get { return this.rect.baseline; }
            set { this.rect.baseline = value; }
        }

        public int Dx
        {
            get { return this.rect.dx; }
            set { this.rect.dx = value; }
        }

        public int Dy
        {
            get { return this.rect.dy; }
            set { this.rect.dy = value; }
        }

        public virtual void UpdateChildPosition(Node childNode)
        {
        }

        public virtual void getSize(Node node)
        {
        }

        public int Height
        {
            get { return this.rect.height; }
            set { this.rect.height = value; }
        }

        public virtual void Draw(Node node, PaintMode printMode, Color color)
        {
        }

        public virtual void setChildSize(Node childNode)
        {
        }

        public Painter Painter
        {
            set { this.painter_ = value; }
        }

        public virtual void SetPosition(Node node)
        {
            this.rect.x = node.box.X;
            this.rect.y = node.box.Y;
        }

        public int Width
        {
            get { return this.rect.width; }
            set { this.rect.width = value; }
        }

        public int X
        {
            get { return this.rect.x; }
            set { this.rect.x = value; }
        }

        protected int max(int num1, int num2)
        {
            if ((num1 < num2) && (num2 >= num1))
            {
                return num2;
            }
            return num1;
        }

        protected int max(int num1, int num2, int num3)
        {
            if ((num1 < num2) || (num1 < num3))
            {
                if ((num2 >= num1) && (num2 >= num3))
                {
                    return num2;
                }
                if ((num3 >= num1) && (num3 >= num1))
                {
                    return num3;
                }
            }
            return num1;
        }

        protected void Skip(Node node, bool skip)
        {
            if (((node.type_.type == ElementType.Mrow) || (node.type_.type == ElementType.Mtd)) || (node.type_.type == ElementType.Mtr))
            {
                node.skip = skip;
            }
        }

        public int Y
        {
            get { return this.rect.y; }
            set { this.rect.y = value; }
        }


        public Painter painter_;
        public BoxRect rect;
        public Color background_;
        public Color foreground_;
    }
}

