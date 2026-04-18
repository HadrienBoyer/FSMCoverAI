namespace TappyTale.PostNavCoverFSM.Runtime.Cover
{
    /// <summary>
    /// Defines the type of cover available. This enum is internal to the AI cover system
    /// and is distinct from any cover type defined by external packages such as the Post Navigation System.
    /// </summary>
    public enum AiCoverType
    {
        /// <summary>
        /// Default unknown cover type.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Represents low cover where the agent must crouch.
        /// </summary>
        Low = 1,
        /// <summary>
        /// Represents high cover that fully conceals the agent.
        /// </summary>
        High = 2
    }
}