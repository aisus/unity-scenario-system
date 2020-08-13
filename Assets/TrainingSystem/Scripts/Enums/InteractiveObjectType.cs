namespace TrainingSystem.Scripts.Enums
{
    /// <summary>
    /// Type of an interactive object
    /// </summary>
    public enum InteractiveObjectType
    {
        /// <summary>
        /// Saves object state (e.g. switch on-off)
        /// </summary>
        Switch,

        /// <summary>
        /// Activate and keep active always (e.g. alarm button)
        /// </summary>
        Trigger,

        /// <summary>
        /// Activate once and disable (e.g. cut the wire)
        /// </summary>
        UseOnce
    }
}