using System.Drawing;
using Microsoft.DirectX;

namespace Genetibase.NuGenRenderCore.View
{
    /// <summary>
    /// Encapsulates a view projection in a scene
    /// </summary>
    public interface ICamera
    {
        Matrix Projection { get; }
        Matrix View { get; }
        Matrix World { get; }
        Size ViewSize { get; set; }
        IViewInputHandler DefaultHandler { get; }

        void Update(bool updProj, bool updView, bool updWorld);
        // TODO: View volume input needed
        bool UsePerspectiveProj { get; set; }
    }

    /// <summary>
    /// Encapsulates a mechanism for transfering view input
    /// into camera manipulation
    /// </summary>
    public interface IViewInputHandler
    {
        void MoveMouse(Point point);
        void MouseWheelScroll(float scroll);

        Size ViewSize { get; set; }
        ICamera Camera { get; }
    }

    public abstract class Camera : ICamera
    {
        protected Matrix proj, view, world;
        protected Size viewSize;
        protected bool usePerspective;
        protected float fov;

        protected Vector3 position;

        public Camera(Size viewSize, Vector3 position, float fov, bool usePer)
        {
            ViewSize = viewSize;
            Position = position;
            this.fov = fov;
            usePerspective = usePer;
        }

        #region Properties

        public Vector3 Position
        {
            get { return position; }
            set { position = value; Update(false, true, true); }
        }
        #endregion

        protected virtual void CalcProjMat()
        {
            if (usePerspective)
            {
                if (fov > 0)
                    proj = Matrix.PerspectiveFovLH(fov, (float)viewSize.Width / viewSize.Height, 10f, 5000f);
                else
                    proj = Matrix.PerspectiveLH(viewSize.Width, viewSize.Height, 10f, 5000f);
            }
        }

        #region ICamera Members

        public Matrix Projection
        {
            get { return proj; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix World
        {
            get { return world; }
        }

        public Size ViewSize
        {
            get { return viewSize; }
            set { viewSize = value; CalcProjMat(); }
        }

        public abstract IViewInputHandler DefaultHandler
        {
            get;
        }

        public abstract void Update(bool updProj, bool updView, bool updWorld);

        public bool UsePerspectiveProj
        {
            get { return usePerspective; }
            set { usePerspective = value; }
        }
        #endregion
    }

    public abstract class ViewInputHandler : IViewInputHandler
    {
        protected Point currentMouseMove;
        protected Size viewSize;
        protected readonly ICamera camera;

        /// <summary>
        /// Initializes a new instance of the ViewInputHandler class.
        /// </summary>
        /// <param name="viewSize"></param>
        /// <param name="camera"></param>
        public ViewInputHandler(Size viewSize, ICamera camera)
        {
            currentMouseMove = new Point();
            this.viewSize = viewSize;
            this.camera = camera;
        }

        #region IViewInputHandler Members

        public abstract void MoveMouse(Point point);
        public abstract void MouseWheelScroll(float scroll);

        public Size ViewSize
        {
            get { return viewSize; }
            set { viewSize = value; }
        }

        public ICamera Camera
        {
            get { return camera; }
        }
        #endregion
    }
}