using System;
using System.Drawing;

namespace NuGenRenderOptics
{
    struct RGBA_D
    {
        public double R, G, B, A;

        public RGBA_D(double r, double g, double b, double a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static RGBA_D Empty
        {
            get { return new RGBA_D(0, 0, 0, 0); }
        }

        public void Normalize()
        {
            R = Math.Min(R, 255);
            G = Math.Min(G, 255);
            B = Math.Min(B, 255);
            A = Math.Min(A, 255);
            R = Math.Max(R, 0);
            G = Math.Max(G, 0);
            B = Math.Max(B, 0);
            A = Math.Max(A, 0);
        }

        public static bool operator !=(RGBA_D left, RGBA_D right)
        {
            return (left.R != right.R || left.G != right.G || left.B != right.B || left.A != right.A);
        }

        public static bool operator ==(RGBA_D left, RGBA_D right)
        {
            return (left.R == right.R && left.G == right.G && left.B == right.B && left.A == right.A);
        }

        public static RGBA_D operator *(RGBA_D left, RGBA_D right)
        {
            return new RGBA_D(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);
        }

        public static RGBA_D operator *(RGBA_D left, Vector3D right)
        {
            return new RGBA_D(left.R * right.X, left.G * right.Y, left.B * right.Z, left.A);
        }

        public static RGBA_D operator *(RGBA_D left, double right)
        {
            return new RGBA_D(left.R * right, left.G * right, left.B * right, left.A);
        }
    }

    class MaterialShader
    {
        RGBA_D clr;
        double shininess, reflection, transmission;
        double emmissive, diffuse;

        Image texture;

        /// <summary>
        /// BSDF material.
        /// Bidirectional scattering distribution function
        /// http://en.wikipedia.org/wiki/Bidirectional_scattering_distribution_function
        /// </summary>
        /// <param name="clr">The colour the material diffuses light into</param>
        /// <param name="shininess">How "evenly" light is reflected from a 'shiny' spot</param>
        /// <param name="reflection">How much of the light gets scattered in the incident direction i.e. reflected with no attenuation</param>
        /// <param name="transmission">How much of the light gets transmitted into the material</param>
        /// <param name="emmissive">How much light is emitted from the material, indipendantly of a light source</param>
        /// <param name="diffuse">How much light is scattered by the surface, i.e. opposite of how much is absorbed</param>
        /// <remarks>
        /// Refraction = Intensity *  Transmission
        /// Reflection + Transmission == <= 1 && >= 0
        /// </remarks>
        public MaterialShader(RGBA_D clr, double shininess, double reflection,
                              double transmission, double emmissive, double diffuse)
        {
            this.clr = clr;
            this.shininess = shininess;
            this.reflection = reflection;
            this.transmission = transmission;
            this.emmissive = emmissive;
            this.diffuse = diffuse;
        }

        public MaterialShader(RGBA_D clr, double shininess, double reflection,
                              double transmission, double emmissive, double diffuse,
                              Image texture)
        {
            this.clr = clr;
            this.shininess = shininess;
            this.reflection = reflection;
            this.transmission = transmission;
            this.emmissive = emmissive;
            this.diffuse = diffuse;
            this.texture = texture;
        }

        #region Properties

        public double Diffuse
        {
            get { return diffuse; }
        }

        public double Shininess
        {
            get { return shininess; }
        }

        public double Transmission
        {
            get { return transmission; }
        }
        #endregion

        public RGBA_D Shade(Ray ray, Vector3D hitPoint, uint subIdx, IOpticalSceneObject obj,
                            ISceneManager scene, out Ray reflection, out Ray refraction)
        {
            RGBA_D color = RGBA_D.Empty;

            // needed?
            // normal.Normalize();
            
            Vector3D normal = obj.GetNormal(hitPoint, subIdx);

            //color.R = normal.X * 255;
            //color.G = normal.Y * 255;
            //color.B = normal.Z * 255;
            //color.A = 255;
            //refraction = null;
            //reflection = null;
            //return color;

            /*double len = (ray.Origin - hitPoint).Length();
            len -= 2;
            color.R = color.G = color.B = len * 42.5;*/

            foreach (Light light in scene.Lights)
            {
                Vector3D lv = light.Position - hitPoint;
                lv.Normalize();

                // deal with light ray first (diffuse)
                if (true)//ray.TraceRayToLight(hitPoint, light.Position))
                {
                    // light pixel
                    double cost = Vector3D.GetCosAngle(lv, normal);
                    Vector3D vRefl = Vector3D.Reflect(-lv, normal);
                    vRefl.Normalize();

                    double cosf = Vector3D.GetCosAngle(ray.DirectionUV, vRefl);
                    double result1 = Math.Max(0, cost) * 255;
                    double result2 = Math.Pow(Math.Max(0, cosf), shininess) * 255;

                    double luminosity = light.LuminosityForPoint(hitPoint);

                    double r = ((clr.R * diffuse * light.Clr3D.X * result1) +
                                (light.Clr3D.X * result2)) * luminosity;
                    double g = ((clr.G * diffuse * light.Clr3D.Y * result1) +
                                (light.Clr3D.Y * result2)) * luminosity;
                    double b = ((clr.B * diffuse * light.Clr3D.Z * result1) +
                                (light.Clr3D.Z * result2)) * luminosity;

                    color.R += r;
                    color.G += g;
                    color.B += b;
                }
            }
            
            // add ambient
            double alpha = 1 - transmission;
            color.R += (diffuse * scene.Ambient.R + (clr.R * emmissive)) * 255;
            //color.R *= alpha;
            color.G += (diffuse * scene.Ambient.G + (clr.G * emmissive)) * 255;
            //color.G *= alpha;
            color.B += (diffuse * scene.Ambient.B + (clr.B * emmissive)) * 255;
            //color.B *= alpha;
            
            color.A = alpha * 255;

            // blend texture (if any)
            /*if (texture != null)
            {
                Vector2D tCoord = obj.GetTexCoord(hitPoint, subIdx);
                // clamp for now
                if (tCoord.X < 0)
                    tCoord.X = 0;
                if (tCoord.Y < 0)
                    tCoord.Y = 0;
                if (tCoord.X > 1)
                    tCoord.X = 1;
                if (tCoord.Y > 1)
                    tCoord.Y = 1;

                int tX = (int)(tCoord.X * (texture.Width - 1));
                int tY = (int)(tCoord.Y * (texture.Height - 1));

                Color tClr = ((Bitmap)texture).GetPixel(tX, tY);
                color.R = (color.R + tClr.R) / 2;
                color.G = (color.G + tClr.G) / 2;
                color.B = (color.B + tClr.B) / 2;
            }*/

            if (ray.Intensity > 0)
            {
                /*if (this.reflection > 0)
                {
                    Vector3D refl = Vector3D.Reflect(ray.DirectionUV, normal);
                    reflection = new Ray(hitPoint, refl, ray.Intensity * this.reflection, ray.Length, ray.MaxLength, ray.scene);
                }
                else*/
                    reflection = null;
                /*if (transmission > 0)
                    refraction = new Ray(hitPoint, Vector3D.Normalize(Vector3D.Refract(1, 1.33, -ray.DirectionUV, normal)), ray.Intensity * transmission, ray.Length, ray.MaxLength, ray.scene);
                else*/
                refraction = null;
            }
            else
                reflection = refraction = null;

            ray.Intensity = 0;

            return color;
        }
    }
}