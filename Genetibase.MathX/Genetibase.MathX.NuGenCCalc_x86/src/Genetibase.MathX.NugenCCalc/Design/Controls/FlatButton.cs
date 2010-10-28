using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
    [ToolboxItem(false)]
    internal class FlatButton : Button
    {
        #region Public Constructors

        public FlatButton()
        {
            base.MouseEnter += new EventHandler(this.FlatButton_MouseEnter);
            base.MouseLeave += new EventHandler(this.FlatButton_MouseLeave);
            base.GotFocus += new EventHandler(this.FlatButton_GotFocus);
            base.LostFocus += new EventHandler(this.FlatButton_LostFocus);
            this._focus = false;
            this._mouseEntered = false;
            this._highlightColor = Color.Gainsboro;
            this._hoverColor = Color.Silver;
            this._highlightHover = Color.DarkGray;
            this._internalBackColor = this.BackColor;
        }

        #endregion

        #region Private Instance Methods

        private void FlatButton_GotFocus(object sender, EventArgs e)
        {
            this._focus = true;
            if (this._mouseEntered)
            {
                _internalBackColor = this._highlightHover;
            }
            else
            {
                _internalBackColor = this._highlightColor;
            }
        }

        private void FlatButton_LostFocus(object sender, EventArgs e)
        {
            this._focus = false;
            if (this._mouseEntered)
            {
                _internalBackColor = this._hoverColor;
            }
            else
            {
                _internalBackColor = this.BackColor;
            }
        }

        private void FlatButton_MouseEnter(object sender, EventArgs e)
        {
            this._mouseEntered = true;
            if (this._focus)
            {
                _internalBackColor = this._highlightHover;
            }
            else
            {
                _internalBackColor = this._hoverColor;
            }
        }

        private void FlatButton_MouseLeave(object sender, EventArgs e)
        {
            this._mouseEntered = false;
            if (this._focus)
            {
                _internalBackColor = this._highlightColor;
            }
            else
            {
                _internalBackColor = this.BackColor;
            }
        }

        #endregion

        #region Public Instance Properties

        public Color HighlightColor
        {
            get
            {
                return this._highlightColor;
            }
            set
            {
                this._highlightColor = value;
            }
        }

        public Color HighlightHoverColor
        {
            get
            {
                return this._highlightHover;
            }
            set
            {
                this._highlightHover = value;
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
            set
            {
                this._hoverColor = value;
            }
        }

        #endregion

        #region Private Instance Fields

        private Color _internalBackColor;
        private bool _focus;
        private Color _highlightColor;
        private Color _highlightHover;
        private Color _hoverColor;
        private bool _mouseEntered;
        private bool _backColorInitialized = false;
        private bool _checked = false;

        #endregion


        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_backColorInitialized)
            {
                if (!_checked)
                    _internalBackColor = BackColor;
                else
                    _internalBackColor = _highlightColor;
                _backColorInitialized = true;
            }
            Brush brush1;
            bool singleLine = false;
            if (!this._focus & !this._mouseEntered)
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(_internalBackColor), 
                    new Rectangle(0, 0, this.Width, this.Height));
            }
            else
            {
                e.Graphics.DrawRectangle(
                    new Pen(this.ForeColor, 0f), 
                    new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                e.Graphics.FillRectangle(
                    new SolidBrush(_internalBackColor), 
                    new Rectangle(1, 1, this.Width - 2, this.Height - 2));
            }
            if (Image != null)
            {
                if (this.Enabled)
                {
                    switch (this.ImageAlign)
                    {
                        case ContentAlignment.BottomCenter:
                            e.Graphics.DrawImage(this.Image, (int)Math.Round((double)(((double)(this.Width - 32)) / 2)), (this.Height - 32) - 5, 32, 32);
                            break;
                        case ContentAlignment.BottomLeft:
                            e.Graphics.DrawImage(this.Image, 5, (this.Height - 32) - 5, 32, 32);
                            break;
                        case ContentAlignment.BottomRight:
                            e.Graphics.DrawImage(this.Image, (this.Width - 32) - 5, (this.Height - 32) - 5, 32, 32);
                            break;
                        case ContentAlignment.MiddleCenter:
                            e.Graphics.DrawImage(this.Image, (int)Math.Round((double)(((double)(this.Width - 32)) / 2)), (int)Math.Round((double)(((double)(this.Height - 32)) / 2)), 32, 32);
                            break;
                        case ContentAlignment.MiddleLeft:
                            e.Graphics.DrawImage(this.Image, 5, (int)Math.Round((double)(((double)(this.Height - 32)) / 2)), 32, 32);
                            break;
                        case ContentAlignment.MiddleRight:
                            e.Graphics.DrawImage(this.Image, (this.Width - 32) - 5, (int)Math.Round((double)(((double)(this.Height - 32)) / 2)), 32, 32);
                            break;
                        case ContentAlignment.TopCenter:
                            e.Graphics.DrawImage(this.Image, (int)Math.Round((double)(((double)(this.Width - 32)) / 2)), 5, 32, 32);
                            break;
                        case ContentAlignment.TopLeft:
                            e.Graphics.DrawImage(this.Image, 5, 5, 32, 32);
                            break;
                        case ContentAlignment.TopRight:
                            e.Graphics.DrawImage(this.Image, (this.Width - 32) - 5, 5, 32, 32);
                            break;
                        default:
                            this.ImageAlign = ContentAlignment.MiddleCenter;
                            this.OnPaint(e);
                            break;
                    }
                }
                else
                {
                    ImageAttributes attributes1 = new ImageAttributes();
                    ColorMatrix matrix1 = new ColorMatrix();
                    matrix1.Matrix00 = 0.3333333f;
                    matrix1.Matrix01 = 0.3333333f;
                    matrix1.Matrix02 = 0.3333333f;
                    matrix1.Matrix10 = 0.3333333f;
                    matrix1.Matrix11 = 0.3333333f;
                    matrix1.Matrix12 = 0.3333333f;
                    matrix1.Matrix20 = 0.3333333f;
                    matrix1.Matrix21 = 0.3333333f;
                    matrix1.Matrix22 = 0.3333333f;
                    attributes1.SetColorMatrix(matrix1);
                    switch (this.ImageAlign)
                    {
                        case ContentAlignment.TopCenter:
                            e.Graphics.DrawImage(this.Image, new Rectangle((int)Math.Round((double)(((double)(this.Width - 32)) / 2)), 5, 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel, attributes1);
                            break;
                        case ContentAlignment.MiddleCenter:
                            e.Graphics.DrawImage(this.Image, new Rectangle((int)Math.Round((double)(((double)(this.Width - 32)) / 2)), (int)Math.Round((double)(((double)(this.Height - 32)) / 2)), 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel, attributes1);
                            break;
                        default:
                            this.ImageAlign = ContentAlignment.MiddleCenter;
                            this.OnPaint(e);
                            break;
                    }
                }
            }

            if (this.Enabled)
            {
                brush1 = new SolidBrush(this.ForeColor);
            }
            else
            {
                brush1 = new SolidBrush(SystemColors.ControlDark);
            }
            if (!string.IsNullOrEmpty(this.Text))
            {
                SizeF ef1 = e.Graphics.MeasureString(this.Text, this.Font);
                if (((int)Math.Round((double)ef1.Width)) < this.Width)
                {
                    singleLine = true;
                }
                StringFormat format1 = StringFormat.GenericTypographic;
                format1.Trimming = StringTrimming.Word;
                format1.FormatFlags = StringFormatFlags.LineLimit;
                int paddingBottom = 3;
                int paddingTop = 3;
                switch (this.TextAlign)
                {
                    case ContentAlignment.BottomCenter:
                        format1.Alignment = StringAlignment.Center;
                        format1.LineAlignment = StringAlignment.Far;
                        if (singleLine)
                        {
                            paddingBottom = (int)Math.Round((double)ef1.Height);
                        }
                        break;
                    case ContentAlignment.BottomLeft:
                        format1.Alignment = StringAlignment.Near;
                        format1.LineAlignment = StringAlignment.Far;
                        if (singleLine)
                        {
                            paddingBottom = (int)Math.Round((double)ef1.Height);
                        }
                        break;
                    case ContentAlignment.BottomRight:
                        format1.Alignment = StringAlignment.Far;
                        format1.LineAlignment = StringAlignment.Far;
                        if (singleLine)
                        {
                            paddingBottom = (int)Math.Round((double)ef1.Height);
                        }
                        break;
                    case ContentAlignment.MiddleCenter:
                        format1.Alignment = StringAlignment.Center;
                        format1.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleLeft:
                        format1.Alignment = StringAlignment.Near;
                        format1.LineAlignment = StringAlignment.Center;
                        break;
                    case ContentAlignment.MiddleRight:
                        format1.Alignment = StringAlignment.Far;
                        format1.LineAlignment = StringAlignment.Center;
                        break;

                    case ContentAlignment.TopCenter:
                        format1.Alignment = StringAlignment.Center;
                        format1.LineAlignment = StringAlignment.Near;
                        if (singleLine)
                        {
                            paddingTop = (int)Math.Round((double)ef1.Height);
                        }
                        break;

                    case ContentAlignment.TopLeft:
                        format1.Alignment = StringAlignment.Near;
                        format1.LineAlignment = StringAlignment.Near;
                        if (singleLine)
                        {
                            paddingTop = (int)Math.Round((double)ef1.Height);
                        }
                        break;
                    case ContentAlignment.TopRight:
                        format1.Alignment = StringAlignment.Far;
                        format1.LineAlignment = StringAlignment.Near;
                        if (singleLine)
                        {
                            paddingTop = (int)Math.Round((double)ef1.Height);
                        }
                        break;
                }

                e.Graphics.DrawString(this.Text, this.Font, brush1, new Rectangle(3, paddingTop, this.Bounds.Width - 6, (this.Bounds.Height - paddingBottom) - paddingTop), format1);
            }
            brush1.Dispose();
        }

 


 


    }
}
