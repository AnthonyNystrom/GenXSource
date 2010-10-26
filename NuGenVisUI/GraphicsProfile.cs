using Genetibase.NuGenRenderCore.Rendering.Devices;

namespace Genetibase.VisUI.Rendering
{
    public class GraphicsProfile
    {
        readonly string name;
        readonly string desc;

        protected readonly GraphicsDeviceRequirements minReqs;
        protected readonly GraphicsDeviceRequirements[] recommendedVars;
        protected int recommendedVarInUse;

        /// <summary>
        /// Initializes a new instance of the GraphicsProfile class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="minReqs"></param>
        /// <param name="recommendedVars"></param>
        public GraphicsProfile(string name, string desc, GraphicsDeviceRequirements minReqs,
                               GraphicsDeviceRequirements[] recommendedVars)
        {
            this.name = name;
            this.desc = desc;
            this.minReqs = minReqs;
            this.recommendedVars = recommendedVars;
        }

        /// <summary>
        /// Initializes a new instance of the GraphicsProfile class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <param name="minReqs"></param>
        /// <param name="recommendedVar"></param>
        public GraphicsProfile(string name, string desc, GraphicsDeviceRequirements minReqs,
                               GraphicsDeviceRequirements recommendedVar)
        {
            this.name = name;
            this.desc = desc;
            this.minReqs = minReqs;
            recommendedVars = new GraphicsDeviceRequirements[] { recommendedVar };
        }

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public string Desc
        {
            get { return desc; }
        }

        public GraphicsDeviceRequirements MinReqs
        {
            get { return minReqs; }
        }

        public GraphicsDeviceRequirements[] RecommendedVariations
        {
            get { return recommendedVars; }
        }

        public GraphicsDeviceRequirements RecommendedVariation
        {
            get { return recommendedVarInUse == -1 ? minReqs : recommendedVars[recommendedVarInUse]; }
        }

        public int RecommendedVarInUse
        {
            get { return recommendedVarInUse; }
            set { recommendedVarInUse = value; }
        }
        #endregion
    }
}