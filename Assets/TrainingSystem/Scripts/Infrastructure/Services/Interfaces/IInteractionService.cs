using TrainingSystem.Scripts.Infrastructure.Services.ServiceLocator;
using TrainingSystem.Scripts.Interaction;

namespace TrainingSystem.Scripts.Infrastructure.Services.Interfaces
{
    /// <summary>
    /// Service to keep track of interactive objects and provide interaction events
    /// </summary>
    public interface IInteractionService : ISceneService
    {
        /// <summary>
        /// Add new interactive object
        /// </summary>
        /// <param name="behaviour"></param>
        void AddInteractiveEntity(InteractiveBehaviour behaviour);
    }
}