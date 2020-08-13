using System;
using System.Collections.Generic;
using TrainingSystem.Scripts.Enums;
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
        public Action<InteractiveObjectEntity> OnActionPerformed   { get; set; }
        public Action<InteractiveObjectEntity> OnActionFailed      { get; set; }
        public Action<InteractiveObjectEntity> OnActionSucceed     { get; set; }
        public Action                          OnScenarioCompleted { get; set; }

        /// <inheritdoc />
        public string CurrentScenarioStageName => _scenarioService.CurrentStageName;

        /// <inheritdoc />
        public IEnumerable<InteractiveBehaviour> InteractiveBehaviours => _interactiveBehaviours;

        private readonly List<InteractiveBehaviour> _interactiveBehaviours;
        private readonly IScenarioService           _scenarioService;

        public InteractionService()
        {
            _interactiveBehaviours = new List<InteractiveBehaviour>();
            _scenarioService       = new ScenarioService(this);
        }

        /// <inheritdoc />
        public void AddInteractiveEntity(InteractiveBehaviour behaviour) => _interactiveBehaviours.Add(behaviour);

        /// <summary>
        /// Executed when action performed on interactive object
        /// </summary>
        /// <param name="behaviour"></param>
        public bool TryPerformAction(InteractiveBehaviour behaviour)
        {
            var entity = behaviour.Entity;
            if (!entity.InteractionEnabled) return false;

            switch (entity.State)
            {
                case InteractiveObjectState.Inactive:
                    // UseOnce type disables itself when activated
                    if (entity.Type == InteractiveObjectType.UseOnce)
                    {
                        entity.State              = InteractiveObjectState.Active;
                        entity.InteractionEnabled = false;
                    }
                    // Trigger type changes to active state when activated
                    // Switch type changes to active state when activated
                    else
                    {
                        entity.State = InteractiveObjectState.Active;
                    }

                    break;
                case InteractiveObjectState.Active:
                    // Switch type changes to inactive state when deactivated
                    if (entity.Type == InteractiveObjectType.Switch) entity.State = InteractiveObjectState.Inactive;
                    // Trigger type can't be deactivated
                    // UseOnce type can't be deactivated
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Logger.Log($"Action performed {behaviour.Entity.Key} -> {behaviour.Entity.State}", LogType.Log);

            OnActionPerformed?.Invoke(behaviour.Entity);
            var result = _scenarioService.TryExecuteScenarioAction(behaviour.Entity);

            Logger.Log($"Result: {result}", LogType.Log);

            switch (result)
            {
                case ScenarioActionResult.ActionNotAllowed:
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

            return result != ScenarioActionResult.ActionNotAllowed;
        }

        /// <inheritdoc />
        public void OnSceneExit()
        {
        }
    }
}