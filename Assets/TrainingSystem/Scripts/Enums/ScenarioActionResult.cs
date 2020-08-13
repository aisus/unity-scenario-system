namespace TrainingSystem.Scripts.Enums
{
    /// <summary>
    /// Result of training scenario action
    /// </summary>
    public enum ScenarioActionResult
    {
        /// <summary>
        /// Action is invalid on current stage
        /// </summary>
        ActionNotAllowed,

        /// <summary>
        /// Action completed successfully, but another action needed to switch stage
        /// </summary>
        Ok,

        /// <summary>
        /// Action completed, scenario stage switched
        /// </summary>
        OkAndNextStage,

        /// <summary>
        /// Action completed, scenario finished
        /// </summary>
        ScenarioCompleted
    }
}