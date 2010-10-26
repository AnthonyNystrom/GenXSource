using System;
using System.Collections.Generic;
using System.Text;
using NuGenSVisualLib.Rendering.Effects;
using Org.OpenScience.CDK.Interfaces;
using NuGenSVisualLib.Rendering.Chem;
using Microsoft.DirectX.Direct3D;
using NuGenSVisualLib.Rendering;

namespace NuGenSVisualLib
{
    public interface IGeometryCreator
    {
        DataFields[] Fields { get; }
    }

    /// <summary>
    /// Generic class to allow the creation of custom spec geometry from objects
    /// </summary>
    /// <typeparam name="T">The type of objects that can be used to create geometry from</typeparam>
    public abstract class GeometryCreator<T> : IGeometryCreator
    {
        protected DataFields[] availableFields;
        protected DataFields[] fields;

        public DataFields[] AvailableFields
        {
            get { return availableFields; }
        }

        public DataFields[] Fields
        {
            get { return fields; }
        }

        public virtual void SetupForCreation(int[] reqFields)
        {
            if (reqFields == null)
            {
                fields = availableFields;
            }
            else
            {
                fields = new DataFields[reqFields.Length];
                for (int i = 0; i < reqFields.Length; i++)
                {
                    fields[i] = availableFields[reqFields[i]];
                }
            }
        }
        public abstract void ClearCache();

        //public abstract void CreateGeometryForObject(Device device, T obj, GeomDataBufferStream buffer, int stream);
        public abstract void CreateGeometryForObjects(Device device, ICollection<T> objs, GeomDataBufferStream geomStream,
                                                      int stream, ref BufferedGeometryData buffer,
                                                      CompleteOutputDescription coDesc);
    }

    public abstract class AtomGeometryCreator : GeometryCreator<IAtom>
    {
        //public override abstract void CreateGeometryForObject(Device device, IAtom obj, GeomDataBufferStream buffer, int stream);
        public override abstract void CreateGeometryForObjects(Device device, ICollection<IAtom> objs,
                                                               GeomDataBufferStream geomStream, int stream,
                                                               ref BufferedGeometryData buffer,
                                                               CompleteOutputDescription coDesc);
    }

    public abstract class BondGeometryCreator : GeometryCreator<IBond>
    {
        public override abstract void CreateGeometryForObjects(Device device, ICollection<IBond> objs,
                                                               GeomDataBufferStream geomStream, int stream,
                                                               ref BufferedGeometryData buffer,
                                                               CompleteOutputDescription coDesc);
    }
}