using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Lighting;
using System.Text.RegularExpressions;
using Microsoft.DirectX;

namespace NuGenSVisualLib
{
    public partial class DirectionalLightControl : UserControl
    {
        DirectionalLight light;

        public event EventHandler OnValueUpdate;

        public DirectionalLightControl()
        {
            InitializeComponent();
        }

        public void SetData(DirectionalLight light)
        {
            this.light = null;

            textBox1.Text = String.Format("{0}, {1}, {2}", light.Direction.X, light.Direction.Y, light.Direction.Z);

            this.light = light;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (light != null)
            {
                // try parse
                Match match = Regex.Match(textBox1.Text, @"(-\w+|\w+),\s(-\w+|\w+),\s(-\w+|\w+)");
                if (match.Success)
                {
                    try
                    {
                        float x = float.Parse(match.Groups[1].Value);
                        float y = float.Parse(match.Groups[2].Value);
                        float z = float.Parse(match.Groups[3].Value);
                        light.Direction = new Vector3(x, y, z);
                        if (OnValueUpdate != null)
                            OnValueUpdate(this, null);
                    }
                    catch { textBox1.Text = String.Format("{0}, {1}, {2}", light.Direction.X, light.Direction.Y, light.Direction.Z); }
                }
                else
                    textBox1.Text = String.Format("{0}, {1}, {2}", light.Direction.X, light.Direction.Y, light.Direction.Z);
            }
        }
    }
}
