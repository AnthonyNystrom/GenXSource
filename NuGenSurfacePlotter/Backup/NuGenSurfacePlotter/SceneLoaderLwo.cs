using System;
using System.Collections.Generic;
using System.Text;
using NuGenCRBase.AvalonBridge;
using Microsoft.DirectX;

namespace NuGenCRBase.SceneFormats.Lwo
{
    class SceneLoaderLwo : IABSceneLoader
    {
        #region IABSceneLoader Members

        public ABScene3D LoadScene(string file)
        {
            uint failId;
            int failPos;
            lwObject lwobj = lwObject.GetObject5(file, out failId, out failPos);
            if (lwobj != null)
            {
                // convert to AB
                ABScene3D scene = new ABScene3D();

                ABMaterial[] materials = null;
                int[] clipIndices = null;
                if (lwobj.surf != null && lwobj.surf.Count > 0)
                {
                    materials = new ABMaterial[lwobj.surf.Count];
                    clipIndices = new int[lwobj.surf.Count];
                    LinkedList<lwSurface>.Enumerator matIter = lwobj.surf.GetEnumerator();
                    int matIdx = 0;
                    while (matIter.MoveNext())
                    {
                        lwSurface surface = matIter.Current;
                        ABMaterial mat = materials[matIdx] = new ABMaterial();

                        mat.Name = surface.name;
                        mat.Ambient = new ABColorARGB();
                        mat.Ambient.A = 0xFF;
                        mat.Ambient.R = (byte)(surface.color.rgb[0] / 255f);
                        mat.Ambient.G = (byte)(surface.color.rgb[1] / 255f);
                        mat.Ambient.B = (byte)(surface.color.rgb[2] / 255f);
                        mat.Diffuse = new ABColorARGB();
                        mat.Diffuse.A = 0xFF;
                        mat.Diffuse.R = (byte)((surface.diffuse.val * surface.color.rgb[0]) / 255f);
                        mat.Diffuse.G = (byte)((surface.diffuse.val * surface.color.rgb[1]) / 255f);
                        mat.Diffuse.B = (byte)((surface.diffuse.val * surface.color.rgb[2]) / 255f);

                        if (surface.color.tex != null)
                        {
                            // resolve texture
                            //mat.TextureName
                            clipIndices[matIdx] = surface.color.tex.First.Value.imap.cindex;
                            mat.TextureName = surface.color.tex.First.Value.imap.vmap_name;
                        }
                        else
                            clipIndices[matIdx] = -1;

                        matIdx++;
                    }
                }

                LinkedList<lwLayer>.Enumerator layIter = lwobj.layer.GetEnumerator();
                scene.Models = new ABModel3D[lwobj.layer.Count];
                int mdlIdx = 0;
                while (layIter.MoveNext())
                {
                    lwLayer layer = layIter.Current;
                    ABModel3D model = scene.Models[mdlIdx] = new ABModel3D();

                    model.Geometry = new ABGeometry3D();
                    model.Geometry.Vertices = new Vector3[layer.point.count];

                    // break up polygons by surface
                    Dictionary<lwSurface, int> surfGroupCounts = new Dictionary<lwSurface, int>();
                    for (int i = 0; i < layer.polygon.pol.Length; i++)
                    {
                        lwPolygon polygon = layer.polygon.pol[i];
                        if (surfGroupCounts.ContainsKey(polygon.surf))
                            surfGroupCounts[polygon.surf]++;
                        else
                            surfGroupCounts[polygon.surf] = 1;
                    }
                    int[] groupIndices = new int[surfGroupCounts.Count];
                    model.MaterialIndices = new ABMaterialIndex[surfGroupCounts.Count];
                    for (int i = 0; i < layer.polygon.pol.Length; i++)
                    {
                        lwPolygon polygon = layer.polygon.pol[i];
                        if (surfGroupCounts.ContainsKey(polygon.surf))
                            surfGroupCounts[polygon.surf]++;
                        else
                            surfGroupCounts[polygon.surf] = 1;
                    }
                }

                // match up clips to materials

            }
            return null;
        }

        #endregion
    }
}