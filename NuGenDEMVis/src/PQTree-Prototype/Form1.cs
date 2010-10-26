using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Genetibase.VisUI.Maths;
using Microsoft.DirectX;
using Matrix=Microsoft.DirectX.Matrix;

namespace PQTree_Prototype
{
    public partial class Form1 : Form
    {
        PureQuadTree qTree;
        Vector2 viewPos, viewDir, viewFOVp1, viewFOVp2;
        Plane viewFOVpl1, viewFOVpl2;
        readonly float falloff;
        Dictionary<ulong, short> leafDetail;
        Dictionary<ulong, byte> leafPatches;
        Dictionary<ulong, ulong[]> patchCodes;

        enum Directions
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        public Form1()
        {
            InitializeComponent();

            PureQuadTree tree = new PureQuadTree(new Vector2(), new Vector2(256, 256));
            tree.Fork(1, false);
            tree.Children[3].Fork(1, false);
            tree.Children[1].Fork(1, false);
            //tree.Children[3].Children[3].Fork(1, false);
            //tree.Children[1].Fork(2, false);
            ////tree.Children[2].Fork(1, false);
            //tree.Children[3].Children[1].Fork(1, false);

            //leafPatches = new Dictionary<ulong, byte>();
            //patchLevels = new Dictionary<ulong, ushort[]>();
            //foreach (PureQuadTreeNode leaf in tree)
            //{
            //    if (leaf.Level > 0)
            //    {
            //        TraceLeaf(leaf);
            //    }
            //}

            qTree = tree;// new PureQuadTree(new Vector2(), new Vector2(256, 256));
            viewPos = new Vector2(200, 300);
            viewDir = Vector2.Normalize(new Vector2(128, 128) - viewPos);
            falloff = 250;

            Vector3 viewDir3 = new Vector3(viewDir.X, 0, viewDir.Y);
            Quaternion q = Quaternion.RotationAxis(new Vector3(0, 1, 0), (float)Math.PI / 8);
            Matrix rot = Matrix.RotationQuaternion(q);
            Vector4 temp = Vector3.Transform(viewDir3, rot);
            viewFOVp1 = new Vector2(temp.X, temp.Z);
            viewFOVp1.Normalize();
            viewFOVp1 = new Vector2(viewPos.X + (viewFOVp1 * falloff).X, viewPos.Y + (viewFOVp1 * falloff).Y);

            q = Quaternion.RotationAxis(new Vector3(0, 1, 0), -(float)Math.PI / 8);
            rot = Matrix.RotationQuaternion(q);
            temp = Vector3.Transform(viewDir3, rot);
            viewFOVp2 = new Vector2(temp.X, temp.Z);
            viewFOVp2.Normalize();
            viewFOVp2 = new Vector2(viewPos.X + (viewFOVp2 * falloff).X, viewPos.Y + (viewFOVp2 * falloff).Y);

            Vector3 viewPos3 = new Vector3(viewPos.X, 0, viewPos.Y);

            q = Quaternion.RotationAxis(new Vector3(0, 1, 0), ((float)Math.PI / 8) + ((float)Math.PI / 2));
            rot = Matrix.RotationQuaternion(q);
            temp = Vector3.Transform(viewDir3, rot);
            Vector3 v3 = new Vector3(-temp.X, 0, -temp.Z);
            v3.Normalize();
            
            viewFOVpl1 = Plane.FromPointNormal(viewPos3, v3);

            q = Quaternion.RotationAxis(new Vector3(0, 1, 0), -((float)Math.PI / 8) - ((float)Math.PI / 2));
            rot = Matrix.RotationQuaternion(q);
            temp = Vector3.Transform(viewDir3, rot);
            v3 = new Vector3(-temp.X, 0, -temp.Z);
            v3.Normalize();

            viewFOVpl2 = Plane.FromPointNormal(viewPos3, v3);
                //Plane.FromPoints(viewDir3, new Vector3(viewDir3.X, 1, viewDir3.Z), new Vector3(viewDir3.X + viewFOVp2.X, 0, viewDir3.Z + viewFOVp2.Y));

            leafDetail = new Dictionary<ulong, short>();
            
            BuildTree();
        }

