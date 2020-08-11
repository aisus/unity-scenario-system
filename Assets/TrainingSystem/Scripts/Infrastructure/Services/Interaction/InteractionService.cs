using System.Collections.Generic;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Interaction;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Interaction
{
    /// <inheritdoc />
    public class InteractionService : IInteractionService
    {
        private List<InteractiveBehaviour> _interactiveEntities;

        public InteractionService()
        {
            _interactiveEntities = new List<InteractiveBehaviour>();
        }

        /// <inheritdoc />
        public void OnSceneExit()
        {
            _interactiveEntities = new List<InteractiveBehaviour>();
        }

        /// <inheritdoc />
        public void AddInteractiveEntity(InteractiveBehaviour behaviour)
        {
            _interactiveEntities.Add(behaviour);
            behaviour.OnActionPerformed += ActionPerformedHandler;
        }

        /// <summary>
        /// Executed when action performed on interactive object
        /// </summary>
        /// <param name="behaviour"></param>
        private void ActionPerformedHandler(InteractiveBehaviour behaviour)
        {
            Debug.Log($"ACTION! {behaviour.Entity.Key}");
            if (behaviour.Entity.State != InteractiveObjectState.Disabled)
                behaviour.UpdateState();
        }
    }
}