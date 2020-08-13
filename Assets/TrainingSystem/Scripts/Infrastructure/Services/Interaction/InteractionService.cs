using System;
using System.Collections.Generic;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;
using UnityEngine;
using Logger = TrainingSystem.Scripts.Infrastructure.Utility.Logger;

namespace TrainingSystem.Scripts.Infrastructure.Services.Interaction
{
    /// <inheritdoc />
    public class InteractionService : IInteractionService
    {
        public string ActiveStageName => _scenarioService.ActiveStageName;
        public Action<InteractiveObjectEntity> OnActionPerformed { get; set; }
        public Action<InteractiveObjectEntity> OnActionFailed { get; set; }
        public Action<InteractiveObjectEntity> OnActionSucceed { get; set; }
        public Action OnScenarioCompleted { get; set; }

        public List<InteractiveBehaviour> InteractiveBehaviours => _interactiveBehaviours;

        private readonly List<InteractiveBehaviour> _interactiveBehaviours;
        private readonly IScenarioService _scenarioService;

        public InteractionService()
        {
            _interactiveBehaviours = new List<InteractiveBehaviour>();
            _scenarioService = new ScenarioService(this);
        }

        /// <inheritdoc />
        public void AddInteractiveEntity(InteractiveBehaviour behaviour)
        {
            _interactiveBehaviours.Add(behaviour);
            behaviour.OnActionPerformed += ActionPerformedHandler;
        }

        /// <summary>
        /// Executed when action performed on interactive object
        /// </summary>
        /// <param name="behaviour"></param>
        private void ActionPerformedHandler(InteractiveBehaviour behaviour)
        {
            Logger.Log($"ACTION {behaviour.Entity.Key}", LogType.Log);

            if (behaviour.Entity.InteractionEnabled)
                behaviour.UpdateState();    
            
            OnActionPerformed?.Invoke(behaviour.Entity);
            var result = _scenarioService.TryExecuteScenarioAction(behaviour.Entity);

            Logger.Log($"Result: {result}", LogType.Log);

            switch (result)
            {
                case ScenarioActionResult.ActionNotAllowed:
                case ScenarioActionResult.ConditionsNotMatch:
                    OnActionFailed?.Invoke(behaviour.Entity);
                    break;
                case ScenarioActionResult.Ok:
                case ScenarioActionResult.OkAndNextStage:
                    OnActionSucceed?.Invoke(behaviour.Entity);
                    break;
                case ScenarioActionResult.ScenarioCompleted:
                    Logger.Log("Scenario completed!", LogType.Log);
                    OnActionSucceed?.Invoke(behaviour.Entity);
                    OnScenarioCompleted?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        public void OnSceneExit()
        {
        }
    }
}