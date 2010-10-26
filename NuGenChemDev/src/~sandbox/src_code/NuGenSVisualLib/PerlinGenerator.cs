using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.Drawing.Imaging;
using NuGenNoiseLib.LibNoise;
using Microsoft.DirectX;

namespace NuGenSVisualLib
{
    public class VolumeGenerator
    {
        public enum NoisePresets
        {
        }

        public class NoisePresetValues
        {
            public string Name;
            public int Seed;
            public double Frequency;
            public double Lacunarity;
            public int OctaveCount;
            public double Persistence;
            public PerlinModuleWrapper.NoiseQuality Quality;
            public NoisePresetValues(string name, int Seed, double Frequency, double Lacunarity,
                                     int OctaveCount, double Persistence,
                                     PerlinModuleWrapper.NoiseQuality Quality)
            {
                this.Name = name;
                this.Seed = Seed;
                this.Frequency = Frequency;
                this.Lacunarity = Lacunarity;
                this.OctaveCount = OctaveCount;
                this.Persistence = Persistence;
                this.Quality = Quality;
            }
        }

        public static readonly NoisePresetValues[] Presets = new NoisePresetValues[] {
            new NoisePresetValues("Test", -1, double.NaN, double.NaN, 1, double.NaN, (PerlinModuleWrapper.NoiseQuality)(int)-1)
        };

