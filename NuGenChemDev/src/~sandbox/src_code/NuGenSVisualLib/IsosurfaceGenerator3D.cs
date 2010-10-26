using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace NuGenSVisualLib.Maths.Volumes
{
    public class IsosurfaceGenerator3D
    {
        public struct MovementAxis
        {
            bool N, S, W, E, U, D;
        }

        public struct Cube
        {
            public Vector3[] points;
            public float[] potentials;
            public int Layer;
            public int Slice;
            public int Index;
            public MovementAxis[] mAxis;

            public Cube(Vector3[] points, float[] potentials, int layer, int slice, int index)
            {
                this.points = points;
                this.potentials = potentials;
                this.Layer = layer;
                this.Slice = slice;
                this.Index = index;
                this.mAxis = null;
            }

            public void CalcMovementAxis()
            {

            }
        }

        class PointBind
        {
            public Vector3 Position;
            public float Potential;
            public PointBind(Vector3 position, float potential) { this.Position = position; Potential = potential; }
        }

        public static Vector3[] GenerateSimplePointField(IVolumeScene scene, Vector3 origin, float sceneCubeVolSize, int numCubes)
        {
            List<Vector3> points = new List<Vector3>();
            float spacing = sceneCubeVolSize / (float)numCubes;

            // scan for points
            float startX = origin.X - (sceneCubeVolSize / 2f);
            float startY = origin.Y - (sceneCubeVolSize / 2f);
            float startZ = origin.Z - (sceneCubeVolSize / 2f);

            float x, y, z = startZ;
            for (int zPointIdx = 0; zPointIdx < numCubes; zPointIdx++)
            {
                x = startX;
                for (int xPointIdx = 0; xPointIdx < numCubes; xPointIdx++)
                {
                    y = startY;
                    for (int yPointIdx = 0; yPointIdx < numCubes; yPointIdx++)
                    {
                        Vector3 point = new Vector3(x, y, z);
                        if (!scene.IsOutside(point))
                        {
                            points.Add(point);
                        }
                        y += spacing;
                    }
                    x += spacing;
                }
                z += spacing;
            }

            return points.ToArray();
        }

        public static void GenerateSimpleMesh(IVolumeScene scene, Vector3 origin, float sceneCubeVolSize,
                                              int numCubes, bool adaptive, out int[] triangles,
                                              out Vector3[] vertices, out Color[] colours)
        {
            // list all cubes / points in volume space
            PointBind[] points = new PointBind[numCubes * numCubes * numCubes];
            float spacing = sceneCubeVolSize / (float)numCubes;

            // scan for points
            float startX = origin.X - (sceneCubeVolSize / 2f);
            float startY = origin.Y - (sceneCubeVolSize / 2f);
            float startZ = origin.Z - (sceneCubeVolSize / 2f);

            float x, y, z = startZ;
            int index = 0;
            for (int zPointIdx = 0; zPointIdx < numCubes; zPointIdx++)
            {
                x = startX;
                for (int xPointIdx = 0; xPointIdx < numCubes; xPointIdx++)
                {
                    y = startY;
                    for (int yPointIdx = 0; yPointIdx < numCubes; yPointIdx++)
                    {
                        Vector3 point = new Vector3(x, y, z);
                        float potential = scene.GetPotentialAtPoint(point);
                        //if (potential > 0.1f)
                            points[index] = new PointBind(point, potential);

                        y += spacing;
                        index++;
                    }
                    x += spacing;
                }
                z += spacing;
            }

            // map cubes
            index = 0;
            List<Cube> cubes = new List<Cube>();
            Vector3[] cPoints = new Vector3[8];
            float[] cPotentials = new float[8];
            int[] indices = new int[8];
            int layerSz = numCubes * numCubes;

            for (int zPointIdx = 0; zPointIdx < numCubes - 1; zPointIdx++)
            {
                for (int xPointIdx = 0; xPointIdx < numCubes - 1; xPointIdx++)
                {
                    for (int yPointIdx = 0; yPointIdx < numCubes - 1; yPointIdx++)
                    {
                        // create cube
                        indices[0] = index;
                        indices[1] = index + 1;
                        indices[2] = index + numCubes + 1;
                        indices[3] = index + numCubes;

                        indices[4] = index + layerSz;
                        indices[5] = index + layerSz + 1;
                        indices[6] = index + layerSz + 1 + numCubes;
                        indices[7] = index + layerSz + numCubes;

                        int numPoints = 0;
                        int numVPoints = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            PointBind value = points[indices[i]];
                            //if (points.TryGetValue(indices[i], out value))
                            //{
                                cPoints[i] = value.Position;
                                cPotentials[i] = value.Potential;
                                numPoints++;

                                if (value.Potential >= 0.1f)
                                    numVPoints++;
                            //}
                            //else
                            //{
                            //    cPotentials[i] = float.NaN;
                            //}
                        }

                        if (numPoints > 0 && numVPoints > 0)
                        {
                            cubes.Add(new Cube(cPoints, cPotentials, zPointIdx, xPointIdx, yPointIdx));
                            cPotentials = new float[8];
                            cPoints = new Vector3[8];
                            // TODO: Switch to indices

                            // calc normals/directions they can move for each point in cube
                            // based on side-plane matching/analysis

                        }
                        index++;
                    }
                    index++; // push on to next line
                }
                index += numCubes;
            }

            Cube[] cubeArray = cubes.ToArray();
            // modify cubes for adaptive points
            if (adaptive)
                ApplyAdaptiveProcess(scene, cubeArray, 1, sceneCubeVolSize);

            // transform into triangles
            MarchingCubes.PolygoniseCubes(cubeArray, 0.1f, (sceneCubeVolSize / (float)numCubes) / 2f, out triangles, out vertices);

            // colourize all vertices
            colours = new Color[vertices.Length];
            for (int vert = 0; vert < vertices.Length; vert++)
            {
                colours[vert] = scene.ColourizePoint(vertices[vert]);
            }
        }

        /// <summary>
        /// PROCESS:
        /// Collect points
        /// Generate cubes
        /// generate cube normals
        /// apply adaptive using cueb normals
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="cubes"></param>
        /// <param name="passes"></param>
        /// <param name="cubeSize"></param>
        private static void ApplyAdaptiveProcess(IVolumeScene scene, Cube[] cubes, int passes, float cubeSize)
        {
            int[] points = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            foreach (Cube cube in cubes)
            {
                // TODO: work out which points we need to re-test

                for (int point = 0; point < points.Length; point++)
                {
                    // run step passes on each axis
                    float step = cubeSize / 2f;
                    bool xHit = true, yHit = true, zHit = true;
                    int xLastHit = -1, yLastHit = -1, zLastHit = -1;
                    float xLastHitPot, yLastHitPot, zLastHitPot;
                    float xLastHitPos = 0;
                    Vector3 testPos = new Vector3();
                    for (int pass = 0; pass < passes; pass++)
                    {
                        // decide where step takes us
                        if (xHit)
                            testPos.X += step;
                        else
                            testPos.X -= step;
                        if (yHit)
                            testPos.Y += step;
                        else
                            testPos.Y -= step;
                        if (zHit)
                            testPos.Z += step;
                        else
                            testPos.Z -= step;

                        // test step on points
                        float pot = scene.GetPotentialAtPoint(cube.points[points[point]] + new Vector3(testPos.X, 0, 0));
                        if (xHit = (pot > 0.1f))
                        {
                            xLastHit = pass;
                            xLastHitPot = pot;
                            xLastHitPos = testPos.X;
                        }

                        //pot = scene.GetPotentialAtPoint(cube.points[points[point]] + new Vector3(0, testPos.Y, 0));
                        //if (yHit = (pot > 0.1f))
                        //{
                        //    yLastHit = pass;
                        //    yLastHitPot = pot;
                        //}

                        //pot = scene.GetPotentialAtPoint(cube.points[points[point]] + new Vector3(0, 0, testPos.Z));
                        //if (zHit = (pot > 0.1f))
                        //{
                        //    zLastHit = pass;
                        //    zLastHitPot = pot;
                        //}

                        step /= 2f;
                    }

                    // choose last hit as final position
                    if (xLastHit != -1)
                    {
                        cube.points[points[point]].X += xLastHitPos;
                    }
                }
            }
        }

        public static void GenerateSimplePointOutline(GenericVolumeScene scene, Vector3 origin, float sceneCubeVolSize, int numCubes, out Vector3[] vertices, out Color[] colours)
        {
            // list all cubes / points in volume space
            PointBind[] points = new PointBind[numCubes * numCubes * numCubes];
            float spacing = sceneCubeVolSize / (float)numCubes;

            // scan for points
            float startX = origin.X - (sceneCubeVolSize / 2f);
            float startY = origin.Y - (sceneCubeVolSize / 2f);
            float startZ = origin.Z - (sceneCubeVolSize / 2f);

            float x, y, z = startZ;
            int index = 0;
            for (int zPointIdx = 0; zPointIdx < numCubes; zPointIdx++)
            {
                x = startX;
                for (int xPointIdx = 0; xPointIdx < numCubes; xPointIdx++)
                {
                    y = startY;
                    for (int yPointIdx = 0; yPointIdx < numCubes; yPointIdx++)
                    {
                        Vector3 point = new Vector3(x, y, z);
                        float potential = scene.GetPotentialAtPoint(point);
                        //if (potential > 0.1f)
                        points[index] = new PointBind(point, potential);

                        y += spacing;
                        index++;
                    }
                    x += spacing;
                }
                z += spacing;
            }

            // map cubes
            index = 0;
            List<Cube> cubes = new List<Cube>();
            Vector3[] cPoints = new Vector3[8];
            float[] cPotentials = new float[8];
            int[] indices = new int[8];
            int layerSz = numCubes * numCubes;

            for (int zPointIdx = 0; zPointIdx < numCubes - 1; zPointIdx++)
            {
                for (int xPointIdx = 0; xPointIdx < numCubes - 1; xPointIdx++)
                {
                    for (int yPointIdx = 0; yPointIdx < numCubes - 1; yPointIdx++)
                    {
                        // create cube
                        indices[0] = index;
                        indices[1] = index + 1;
                        indices[2] = index + numCubes + 1;
                        indices[3] = index + numCubes;

                        indices[4] = index + layerSz;
                        indices[5] = index + layerSz + 1;
                        indices[6] = index + layerSz + 1 + numCubes;
                        indices[7] = index + layerSz + numCubes;

                        int numPoints = 0;
                        int numVPoints = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            PointBind value = points[indices[i]];
                            //if (points.TryGetValue(indices[i], out value))
                            //{
                            cPoints[i] = value.Position;
                            cPotentials[i] = value.Potential;
                            numPoints++;

                            if (value.Potential >= 0.1f)
                                numVPoints++;
                            //}
                            //else
                            //{
                            //    cPotentials[i] = float.NaN;
                            //}
                        }

                        if (numPoints > 0 && numVPoints > 0)
                        {
                            cubes.Add(new Cube(cPoints, cPotentials, zPointIdx, xPointIdx, yPointIdx));
                            cPotentials = new float[8];
                            cPoints = new Vector3[8];
                            // TODO: Switch to indices

                            // calc normals/directions they can move for each point in cube
                            // based on side-plane matching/analysis

                        }
                        index++;
                    }
                    index++; // push on to next line
                }
                index += numCubes;
            }

            Cube[] cubeArray = cubes.ToArray();
            // modify cubes for adaptive points
            //if (adaptive)
            //    ApplyAdaptiveProcess(scene, cubeArray, 1, sceneCubeVolSize);

            // transform into triangles
            MarchingCubes.PointsCubes(cubeArray, 0.1f, (sceneCubeVolSize / (float)numCubes) / 2f, out vertices);

            // colourize all vertices
            colours = new Color[vertices.Length];
            for (int vert = 0; vert < vertices.Length; vert++)
            {
                colours[vert] = scene.ColourizePoint(vertices[vert]);
            }
        }
    }
}