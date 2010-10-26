using Genetibase.NuGenRenderCore.Rendering;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    /// <summary>
    /// Encapsulates a peice of geometry that represents a elevation map
    /// </summary>
    interface IDEMGeometry
    {
        /// <summary>
        /// The absolute min dimension position
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The geometry dimensions relative to the Position
        /// </summary>
        Vector3 Dimensions { get; }

        /// <summary>
        /// The absolute center of the geometry
        /// </summary>
        Vector3 Center { get; }

        /// <summary>
        /// Total number of vertices that form the geometry
        /// </summary>
        ulong VertexCount { get; }

        /// <summary>
        /// Total number of primitive objects that form the geometry
        /// </summary>
        uint PrimitiveCount { get; }

        /// <summary>
        /// The type of primitives used to represent the geometry
        /// </summary>
        PrimitiveType PrimitivesType { get; }

        /// <summary>
        /// Any sub-geometry that overall geometry is composed from
        /// </summary>
        IDemSubGeometry[] SubGeometry { get; }

        void Render(GraphicsPipeline gPipe);
        void ProcessSamplers();
    }

    /// <summary>
    /// Encapsulates a quad-tree compatible 2d area of sub-geometry
    /// </summary>
    interface IDemSubGeometry
    {
        /// <summary>
        /// The absolute position the area starts
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// The relative dimensions to the Position
        /// </summary>
        Vector2 Dimensions { get; }

        /// <summary>
        /// The absolute center of the area
        /// </summary>
        Vector2 Center { get; }

        /// <summary>
        /// Total number of vertices that form the geometry
        /// </summary>
        ulong VertexCount { get; }

        /// <summary>
        /// Total number of primitive objects that form the geometry
        /// </summary>
        uint PrimitiveCount { get; }

        /// <summary>
        /// The type of primitives used to represent the geometry
        /// </summary>
        PrimitiveType PrimitivesType { get; }

        /// <summary>
        /// Any composing sub-geometry
        /// </summary>
        IDemSubGeometry[] SubGeometry { get; }
    }
}