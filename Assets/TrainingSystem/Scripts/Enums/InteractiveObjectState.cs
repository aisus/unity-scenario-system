namespace TrainingSystem.Scripts.Enums
{
    /// <summary>
    /// Possible interactive object states
    /// </summary>
    public enum InteractiveObjectState
    {
        /// <summary>
        /// Object is inactive - action can be performed
        /// (e.g. switched off button)
        /// </summary>
        Inactive, 
        
        /// <summary>
        /// Object is active - performing an action will return in to Inactive state
        /// (e.g. switched on button)
        /// </summary>
        Active
    }
}