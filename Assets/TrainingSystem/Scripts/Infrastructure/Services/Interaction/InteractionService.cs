using System.Collections.Generic;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Interaction;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Interaction
{
    /// <inheritdoc />
    public class InteractionService : IInteractionService
    {
        private List<InteractiveBehaviour> _interactiveEntities;
        private IScenarioService _scenarioService;

        public InteractionService()
        {
            _interactiveEntities = new List<InteractiveBehaviour>();
            _scenarioService = ServiceLocator.Current.ResolveDependency<IScenarioService>();
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
            if (!_scenarioService.IsScenarioCompleted())
            {
                var result = _scenarioService.TryExecuteScenarioAction(behaviour.Entity);
                Debug.Log($"Result: {result}");
            }
            else
            {
                Debug.Log($"Scenario completed!");
            }
        }

        /// <inheritdoc />
        public void OnSceneExit()
        {
        }
    }
}