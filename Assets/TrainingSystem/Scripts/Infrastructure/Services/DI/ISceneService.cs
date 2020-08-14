namespace TrainingSystem.Scripts.Infrastructure.Services.DI
{
    /// <summary>
    /// Scene-specific service class, only available in a scene context
    /// </summary>
    public interface ISceneService : IService
    {
        /// <summary>
        /// Final operations before scene exit
        /// </summary>
        void OnSceneExit();
    }
}