        public static VolumeTexture GenerateVolumeTexture(string path, string name, Device device, int width, int height, int depth, int levels,
                                                 NoisePresetValues settings, Usage usage, Pool pool)
        {
            PerlinModuleWrapper noiseModule = new PerlinModuleWrapper();

            // apply any presets
            if (settings != null)
            {
                if (settings.Seed != -1)
                    noiseModule.Seed = settings.Seed;
                if (!double.IsNaN(settings.Frequency))
                    noiseModule.Frequency = settings.Frequency;
                if (!double.IsNaN(settings.Lacunarity))
                    noiseModule.Lacunarity = settings.Lacunarity;
                if (!double.IsNaN(settings.OctaveCount))
                    noiseModule.OctaveCount = settings.OctaveCount;
                if (!double.IsNaN(settings.Persistence))
                    noiseModule.Persistence = settings.Persistence;
                if (settings.Quality != (PerlinModuleWrapper.NoiseQuality)(int)-1)
                    noiseModule.Quality = settings.Quality;
            }

            // generate slices
            // vol needs to be 32-bit stride
            //VolumeTexture volumeTex = new VolumeTexture(device, 2, 2, 2, 1, usage, Format.A8B8G8R8, pool);
            
            float sliceStep = 0;
            if (levels > 1)
                sliceStep = (float)depth / (levels - 1);

            float z = depth;
            for (int level = 0; level < levels; level++)
            {
                // sample texels for slice
                //Volume vol = volumeTex.GetVolumeLevel(level);
                Texture tex = new Texture(device, width, height, 1, Usage.None, Format.X8R8G8B8, Pool.Managed);
                //GraphicsStream lvlStream = vol.LockBox(LockFlags.None);
                GraphicsStream lvlStream = tex.LockRectangle(0, LockFlags.None);
                for (float x = 0; x < width; x++)
                {
                    for (float y = 0; y < height; y++)
                    {
                        // sample texel value
                        double value = noiseModule.GetPerlinNoiseValue(x / 3f, y / 3f, z / 3f);
                        // convert to colour data
                        // just write in direct range of -1 -> 1 as 32-bit float
                        //float valueF = (float)value;
                        //lvlStream.Write(valueF);
                        value++;
                        if (value < 0)
                            value = 0;
                        if (value > 2)
                            value = 2;
                        byte R = (byte)(value * 127f);
                        lvlStream.Write((byte)255);
                        lvlStream.Write(R);
                        lvlStream.Write(R);
                        lvlStream.Write(R);
                    }
                }
                tex.UnlockRectangle(0);
                TextureLoader.Save(path + name + level.ToString() + ".dds", ImageFileFormat.Dds, tex);
                tex.Dispose();
                //vol.UnlockBox();
                z += sliceStep;
            }

            return null;// volumeTex;
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <remarks>Based on 'JAVA REFERENCE IMPLEMENTATION OF IMPROVED NOISE - COPYRIGHT 2002 KEN PERLIN.'</remarks>
    //class PerlinGenerator
    //{
    //    static Random r = new Random();
    //    static int r1 = r.Next(1000, 10000);
    //    static int r2 = r.Next(100000, 1000000);
    //    static int r3 = r.Next(1000000000, 2000000000);

    //    public double TexelValue3D(double x, double y, double z)
    //    {
    //        double total = 0.0;
    //        double frequency = .015;    // USER ADJUSTABLE
    //        double persistence = .65;   // USER ADJUSTABLE
    //        double octaves = 8;         // USER ADJUSTABLE
    //        double amplitude = 1;       // USER ADJUSTABLE

    //        double cloudCoverage = 0;   // USER ADJUSTABLE
    //        double cloudDensity = 1;    // USER ADJUSTABLE

    //        for (int lcv = 0; lcv < octaves; lcv++)
    //        {
    //            total = total + Smooth(x * frequency, y * frequency) * amplitude;
    //            frequency = frequency * 2;
    //            amplitude = amplitude * persistence;
    //        }

    //        total = (total + cloudCoverage) * cloudDensity;

    //        //if (total < 0) total = 0.0;
    //        //if (total > 1) total = 1.0;

    //        return total;
    //    }

    //    double Smooth(double x, double y)
    //    {
    //        double n1 = Noise((int)x, (int)y);
    //        double n2 = Noise((int)x + 1, (int)y);
    //        double n3 = Noise((int)x, (int)y + 1);
    //        double n4 = Noise((int)x + 1, (int)y + 1);

    //        double i1 = Interpolate(n1, n2, x - (int)x);
    //        double i2 = Interpolate(n3, n4, x - (int)x);

    //        return Interpolate(i1, i2, y - (int)y);
    //    }

    //    double Noise(int x, int y)
    //    {
    //        int n = x + y * 57;
    //        n = (n << 13) ^ n;

    //        return (1.0 - ((n * (n * n * r1 + r2) + r3) & 0x7fffffff) / 1073741824.0);
    //    }

    //    double Interpolate(double x, double y, double a)
    //    {
    //        double val = (1 - Math.Cos(a * Math.PI)) * .5;
    //        return x * (1 - val) + y * val;
    //    }

    //    public double TexelValue(double x, double y, double z)
    //    {
    //        int X = (int)Math.Floor(x) & 255,                  // FIND UNIT CUBE THAT
    //        Y = (int)Math.Floor(y) & 255,                      // CONTAINS POINT.
    //        Z = (int)Math.Floor(z) & 255;
    //        x -= Math.Floor(x);                                // FIND RELATIVE X,Y,Z
    //        y -= Math.Floor(y);                                // OF POINT IN CUBE.
    //        z -= Math.Floor(z);
    //        double u = fade(x),                                // COMPUTE FADE CURVES
    //               v = fade(y),                                // FOR EACH OF X,Y,Z.
    //               w = fade(z);
    //        int A = p[X] + Y, AA = p[A] + Z, AB = p[A + 1] + Z,      // HASH COORDINATES OF
    //            B = p[X + 1] + Y, BA = p[B] + Z, BB = p[B + 1] + Z;      // THE 8 CUBE CORNERS,
    //        return lerp(w, lerp(v, lerp(u, grad(p[AA], x, y, z),  // AND ADD
    //                                 grad(p[BA], x - 1, y, z)), // BLENDED
    //                         lerp(u, grad(p[AB], x, y - 1, z),  // RESULTS
    //                                 grad(p[BB], x - 1, y - 1, z))),// FROM  8
    //                 lerp(v, lerp(u, grad(p[AA + 1], x, y, z - 1),  // CORNERS
    //                                 grad(p[BA + 1], x - 1, y, z - 1)), // OF CUBE
    //                         lerp(u, grad(p[AB + 1], x, y - 1, z - 1),
    //                                 grad(p[BB + 1], x - 1, y - 1, z - 1))));
    //    }

    //    static double fade(double t) { return t * t * t * (t * (t * 6 - 15) + 10); }
    //    static double lerp(double t, double a, double b) { return a + t * (b - a); }
    //    static double grad(int hash, double x, double y, double z)
    //    {
    //        int h = hash & 15; // CONVERT LO 4 BITS OF HASH CODE
    //        double u = h < 8 ? x : y, v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
    //        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);

    //        //int n = (int)(x + y * 57);
    //        //n = (n << 13) ^ n;

    //        //return (1.0 - ((n * (n * n * r1 + r2) + r3) & 0x7fffffff) / 1073741824.0);
    //    }

    //    static int[] p = new int[512];
    //    static int[] permutation = new int[] { 151,160,137,91,90,15,
    //    131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
    //    190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
    //    88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
    //    77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
    //    102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
    //    135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
    //    5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
    //    223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
    //    129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
    //    251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
    //    49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
    //    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180     };

    //    static PerlinGenerator()
    //    {
    //        {
    //            for (int i = 0; i < 256; i++)
    //                p[256 + i] = p[i] = permutation[i];
    //        }
    //    }
    //}
}