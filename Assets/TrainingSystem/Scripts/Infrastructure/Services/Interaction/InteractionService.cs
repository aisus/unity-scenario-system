using System;
using System.Collections.Generic;
using TrainingSystem.Scripts.Enums;
using TrainingSystem.Scripts.Infrastructure.Services.DI;
using TrainingSystem.Scripts.Infrastructure.Services.Scenarios;
using TrainingSystem.Scripts.Model;
using TrainingSystem.Scripts.SceneInteraction;
using UnityEngine;
using ILogger = TrainingSystem.Scripts.Infrastructure.Services.Utility.Logging.ILogger;

namespace TrainingSystem.Scripts.Infrastructure.Services.Interaction
{
    /// <inheritdoc />
    public class InteractionService : IInteractionService
    {
        public Action<InteractiveObjectEntity> OnActionPerformed { get; set; }
        public Action<InteractiveObjectEntity> OnActionFailed { get; set; }
        public Action<InteractiveObjectEntity> OnActionSucceed { get; set; }
        public Action OnScenarioCompleted { get; set; }

        private readonly List<InteractiveBehaviour> _interactiveObjects;
        private readonly IScenarioService _scenarioService;
        private readonly ILogger _logger;

        public InteractionService()
        {
            _interactiveObjects = new List<InteractiveBehaviour>();
            _scenarioService = ServiceLocator.Current.ResolveDependency<IScenarioService>();
            _logger = ServiceLocator.Current.ResolveDependency<ILogger>();
        }

        /// <inheritdoc />
        public void AddInteractiveEntity(InteractiveBehaviour behaviour)
        {
            _interactiveObjects.Add(behaviour);
            behaviour.OnActionPerformed += ActionPerformedHandler;
        }

        /// <summary>
        /// Executed when action performed on interactive object
        /// </summary>
        /// <param name="behaviour"></param>
        private void ActionPerformedHandler(InteractiveBehaviour behaviour)
        {
            _logger.Log($"ACTION {behaviour.Entity.Key}", LogType.Log);

            if (behaviour.Entity.State != InteractiveObjectState.Disabled)
                behaviour.UpdateState();

            if (_scenarioService.IsScenarioCompleted()) return;

            OnActionPerformed?.Invoke(behaviour.Entity);
            var result = _scenarioService.TryExecuteScenarioAction(behaviour.Entity);

            _logger.Log($"Result: {result}", LogType.Log);

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
                    _logger.Log("Scenario completed!", LogType.Log);
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