        private void BuildTree()
        {
            //BuildNode(qTree);
            // detect what patches are needed
            leafPatches = new Dictionary<ulong, byte>();
            patchCodes = new Dictionary<ulong, ulong[]>();
            foreach (PureQuadTreeNode leaf in qTree)
            {
                if (leaf.Level > 0)
                {
                    TraceLeaf(leaf);
                }
            }
            // now check internal patches
            foreach (PureQuadTreeNode leaf in qTree)
            {
                if (leaf.Level > 0)
                {
                    TestInternals(leaf);
                }
            }
        }

        private void TestInternals(PureQuadTreeNode leaf)
        {
            if (leaf.ChildNum == 0)
            {
                if (leaf.Parent.Children[1].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][1] = leaf.Parent.Children[1].Code;
                }
                if (leaf.Parent.Children[2].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][2] = leaf.Parent.Children[2].Code;
                }
            }
            else if (leaf.ChildNum == 1)
            {
                if (leaf.Parent.Children[0].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][0] = leaf.Parent.Children[0].Code;
                }
                if (leaf.Parent.Children[3].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][2] = leaf.Parent.Children[3].Code;
                }
            }
            else if (leaf.ChildNum == 2)
            {
                if (leaf.Parent.Children[0].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][3] = leaf.Parent.Children[0].Code;
                }
                if (leaf.Parent.Children[3].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][1] = leaf.Parent.Children[3].Code;
                }
            }
            else if (leaf.ChildNum == 3)
            {
                if (leaf.Parent.Children[2].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][0] = leaf.Parent.Children[2].Code;
                }
                if (leaf.Parent.Children[1].Children == null)
                {
                    if (!patchCodes.ContainsKey(leaf.Code))
                        patchCodes[leaf.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[leaf.Code][3] = leaf.Parent.Children[1].Code;
                }
            }
        }

        private void TraceLeaf(PureQuadTreeNode leaf)
        {
            // decide which directions to look at
            switch (leaf.ChildNum)
            {
                case 0:
                    if (TraceNode(leaf, true, 0))
                    {
                        leafPatches[leaf.Code] = (byte)Directions.Left;
                        if (TraceNode(leaf, false, 3))
                            leafPatches[leaf.Code] |= (byte)Directions.Down;
                    }
                    else if (TraceNode(leaf, false, 3))
                        leafPatches[leaf.Code] = (byte)Directions.Down;
                    break;
                case 1:
                    if (TraceNode(leaf, true, 1))
                    {
                        leafPatches[leaf.Code] = (byte)Directions.Right;
                        if (TraceNode(leaf, false, 3))
                            leafPatches[leaf.Code] |= (byte)Directions.Down;
                    }
                    else if (TraceNode(leaf, false, 3))
                        leafPatches[leaf.Code] = (byte)Directions.Down;
                    break;
                case 2:
                    if (TraceNode(leaf, true, 0))
                    {
                        leafPatches[leaf.Code] = (byte)Directions.Left;
                        if (TraceNode(leaf, false, 2))
                            leafPatches[leaf.Code] |= (byte)Directions.Up;
                    }
                    else if (TraceNode(leaf, false, 2))
                        leafPatches[leaf.Code] = (byte)Directions.Up;
                    break;
                case 3:
                    if (TraceNode(leaf, true, 1))
                    {
                        leafPatches[leaf.Code] = (byte)Directions.Right;
                        if (TraceNode(leaf, false, 2))
                            leafPatches[leaf.Code] |= (byte)Directions.Up;
                        break;
                    }
                    else if (TraceNode(leaf, false, 2))
                        leafPatches[leaf.Code] = (byte)Directions.Up;
                    break;
            }
        }

        private bool TraceNode(PureQuadTreeNode node, bool axis, int index)
        {
            PureQuadTreeNode current = node.Parent;
            // move upwards until we can switch over
            Stack<byte> upTrace = new Stack<byte>();
            upTrace.Push((byte)node.ChildNum);
            while (current != null && current.Level > 0)
            {
                bool flip = false;
                if (axis)
                {
                    if (node.ChildNum == 0 || node.ChildNum == 2)
                    {
                        if (current.ChildNum == 1 || current.ChildNum == 3)
                            flip = true;
                    }
                    else if (node.ChildNum == 1 || node.ChildNum == 3)
                    {
                        if (current.ChildNum == 0 || current.ChildNum == 2)
                            flip = true;
                    }
                }
                else
                {
                    if (node.ChildNum == 0 || node.ChildNum == 1)
                    {
                        if (current.ChildNum == 2 || current.ChildNum == 3)
                            flip = true;
                    }
                    else if (node.ChildNum == 2 || node.ChildNum == 3)
                    {
                        if (current.ChildNum == 0 || current.ChildNum == 1)
                            flip = true;
                    }
                }
                if (flip)
                {
                    // flip this and move down tree by slipping the stack until we hit and end
                    //upTrace.Push((byte)current.ChildNum);
                    int target = FlipNode(current.ChildNum, axis);
                    current = current.Parent.Children[target];
                    ushort level = current.Level;
                    ulong code = current.Code;
                    while (current != null)
                    {
                        level = current.Level;
                        code = current.Code;
                        if (level == node.Level)
                            break;
                        if (level >= node.Level)
                            return false;
                        /*if (upTrace.Count == 0)
                            break;*/

                        target = FlipNode(upTrace.Pop(), axis);
                        if (current.Children != null)
                            current = current.Children[target];
                        else
                            current = null;
                    }
                    if (!patchCodes.ContainsKey(node.Code))
                        patchCodes[node.Code] = new ulong[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue };
                    patchCodes[node.Code][index] = code;
                    return true;
                }
                upTrace.Push((byte)current.ChildNum);
                current = current.Parent;
            }
            return false;
        }

        private static int FlipNode(ushort node, bool axis)
        {
            switch (node)
            {
                case 0:
                    if (axis)
                        return 1;
                    else
                        return 2;
                case 1:
                    if (axis)
                        return 0;
                    else
                        return 3;
                case 2:
                    if (axis)
                        return 3;
                    else
                        return 0;
                case 3:
                    if (axis)
                        return 2;
                    else
                        return 1;
            }
            throw new Exception("Unknown flip params");
        }

        private void BuildNode(PureQuadTreeNode node)
        {
            float levelFalloff = falloff / node.Level;

            // first establish if visible or not by testing centre of node, then corners until visible
            bool visible = (node.Level == 0 || TestPoint(node.Centre) || TestPoint(node.Location) ||
                            TestPoint(new Vector2(node.Location.X + node.Size.X, node.Location.Y)) ||
                            TestPoint(new Vector2(node.Location.X, node.Location.Y + node.Size.Y)) ||
                            TestPoint(new Vector2(node.Location.X + node.Size.X, node.Location.Y + node.Size.Y)));

            // log what detail the leaf is to be, and if visible
            if (visible)
                leafDetail[node.Code] = (short)node.Level;
            else
                leafDetail[node.Code] = (short)-node.Level;

            if (visible)
            {
                // decide if to fork or not
                //Vector2 halfChildSize = node.Size * 0.25f;
                float distance = (node.Centre - viewPos).Length();
                // test this node first directly
                if (distance < levelFalloff)
                {
                    node.Fork(1, false);
                    for (int c = 0; c < 4; c++)
                    {
                        BuildNode(node.Children[c]);
                    }
                }
            }
            
            //else
            //{
            //    // test children directly
            //    for (int i = 0; i < 4; i++)
            //    {
            //        // check centre distance
            //        Vector2 centre = new Vector2();
            //        switch (i)
            //        {
            //            case 2:
            //                centre = new Vector2(node.Location.X + halfChildSize.X, node.Location.Y + halfChildSize.Y);
            //                break;
            //            case 3:
            //                centre = new Vector2(node.Location.X + (halfChildSize.X * 3), node.Location.Y + halfChildSize.Y);
            //                break;
            //            case 0:
            //                centre = new Vector2(node.Location.X + halfChildSize.X, node.Location.Y + (halfChildSize.Y * 3));
            //                break;
            //            case 1:
            //                centre = new Vector2(node.Location.X + (halfChildSize.X * 3), node.Location.Y + (halfChildSize.Y * 3));
            //                break;
            //        }
            //        float distance = (centre - viewPos).Length();
            //        if (distance < levelFalloff)
            //        {
            //            if (node.Children == null)
            //            {
            //                node.Fork(1, false);
            //                for (int c = 0; c < 4; c++)
            //                {
            //                    BuildNode(node.Children[c]);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            // check visibility
            //            bool visible = false;

            //            //if (distance < falloff)
            //            //{
            //            float value = viewFOVpl1.Dot(new Vector3(node.Centre.X, 0, node.Centre.Y));
            //            if (value > 0)
            //            {
            //                //if (viewFOVpl2.Dot(new Vector3(node.Centre.X, 0, node.Centre.Y)) < 0)
            //                visible = true;
            //            }
            //            //}

            //            // log what detail the leaf is to be, and if visible
            //            if (visible)
            //                leafDetail[node.Code] = (short)node.Level;
            //            else
            //                leafDetail[node.Code] = (short)-node.Level;
            //        }
            //    }
            //}
        }

        private bool TestPoint(Vector2 point)
        {
            Vector3 point3 = new Vector3(point.X, 0, point.Y);
            return (viewFOVpl1.Dot(point3) >= 0 &&
                    viewFOVpl2.Dot(point3) >= 0);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // draw tree
            g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)qTree.Location.X, (int)qTree.Location.Y),
                                        new Size((int)qTree.Size.X, (int)qTree.Size.Y)));
            DrawChildren(g, qTree);

            // draw view
            g.FillEllipse(Brushes.Blue, viewPos.X - 4, viewPos.Y - 4, 8, 8);
            g.DrawLine(Pens.Red, viewPos.X, viewPos.Y, viewPos.X + (viewDir.X * 30f), viewPos.Y + (viewDir.Y * 30f));

            g.DrawLine(Pens.Blue, new PointF(viewPos.X, viewPos.Y), new PointF(viewFOVp1.X, viewFOVp1.Y));
            g.DrawLine(Pens.Blue, new PointF(viewPos.X, viewPos.Y),new PointF(viewFOVp2.X, viewFOVp2.Y));
            g.DrawLine(Pens.Blue, new PointF(viewFOVp1.X, viewFOVp1.Y), new PointF(viewFOVp2.X, viewFOVp2.Y));
        }

        private void DrawChildren(Graphics g, PureQuadTreeNode node)
        {
            if (node.Children != null)
            {
                float halfX = node.Size.X / 2;
                float halfY = node.Size.Y / 2;
                g.DrawLine(Pens.Black, node.Location.X + halfX, node.Location.Y,
                                       node.Location.X + halfX, node.Location.Y + node.Size.Y);
                g.DrawLine(Pens.Black, node.Location.X, node.Location.Y + halfY,
                                       node.Location.X + node.Size.X, node.Location.Y + halfY);

                for (int i = 0; i < 4; i++)
                {
                    DrawChildren(g, node.Children[i]);
                }
            }
            else
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 152)))
                {
                    g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X - 1, node.Size.Y - 1);
                }
                string debug = "[" + node.Code + "]";
                if (leafPatches.ContainsKey(node.Code))
                {
                    // draw right patches
                    if ((leafPatches[node.Code] & (byte)Directions.Down) > 0)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 111, 49, 152)))
                        {
                            g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X - 1, node.Size.Y / 8);
                        }
                        if (patchCodes[node.Code][2] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + node.Size.Y - (node.Size.Y / 8), node.Size.X - 1, node.Size.Y / 8);
                            }
                        }
                        //debug += "(D=" + patchCodes[node.Code][2] + ")";
                    }
                    else if ((leafPatches[node.Code] & (byte)Directions.Up) > 0)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 111, 49, 152)))
                        {
                            g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + node.Size.Y - (node.Size.Y / 8), node.Size.X - 1, node.Size.Y / 8);
                        }
                        if (patchCodes[node.Code][3] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X - 1, node.Size.Y / 8);
                            }
                        }
                        // += "(U=" + patchCodes[node.Code][3] + ")";
                    }
                    else
                    {
                        if (patchCodes[node.Code][2] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + node.Size.Y - (node.Size.Y / 8), node.Size.X - 1, node.Size.Y / 8);
                            }
                            //debug += "(D=" + patchCodes[node.Code][2] + ")";
                        }
                        if (patchCodes[node.Code][3] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X - 1, node.Size.Y / 8);
                            }
                            //debug += "(U=" + patchCodes[node.Code][3] + ")";
                        }
                    }

                    if ((leafPatches[node.Code] & (byte)Directions.Left) > 0)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 111, 49, 152)))
                        {
                            g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                        }
                        if (patchCodes[node.Code][1] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + node.Size.X - (node.Size.X / 8), node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                            }
                        }
                    }
                    else if ((leafPatches[node.Code] & (byte)Directions.Right) > 0)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 111, 49, 152)))
                        {
                            g.FillRectangle(brush, node.Location.X + node.Size.X - (node.Size.X / 8), node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                        }
                        if (patchCodes[node.Code][0] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                            }
                        }
                    }
                    else
                    {
                        if (patchCodes[node.Code][1] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + node.Size.X - (node.Size.X / 8), node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                            }
                        }
                        if (patchCodes[node.Code][0] != ulong.MaxValue)
                        {
                            using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                            {
                                g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                            }
                        }
                    }
                    /*if (patchCodes[node.Code][0] != ulong.MaxValue)
                        debug += "L(" + patchCodes[node.Code][0] + ")";
                    if (patchCodes[node.Code][1] != ulong.MaxValue)
                        debug += "R(" + patchCodes[node.Code][1] + ")";*/
                    if (patchCodes[node.Code][2] != ulong.MaxValue)
                        debug += "D(" + patchCodes[node.Code][2] + ")";
                    if (patchCodes[node.Code][3] != ulong.MaxValue)
                        debug += "U(" + patchCodes[node.Code][3] + ")";
                    /*if (patchCodes[node.Code][1] != 14)
                        g.DrawString(patchCodes[node.Code][0] + "," + patchCodes[node.Code][1], Font, Brushes.Red, (int)node.Location.X, (int)node.Location.Y);
                    else
                        g.DrawString(patchCodes[node.Code][0].ToString(), Font, Brushes.Red, (int)node.Location.X, (int)node.Location.Y);*/
                }
                else if (patchCodes.ContainsKey(node.Code))
                {
                    // just internal patches
                    if (patchCodes[node.Code][2] != ulong.MaxValue)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                        {
                            g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + node.Size.Y - (node.Size.Y / 8), node.Size.X - 1, node.Size.Y / 8);
                        }
                    }
                    if (patchCodes[node.Code][3] != ulong.MaxValue)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                        {
                            g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X - 1, node.Size.Y / 8);
                        }
                    }
                    if (patchCodes[node.Code][1] != ulong.MaxValue)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                        {
                            g.FillRectangle(brush, node.Location.X + node.Size.X - (node.Size.X / 8), node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                        }
                    }
                    if (patchCodes[node.Code][0] != ulong.MaxValue)
                    {
                        using (Brush brush = new SolidBrush(Color.FromArgb(255, 64, 49, 96)))
                        {
                            g.FillRectangle(brush, node.Location.X + 1, node.Location.Y + 1, node.Size.X / 8, node.Size.Y - 1);
                        }
                    }
                }

                Font font = new Font("Verdana", 6);
                g.DrawString(debug, font, Brushes.Orange, (int)node.Location.X, (int)node.Location.Y);
            }
        }
    }
}