using System;
using System.Collections.Generic;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;

namespace TrainingSystem.Scripts.Infrastructure.Services.Interaction
{
    /// <summary>
    /// Service to keep track of interactive objects and provide interaction events
    /// </summary>
    public interface IInteractionService : ISceneService
    {
        Action<InteractiveObjectEntity> OnActionPerformed { get; set; }
        Action<InteractiveObjectEntity> OnActionSucceed { get; set; }
        Action<InteractiveObjectEntity> OnActionFailed { get; set; }
        Action OnScenarioCompleted { get; set; }

        List<InteractiveBehaviour> InteractiveBehaviours { get; }

        /// <summary>
        /// Add new interactive object
        /// </summary>
        /// <param name="behaviour"></param>
        void AddInteractiveEntity(InteractiveBehaviour behaviour);
    }
}