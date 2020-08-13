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
        Action<InteractiveObjectEntity> OnActionPerformed   { get; set; }
        Action<InteractiveObjectEntity> OnActionSucceed     { get; set; }
        Action<InteractiveObjectEntity> OnActionFailed      { get; set; }
        Action                          OnScenarioCompleted { get; set; }

        string CurrentScenarioStageName { get; }

        /// <summary>
        /// All interactive objects in scene
        /// </summary>
        IEnumerable<InteractiveBehaviour> InteractiveBehaviours { get; }

        /// <summary>
        /// Add new interactive object to tracked by service
        /// </summary>
        /// <param name="behaviour"></param>
        void AddInteractiveEntity(InteractiveBehaviour behaviour);

        /// <summary>
        /// Check if action can be performed
        /// </summary>
        /// <param name="behaviour"></param>
        bool TryPerformAction(InteractiveBehaviour behaviour);
    }
}